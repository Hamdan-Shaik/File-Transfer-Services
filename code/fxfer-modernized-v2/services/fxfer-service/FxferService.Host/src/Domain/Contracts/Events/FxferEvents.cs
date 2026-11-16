namespace FxferService.Domain.Contracts.Events;

public sealed record CanonicalEventEnvelope(
    string TenantId,
    string EventName,
    string EventVersion,
    Guid EventId,
    DateTimeOffset OccurredAt,
    string CorrelationId,
    string TraceId,
    string Producer);

public sealed record FxferConfigurationChangedPayload(
    string AggregateType,
    string AggregateId,
    string ChangeType,
    DateTimeOffset EffectiveAt);

public sealed record FxferRunStateChangedPayload(
    Guid RunId,
    Guid TransferConfigurationId,
    string State,
    DateTimeOffset StateChangedAt);

public sealed record FxferDeliveryOutcomeRecordedPayload(
    Guid RunId,
    Guid FileId,
    Guid DestinationId,
    string OutcomeCode,
    DateTimeOffset OccurredAt);

public sealed record FxferSuspensionStateChangedPayload(
    Guid TransferConfigurationId,
    bool IsSuspended,
    string ReasonCode,
    DateTimeOffset ChangedAt);
