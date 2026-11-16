---
type: Target Placement
title: technical/components/backend-batch-business/TARGET
description: Where the backend-batch-business component goes in the new system and what to build, method by method.
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
  body_sha256: 3d794252ae4f1e4715ff2efa92e136219a96e074a332967b791cf9f6d1dd8e87
---

﻿# Target: `backend-batch-business`

## Where

- Deployable: `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`)
- Folder: `workers/fxfer-transfer-worker/src/Application`

## What to create

Create the worker-side application-layer batch transfer execution logic:

- Constants: `AppSettingsConstants`, `BatchJobConstants`, `ExceptionConstants`, `IocConstants`.
- Batch entry: `FileTransferBatchExecutor : IBatchExecutor<int>`.
- Work discovery: `FTSProducer : IFTSProducer`.
- Work execution: `FTSConsumer : IFTSConsumer` + `ConsumerFactory : IConsumerFactory`.
- Protocol operations: `FileTransferOperations : IFileTransferOperations` and `FileTransferProtocols : IFileTransferProtocols`.
- Pattern matching: `PatternMatcher : IPatternMatcher`.

Implementation notes:

- Preserve legacy behavior exactly (status transitions, notification conditions, `.ftsUpload` temp file behavior, CRC32 format, rename patterns, and duplicate-file handling).
- Keep package dependencies used by protocol execution: `edtFTPnetPRO` (`EnterpriseDT.Net.Ftp`) and `ICSharpCode.SharpZipLib`.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `ConsumerFactory.CreateConsumer(int)` | `ConsumerFactory.CreateConsumer(int)` | Direct parity. |
| `FTSConsumer.CreateFileTrackingEventRecord(System.Guid, System.Guid, string)` | `FTSConsumer.CreateFileTrackingEventRecord(System.Guid, System.Guid, string)` | Direct parity. |
| `FTSConsumer.DeliverFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel)` | `FTSConsumer.DeliverFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel)` | Direct parity. |
| `FTSConsumer.DownloadWorkingFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.IFileTransferOperations, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, bool)` | `FTSConsumer.DownloadWorkingFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.IFileTransferOperations, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, bool)` | Direct parity. |
| `FTSConsumer.Log(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.FTSConsumer.LogCode)` | `FTSConsumer.Log(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.FTSConsumer.LogCode)` | Direct parity. |
| `FTSConsumer.Log(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.FTSConsumer.LogCode, string)` | `FTSConsumer.Log(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.FTSConsumer.LogCode, string)` | Direct parity. |
| `FTSConsumer.Log(string)` | `FTSConsumer.Log(string)` | Direct parity. |
| `FTSConsumer.LogMessages(bool)` | `FTSConsumer.LogMessages(bool)` | Direct parity. |
| `FTSConsumer.ProcessDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>)` | `FTSConsumer.ProcessDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>)` | Direct parity. |
| `FTSConsumer.SendNotificationOfDelivery(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel)` | `FTSConsumer.SendNotificationOfDelivery(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel)` | Direct parity. |
| `FTSConsumer.SendNotificationOfError(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel, string)` | `FTSConsumer.SendNotificationOfError(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel, string)` | Direct parity. |
| `FTSConsumer.UpdateFileTrackingRecord(System.Guid, string)` | `FTSConsumer.UpdateFileTrackingRecord(System.Guid, string)` | Direct parity. |
| `FTSConsumer.UpdateFileWithExceptionAndStatus(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel, System.Exception, string)` | `FTSConsumer.UpdateFileWithExceptionAndStatus(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel, System.Exception, string)` | Direct parity. |
| `FTSProducer.AddNewFileToDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string)` | `FTSProducer.AddNewFileToDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string)` | Direct parity. |
| `FTSProducer.AddPendingFileToDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel, string)` | `FTSProducer.AddPendingFileToDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel, string)` | Direct parity. |
| `FTSProducer.AddSourceFilesToQueue(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | `FTSProducer.AddSourceFilesToQueue(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | Direct parity. |
| `FTSProducer.CheckAndAddPendingFile(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, System.IO.FileInfo, string)` | `FTSProducer.CheckAndAddPendingFile(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, System.IO.FileInfo, string)` | Direct parity. |
| `FTSProducer.DumpErrorToConsole(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, System.Exception)` | `FTSProducer.DumpErrorToConsole(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, System.Exception)` | Direct parity. |
| `FTSProducer.GetPendingFilesByStatusDate()` | `FTSProducer.GetPendingFilesByStatusDate()` | Direct parity. |
| `FTSProducer.GetPendingFilesToReProcess(System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>, System.Guid?, int?, int?)` | `FTSProducer.GetPendingFilesToReProcess(System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>, System.Guid?, int?, int?)` | Direct parity. |
| `FTSProducer.HandleLocationException(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, System.Exception)` | `FTSProducer.HandleLocationException(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, System.Exception)` | Direct parity. |
| `FTSProducer.InitializeAppsettings()` | `FTSProducer.InitializeAppsettings()` | Direct parity. |
| `FTSProducer.IsSourceSetToRun(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | `FTSProducer.IsSourceSetToRun(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | Direct parity. |
| `FTSProducer.PrintLast50FileStatus(System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>)` | `FTSProducer.PrintLast50FileStatus(System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>)` | Direct parity. |
| `FTSProducer.ProcessAllFilesSources()` | `FTSProducer.ProcessAllFilesSources()` | Direct parity. |
| `FTSProducer.ProcessAllPendingFiles(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel>, System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>)` | `FTSProducer.ProcessAllPendingFiles(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel>, System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>)` | Direct parity. |
| `FTSProducer.ProcessAllSourceFilesInLocation(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | `FTSProducer.ProcessAllSourceFilesInLocation(System.Collections.Concurrent.BlockingCollection<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.FileToDeliverModel>, System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.PendingFileModel>, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | Direct parity. |
| `FTSProducer.ProcessFailedTransfers(int?, System.Guid?, int?)` | `FTSProducer.ProcessFailedTransfers(int?, System.Guid?, int?)` | Direct parity. |
| `FileTransferBatchExecutor.AddFilesToDeliveryQueue(string[])` | `FileTransferBatchExecutor.AddFilesToDeliveryQueue(string[])` | Direct parity. |
| `FileTransferBatchExecutor.CheckAndThrowConfigurationException(string)` | `FileTransferBatchExecutor.CheckAndThrowConfigurationException(string)` | Direct parity. |
| `FileTransferBatchExecutor.CreateConsumerTasks()` | `FileTransferBatchExecutor.CreateConsumerTasks()` | Direct parity. |
| `FileTransferBatchExecutor.Execute(string[])` | `FileTransferBatchExecutor.Execute(string[])` | Direct parity. |
| `FileTransferBatchExecutor.GetCommandValue(string)` | `FileTransferBatchExecutor.GetCommandValue(string)` | Direct parity. |
| `FileTransferBatchExecutor.ParseInputArguments(string[], out System.Guid?, out int?, out int?)` | `FileTransferBatchExecutor.ParseInputArguments(string[], out System.Guid?, out int?, out int?)` | Direct parity. |
| `FileTransferBatchExecutor.PrintUsage()` | `FileTransferBatchExecutor.PrintUsage()` | Direct parity. |
| `FileTransferBatchExecutor.ProcessFilesInDeliverQueue()` | `FileTransferBatchExecutor.ProcessFilesInDeliverQueue()` | Direct parity. |
| `FileTransferBatchExecutor.Validate(string[])` | `FileTransferBatchExecutor.Validate(string[])` | Direct parity. |
| `FileTransferBatchExecutor.ValidateAppConfig()` | `FileTransferBatchExecutor.ValidateAppConfig()` | Direct parity. |
| `FileTransferBatchExecutor.ValidateInputArguments(string[])` | `FileTransferBatchExecutor.ValidateInputArguments(string[])` | Direct parity. |
| `FileTransferBatchExecutor.WaitForCompletion(System.Threading.Tasks.Task[])` | `FileTransferBatchExecutor.WaitForCompletion(System.Threading.Tasks.Task[])` | Direct parity. |
| `FileTransferOperations.CreateZipFile(string)` | `FileTransferOperations.CreateZipFile(string)` | Direct parity. |
| `FileTransferOperations.DeleteFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string)` | `FileTransferOperations.DeleteFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string)` | Direct parity. |
| `FileTransferOperations.DeleteFileFromFtp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string)` | `FileTransferOperations.DeleteFileFromFtp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string)` | Direct parity. |
| `FileTransferOperations.DeleteFileFromFtps(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string)` | `FileTransferOperations.DeleteFileFromFtps(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string)` | Direct parity. |
| `FileTransferOperations.DeleteFileFromSftp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string)` | `FileTransferOperations.DeleteFileFromSftp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string)` | Direct parity. |
| `FileTransferOperations.DownloadFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferOperations.DownloadFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `FileTransferOperations.DownloadFileFromFtp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferOperations.DownloadFileFromFtp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `FileTransferOperations.DownloadFileFromFtps(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferOperations.DownloadFileFromFtps(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `FileTransferOperations.DownloadFileFromSftp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferOperations.DownloadFileFromSftp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `FileTransferOperations.GetCrc32(string)` | `FileTransferOperations.GetCrc32(string)` | Direct parity. |
| `FileTransferOperations.GetDestinationFileName(string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel, System.Guid, string, string)` | `FileTransferOperations.GetDestinationFileName(string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel, System.Guid, string, string)` | Direct parity. |
| `FileTransferOperations.GetFileListFromFtpSource(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | `FileTransferOperations.GetFileListFromFtpSource(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | Direct parity. |
| `FileTransferOperations.GetFileListFromFtpsSource(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | `FileTransferOperations.GetFileListFromFtpsSource(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | Direct parity. |
| `FileTransferOperations.GetFileListFromSftpSource(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | `FileTransferOperations.GetFileListFromSftpSource(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | Direct parity. |
| `FileTransferOperations.GetListOfFileNamesFromSource(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, int)` | `FileTransferOperations.GetListOfFileNamesFromSource(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, int)` | Direct parity. |
| `FileTransferOperations.RenameFileFromFtp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, string)` | `FileTransferOperations.RenameFileFromFtp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, string)` | Direct parity. |
| `FileTransferOperations.RenameFileFromFtps(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, string)` | `FileTransferOperations.RenameFileFromFtps(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, string)` | Direct parity. |
| `FileTransferOperations.RenameFileFromSftp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, string)` | `FileTransferOperations.RenameFileFromSftp(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, string)` | Direct parity. |
| `FileTransferOperations.RenameSourceFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, string)` | `FileTransferOperations.RenameSourceFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, string)` | Direct parity. |
| `FileTransferOperations.SendNotification(long, string, string, bool)` | `FileTransferOperations.SendNotification(long, string, string, bool)` | Direct parity. |
| `FileTransferOperations.UploadFile(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferOperations.UploadFile(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `FileTransferOperations.UploadFileToFtp(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferOperations.UploadFileToFtp(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `FileTransferOperations.UploadFiletoFtps(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferOperations.UploadFiletoFtps(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `FileTransferOperations.UploadFiletoSftp(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferOperations.UploadFiletoSftp(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `FileTransferProtocols.CreateZipFile(string, string, System.IO.FileInfo)` | `FileTransferProtocols.CreateZipFile(string, string, System.IO.FileInfo)` | Direct parity. |
| `FileTransferProtocols.DeleteFromFtp(EnterpriseDT.Net.Ftp.FTPConnection, string)` | `FileTransferProtocols.DeleteFromFtp(EnterpriseDT.Net.Ftp.FTPConnection, string)` | Direct parity. |
| `FileTransferProtocols.DeleteFromUNC(string)` | `FileTransferProtocols.DeleteFromUNC(string)` | Direct parity. |
| `FileTransferProtocols.DownloadFromFtpConnection(EnterpriseDT.Net.Ftp.FTPConnection, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferProtocols.DownloadFromFtpConnection(EnterpriseDT.Net.Ftp.FTPConnection, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `FileTransferProtocols.DownloadFromUNCConnection(string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferProtocols.DownloadFromUNCConnection(string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `FileTransferProtocols.GetCrc32(string)` | `FileTransferProtocols.GetCrc32(string)` | Direct parity. |
| `FileTransferProtocols.GetFileListFromUncSource(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | `FileTransferProtocols.GetFileListFromUncSource(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | Direct parity. |
| `FileTransferProtocols.GetFilesFromSource(EnterpriseDT.Net.Ftp.FTPConnection, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | `FileTransferProtocols.GetFilesFromSource(EnterpriseDT.Net.Ftp.FTPConnection, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.SourceLocationModel)` | Direct parity. |
| `FileTransferProtocols.RenameFileFromUNC(string, string)` | `FileTransferProtocols.RenameFileFromUNC(string, string)` | Direct parity. |
| `FileTransferProtocols.RenameFtpFile(string, string, EnterpriseDT.Net.Ftp.FTPConnection)` | `FileTransferProtocols.RenameFtpFile(string, string, EnterpriseDT.Net.Ftp.FTPConnection)` | Direct parity. |
| `FileTransferProtocols.SendNotificationOfBadConfig(long)` | `FileTransferProtocols.SendNotificationOfBadConfig(long)` | Direct parity. |
| `FileTransferProtocols.UploadToFTPServer(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel, EnterpriseDT.Net.Ftp.FTPConnection)` | `FileTransferProtocols.UploadToFTPServer(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel, EnterpriseDT.Net.Ftp.FTPConnection)` | Direct parity. |
| `FileTransferProtocols.UploadToUnc(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | `FileTransferProtocols.UploadToUnc(string, string, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch.DestinationModel)` | Direct parity. |
| `PatternMatcher.StrictMatchPattern(string, string)` | `PatternMatcher.StrictMatchPattern(string, string)` | Direct parity. |

## Dependencies

- Component dependencies:
  - `backend-business-helpers` (PGP operations + duplicate-file exception type).
- Package dependencies:
  - `edtFTPnetPRO` (`EnterpriseDT.Net.Ftp`)
  - `ICSharpCode.SharpZipLib`

## Enterprise-Architecture standards applied

- `patterns/itsm-operations/api-microservices/PTN-OPS-API-004-structured-logging-pattern.md` section `Solution Overview` (structured logs; include contextual fields rather than embedding payload bytes)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Must-Know Standards` (correlation identifiers are opaque UUIDs; never embed PHI/PII in ids)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Solution Overview` (endpoint credentials and key material are treated as secrets and retrieved at runtime)
