using System.Linq;
using Fxfer.ExecutionWorker.Domain.Clients;

namespace Fxfer.ExecutionWorker.Application.Clients;

public sealed class FileTransferSvcQueryServiceProvider : IFileTransferSvcQueryServiceProvider
{
    private readonly IFileTransferSvcQueryService queryService;

    public FileTransferSvcQueryServiceProvider(IFileTransferSvcQueryService queryService)
    {
        this.queryService = queryService;
    }

    public FtsIntegrationFileModel? GetIntegrationFile(GetIntegrationFileQuery query)
    {
        GetIntegrationFileQuery? response = this.queryService.GetIntegrationFile(query);

        return response != null && response.Results != null && response.Results.Count > 0 ? response.Results.FirstOrDefault() : null;
    }
}


