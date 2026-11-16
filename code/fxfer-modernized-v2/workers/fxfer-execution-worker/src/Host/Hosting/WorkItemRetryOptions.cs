namespace Fxfer.ExecutionWorker.Host.Hosting;

public sealed class WorkItemRetryOptions
{
    public int RetryDelayMs { get; set; } = 1000;
}
