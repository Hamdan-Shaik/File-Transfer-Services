using Fxfer.ExecutionWorker.Domain.Clients;

namespace Fxfer.ExecutionWorker.Application.Clients;

public sealed class FileTransferSvcServiceProvider : IFileTransferSvcServiceProvider
{
    private readonly IFileTransferSvcService service;

    public FileTransferSvcServiceProvider(IFileTransferSvcService service)
    {
        this.service = service;
    }

    public IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command)
    {
        return this.service.UpdateIntegrationFile(command);
    }
}


