namespace FxferService.Application.LegacyContracts;

public record AddFtsInboundTargetCommand : Command
{
    public Guid FTEMetadataId { get; init; }

    public string? ImportJobName { get; init; }

    public string? ContractName { get; init; }

    public bool IsValid { get; init; } = true;
}

