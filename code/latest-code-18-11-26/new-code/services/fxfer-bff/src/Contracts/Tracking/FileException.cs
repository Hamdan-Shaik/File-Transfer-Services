namespace Fxfer.Bff.Contracts.Tracking;

public sealed record FileException
{
    public Guid ExceptionId { get; init; }
    public Guid FileId { get; init; }
    public Guid? DestinationId { get; init; }
    public Guid? EventId { get; init; }
    public required string ExceptionType { get; init; }
    public required string MethodName { get; init; }
    public required string Message { get; init; }
    public required string StackTrace { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
}

