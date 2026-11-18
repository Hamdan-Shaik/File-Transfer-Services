namespace Fxfer.Bff.Contracts.TransferConfigurations;

public sealed record TransferConfigurationSummary
{
    public Guid FtsSourceLocationId { get; init; }
    public int ConfigId { get; init; }
    public required string ConfigName { get; init; }
    public required string Protocol { get; init; }
    public required string Address { get; init; }
    public required string OwnerName { get; init; }
    public string? TradingPartner { get; init; }
    public bool IsActive { get; init; }
    public bool Suspended { get; init; }
}

