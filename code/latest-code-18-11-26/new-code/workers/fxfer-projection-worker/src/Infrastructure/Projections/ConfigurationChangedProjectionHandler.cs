using Fxfer.ProjectionWorker.Worker.Eventing;

namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed class ConfigurationChangedProjectionHandler : IFxferEventHandler
{
    private readonly ILogger<ConfigurationChangedProjectionHandler> _logger;

    public ConfigurationChangedProjectionHandler(ILogger<ConfigurationChangedProjectionHandler> logger)
    {
        _logger = logger;
    }

    public bool CanHandle(string eventName, string eventVersion)
    {
        return eventName == ConsumedEvents.ConfigurationChangedV1Name && eventVersion == ConsumedEvents.V1;
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

