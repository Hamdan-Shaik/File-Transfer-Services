using System.Threading.Channels;
using FxferService.Projections.Infrastructure.Messaging.Idempotency;
using FxferService.Projections.Infrastructure.Tenancy;

namespace FxferService.Projections.Infrastructure.Messaging.Consumer;

public interface IFxferEventQueue
{
    ValueTask EnqueueAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default);
}

public sealed class FxferEventQueue : IFxferEventQueue
{
    private readonly Channel<MessageEnvelope> _channel = Channel.CreateUnbounded<MessageEnvelope>();

    public ChannelReader<MessageEnvelope> Reader => _channel.Reader;

    public ValueTask EnqueueAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default)
    {
        return _channel.Writer.WriteAsync(envelope, cancellationToken);
    }
}

public sealed class FxferEventConsumer : BackgroundService
{
    private readonly FxferEventQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<FxferEventConsumer> _logger;

    public FxferEventConsumer(FxferEventQueue queue, IServiceScopeFactory scopeFactory, ILogger<FxferEventConsumer> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Fxfer projections consumer started");

        await foreach (var envelope in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();

            var requestContext = scope.ServiceProvider.GetRequiredService<RequestContext>();
            requestContext.TenantId = envelope.TenantId;
            requestContext.CorrelationId = envelope.CorrelationId;

            using var _ = _logger.BeginScope(new Dictionary<string, object?>
            {
                ["tenant_id"] = envelope.TenantId,
                ["correlation_id"] = envelope.CorrelationId,
                ["event_id"] = envelope.EventId,
                ["event_name"] = envelope.EventName,
                ["event_version"] = envelope.EventVersion,
                ["trace_id"] = envelope.TraceId,
            });

            var inbox = scope.ServiceProvider.GetRequiredService<InboxStore>();
            var firstTime = await inbox.TryMarkProcessedAsync(envelope.EventId, stoppingToken);
            if (!firstTime)
            {
                _logger.LogInformation("Skipping already-processed event");
                continue;
            }

            // Skeleton: projection writers are added in later sessions.
            _logger.LogInformation("Accepted event for projection processing");
        }
    }
}
