namespace Fxfer.DomainApi.Contracts.Events;

public sealed record FtsInboundTargetAdded
{
    public Guid FtsInboundTargetId { get; init; }
    public Guid FTEMetadataId { get; init; }
    public string? ImportJobName { get; init; }
    public string? ContractName { get; init; }
    public string? OperatorId { get; init; }
    public bool IsValid { get; init; }
}

