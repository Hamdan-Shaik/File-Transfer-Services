namespace Fxfer.ExecutionWorker.Domain.Clients;

public interface IFileTransferSvcService
{
    IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command);
}

