using EnterpriseDT.Net.Ftp;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Domain.Batch;
using Renci.SshNet;

namespace Fxfer.TransferWorker.Application.Batch;

public interface IFileTransferProtocols
{
    void DownloadFromUNCConnection(string sourceFileName, DestinationModel destination);

    void DownloadFromFtpConnection(FTPConnection remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation, string tenantId);

    void DownloadFromSSHFtpConnection(SftpClient remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation, string tenantId);

    string UploadToUnc(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation);

    bool UploadToFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, FTPConnection remoteServer);

    void UploadToSSHFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, SftpClient remoteServer);

    bool DeleteFromUNC(string fullyQualifiedSourceFileName);

    bool DeleteFromFtp(FTPConnection remoteServer, string fileName);

    bool DeleteFromSFtp(SftpClient remoteServer, string fileName);

    bool RenameFileFromUNC(string fullyQualifiedSourceFileName, string renameExtension);

    bool RenameFtpFile(string fullyQualifiedSourceFileName, string renameExtension, FTPConnection remoteServer);

    bool RenameSSHFtpFile(string fullyQualifiedSourceFileName, string renameExtension, SftpClient remoteServer);

    List<FileBasicInfo> GetFileListFromUncSource(SourceLocationModel location);

    List<FileBasicInfo> GetFilesFromSource(FTPConnection remoteServer, SourceLocationModel location);

    List<FileBasicInfo> GetFilesFromSSHSource(SftpClient remoteServer, SourceLocationModel location);

    void CreateZipFile(string inputFileName, string zipFileName, FileInfo fileInfo);

    string GetCrc32(string filePath);
}

