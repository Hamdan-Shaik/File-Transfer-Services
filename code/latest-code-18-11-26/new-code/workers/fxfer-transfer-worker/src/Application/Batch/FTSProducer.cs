using System.Collections.Concurrent;
using System.Text;

using Fxfer.TransferWorker.Application.Constants;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Domain.Batch;
using Fxfer.TransferWorker.Domain.Exceptions;
using Fxfer.TransferWorker.Worker.Configuration;

using Microsoft.Extensions.Logging;

namespace Fxfer.TransferWorker.Application.Batch;

/// <summary>
/// This class is responsible for querying the various source locations
/// and determining if files need to be moved
/// </summary>
public sealed class FTSProducer : IFTSProducer
{
    private readonly IDataAccessFactory dataAccessFactory;
    private readonly IFileTransferOperations fileTrasferOperations;
    private readonly ITenantAppSettingsManager configurationManager;
    private readonly IRequestContextAccessor requestContextAccessor;
    private readonly ILogger<FTSProducer> logger;

    private bool duplicateRenameActive;
    private string? duplicateFileExtenstion;
    private int maxFilesPerCycle;
    private int minutesBetweenRetry;
    private int minutesBetweenSuspendedRetry;
    private bool disableNotifications;
    private string? environmentName;

    public FTSProducer(
        IDataAccessFactory dacFactory,
        IFileTransferOperations ftoperations,
        ITenantAppSettingsManager configurationManager,
        IRequestContextAccessor requestContextAccessor,
        ILogger<FTSProducer> logger)
    {
        this.dataAccessFactory = dacFactory;
        this.fileTrasferOperations = ftoperations;
        this.configurationManager = configurationManager;
        this.requestContextAccessor = requestContextAccessor;
        this.logger = logger;

        if (this.configurationManager.AppSettings is null && !string.IsNullOrWhiteSpace(requestContextAccessor.TenantId))
        {
            this.configurationManager.LoadApplicationConfiguration(requestContextAccessor.TenantId);
        }

        this.InitializeAppsettings();
    }

    public BlockingCollection<FileToDeliverModel> ProcessAllFilesSources(string tenantId)
    {
        if (this.configurationManager.AppSettings is null && !string.IsNullOrWhiteSpace(tenantId))
        {
            this.configurationManager.LoadApplicationConfiguration(tenantId);
            this.InitializeAppsettings();
        }

        BlockingCollection<FileToDeliverModel> filesToDeliverQueue = new BlockingCollection<FileToDeliverModel>();
        List<PendingFileModel> pendingFiles = ExecuteSync(this.dataAccessFactory.CreateGetPendingFileListDataAccess());
        List<SourceLocationModel> sourceLocationList = ExecuteSync(this.dataAccessFactory.CreateGetSourceLocationsDataAccess());

        bool isSourceSetToRun = false;

        foreach (var location in sourceLocationList)
        {
            isSourceSetToRun = this.IsSourceSetToRun(location);

            if (isSourceSetToRun && location.IsScheduledToRun)
            {
                this.ProcessAllSourceFilesInLocation(filesToDeliverQueue, pendingFiles, location);
            }
            else
            {
                this.logger.LogInformation("Skipped location: " + location.ConfigurationId.ToString() + " SetToRun: " + isSourceSetToRun + " ScheduledToRun: " + location.IsScheduledToRun);
            }
        }

        return filesToDeliverQueue;
    }

    public BlockingCollection<FileToDeliverModel> ProcessFailedTransfers(int? topXFailedTransfers, Guid? specificSakFileTracking, int? specificConfigId, string tenantId)
    {
        if (this.configurationManager.AppSettings is null && !string.IsNullOrWhiteSpace(tenantId))
        {
            this.configurationManager.LoadApplicationConfiguration(tenantId);
            this.InitializeAppsettings();
        }

        BlockingCollection<FileToDeliverModel> filesToDeliverQueue = new BlockingCollection<FileToDeliverModel>();
        List<PendingFileModel> pendingFiles = this.GetPendingFilesByStatusDate();
        List<SourceLocationModel> sourceLocations = ExecuteSync(this.dataAccessFactory.CreateGetSourceLocationsDataAccess());
        foreach (PendingFileModel pending in pendingFiles)
        {
            var location = sourceLocations.Find(x => x.SourceLocationId == pending.FileSourceLocationId);
            if (location != null)
            {
                pending.ConfigId = (int)location.ConfigurationId;
            }
        }

        StringBuilder msg = new StringBuilder();
        msg.AppendLine(string.Format("There are {0} source locations defined", sourceLocations.Count))
            .AppendFormat("There are {0} pending files in the database, here are the oldest 50", pendingFiles.Count(x => x.FileStatus != FileStatusConstant.Completed));
        this.logger.LogInformation(msg.ToString());

        this.PrintLast50FileStatus(pendingFiles);

        List<PendingFileModel> filesToProcess = this.GetPendingFilesToReProcess(pendingFiles, specificSakFileTracking, specificConfigId, topXFailedTransfers);
        if (filesToProcess.Count == 0)
        {
            return filesToDeliverQueue;
        }

        this.ProcessAllPendingFiles(filesToDeliverQueue, sourceLocations, filesToProcess);

        return filesToDeliverQueue;
    }

    private void ProcessAllSourceFilesInLocation(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location)
    {
        this.logger.LogInformation("Trying location " + location.ConfigurationId.ToString());
        Exception? locationException = null;
        int totalFilesInLocation = 0;
        try
        {
            totalFilesInLocation = this.AddSourceFilesToQueue(filesToDeliverQueue, pendingFiles, location);
        }
        catch (Exception ex)
        {
            locationException = ex;
        }

        this.logger.LogInformation(string.Format("  Total files queued from location {0} = {1}", location.ConfigurationId, totalFilesInLocation));

        if (locationException != null)
        {
            this.HandleLocationException(location, locationException);
        }
    }

    private int AddSourceFilesToQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location)
    {
        List<FileBasicInfo> sourceFiles = this.fileTrasferOperations.GetListOfFileNamesFromSource(location, this.maxFilesPerCycle);
        int totalFilesInLocation = 0;
        foreach (FileBasicInfo file in sourceFiles)
        {
            FileInfo fileInfo = new FileInfo(file.Name);

            // Ignore any files that have the fts dup extension on them
            if (this.duplicateRenameActive && string.Compare(fileInfo.Extension, this.duplicateFileExtenstion, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                this.logger.LogInformation(string.Format("Ignorning duplicate file from source {0}: {1}", location.ConfigurationId, fileInfo.Name));
                continue;
            }

            totalFilesInLocation += this.CheckAndAddPendingFile(filesToDeliverQueue, pendingFiles, location, fileInfo, file.Name);
        }

        return totalFilesInLocation;
    }

    private int CheckAndAddPendingFile(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location, FileInfo fileInfo, string file)
    {
        var pendingFile = pendingFiles.Find(x => x.FileSourceLocationId == location.SourceLocationId && x.FileName == fileInfo.Name);
        if (pendingFile == null)
        {
            return this.AddNewFileToDeliveryQueue(filesToDeliverQueue, location, file);
        }
        else
        {
            return this.AddPendingFileToDeliveryQueue(filesToDeliverQueue, location, pendingFile, file);
        }
    }

    private int AddNewFileToDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, SourceLocationModel location, string file)
    {
        // Try to retain the previous file tracking sak if requested
        int sakFileTrack = 0;
        if (location.PreviousConfigurationId != 0)
        {
            var x = file.Split('_');
            if (x.Length > 0)
            {
                int.TryParse(x[0], out sakFileTrack);
            }
        }

        // Queue up a new file to be delivered
        filesToDeliverQueue.Add(new FileToDeliverModel()
        {
            SourceLocation = location,
            FileName = file,
            SakFileTrack = sakFileTrack,
            FileSize = File.Exists(file) ? new FileInfo(file).Length : 0,
            FileType = Path.GetExtension(file),
            GroupId = Guid.NewGuid(),
        });

        return 1;
    }

    private int AddPendingFileToDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, SourceLocationModel location, PendingFileModel pendingFile, string file)
    {
        // Queue up a restart file
        if (pendingFile.StatusDate.AddMinutes(this.minutesBetweenRetry) <= DateTime.Now)
        {
            _ = ExecuteSync(this.dataAccessFactory.CreateUpdateFileManagementStatusDataAccess(pendingFile.FileId, FileStatusConstant.Transfering));

            filesToDeliverQueue.Add(new FileToDeliverModel()
            {
                SourceLocation = location,
                FileName = file,
                FileId = pendingFile.FileId,
                SakFileTrack = pendingFile.FileTrackId,
                FileSize = File.Exists(file) ? new FileInfo(file).Length : 0,
                FileType = Path.GetExtension(file),
                GroupId = Guid.NewGuid(),
            });

            return 1;
        }

        return 0;
    }

    private void HandleLocationException(SourceLocationModel location, Exception ex)
    {
        // Handle the exception outside the try/catch in the main loop so we won't get caught with another exception
        // that we cannot catch which would stop processing of all other locations
        try
        {
            this.DumpErrorToConsole(location, ex);

            FtsSuspensionSearchCriteria input = new FtsSuspensionSearchCriteria { SourceLocationId = location.SourceLocationId, ErrorText = ex.Message, StackTrace = ex.StackTrace };
            _ = ExecuteSync(this.dataAccessFactory.CreateFtsSuspensionDataAccess(input));

            if (!this.disableNotifications)
            {
                this.fileTrasferOperations.SendNotification(location, "FTS: " + this.environmentName + " location " + location.ConfigurationId + " has been suspended", ex.Message, true, this.requestContextAccessor.TenantId ?? string.Empty);
            }
        }
        catch (Exception)
        {
            // Ignore this since it is likely from trying to send the notification
        }
    }

    private void DumpErrorToConsole(SourceLocationModel location, Exception ex)
    {
        StringBuilder errDump = new StringBuilder();
        errDump.AppendLine("ERROR: Could not access source: ConfigId: " + location.ConfigurationId.ToString());

        if (location.FtpServerDetail != null)
        {
            errDump.AppendLine(location.SourceProtocolCode + " Sever: " + location.FtpServerDetail.FtpUser + "@" + location.FtpServerDetail.FtpServerIp + ":" + location.SourceAddress);
        }
        else
        {
            errDump.AppendLine(location.SourceProtocolCode + " Sever: " + location.SourceAddress);
        }

        errDump.Append("  Location will be suspended.  You must reactivate it via the UI when problem is corrected.");
        this.logger.LogError(ex, errDump.ToString());
    }

    private List<PendingFileModel> GetPendingFilesByStatusDate()
    {
        List<PendingFileModel> pendingFiles = ExecuteSync(this.dataAccessFactory.CreateGetPendingFileListDataAccess());

        if (pendingFiles.Count > 0)
        {
            pendingFiles = pendingFiles.OrderBy(p => p.StatusDate.Date).ToList();
        }

        return pendingFiles;
    }

    private void PrintLast50FileStatus(List<PendingFileModel> pendingFiles)
    {
        int i = 0;
        foreach (var pend in pendingFiles)
        {
            if (i > 50)
            {
                break;
            }

            if (pend.FileStatus != FileStatusConstant.Completed)
            {
                this.logger.LogInformation(string.Format("{0};{1}: {2} {3} {4}", pend.FileTrackId, pend.FileId, pend.StatusDate.ToShortDateString(), pend.FileStatus, pend.FileName));
                i++;
            }
        }
    }

    private List<PendingFileModel> GetPendingFilesToReProcess(List<PendingFileModel> pendingFilesList, Guid? specificSakFileTracking, int? specificConfigId, int? topXFailedTransfers)
    {
        // Create the list of pending files to re-process
        List<PendingFileModel> filesToProcess = new List<PendingFileModel>();
        if (specificSakFileTracking != null)
        {
            // Make sure this sak exists in the pending list
            var pendingFile = pendingFilesList.Find(x => x.FileId == specificSakFileTracking.Value);
            if (pendingFile == null || pendingFile.FileStatus == FileStatusConstant.Completed)
            {
                this.logger.LogError("Could not find failed sak_file_track " + specificSakFileTracking.Value.ToString());
                return filesToProcess;
            }

            filesToProcess.Add(pendingFile);
        }
        else if (specificConfigId != null)
        {
            foreach (var pendingFile in pendingFilesList)
            {
                if (pendingFile.ConfigId == specificConfigId.Value && pendingFile.FileStatus != FileStatusConstant.Completed)
                {
                    filesToProcess.Add(pendingFile);
                }
            }
        }
        else if (topXFailedTransfers != null)
        {
            for (int j = 0, added = 0; added < topXFailedTransfers.Value && j < pendingFilesList.Count; j++)
            {
                if (pendingFilesList[j].FileStatus != FileStatusConstant.Completed)
                {
                    filesToProcess.Add(pendingFilesList[j]);
                    added++;
                }
            }
        }
        else
        {
            this.logger.LogError("You must specify either a specific sak_file_track or request the top X failed transfers to be reprocessed");
            return filesToProcess;
        }

        return filesToProcess;
    }

    private void ProcessAllPendingFiles(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<SourceLocationModel> sourceLocationList, List<PendingFileModel> filesToProcess)
    {
        // Process all of the files that were identified
        foreach (var pendingFile in filesToProcess)
        {
            try
            {
                var location = sourceLocationList.Find(x => x.SourceLocationId == pendingFile.FileSourceLocationId);
                if (location == null)
                {
                    this.logger.LogError(string.Format("ERROR: Location {0} not found failed for sak_file_track {1}", pendingFile.FileSourceLocationId, pendingFile.FileTrackId));
                    continue;
                }

                // Make sure the file still exists at the source
                var fileList = this.fileTrasferOperations.GetListOfFileNamesFromSource(location, this.maxFilesPerCycle);
                string? sourceFile = null;
                FileBasicInfo? sourceFileInfo = null;
                foreach (var file in fileList)
                {
                    if (Path.GetFileName(file.Name) == pendingFile.FileName)
                    {
                        sourceFile = file.Name;
                        sourceFileInfo = file;
                        break;
                    }
                }

                if (sourceFile == null)
                {
                    this.logger.LogError(string.Format("A file name {0} no longer exists at the source location for sak_file_track {1}.  Marking this file as completed.", pendingFile.FileName, pendingFile.FileTrackId));
                    ErrorLogDetailModel errorDetail = new ErrorLogDetailModel { FileId = pendingFile.FileId, ErrorType = "RestartFileIssue", ErrorText = "This file no longer exists at the source location so was marked as completed", StackTrace = "N/A", MethodName = "ProcessSingleTracking" };
                    _ = ExecuteSync(this.dataAccessFactory.CreateErrorLogDataAccess(errorDetail));
                }
                else
                {
                    // Reset to pending
                    this.dataAccessFactory.CreateUpdateFileManagementStatusDataAccess(pendingFile.FileId, FileStatusConstant.Transfering);

                    // Add to the queue so it will be processed by one of client tasks
                    filesToDeliverQueue.Add(new FileToDeliverModel()
                    {
                        SourceLocation = location,
                        FileName = sourceFile,
                        FileId = pendingFile.FileId,
                        SakFileTrack = pendingFile.FileTrackId,
                         FileSize = sourceFileInfo?.Size ?? 0,
                        FileType = Path.GetExtension(sourceFile),
                        GroupId = Guid.NewGuid(),
                    });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, string.Format("Exception on: file name {0} sak_file_track {1}. continuing to next record", pendingFile.FileName, pendingFile.FileTrackId));
            }
        }
    }

    private bool IsSourceSetToRun(SourceLocationModel sourceModel)
    {
        bool result = false;

        if (sourceModel.IsActive)
        {
            if (!sourceModel.IsSuspended)
            {
                result = true;
            }
            else if (sourceModel.SuspendedIndChanged <= DateTime.Now.AddMinutes(this.minutesBetweenSuspendedRetry * -1))
            {
                _ = ExecuteSync(this.dataAccessFactory.CreateUpdateSourceSuspendStatusDataAccess(sourceModel.SourceLocationId));
                result = true;
            }
        }

        return result;
    }

    private void InitializeAppsettings()
    {
        this.duplicateRenameActive = Convert.ToBoolean(this.configurationManager.AppSettings?[AppSettingsConstants.ActivateDuplicateRenameLogic]);
        this.duplicateFileExtenstion = this.configurationManager.AppSettings?[AppSettingsConstants.DuplicateFileExtension];
        this.maxFilesPerCycle = Convert.ToInt32(this.configurationManager.AppSettings?[AppSettingsConstants.MaxFilesPerCycle]);
        this.minutesBetweenRetry = Convert.ToInt32(this.configurationManager.AppSettings?[AppSettingsConstants.MinsToWaitBeforeRetryFailedFile]);
        this.disableNotifications = Convert.ToBoolean(this.configurationManager.AppSettings?[AppSettingsConstants.DisableNotifications]);
        this.environmentName = this.configurationManager.AppSettings?[AppSettingsConstants.EnvironmentName];
        this.minutesBetweenSuspendedRetry = Convert.ToInt32(this.configurationManager.AppSettings?[AppSettingsConstants.MinsToWaitBeforeRetrySuspendedConfig]);
    }

    private static TOutput ExecuteSync<TOutput>(IDataAccess<TOutput> dataAccess)
    {
        return dataAccess.ExecuteAsync(CancellationToken.None).GetAwaiter().GetResult();
    }
}

