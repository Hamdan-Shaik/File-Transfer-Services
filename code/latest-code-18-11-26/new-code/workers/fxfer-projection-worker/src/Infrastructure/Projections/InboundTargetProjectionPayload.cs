namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed record InboundTargetProjectionPayload
{
    public required Guid FtsInboundTargetId { get; init; }
    public required Guid FteMetadataId { get; init; }
    public required string ImportJobName { get; init; }
    public required string ContractName { get; init; }
    public required string OperatorId { get; init; }
    public required bool IsValid { get; init; }
}
