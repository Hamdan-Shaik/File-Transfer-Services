namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed record IntegrationFileCreatedProjectionPayload
{
    public required Guid FtsIntegrationFileId { get; init; }
    public required IReadOnlyList<GroupInfoProjectionPayload> GroupInfo { get; init; }
    public int? ConfigId { get; init; }
    public required string FileName { get; init; }
    public required Guid EtfOutboundMetadataDefnId { get; init; }
    public required string OperatorId { get; init; }
}
