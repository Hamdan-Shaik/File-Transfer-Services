using System;
using System.Collections.Generic;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;
using Fxfer.ExecutionWorker.Domain.Batch.Business;

namespace Fxfer.ExecutionWorker.Batch.Business;

public interface IFileTransferOperations
{
    bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination);

    bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination, string tenantId);

    string UploadFile(string fullyQualifiedSourceFileName, string fullyQualifiedDestinationFileName, DestinationModel destinationLocation);

    bool DeleteFile(SourceLocationModel source, string fullyQualifedSourceFile);

    bool RenameSourceFile(SourceLocationModel source, string fullyQualifedSourceFile, string renameExtension);

    List<FileBasicInfo> GetListOfFileNamesFromSource(SourceLocationModel location, int maxFilesPerCycle);

    string GetDestinationFileName(string fullyQualifiedSourceFileName, DestinationModel destinationLocation, Guid fileId, string crc32, string uploadedSFTPUserName);

    string CreateZipFile(string inputFileName);

    void SendNotification(long configId, string subject, string body, bool isErrorNotification);

    void SendNotification(SourceLocationModel source, string subject, string body, bool isErrorNotification, string tenantId);

    void PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId);

    string GetCrc32(string filePath);
}
