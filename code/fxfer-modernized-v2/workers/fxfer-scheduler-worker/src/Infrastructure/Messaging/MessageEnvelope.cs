using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fxfer.SchedulerWorker.Infrastructure.Messaging;

public sealed record MessageEnvelope(
    [property: JsonPropertyName("tenant_id")] string TenantId,
    [property: JsonPropertyName("event_name")] string EventName,
    [property: JsonPropertyName("event_version")] string EventVersion,
    [property: JsonPropertyName("event_id")] Guid EventId,
    [property: JsonPropertyName("occurred_at")] DateTimeOffset OccurredAt,
    [property: JsonPropertyName("correlation_id")] string? CorrelationId,
    [property: JsonPropertyName("trace_id")] string? TraceId,
    [property: JsonPropertyName("producer")] string Producer,
    [property: JsonPropertyName("payload")] JsonElement Payload);
