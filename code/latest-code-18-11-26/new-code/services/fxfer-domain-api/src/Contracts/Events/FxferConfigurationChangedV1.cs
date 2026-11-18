using System.Text.Json.Serialization;

namespace Fxfer.DomainApi.Contracts.Events;

public sealed record FxferConfigurationChangedV1
{
    [JsonPropertyName("aggregate_type")]
    public required string AggregateType { get; init; }

    [JsonPropertyName("aggregate_id")]
    public required Guid AggregateId { get; init; }

    [JsonPropertyName("change_kind")]
    public required string ChangeKind { get; init; }

    [JsonPropertyName("operator_id")]
    public string? OperatorId { get; init; }

    [JsonPropertyName("changed_at")]
    public required DateTimeOffset ChangedAt { get; init; }
}
