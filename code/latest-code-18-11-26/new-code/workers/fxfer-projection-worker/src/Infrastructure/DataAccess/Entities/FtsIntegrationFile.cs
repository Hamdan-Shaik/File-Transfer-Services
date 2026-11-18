namespace Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;

public sealed record FtsIntegrationFile
{
    public required Guid FtsIntegrationFileId { get; init; }
    public Guid? GroupId { get; init; }
    public int? ConfigId { get; init; }
    public required string FileName { get; init; }
    public required Guid EtfOutboundMetadataDefnId { get; init; }
    public required bool IsProcessed { get; init; }
    public required string OperatorId { get; init; }
    public required bool IsValid { get; init; }
    public required DateTimeOffset CreatedTs { get; init; }
    public required DateTimeOffset LastModifiedTs { get; init; }
}

