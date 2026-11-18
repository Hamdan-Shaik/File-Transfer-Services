using System.Text.Json.Serialization;

namespace Fxfer.TransferWorker.Worker.Eventing;

public sealed record TransferWorkItemEnvelope<TPayload>
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
    public string? CorrelationId { get; init; }

    [JsonPropertyName("trace_id")]
    public string? TraceId { get; init; }

    [JsonPropertyName("traceparent")]
    public string? TraceParent { get; init; }

    [JsonPropertyName("tracestate")]
    public string? TraceState { get; init; }

    [JsonPropertyName("baggage")]
    public string? Baggage { get; init; }

    [JsonPropertyName("producer")]
    public string? Producer { get; init; }

    [JsonPropertyName("payload")]
    public required TPayload Payload { get; init; }
}

public sealed record FxferTransferWorkItemV1
{
    [JsonPropertyName("execution_id")]
    public required Guid ExecutionId { get; init; }

    [JsonPropertyName("source_location_id")]
    public required Guid SourceLocationId { get; init; }

    [JsonPropertyName("destination_id")]
    public Guid? DestinationId { get; init; }

    [JsonPropertyName("file_identity_key")]
    public string? FileIdentityKey { get; init; }

    [JsonPropertyName("run_mode")]
    public required string RunMode { get; init; }

    [JsonPropertyName("requested_by")]
    public string? RequestedBy { get; init; }
}

