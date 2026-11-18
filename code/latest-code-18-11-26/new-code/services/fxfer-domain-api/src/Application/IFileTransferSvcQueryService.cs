using Fxfer.DomainApi.Contracts.Queries;

namespace Fxfer.DomainApi.Application;

public interface IFileTransferSvcQueryService : IServiceAvailability
{
    SourceLocationQuery SearchSourceLocations(SourceLocationQuery query);

    FtpServerQuery GetFtpServers(FtpServerQuery query);

    GetSourceLocationQuery GetSourceLocations(GetSourceLocationQuery query);

    FileTransferTrackingQuery SearchFileTransferMovement(FileTransferTrackingQuery query);

    InboundTargetQuery SearchInboundTarget(InboundTargetQuery query);
}
