using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Domain.Adapters;

public interface IFileTransferSvcQueryServiceProvider
{
    FtsIntegrationFileModel? GetIntegrationFile(GetIntegrationFileQuery query);
}

