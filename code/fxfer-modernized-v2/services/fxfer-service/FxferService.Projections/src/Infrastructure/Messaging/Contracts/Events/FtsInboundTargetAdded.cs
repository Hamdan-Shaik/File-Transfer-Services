namespace FxferService.Projections.Infrastructure.Messaging.Contracts.Events;

public class FtsInboundTargetAdded
{
    public Guid FtsInboundTargetId { get; init; }
    public Guid FTEMetadataId { get; init; }
    public required string ImportJobName { get; init; }
    public required string ContractName { get; init; }
    public required string OperatorId { get; init; }
    public bool IsValid { get; init; }
}

