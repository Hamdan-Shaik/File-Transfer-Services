using FxferService.Infrastructure.Messaging.EventAdapter;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Infrastructure.Messaging.Outbox;

public sealed class OutboxPublisherService : BackgroundService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(5);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxPublisherService> _logger;
    private readonly IConfiguration _configuration;

    public OutboxPublisherService(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxPublisherService> logger,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(PollInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await PublishPendingAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox publisher tick failed");
            }
        }
    }

    private async Task PublishPendingAsync(CancellationToken cancellationToken)
    {
        if (_configuration.GetValue("Tenancy:UseInMemoryPerTenant", false))
        {
            return;
        }

        var tenantIds = _configuration
            .GetSection("Tenancy:Tenants")
            .GetChildren()
            .Select(t => t.Key)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToArray();

        if (tenantIds.Length == 0)
        {
            return;
        }

        foreach (var tenantId in tenantIds)
        {
            using var scope = _scopeFactory.CreateScope();
            var requestContext = scope.ServiceProvider.GetRequiredService<RequestContext>();
            requestContext.TenantId = tenantId;

            var db = scope.ServiceProvider.GetRequiredService<FxferDbContext>();
            var adapter = scope.ServiceProvider.GetRequiredService<IEventAdapter>();

            if (db.Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) == true)
            {
                continue;
            }

            var pending = await db.OutboxMessages
                .Where(m => m.DispatchedAt == null)
                .OrderBy(m => m.OccurredAt)
                .Take(20)
                .ToListAsync(cancellationToken);

            if (pending.Count == 0)
            {
                continue;
            }

            foreach (var message in pending)
            {
                await adapter.PublishAsync(message.TenantId, message.EventName, message.EventVersion, message.PayloadJson, cancellationToken);
                message.DispatchedAt = DateTimeOffset.UtcNow;
            }

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}