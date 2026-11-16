using System.Text.Json;
using Fxfer.SchedulerWorker.Infrastructure.Messaging;

namespace Fxfer.SchedulerWorker.Infrastructure.Messaging.Publisher;

public sealed class TransferRunRequestedPublisher
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TransferRunRequestedPublisher> _logger;

    public TransferRunRequestedPublisher(IConfiguration configuration, ILogger<TransferRunRequestedPublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task<bool> CanPublishAsync(CancellationToken cancellationToken)
    {
        var mode = _configuration["Messaging:Publisher"] ?? "in-memory";
        return Task.FromResult(string.Equals(mode, "in-memory", StringComparison.Ordinal));
    }

    public ValueTask PublishAsync(
        string tenantId,
        Guid runId,
        string? correlationId,
        string? traceId,
        CancellationToken cancellationToken)
    {
        var producer = _configuration["Messaging:Producer"] ?? "fxfer-scheduler-worker";

        var payload = JsonSerializer.SerializeToElement(new { run_id = runId });

        var envelope = new MessageEnvelope(
            TenantId: tenantId,
            EventName: "TransferRunRequested",
            EventVersion: "v1",
            EventId: Guid.NewGuid(),
            OccurredAt: DateTimeOffset.UtcNow,
            CorrelationId: correlationId,
            TraceId: traceId,
            Producer: producer,
            Payload: payload);

        _logger.LogInformation(
            "Published {EventName} for tenant {TenantId} run {RunId}",
            envelope.EventName,
            tenantId,
            runId);

        // shortcut: event backbone adapter not implemented yet, upgrade when SNS/SQS publishing contract is wired.
        return ValueTask.CompletedTask;
    }
}
