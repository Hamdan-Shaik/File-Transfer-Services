using System;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class ErrorLogDetailModel
{
    public Guid FileId { get; set; }

    public Guid DestinationId { get; set; }

    public string ErrorType { get; set; } = null!;

    public string ErrorText { get; set; } = null!;

    public string StackTrace { get; set; } = null!;

    public string MethodName { get; set; } = null!;

    public string AssemblyName { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public string MachineName { get; set; } = null!;

    public string OperatorId { get; set; } = null!;
}