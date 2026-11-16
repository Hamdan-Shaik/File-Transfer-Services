using FxferService.Domain.Contracts.Domain;

namespace FxferService.Application.LegacyContracts;

public sealed record AddIntegrationFileCommand : Command
{
    public List<GroupInfo>? GroupInfo { get; init; }

    public string? FileName { get; init; }

    public Guid ETFOutboundMetadataDefnId { get; init; }

    public bool IsValid { get; init; } = true;
}

