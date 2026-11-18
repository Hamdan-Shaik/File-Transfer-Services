namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed record IntegrationFileUpdatedProjectionPayload
{
    public required Guid FtsIntegrationFileId { get; init; }
    public int? ConfigId { get; init; }
    public required string OperatorId { get; init; }
    public required bool IsValid { get; init; }
}
