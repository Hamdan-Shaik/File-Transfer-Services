using FxferService.Projections.Infrastructure.Messaging.Contracts.Domain;

namespace FxferService.Projections.Infrastructure.Messaging.Contracts.Events;

public class IntegrationFileAdded
{
    public Guid FtsIntegrationFileId { get; init; }
    public required List<GroupInfo> GroupInfo { get; init; }
    public int? ConfigId { get; init; }
    public required string FileName { get; init; }
    public Guid ETFOutboundMetadataId { get; init; }
    public bool IsProcessed { get; init; }
    public required string OperatorId { get; init; }
    public bool IsValid { get; init; }
}

