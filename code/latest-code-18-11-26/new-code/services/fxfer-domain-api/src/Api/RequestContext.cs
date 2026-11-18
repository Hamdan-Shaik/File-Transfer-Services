using System.Diagnostics;
using Fxfer.DomainApi.Contracts.Utilities;

namespace Fxfer.DomainApi.Api;

internal static class RequestContext
{
    // T-005: Provide tenant/correlation context for OpenTelemetry tags and propagation.

    public static string GetOrCreateCorrelationId(HttpContext httpContext)
    {
        if (httpContext.Items.TryGetValue(ContextKeys.CorrelationIdItemKey, out var existing) && existing is string value)
        {
            return value;
        }

        var correlationId = Guid.NewGuid().ToString();
        SetCorrelationId(httpContext, correlationId);
        return correlationId;
    }

    public static void SetCorrelationId(HttpContext httpContext, string correlationId)
    {
        httpContext.Items[ContextKeys.CorrelationIdItemKey] = correlationId;

        // Required tags (target-state): correlation_id + trace_id must be available end-to-end.
        // Semconv/custom (STD-APP-MON-002): keep Gainwell-prefixed attributes for redaction allowlists.
        Activity.Current?.SetTag("correlation_id", correlationId);
        Activity.Current?.SetTag("gainwell.correlation.id", correlationId);
        Activity.Current?.SetTag("gainwell.correlation_id", correlationId);

        Activity.Current?.AddBaggage("correlation_id", correlationId);
    }

    public static string? GetCorrelationId(HttpContext httpContext)
    {
        return httpContext.Items.TryGetValue(ContextKeys.CorrelationIdItemKey, out var existing) ? existing as string : null;
    }

    public static void SetTenantId(HttpContext httpContext, string tenantId)
    {
        httpContext.Items[ContextKeys.TenantIdItemKey] = tenantId;
        Activity.Current?.SetTag("gainwell.tenant.id", tenantId);

        Activity.Current?.SetTag("tenant_id", tenantId);
        Activity.Current?.AddBaggage("tenant_id", tenantId);
    }

    public static string? GetTenantId(HttpContext httpContext)
    {
        return httpContext.Items.TryGetValue(ContextKeys.TenantIdItemKey, out var existing) ? existing as string : null;
    }
}
