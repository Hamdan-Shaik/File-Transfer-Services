using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;

namespace Fxfer.DomainApi.Api.Middleware;

public sealed class RequestLoggingScopeMiddleware
{
    // T-005: Emit request metrics with tenant_id/correlation_id/trace_id tags.
    public const string MeterName = "Fxfer.DomainApi.Api";
    private static readonly Meter Meter = new(MeterName);
    private static readonly Counter<long> RequestCount = Meter.CreateCounter<long>("fxfer.domainapi.request.count");
    private static readonly Histogram<double> RequestDurationMs = Meter.CreateHistogram<double>("fxfer.domainapi.request.duration_ms");

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingScopeMiddleware> _logger;

    public RequestLoggingScopeMiddleware(RequestDelegate next, ILogger<RequestLoggingScopeMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTimestamp = Stopwatch.GetTimestamp();

        var correlationId = RequestContext.GetCorrelationId(context) ?? RequestContext.GetOrCreateCorrelationId(context);
        var tenantId = RequestContext.GetTenantId(context);
        var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

        // T-005: Ensure server spans carry the required tag set.
        if (Activity.Current is { } activity)
        {
            activity.SetTag("trace_id", traceId);
            activity.AddBaggage("trace_id", traceId);

            if (correlationId is not null)
            {
                activity.SetTag("correlation_id", correlationId);
                activity.AddBaggage("correlation_id", correlationId);
            }

            if (tenantId is not null)
            {
                activity.SetTag("tenant_id", tenantId);
                activity.AddBaggage("tenant_id", tenantId);
            }
        }

        using (_logger.BeginScope(new Dictionary<string, object?>
        {
            ["correlation_id"] = correlationId,
            ["tenant_id"] = tenantId,
            ["trace_id"] = traceId
        }))
        {
            try
            {
                await _next(context);
            }
            finally
            {
                var elapsed = Stopwatch.GetElapsedTime(startTimestamp);

                // Note: This tagging matches target-state T-005, even though
                // correlation_id/trace_id are high-cardinality metric dimensions.
                var tags = new TagList
                {
                    { "tenant_id", tenantId ?? string.Empty },
                    { "correlation_id", correlationId ?? string.Empty },
                    { "trace_id", traceId }
                };

                RequestCount.Add(1, tags);
                RequestDurationMs.Record(elapsed.TotalMilliseconds, tags);
            }
        }
    }
}
