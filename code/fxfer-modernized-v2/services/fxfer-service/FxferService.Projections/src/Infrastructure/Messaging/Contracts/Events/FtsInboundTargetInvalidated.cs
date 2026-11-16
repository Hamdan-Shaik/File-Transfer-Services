namespace FxferService.Projections.Infrastructure.Messaging.Contracts.Events;

public sealed class FtsInboundTargetInvalidated : FtsInboundTargetAdded
{
    public required string UserID { get; init; }
}

