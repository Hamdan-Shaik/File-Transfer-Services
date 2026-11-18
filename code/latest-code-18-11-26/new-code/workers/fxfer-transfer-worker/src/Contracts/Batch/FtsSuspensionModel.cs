namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class FtsSuspensionModel
{
    public Guid SakFileTrack { get; set; }

    public Guid SuspensionId { get; set; }
}

