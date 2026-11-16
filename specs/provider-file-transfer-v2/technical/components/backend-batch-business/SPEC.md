---
type: Behavior Specification
title: technical/components/backend-batch-business/SPEC
description: 'What the backend-batch-business component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-batch-business
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T00:36:41Z
sources:
- id: inventory
  resource: /technical/components/backend-batch-business/INVENTORY.yaml
  title: Every legacy class and method (19 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-batch-business
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 19
  methods: 78
  owned_rules: 0
  flows: []
  depends_on:
  - backend-business-helpers
  deployable: fxfer-transfer-worker
  folder: src/Application
  body_sha256: 1566569bc3755b9242348d82898c8daf893bcc55a858f2fb6cbf2a621519d6d9
---

﻿# backend-batch-business: Batch transfer execution (discover → stage → deliver → track → notify)

This component implements the legacy batch worker loop for file delivery.

- `FTSProducer` enumerates eligible source locations and queues `FileToDeliverModel` items.
- `FTSConsumer` drains the queue, stages a working copy, optionally zips and/or PGP encrypts/decrypts, uploads to each destination, updates tracking, deletes the source file, and sends notifications.

It depends on appsettings keys in `AppSettingsConstants`, uses protocol libraries (`edtFTPnetPRO`, SharpZipLib), and persists tracking/notifications through legacy batch data access surfaces.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.AppSettingsConstants
Static app.config key names used by the batch runtime.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.BatchJobConstants
Static command/identifier constants used by batch execution.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.ConsumerFactory
Creates an `FTSConsumer` for a given consumer identifier.

### CreateConsumer(int consumerIdentifier)
- Purpose: Return a new consumer instance for `consumerIdentifier`.
- Inputs: `consumerIdentifier`.
- Logic: `return new FTSConsumer(consumerIdentifier);`.
- Data: None.
- Errors: None.
- Rules: None.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.ExceptionConstants
Static batch exception metadata constants.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.FTSConsumer
Consumes queued `FileToDeliverModel` items and executes delivery (download → transform → upload → track → notify).

### Log(string message)
- Purpose: Append an informational message to the shared in-memory log list.
- Inputs: `message`.
- Logic: Adds a log entry with `Code = Info` and the given message.
- Data: None.
- Errors: None.
- Rules: None.

### ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue)
- Purpose: Drain the queue until complete and deliver each item.
- Inputs: `filesToDeliverQueue`.
- Logic:
  - While queue not completed, `Take()` the next item when `Count > 0` and call `DeliverFile`.
  - On `DuplicateFileNameException`:
    - If `ActivateDuplicateRenameLogic` is true: rename the source with `DuplicateFileExtension`, mark status `Completed`, send "Duplicate" notification.
    - Else: mark status `Error`, send "Error" notification.
  - On any other exception: mark status `Error`, send "Error" notification.
  - Finally prints each log entry to console prefixed with `ftsProcessId`.
- Data: Writes status + error log rows when `FileId` is known.
- Errors: Exceptions are caught and converted into status + notification.
- Rules: None.

### DeliverFile(FileToDeliverModel fileDeliverModel)
- Purpose: End-to-end processing for a single file.
- Inputs: `fileDeliverModel` (source location, file name, tracking ids).
- Logic:
  - If `FileId == Guid.Empty`, create a tracking record and set `fileDeliverModel.FileId` from returned key.
  - Stage a local working copy via `DownloadWorkingFile`, then compute `crc32`.
  - Load destination metadata for `fileDeliverModel.FileId` and reorder destinations to process `EncryptionCode == "ENCRYPT"` last.
  - For each destination:
    - If `IsCompressAtDestination`, create a `.zip` working file.
    - Compute the destination path via `GetDestinationFileName`.
    - If `EncryptionCode == "ENCRYPT"`: call `EncryptionHelper.EncryptPgpFile(workingFileToXfer, workingFileToXfer, ...)` and append `.pgp` to the destination filename when `result == 1`.
    - If `EncryptionCode == "DECRYPT"`: write decrypted bytes to `outputFileName = workingFileToXfer.Replace(".pgp", "")` and switch the upload input to that file when `DecryptPgpData(...)` returns whitespace-only.
    - Upload using `UploadFile(...)`.
    - Create a per-destination delivery event record via `CreateFileTrackingEventRecord`.
  - Delete the source file, mark status `Completed`, send success notification, and delete staged working files.
- Data: Creates/updates tracking state and delivery event records; uses `WorkingDirectory` for staged files.
- Errors: Exceptions propagate to `ProcessDeliveryQueue`.
- Rules: None.

### UpdateFileWithExceptionAndStatus(FileToDeliverModel fileDeliverModel, Exception exceptionDetail, string status)
- Purpose: Persist failure detail and set file status.
- Inputs: `fileDeliverModel`, `exceptionDetail`, `status`.
- Logic: Concatenates all log entries into `ErrorText` and writes status + error log when `FileId != Guid.Empty`.
- Data: Writes file management status and error log.
- Errors: Data-access errors propagate.
- Rules: None.

### CreateFileTrackingEventRecord(Guid fileId, Guid destinationId, string destinationFileName)
- Purpose: Record delivery of a file to one destination.
- Inputs: `fileId`, `destinationId`, `destinationFileName`.
- Logic: Logs and calls `CreateFtsFileDeliveryDataAccess(fileId, destinationId, destinationFileName, "Operator ID").Execute()`.
- Data: Writes delivery event record.
- Errors: Data-access errors propagate.
- Rules: None.

### UpdateFileTrackingRecord(Guid fileId, string status)
- Purpose: Persist a status update.
- Inputs: `fileId`, `status`.
- Logic: Logs and calls `CreateUpdateFileManagementStatusDataAccess(fileId, status).Execute()`.
- Data: Writes file management status.
- Errors: Data-access errors propagate.
- Rules: None.

### SendNotificationOfDelivery(SourceLocationModel source, FileToDeliverModel fileDeliverModel)
- Purpose: Send a success notification after all destinations are processed.
- Inputs: `source`, `fileDeliverModel`.
- Logic:
  - No-op when `SendNotificationsIndicator` is false or `DisableNotifications` is true.
  - Subject: `<EnvironmentName>: File Delivery Success: <filename>`.
  - Body: source line + one destination line per destination + `LogMessages(false)`.
  - Sends email via `IFileTransferOperations.SendNotification(..., isErrorNotification: false)` and posts an event via `INotificationService.PostEvent(...)`.
- Data: Reads notification recipients via `SendNotification` implementation.
- Errors: Proxy errors propagate.
- Rules: None.

### SendNotificationOfError(SourceLocationModel source, FileToDeliverModel fileDeliverModel, string errorType)
- Purpose: Send a failure notification.
- Inputs: `source`, `fileDeliverModel`, `errorType` (legacy: `Duplicate` or `Error`).
- Logic:
  - No-op when `SendNotificationsIndicator` is false or `DisableNotifications` is true.
  - Subject: `<EnvironmentName>: File Delivery <errorType>: <filename>`.
  - Body includes `LogMessages(false)`.
  - Sends email with `isErrorNotification: true` and posts an event.
- Data: Reads notification recipients via `SendNotification` implementation.
- Errors: Proxy errors propagate.
- Rules: None.

### DownloadWorkingFile(IFileTransferOperations fileTransferOperations, SourceLocationModel source, string fileName, bool reuseWorkFile)
- Purpose: Download a working copy to `<WorkingDirectory>\\<configId>\\<basename>`.
- Inputs: `fileTransferOperations`, `source`, `fileName`, `reuseWorkFile`.
- Logic:
  - Ensures working directory exists.
  - When `reuseWorkFile == true` and the working copy exists, returns it.
  - Otherwise downloads via `fileTransferOperations.DownloadFile(...)`.
  - On exception, logs `Info` and returns the working file path.
- Data: Writes a file in the working directory.
- Errors: Download exceptions are swallowed.
- Rules: None.

### Log(LogCode code)
- Purpose: Append a log entry with empty message.
- Inputs: `code`.
- Logic: Adds `{ Code = code, Message = string.Empty }`.
- Data: None.
- Errors: None.
- Rules: None.

### Log(LogCode code, string message)
- Purpose: Append a log entry.
- Inputs: `code`, `message`.
- Logic: Adds `{ Code = code, Message = message }`.
- Data: None.
- Errors: None.
- Rules: None.

### LogMessages(bool includeConsumerId)
- Purpose: Render log entries to a string.
- Inputs: `includeConsumerId` (unused in legacy).
- Logic: Returns lines of `<Code>: <Message>\\r\\n`.
- Data: None.
- Errors: None.
- Rules: None.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.FTSConsumer.LogMessage
Private nested log-entry type.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.FTSProducer
Lists source files and enqueues new or retry-eligible pending files.

### ProcessAllFilesSources()
- Purpose: Queue files from all scheduled and eligible source locations.
- Inputs: None.
- Logic: Reads pending files + source locations; for each location, if `IsSourceSetToRun(location)` and `location.IsScheduledToRun`, calls `ProcessAllSourceFilesInLocation`.
- Data: Reads pending list and location list.
- Errors: Location errors handled by `HandleLocationException`.
- Rules: None.

### ProcessFailedTransfers(int? topXFailedTransfers, Guid? specificFileId, int? specificConfigId)
- Purpose: Requeue failed transfers based on criteria.
- Inputs: optional `topXFailedTransfers`, `specificFileId`, `specificConfigId`.
- Logic: Orders pending list by `StatusDate.Date`, prints last 50, chooses candidates via `GetPendingFilesToReProcess`, then calls `ProcessAllPendingFiles`.
- Data: Updates file status to `Transfering` for requeued items.
- Errors: Per-file exceptions are caught and printed.
- Rules: None.

### ProcessAllSourceFilesInLocation(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location)
- Purpose: Queue files from one location.
- Inputs: queue, pending list, location.
- Logic: Calls `AddSourceFilesToQueue` and, if it throws, calls `HandleLocationException`.
- Data: Reads source listing.
- Errors: Captures per-location exception and suspends location.
- Rules: None.

### AddSourceFilesToQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location)
- Purpose: List source files and enqueue each new or retry-eligible pending file.
- Inputs: queue, pending list, location.
- Logic: Lists up to `maxFilesPerCycle` files and skips those with the duplicate extension when duplicate rename logic is active.
- Data: Reads source listing.
- Errors: Propagates to caller.
- Rules: None.

### CheckAndAddPendingFile(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<PendingFileModel> pendingFiles, SourceLocationModel location, FileInfo fileInfo, string file)
- Purpose: Select between new-file enqueue and pending-file retry enqueue.
- Inputs: queue, pending list, location, file info, file.
- Logic: Finds pending row by `(SourceLocationId, FileName)`; calls `AddNewFileToDeliveryQueue` when none, else `AddPendingFileToDeliveryQueue`.
- Data: Reads pending list.
- Errors: None.
- Rules: None.

### AddNewFileToDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, SourceLocationModel location, string file)
- Purpose: Enqueue a new file.
- Inputs: queue, location, file.
- Logic: If `PreviousConfigurationId != 0`, parses a leading int before `_` from the filename into `SakFileTrack`.
- Data: Enqueues a `FileToDeliverModel`.
- Errors: None.
- Rules: None.

### AddPendingFileToDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, SourceLocationModel location, PendingFileModel pendingFile, string file)
- Purpose: Enqueue a pending file only after retry wait window.
- Inputs: queue, location, pending row, file.
- Logic: When `pendingFile.StatusDate + minutesBetweenRetry <= now`, updates status to `Transfering` and enqueues with existing `FileId` and `FileTrackId`.
- Data: Writes status update; enqueues delivery model.
- Errors: Data-access errors propagate.
- Rules: None.

### HandleLocationException(SourceLocationModel location, Exception ex)
- Purpose: Persist suspension info and optionally notify when a location cannot be accessed.
- Inputs: `location`, `ex`.
- Logic: Writes console diagnostics, persists a suspension row, and (when notifications are enabled) calls `SendNotification` with subject `"FTS: <environmentName> location <configId> has been suspended"`.
- Data: Writes suspension row.
- Errors: Swallows any exception inside this handler.
- Rules: None.

### DumpErrorToConsole(SourceLocationModel location, Exception ex)
- Purpose: Write the legacy diagnostic block to console.
- Inputs: `location`, `ex`.
- Logic: Prints server details and exception messages.
- Data: None.
- Errors: None.
- Rules: None.

### GetPendingFilesByStatusDate()
- Purpose: Load pending list ordered by `StatusDate.Date`.
- Inputs: None.
- Logic: Reads pending list and orders ascending by date.
- Data: Reads pending list.
- Errors: Data-access errors propagate.
- Rules: None.

### PrintLast50FileStatus(List<PendingFileModel> pendingFiles)
- Purpose: Print up to 50 non-completed pending rows.
- Inputs: pending list.
- Logic: Prints `<FileTrackId>;<FileId>: <StatusDate> <FileStatus> <FileName>`.
- Data: None.
- Errors: None.
- Rules: None.

### GetPendingFilesToReProcess(List<PendingFileModel> pendingFilesList, Guid? specificSakFileTracking, int? specificConfigId, int? topXFailedTransfers)
- Purpose: Choose which pending rows to requeue.
- Inputs: pending list and selection criteria.
- Logic:
  - `specificSakFileTracking`: match by `FileId`.
  - `specificConfigId`: match by `ConfigId`.
  - `topXFailedTransfers`: take first N non-completed rows.
- Data: None.
- Errors: None.
- Rules: None.

### ProcessAllPendingFiles(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, List<SourceLocationModel> sourceLocationList, List<PendingFileModel> filesToProcess)
- Purpose: Verify source existence and enqueue restart work.
- Inputs: queue, locations, pending rows.
- Logic: For each pending row, finds the location, verifies the file still exists at source, updates status to `Transfering` and enqueues; if missing, marks `Completed` and writes an error log entry of type `RestartFileIssue`.
- Data: Writes status updates and error log rows.
- Errors: Per-file exceptions are caught and printed.
- Rules: None.

### IsSourceSetToRun(SourceLocationModel sourceModel)
- Purpose: Determine if a source is runnable.
- Inputs: source model.
- Logic: Active + not suspended → true; suspended + `SuspendedDate <= Now + MinsToWaitBeforeRetrySuspendedConfig` → unsuspend via data access and true.
- Data: May update suspension status.
- Errors: Data-access errors propagate.
- Rules: None.

### InitializeAppsettings()
- Purpose: Read appsettings into producer fields.
- Inputs: None.
- Logic: Reads `ActivateDuplicateRenameLogic`, `DuplicateFileExtension`, `MaxFilesPerCycle`, `MinsToWaitBeforeRetryFailedFile`, `DisableNotifications`, `EnvironmentName`.
- Data: Reads configuration.
- Errors: Format errors propagate.
- Rules: None.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.FileTransferBatchExecutor
CLI/batch entry point coordinating producer + consumer processing.

### Execute(string[] inputArgs)
- Purpose: Run the batch execution.
- Inputs: `inputArgs`.
- Logic: Validate, populate queue, start consumer tasks, wait; returns `0` on success and `-1` when validation fails.
- Data: Reads appsettings; delegates to producer/consumer.
- Errors: Missing appsettings throw `ConfigurationErrorsException` during validation.
- Rules: None.

### Validate(string[] args)
- Purpose: Validate input arguments and ensure required appsettings keys exist.
- Inputs: `args`.
- Logic: `ValidateInputArguments` then `ValidateAppConfig`.
- Data: Reads appsettings.
- Errors: Configuration errors throw.
- Rules: None.

### ValidateInputArguments(string[] args)
- Purpose: Validate CLI usage.
- Inputs: `args`.
- Logic: allows empty args; otherwise requires exactly 2 args and `args[1]` to be int or guid.
- Data: Console output.
- Errors: None.
- Rules: None.

### ValidateAppConfig()
- Purpose: Require presence of all keys used by the job.
- Inputs: None.
- Logic: Checks all keys listed in `FileTransferBatchExecutor.ValidateAppConfig`.
- Data: Reads appsettings.
- Errors: Throws `ConfigurationErrorsException` for missing keys.
- Rules: None.

### CheckAndThrowConfigurationException(string key)
- Purpose: Throw when `AppSettings[key] == null`.
- Inputs: key.
- Logic: Throws with message `"{0} is not specified in app.config"`.
- Data: None.
- Errors: Throws.
- Rules: None.

### AddFilesToDeliveryQueue(string[] arguments)
- Purpose: Select producer mode and populate queue.
- Inputs: CLI args.
- Logic: When no criteria are parsed, runs `ProcessAllFilesSources`; otherwise runs `ProcessFailedTransfers`.
- Data: Console output.
- Errors: None.
- Rules: None.

### ProcessFilesInDeliverQueue()
- Purpose: Create consumers, complete adding, and wait.
- Inputs: None.
- Logic: Calls `CreateConsumerTasks`, then `CompleteAdding()` when queue has items, then `WaitForCompletion`.
- Data: None.
- Errors: None.
- Rules: None.

### CreateConsumerTasks()
- Purpose: Start N consumers.
- Inputs: None.
- Logic: Parses `MaxConcurrentDeliveryProcesses`, creates that many tasks, each calling `consumer.ProcessDeliveryQueue(queue)`.
- Data: Reads appsettings.
- Errors: None.
- Rules: None.

### WaitForCompletion(Task[] consumers)
- Purpose: Wait for all consumers.
- Inputs: task array.
- Logic: `Task.WaitAll`; on `AggregateException` prints each inner exception message.
- Data: Console output.
- Errors: AggregateException swallowed.
- Rules: None.

### ParseInputArguments(string[] arguments, out Guid? sakTrack, out int? topXFailures, out int? configId)
- Purpose: Parse special-mode args.
- Inputs: args.
- Logic: Supports `sakTrack <guid>`, `configId <int>`, `top <int>`; otherwise prints usage.
- Data: Console output.
- Errors: None.
- Rules: None.

### GetCommandValue(string commandDataString)
- Purpose: Parse an int value.
- Inputs: string.
- Logic: `int.TryParse` and returns the parsed value (0 on failure).
- Data: None.
- Errors: None.
- Rules: None.

### PrintUsage()
- Purpose: Print the legacy usage block.
- Inputs: None.
- Logic: Writes legacy lines starting with `Usage: FileTransferService <command> <command_value>`.
- Data: Console output.
- Errors: None.
- Rules: None.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.FileTransferOperations
Routes file operations to the correct protocol implementation.

### DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination)
- Purpose: Download a file from the source endpoint to a destination file.
- Inputs: `source`, `sourceFileName`, `destination`.
- Logic: Switches on `SourceProtocolCode` (UNC/FTP/SFTP/FTPS) and delegates.
- Data: Reads external endpoint; writes destination file.
- Errors: Underlying protocol errors propagate.
- Rules: None.

### UploadFile(string fullyQualifiedSourceFileName, string fullyQualifiedDestinationFileName, DestinationModel destinationLocation)
- Purpose: Upload a file using destination protocol.
- Inputs: file paths + destination model.
- Logic: Switches on `DestinationProtocolCode` (UNC/FTP/SFTP/FTPS) and delegates.
- Data: Writes destination.
- Errors: Underlying protocol errors propagate.
- Rules: None.

### DeleteFile(SourceLocationModel source, string fullyQualifedSourceFile)
- Purpose: Delete a file from a source endpoint.
- Inputs: source model + file name.
- Logic: Switches on protocol and delegates.
- Data: Deletes source.
- Errors: Underlying errors propagate.
- Rules: None.

### RenameSourceFile(SourceLocationModel source, string fullyQualifedSourceFile, string renameExtension)
- Purpose: Rename a source file using protocol-specific implementation.
- Inputs: source model + file name + extension.
- Logic: Switches on protocol and delegates.
- Data: Renames source.
- Errors: Underlying errors propagate.
- Rules: None.

### GetListOfFileNamesFromSource(SourceLocationModel location, int maxFilesPerCycle)
- Purpose: List candidate files, bounded by `maxFilesPerCycle`.
- Inputs: location + max.
- Logic: Delegates by protocol, then truncates when needed.
- Data: Reads listing.
- Errors: Underlying errors propagate.
- Rules: None.

### GetCrc32(string filePath)
- Purpose: Delegate CRC32 calculation.
- Inputs: file path.
- Logic: Calls `IFileTransferProtocols.GetCrc32(filePath)`.
- Data: Reads file.
- Errors: Propagates.
- Rules: None.

### GetDestinationFileName(string fullyQualifiedSourceFileName, DestinationModel destinationLocation, Guid fileId, string crc32, string uploadedSFTPUserName)
- Purpose: Compute final destination path/name from rename pattern and destination address.
- Inputs: source file, destination model, `fileId`, `crc32`, `uploadedSFTPUserName`.
- Logic:
  - Expands `<DATEDIR>` into `DestinationAddress` using `\\` for UNC and `/` otherwise.
  - When `IsRenameAtDestination` is true, supports rename patterns: `ORIG_<DATE>_<TIME>`, `<SAK>_<CRC32>_ORIG`, `<USER>_<SAK>_<CRC32>_ORIG`, `<SAK>_ORIG`, `<FILEID>_ORIG`.
  - Prefixes `DestinationAddress` and joins with `\\` (UNC) or `/`.
- Data: Mutates `destinationLocation.DestinationAddress` and `RenamePattern` in-memory.
- Errors: None.
- Rules: None.

### CreateZipFile(string inputFileName)
- Purpose: Create a zip file when needed.
- Inputs: input file.
- Logic: When already `.zip`, returns input; otherwise returns `<input>.zip` (creating it only when missing).
- Data: Writes zip file.
- Errors: IO errors propagate.
- Rules: None.

### SendNotification(long configId, string subject, string body, bool isErrorNotification)
- Purpose: Send email to configured recipients.
- Inputs: config id + email subject/body + error flag.
- Logic: Filters recipients by `NotifyOnErrorInd` or `NotifyOnSuccessInd` and sends email via `NotificationServiceClient.SendEmail`.
- Data: Reads notifications table; sends email.
- Errors: Proxy errors propagate.
- Rules: None.

### GetFileListFromFtpSource(SourceLocationModel location)
- Purpose: FTP listing helper.
- Inputs: location.
- Logic: Creates `FTPConnection` from `FtpServerDetail` and calls `GetFilesFromSource`.
- Data: Reads remote listing.
- Errors: Propagates.
- Rules: None.

### GetFileListFromFtpsSource(SourceLocationModel location)
- Purpose: FTPS listing helper.
- Inputs: location.
- Logic: Uses `SecureFTPConnection` with `Protocol = FTPSImplicit`, port `990`, `PASV`.
- Data: Reads remote listing.
- Errors: Propagates.
- Rules: None.

### GetFileListFromSftpSource(SourceLocationModel location)
- Purpose: SFTP listing helper.
- Inputs: location.
- Logic: Uses `SecureFTPConnection` with `Protocol = SFTP` and `SSHDisableChmodAfterPut`.
- Data: Reads remote listing.
- Errors: Propagates.
- Rules: None.

### DownloadFileFromFtp(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
- Purpose: Download from FTP.
- Inputs: source, remote filename, destination.
- Logic: Creates `FTPConnection` with port `21` and calls `DownloadFromFtpConnection`.
- Data: Writes destination.
- Errors: Exceptions are handled in `FileTransferProtocols.DownloadFromFtpConnection`.
- Rules: None.

### DownloadFileFromSftp(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
- Purpose: Download from SFTP.
- Inputs: source, remote filename, destination.
- Logic: Creates `SecureFTPConnection` with port `22`, `Protocol = SFTP`, and `SSHDisableChmodAfterPut` and calls `DownloadFromFtpConnection`.
- Data: Writes destination.
- Errors: Handled in protocol layer.
- Rules: None.

### DownloadFileFromFtps(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
- Purpose: Download from FTPS.
- Inputs: source, remote filename, destination.
- Logic: Creates `SecureFTPConnection` with port `990`, `Protocol = FTPSImplicit` and calls `DownloadFromFtpConnection`.
- Data: Writes destination.
- Errors: Handled in protocol layer.
- Rules: None.

### UploadFileToFtp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
- Purpose: Upload to FTP.
- Inputs: local source, remote destination, destination config.
- Logic: Creates `FTPConnection` with port `21` and calls `UploadToFTPServer`.
- Data: Writes remote file.
- Errors: Propagates.
- Rules: None.

### UploadFiletoSftp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
- Purpose: Upload to SFTP.
- Inputs: local source, remote destination, destination config.
- Logic: Creates `SecureFTPConnection` with port `22`, protocol SFTP and compatibility flag; calls `UploadToFTPServer`.
- Data: Writes remote file.
- Errors: Propagates.
- Rules: None.

### UploadFiletoFtps(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
- Purpose: Upload to FTPS.
- Inputs: local source, remote destination, destination config.
- Logic: Creates `SecureFTPConnection` with port `990`, protocol FTPSImplicit; calls `UploadToFTPServer`.
- Data: Writes remote file.
- Errors: Propagates.
- Rules: None.

### DeleteFileFromFtp(SourceLocationModel source, string fullyQualifiedSourceFileName)
- Purpose: Delete from FTP.
- Inputs: source + remote filename.
- Logic: Creates `FTPConnection` with port `21` and calls `DeleteFromFtp`.
- Data: Deletes remote file.
- Errors: Propagates.
- Rules: None.

### DeleteFileFromFtps(SourceLocationModel source, string fullyQualifiedSourceFileName)
- Purpose: Delete from FTPS.
- Inputs: source + remote filename.
- Logic: Creates `SecureFTPConnection` with port `990`, FTPSImplicit; calls `DeleteFromFtp`.
- Data: Deletes remote file.
- Errors: Propagates.
- Rules: None.

### DeleteFileFromSftp(SourceLocationModel source, string fullyQualifiedSourceFileName)
- Purpose: Delete from SFTP.
- Inputs: source + remote filename.
- Logic: Creates `SecureFTPConnection` with port `22`, SFTP, compatibility flag; calls `DeleteFromFtp`.
- Data: Deletes remote file.
- Errors: Propagates.
- Rules: None.

### RenameFileFromFtp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
- Purpose: Rename on FTP.
- Inputs: source, remote filename, extension.
- Logic: Creates `FTPConnection` with port `21` and calls `RenameFtpFile`.
- Data: Renames remote file.
- Errors: Propagates.
- Rules: None.

### RenameFileFromFtps(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
- Purpose: Rename on FTPS.
- Inputs: source, remote filename, extension.
- Logic: Creates `SecureFTPConnection` with port `990`, FTPSImplicit and calls `RenameFtpFile`.
- Data: Renames remote file.
- Errors: Propagates.
- Rules: None.

### RenameFileFromSftp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
- Purpose: Rename on SFTP.
- Inputs: source, remote filename, extension.
- Logic: Creates `SecureFTPConnection` with port `22`, SFTP, compatibility flag and calls `RenameFtpFile`.
- Data: Renames remote file.
- Errors: Propagates.
- Rules: None.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.FileTransferProtocols
Protocol primitives and file utilities.

### DownloadFromUNCConnection(string sourceFileName, DestinationModel destination)
- Purpose: Copy a file to `destination.DestinationFileName`.
- Inputs: source path, destination model.
- Logic: `File.Copy(source, destinationFileName)`.
- Data: Filesystem read/write.
- Errors: IO errors propagate.
- Rules: None.

### DownloadFromFtpConnection(FTPConnection remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
- Purpose: Download one file from FTP/SFTP/FTPS connection.
- Inputs: preconfigured server connection, source location, remote filename, destination model.
- Logic:
  - Connects and downloads to `destinationLocation.DestinationFileName`.
  - On exception: logs the exception message via `FTSConsumer.Log` and (first time per config id) calls `SendNotificationOfBadConfig`.
  - Closes connection.
- Data: Writes local destination file.
- Errors: Download exceptions swallowed after logging.
- Rules: None.

### UploadToUnc(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
- Purpose: Copy a file to a UNC/local destination using temp `.ftsUpload` then rename.
- Inputs: source path, destination path, destination model.
- Logic:
  - Ensures destination directory exists.
  - Copies source to `<dest>.ftsUpload`.
  - If dest exists: deletes it only when overwrite is allowed OR (archive + `DeleteArchiveIfExists` enabled); otherwise throws `DuplicateFileNameException`.
  - Moves temp to final dest.
- Data: Filesystem writes.
- Errors: Throws `DuplicateFileNameException` when overwrite is not permitted.
- Rules: None.

### UploadToFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, FTPConnection remoteServer)
- Purpose: Upload using temp `.ftsUpload` then rename.
- Inputs: source path, destination path, destination config, connection.
- Logic:
  - Connects.
  - Ensures remote destination directory exists (auto-creates only when path starts with `/`).
  - Deletes existing destination file when overwrite is enabled OR (archive + `DeleteArchiveIfExists`).
  - Deletes any existing temp upload file.
  - Uploads to temp then renames to final.
  - Closes.
- Data: Remote writes.
- Errors: Directory creation throws when destination path does not start with `/`.
- Rules: None.

### DeleteFromUNC(string fullyQualifiedSourceFileName)
- Purpose: Delete a local file when present.
- Inputs: file path.
- Logic: Deletes when `File.Exists`.
- Data: Filesystem delete.
- Errors: IO errors propagate.
- Rules: None.

### DeleteFromFtp(FTPConnection remoteServer, string fileName)
- Purpose: Delete a remote file.
- Inputs: connection + file name.
- Logic: Connect → delete → close.
- Data: Remote delete.
- Errors: Propagates.
- Rules: None.

### RenameFileFromUNC(string fullyQualifiedSourceFileName, string renameExtension)
- Purpose: Rename a file by appending `renameExtension`, adding `.N` suffix when needed.
- Inputs: file path + extension.
- Logic: Uses the first available name among `<file><ext>`, `<file>.1<ext>`, ...
- Data: Filesystem rename.
- Errors: IO errors propagate.
- Rules: None.

### RenameFtpFile(string fullyQualifiedSourceFileName, string renameExtension, FTPConnection remoteServer)
- Purpose: Rename remote file to an available name using the same suffixing approach as UNC.
- Inputs: remote filename + extension + connection.
- Logic: Uses `remoteServer.Exists` to choose a unique target, then renames.
- Data: Remote rename.
- Errors: Propagates.
- Rules: None.

### GetFileListFromUncSource(SourceLocationModel location)
- Purpose: List local files matching `location.FileMask`.
- Inputs: location.
- Logic: `Directory.GetFiles(location.SourceAddress, location.FileMask)`.
- Data: Filesystem read.
- Errors: Propagates.
- Rules: None.

### GetFilesFromSource(FTPConnection remoteServer, SourceLocationModel location)
- Purpose: List remote files under `location.SourceAddress` filtered by strict pattern match.
- Inputs: connection, location.
- Logic: Gets `FTPFile` entries, filters `Dir == false` and `patternMatcher.StrictMatchPattern(location.FileMask, f.Name)`.
- Data: Remote listing.
- Errors: Propagates.
- Rules: None.

### CreateZipFile(string inputFileName, string zipFileName, FileInfo fileInfo)
- Purpose: Create a zip archive containing one entry.
- Inputs: input file path, zip path, file info.
- Logic: Uses `ZipArchive.CreateEntryFromFile(inputFileName, fileInfo.Name)`.
- Data: Writes zip file.
- Errors: Propagates.
- Rules: None.

### GetCrc32(string filePath)
- Purpose: Compute CRC32 as uppercase hex.
- Inputs: file path.
- Logic: Reads in 1MB chunks (`1048576`), updates SharpZipLib `Crc32`, formats hex and uppercases.
- Data: Reads file.
- Errors: IO errors propagate.
- Rules: None.

### SendNotificationOfBadConfig(long configId)
- Purpose: Notify recipients when a config id causes a download failure.
- Inputs: failing `configId`.
- Logic: Builds a fixed subject/body, overwrites `configId` to `911` to query recipients, then sends email.
- Data: Reads notification recipients and sends email.
- Errors: Propagates.
- Rules: None.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.IBatchExecutor<Toutput>
Generic batch executor interface.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.IConsumerFactory
Consumer factory interface.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.IFTSConsumer
Consumer interface.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.IFTSProducer
Producer interface.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.IFileTransferOperations
File-transfer operations interface.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.IFileTransferProtocols
Protocol primitives interface.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.IPatternMatcher
Strict file mask matcher interface.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.IocConstants
Unity registration key constants.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.PatternMatcher
Strict wildcard matcher ported from .NET libraries for FTP listing consistency.

### StrictMatchPattern(string expression, string name)
- Purpose: Determine whether a filename matches the mask expression.
- Inputs: `expression` (may contain wildcards) and `name`.
- Logic: Implements strict wildcard semantics (`*`, `?`, and DOS wildcard variants) using a state-machine matcher.
- Data: None.
- Errors: None.
- Rules: None.

## Rules implemented

None (this component owns no business rules).

## Not carried over

None.
