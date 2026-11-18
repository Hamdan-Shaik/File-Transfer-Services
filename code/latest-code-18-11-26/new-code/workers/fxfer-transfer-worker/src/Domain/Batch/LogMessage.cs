namespace Fxfer.TransferWorker.Domain.Batch;

internal sealed class LogMessage
{
    public LogCode Code { get; set; }

    public string Message { get; set; } = string.Empty;
}

