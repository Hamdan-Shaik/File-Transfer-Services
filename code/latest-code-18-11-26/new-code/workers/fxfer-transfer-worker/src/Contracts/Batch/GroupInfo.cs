namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class GroupInfo
{
    public Guid? GroupId { get; set; }

    public int NumberOfRecords { get; set; }
}

