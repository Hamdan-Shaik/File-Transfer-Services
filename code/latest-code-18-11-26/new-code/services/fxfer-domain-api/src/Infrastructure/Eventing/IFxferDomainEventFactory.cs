using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Infrastructure.Eventing;

public interface IFxferDomainEventFactory
{
    FxferDomainEvent<TPayload> Create<TPayload>(string eventName, string eventVersion, TPayload payload, DateTimeOffset? occurredAt = null);
}

