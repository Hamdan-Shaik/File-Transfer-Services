namespace Fxfer.ExecutionWorker.Domain.Clients;

public interface IFileTransferSvcQueryService
{
    GetIntegrationFileQuery? GetIntegrationFile(GetIntegrationFileQuery query);
}

