export interface IFileTransferServiceDataProvider {
  AddFileTransferBaseConfiguration(
    request: RequestModel<FileTransferConfigurationModel>,
  ): ResponseModel<FileTransferConfigurationModel>;

  UpdateFileTransferBaseConfiguration(
    request: RequestModel<FileTransferConfigurationModel>,
  ): ResponseModel<FileTransferConfigurationModel>;

  InvalidateFileTransferBaseConfiguration(
    request: RequestModel<FileTransferConfigurationModel>,
  ): ResponseModel<FileTransferConfigurationModel>;

  AddFtpServer(request: RequestModel<FtpServerModel>): ResponseModel<FtpServerModel>;

  UpdateFtpServer(request: RequestModel<FtpServerModel>): ResponseModel<FtpServerModel>;

  InvalidateFtpServer(request: RequestModel<FtpServerModel>): ResponseModel<FtpServerModel>;
}

type RequestModel<T> = {
  Data: T;
  Requestor?: unknown;
};

type ResponseModel<T> = {
  Data: T | null;
  Result?: unknown;
};

type FileTransferConfigurationModel = unknown;
type FtpServerModel = unknown;
