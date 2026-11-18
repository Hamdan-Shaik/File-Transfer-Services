namespace Fxfer.DomainApi.Infrastructure.Eventing.Outbox;

// T-006: Background publisher for the transactional outbox.
public sealed class OutboxPublisherHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxPublisherHostedService> _logger;
    private readonly TimeSpan _pollInterval;

    public OutboxPublisherHostedService(IServiceScopeFactory scopeFactory, IConfiguration configuration, ILogger<OutboxPublisherHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;

        var seconds = configuration.GetValue("Eventing:OutboxPollSeconds", 5);
        _pollInterval = TimeSpan.FromSeconds(Math.Max(1, seconds));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var publisher = scope.ServiceProvider.GetRequiredService<OutboxPublisher>();
                await publisher.PublishPendingAsync(batchSize: 50, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox publisher loop failed.");
            }

            await Task.Delay(_pollInterval, stoppingToken);
        }
    }
}

