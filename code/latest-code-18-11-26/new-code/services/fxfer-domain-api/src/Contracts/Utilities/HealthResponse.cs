using System.Text.Json.Serialization;

namespace Fxfer.DomainApi.Contracts.Utilities;

public sealed record HealthResponse
{
    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("time_utc")]
    public required DateTimeOffset TimeUtc { get; init; }
}
