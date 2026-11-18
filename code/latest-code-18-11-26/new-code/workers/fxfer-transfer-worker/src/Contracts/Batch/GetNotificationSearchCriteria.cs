namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class GetNotificationSearchCriteria
{
    public Guid ConfigId { get; set; }

    public bool NotifyOnErrorInd { get; set; }

    public bool NotifyOnSuccessInd { get; set; }
}

