using System.Diagnostics;

namespace Fxfer.ProjectionWorker.Worker.Middleware;

public sealed class CorrelationIdMiddleware
{
    public const string HeaderName = "X-Correlation-ID";

    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var values)
            ? values.FirstOrDefault()
            : null;

        if (!Guid.TryParse(correlationId, out _))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        Activity.Current?.SetTag("gainwell.correlation_id", correlationId);

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        return _next(context);
    }
}

