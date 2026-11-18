using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Fxfer.DomainApi.Contracts.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fxfer.DomainApi.Tests;

public sealed class T003TenantAuthorizationTests
{
    [Fact]
    public async Task T_003_missing_tenant_context_is_forbidden()
    {
        await using var factory = new FxferDomainApiFactoryWithoutTenant();
        var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/api/fxfer-domain/v1/ftp-servers", CreateRequest());
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Missing tenant context", problem!.Title);
    }

    [Fact]
    public async Task T_003_unknown_tenant_context_is_forbidden()
    {
        await using var factory = new FxferDomainApiFactoryWithTenant("tenant-unknown");
        var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/api/fxfer-domain/v1/ftp-servers", CreateRequest());
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Unknown tenant context", problem!.Title);
    }

    private static FtpServerUpsertRequest CreateRequest()
    {
        return new FtpServerUpsertRequest
        {
            FtpServerName = "server",
            FtpServerIp = "127.0.0.1",
            FtpTransferMode = "1",
            FtpUser = "user",
            CredentialSecretRef = "secret-ref",
            FtpDescription = "desc",
            FtpPort = 21,
            UseSshAuth = false,
            IsValid = true,
            OperatorId = "operator"
        };
    }

    private sealed class FxferDomainApiFactoryWithoutTenant : WebApplicationFactory<global::Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = NoTenantAuthHandler.SchemeName;
                        options.DefaultChallengeScheme = NoTenantAuthHandler.SchemeName;
                    })
                    .AddScheme<AuthenticationSchemeOptions, NoTenantAuthHandler>(NoTenantAuthHandler.SchemeName, _ => { });
            });
        }
    }

    private sealed class FxferDomainApiFactoryWithTenant(string tenantId) : WebApplicationFactory<global::Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(new TenantAuthHandlerOptions(tenantId));

                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = TenantAuthHandler.SchemeName;
                        options.DefaultChallengeScheme = TenantAuthHandler.SchemeName;
                    })
                    .AddScheme<AuthenticationSchemeOptions, TenantAuthHandler>(TenantAuthHandler.SchemeName, _ => { });
            });
        }
    }

    private sealed class NoTenantAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        Microsoft.Extensions.Logging.ILoggerFactory logger,
        UrlEncoder encoder)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        public const string SchemeName = "NoTenant";

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] { new Claim("sub", "test-user") };
            var identity = new ClaimsIdentity(claims, SchemeName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    private sealed record TenantAuthHandlerOptions(string TenantId);

    private sealed class TenantAuthHandler(
        TenantAuthHandlerOptions options,
        IOptionsMonitor<AuthenticationSchemeOptions> authOptions,
        Microsoft.Extensions.Logging.ILoggerFactory logger,
        UrlEncoder encoder)
        : AuthenticationHandler<AuthenticationSchemeOptions>(authOptions, logger, encoder)
    {
        public const string SchemeName = "Tenant";

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim("sub", "test-user"),
                new Claim("tenant_id", options.TenantId),
            };

            var identity = new ClaimsIdentity(claims, SchemeName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}

