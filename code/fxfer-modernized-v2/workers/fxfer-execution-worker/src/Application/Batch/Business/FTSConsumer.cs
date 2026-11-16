using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;
using Fxfer.ExecutionWorker.Application.Helpers;
using Fxfer.ExecutionWorker.Batch.Business.DataAccess;
using Fxfer.ExecutionWorker.Domain.Batch.Business;
using Fxfer.ExecutionWorker.Domain.Clients;
using Fxfer.ExecutionWorker.Domain.Constants;
using Fxfer.ExecutionWorker.Domain.Exceptions;
using Fxfer.ExecutionWorker.Infrastructure.Tenancy;

namespace Fxfer.ExecutionWorker.Batch.Business;

public class FTSConsumer : IFTSConsumer
{
    private const string Decrypt = "DECRYPT";
    private const string Encrypt = "ENCRYPT";
    private const string PgpExtention = ".pgp";

    private static readonly List<LogMessage> logFile = new();

    private readonly IDataAccessFactory dataAccessFactory;
    private readonly IFileTransferOperations fileTrasferOperations;
    private readonly IFileTransferSvcQueryServiceProvider fileTransferQueryProvider;
    private readonly IFileTransferSvcServiceProvider fileTransferProvider;
    private readonly EventHelper eventHelper;
    private readonly IAppSettings appSettings;
    private readonly TenantContext tenantContext;

    private string fullyQualifiedDestinationFileName = string.Empty;
    private int ftsProcessId;
    private Guid eventId;
    private DestinationModel destmodel;
    private string? fullyQualifiedWorkingZipFile;
    private string decryptedPgpData = string.Empty;
    private string outputFileName = string.Empty;
    private bool deleteSourceFile;

    private string TenantId { get; set; } = string.Empty;

    public FTSConsumer(int consumerIdentifier)
        : this(
            fileTransferOperations: BatchServiceLocator.GetRequiredService<IFileTransferOperations>(),
            dataAccessFactory: BatchServiceLocator.GetRequiredService<IDataAccessFactory>(),
            appSettings: BatchServiceLocator.GetRequiredService<IAppSettings>(),
            tenantContext: BatchServiceLocator.GetRequiredService<TenantContext>(),
            eventHelper: BatchServiceLocator.GetRequiredService<EventHelper>(),
            queryService: BatchServiceLocator.GetRequiredService<IFileTransferSvcQueryServiceProvider>(),
            service: BatchServiceLocator.GetRequiredService<IFileTransferSvcServiceProvider>())
    {
        ftsProcessId = consumerIdentifier;
    }

    public FTSConsumer(
        IFileTransferOperations fileTransferOperations,
        IDataAccessFactory dataAccessFactory,
        IAppSettings appSettings,
        TenantContext tenantContext,
        EventHelper eventHelper,
        IFileTransferSvcQueryServiceProvider queryService,
        IFileTransferSvcServiceProvider service)
    {
        fileTrasferOperations = fileTransferOperations;
        this.dataAccessFactory = dataAccessFactory;
        this.appSettings = appSettings;
        this.tenantContext = tenantContext;
        this.eventHelper = eventHelper;
        fileTransferQueryProvider = queryService;
        fileTransferProvider = service;

        destmodel = new DestinationModel();
        deleteSourceFile = true;
    }

    public static void Log(string message)
    {
        logFile.Add(new LogMessage
        {
            Code = LogCode.Info,
            Message = message,
        });
    }

    public virtual SourceLocationModel? GetSourceLocation(FileToDeliverModel fileDeliverModel)
    {
        IDataAccess<SourceLocationModel>? configMetaDataDataAccess = dataAccessFactory.CreateGetConfigMetaDataDataAccess(fileDeliverModel.FileId);
        return configMetaDataDataAccess != null ? configMetaDataDataAccess.Execute() : null;
    }

    public bool ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue)
    {
        return ProcessDeliveryQueue(filesToDeliverQueue, tenantContext.TenantId);
    }

    // F03-020, F03-029, F03-030
    public bool ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId)
    {
        TenantId = tenantId;

        // legacy called ApplicationConfigurationManager.LoadApplicationConfiguration(tenantId, new CacheManager());
        // gap: tenant-scoped configuration loading is handled by the worker host.

        while (!filesToDeliverQueue.IsCompleted)
        {
            FileToDeliverModel? fileDeliveryData = null;
            try
            {
                deleteSourceFile = true;
                if (filesToDeliverQueue.Count > 0)
                {
                    logFile.Clear();
                    fileDeliveryData = filesToDeliverQueue.Take();
                    DeliverFile(fileDeliveryData);
                }
                else
                {
                    break;
                }
            }
            catch (DuplicateFileNameException ex)
            {
                Log(LogCode.ExceptionEncountered, ex.Message);
                // F03-028: duplicate deliveries are rename-completed when enabled; otherwise they are marked as errors.
                if (Convert.ToBoolean(appSettings[AppSettingsConstants.ActivateDuplicateRenameLogic]))
                {
                    Log(LogCode.DuplicateFileFound, "Duplicate file found at destination. The source file was renamed with " + appSettings[AppSettingsConstants.DuplicateFileExtension] + " extension");
                    Log(LogCode.Info, "To requeue this file clear up the problem at destination and then rename the source file back.");
                    fileTrasferOperations.RenameSourceFile(fileDeliveryData!.SourceLocation, fileDeliveryData.FileName, appSettings[AppSettingsConstants.DuplicateFileExtension]!);
                    UpdateFileWithExceptionAndStatus(fileDeliveryData!, ex, DataAccessConstants.FileStatusConstant.Completed);
                    SendNotificationOfError(fileDeliveryData.SourceLocation, fileDeliveryData, "Duplicate", destmodel);
                }
                else
                {
                    UpdateFileWithExceptionAndStatus(fileDeliveryData!, ex, DataAccessConstants.FileStatusConstant.Error);
                    SendNotificationOfError(fileDeliveryData!.SourceLocation, fileDeliveryData!, "Error", destmodel);
                }
            }
            catch (Exception ex)
            {
                Log(LogCode.ExceptionEncountered, ex.Message);
                UpdateFileWithExceptionAndStatus(fileDeliveryData!, ex, DataAccessConstants.FileStatusConstant.Error);
                SendNotificationOfError(fileDeliveryData!.SourceLocation, fileDeliveryData!, "Error", destmodel);
            }
            finally
            {
                foreach (var logDetail in logFile)
                {
                    LoggerManager.Logger.LogInformational(string.Concat(ftsProcessId.ToString(), ": ", logDetail.Code, ": ", logDetail.Message));
                }
            }
        }

        return true;
    }

    private void DeliverFile(FileToDeliverModel fileDeliverModel)
    {
        DateTime beginTime = DateTime.Now;
        SetFileStatus(fileDeliverModel);
        Log(LogCode.GetListOfDestinations, string.Concat("fileId: ", fileDeliverModel.FileId.ToString(), " sakFileTrack: ", fileDeliverModel.SakFileTrack.ToString()));
        SourceLocationModel? sourceLocation = GetSourceLocation(fileDeliverModel);

        if (sourceLocation != null && sourceLocation.Destinations != null)
        {
            string uploadedSFTPUserName = GetSFTPUserName(fileDeliverModel);
            string fullyQualifiedWorkingFile = DownloadWorkingFile(fileTrasferOperations, fileDeliverModel.SourceLocation, fileDeliverModel.FileName, fileDeliverModel.FileId != Guid.Empty);
            string crc32 = fileTrasferOperations.GetCrc32(fullyQualifiedWorkingFile);
            SetFileDestinations(sourceLocation);

            foreach (DestinationModel destination in sourceLocation.Destinations)
            {
                Stopwatch watcher = Stopwatch.StartNew();

                TransferFile(
                    fileDeliverModel,
                    uploadedSFTPUserName,
                    fullyQualifiedWorkingFile,
                    crc32,
                    sourceLocation,
                    destination,
                    watcher);

                eventHelper.PublishFileTransferredEvent(fileDeliverModel, TenantId, watcher.Elapsed);
            }

            CleanupTransferDirectories(fileDeliverModel, fullyQualifiedWorkingFile, sourceLocation);
        }

        Log(LogCode.Finished, (DateTime.Now - beginTime).ToString());
    }

    private string GetSFTPUserName(FileToDeliverModel fileDeliverModel)
    {
        Log(LogCode.Info, " Getting the Uploaded User Name from the File using Move IT for fileName" + fileDeliverModel.SourceLocation.FtpServerDetail.FtpServerIp + " " + fileDeliverModel.SourceLocation.SourceAddress + " " + fileDeliverModel.FileName + " " + DateTime.Now.ToString());
        string uploadedSFTPUserName = fileDeliverModel.SourceLocation.FtpServerDetail.FtpUser;
        Log(LogCode.Info, "SFTP User name is " + (string.IsNullOrEmpty(uploadedSFTPUserName) ? "empty" : uploadedSFTPUserName) + " for file " + fileDeliverModel.FileName + DateTime.Now.ToString());

        return uploadedSFTPUserName;
    }

    private void TransferFile(
        FileToDeliverModel fileDeliverModel,
        string uploadedSFTPUserName,
        string fullyQualifiedWorkingFile,
        string crc32,
        SourceLocationModel sourceLocation,
        DestinationModel destination,
        Stopwatch watcher)
    {
        string workingFileToXfer = fullyQualifiedWorkingFile;

        if (destination.IsCompressAtDestination)
        {
            workingFileToXfer = fileTrasferOperations.CreateZipFile(fullyQualifiedWorkingFile);
            fullyQualifiedWorkingZipFile = workingFileToXfer;
        }

        destmodel = destination;

        fullyQualifiedDestinationFileName = fileTrasferOperations.GetDestinationFileName(workingFileToXfer, destination, fileDeliverModel.FileId, crc32, uploadedSFTPUserName);

        if (Encrypt.Equals(destination.EncryptionCode, StringComparison.OrdinalIgnoreCase))
        {
            int result;
            EncryptionHelper.EncryptPgpFile(workingFileToXfer, workingFileToXfer, destination.EncryptionPublicKey, true, true, out result);
            if (result == 1)
            {
                Log(LogCode.Info, " File encrypted successfully for  configId: " + fileDeliverModel.SourceLocation.ConfigurationId.ToString() + DateTime.Now.ToString());
                fullyQualifiedDestinationFileName = fullyQualifiedDestinationFileName + PgpExtention;
            }
            else
            {
                Log(LogCode.Info, "Encryption failed for configId: " + fileDeliverModel.SourceLocation.ConfigurationId.ToString() + DateTime.Now.ToString());
            }
        }
        else if (Decrypt.Equals(destination.EncryptionCode, StringComparison.OrdinalIgnoreCase))
        {
            outputFileName = workingFileToXfer.Replace(PgpExtention, string.Empty);
            decryptedPgpData = EncryptionHelper.DecryptPgpData(workingFileToXfer, outputFileName, destination.EncryptionPrivateKey, destination.PrivateKeyPassword);

            if (!string.IsNullOrEmpty(decryptedPgpData.Trim()))
            {
                Log(LogCode.Info, "Decryption failed for configId: " + fileDeliverModel.SourceLocation.ConfigurationId.ToString() + DateTime.Now.ToString());
            }
            else
            {
                workingFileToXfer = outputFileName;
                fullyQualifiedDestinationFileName = fullyQualifiedDestinationFileName.Replace(PgpExtention, string.Empty);
                Log(LogCode.Info, " File decrypted successfully for  configId: " + fileDeliverModel.SourceLocation.ConfigurationId.ToString() + DateTime.Now.ToString());
            }
        }

        Log(LogCode.UploadToDestination, destination.DestinationProtocolCode + ":" + fullyQualifiedDestinationFileName + " destinationId: " + destination.DestinationId.ToString());

        SendDeliveryNotification(fileDeliverModel, sourceLocation, destination, watcher, workingFileToXfer);
    }

    // F03-030
    private void SendDeliveryNotification(FileToDeliverModel fileDeliverModel, SourceLocationModel sourceLocation, DestinationModel dest, Stopwatch stopwatch, string workingFileToXfer)
    {
        string? uploadedFileName = null;
        try
        {
            uploadedFileName = fileTrasferOperations.UploadFile(workingFileToXfer, fullyQualifiedDestinationFileName, dest);

            if ((!string.IsNullOrEmpty(uploadedFileName)) && (!dest.IsFullyUploaded))
            {
                eventId = CreateFileTrackingEventRecord(fileDeliverModel.FileId, destmodel.DestinationId, uploadedFileName, sourceLocation.Destinations);
            }

            try
            {
                if ((!string.IsNullOrEmpty(uploadedFileName)) && (!dest.IsFullyUploaded))
                {
                    sourceLocation.SendNotificationsIndicator = fileDeliverModel.SourceLocation.SendNotificationsIndicator;
                    dest.IsFullyUploaded = true;
                    SendNotificationDestinationOfDelivery(sourceLocation, fileDeliverModel, dest);
                    fileDeliverModel.SourceLocation.Destinations = sourceLocation.Destinations;
                    fileDeliverModel = HandleIntegrationFiles(fileDeliverModel);
                }
                else if (!dest.IsFullyUploaded)
                {
                    deleteSourceFile = false;
                    Log(LogCode.ExceptionEncountered, " Unable to upload for: " + dest.DestinationAddress.ToString());
                    SendNotificationOfError(fileDeliverModel.SourceLocation, fileDeliverModel, "Error", dest);
                }

                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                deleteSourceFile = false;
                LoggerManager.Logger.LogError(" Unable to send Notification for: " + dest.DestinationAddress.ToString() + "  Error Message: " + ex.Message);
            }
        }
        catch (DuplicateFileNameException)
        {
            // F03-028/F03-029: duplicates are handled by the outer ProcessDeliveryQueue exception policy.
            throw;
        }
        catch (Exception ex)
        {
            deleteSourceFile = false;
            Log(LogCode.ExceptionEncountered, ex.Message);
            UpdateFileWithExceptionAndStatus(fileDeliverModel, ex, DataAccessConstants.FileStatusConstant.Error);
            SendCreateFileTrackingEventRecordNotification(fileDeliverModel.SourceLocation, fileDeliverModel, "Error", dest);
        }
    }

    private void SetFileDestinations(SourceLocationModel sourceLocation)
    {
        try
        {
            List<DestinationModel> encrptDestinations = new();

            foreach (DestinationModel dest in sourceLocation.Destinations)
            {
                if (Encrypt.Equals(dest.EncryptionCode, StringComparison.OrdinalIgnoreCase))
                {
                    encrptDestinations.Add(dest);
                    sourceLocation.Destinations.Remove(dest);
                }
            }

            if (encrptDestinations.Any())
            {
                sourceLocation.Destinations.AddRange(encrptDestinations);
            }
        }
        catch (Exception ex)
        {
            Log(LogCode.Info, ex.ToString());
        }
    }

    // F03-020
    private void SetFileStatus(FileToDeliverModel fileDeliverModel)
    {
        if (fileDeliverModel.FileId == Guid.Empty)
        {
            Log(LogCode.BeginFile, string.Concat("configId: ", fileDeliverModel.SourceLocation.ConfigurationId.ToString(), " file: ", fileDeliverModel.FileName, " ", DateTime.Now.ToString()));

            FileManagementRequestModel requestModel = new()
            {
                FileName = fileDeliverModel.FileName,
                FileID = fileDeliverModel.FileId,
                SourceLocationId = fileDeliverModel.SourceLocation.SourceLocationId,
                OperatorId = BatchJobConstants.OperatorId,
                FileSize = fileDeliverModel.FileSize,
                GroupId = fileDeliverModel.GroupId,
            };

            FileManagementResponseModel fileTrackingKeys = dataAccessFactory.CreateFileManagementDataAccess(requestModel).Execute();
            fileDeliverModel.FileId = fileTrackingKeys.FileId;
        }
        else
        {
            Log(LogCode.BeginFileRestart, string.Concat("configId: ", fileDeliverModel.SourceLocation.ConfigurationId.ToString(), " file: ", fileDeliverModel.FileName, " sakFileTrack: ", fileDeliverModel.SakFileTrack.ToString(), " ", DateTime.Now.ToString()));
        }
    }

    // F03-020
    private void CleanupTransferDirectories(FileToDeliverModel fileDeliverModel, string fullyQualifiedWorkingFile, SourceLocationModel sourceLocation)
    {
        DeleteSourceFile(fileDeliverModel, sourceLocation);
        UpdateFileTrackingRecord(fileDeliverModel.FileId, DataAccessConstants.FileStatusConstant.Completed);
        SendDeliveryNotification(fileDeliverModel, sourceLocation);
        DeleteTemporalFiles(fullyQualifiedWorkingFile);

        if (string.IsNullOrEmpty(decryptedPgpData) && File.Exists(outputFileName))
        {
            Log(LogCode.DeleteSourceFile, "Encryption/Decryption working copy");
            File.Delete(outputFileName);
        }
    }

    private void DeleteTemporalFiles(string fullyQualifiedWorkingFile)
    {
        Log(LogCode.CleanupWorkDirectory);
        File.Delete(fullyQualifiedWorkingFile);

        if (fullyQualifiedWorkingZipFile != null)
        {
            File.Delete(fullyQualifiedWorkingZipFile);
        }

        if (File.Exists(string.Format("{0}.zip", fullyQualifiedWorkingFile)))
        {
            File.Delete(string.Format("{0}.zip", fullyQualifiedWorkingFile));
        }
    }

    private void SendDeliveryNotification(FileToDeliverModel fileDeliverModel, SourceLocationModel sourceLocation)
    {
        try
        {
            sourceLocation.SendNotificationsIndicator = fileDeliverModel.SourceLocation.SendNotificationsIndicator;
            SendNotificationOfDelivery(sourceLocation, fileDeliverModel);
        }
        catch (Exception ex)
        {
            LoggerManager.Logger.LogError(" Unable to send Notification for: " + fileDeliverModel.SourceLocation.ConfigurationId.ToString() + "  Error Message: " + ex.Message);
        }
    }

    // F03-030
    private void DeleteSourceFile(FileToDeliverModel fileDeliverModel, SourceLocationModel sourceLocation)
    {
        if (deleteSourceFile && sourceLocation != null && sourceLocation.Destinations.All(x => x.IsFullyUploaded))
        {
            Log(LogCode.DeleteSourceFile);
            string sourceFullFilePath = (sourceLocation.SourceProtocolCode == ProtocolCodeConstant.UNC ? sourceLocation.SourceAddress + "\\" : sourceLocation.SourceAddress + "/") + Path.GetFileName(fileDeliverModel.FileName);
            fileTrasferOperations.DeleteFile(fileDeliverModel.SourceLocation, sourceFullFilePath);
        }
    }

    private FileToDeliverModel HandleIntegrationFiles(FileToDeliverModel fileDeliverModel)
    {
        FtsIntegrationFileModel? integrationFile = fileTransferQueryProvider.GetIntegrationFile(BuildIntegrationFileQuery(fileDeliverModel));

        if (integrationFile != null)
        {
            fileTransferProvider.UpdateIntegrationFile(BuildIntegrationFileCommand(fileDeliverModel, integrationFile));
            fileDeliverModel.GroupId = integrationFile.GroupId ?? Guid.Empty;
        }

        return fileDeliverModel;
    }

    private GetIntegrationFileQuery BuildIntegrationFileQuery(FileToDeliverModel fileDeliverModel)
    {
        return new GetIntegrationFileQuery()
        {
            Requestor = new RequestorModel()
            {
                TenantId = TenantId,
            },
            Where = new IntegrationFileQueryParms
            {
                FileName = fileDeliverModel.FileName,
            },
        };
    }

    private UpdateIntegrationFileCommand BuildIntegrationFileCommand(FileToDeliverModel fileDeliverModel, FtsIntegrationFileModel integrationFile)
    {
        return new UpdateIntegrationFileCommand()
        {
            Requestor = new RequestorModel()
            {
                IdentifierId = BatchJobConstants.OperatorId,
                TenantId = TenantId,
            },
            ConfigId = (int?)fileDeliverModel.SourceLocation.ConfigurationId,
            FtsIntegrationFileId = integrationFile.FtsIntegrationFileId,
            IsValid = true,
        };
    }

    // F03-020
    private void UpdateFileWithExceptionAndStatus(FileToDeliverModel fileDeliverModel, Exception exceptionDetail, string status)
    {
        string errorText = string.Empty;

        foreach (LogMessage logDetail in logFile)
        {
            errorText += logDetail.Code + ": " + logDetail.Message + "\r\n";
        }

        if (fileDeliverModel.FileId != Guid.Empty)
        {
            dataAccessFactory.CreateUpdateFileManagementStatusDataAccess(fileDeliverModel.FileId, status).Execute();

            ErrorLogDetailModel errorLog = new()
            {
                FileId = fileDeliverModel.FileId,
                ErrorText = errorText,
                DestinationId = destmodel.DestinationId,
                StackTrace = exceptionDetail.StackTrace ?? string.Empty,
                ErrorType = "DELIVERY",
                MethodName = BatchJobConstants.ProcessDeliveryQueue,
                AssemblyName = string.Empty,
                ClassName = string.Empty,
                MachineName = exceptionDetail.Source ?? string.Empty,
                OperatorId = BatchJobConstants.OperatorId,
            };

            Log(LogCode.CreateEventRecord, "destinationId: " + destmodel.DestinationId.ToString());
            dataAccessFactory.CreateFtsFileExceptionEventDataAccess(fileDeliverModel.FileId, destmodel.DestinationId, fullyQualifiedDestinationFileName, BatchJobConstants.OperatorId, errorLog).Execute();
        }
    }

    private Guid CreateFileTrackingEventRecord(Guid fileId, Guid destinationId, string destinationFileName, List<DestinationModel> destinations)
    {
        Log(LogCode.CreateEventRecord, "destinationId: " + destinationId.ToString());
        return dataAccessFactory.CreateFtsFileDeliveryDataAccess(fileId, destinationId, destinationFileName, BatchJobConstants.OperatorId, destinations).Execute();
    }

    private void UpdateFileTrackingRecord(Guid fileId, string status)
    {
        Log(LogCode.UpdateFileTrackingRecord);
        dataAccessFactory.CreateUpdateFileManagementStatusDataAccess(fileId, status).Execute();
    }

    private void SendNotificationOfDelivery(SourceLocationModel source, FileToDeliverModel fileDeliverModel)
    {
        if (source.SendNotificationsIndicator && !string.IsNullOrEmpty(source.OwnerEmail))
        {
            FileInfo fileInfo = new(fileDeliverModel.FileName);

            string subject = appSettings[AppSettingsConstants.EnvironmentName] + ": File Delivery Success: " + fileInfo.Name;

            string body = FormatEmailBody(source, fileInfo, fileDeliverModel);

            fileTrasferOperations.SendNotification(source, subject, body, false, TenantId);

            fileTrasferOperations.PostEvent("File Delivery Success " + source.ConfigurationId.ToString(), BatchJobConstants.EDI, body, false, BatchJobConstants.OperatorId);
        }
    }

    private void SendCreateFileTrackingEventRecordNotification(SourceLocationModel source, FileToDeliverModel fileDeliverModel, string errorType, DestinationModel destination)
    {
        if (fileDeliverModel != null && destination != null && source != null && !string.IsNullOrEmpty(fileDeliverModel.FileName) && source.SendNotificationsIndicator && !string.IsNullOrEmpty(destination.DestinationAddress))
        {
            FileInfo fileInfo = new(fileDeliverModel.FileName);

            string subject = string.Concat(appSettings[AppSettingsConstants.EnvironmentName], ": File Delivery ", errorType, ": ", fileInfo.Name);
            string body = string.Concat(fileInfo.Name, " could not be create file tracking event :", destination.DestinationAddress, "Destination Id :", destination.DestinationId, "Detailed logging information follows:\n\n", BuildLogMessages());

            fileTrasferOperations.SendNotification(source, subject, body, true, TenantId);

            fileTrasferOperations.PostEvent("File Delivery " + errorType + source.ConfigurationId.ToString(), BatchJobConstants.EDI, body, false, BatchJobConstants.OperatorId);
        }
    }

    private void SendNotificationOfError(SourceLocationModel source, FileToDeliverModel fileDeliverModel, string errorType, DestinationModel destination)
    {
        if (source.SendNotificationsIndicator && !string.IsNullOrEmpty(destination.DestinationAddress))
        {
            FileInfo fileInfo = new(fileDeliverModel.FileName);

            string subject = string.Concat(appSettings[AppSettingsConstants.EnvironmentName], ": File Delivery ", errorType, ": ", fileInfo.Name);
            string body = string.Concat(fileInfo.Name, " could not be delivered to location with destination address :", destination.DestinationAddress, "Destination Id :", destination.DestinationId, "Detailed logging information follows:\n\n", BuildLogMessages());

            fileTrasferOperations.SendNotification(source, subject, body, true, TenantId);

            fileTrasferOperations.PostEvent("File Delivery " + errorType + source.ConfigurationId.ToString(), BatchJobConstants.EDI, body, false, BatchJobConstants.OperatorId);
        }
    }

    private void SendNotificationDestinationOfDelivery(SourceLocationModel source, FileToDeliverModel fileDeliverModel, DestinationModel destination)
    {
        if (source.SendNotificationsIndicator && !string.IsNullOrEmpty(destination.EmailId))
        {
            FileInfo fileInfo = new(fileDeliverModel.FileName);

            string subject = appSettings[AppSettingsConstants.EnvironmentName] + ": File Delivery Success: " + fileInfo.Name;
            string body = FormatEmailBody(source, fileInfo, fileDeliverModel, destination);

            SourceLocationModel destinationEmailModel = new()
            {
                OwnerEmail = destination.EmailId,
                OwnerName = destination.ContactName,
            };

            fileTrasferOperations.SendNotification(destinationEmailModel, subject, body, false, TenantId);

            fileTrasferOperations.PostEvent("File Delivery Success " + source.ConfigurationId.ToString(), BatchJobConstants.EDI, body, false, BatchJobConstants.OperatorId);
        }
    }

    private string FormatEmailBody(SourceLocationModel source, FileInfo fileInfo, FileToDeliverModel fileDeliverModel, DestinationModel? destination = null)
    {
        string body = fileInfo.Name + @"<html> was delivered to the following locations:<br><br>";

        body += " SourceId: " + source.ConfigurationId.ToString() + " " + source.SourceProtocolCode + ":" + source.SourceAddress + "<br><br>";
        body += GetDestinationsForEmailBody(source, destination) + "<br>";

        body += " GroupId: " + fileDeliverModel.GroupId.ToString() + "<br>";

        if (destination != null && destination.FTEMetadataId != null && destination.FTEMetadataId != Guid.Empty)
        {
            string link = BuildSTPUrl(fileDeliverModel.GroupId.ToString());
            body += "<br>Tracking Process Link: <a href='" + link + "'>Track History Process</a>";
        }

        body += "<br><br>Detailed log info follows<br>-------------------------<br>" + BuildLogMessages();

        return body += "</html>";
    }

    private string BuildSTPUrl(string groupId)
    {
        return !string.IsNullOrEmpty(appSettings[AppSettingsConstants.HostName])
            ? string.Format("{0}{1}{2}", appSettings[AppSettingsConstants.HostName], AppSettingsKey.SPTURLPath, groupId)
            : string.Empty;
    }

    private string GetDestinationsForEmailBody(SourceLocationModel source, DestinationModel? destination)
    {
        if (destination == null)
        {
            string destinations = string.Empty;

            foreach (DestinationModel d in source.Destinations)
            {
                destinations += " DestinationId: " + d.DestinationId.ToString() + " " + d.DestinationProtocolCode + ":" + d.DestinationAddress + "/" + d.DestinationFileName + "\n";
            }

            return destinations;
        }

        return " DestinationId: " + destination.DestinationId.ToString() + " " + destination.DestinationProtocolCode + ":" + destination.DestinationAddress + "/" + destination.DestinationFileName + "\n";
    }

    private string DownloadWorkingFile(IFileTransferOperations fileTransferOperations, SourceLocationModel source, string fileName, bool reuseWorkFile)
    {
        Log(LogCode.DownloadWorkingFile);

        string fullyQualifiedWorkingFile = string.Concat(appSettings[AppSettingsConstants.WorkingDirectory], "\\", source.ConfigurationId, "\\", new FileInfo(fileName).Name);
        try
        {
            DestinationModel destination = new();
            destination.DestinationProtocolCode = "UNC";
            destination.DestinationFileName = fullyQualifiedWorkingFile;
            destination.DestinationAddress = appSettings[AppSettingsConstants.WorkingDirectory]!;

            Directory.CreateDirectory(new FileInfo(fullyQualifiedWorkingFile).DirectoryName!);

            if (reuseWorkFile && File.Exists(fullyQualifiedWorkingFile))
            {
                return fullyQualifiedWorkingFile;
            }

            fileTransferOperations.DownloadFile(source, fileName, destination, TenantId);
        }
        catch (Exception ex)
        {
            Log(LogCode.Info, ex.Message);
        }

        return fullyQualifiedWorkingFile;
    }

    private void Log(LogCode code)
    {
        logFile.Add(new LogMessage
        {
            Code = code,
            Message = string.Empty,
        });
    }

    private void Log(LogCode code, string message)
    {
        logFile.Add(new LogMessage
        {
            Code = code,
            Message = message,
        });
    }

    private string BuildLogMessages()
    {
        string prefix = string.Empty;

        StringBuilder logMessages = new();

        foreach (LogMessage detail in logFile)
        {
            logMessages.Append(prefix).Append(detail.Code).Append(": ").Append(detail.Message).Append("\r\n");
        }

        return logMessages.ToString();
    }
}
