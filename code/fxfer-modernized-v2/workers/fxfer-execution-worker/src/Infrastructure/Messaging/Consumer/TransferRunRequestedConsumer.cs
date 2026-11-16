using System.Threading.Channels;
using Fxfer.ExecutionWorker.Infrastructure.Messaging;
using Fxfer.ExecutionWorker.Infrastructure.Tenancy;

namespace Fxfer.ExecutionWorker.Infrastructure.Messaging.Consumer;

public interface IWorkItemQueue
{
    ChannelReader<MessageEnvelope> Reader { get; }
    ValueTask EnqueueAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default);
}

public sealed class InMemoryWorkItemQueue : IWorkItemQueue
{
    private readonly Channel<MessageEnvelope> _channel = Channel.CreateUnbounded<MessageEnvelope>();

    public ChannelReader<MessageEnvelope> Reader => _channel.Reader;

    public ValueTask EnqueueAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default)
    {
        return _channel.Writer.WriteAsync(envelope, cancellationToken);
    }
}

public sealed class TransferRunRequestedConsumer
{
    private readonly TenantContext _tenant;
    private readonly TransferRunProcessor _processor;

    public TransferRunRequestedConsumer(TenantContext tenant, TransferRunProcessor processor)
    {
        _tenant = tenant;
        _processor = processor;
    }

    public Task ConsumeAsync(MessageEnvelope envelope, CancellationToken cancellationToken)
    {
        if (!string.Equals(envelope.EventName, "TransferRunRequested", StringComparison.Ordinal))
        {
            return Task.CompletedTask;
        }

        if (envelope.Payload.ValueKind != System.Text.Json.JsonValueKind.Object)
        {
            return Task.CompletedTask;
        }

        if (!envelope.Payload.TryGetProperty("run_id", out var runIdElement))
        {
            return Task.CompletedTask;
        }

        var runIdString = runIdElement.GetString();
        return Guid.TryParse(runIdString, out var runId)
            ? _processor.ProcessAsync(_tenant.TenantId, runId, cancellationToken)
            : Task.CompletedTask;
    }
}
