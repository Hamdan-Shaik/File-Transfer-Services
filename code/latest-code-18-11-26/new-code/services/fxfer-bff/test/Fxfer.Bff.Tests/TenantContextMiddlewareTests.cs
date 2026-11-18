using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using Fxfer.Bff.Api;
using Fxfer.Bff.Api.Configuration;
using Fxfer.Bff.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fxfer.Bff.Tests;

public sealed class TenantContextMiddlewareTests
{
    [Fact]
    public async Task T_001_does_not_require_tenant_when_outside_file_transfer_api()
    {
        var invokedNext = false;

        var middleware = new TenantContextMiddleware(
            _ =>
            {
                invokedNext = true;
                return Task.CompletedTask;
            },
            Options.Create(new TenantResolutionOptions()));

        var context = new DefaultHttpContext();
        context.Request.Path = "/health";

        await middleware.InvokeAsync(context);

        Assert.True(invokedNext);
    }

    [Fact]
    public async Task T_001_rejects_when_tenant_claim_missing_on_file_transfer_api_request()
    {
        var middleware = new TenantContextMiddleware(
            _ => Task.CompletedTask,
            Options.Create(new TenantResolutionOptions()));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.User = new ClaimsPrincipal(new ClaimsIdentity());

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        AssertProblemTitle(context, "Unauthorized");
    }

    [Fact]
    public async Task T_001_rejects_when_tenant_claim_missing_even_if_tenant_header_is_present()
    {
        var middleware = new TenantContextMiddleware(
            _ => Task.CompletedTask,
            Options.Create(new TenantResolutionOptions()));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.Request.Headers["X-Tenant-Id"] = "tenant-from-header";
        context.User = new ClaimsPrincipal(new ClaimsIdentity());

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        AssertProblemTitle(context, "Unauthorized");
    }

    [Fact]
    public async Task T_001_sets_tenant_id_from_jwt_claim_and_not_from_header()
    {
        string? observedTenantId = null;

        var middleware = new TenantContextMiddleware(
            ctx =>
            {
                observedTenantId = RequestContext.GetTenantId(ctx);
                return Task.CompletedTask;
            },
            Options.Create(new TenantResolutionOptions()));

        var context = new DefaultHttpContext();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.Request.Headers["X-Tenant-Id"] = "tenant-from-header";
        context.User = new ClaimsPrincipal(
            new ClaimsIdentity(new[] { new Claim("tenant_id", "tenant-a") }, authenticationType: "jwt"));

        await middleware.InvokeAsync(context);

        Assert.Equal("tenant-a", observedTenantId);
    }

    [Fact]
    public async Task T_001_tags_current_activity_with_tenant_id()
    {
        var invokedNext = false;

        var middleware = new TenantContextMiddleware(
            _ =>
            {
                invokedNext = true;
                return Task.CompletedTask;
            },
            Options.Create(new TenantResolutionOptions()));

        var context = new DefaultHttpContext();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.User = new ClaimsPrincipal(
            new ClaimsIdentity(new[] { new Claim("tenant_id", "tenant-a") }, authenticationType: "jwt"));

        using var activity = new Activity("t001");
        activity.Start();

        await middleware.InvokeAsync(context);

        Assert.True(invokedNext);
        Assert.Contains(activity.Tags, tag => tag.Key == "tenant_id" && (string?)tag.Value == "tenant-a");
        Assert.Contains(activity.Baggage, kvp => kvp.Key == "tenant_id" && kvp.Value == "tenant-a");
    }

    private static void AssertProblemTitle(DefaultHttpContext context, string expectedTitle)
    {
        context.Response.Body.Position = 0;

        using var json = JsonDocument.Parse(context.Response.Body);
        Assert.True(json.RootElement.TryGetProperty("title", out var title));
        Assert.Equal(expectedTitle, title.GetString());
    }
}
