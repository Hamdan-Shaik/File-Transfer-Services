using System.Text.Json;
using Fxfer.Bff.Api.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using BffCorsOptions = Fxfer.Bff.Api.Configuration.CorsOptions;

namespace Fxfer.Bff.Tests;

public sealed class T_002_StatelessJwtAndCorsTests
{
    [Fact]
    public async Task T_002_registers_jwt_bearer_authentication_and_no_cookie_schemes()
    {
        using var serviceProvider = BuildServiceProvider(new Dictionary<string, string?>
        {
            ["Authentication:Authority"] = "https://auth.example.test",
            ["Authentication:Audience"] = "fxfer-bff",
        });

        var schemeProvider = serviceProvider.GetRequiredService<IAuthenticationSchemeProvider>();
        var schemes = await schemeProvider.GetAllSchemesAsync();

        Assert.Contains(schemes, s => s.Name == JwtBearerDefaults.AuthenticationScheme);
        Assert.DoesNotContain(schemes, s => string.Equals(s.Name, "Cookies", StringComparison.OrdinalIgnoreCase));

        var authOptions = serviceProvider.GetRequiredService<IOptions<Microsoft.AspNetCore.Authentication.AuthenticationOptions>>().Value;
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, authOptions.DefaultScheme);
    }

    [Fact]
    public async Task T_002_configures_strict_cors_allow_list()
    {
        using var serviceProvider = BuildServiceProvider(new Dictionary<string, string?>
        {
            ["Authentication:Authority"] = "https://auth.example.test",
            ["Authentication:Audience"] = "fxfer-bff",
            ["Cors:AllowedOriginsCsv"] = "https://shell.example.test,https://mfe.example.test",
        });

        var corsPolicyProvider = serviceProvider.GetRequiredService<ICorsPolicyProvider>();

        var httpContext = new DefaultHttpContext();
        var policy = await corsPolicyProvider.GetPolicyAsync(httpContext, "BffCors");

        Assert.NotNull(policy);
        Assert.False(policy!.AllowAnyOrigin);
        Assert.False(policy.SupportsCredentials);
        Assert.Equal(2, policy.Origins.Count);
        Assert.Contains("https://shell.example.test", policy.Origins);
        Assert.Contains("https://mfe.example.test", policy.Origins);
    }

    [Fact]
    public async Task T_002_rejects_state_changing_requests_with_missing_origin_or_referer()
    {
        var middleware = new OriginValidationMiddleware(
            _ => Task.CompletedTask,
            Options.Create(new BffCorsOptions { AllowedOriginsCsv = "https://example.test" }));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.Request.Method = HttpMethods.Post;

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        AssertProblemTitle(context, "Missing Origin/Referer");
    }

    [Fact]
    public async Task T_002_rejects_state_changing_requests_with_disallowed_origin()
    {
        var middleware = new OriginValidationMiddleware(
            _ => Task.CompletedTask,
            Options.Create(new BffCorsOptions { AllowedOriginsCsv = "https://allowed.test" }));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers.Origin = "https://example.test";

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        AssertProblemTitle(context, "Origin is not allowed");
    }

    [Fact]
    public async Task T_002_allows_state_changing_requests_with_allowed_referer()
    {
        var invokedNext = false;

        var middleware = new OriginValidationMiddleware(
            _ =>
            {
                invokedNext = true;
                return Task.CompletedTask;
            },
            Options.Create(new BffCorsOptions { AllowedOriginsCsv = "https://example.test" }));

        var context = new DefaultHttpContext();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers.Referer = "https://example.test/some/page";

        await middleware.InvokeAsync(context);

        Assert.True(invokedNext);
    }

    [Fact]
    public async Task T_002_rejects_state_changing_requests_when_allowed_origins_are_not_configured()
    {
        var middleware = new OriginValidationMiddleware(
            _ => Task.CompletedTask,
            Options.Create(new BffCorsOptions { AllowedOriginsCsv = "" }));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers.Origin = "https://example.test";

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        AssertProblemTitle(context, "Origin validation is not configured");
    }

    private static ServiceProvider BuildServiceProvider(IReadOnlyDictionary<string, string?> configurationValues)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationValues)
            .Build();

        var services = new ServiceCollection();
        Program.ConfigureServices(services, configuration);

        return services.BuildServiceProvider();
    }

    private static void AssertProblemTitle(DefaultHttpContext context, string expectedTitle)
    {
        context.Response.Body.Position = 0;

        using var json = JsonDocument.Parse(context.Response.Body);
        Assert.True(json.RootElement.TryGetProperty("title", out var title));
        Assert.Equal(expectedTitle, title.GetString());
    }
}

