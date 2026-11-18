using System.Diagnostics;
using Fxfer.DomainApi.Contracts.Utilities;

namespace Fxfer.DomainApi.Api.Middleware;

public sealed class CorrelationIdMiddleware
{
    public const string HeaderName = Fxfer.DomainApi.Domain.Constants.CommonConstants.CorrelationIdHeaderName;

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

        // T-005: Correlation id must flow through traces/logs/metrics and outbound calls.
        RequestContext.SetCorrelationId(context, correlationId);

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        return _next(context);
    }
}
