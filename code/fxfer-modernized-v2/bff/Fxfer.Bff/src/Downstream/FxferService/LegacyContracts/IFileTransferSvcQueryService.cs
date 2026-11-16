namespace FileTransferSvcQueryService;

public interface IFileTransferSvcQueryService
{
    SourceLocationQuery SearchSourceLocations(SourceLocationQuery query);
    FtpServerQuery GetFtpServers(FtpServerQuery query);
    GetSourceLocationQuery GetSourceLocations(GetSourceLocationQuery query);
    FileTransferTrackingQuery SearchFileTransferMovement(FileTransferTrackingQuery query);
    InboundTargetQuery SearchInboundTarget(InboundTargetQuery query);
}