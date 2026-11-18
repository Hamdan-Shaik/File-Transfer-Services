using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Fxfer.DomainApi.Contracts.Events;

public record FtsSourceLocationEvents
{
    [IgnoreDataMember]
    [JsonIgnore]
    public string ModelType => "FileTransfer";

    public string? FtsSourceLocationsId { get; init; }
}

