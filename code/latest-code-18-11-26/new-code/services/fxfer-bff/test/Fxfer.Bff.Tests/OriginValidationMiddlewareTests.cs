using System.Text.Json;
using Fxfer.Bff.Api.Configuration;
using Fxfer.Bff.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fxfer.Bff.Tests;

public sealed class OriginValidationMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_AllowsRefererWithPath_WhenOriginIsAllowed()
    {
        var invokedNext = false;

        var middleware = new OriginValidationMiddleware(
            _ =>
            {
                invokedNext = true;
                return Task.CompletedTask;
            },
            Options.Create(new CorsOptions { AllowedOriginsCsv = "https://example.test" }));

        var context = new DefaultHttpContext();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers.Referer = "https://example.test/some/page";

        await middleware.InvokeAsync(context);

        Assert.True(invokedNext);
    }

    [Fact]
    public async Task InvokeAsync_RejectsWhenOriginIsMissing_OnStateChangingRequest()
    {
        var middleware = new OriginValidationMiddleware(
            _ => Task.CompletedTask,
            Options.Create(new CorsOptions { AllowedOriginsCsv = "https://example.test" }));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.Request.Method = HttpMethods.Post;

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        AssertProblemTitle(context, "Missing Origin/Referer");
    }

    [Fact]
    public async Task InvokeAsync_RejectsWhenOriginIsNotAllowed_OnStateChangingRequest()
    {
        var middleware = new OriginValidationMiddleware(
            _ => Task.CompletedTask,
            Options.Create(new CorsOptions { AllowedOriginsCsv = "https://allowed.test" }));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Path = "/api/file-transfer/v1/ftp-servers";
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers.Origin = "https://example.test";

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        AssertProblemTitle(context, "Origin is not allowed");
    }

    private static void AssertProblemTitle(DefaultHttpContext context, string expectedTitle)
    {
        context.Response.Body.Position = 0;

        using var json = JsonDocument.Parse(context.Response.Body);
        Assert.True(json.RootElement.TryGetProperty("title", out var title));
        Assert.Equal(expectedTitle, title.GetString());
    }
}

