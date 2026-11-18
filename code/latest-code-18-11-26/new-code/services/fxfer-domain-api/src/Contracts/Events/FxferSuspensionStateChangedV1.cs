using System.Text.Json.Serialization;

namespace Fxfer.DomainApi.Contracts.Events;

public sealed record FxferSuspensionStateChangedV1
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
