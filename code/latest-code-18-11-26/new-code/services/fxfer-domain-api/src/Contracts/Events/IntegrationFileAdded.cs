using Fxfer.DomainApi.Contracts.Domain;

namespace Fxfer.DomainApi.Contracts.Events;

public record IntegrationFileAdded
{
    public Guid FtsIntegrationFileId { get; init; }
    public List<GroupInfo>? GroupInfo { get; init; }
    public int? ConfigId { get; init; }
    public string? FileName { get; init; }
    public Guid ETFOutboundMetadataId { get; init; }
    public bool IsProcessed { get; init; }
    public string? OperatorId { get; init; }
    public bool IsValid { get; init; }
}

