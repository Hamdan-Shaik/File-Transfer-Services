using System.Text.Json.Serialization;

namespace FxferService.Projections.Infrastructure.Messaging.Consumer;

public sealed record MessageEnvelope(
    [property: JsonPropertyName("tenant_id")] string TenantId,
    [property: JsonPropertyName("event_name")] string EventName,
    [property: JsonPropertyName("event_version")] int EventVersion,
    [property: JsonPropertyName("event_id")] Guid EventId,
    [property: JsonPropertyName("occurred_at")] DateTimeOffset OccurredAt,
    [property: JsonPropertyName("correlation_id")] string? CorrelationId,
    [property: JsonPropertyName("trace_id")] string? TraceId,
    [property: JsonPropertyName("producer")] string Producer,
    [property: JsonPropertyName("payload")] object Payload);
