namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class IntegrationFileQueryParms
{
    public Guid FtsIntegrationFileId { get; set; }

    public string? FileName { get; set; }
}


