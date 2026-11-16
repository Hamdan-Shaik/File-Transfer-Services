namespace FxferService.Projections.Infrastructure.Messaging.Contracts.Events;

public sealed class FtsSourceLocationInvalidated : FtsSourceLocationEventBase
{
    public long ConfigId { get; init; }
    public required string OperatorId { get; init; }
    public List<FtsDestinationLocationAdded>? DestinationAddress { get; init; }
}

