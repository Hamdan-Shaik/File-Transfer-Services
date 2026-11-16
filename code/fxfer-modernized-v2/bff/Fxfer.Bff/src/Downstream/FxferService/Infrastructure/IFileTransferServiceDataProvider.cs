using Fxfer.Bff.Downstream.FxferService.Models;

namespace Fxfer.Bff.Downstream.FxferService;

public interface IFileTransferServiceDataProvider
{
    ResponseModel<FileTransferConfigurationModel> AddFileTransferBaseConfiguration(RequestModel<FileTransferConfigurationModel> request);
    ResponseModel<FileTransferConfigurationModel> UpdateFileTransferBaseConfiguration(RequestModel<FileTransferConfigurationModel> request);
    ResponseModel<FileTransferConfigurationModel> InvalidateFileTransferBaseConfiguration(RequestModel<FileTransferConfigurationModel> request);

    ResponseModel<FtpServerModel> AddFtpServer(RequestModel<FtpServerModel> request);
    ResponseModel<FtpServerModel> UpdateFtpServer(RequestModel<FtpServerModel> request);
    ResponseModel<FtpServerModel> InvalidateFtpServer(RequestModel<FtpServerModel> request);
}

