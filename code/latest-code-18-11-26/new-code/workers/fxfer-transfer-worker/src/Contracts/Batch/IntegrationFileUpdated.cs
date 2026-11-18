namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class IntegrationFileUpdated : IntegrationFileAdded
{
    public DateTime LastModifiedDate { get; set; }
}

