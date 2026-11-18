using System.Text.Json.Serialization;

namespace Fxfer.DomainApi.Contracts.Events;

public sealed record FxferDomainEvent<TPayload>
{
    [JsonPropertyName("envelope")]
    public required FxferEventEnvelope Envelope { get; init; }

    [JsonPropertyName("payload")]
    public required TPayload Payload { get; init; }
}
