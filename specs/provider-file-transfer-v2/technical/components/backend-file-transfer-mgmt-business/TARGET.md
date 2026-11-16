---
type: Target Placement
title: 'Target: ackend-file-transfer-mgmt-business'
description: Where the backend-file-transfer-mgmt-business component goes in the new system and what to build, method by method.
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
  body_sha256: 59a0d7276664bb6ca31740228be1b9172fe41a7f87dc52824b89996bc3d9e899
---

# Target: ackend-file-transfer-mgmt-business

## Where

- Deployable: xfer-transfer-worker (workers/fxfer-transfer-worker)
- Folder: workers/fxfer-transfer-worker/src/Application

## What to create

Create the worker-side legacy batch file-transfer management logic:

- Constants: AppSettingsConstants, BatchJobConstants, ExceptionConstants, IocConstants.
- Batch entry: FileTransferBatchExecutor : IBatchExecutor<int>.
- Work discovery: FTSProducer : IFTSProducer.
- Work execution: FTSConsumer : IFTSConsumer + ConsumerFactory : IConsumerFactory.
- Protocol selection: FileTransferOperations : IFileTransferOperations.
- Protocol implementations: FileTransferProtocols : IFileTransferProtocols.
- File matching: PatternMatcher : IPatternMatcher.
- Value objects: FileBasicInfo.

Implementation notes:

- Preserve legacy behavior exactly (status transitions, .ftsUpload temp file behavior, CRC32 format, rename patterns, duplicate-file handling, and notification/event emission).
- Keep the legacy protocol/client libraries used by this component: edtFTPnetPRO (FTP/FTPS/SFTP), Renci.SshNet (SSH-key SFTP), and ICSharpCode.SharpZipLib (CRC32).
- Keep the legacy BAS FileTransferSvc integration-file update behavior (GetIntegrationFileQuery + UpdateIntegrationFileCommand) as called by FTSConsumer.HandleIntegrationFiles.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| ConsumerFactory.CreateConsumer(int) | ConsumerFactory.CreateConsumer(int) | Direct parity. |
| FTSConsumer.BuildIntegrationFileCommand(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.FtsIntegrationFileModel) | FTSConsumer.BuildIntegrationFileCommand(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.FtsIntegrationFileModel) | Direct parity. |
| FTSConsumer.BuildIntegrationFileQuery(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | FTSConsumer.BuildIntegrationFileQuery(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | Direct parity. |
| FTSConsumer.BuildLogMessages() | FTSConsumer.BuildLogMessages() | Direct parity. |
| FTSConsumer.BuildSTPUrl(string) | FTSConsumer.BuildSTPUrl(string) | Direct parity. |
| FTSConsumer.CleanupTransferDirectories(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FTSConsumer.CleanupTransferDirectories(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FTSConsumer.CreateFileTrackingEventRecord(System.Guid, System.Guid, string, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel>) | FTSConsumer.CreateFileTrackingEventRecord(System.Guid, System.Guid, string, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel>) | Direct parity. |
| FTSConsumer.DeleteSourceFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FTSConsumer.DeleteSourceFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FTSConsumer.DeleteTemporalFiles(string) | FTSConsumer.DeleteTemporalFiles(string) | Direct parity. |
| FTSConsumer.DeliverFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | FTSConsumer.DeliverFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | Direct parity. |
| FTSConsumer.DownloadWorkingFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.IFileTransferOperations, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, bool) | FTSConsumer.DownloadWorkingFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.IFileTransferOperations, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, bool) | Direct parity. |
| FTSConsumer.FormatEmailBody(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, System.IO.FileInfo, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FTSConsumer.FormatEmailBody(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, System.IO.FileInfo, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FTSConsumer.GetDestinationsForEmailBody(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FTSConsumer.GetDestinationsForEmailBody(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FTSConsumer.GetSFTPUserName(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | FTSConsumer.GetSFTPUserName(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | Direct parity. |
| FTSConsumer.GetSourceLocation(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | FTSConsumer.GetSourceLocation(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | Direct parity. |
| FTSConsumer.HandleIntegrationFiles(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | FTSConsumer.HandleIntegrationFiles(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | Direct parity. |
| FTSConsumer.Log(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FTSConsumer.LogCode) | FTSConsumer.Log(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FTSConsumer.LogCode) | Direct parity. |
| FTSConsumer.Log(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FTSConsumer.LogCode, string) | FTSConsumer.Log(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FTSConsumer.LogCode, string) | Direct parity. |
| FTSConsumer.Log(string) | FTSConsumer.Log(string) | Direct parity. |
| FTSConsumer.ProcessDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, string) | FTSConsumer.ProcessDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, string) | Direct parity. |
| FTSConsumer.SendCreateFileTrackingEventRecordNotification(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FTSConsumer.SendCreateFileTrackingEventRecordNotification(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FTSConsumer.SendDeliveryNotification(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FTSConsumer.SendDeliveryNotification(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FTSConsumer.SendDeliveryNotification(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, System.Diagnostics.Stopwatch, string) | FTSConsumer.SendDeliveryNotification(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, System.Diagnostics.Stopwatch, string) | Direct parity. |
| FTSConsumer.SendNotificationDestinationOfDelivery(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FTSConsumer.SendNotificationDestinationOfDelivery(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FTSConsumer.SendNotificationOfDelivery(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | FTSConsumer.SendNotificationOfDelivery(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | Direct parity. |
| FTSConsumer.SendNotificationOfError(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FTSConsumer.SendNotificationOfError(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FTSConsumer.SetFileDestinations(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FTSConsumer.SetFileDestinations(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FTSConsumer.SetFileStatus(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | FTSConsumer.SetFileStatus(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel) | Direct parity. |
| FTSConsumer.TransferFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, string, string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, System.Diagnostics.Stopwatch) | FTSConsumer.TransferFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, string, string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, System.Diagnostics.Stopwatch) | Direct parity. |
| FTSConsumer.UpdateFileTrackingRecord(System.Guid, string) | FTSConsumer.UpdateFileTrackingRecord(System.Guid, string) | Direct parity. |
| FTSConsumer.UpdateFileWithExceptionAndStatus(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, System.Exception, string) | FTSConsumer.UpdateFileWithExceptionAndStatus(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel, System.Exception, string) | Direct parity. |
| FTSProducer.AddNewFileToDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo) | FTSProducer.AddNewFileToDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo) | Direct parity. |
| FTSProducer.AddPendingFileToDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo) | FTSProducer.AddPendingFileToDeliveryQueue(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo) | Direct parity. |
| FTSProducer.AddSourceFilesToQueue(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FTSProducer.AddSourceFilesToQueue(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FTSProducer.BuildNewFileToDeliverModel(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo, int) | FTSProducer.BuildNewFileToDeliverModel(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo, int) | Direct parity. |
| FTSProducer.BuildPendingFileToDeliverModel(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo) | FTSProducer.BuildPendingFileToDeliverModel(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo) | Direct parity. |
| FTSProducer.CheckAndAddPendingFile(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo) | FTSProducer.CheckAndAddPendingFile(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business.FileBasicInfo) | Direct parity. |
| FTSProducer.DumpErrorToConsole(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, System.Exception) | FTSProducer.DumpErrorToConsole(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, System.Exception) | Direct parity. |
| FTSProducer.GetPendingFilesByStatusDate() | FTSProducer.GetPendingFilesByStatusDate() | Direct parity. |
| FTSProducer.GetPendingFilesToReProcess(System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>, System.Guid?, int?, int?) | FTSProducer.GetPendingFilesToReProcess(System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>, System.Guid?, int?, int?) | Direct parity. |
| FTSProducer.HandleLocationException(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, System.Exception) | FTSProducer.HandleLocationException(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, System.Exception) | Direct parity. |
| FTSProducer.InitializeAppsettings() | FTSProducer.InitializeAppsettings() | Direct parity. |
| FTSProducer.IsSourceSetToRun(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FTSProducer.IsSourceSetToRun(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FTSProducer.PrintLast50FileStatus(System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>) | FTSProducer.PrintLast50FileStatus(System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>) | Direct parity. |
| FTSProducer.ProcessAllFilesSources(string) | FTSProducer.ProcessAllFilesSources(string) | Direct parity. |
| FTSProducer.ProcessAllPendingFiles(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel>, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>) | FTSProducer.ProcessAllPendingFiles(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel>, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>) | Direct parity. |
| FTSProducer.ProcessAllSourceFilesInLocation(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FTSProducer.ProcessAllSourceFilesInLocation(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel>, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FTSProducer.ProcessFailedTransfers(int?, System.Guid?, int?, string) | FTSProducer.ProcessFailedTransfers(int?, System.Guid?, int?, string) | Direct parity. |
| FileTransferBatchExecutor.AddFilesToDeliveryQueue(string[], System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>) | FileTransferBatchExecutor.AddFilesToDeliveryQueue(string[], System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>) | Direct parity. |
| FileTransferBatchExecutor.AssignTenantId(string[]) | FileTransferBatchExecutor.AssignTenantId(string[]) | Direct parity. |
| FileTransferBatchExecutor.CreateConsumerTasks(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>) | FileTransferBatchExecutor.CreateConsumerTasks(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>) | Direct parity. |
| FileTransferBatchExecutor.Execute(string[]) | FileTransferBatchExecutor.Execute(string[]) | Direct parity. |
| FileTransferBatchExecutor.GetCommandValue(string) | FileTransferBatchExecutor.GetCommandValue(string) | Direct parity. |
| FileTransferBatchExecutor.ParseInputArguments(string[], out System.Guid?, out int?, out int?) | FileTransferBatchExecutor.ParseInputArguments(string[], out System.Guid?, out int?, out int?) | Direct parity. |
| FileTransferBatchExecutor.PrintUsage() | FileTransferBatchExecutor.PrintUsage() | Direct parity. |
| FileTransferBatchExecutor.ProcessFilesInDeliverQueue(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>) | FileTransferBatchExecutor.ProcessFilesInDeliverQueue(System.Collections.Concurrent.BlockingCollection<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel>) | Direct parity. |
| FileTransferBatchExecutor.WaitForCompletion(System.Threading.Tasks.Task[]) | FileTransferBatchExecutor.WaitForCompletion(System.Threading.Tasks.Task[]) | Direct parity. |
| FileTransferOperations.BuildSftpClient(string, string, string, int) | FileTransferOperations.BuildSftpClient(string, string, string, int) | Direct parity. |
| FileTransferOperations.CreateZipFile(string) | FileTransferOperations.CreateZipFile(string) | Direct parity. |
| FileTransferOperations.DeleteFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | FileTransferOperations.DeleteFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | Direct parity. |
| FileTransferOperations.DeleteFileFromFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | FileTransferOperations.DeleteFileFromFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | Direct parity. |
| FileTransferOperations.DeleteFileFromFtps(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | FileTransferOperations.DeleteFileFromFtps(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | Direct parity. |
| FileTransferOperations.DeleteFileFromSSHFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | FileTransferOperations.DeleteFileFromSSHFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | Direct parity. |
| FileTransferOperations.DeleteFileFromSftp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | FileTransferOperations.DeleteFileFromSftp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | Direct parity. |
| FileTransferOperations.DownloadFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, string) | FileTransferOperations.DownloadFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, string) | Direct parity. |
| FileTransferOperations.DownloadFileFromFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferOperations.DownloadFileFromFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferOperations.DownloadFileFromFtps(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferOperations.DownloadFileFromFtps(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferOperations.DownloadFileFromSftp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferOperations.DownloadFileFromSftp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferOperations.DownloadFileFromSftpUsingSSHKeys(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferOperations.DownloadFileFromSftpUsingSSHKeys(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferOperations.GetCrc32(string) | FileTransferOperations.GetCrc32(string) | Direct parity. |
| FileTransferOperations.GetDestinationFileName(string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, System.Guid, string, string) | FileTransferOperations.GetDestinationFileName(string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, System.Guid, string, string) | Direct parity. |
| FileTransferOperations.GetFileListFromFtpSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FileTransferOperations.GetFileListFromFtpSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FileTransferOperations.GetFileListFromFtpsSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FileTransferOperations.GetFileListFromFtpsSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FileTransferOperations.GetFileListFromSSHFtpSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FileTransferOperations.GetFileListFromSSHFtpSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FileTransferOperations.GetFileListFromSftpSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FileTransferOperations.GetFileListFromSftpSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FileTransferOperations.GetListOfFileNamesFromSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, int) | FileTransferOperations.GetListOfFileNamesFromSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, int) | Direct parity. |
| FileTransferOperations.ParseEnum<T>(string) | FileTransferOperations.ParseEnum<T>(string) | Direct parity. |
| FileTransferOperations.PostEvent(string, string, string, bool, string) | FileTransferOperations.PostEvent(string, string, string, bool, string) | Direct parity. |
| FileTransferOperations.RenameFileFromFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string) | FileTransferOperations.RenameFileFromFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string) | Direct parity. |
| FileTransferOperations.RenameFileFromFtps(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string) | FileTransferOperations.RenameFileFromFtps(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string) | Direct parity. |
| FileTransferOperations.RenameFileFromSSHFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string) | FileTransferOperations.RenameFileFromSSHFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string) | Direct parity. |
| FileTransferOperations.RenameFileFromSftp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string) | FileTransferOperations.RenameFileFromSftp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string) | Direct parity. |
| FileTransferOperations.RenameSourceFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string) | FileTransferOperations.RenameSourceFile(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string) | Direct parity. |
| FileTransferOperations.SendNotification(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string, bool, string) | FileTransferOperations.SendNotification(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, string, bool, string) | Direct parity. |
| FileTransferOperations.SetPrivateKey(string) | FileTransferOperations.SetPrivateKey(string) | Direct parity. |
| FileTransferOperations.UploadFile(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferOperations.UploadFile(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferOperations.UploadFileToFtp(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferOperations.UploadFileToFtp(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferOperations.UploadFiletoFtps(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferOperations.UploadFiletoFtps(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferOperations.UploadFiletoSSHFtp(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferOperations.UploadFiletoSSHFtp(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferOperations.UploadFiletoSftp(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferOperations.UploadFiletoSftp(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferProtocols.AttempToDownloadFileWithSSHFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, string, string, Renci.SshNet.SftpClient) | FileTransferProtocols.AttempToDownloadFileWithSSHFtp(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, string, string, Renci.SshNet.SftpClient) | Direct parity. |
| FileTransferProtocols.CheckAndHandleFileOverwrite(EnterpriseDT.Net.Ftp.FTPConnection, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferProtocols.CheckAndHandleFileOverwrite(EnterpriseDT.Net.Ftp.FTPConnection, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferProtocols.CloseRemoteServerConnection(EnterpriseDT.Net.Ftp.FTPConnection) | FileTransferProtocols.CloseRemoteServerConnection(EnterpriseDT.Net.Ftp.FTPConnection) | Direct parity. |
| FileTransferProtocols.CreateRemoteDirectoryIfNotExists(EnterpriseDT.Net.Ftp.FTPConnection, string) | FileTransferProtocols.CreateRemoteDirectoryIfNotExists(EnterpriseDT.Net.Ftp.FTPConnection, string) | Direct parity. |
| FileTransferProtocols.CreateZipFile(string, string, System.IO.FileInfo) | FileTransferProtocols.CreateZipFile(string, string, System.IO.FileInfo) | Direct parity. |
| FileTransferProtocols.DeleteFromFtp(EnterpriseDT.Net.Ftp.FTPConnection, string) | FileTransferProtocols.DeleteFromFtp(EnterpriseDT.Net.Ftp.FTPConnection, string) | Direct parity. |
| FileTransferProtocols.DeleteFromSFtp(Renci.SshNet.SftpClient, string) | FileTransferProtocols.DeleteFromSFtp(Renci.SshNet.SftpClient, string) | Direct parity. |
| FileTransferProtocols.DeleteFromUNC(string) | FileTransferProtocols.DeleteFromUNC(string) | Direct parity. |
| FileTransferProtocols.DownloadFromFtpConnection(EnterpriseDT.Net.Ftp.FTPConnection, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, string) | FileTransferProtocols.DownloadFromFtpConnection(EnterpriseDT.Net.Ftp.FTPConnection, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, string) | Direct parity. |
| FileTransferProtocols.DownloadFromSSHFtpConnection(Renci.SshNet.SftpClient, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, string) | FileTransferProtocols.DownloadFromSSHFtpConnection(Renci.SshNet.SftpClient, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, string) | Direct parity. |
| FileTransferProtocols.DownloadFromUNCConnection(string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferProtocols.DownloadFromUNCConnection(string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferProtocols.FileAndTemporalFileExistsOnRemoteServer(EnterpriseDT.Net.Ftp.FTPConnection, string, string) | FileTransferProtocols.FileAndTemporalFileExistsOnRemoteServer(EnterpriseDT.Net.Ftp.FTPConnection, string, string) | Direct parity. |
| FileTransferProtocols.GetCrc32(string) | FileTransferProtocols.GetCrc32(string) | Direct parity. |
| FileTransferProtocols.GetFileListFromUncSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FileTransferProtocols.GetFileListFromUncSource(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FileTransferProtocols.GetFilesFromSSHSource(Renci.SshNet.SftpClient, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FileTransferProtocols.GetFilesFromSSHSource(Renci.SshNet.SftpClient, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FileTransferProtocols.GetFilesFromSource(EnterpriseDT.Net.Ftp.FTPConnection, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | FileTransferProtocols.GetFilesFromSource(EnterpriseDT.Net.Ftp.FTPConnection, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel) | Direct parity. |
| FileTransferProtocols.HandleOverwriteAtDestination(string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, Renci.SshNet.SftpClient) | FileTransferProtocols.HandleOverwriteAtDestination(string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, Renci.SshNet.SftpClient) | Direct parity. |
| FileTransferProtocols.IsAValidDestination(string) | FileTransferProtocols.IsAValidDestination(string) | Direct parity. |
| FileTransferProtocols.IsFileFullyUploaded(EnterpriseDT.Net.Ftp.FTPConnection, string) | FileTransferProtocols.IsFileFullyUploaded(EnterpriseDT.Net.Ftp.FTPConnection, string) | Direct parity. |
| FileTransferProtocols.RemoveTemporaryUploadFile(EnterpriseDT.Net.Ftp.FTPConnection, string) | FileTransferProtocols.RemoveTemporaryUploadFile(EnterpriseDT.Net.Ftp.FTPConnection, string) | Direct parity. |
| FileTransferProtocols.RenameFileFromUNC(string, string) | FileTransferProtocols.RenameFileFromUNC(string, string) | Direct parity. |
| FileTransferProtocols.RenameFtpFile(string, string, EnterpriseDT.Net.Ftp.FTPConnection) | FileTransferProtocols.RenameFtpFile(string, string, EnterpriseDT.Net.Ftp.FTPConnection) | Direct parity. |
| FileTransferProtocols.RenameSSHFtpFile(string, string, Renci.SshNet.SftpClient) | FileTransferProtocols.RenameSSHFtpFile(string, string, Renci.SshNet.SftpClient) | Direct parity. |
| FileTransferProtocols.RenameUploadedFile(EnterpriseDT.Net.Ftp.FTPConnection, string, string) | FileTransferProtocols.RenameUploadedFile(EnterpriseDT.Net.Ftp.FTPConnection, string, string) | Direct parity. |
| FileTransferProtocols.SendNotificationOfBadConfig(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | FileTransferProtocols.SendNotificationOfBadConfig(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, string) | Direct parity. |
| FileTransferProtocols.TryToAutoCreateDirectoryIfDestinationLocationDoesNotExists(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, Renci.SshNet.SftpClient) | FileTransferProtocols.TryToAutoCreateDirectoryIfDestinationLocationDoesNotExists(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, Renci.SshNet.SftpClient) | Direct parity. |
| FileTransferProtocols.TryUploadFileToSFTP(EnterpriseDT.Net.Ftp.FTPConnection, string, string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferProtocols.TryUploadFileToSFTP(EnterpriseDT.Net.Ftp.FTPConnection, string, string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| FileTransferProtocols.UploadToFTPServer(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, EnterpriseDT.Net.Ftp.FTPConnection) | FileTransferProtocols.UploadToFTPServer(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, EnterpriseDT.Net.Ftp.FTPConnection) | Direct parity. |
| FileTransferProtocols.UploadToSSHFTPServer(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, Renci.SshNet.SftpClient) | FileTransferProtocols.UploadToSSHFTPServer(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel, Renci.SshNet.SftpClient) | Direct parity. |
| FileTransferProtocols.UploadToUnc(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | FileTransferProtocols.UploadToUnc(string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel) | Direct parity. |
| PatternMatcher.StrictMatchPattern(string, string) | PatternMatcher.StrictMatchPattern(string, string) | Direct parity. |

## Dependencies

- Component dependencies:
  - ackend-business-helpers-2 (PGP encryption/decryption, integration event publishing, notification service helper, duplicate-file exception type).
- Package dependencies:
  - ICSharpCode.SharpZipLib
  - Renci.SshNet
  - edtFTPnetPRO

## Enterprise-Architecture standards applied

- standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md section 2.1 Data Protection Requirements (tenant-scoped execution; no cross-tenant access)
- standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md section 2.1 Mandatory Requirements (work-item processing must be observable and correlateable)
- patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md section Solution Overview (correlation id extraction/generation at entry points including batch/scheduled jobs)
- patterns/itsm-operations/api-microservices/PTN-OPS-API-004-structured-logging-pattern.md section Solution Overview (structured logging; avoid embedding payload bytes and secrets in log messages)
- patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md section Solution Overview (endpoint credentials and key material treated as secrets and retrieved at runtime)
