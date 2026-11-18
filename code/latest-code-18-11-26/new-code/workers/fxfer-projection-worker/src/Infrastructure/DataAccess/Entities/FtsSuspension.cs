namespace Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;

public sealed record FtsSuspension
{
    public required Guid SuspensionId { get; init; }
    public required Guid FtsSourceLocationId { get; init; }
    public required string ErrorText { get; init; }
    public required string StackTrace { get; init; }
    public required string OperatorId { get; init; }
    public required DateTimeOffset CreatedTs { get; init; }
    public required DateTimeOffset LastModifiedTs { get; init; }
}

