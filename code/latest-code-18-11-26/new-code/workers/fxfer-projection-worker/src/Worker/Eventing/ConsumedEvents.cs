using System.Text.Json.Serialization;

namespace Fxfer.ProjectionWorker.Worker.Eventing;

public static class ConsumedEvents
{
    public const string ConfigurationChangedV1Name = "Fxfer.ConfigurationChanged";
    public const string SuspensionStateChangedV1Name = "Fxfer.SuspensionStateChanged";
    public const string DeliveryOutcomeRecordedV1Name = "Fxfer.DeliveryOutcomeRecorded";

    public const string V1 = "v1";
}

public sealed record ConfigurationChangedV1
{
    [JsonPropertyName("aggregate_type")]
    public required string AggregateType { get; init; }

    [JsonPropertyName("aggregate_id")]
    public required Guid AggregateId { get; init; }

    [JsonPropertyName("change_kind")]
    public required string ChangeKind { get; init; }

    [JsonPropertyName("operator_id")]
    public required string OperatorId { get; init; }

    [JsonPropertyName("changed_at")]
    public required DateTimeOffset ChangedAt { get; init; }
}

public sealed record SuspensionStateChangedV1
{
    [JsonPropertyName("source_location_id")]
    public required Guid SourceLocationId { get; init; }

    [JsonPropertyName("suspended")]
    public required bool Suspended { get; init; }

    [JsonPropertyName("reason_code")]
    public required string ReasonCode { get; init; }

    [JsonPropertyName("changed_at")]
    public required DateTimeOffset ChangedAt { get; init; }
}

public sealed record DeliveryOutcomeRecordedV1
{
    [JsonPropertyName("file_id")]
    public required Guid FileId { get; init; }

    [JsonPropertyName("source_location_id")]
    public required Guid SourceLocationId { get; init; }

    [JsonPropertyName("destination_id")]
    public required Guid DestinationId { get; init; }

    [JsonPropertyName("outcome_status")]
    public required string OutcomeStatus { get; init; }

    [JsonPropertyName("occurred_at")]
    public required DateTimeOffset OccurredAt { get; init; }
}

