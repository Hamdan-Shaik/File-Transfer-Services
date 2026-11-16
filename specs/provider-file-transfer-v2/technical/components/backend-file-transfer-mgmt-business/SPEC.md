---
type: Behavior Specification
title: technical/components/backend-file-transfer-mgmt-business/SPEC
description: 'What the backend-file-transfer-mgmt-business component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-mgmt-business
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:00:07Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-mgmt-business/INVENTORY.yaml
  title: Every legacy class and method (23 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-mgmt-business
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 23
  methods: 121
  owned_rules: 0
  flows: []
  depends_on:
  - backend-business-helpers-2
  deployable: fxfer-transfer-worker
  folder: src/Application
  body_sha256: 17ec121aa278d822b367d61aaefec5b92e8cbbaf3faab9f60acdeee90b9363c3
---

﻿# backend-file-transfer-mgmt-business: Legacy batch file-transfer management business logic

This component is the worker-side legacy batch file-transfer engine for Provider File Transfer.
It discovers files at configured source locations, stages them into a working directory, optionally compresses and/or encrypts/decrypts them, transfers them to one or more destinations (UNC/FTP/FTPS/SFTP), and records tracking/outcome information.

It is invoked by `FileTransferBatchExecutor` and relies on legacy data-access (`IDataAccessFactory`), legacy notification service calls, and the helper component `backend-business-helpers-2`.

It owns no business rules and exposes no HTTP surface.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.AppSettingsConstants
Static string constants for legacy `ApplicationConfigurationManager.AppSettings` keys used by the batch process.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.AppSettingsConstants.AppSettingsKey
Nested constants under `AppSettingsConstants`.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.BatchJobConstants
Static constants used across the batch engine.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.BatchJobConstants.EventFilters
Static constants for event filter metadata.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.BatchJobConstants.EventFilters.EventMessageNodes
XML node-name constants used when generating event filter XML.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.ExceptionConstants
Constants describing error origins.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.IocConstants
Legacy Unity-container registration keys used to resolve dependencies.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.IBatchExecutor<Toutput>
Generic interface for invoking a batch executor.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.IConsumerFactory
Factory interface for creating `IFTSConsumer` instances.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.IFTSConsumer
Consumer interface for processing the file-delivery work queue.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.IFTSProducer
Producer interface for discovering work items.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.IFileTransferOperations
Facade over protocol-specific operations plus naming/zip/notification helpers.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.IFileTransferProtocols
Low-level protocol operations (UNC + FTP/FTPS via `edtFTPnetPRO` + SSH/SFTP via `Renci.SshNet`).

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.IPatternMatcher
Interface for file-mask matching.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.ConsumerFactory
Factory implementation that constructs a concrete `FTSConsumer`.

### CreateConsumer(int consumerIdentifier)
- Purpose: Instantiate a consumer for a given worker id.
- Logic: Returns `new FTSConsumer(consumerIdentifier)`.
- Data: None.
- Errors: None.
- Rules: None.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo
Value object holding basic file metadata used during discovery.

Constructor behavior:

- `FileBasicInfo(string name, long size)` sets `Name = name`, `Extension = Path.GetExtension(name)`, `Size = size`.
- `FileBasicInfo(string name, string extension, long size)` delegates to `FileBasicInfo(string name, long size)` and then sets `Extension = extension`.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileTransferBatchExecutor
Batch entry-point that chooses a run mode, queues work items, and runs concurrent consumers.

### Execute(string[] inputArgs)
- Purpose: Run the batch.
- Inputs:
  - `inputArgs`: either `[tenantId]` (normal mode) or `[command, commandValue, tenantId]` (rerun mode).
- Logic:
  - Calls `AssignTenantId(inputArgs)`.
  - Creates a `BlockingCollection<FileToDeliverModel>`.
  - Calls `AddFilesToDeliveryQueue(inputArgs, blockingCollection)`.
  - Calls `ProcessFilesInDeliverQueue(filesToDeliverQueue)`.
  - Returns `0`.
- Data:
  - Reads: `ApplicationConfigurationManager.AppSettings[MaxConcurrentDeliveryProcesses]`.
- Errors:
  - Consumer task exceptions are logged in `WaitForCompletion`.
- Rules: None.

### AssignTenantId(string[] inputArgs)
- Purpose: Determine tenant id from args.
- Logic:
  - If `inputArgs.Length == 1`, uses `inputArgs[0]`.
  - Else uses `inputArgs[2]`.

### AddFilesToDeliveryQueue(string[] arguments, BlockingCollection<FileToDeliverModel> filesToDeliverQueue)
- Purpose: Populate the delivery queue (normal or rerun mode).
- Logic:
  - If `arguments.Length > 1`, calls `ParseInputArguments(arguments, out fileId, out topXFailures, out configId)`.
  - If `fileId`, `topXFailures`, and `configId` are all null:
    - logs `"INFO: Running for all locations"`.
    - sets `filesToDeliverQueue = ftsProducer.ProcessAllFilesSources(tenantId)`.
  - Else:
    - logs `"INFO: Running in a special mode to re-run failed transfers using the specified criteria"`.
    - sets `filesToDeliverQueue = ftsProducer.ProcessFailedTransfers(topXFailures, fileId, configId, tenantId)`.
  - Returns the resulting queue.

### ProcessFilesInDeliverQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue)
- Purpose: Spin up consumers and wait for completion.
- Logic:
  - Calls `CreateConsumerTasks(filesToDeliverQueue)`.
  - If queue is not null and `Count > 0`, calls `CompleteAdding()`.
  - Calls `WaitForCompletion(consumerTasks)`.

### CreateConsumerTasks(BlockingCollection<FileToDeliverModel> filesToDeliverQueue)
- Purpose: Start concurrent consumers.
- Logic:
  - Parses `AppSettingsConstants.MaxConcurrentDeliveryProcesses` to `maxConcurrentDeliveryProcesses`.
  - Creates `Task[maxConcurrentDeliveryProcesses]`.
  - For each `processId`:
    - creates `consumerFactory.CreateConsumer(processId)`.
    - starts `Task.Run(() => consumer.ProcessDeliveryQueue(filesToDeliverQueue, tenantId))`.

### WaitForCompletion(Task[] consumers)
- Purpose: Join consumer tasks.
- Logic:
  - Calls `Task.WaitAll(consumers)`.
  - On `AggregateException`, logs each `InnerException.Message`.

### ParseInputArguments(string[] arguments, out Guid? sakTrack, out int? topXFailures, out int? configId)
- Purpose: Parse rerun-mode CLI arguments.
- Inputs: `arguments[0]` is the command (`sakTrack|configId|top`) and `arguments[1]` is the command value.
- Logic:
  - `sakTrack`: `Guid.TryParse(arguments[1])`.
  - `configId`: `GetCommandValue(arguments[1])`.
  - `top`: `GetCommandValue(arguments[1])`.
  - Else calls `PrintUsage()`.

### GetCommandValue(string commandDataString)
- Purpose: Parse an int value.
- Logic: `int.TryParse` and returns the parsed value (0 on parse failure).

### PrintUsage()
- Purpose: Log usage information.
- Logic: Logs a multi-line usage string via `LoggerManager.Logger.LogInformational`.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FTSProducer
Discovers candidate files per source location and populates a delivery queue.

### ProcessAllFilesSources(string tenantId)
- Purpose: Discover work for all source locations.
- Logic:
  - Loads pending-file list and source-location list (no transaction).
  - Sets `TenantId`.
  - For each location:
    - `isSourceSetToRun = IsSourceSetToRun(location)`.
    - If set-to-run and `location.IsScheduledToRun`, calls `ProcessAllSourceFilesInLocation(...)`.
    - Else logs `Skipped location`.
  - Returns the `BlockingCollection<FileToDeliverModel>`.

### ProcessFailedTransfers(int? topXFailedTransfers, Guid? specificFileId, int? specificConfigId, string tenantId)
- Purpose: Discover rerun candidates (failed/pending transfers).
- Logic:
  - Loads pending files via `GetPendingFilesByStatusDate()`.
  - Loads all source locations.
  - Populates each `pending.ConfigId` by looking up its source location.
  - Logs summary counts and prints up to the oldest 50 non-completed items (`PrintLast50FileStatus`).
  - Filters candidates via `GetPendingFilesToReProcess(pendingFiles, specificFileId, specificConfigId, topXFailedTransfers)`.
  - Enqueues rerun work via `ProcessAllPendingFiles(queue, sourceLocations, filesToProcess)`.

### ProcessAllSourceFilesInLocation(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location)
- Purpose: Discover files for one location.
- Logic:
  - Logs `Trying location <configId>`.
  - Calls `AddSourceFilesToQueue(...)`.
  - Logs total queued.
  - If a location-level exception occurred, calls `HandleLocationException(location, ex)`.

### AddSourceFilesToQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location)
- Purpose: Add eligible source files to the queue.
- Logic:
  - Lists files via `fileTrasferOperations.GetListOfFileNamesFromSource(location, maxFilesPerCycle)`.
  - Skips:
    - files with the duplicate extension when `duplicateRenameActive` is true.
    - files with excluded extensions (enum `ExcludedFileExtensionList`).
  - For each remaining file, calls `CheckAndAddPendingFile(...)`.
  - Returns number queued.

### CheckAndAddPendingFile(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location, FileBasicInfo fileInfo)
- Purpose: Decide whether a source file is new or pending.
- Logic:
  - If there is no `pendingFiles` match for `(location.SourceLocationId, fileInfo.Name)`, calls `AddNewFileToDeliveryQueue`.
  - Else calls `AddPendingFileToDeliveryQueue`.

### AddNewFileToDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, SourceLocationModel location, FileBasicInfo fileInfo)
- Purpose: Enqueue a brand-new file.
- Logic:
  - If `location.PreviousConfigurationId != 0`, tries to parse the first `_`-delimited token of the filename into `sakFileTrack`.
  - Adds `BuildNewFileToDeliverModel(location, fileInfo, sakFileTrack)` to the queue.

### BuildNewFileToDeliverModel(SourceLocationModel location, FileBasicInfo fileInfo, int sakFileTrack)
- Purpose: Create a new work item.
- Logic: Sets `SakFileTrack`, `FileSize`, `FileType`, and generates `GroupId = Guid.NewGuid()`.

### AddPendingFileToDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, SourceLocationModel location, PendingFileModel pendingFile, FileBasicInfo fileInfo)
- Purpose: Enqueue a restart/rerun work item.
- Logic:
  - Only enqueues when `pendingFile.StatusDate.AddMinutes(minutesBetweenRetry) <= DateTime.UtcNow`.
  - Updates status to `FileStatusConstant.Transfering` and adds `BuildPendingFileToDeliverModel`.

### BuildPendingFileToDeliverModel(SourceLocationModel location, PendingFileModel pendingFile, FileBasicInfo fileInfo)
- Purpose: Create a pending work item.
- Logic: Uses `pendingFile.FileId` and generates a new `GroupId`.

### HandleLocationException(SourceLocationModel location, Exception ex)
- Purpose: Suspend a location and optionally notify operators.
- Logic:
  - Logs details via `DumpErrorToConsole`.
  - Stores suspension via `CreateFtsSuspensionDataAccess`.
  - When `disableNotifications` is false, emails the location owner.
  - Swallows secondary exceptions to avoid stopping processing of other locations.

### DumpErrorToConsole(SourceLocationModel location, Exception ex)
- Purpose: Emit a detailed error message for a location.
- Logic: Logs config id and either `user@server:sourcePath` or `sourcePath`, then notes the location will be suspended.

### GetPendingFilesByStatusDate()
- Purpose: Load and sort pending files by status date.
- Logic: Loads via data access and sorts by `StatusDate.Date` ascending.

### PrintLast50FileStatus(List<PendingFileModel> pendingFiles)
- Purpose: Log up to the oldest 50 non-completed items.
- Logic: Logs `<trackId>;<fileId>: <date> <status> <fileName>`.

### GetPendingFilesToReProcess(List<PendingFileModel> pendingFilesList, Guid? specificSakFileTracking, int? specificConfigId, int? topXFailedTransfers)
- Purpose: Select pending files for rerun.
- Logic:
  - If `specificSakFileTracking` is set, returns that item (if present and not completed) else logs error and returns empty.
  - Else if `specificConfigId` is set, returns all non-completed matching that config id.
  - Else if `topXFailedTransfers` is set, returns up to N non-completed items.
  - Else logs error and returns empty.

### ProcessAllPendingFiles(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<SourceLocationModel> sourceLocationList, List<PendingFileModel> filesToProcess)
- Purpose: Enqueue rerun work items.
- Logic (per pending file):
  - Resolves its source location.
  - Checks the file still exists at the source by listing source files and matching by base name.
  - When missing, logs an error and writes an `ErrorLogDetailModel` (`ErrorType = "RestartFileIssue"`).
  - When present, updates status to `Transfering` and adds a `FileToDeliverModel` with `FileId`.

### IsSourceSetToRun(SourceLocationModel sourceModel)
- Purpose: Decide whether a source is runnable.
- Logic:
  - Requires `sourceModel.IsActive`.
  - If not suspended, returns true.
  - If suspended and the suspension age exceeds `minutesBetweenSuspendedRetry`, clears suspension via data access and returns true.

### InitializeAppsettings()
- Purpose: Read tuning configuration.
- Logic: Loads `duplicateRenameActive`, `duplicateFileExtenstion`, `maxFilesPerCycle`, `minutesBetweenRetry`, `disableNotifications`, `environmentName`, `minutesBetweenSuspendedRetry`.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FTSConsumer
Consumes queued work items and performs the end-to-end staging/transfer/recording flow.

### Log(string message)
- Purpose: Append an informational entry into the in-memory `logFile` buffer.

### GetSourceLocation(FileToDeliverModel fileDeliverModel)
- Purpose: Resolve source-location configuration for a tracked file.
- Logic: Calls `dataAccessFactory.CreateGetConfigMetaDataDataAccess(fileDeliverModel.FileId)?.ExecuteWithNoTransaction()`.

### ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId)
- Purpose: Main consumer loop.
- Logic:
  - Sets `TenantId` and loads application configuration for the tenant.
  - While the queue is not complete:
    - resets `deleteSourceFile = true`.
    - takes a work item and calls `DeliverFile`.
  - On `DuplicateFileNameException`:
    - if `ActivateDuplicateRenameLogic` is true, renames the source file with `DuplicateFileExtension`, sets status to `Completed`, and notifies with errorType `Duplicate`.
    - else sets status to `Error` and notifies with errorType `Error`.
  - On any other exception: sets status to `Error` and notifies.
  - Always flushes buffered log entries to `LoggerManager.Logger.LogInformational`.

### DeliverFile(FileToDeliverModel fileDeliverModel)
- Purpose: Transfer one file to all configured destinations.
- Logic:
  - Calls `SetFileStatus` (creates file-management record for new work items).
  - Resolves `sourceLocation = GetSourceLocation(fileDeliverModel)`.
  - Stages the file into working directory via `DownloadWorkingFile`.
  - Computes `crc32`.
  - Reorders destinations via `SetFileDestinations`.
  - For each destination:
    - calls `TransferFile(...)`.
    - publishes `FileTransferred` integration event via `eventHelper.PublishFileTransferredEvent`.
  - Calls `CleanupTransferDirectories`.

### GetSFTPUserName(FileToDeliverModel fileDeliverModel)
- Purpose: Return the SFTP user name for rename-pattern usage.
- Logic: Returns `fileDeliverModel.SourceLocation.FtpServerDetail.FtpUser`.

### TransferFile(FileToDeliverModel fileDeliverModel, string uploadedSFTPUserName, string fullyQualifiedWorkingFile, string crc32, SourceLocationModel sourceLocation, DestinationModel destination, Stopwatch watcher)
- Purpose: Apply compression/encryption behavior and perform one destination upload.
- Logic:
  - If `destination.IsCompressAtDestination`, zips the working file.
  - Computes `fullyQualifiedDestinationFileName` using `GetDestinationFileName`.
  - When `EncryptionCode == ENCRYPT`, encrypts with PGP and appends `.pgp` to destination name.
  - When `EncryptionCode == DECRYPT`, decrypts PGP payload to `outputFileName` and uses that as upload payload.
  - Calls `SendDeliveryNotification(fileDeliverModel, sourceLocation, destination, watcher, workingFileToXfer)`.

### SendDeliveryNotification(FileToDeliverModel fileDeliverModel, SourceLocationModel sourceLocation)
- Purpose: Send owner-level success notification.
- Logic: Calls `SendNotificationOfDelivery` when notifications are enabled.

### SendDeliveryNotification(FileToDeliverModel fileDeliverModel, SourceLocationModel sourceLocation, DestinationModel dest, Stopwatch stopwatch, string workingFileToXfer)
- Purpose: Upload to one destination and emit tracking + destination notifications.
- Logic:
  - Uploads via `fileTrasferOperations.UploadFile`.
  - If upload succeeds and `dest.IsFullyUploaded` is false:
    - creates a tracking event record (`CreateFileTrackingEventRecord`).
    - sets `dest.IsFullyUploaded = true`.
    - sends destination-level success mail (`SendNotificationDestinationOfDelivery`).
    - calls `HandleIntegrationFiles`.
  - If upload fails and `dest.IsFullyUploaded` is false:
    - sets `deleteSourceFile = false` and notifies error.
  - On upload exception: sets `deleteSourceFile = false`, persists error (`UpdateFileWithExceptionAndStatus`), and notifies create-event failure (`SendCreateFileTrackingEventRecordNotification`).

### SetFileDestinations(SourceLocationModel sourceLocation)
- Purpose: Reorder destinations so `ENCRYPT` destinations are processed last.

### SetFileStatus(FileToDeliverModel fileDeliverModel)
- Purpose: Create or resume file management status.
- Logic:
  - If `FileId` is empty, calls `CreateFileManagementDataAccess` to allocate a new file id.
  - Else logs restart.

### CleanupTransferDirectories(FileToDeliverModel fileDeliverModel, string fullyQualifiedWorkingFile, SourceLocationModel sourceLocation)
- Purpose: Finalize a work item.
- Logic:
  - Deletes source file when allowed.
  - Updates status to `Completed`.
  - Sends owner-level success notification.
  - Deletes working files (including zip and encryption/decryption working copy).

### DeleteTemporalFiles(string fullyQualifiedWorkingFile)
- Purpose: Delete the working staging file(s).

### DeleteSourceFile(FileToDeliverModel fileDeliverModel, SourceLocationModel sourceLocation)
- Purpose: Delete source file only when all destinations report `IsFullyUploaded`.

### HandleIntegrationFiles(FileToDeliverModel fileDeliverModel)
- Purpose: If there is a BAS integration-file record for the file name, mark it valid and adopt its group id.
- Logic: `GetIntegrationFile` (query) then `UpdateIntegrationFile` (command).

### BuildIntegrationFileQuery(FileToDeliverModel fileDeliverModel)
- Purpose: Build `GetIntegrationFileQuery` with `TenantId` and `Where.FileName`.

### BuildIntegrationFileCommand(FileToDeliverModel fileDeliverModel, FtsIntegrationFileModel integrationFile)
- Purpose: Build `UpdateIntegrationFileCommand` (sets `IdentifierId`, `TenantId`, `ConfigId`, `FtsIntegrationFileId`, `IsValid = true`).

### UpdateFileWithExceptionAndStatus(FileToDeliverModel fileDeliverModel, Exception exceptionDetail, string status)
- Purpose: Persist error details and file status.
- Logic:
  - Updates file status.
  - Builds an `ErrorLogDetailModel` including concatenated buffered log messages.
  - Writes an exception event via `CreateFtsFileExceptionEventDataAccess`.

### CreateFileTrackingEventRecord(Guid fileId, Guid destinationId, string destinationFileName, List<DestinationModel> destinations)
- Purpose: Persist a delivery event record.

### UpdateFileTrackingRecord(Guid fileId, string status)
- Purpose: Update file management status.

### SendNotificationOfDelivery(SourceLocationModel source, FileToDeliverModel fileDeliverModel)
- Purpose: Send source-owner success email and notification event.

### SendCreateFileTrackingEventRecordNotification(SourceLocationModel source, FileToDeliverModel fileDeliverModel, string errorType, DestinationModel destination)
- Purpose: Send notification when tracking event record creation fails.

### SendNotificationOfError(SourceLocationModel source, FileToDeliverModel fileDeliverModel, string errorType, DestinationModel destination)
- Purpose: Send owner error notification including `BuildLogMessages()` output.

### SendNotificationDestinationOfDelivery(SourceLocationModel source, FileToDeliverModel fileDeliverModel, DestinationModel destination)
- Purpose: Send destination-contact success notification.

### FormatEmailBody(SourceLocationModel source, FileInfo fileInfo, FileToDeliverModel fileDeliverModel, DestinationModel destination = null)
- Purpose: Build HTML-ish email body.
- Logic:
  - Lists source and destination details and `GroupId`.
  - When a destination has `FTEMetadataId`, includes a system-process-tracking link from `BuildSTPUrl`.
  - Appends detailed logs.

### BuildSTPUrl(string groupId)
- Purpose: Build system process tracking link using `<HostName> + SPTURLPath + groupId`.

### GetDestinationsForEmailBody(SourceLocationModel source, DestinationModel destination)
- Purpose: Render destination list lines.

### DownloadWorkingFile(IFileTransferOperations fileTransferOperations, SourceLocationModel source, string fileName, bool reuseWorkFile)
- Purpose: Stage a remote file into the local working directory.
- Logic:
  - Builds working path `<WorkingDirectory>\<configId>\<fileName>`.
  - If `reuseWorkFile` and local file exists, returns it.
  - Else downloads using `fileTransferOperations.DownloadFile`.

### Log(LogCode code)
### Log(LogCode code, string message)
- Purpose: Append a log entry into the in-memory buffer.

### BuildLogMessages()
- Purpose: Build a multi-line text block from buffered log entries.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FTSConsumer.LogMessage
+Internal in-memory log-buffer entry (`Code`, `Message`).

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileTransferOperations
Protocol-selection layer that uses `IFileTransferProtocols` plus naming/zip/notification helpers.

### DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination, string tenantId)
- Purpose: Download one file from a source into the provided destination path.
- Logic:
  - Sets `TenantId`.
  - Switches on `source.SourceProtocolCode` and delegates to UNC / FTP / FTPS / SFTP (SSH-key path when `SSHAuth == true`).
  - Returns `true`.

### UploadFile(string fullyQualifiedSourceFileName, string fullyQualifiedDestinationFileName, DestinationModel destinationLocation)
- Purpose: Upload one file to a destination.
- Logic: Switches on `DestinationProtocolCode`; returns null when an FTP-like upload reports failure.

### DeleteFile(SourceLocationModel source, string fullyQualifedSourceFile)
- Purpose: Delete a file from the source.
- Logic: Switches on protocol; returns `true`.

### RenameSourceFile(SourceLocationModel source, string fullyQualifedSourceFile, string renameExtension)
- Purpose: Rename a file at the source.
- Logic: Switches on protocol; returns `true`.

### GetListOfFileNamesFromSource(SourceLocationModel location, int maxFilesPerCycle)
- Purpose: List candidate files from a source location.
- Logic: Switches on protocol and truncates the result to `maxFilesPerCycle`.

### GetCrc32(string filePath)
- Purpose: Compute CRC32 via `IFileTransferProtocols.GetCrc32`.

### GetDestinationFileName(string fullyQualifiedSourceFileName, DestinationModel destinationLocation, Guid fileId, string crc32, string uploadedSFTPUserName)
- Purpose: Compute the fully qualified destination path based on rename rules.
- Logic:
  - If `FTEMetadataId != null`, overrides destination folder to appsetting `ExternalFTECommonFolder`.
  - Supports `<DATEDIR>` directory suffix and numeric rename patterns `1..5`.
  - Supports optional `<CONST>...</CONST>` constant prefix.
  - Prepends folder with `\` for UNC and `/` otherwise.

### CreateZipFile(string inputFileName)
- Purpose: Create a `.zip` alongside the file when needed.
- Logic: Returns the input path when already `.zip`; else calls `IFileTransferProtocols.CreateZipFile`.

### SendNotification(SourceLocationModel sourceLocation, string subject, string body, bool isErrorNotification, string tenantId)
- Purpose: Send an email via the legacy notification service.

### PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId)
- Purpose: Post a notification event via the legacy notification service.

### GetFileListFromFtpSource(SourceLocationModel location)
### GetFileListFromFtpsSource(SourceLocationModel location)
### GetFileListFromSftpSource(SourceLocationModel location)
- Purpose: Build an `edtFTPnetPRO` connection and list remote files.

### ParseEnum<T>(string value)
- Purpose: Case-insensitive `Enum.Parse`.

### DownloadFileFromFtp(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
### DownloadFileFromSftp(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
### DownloadFileFromFtps(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
- Purpose: Build an `edtFTPnetPRO` connection and call `DownloadFromFtpConnection`.

### UploadFileToFtp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
### UploadFiletoSftp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
### UploadFiletoFtps(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
- Purpose: Build an `edtFTPnetPRO` connection and call `UploadToFTPServer`.

### DeleteFileFromFtp(SourceLocationModel source, string fullyQualifiedSourceFileName)
### DeleteFileFromFtps(SourceLocationModel source, string fullyQualifiedSourceFileName)
### DeleteFileFromSftp(SourceLocationModel source, string fullyQualifiedSourceFileName)
- Purpose: Build an `edtFTPnetPRO` connection and call `DeleteFromFtp`.

### RenameFileFromFtp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
### RenameFileFromFtps(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
### RenameFileFromSftp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
- Purpose: Build an `edtFTPnetPRO` connection and call `RenameFtpFile`.

### DownloadFileFromSftpUsingSSHKeys(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
### UploadFiletoSSHFtp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
### DeleteFileFromSSHFtp(SourceLocationModel source, string fullyQualifiedSourceFileName)
### RenameFileFromSSHFtp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
### GetFileListFromSSHFtpSource(SourceLocationModel location)
- Purpose: SSH-key-based SFTP operations using `Renci.SshNet`.

### BuildSftpClient(string ftpUser, string passprhase, string ftpServerIp, int ftpPort)
- Purpose: Build a `Renci.SshNet.SftpClient` configured for RSA-SHA256.
- Logic: Loads the private key (`SetPrivateKey`), sets up `ConnectionInfo`, and returns a new `SftpClient`.

### SetPrivateKey(string passprhase)
- Purpose: Load an SSH private key file from the executing assembly directory.
- Logic:
  - Uses `FileTransferManagementConstants.Paths.PathToPrivateKeyFile` relative to executing assembly `CodeBase`.
  - Uses passphrase only when the file text contains the `Encrypted` marker.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileTransferProtocols
Protocol-specific I/O and file utilities.

### DownloadFromUNCConnection(string sourceFileName, DestinationModel destination)
- Purpose: Copy a local/UNC file into the destination file path.
- Logic: `File.Copy(sourceFileName, destination.DestinationFileName)`.

### DownloadFromFtpConnection(FTPConnection remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation, string tenantId)
- Purpose: Download from an edtFTPnetPRO connection.
- Logic:
  - Connects and downloads.
  - On exception: buffers the message via `FTSConsumer.Log` and, once per config id, sends a "bad config" email.
  - Closes the connection.

### DownloadFromSSHFtpConnection(SftpClient remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation, string tenantId)
- Purpose: Download using `Renci.SshNet`.
- Logic:
  - Builds a full remote path with `\` or `/` depending on protocol code.
  - Calls `AttempToDownloadFileWithSSHFtp`.
  - Disconnects.

### UploadToUnc(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
- Purpose: Upload to UNC with `.ftsUpload` temp file semantics and duplicate-file handling.
- Logic:
  - Copies to `<dest>.ftsUpload`, applies overwrite/archive rules, then moves temp to final.

### UploadToFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, FTPConnection remoteServer)
- Purpose: Upload to FTP/FTPS/SFTP via edtFTPnetPRO using `.ftsUpload` and a rename.
- Logic:
  - Ensures destination directory exists (auto-create only for `/...`).
  - Applies overwrite rules and removes any existing `.ftsUpload`.
  - Uploads to temp only when neither temp nor final exists; then renames temp to final.
  - Closes/disposes the connection.

### UploadToSSHFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, SftpClient remoteServer)
- Purpose: Upload via `Renci.SshNet` using `.ftsUpload` then rename.
- Logic:
  - Auto-creates nested directories when destination starts with `/`.
  - Applies overwrite rules.
  - Uploads temp then renames to final.

### DeleteFromUNC(string fullyQualifiedSourceFileName)
- Purpose: Delete a local/UNC file if present.

### DeleteFromFtp(FTPConnection remoteServer, string fileName)
- Purpose: Delete remote file via edtFTPnetPRO.

### DeleteFromSFtp(SftpClient remoteServer, string fileName)
- Purpose: Delete remote file via `Renci.SshNet`.

### RenameFileFromUNC(string fullyQualifiedSourceFileName, string renameExtension)
- Purpose: Rename local file by appending `renameExtension`, adding `.<i>` while collisions exist.

### RenameFtpFile(string fullyQualifiedSourceFileName, string renameExtension, FTPConnection remoteServer)
- Purpose: Rename remote file by appending `renameExtension`, adding `.<i>` while collisions exist.

### RenameSSHFtpFile(string fullyQualifiedSourceFileName, string renameExtension, SftpClient remoteServer)
- Purpose: Rename remote file via `Renci.SshNet` using the same `.<i>` deconfliction scheme.

### GetFileListFromUncSource(SourceLocationModel location)
- Purpose: List local files using `Directory.GetFiles(location.SourceAddress, location.FileMask)`.

### GetFilesFromSource(FTPConnection remoteServer, SourceLocationModel location)
- Purpose: List remote files via edtFTPnetPRO and filter with `patternMatcher.StrictMatchPattern(location.FileMask, f.Name)`.

### GetFilesFromSSHSource(SftpClient remoteServer, SourceLocationModel location)
- Purpose: List remote files via `Renci.SshNet` and filter with `StrictMatchPattern`.

### CreateZipFile(string inputFileName, string zipFileName, FileInfo fileInfo)
- Purpose: Create a zip file with one entry.

### GetCrc32(string filePath)
- Purpose: Compute CRC32 using SharpZipLib and return uppercase hex.

### SendNotificationOfBadConfig(SourceLocationModel sourceLocation, string tenantId)
- Purpose: Send a "bad config" email via `NotificationServiceHelper`.

### IsAValidDestination(string destinationAddress)
- Purpose: Validate destination root exists.

### CloseRemoteServerConnection(FTPConnection remoteServer)
- Purpose: Dispose/close FTP connection.

### IsFileFullyUploaded(FTPConnection remoteServer, string uploadFileName)
- Purpose: Consider upload complete when the `.ftsUpload` name no longer exists.

### AttempToDownloadFileWithSSHFtp(SourceLocationModel sourceLocation, DestinationModel destinationLocation, string tenantId, string sourceFullFilePath, SftpClient sftpClient)
- Purpose: Download via SSH and record bad-config notifications.

### TryToAutoCreateDirectoryIfDestinationLocationDoesNotExists(DestinationModel destinationLocation, SftpClient remoteServer)
- Purpose: Auto-create nested directories for `/...` SFTP destinations.

### HandleOverwriteAtDestination(string fullyQualifiedDestinationFile, DestinationModel destinationLocation, SftpClient remoteServer)
- Purpose: Apply overwrite/archive rules for SFTP destinations.

### TryUploadFileToSFTP(FTPConnection remoteServer, string fullyQualifiedSourceFile, string uploadFileName, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
- Purpose: Upload only when neither temp nor final exists; otherwise sets `IsFullyUploaded = true` and returns false.

### FileAndTemporalFileExistsOnRemoteServer(FTPConnection remoteServer, string uploadFileName, string fullyQualifiedDestinationFile)
- Purpose: Predicate used by `TryUploadFileToSFTP`.

### RenameUploadedFile(FTPConnection remoteServer, string uploadFileName, string fullyQualifiedDestinationFile)
- Purpose: Rename temp to final and verify the temp no longer exists.

### RemoveTemporaryUploadFile(FTPConnection remoteServer, string uploadFileName)
- Purpose: Delete `.ftsUpload` when present.

### CheckAndHandleFileOverwrite(FTPConnection remoteServer, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
- Purpose: Apply overwrite/archive rules for FTP-like destinations.

### CreateRemoteDirectoryIfNotExists(FTPConnection remoteServer, string destinationAddress)
- Purpose: Auto-create a remote directory only when the path begins with `/`.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.PatternMatcher
+Pattern-matching implementation used when filtering remote file listings.
+
+### StrictMatchPattern(string expression, string name)
+- Purpose: Return true when `name` matches `expression` with strict DOS wildcard semantics.
+- Logic: Port of the Windows/.NET strict match implementation; supports `*`, `?`, and DOS-specific wildcard handling around the final `.`.
+
+## Rules implemented
+
+None (this component owns no business rules).
+
+## Not carried over
+
+None.
+
