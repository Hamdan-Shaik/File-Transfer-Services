using System.Diagnostics;
using OpenTelemetry;

namespace FxferService.Host.Composition;

internal sealed class TelemetryTaggingProcessor : BaseProcessor<Activity>
{
    public override void OnStart(Activity activity)
    {
        // T-006: Ensure all spans are taggable with tenant_id, correlation_id, and trace_id.
        var tenantId = activity.GetBaggageItem("tenant_id");
        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            activity.SetTag("tenant_id", tenantId);
        }

        var correlationId = activity.GetBaggageItem("correlation_id");
        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            activity.SetTag("correlation_id", correlationId);
        }

        activity.SetTag("trace_id", activity.TraceId.ToString());
    }
}
