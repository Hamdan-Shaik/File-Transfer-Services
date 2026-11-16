namespace Fxfer.ExecutionWorker.Domain.Batch.Business;

public sealed class LogMessage
{
    public LogCode Code { get; set; }

    public string Message { get; set; } = string.Empty;
}