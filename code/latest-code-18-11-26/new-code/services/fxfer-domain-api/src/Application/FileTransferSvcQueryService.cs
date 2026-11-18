using Fxfer.DomainApi.Contracts.Queries;

namespace Fxfer.DomainApi.Application;

public sealed class FileTransferSvcQueryService : IFileTransferSvcQueryService
{
    public SourceLocationQuery SearchSourceLocations(SourceLocationQuery query) => query;

    public FtpServerQuery GetFtpServers(FtpServerQuery query) => query;

    public GetSourceLocationQuery GetSourceLocations(GetSourceLocationQuery query) => query;

    public FileTransferTrackingQuery SearchFileTransferMovement(FileTransferTrackingQuery query) => query;

    public InboundTargetQuery SearchInboundTarget(InboundTargetQuery query) => query;
}
