namespace Fxfer.ExecutionWorker.Domain.Clients;

public interface IFileTransferSvcServiceProvider
{
    IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command);
}


