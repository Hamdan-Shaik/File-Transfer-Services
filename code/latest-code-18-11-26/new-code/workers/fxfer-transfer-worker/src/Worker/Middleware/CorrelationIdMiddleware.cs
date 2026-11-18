using System.Diagnostics;
using Fxfer.TransferWorker.Worker.Services;

namespace Fxfer.TransferWorker.Worker.Middleware;

public sealed class CorrelationIdMiddleware
{
    public const string HeaderName = "X-Correlation-ID";

    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context, CorrelationIdContext correlationIdContext)
    {
        var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var values)
            ? values.FirstOrDefault()
            : null;

        if (!Guid.TryParse(correlationId, out _))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        correlationIdContext.Set(correlationId);
        Activity.Current?.SetTag("gainwell.correlation_id", correlationId);

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        return _next(context);
    }
}

