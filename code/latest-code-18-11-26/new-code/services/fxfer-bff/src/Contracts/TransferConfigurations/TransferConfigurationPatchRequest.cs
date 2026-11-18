namespace Fxfer.Bff.Contracts.TransferConfigurations;

public sealed record TransferConfigurationPatchRequest
{
    public bool? IsActive { get; init; }
    public bool? Suspended { get; init; }
    public required string OperatorId { get; init; }
}

