using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

using Fxfer.TransferWorker.Application.Constants;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Application.Helpers;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Contracts.NotificationService;
using Fxfer.TransferWorker.Domain.Batch;
using Fxfer.TransferWorker.Domain.Constants;
using Fxfer.TransferWorker.Domain.Exceptions;
using Fxfer.TransferWorker.Worker.Configuration;

namespace Fxfer.TransferWorker.Application.Batch;

public class FTSConsumer : IFTSConsumer
{
    private const string Decrypt = "DECRYPT";
    private const string Encrypt = "ENCRYPT";
    private const string PgpExtention = ".pgp";

    private static readonly List<LogMessage> logFile = new();

    private readonly IFileTransferOperations fileTrasferOperations;
    private readonly IDataAccessFactory dataAccessFactory;
    private readonly INotificationService notificationService;
    private readonly ITenantAppSettingsManager configurationManager;
    private readonly IRequestContextAccessor requestContextAccessor;
    private readonly int ftsProcessId;
    private readonly EventHelper eventHelper;

    private bool deleteSourceFile;
    private Guid eventId;
    private DestinationModel destmodel = new();
    private string fullyQualifiedDestinationFileName = string.Empty;
    private string? fullyQualifiedWorkingZipFile;
    private string decryptedPgpData = string.Empty;
    private string outputFileName = string.Empty;

    public FTSConsumer(
        int consumerIdentifier,
        IFileTransferOperations fileTransferOperations,
        IDataAccessFactory dataAccessFactory,
        INotificationService notificationService,
        ITenantAppSettingsManager configurationManager,
        IRequestContextAccessor requestContextAccessor)
    {
        this.ftsProcessId = consumerIdentifier;
        this.fileTrasferOperations = fileTransferOperations;
        this.dataAccessFactory = dataAccessFactory;
        this.notificationService = notificationService;
        this.configurationManager = configurationManager;
        this.requestContextAccessor = requestContextAccessor;
        this.eventHelper = new EventHelper();
    }

    public string TenantId { get; private set; } = string.Empty;

    public static void Log(string message)
    {
        logFile.Add(new LogMessage
        {
            Code = LogCode.Info,
            Message = message
        });
    }

    public bool ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId)
    {
        var effectiveTenantId = !string.IsNullOrWhiteSpace(tenantId) ? tenantId : this.requestContextAccessor.TenantId;
        if (!string.IsNullOrWhiteSpace(effectiveTenantId))
        {
            this.TenantId = effectiveTenantId;
            this.configurationManager.LoadApplicationConfiguration(effectiveTenantId);
        }

        while (!filesToDeliverQueue.IsCompleted)
        {
            FileToDeliverModel? fileDeliveryData = null;
            try
            {
                this.deleteSourceFile = true;
                if (filesToDeliverQueue.Count > 0)
                {
                    logFile.Clear();
                    fileDeliveryData = filesToDeliverQueue.Take();
                    this.DeliverFile(fileDeliveryData);
                }
                else
                {
                    break;
                }
            }
            catch (DuplicateFileNameException ex)
            {
                Log(LogCode.ExceptionEncountered, ex.Message);
                if (Convert.ToBoolean(this.configurationManager.AppSettings?[AppSettingsConstants.ActivateDuplicateRenameLogic]))
                {
                    Log(
                        LogCode.DuplicateFileFound,
                        "Duplicate file found at destination. The source file was renamed with "
                        + this.configurationManager.AppSettings?[AppSettingsConstants.DuplicateFileExtension] + " extension");
                    Log(LogCode.Info, "To requeue this file clear up the problem at destination and then rename the source file back.");
                    string duplicateFileExtension = Convert.ToString(this.configurationManager.AppSettings?[AppSettingsConstants.DuplicateFileExtension]);
                    string sourceFullFilePath =
                        (fileDeliveryData!.SourceLocation.SourceProtocolCode == ProtocolCodeConstant.UNC ? fileDeliveryData.SourceLocation.SourceAddress + "\\" :
                            fileDeliveryData.SourceLocation.SourceAddress + "/") + Path.GetFileName(fileDeliveryData.FileName);
                    _ = this.fileTrasferOperations.RenameSourceFile(
                        fileDeliveryData.SourceLocation,
                        sourceFullFilePath,
                        duplicateFileExtension);
                    this.UpdateFileWithExceptionAndStatus(fileDeliveryData, ex, FileStatusConstant.Completed);
                    this.SendNotificationOfError(fileDeliveryData.SourceLocation, fileDeliveryData, "Duplicate", this.destmodel);
                }
                else
                {
                    this.UpdateFileWithExceptionAndStatus(fileDeliveryData!, ex, FileStatusConstant.Error);
                    this.SendNotificationOfError(fileDeliveryData!.SourceLocation, fileDeliveryData, "Error", this.destmodel);
                }
            }
            catch (Exception ex)
            {
                Log(LogCode.ExceptionEncountered, ex.Message);
                this.UpdateFileWithExceptionAndStatus(fileDeliveryData!, ex, FileStatusConstant.Error);
                this.SendNotificationOfError(fileDeliveryData!.SourceLocation, fileDeliveryData!, "Error", this.destmodel);
            }
            finally
            {
                Console.WriteLine(string.Empty);
                foreach (var logDetail in logFile)
                {
                    Console.WriteLine(string.Concat(this.ftsProcessId.ToString(), ": ", logDetail.Code, ": ", logDetail.Message));
                }
            }
        }

        return true;
    }

    private FileToDeliverModel HandleIntegrationFiles(FileToDeliverModel fileDeliverModel)
    {
        var query = this.BuildIntegrationFileQuery(fileDeliverModel);
        var integrationFile = ExecuteSync(this.dataAccessFactory.CreateGetIntegrationFileDataAccess(query.Where?.FileName ?? string.Empty));

        if (integrationFile != null)
        {
            _ = ExecuteSync(this.dataAccessFactory.CreateUpdateIntegrationFileDataAccess(this.BuildIntegrationFileCommand(fileDeliverModel, integrationFile)));
            fileDeliverModel.GroupId = integrationFile.GroupId ?? Guid.Empty;
        }

        return fileDeliverModel;
    }

    private UpdateIntegrationFileCommand BuildIntegrationFileCommand(FileToDeliverModel fileDeliverModel, FtsIntegrationFileModel integrationFile)
    {
        return new UpdateIntegrationFileCommand()
        {
            Requestor = new RequestorModel()
            {
                IdentifierId = BatchJobConstants.OperatorId,
                TenantId = this.TenantId
            },
            ConfigId = (int?)fileDeliverModel.SourceLocation.ConfigurationId,
            FtsIntegrationFileId = integrationFile.FtsIntegrationFileId,
            IsValid = true
        };
    }

    private GetIntegrationFileQuery BuildIntegrationFileQuery(FileToDeliverModel fileDeliverModel)
    {
        return new GetIntegrationFileQuery()
        {
            Requestor = new RequestorModel()
            {
                TenantId = this.TenantId
            },
            Where = new IntegrationFileQueryParms
            {
                FileName = fileDeliverModel.FileName
            }
        };
    }

    private void DeliverFile(FileToDeliverModel fileDeliverModel)
    {
        DateTime beginTime = DateTime.Now;
        this.SetFileStatus(fileDeliverModel);
        this.Log(LogCode.GetListOfDestinations, string.Concat("fileId: ", fileDeliverModel.FileId.ToString(), " sakFileTrack: ", fileDeliverModel.SakFileTrack?.ToString()));
        SourceLocationModel? sourceLocation = this.GetSourceLocation(fileDeliverModel);

        if (sourceLocation != null && sourceLocation.Destinations != null)
        {
            string uploadedSFTPUserName = this.GetSFTPUserName(fileDeliverModel);
            string fullyQualifiedWorkingFile = this.DownloadWorkingFile(this.fileTrasferOperations, fileDeliverModel.SourceLocation, fileDeliverModel.FileName, fileDeliverModel.FileId != Guid.Empty);
            string crc32 = this.fileTrasferOperations.GetCrc32(fullyQualifiedWorkingFile);
            this.SetFileDestinations(sourceLocation);

            foreach (DestinationModel destination in sourceLocation.Destinations)
            {
                Stopwatch watcher = Stopwatch.StartNew();

                this.TransferFile(
                        fileDeliverModel,
                        uploadedSFTPUserName,
                        fullyQualifiedWorkingFile,
                        crc32,
                        sourceLocation,
                        destination,
                        watcher);

                this.eventHelper.PublishFileTransferredEvent(fileDeliverModel, this.TenantId, watcher.Elapsed);
            }

            this.CleanupTransferDirectories(fileDeliverModel, fullyQualifiedWorkingFile, sourceLocation);
        }

        this.Log(LogCode.Finished, (DateTime.Now - beginTime).ToString());
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
            workingFileToXfer = this.fileTrasferOperations.CreateZipFile(fullyQualifiedWorkingFile);
        }

        this.destmodel = destination;

        this.fullyQualifiedDestinationFileName = this.fileTrasferOperations.GetDestinationFileName(workingFileToXfer, destination, fileDeliverModel.FileId, crc32, uploadedSFTPUserName);

        if (Encrypt.Equals(destination.EncryptionCode, StringComparison.OrdinalIgnoreCase))
        {
            int result;
            EncryptionHelper.EncryptPgpFile(workingFileToXfer, workingFileToXfer, destination.EncryptionPublicKey ?? string.Empty, true, true, out result);
            if (result == 1)
            {
                this.Log(LogCode.Info, " File encrypted successfully for  configId: " + fileDeliverModel.SourceLocation.ConfigurationId.ToString() + DateTime.Now.ToString());
                this.fullyQualifiedDestinationFileName = this.fullyQualifiedDestinationFileName + PgpExtention;
            }
            else
            {
                this.Log(LogCode.Info, "Encryption failed for configId: " + fileDeliverModel.SourceLocation.ConfigurationId.ToString() + DateTime.Now.ToString());
            }
        }
        else if (Decrypt.Equals(destination.EncryptionCode, StringComparison.OrdinalIgnoreCase))
        {
            this.outputFileName = workingFileToXfer.Replace(PgpExtention, string.Empty);
            this.decryptedPgpData = EncryptionHelper.DecryptPgpData(workingFileToXfer, outputFileName, destination.EncryptionPrivateKey ?? string.Empty, destination.PrivateKeyPassword ?? string.Empty);

            if (!string.IsNullOrEmpty(decryptedPgpData.Trim()))
            {
                this.Log(LogCode.Info, "Decryption failed for configId: " + fileDeliverModel.SourceLocation.ConfigurationId.ToString() + DateTime.Now.ToString());
            }
            else
            {
                workingFileToXfer = outputFileName;
                this.fullyQualifiedDestinationFileName = this.fullyQualifiedDestinationFileName.Replace(PgpExtention, string.Empty);
                this.Log(LogCode.Info, " File decrypted successfully for  configId: " + fileDeliverModel.SourceLocation.ConfigurationId.ToString() + DateTime.Now.ToString());
            }
        }

        this.Log(LogCode.UploadToDestination, destination.DestinationProtocolCode + ":" + this.fullyQualifiedDestinationFileName + " destinationId: " + destination.DestinationId.ToString());

        this.SendDeliveryNotification(fileDeliverModel, sourceLocation, destination, watcher, workingFileToXfer);
    }

    private void CleanupTransferDirectories(
                                    FileToDeliverModel fileDeliverModel,
                                    string fullyQualifiedWorkingFile,
                                    SourceLocationModel sourceLocation)
    {
        this.DeleteSourceFile(fileDeliverModel, sourceLocation);
        this.UpdateFileTrackingRecord(fileDeliverModel.FileId, FileStatusConstant.Completed);
        this.SendDeliveryNotification(fileDeliverModel, sourceLocation);
        this.DeleteTemporalFiles(fullyQualifiedWorkingFile);

        if (string.IsNullOrEmpty(this.decryptedPgpData) && File.Exists(this.outputFileName))
        {
            this.Log(LogCode.DeleteSourceFile, "Encryption/Decryption working copy");
            File.Delete(this.outputFileName);
        }
    }

    private void DeleteTemporalFiles(string fullyQualifiedWorkingFile)
    {
        this.Log(LogCode.CleanupWorkDirectory);
        File.Delete(fullyQualifiedWorkingFile);

        if (this.fullyQualifiedWorkingZipFile != null)
        {
            File.Delete(this.fullyQualifiedWorkingZipFile);
        }

        if (File.Exists(string.Format("{0}.zip", fullyQualifiedWorkingFile)))
        {
            File.Delete(string.Format("{0}.zip", fullyQualifiedWorkingFile));
        }
    }

    private void DeleteSourceFile(FileToDeliverModel fileDeliverModel, SourceLocationModel sourceLocation)
    {
        if (this.deleteSourceFile && sourceLocation != null && sourceLocation.Destinations.All(x => x.IsFullyUploaded))
        {
            this.Log(LogCode.DeleteSourceFile);
            string sourceFullFilePath = (sourceLocation.SourceProtocolCode == ProtocolCodeConstant.UNC ? sourceLocation.SourceAddress + "\\" : sourceLocation.SourceAddress + "/") + Path.GetFileName(fileDeliverModel.FileName);
            _ = this.fileTrasferOperations.DeleteFile(fileDeliverModel.SourceLocation, sourceFullFilePath);
        }
    }

    private void SetFileDestinations(SourceLocationModel sourceLocation)
    {
        try
        {
            List<DestinationModel> encrptDestinations = new List<DestinationModel>();

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
            this.Log(LogCode.Info, ex.ToString());
        }
    }

    private void SetFileStatus(FileToDeliverModel fileDeliverModel)
    {
        if (fileDeliverModel.FileId == Guid.Empty)
        {
            this.Log(LogCode.BeginFile, string.Concat("configId: ", fileDeliverModel.SourceLocation.ConfigurationId.ToString(), " file: ", fileDeliverModel.FileName, " ", DateTime.Now.ToString()));

            FileManagementRequestModel requestModel = new FileManagementRequestModel
            {
                FileName = fileDeliverModel.FileName,
                FileID = fileDeliverModel.FileId,
                SourceLocationId = fileDeliverModel.SourceLocation.SourceLocationId,
                OperatorId = BatchJobConstants.OperatorId,
                FileSize = fileDeliverModel.FileSize,
                GroupId = fileDeliverModel.GroupId
            };

            FileManagementResponseModel fileTrackingKeys = ExecuteSync(this.dataAccessFactory.CreateFileManagementDataAccess(requestModel));
            fileDeliverModel.FileId = fileTrackingKeys.FileId;
        }
        else
        {
            this.Log(LogCode.BeginFileRestart, string.Concat("configId: ", fileDeliverModel.SourceLocation.ConfigurationId.ToString(), " file: ", fileDeliverModel.FileName, " sakFileTrack: ", fileDeliverModel.SakFileTrack?.ToString(), " ", DateTime.Now.ToString()));
        }
    }

    private Guid CreateFileTrackingEventRecord(Guid fileId, Guid destinationId, string destinationFileName)
    {
        this.Log(LogCode.CreateEventRecord, "destinationId: " + destinationId.ToString());
        return ExecuteSync(this.dataAccessFactory.CreateFtsFileDeliveryDataAccess(fileId, destinationId, destinationFileName, BatchJobConstants.OperatorId));
    }

    private void UpdateFileTrackingRecord(Guid fileId, string status)
    {
        this.Log(LogCode.UpdateFileTrackingRecord);
        _ = ExecuteSync(this.dataAccessFactory.CreateUpdateFileManagementStatusDataAccess(fileId, status));
    }

    private void UpdateFileWithExceptionAndStatus(FileToDeliverModel fileDeliverModel, Exception exceptionDetail, string status)
    {
        string errorText = string.Empty;

        foreach (LogMessage logDetail in logFile)
        {
            errorText += logDetail.Code + ": " + logDetail.Message + "\r\n";
        }

        if (fileDeliverModel.FileId != Guid.Empty)
        {
            _ = ExecuteSync(this.dataAccessFactory.CreateUpdateFileManagementStatusDataAccess(fileDeliverModel.FileId, status));

            ErrorLogDetailModel errorLog = new ErrorLogDetailModel
            {
                FileId = fileDeliverModel.FileId,
                ErrorText = errorText,
                DestinationId = this.destmodel.DestinationId,
                StackTrace = exceptionDetail.StackTrace,
                ErrorType = "DELIVERY",
                MethodName = BatchJobConstants.ProcessDeliveryQueue,
                MachineName = exceptionDetail.Source,
                OperatorId = BatchJobConstants.OperatorId
            };

            this.Log(LogCode.CreateEventRecord, "destinationId: " + this.destmodel.DestinationId.ToString());
            _ = ExecuteSync(this.dataAccessFactory.CreateFtsFileExceptionEventDataAccess(fileDeliverModel.FileId, this.destmodel.DestinationId, this.fullyQualifiedDestinationFileName, BatchJobConstants.OperatorId, errorLog));
        }
    }

    private void SendDeliveryNotification(FileToDeliverModel fileDeliverModel, SourceLocationModel sourceLocation)
    {
        try
        {
            sourceLocation.SendNotificationsIndicator = fileDeliverModel.SourceLocation.SendNotificationsIndicator;
            this.SendNotificationOfDelivery(sourceLocation, fileDeliverModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine(" Unable to send Notification for: " + fileDeliverModel.SourceLocation.ConfigurationId.ToString() + "  Error Message: " + ex.Message);
        }
    }

    private void SendDeliveryNotification(FileToDeliverModel fileDeliverModel, SourceLocationModel sourceLocation, DestinationModel dest, Stopwatch stopwatch, string workingFileToXfer)
    {
        string? uploadedFileName = null;
        try
        {
            uploadedFileName = this.fileTrasferOperations.UploadFile(workingFileToXfer, this.fullyQualifiedDestinationFileName, dest);

            if ((!string.IsNullOrEmpty(uploadedFileName)) && (!dest.IsFullyUploaded))
            {
                this.eventId = this.CreateFileTrackingEventRecord(fileDeliverModel.FileId, this.destmodel.DestinationId, uploadedFileName);
            }

            try
            {
                if ((!string.IsNullOrEmpty(uploadedFileName)) && (!dest.IsFullyUploaded))
                {
                    sourceLocation.SendNotificationsIndicator = fileDeliverModel.SourceLocation.SendNotificationsIndicator;
                    dest.IsFullyUploaded = true;
                    this.SendNotificationDestinationOfDelivery(sourceLocation, fileDeliverModel, dest);
                    fileDeliverModel.SourceLocation.Destinations = sourceLocation.Destinations;
                    fileDeliverModel = this.HandleIntegrationFiles(fileDeliverModel);
                }
                else if (!dest.IsFullyUploaded)
                {
                    this.deleteSourceFile = false;
                    this.Log(LogCode.ExceptionEncountered, " Unable to upload for: " + dest.DestinationAddress);
                    this.SendNotificationOfError(fileDeliverModel.SourceLocation, fileDeliverModel, "Error", dest);
                }
                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                this.deleteSourceFile = false;
                Console.WriteLine(" Unable to send Notification for: " + dest.DestinationAddress + "  Error Message: " + ex.Message);
            }
        }
        catch (Exception ex)
        {
            this.deleteSourceFile = false;
            this.Log(LogCode.ExceptionEncountered, ex.Message);
            this.UpdateFileWithExceptionAndStatus(fileDeliverModel, ex, FileStatusConstant.Error);
            this.SendCreateFileTrackingEventRecordNotification(fileDeliverModel.SourceLocation, fileDeliverModel, "Error", dest);
        }
    }

    private void SendCreateFileTrackingEventRecordNotification(SourceLocationModel source, FileToDeliverModel fileDeliverModel, string errorType, DestinationModel destination)
    {
        if (fileDeliverModel != null && destination != null && source != null && !string.IsNullOrEmpty(fileDeliverModel.FileName) && source.SendNotificationsIndicator && !string.IsNullOrEmpty(destination.DestinationAddress))
        {
            FileInfo fileInfo = new FileInfo(fileDeliverModel.FileName);

            string subject = string.Concat(this.configurationManager.AppSettings?[AppSettingsConstants.EnvironmentName], ": File Delivery ", errorType, ": ", fileInfo.Name);
            string body = string.Concat(fileInfo.Name, " could not be create file tracking event :", destination.DestinationAddress, "Destination Id :", destination.DestinationId, "Detailed logging information follows:\n\n", this.BuildLogMessages());

            this.fileTrasferOperations.SendNotification(source, subject, body, true, this.TenantId);

            _ = this.notificationService.PostEvent("File Delivery " + errorType + source.ConfigurationId.ToString(), BatchJobConstants.EDI, body, false, DateTime.UtcNow, null, BatchJobConstants.OperatorId, this.TenantId);
        }
    }

    private void SendNotificationDestinationOfDelivery(SourceLocationModel source, FileToDeliverModel fileDeliverModel, DestinationModel destination)
    {
        if (source.SendNotificationsIndicator && !string.IsNullOrEmpty(destination.EmailId))
        {
            FileInfo fileInfo = new FileInfo(fileDeliverModel.FileName);

            string subject = this.configurationManager.AppSettings?[AppSettingsConstants.EnvironmentName] + ": File Delivery Success: " + fileInfo.Name;
            string body = this.FormatEmailBody(source, fileInfo, fileDeliverModel, destination);

            SourceLocationModel destinationEmailModel = new SourceLocationModel()
            {
                OwnerEmail = destination.EmailId,
                OwnerName = destination.ContactName
            };

            this.fileTrasferOperations.SendNotification(source, subject, body, false, this.TenantId);

            _ = this.notificationService.PostEvent("File Delivery Success " + source.ConfigurationId.ToString(), BatchJobConstants.EDI, body, false, DateTime.UtcNow, null, BatchJobConstants.OperatorId, this.TenantId);
        }
    }

    private void SendNotificationOfDelivery(SourceLocationModel source, FileToDeliverModel fileDeliverModel)
    {
        if (source.SendNotificationsIndicator && !string.IsNullOrEmpty(source.OwnerEmail))
        {
            FileInfo fileInfo = new FileInfo(fileDeliverModel.FileName);

            string subject = this.configurationManager.AppSettings?[AppSettingsConstants.EnvironmentName] + ": File Delivery Success: " + fileInfo.Name;

            string body = this.FormatEmailBody(source, fileInfo, fileDeliverModel);

            this.fileTrasferOperations.SendNotification(source, subject, body, false, this.TenantId);

            _ = this.notificationService.PostEvent("File Delivery Success " + source.ConfigurationId.ToString(), BatchJobConstants.EDI, body, false, DateTime.UtcNow, null, BatchJobConstants.OperatorId, this.TenantId);
        }
    }

    private void SendNotificationOfError(SourceLocationModel source, FileToDeliverModel fileDeliverModel, string errorType, DestinationModel destination)
    {
        if (source.SendNotificationsIndicator && !string.IsNullOrEmpty(destination.DestinationAddress))
        {
            FileInfo fileInfo = new FileInfo(fileDeliverModel.FileName);

            string subject = string.Concat(this.configurationManager.AppSettings?[AppSettingsConstants.EnvironmentName], ": File Delivery ", errorType, ": ", fileInfo.Name);
            string body = string.Concat(fileInfo.Name, " could not be delivered to location with destination address :", destination.DestinationAddress, "Destination Id :", destination.DestinationId, "Detailed logging information follows:\n\n", this.BuildLogMessages());

            this.fileTrasferOperations.SendNotification(source, subject, body, true, this.TenantId);

            _ = this.notificationService.PostEvent("File Delivery " + errorType + source.ConfigurationId.ToString(), BatchJobConstants.EDI, body, false, DateTime.UtcNow, null, BatchJobConstants.OperatorId, this.TenantId);
        }
    }

    private string FormatEmailBody(SourceLocationModel source, FileInfo fileInfo, FileToDeliverModel fileDeliverModel, DestinationModel? destination = null)
    {
        string body = fileInfo.Name + @"<html> was delivered to the following locations:<br><br>";

        body += " SourceId: " + source.ConfigurationId.ToString() + " " + source.SourceProtocolCode + ":" + source.SourceAddress + "<br><br>";
        body += this.GetDestinationsForEmailBody(source, destination) + "<br>";

        body += " GroupId: " + fileDeliverModel.GroupId.ToString() + "<br>";

        if (destination != null && destination.FTEMetadataId != null && destination.FTEMetadataId != Guid.Empty)
        {
            string link = this.BuildSTPUrl(fileDeliverModel.GroupId.ToString());
            body += "<br>Tracking Process Link: <a href='" + link + "'>Track History Process</a>";
        }

        body += "<br><br>Detailed log info follows<br>-------------------------<br>" + this.BuildLogMessages();

        return body += "</html>";
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

    private string BuildSTPUrl(string groupId)
    {
        return !string.IsNullOrEmpty(this.configurationManager.AppSettings?[AppSettingsConstants.HostName])
                ? string.Format("{0}{1}{2}", this.configurationManager.AppSettings?[AppSettingsConstants.HostName], AppSettingsConstants.AppSettingsKey.SPTURLPath, groupId)
                : string.Empty;
    }

    private string GetSFTPUserName(FileToDeliverModel fileDeliverModel)
    {
        this.Log(LogCode.Info, " Getting the Uploaded User Name from the File using Move IT for fileName" + fileDeliverModel.SourceLocation.FtpServerDetail?.FtpServerIp + " " + fileDeliverModel.SourceLocation.SourceAddress + " " + fileDeliverModel.FileName + " " + DateTime.Now.ToString());
        string uploadedSFTPUserName = fileDeliverModel.SourceLocation.FtpServerDetail?.FtpUser ?? string.Empty;
        this.Log(LogCode.Info, "SFTP User name is " + (string.IsNullOrEmpty(uploadedSFTPUserName) ? "empty" : uploadedSFTPUserName) + " for file " + fileDeliverModel.FileName + DateTime.Now.ToString());

        return uploadedSFTPUserName;
    }

    public virtual SourceLocationModel? GetSourceLocation(FileToDeliverModel fileDeliverModel)
    {
        IDataAccess<SourceLocationModel?> configMetaDataDataAccess = this.dataAccessFactory.CreateGetConfigMetaDataDataAccess(fileDeliverModel.FileId);
        return configMetaDataDataAccess != null ? ExecuteSync(configMetaDataDataAccess) : null;
    }

    private string DownloadWorkingFile(IFileTransferOperations fileTransferOperations, SourceLocationModel source, string fileName, bool reuseWorkFile)
    {
        this.Log(LogCode.DownloadWorkingFile);

        var workingDirectory = this.configurationManager.AppSettings?[AppSettingsConstants.WorkingDirectory];
        string fullyQualifiedWorkingFile = string.Concat(workingDirectory, "\\", source.ConfigurationId, "\\", new FileInfo(fileName).Name);
        try
        {
            DestinationModel destination = new DestinationModel();
            destination.DestinationProtocolCode = "UNC";
            destination.DestinationFileName = fullyQualifiedWorkingFile;
            destination.DestinationAddress = workingDirectory;

            Directory.CreateDirectory(new FileInfo(fullyQualifiedWorkingFile).DirectoryName!);

            if (reuseWorkFile && File.Exists(fullyQualifiedWorkingFile))
            {
                return fullyQualifiedWorkingFile;
            }

        _ = fileTransferOperations.DownloadFile(source, fileName, destination, this.TenantId);
        }
        catch (Exception ex)
        {
            this.Log(LogCode.Info, ex.Message);
        }

        return fullyQualifiedWorkingFile;
    }

    private void Log(LogCode code)
    {
        logFile.Add(new LogMessage
        {
            Code = code,
            Message = string.Empty
        });
    }

    private void Log(LogCode code, string message)
    {
        logFile.Add(new LogMessage
        {
            Code = code,
            Message = message
        });
    }

    private string BuildLogMessages()
    {
        string prefix = string.Empty;

        StringBuilder logMessages = new StringBuilder();

        foreach (LogMessage detail in logFile)
        {
            logMessages.Append(prefix).Append(detail.Code).Append(": ").Append(detail.Message).Append("\r\n");
        }

        return logMessages.ToString();
    }

    private static TOutput ExecuteSync<TOutput>(IDataAccess<TOutput> dataAccess)
    {
        return dataAccess.ExecuteAsync(CancellationToken.None).GetAwaiter().GetResult();
    }
}

