using Fxfer.ProjectionWorker.Worker.Eventing;

namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed class SuspensionStateChangedProjectionHandler : IFxferEventHandler
{
    private readonly ILogger<SuspensionStateChangedProjectionHandler> _logger;

    public SuspensionStateChangedProjectionHandler(ILogger<SuspensionStateChangedProjectionHandler> logger)
    {
        _logger = logger;
    }

    public bool CanHandle(string eventName, string eventVersion)
    {
        return eventName == ConsumedEvents.SuspensionStateChangedV1Name && eventVersion == ConsumedEvents.V1;
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

