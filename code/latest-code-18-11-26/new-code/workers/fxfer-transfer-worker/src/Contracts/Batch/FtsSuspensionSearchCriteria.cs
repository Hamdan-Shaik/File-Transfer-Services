namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class FtsSuspensionSearchCriteria
{
    public Guid SourceLocationId { get; set; }

    public string? ErrorText { get; set; }

    public string? StackTrace { get; set; }
}

