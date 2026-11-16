namespace FxferService.Infrastructure.Messaging.EventAdapter;

public interface IEventAdapter
{
    Task PublishAsync(string tenantId, string eventName, string eventVersion, string payloadJson, CancellationToken cancellationToken);
}

public sealed class NoopEventAdapter : IEventAdapter
{
    private readonly ILogger<NoopEventAdapter> _logger;

    public NoopEventAdapter(ILogger<NoopEventAdapter> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync(string tenantId, string eventName, string eventVersion, string payloadJson, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Noop publish: {eventName} v{eventVersion} (tenant_id={tenantId})", eventName, eventVersion, tenantId);
        return Task.CompletedTask;
    }
}
