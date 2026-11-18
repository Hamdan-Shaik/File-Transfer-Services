using System.Diagnostics;
using Fxfer.ProjectionWorker.Worker.Processing;

namespace Fxfer.ProjectionWorker.Worker.Eventing;

public sealed class FxferEventDispatcher
{
    private static readonly ActivitySource ActivitySource = new("fxfer-projection-worker");
    private readonly IEnumerable<IFxferEventHandler> _handlers;
    private readonly ProcessingContextAccessor _contextAccessor;
    private readonly ILogger<FxferEventDispatcher> _logger;

    public FxferEventDispatcher(
        IEnumerable<IFxferEventHandler> handlers,
        ProcessingContextAccessor contextAccessor,
        ILogger<FxferEventDispatcher> logger)
    {
        _handlers = handlers;
        _contextAccessor = contextAccessor;
        _logger = logger;
    }

    public async Task DispatchAsync(FxferEventEnvelope envelope, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(envelope.TenantId))
        {
            throw new InvalidOperationException("tenant_id is required");
        }

        var correlationId = Guid.TryParse(envelope.CorrelationId, out _) ? envelope.CorrelationId : Guid.NewGuid().ToString();
        var traceId = envelope.TraceId;

        _contextAccessor.Current = new ProcessingContext
        {
            TenantId = envelope.TenantId,
            CorrelationId = correlationId,
            TraceId = traceId,
            EventId = envelope.EventId
        };

        var scopeState = new Dictionary<string, object?>
        {
            ["tenant_id"] = envelope.TenantId,
            ["correlation_id"] = correlationId,
            ["trace_id"] = traceId,
            ["event_name"] = envelope.EventName,
            ["event_version"] = envelope.EventVersion,
            ["event_id"] = envelope.EventId
        };

        using var scope = _logger.BeginScope(scopeState);
        using var activity = StartConsumeActivity(envelope, correlationId);

        var handler = _handlers.FirstOrDefault(h => h.CanHandle(envelope.EventName, envelope.EventVersion));
        if (handler is null)
        {
            throw new InvalidOperationException($"No handler registered for {envelope.EventName} {envelope.EventVersion}");
        }

        _logger.LogInformation(
            "Dispatching {EventName} {EventVersion} for tenant {TenantId}",
            envelope.EventName,
            envelope.EventVersion,
            envelope.TenantId);

        await handler.HandleAsync(envelope, cancellationToken);
    }

    private static Activity? StartConsumeActivity(FxferEventEnvelope envelope, string correlationId)
    {
        ActivityContext parentContext = default;

        if (TryParseTraceId(envelope.TraceId, out var traceId))
        {
            parentContext = new ActivityContext(traceId, ActivitySpanId.CreateRandom(), ActivityTraceFlags.Recorded);
        }

        var activity = ActivitySource.StartActivity("fxfer.event.consume", ActivityKind.Consumer, parentContext);

        activity?.SetTag("gainwell.tenant_id", envelope.TenantId);
        activity?.SetTag("gainwell.correlation_id", correlationId);
        activity?.SetTag("fxfer.event_name", envelope.EventName);
        activity?.SetTag("fxfer.event_version", envelope.EventVersion);
        activity?.SetTag("fxfer.event_id", envelope.EventId.ToString());
        activity?.SetTag("fxfer.producer", envelope.Producer);

        return activity;
    }

    private static bool TryParseTraceId(string? traceIdText, out ActivityTraceId traceId)
    {
        traceId = default;

        if (string.IsNullOrWhiteSpace(traceIdText) || traceIdText.Length != 32)
        {
            return false;
        }

        try
        {
            traceId = ActivityTraceId.CreateFromString(traceIdText.AsSpan());
            return traceId != default;
        }
        catch
        {
            traceId = default;
            return false;
        }
    }
}

