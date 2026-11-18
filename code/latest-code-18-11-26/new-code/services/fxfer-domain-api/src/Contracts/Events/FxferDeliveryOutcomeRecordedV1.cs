using System.Text.Json.Serialization;

namespace Fxfer.DomainApi.Contracts.Events;

public sealed record FxferDeliveryOutcomeRecordedV1
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
