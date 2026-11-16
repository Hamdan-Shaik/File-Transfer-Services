using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;
using Fxfer.ExecutionWorker.Batch.Business.DataAccess;
using Fxfer.ExecutionWorker.Domain.Batch.Business;
using Fxfer.ExecutionWorker.Domain.Constants;
using Fxfer.ExecutionWorker.Domain.Exceptions;

namespace Fxfer.ExecutionWorker.Batch.Business;

/// <summary>
/// This class is responsible for querying the various source locations
/// and determining if files need to be moved.
/// </summary>
public sealed class FTSProducer : IFTSProducer
{
    private readonly IDataAccessFactory dataAccessFactory;
    private readonly IFileTransferOperations fileTrasferOperations;
    private readonly IAppSettings appSettings;

    private bool duplicateRenameActive;
    private string duplicateFileExtenstion = string.Empty;
    private int maxFilesPerCycle;
    private int minutesBetweenRetry;
    private bool disableNotifications;
    private string environmentName = string.Empty;
    private int minutesBetweenSuspendedRetry;

    private string TenantId { get; set; } = string.Empty;

    public FTSProducer()
    {
        dataAccessFactory = BatchServiceLocator.GetRequiredService<IDataAccessFactory>();
        fileTrasferOperations = BatchServiceLocator.GetRequiredService<IFileTransferOperations>();
        appSettings = BatchServiceLocator.GetRequiredService<IAppSettings>();
        InitializeAppsettings();
    }

    public FTSProducer(IDataAccessFactory dacFactory, IFileTransferOperations ftoperations)
        : this(dacFactory, ftoperations, BatchServiceLocator.GetRequiredService<IAppSettings>())
    {
    }

    public FTSProducer(IDataAccessFactory dacFactory, IFileTransferOperations ftoperations, IAppSettings appSettings)
    {
        dataAccessFactory = dacFactory;
        fileTrasferOperations = ftoperations;
        this.appSettings = appSettings;
        InitializeAppsettings();
    }

    public BlockingCollection<FileToDeliverModel> ProcessAllFilesSources(string tenantId)
    {
        // F03-015: in the all-locations run, only process eligible and scheduled locations.
        BlockingCollection<FileToDeliverModel> filesToDeliverQueue = new BlockingCollection<FileToDeliverModel>();
        List<PendingFileModel> pendingFiles = dataAccessFactory.CreateGetPendingFileListDataAccess().Execute();
        List<SourceLocationModel> sourceLocationList = dataAccessFactory.CreateGetSourceLocationsDataAccess().Execute();
        TenantId = tenantId;

        bool isSourceSetToRun = false;

        foreach (var location in sourceLocationList)
        {
            isSourceSetToRun = IsSourceSetToRun(location);

            if (isSourceSetToRun && location.IsScheduledToRun)
            {
                ProcessAllSourceFilesInLocation(filesToDeliverQueue, pendingFiles, location);
            }
            else
            {
                Console.WriteLine("Skipped location: " + location.ConfigurationId.ToString() + " SetToRun: " + isSourceSetToRun + " ScheduledToRun: " + location.IsScheduledToRun);
            }
        }

        return filesToDeliverQueue;
    }

    public BlockingCollection<FileToDeliverModel> ProcessFailedTransfers(int? topXFailedTransfers, Guid? specificSakFileTracking, int? specificConfigId, string tenantId)
    {
        BlockingCollection<FileToDeliverModel> filesToDeliverQueue = new BlockingCollection<FileToDeliverModel>();
        List<PendingFileModel> pendingFiles = GetPendingFilesByStatusDate();
        List<SourceLocationModel> sourceLocations = dataAccessFactory.CreateGetSourceLocationsDataAccess().Execute();
        foreach (PendingFileModel pending in pendingFiles)
        {
            var location = sourceLocations.Find(x => x.SourceLocationId == pending.FileSourceLocationId);
            if (location != null)
            {
                pending.ConfigId = (int)location.ConfigurationId;
            }
        }

        TenantId = tenantId;

        StringBuilder msg = new StringBuilder();
        msg.AppendLine(string.Format("There are {0} source locations defined", sourceLocations.Count))
            .AppendFormat(
                "There are {0} pending files in the database, here are the oldest 50",
                pendingFiles.Count(x => x.FileStatus != DataAccessConstants.FileStatusConstant.Completed));
        Console.WriteLine(msg.ToString());

        PrintLast50FileStatus(pendingFiles);

        // F03-018: select reprocess items using one of (fileId, configId, top X), in that order.
        List<PendingFileModel> filesToProcess = GetPendingFilesToReProcess(pendingFiles, specificSakFileTracking, specificConfigId, topXFailedTransfers);
        if (filesToProcess.Count == 0)
        {
            return filesToDeliverQueue;
        }

        ProcessAllPendingFiles(filesToDeliverQueue, sourceLocations, filesToProcess);

        return filesToDeliverQueue;
    }

    private void ProcessAllSourceFilesInLocation(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location)
    {
        Console.WriteLine("Trying location " + location.ConfigurationId.ToString());
        Exception? locationException = null;
        int totalFilesInLocation = 0;
        try
        {
            totalFilesInLocation = AddSourceFilesToQueue(filesToDeliverQueue, pendingFiles, location);
        }
        catch (Exception ex)
        {
            locationException = ex;
        }

        Console.WriteLine(string.Format("  Total files queued from location {0} = {1}", location.ConfigurationId, totalFilesInLocation));

        if (locationException != null)
        {
            // F03-025: per-location failures suspend that location.
            HandleLocationException(location, locationException);
        }
    }

    private int AddSourceFilesToQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location)
    {
        List<FileBasicInfo> sourceFiles = fileTrasferOperations.GetListOfFileNamesFromSource(location, maxFilesPerCycle);

        int totalFilesInLocation = 0;
        foreach (FileBasicInfo fileInfo in sourceFiles)
        {
            // Ignore any files that have the fts dup extension on them
            if (duplicateRenameActive && string.Compare(fileInfo.Extension, duplicateFileExtenstion, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                Console.WriteLine(string.Format("Ignoring duplicate file from source {0}: {1}", location.ConfigurationId, fileInfo.Name));
                continue;
            }

            string extension = fileInfo.Extension.Substring(fileInfo.Extension.LastIndexOf('.') + 1).ToLower();
            if (Enum.IsDefined(typeof(ExcludedFileExtensions.ExcludedFileExtensionList), extension))
            {
                Console.WriteLine(string.Format("Ignoring file with excluded extension {0}: {1}", location.ConfigurationId, fileInfo.Name));
                continue;
            }

            totalFilesInLocation += CheckAndAddPendingFile(filesToDeliverQueue, pendingFiles, location, fileInfo);
        }

        return totalFilesInLocation;
    }

    private int CheckAndAddPendingFile(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location, FileBasicInfo fileInfo)
    {
        var pendingFile = pendingFiles.Find(x => x.FileSourceLocationId == location.SourceLocationId && x.FileName == fileInfo.Name);
        if (pendingFile == null)
        {
            return AddNewFileToDeliveryQueue(filesToDeliverQueue, location, fileInfo);
        }
        else
        {
            return AddPendingFileToDeliveryQueue(filesToDeliverQueue, location, pendingFile, fileInfo);
        }
    }

    private int AddNewFileToDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, SourceLocationModel location, FileBasicInfo fileInfo)
    {
        // F03-027: try to retain the previous file tracking sak if requested.
        int sakFileTrack = 0;
        if (location.PreviousConfigurationId != 0)
        {
            var x = fileInfo.Name.Split('_');
            if (x.Length > 0)
            {
                int.TryParse(x[0], out sakFileTrack);
            }
        }

        // Queue up a new file to be delivered
        filesToDeliverQueue.Add(BuildNewFileToDeliverModel(location, fileInfo, sakFileTrack));

        return 1;
    }

    private FileToDeliverModel BuildNewFileToDeliverModel(SourceLocationModel location, FileBasicInfo fileInfo, int sakFileTrack)
    {
        return new FileToDeliverModel()
        {
            SourceLocation = location,
            FileName = fileInfo.Name,
            SakFileTrack = sakFileTrack,
            FileSize = fileInfo.Size,
            FileType = fileInfo.Extension,
            GroupId = Guid.NewGuid(),
        };
    }

    private int AddPendingFileToDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, SourceLocationModel location, PendingFileModel pendingFile, FileBasicInfo fileInfo)
    {
        // F03-017: a tracked file is only re-tried after the retry window has passed; when re-queued set status to "Transfering".
        // Queue up a restart file
        if (pendingFile.StatusDate.AddMinutes(minutesBetweenRetry) <= DateTime.UtcNow)
        {
            dataAccessFactory.CreateUpdateFileManagementStatusDataAccess(pendingFile.FileId, DataAccessConstants.FileStatusConstant.Transfering).Execute();

            filesToDeliverQueue.Add(BuildPendingFileToDeliverModel(location, pendingFile, fileInfo));

            return 1;
        }

        return 0;
    }

    private FileToDeliverModel BuildPendingFileToDeliverModel(SourceLocationModel location, PendingFileModel pendingFile, FileBasicInfo fileInfo)
    {
        return new FileToDeliverModel()
        {
            SourceLocation = location,
            FileName = fileInfo.Name,
            FileId = pendingFile.FileId,
            FileSize = fileInfo.Size,
            FileType = fileInfo.Extension,
            GroupId = Guid.NewGuid(),
        };
    }

    private void HandleLocationException(SourceLocationModel location, Exception ex)
    {
        // F03-025: handle exceptions per location to avoid stopping other locations.
        // Handle the exception outside the try/catch in the main loop so we won't get caught with another exception
        // that we cannot catch which would stop processing of all other locations
        try
        {
            DumpErrorToConsole(location, ex);

            FtsSuspensionSearchCriteria input = new() { SourceLocationId = location.SourceLocationId, ErrorText = ex.Message, StackTrace = ex.StackTrace };
            dataAccessFactory.CreateFtsSuspensionDataAccess(input).Execute();

            if (!disableNotifications)
            {
                fileTrasferOperations.SendNotification(location, "FTS: " + environmentName + " location " + location.ConfigurationId + " has been suspended", ex.Message, true, TenantId);
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
        Console.WriteLine(errDump.ToString());
        Console.WriteLine(ex);
    }

    private List<PendingFileModel> GetPendingFilesByStatusDate()
    {
        List<PendingFileModel> pendingFiles = dataAccessFactory.CreateGetPendingFileListDataAccess().Execute();

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

            if (pend.FileStatus != DataAccessConstants.FileStatusConstant.Completed)
            {
                Console.WriteLine(string.Format("{0};{1}: {2} {3} {4}", pend.FileTrackId, pend.FileId, pend.StatusDate.ToShortDateString(), pend.FileStatus, pend.FileName));
                i++;
            }
        }
    }

    private List<PendingFileModel> GetPendingFilesToReProcess(List<PendingFileModel> pendingFilesList, Guid? specificSakFileTracking, int? specificConfigId, int? topXFailedTransfers)
    {
        // F03-018: select pending items by file id, config id, or top X oldest non-completed items.
        // Create the list of pending files to re-process
        List<PendingFileModel> filesToProcess = new List<PendingFileModel>();
        if (specificSakFileTracking != null)
        {
            // Make sure this sak exists in the pending list
            var pendingFile = pendingFilesList.Find(x => x.FileId == specificSakFileTracking.Value);
            if (pendingFile == null || pendingFile.FileStatus == DataAccessConstants.FileStatusConstant.Completed)
            {
                Console.WriteLine("Could not find failed sak_file_track " + specificSakFileTracking.Value.ToString());
                return filesToProcess;
            }

            filesToProcess.Add(pendingFile);
        }
        else if (specificConfigId != null)
        {
            foreach (var pendingFile in pendingFilesList)
            {
                if (pendingFile.ConfigId == specificConfigId.Value && pendingFile.FileStatus != DataAccessConstants.FileStatusConstant.Completed)
                {
                    filesToProcess.Add(pendingFile);
                }
            }
        }
        else if (topXFailedTransfers != null)
        {
            for (int j = 0, added = 0; added < topXFailedTransfers.Value && j < pendingFilesList.Count; j++)
            {
                if (pendingFilesList[j].FileStatus != DataAccessConstants.FileStatusConstant.Completed)
                {
                    filesToProcess.Add(pendingFilesList[j]);
                    added++;
                }
            }
        }
        else
        {
            Console.WriteLine("You must specify either a specific sak_file_track or request the top X failed transfers to be reprocessed");
            return filesToProcess;
        }

        return filesToProcess;
    }

    private void ProcessAllPendingFiles(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<SourceLocationModel> sourceLocationList, List<PendingFileModel> filesToProcess)
    {
        // F03-018: if the source file no longer exists, record an error and do not re-queue.
        // Process all of the files that were identified
        foreach (var pendingFile in filesToProcess)
        {
            try
            {
                var location = sourceLocationList.Find(x => x.SourceLocationId == pendingFile.FileSourceLocationId);
                if (location == null)
                {
                    Console.WriteLine(string.Format("ERROR: Location {0} not found failed for sak_file_track {1}", pendingFile.FileSourceLocationId, pendingFile.FileTrackId));
                    continue;
                }

                // Make sure the file still exists at the source
                var fileList = fileTrasferOperations.GetListOfFileNamesFromSource(location, maxFilesPerCycle);
                string? sourceFile = null;
                foreach (var file in fileList)
                {
                    if (Path.GetFileName(file.Name) == Path.GetFileName(pendingFile.FileName))
                    {
                        sourceFile = file.Name;
                        break;
                    }
                }

                if (sourceFile == null)
                {
                    Console.WriteLine(string.Format("A file name {0} no longer exists at the source location for sak_file_track {1}.  Marking this file as completed.", pendingFile.FileName, pendingFile.FileTrackId));
                    ErrorLogDetailModel errorDetail = new ErrorLogDetailModel { FileId = pendingFile.FileId, ErrorType = "RestartFileIssue", ErrorText = "This file no longer exists at the source location so was marked as completed", StackTrace = "N/A", MethodName = "ProcessSingleTracking" };
                    dataAccessFactory.CreateErrorLogDataAccess(errorDetail).Execute();
                }
                else
                {
                    // Reset to pending
                    dataAccessFactory.CreateUpdateFileManagementStatusDataAccess(pendingFile.FileId, DataAccessConstants.FileStatusConstant.Transfering);

                    // Add to the queue so it will be processed by one of client tasks
                    filesToDeliverQueue.Add(new FileToDeliverModel()
                    {
                        SourceLocation = location,
                        FileName = sourceFile,
                        FileId = pendingFile.FileId,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception on: file name {0} sak_file_track {1}. continuing to next record", pendingFile.FileName, pendingFile.FileTrackId));
                Console.WriteLine(ex);
            }
        }
    }

    private bool IsSourceSetToRun(SourceLocationModel sourceModel)
    {
        // F03-015: determine if a location is eligible to run (active and not suspended, or eligible for retry).
        bool result = false;

        if (sourceModel.IsActive)
        {
            if (!sourceModel.IsSuspended)
            {
                result = true;
            }
            else if (sourceModel.SuspendedIndChanged <= DateTime.Now.AddMinutes(minutesBetweenSuspendedRetry * -1))
            {
                // shortcut: legacy updates suspension status via a full source-location record; target updates by id.
                dataAccessFactory.CreateUpdateSourceSuspendStatusDataAccess(sourceModel.SourceLocationId).Execute();
                result = true;
            }
        }

        return result;
    }

    private void InitializeAppsettings()
    {
        duplicateRenameActive = Convert.ToBoolean(appSettings[AppSettingsConstants.ActivateDuplicateRenameLogic]);
        duplicateFileExtenstion = appSettings[AppSettingsConstants.DuplicateFileExtension]!;
        maxFilesPerCycle = Convert.ToInt32(appSettings[AppSettingsConstants.MaxFilesPerCycle]);
        minutesBetweenRetry = Convert.ToInt32(appSettings[AppSettingsConstants.MinsToWaitBeforeRetryFailedFile]);
        disableNotifications = Convert.ToBoolean(appSettings[AppSettingsConstants.DisableNotifications]);
        environmentName = appSettings[AppSettingsConstants.EnvironmentName]!;
        minutesBetweenSuspendedRetry = Convert.ToInt32(appSettings[AppSettingsConstants.MinsToWaitBeforeRetrySuspendedConfig]);
    }
}

