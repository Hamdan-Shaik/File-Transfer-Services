using Fxfer.ProjectionWorker.Worker.Eventing;

namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed class DeliveryOutcomeRecordedProjectionHandler : IFxferEventHandler
{
    private readonly ILogger<DeliveryOutcomeRecordedProjectionHandler> _logger;

    public DeliveryOutcomeRecordedProjectionHandler(ILogger<DeliveryOutcomeRecordedProjectionHandler> logger)
    {
        _logger = logger;
    }

    public bool CanHandle(string eventName, string eventVersion)
    {
        return eventName == ConsumedEvents.DeliveryOutcomeRecordedV1Name && eventVersion == ConsumedEvents.V1;
    }

    public Task HandleAsync(FxferEventEnvelope envelope, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "skeleton: received {EventName} {EventVersion} (event_id={EventId})",
            envelope.EventName,
            envelope.EventVersion,
            envelope.EventId);

        return Task.CompletedTask;
    }
}

