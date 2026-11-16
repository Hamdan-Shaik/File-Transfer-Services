using FxferService.Infrastructure.Tenancy;
using System.Diagnostics;

namespace FxferService.Host.Composition;

public sealed class RequestContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestContextMiddleware> _logger;

    public RequestContextMiddleware(RequestDelegate next, ILogger<RequestContextMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, RequestContext requestContext)
    {
        requestContext.CorrelationId = context.Request.Headers[RequestContextHeaderNames.CorrelationId].ToString();

        if (context.User.Identity?.IsAuthenticated == true)
        {
            requestContext.TenantId = context.User.FindFirst("tenant_id")?.Value;
            if (string.IsNullOrWhiteSpace(requestContext.TenantId))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = new
                    {
                        code = "TENANT_MISSING",
                        message = "Missing required tenant context",
                        details = Array.Empty<object>(),
                        timestamp = DateTimeOffset.UtcNow,
                        trace_id = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier,
                    },
                });
                return;
            }
        }

        var traceId = Activity.Current?.TraceId.ToString();
        var spanId = Activity.Current?.SpanId.ToString();

        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["correlation_id"] = requestContext.CorrelationId,
            ["tenant_id"] = requestContext.TenantId,
            ["trace_id"] = traceId,
            ["span_id"] = spanId,
        });

        if (Activity.Current is { } activity)
        {
            // T-006: FXFER backend components emit and propagate OpenTelemetry telemetry end-to-end with tenant_id, correlation_id, and trace_id tags.
            if (!string.IsNullOrWhiteSpace(requestContext.TenantId))
            {
                activity.SetTag("tenant_id", requestContext.TenantId);
                activity.AddBaggage("tenant_id", requestContext.TenantId);
            }

            activity.SetTag("correlation_id", requestContext.CorrelationId);
            activity.AddBaggage("correlation_id", requestContext.CorrelationId);

            activity.SetTag("trace_id", activity.TraceId.ToString());
        }

        await _next(context);
    }
}
