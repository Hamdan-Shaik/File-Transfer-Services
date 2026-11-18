namespace Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;

public sealed record FtsFileException
{
    public required Guid ExceptionId { get; init; }
    public required Guid FileId { get; init; }
    public required Guid DestinationId { get; init; }
    public required Guid EventId { get; init; }
    public required string ExceptionType { get; init; }
    public string? MethodName { get; init; }
    public required string Message { get; init; }
    public string? StackTrace { get; init; }
    public required DateTimeOffset CreatedTs { get; init; }
    public required string OperatorId { get; init; }
    public required DateTimeOffset LastModifiedTs { get; init; }
}

