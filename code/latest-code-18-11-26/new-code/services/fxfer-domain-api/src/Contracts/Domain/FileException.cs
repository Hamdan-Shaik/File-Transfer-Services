namespace Fxfer.DomainApi.Contracts.Domain;

/// <summary>
/// <para>Object containing properties of a File Tracking Exception.</para>
/// </summary>
public sealed record FileException
{
    public required Guid ExceptionId { get; init; }
    public required Guid FileId { get; init; }
    public Guid DestinationId { get; init; }
    public Guid EventId { get; init; }
    public required string ExceptionType { get; init; }
    public required string MethodName { get; init; }
    public string? Message { get; init; }
    public string? StackTrace { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
}

