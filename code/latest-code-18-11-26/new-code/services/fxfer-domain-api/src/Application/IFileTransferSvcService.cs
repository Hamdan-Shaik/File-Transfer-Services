using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Application;

public interface IFileTransferSvcService : IServiceAvailability
{
    FtsSourceLocationAdded FileTransferAdd(FtsSourceLocationAddCommand command);

    FtsSourceLocationUpdated UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand command);

    FtsSourceLocationInvalidated InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand command);

    FtpServerAdded AddFtpServer(AddFtpServerCommand command);

    FtpServerUpdated UpdateFtpServer(UpdateFtpServerCommand command);

    FtpServerInvalidated InvalidateFtpServer(InvalidateFtpServerCommand command);
}
