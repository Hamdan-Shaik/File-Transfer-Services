using System.Text.Json;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Infrastructure.DataAccess;

namespace Fxfer.DomainApi.Infrastructure.Eventing.Outbox;

// T-006: Store tenant-aware, schema-versioned domain events in the outbox.
public sealed class OutboxMessageWriter
{
    private readonly FxferDbContext _dbContext;
    private readonly FxferDomainEventFactory _eventFactory;

    public OutboxMessageWriter(FxferDbContext dbContext, FxferDomainEventFactory eventFactory)
    {
        _dbContext = dbContext;
        _eventFactory = eventFactory;
    }

    public OutboxMessage Enqueue<TPayload>(string eventName, string eventVersion, TPayload payload, DateTimeOffset? occurredAt = null)
    {
        var domainEvent = _eventFactory.Create(eventName, eventVersion, payload, occurredAt);
        return Enqueue(domainEvent);
    }

    public OutboxMessage Enqueue<TPayload>(FxferDomainEvent<TPayload> domainEvent)
    {
        var json = JsonSerializer.Serialize(domainEvent);
        var message = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            TenantId = domainEvent.Envelope.TenantId,
            EventName = domainEvent.Envelope.EventName,
            EventVersion = domainEvent.Envelope.EventVersion,
            EventId = domainEvent.Envelope.EventId,
            OccurredAt = domainEvent.Envelope.OccurredAt,
            CorrelationId = domainEvent.Envelope.CorrelationId,
            TraceId = domainEvent.Envelope.TraceId,
            Producer = domainEvent.Envelope.Producer,
            PayloadJson = json,
            PublishedAt = null
        };

        _dbContext.OutboxMessages.Add(message);
        return message;
    }
}

