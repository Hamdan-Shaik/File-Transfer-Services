namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class GetIntegrationFileQuery
{
    public RequestorModel? Requestor { get; set; }

    public IntegrationFileQueryParms? Where { get; set; }
}

