namespace FileTransferConfig;

public interface IFileTransferSvcService
{
    FtsSourceLocationAdded FileTransferAdd(FtsSourceLocationAddCommand command);
    FtsSourceLocationUpdated UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand command);
    FtsSourceLocationInvalidated InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand command);

    FtpServerAdded AddFtpServer(AddFtpServerCommand command);
    FtpServerUpdated UpdateFtpServer(UpdateFtpServerCommand command);
    FtpServerInvalidated InvalidateFtpServer(InvalidateFtpServerCommand command);
}
