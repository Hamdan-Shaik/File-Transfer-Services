namespace Fxfer.DomainApi.Contracts.Events;

public sealed record FtsInboundTargetInvalidated
{
    public Guid FtsInboundTargetId { get; init; }
    public bool IsValid { get; init; }
}
