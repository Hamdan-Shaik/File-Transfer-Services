using Fxfer.Bff.Host.Composition;
using Fxfer.Bff.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Mvc;

namespace Fxfer.Bff.Host.Middleware;

public sealed class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationMiddleware> _logger;

    public CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, TenantContext tenantContext)
    {
        var correlationId = context.Request.Headers[RequestContextHeaderNames.CorrelationId].ToString();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("n");
            context.Request.Headers[RequestContextHeaderNames.CorrelationId] = correlationId;
        }

        context.Response.Headers[RequestContextHeaderNames.CorrelationId] = correlationId;
        tenantContext.CorrelationId = correlationId;

        if (context.User.Identity?.IsAuthenticated == true)
        {
            tenantContext.TenantId = context.User.FindFirst("tenant_id")?.Value;
            if (string.IsNullOrWhiteSpace(tenantContext.TenantId))
            {
                var traceId = System.Diagnostics.Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

                // T-002: Operator JWT must carry tenant context.
                var problem = new ProblemDetails
                {
                    Title = "Unauthorized",
                    Status = StatusCodes.Status401Unauthorized,
                    Type = "https://httpstatuses.com/401",
                };

                problem.Extensions["error"] = new ErrorEnvelope(
                    code: "TENANT_MISSING",
                    message: "Missing required tenant context",
                    details: Array.Empty<ErrorDetail>(),
                    timestamp: DateTimeOffset.UtcNow,
                    trace_id: traceId);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json");
                return;
            }
        }

        var traceIdValue = System.Diagnostics.Activity.Current?.TraceId.ToString();
        var spanIdValue = System.Diagnostics.Activity.Current?.SpanId.ToString();

        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["correlation_id"] = tenantContext.CorrelationId,
            ["tenant_id"] = tenantContext.TenantId,
            ["trace_id"] = traceIdValue,
            ["span_id"] = spanIdValue,
        });

        if (System.Diagnostics.Activity.Current is { } activity)
        {
            if (!string.IsNullOrWhiteSpace(tenantContext.TenantId))
            {
                activity.SetTag("tenant_id", tenantContext.TenantId);
            }

            activity.SetTag("correlation_id", tenantContext.CorrelationId);
        }

        await _next(context);
    }
}
