using System.Collections.Generic;
using System.IO;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;
using Fxfer.ExecutionWorker.Domain.Batch.Business;
using Fxfer.ExecutionWorker.Infrastructure.Protocols;
using Renci.SshNet;

namespace Fxfer.ExecutionWorker.Batch.Business;

public interface IFileTransferProtocols
{
    void DownloadFromUNCConnection(string sourceFileName, DestinationModel destination);

    void DownloadFromFtpConnection(FTPConnection remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation, string tenantId);

    void DownloadFromSSHFtpConnection(SftpClient remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation, string tenantId);

    bool UploadToFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, FTPConnection remoteServer);

    void UploadToSSHFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, SftpClient remoteServer);

    bool DeleteFromUNC(string fullyQualifiedSourceFileName);

    bool RenameFileFromUNC(string fullyQualifiedSourceFileName, string renameExtension);

    List<FileBasicInfo> GetFileListFromUncSource(SourceLocationModel location);

    string GetCrc32(string filePath);

    void CreateZipFile(string inputFileName, string zipFileName, FileInfo fileInfo);

    List<FileBasicInfo> GetFilesFromSource(FTPConnection connection, SourceLocationModel location);

    List<FileBasicInfo> GetFilesFromSSHSource(SftpClient remoteServer, SourceLocationModel location);

    string UploadToUnc(string fullyQualifiedSourceFileName, string fullyQualifiedDestinationFileName, DestinationModel destinationLocation);

    bool DeleteFromFtp(FTPConnection remoteServer, string fullyQualifiedSourceFileName);

    bool DeleteFromSFtp(SftpClient remoteServer, string fullyQualifiedSourceFileName);

    bool RenameFtpFile(string fullyQualifiedSourceFileName, string renameExtension, FTPConnection remoteServer);

    bool RenameSSHFtpFile(string fullyQualifiedSourceFileName, string renameExtension, SftpClient remoteServer);
}

