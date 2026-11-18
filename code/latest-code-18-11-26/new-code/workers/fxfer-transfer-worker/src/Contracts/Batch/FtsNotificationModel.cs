namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class FtsNotificationModel
{
    public string? EmailAddress { get; set; }

    public string? ContactName { get; set; }

    public bool NotifyOnErrorInd { get; set; }

    public bool NotifyOnSuccessInd { get; set; }

    public Guid ConfigId { get; set; }
}

