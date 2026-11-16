namespace FxferService.Domain.Contracts.Batch;

public class ErrorLogDetailModel
{
    public Guid FileId { get; set; }

    public string? ErrorType { get; set; }

    public string? ErrorText { get; set; }

    public string? StackTrace { get; set; }

    public string? MethodName { get; set; }

    public string? AssemblyName { get; set; }

    public string? ClassName { get; set; }

    public string? MachineName { get; set; }

    public string? OperatorId { get; set; }
}

