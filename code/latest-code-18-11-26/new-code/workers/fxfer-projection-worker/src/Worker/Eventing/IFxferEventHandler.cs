namespace Fxfer.ProjectionWorker.Worker.Eventing;

public interface IFxferEventHandler
{
    bool CanHandle(string eventName, string eventVersion);

    Task HandleAsync(FxferEventEnvelope envelope, CancellationToken cancellationToken);
}

