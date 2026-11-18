namespace Fxfer.DomainApi.Contracts.Domain;

public sealed class FileExceptionModel
{
    public Guid ExceptionId { get; set; }

    public Guid FileId { get; set; }

    public Guid DestinationId { get; set; }

    public Guid EventId { get; set; }

    public string? ExceptionType { get; set; }

    public string? MethodName { get; set; }

    public string? Message { get; set; }

    public string? StackTrace { get; set; }

    public DateTime CreatedDate { get; set; }
}

