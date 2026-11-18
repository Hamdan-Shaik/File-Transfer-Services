using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fxfer.ProjectionWorker.Worker.Eventing;

public sealed record FxferEventEnvelope
{
    [JsonPropertyName("tenant_id")]
    public required string TenantId { get; init; }

    [JsonPropertyName("event_name")]
    public required string EventName { get; init; }

    [JsonPropertyName("event_version")]
    public required string EventVersion { get; init; }

    [JsonPropertyName("event_id")]
    public required Guid EventId { get; init; }

    [JsonPropertyName("occurred_at")]
    public required DateTimeOffset OccurredAt { get; init; }

    [JsonPropertyName("correlation_id")]
    public required string CorrelationId { get; init; }

    [JsonPropertyName("trace_id")]
    public required string TraceId { get; init; }

    [JsonPropertyName("producer")]
    public required string Producer { get; init; }

    [JsonPropertyName("payload")]
    public JsonElement Payload { get; init; }
}

