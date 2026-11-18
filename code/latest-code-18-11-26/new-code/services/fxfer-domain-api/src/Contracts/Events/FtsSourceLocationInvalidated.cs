namespace Fxfer.DomainApi.Contracts.Events;

public sealed record FtsSourceLocationInvalidated : FtsSourceLocationEvents
{
    public long ConfigId { get; init; }
    public string? OperatorId { get; init; }
    public List<FtsDestinationLocationAdded>? DestinationAddress { get; init; }
}
