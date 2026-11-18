namespace Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;

public sealed record FtsInboundTarget
{
    public required Guid FtsInboundTargetId { get; init; }
    public required Guid FteMetadataId { get; init; }
    public required string ImportJobName { get; init; }
    public required string ContractName { get; init; }
    public required string OperatorId { get; init; }
    public required bool IsValid { get; init; }
    public required DateTimeOffset CreatedTs { get; init; }
    public required DateTimeOffset LastModifiedTs { get; init; }
}

