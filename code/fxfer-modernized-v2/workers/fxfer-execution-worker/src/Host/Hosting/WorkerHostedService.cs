using Fxfer.ExecutionWorker.Infrastructure.Messaging.Consumer;
using Fxfer.ExecutionWorker.Infrastructure.Tenancy;
using Fxfer.ExecutionWorker.Infrastructure.Messaging;
using Microsoft.Extensions.Options;

namespace Fxfer.ExecutionWorker.Host.Hosting;

public sealed class WorkerHostedService : BackgroundService
{
    private readonly IWorkItemQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<WorkerHostedService> _logger;
    private readonly WorkItemRetryOptions _retryOptions;

    public WorkerHostedService(
        IWorkItemQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<WorkerHostedService> logger,
        IOptions<WorkItemRetryOptions> retryOptions)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _retryOptions = retryOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Fxfer execution worker started");

        await foreach (var envelope in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            await HandleEnvelopeAsync(envelope, stoppingToken);
        }
    }

    internal async Task HandleEnvelopeAsync(MessageEnvelope envelope, CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();

            var tenantContext = scope.ServiceProvider.GetRequiredService<TenantContext>();
            tenantContext.TenantId = envelope.TenantId;
            tenantContext.CorrelationId = envelope.CorrelationId;
            tenantContext.TraceId = envelope.TraceId;

            using var _ = _logger.BeginScope(new Dictionary<string, object?>
            {
                ["tenant_id"] = envelope.TenantId,
                ["correlation_id"] = envelope.CorrelationId,
                ["event_id"] = envelope.EventId,
                ["event_name"] = envelope.EventName,
                ["event_version"] = envelope.EventVersion,
                ["trace_id"] = envelope.TraceId,
            });

            var consumer = scope.ServiceProvider.GetRequiredService<TransferRunRequestedConsumer>();
            await consumer.ConsumeAsync(envelope, stoppingToken);
        }
        catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
        {
            // T-005: at-least-once delivery semantics require redelivery on failure.
            _logger.LogError(ex, "Work item processing failed; scheduling retry");

            await _queue.EnqueueAsync(envelope, stoppingToken);

            if (_retryOptions.RetryDelayMs > 0)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(_retryOptions.RetryDelayMs), stoppingToken);
            }
        }
    }
}
