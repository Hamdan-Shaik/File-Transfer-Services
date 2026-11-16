using System.Collections.Generic;
using System.IO;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;

namespace Fxfer.ExecutionWorker.Infrastructure.Protocols;

public interface IFileTransferProtocols
{
    void DownloadFromUNCConnection(string sourceFileName, DestinationModel destination);

    void UploadToUnc(string fullyQualifiedSourceFileName, string fullyQualifiedDestinationFileName, DestinationModel destinationLocation);

    void DeleteFromUNC(string fullyQualifedSourceFile);

    bool RenameFileFromUNC(string fullyQualifiedSourceFileName, string renameExtension);

    List<string> GetFileListFromUncSource(SourceLocationModel location);

    string GetCrc32(string filePath);

    void CreateZipFile(string inputFileName, string zipFileName, FileInfo fileInfo);

    List<string> GetFilesFromSource(FTPConnection connection, SourceLocationModel location);

    void DownloadFromFtpConnection(FTPConnection remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation);

    void UploadToFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, FTPConnection remoteServer);

    bool DeleteFromFtp(FTPConnection remoteServer, string fullyQualifiedSourceFileName);

    bool RenameFtpFile(string fullyQualifiedSourceFileName, string renameExtension, FTPConnection remoteServer);
}
