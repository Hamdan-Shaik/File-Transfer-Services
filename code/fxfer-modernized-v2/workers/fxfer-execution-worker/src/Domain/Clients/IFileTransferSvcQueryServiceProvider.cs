namespace Fxfer.ExecutionWorker.Domain.Clients;

public interface IFileTransferSvcQueryServiceProvider
{
    FtsIntegrationFileModel? GetIntegrationFile(GetIntegrationFileQuery query);
}


