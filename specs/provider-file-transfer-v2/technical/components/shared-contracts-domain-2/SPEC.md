---
type: Behavior Specification
title: 'shared-contracts-domain-2: legacy batch file-transfer domain contract models'
description: 'What the shared-contracts-domain-2 component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-domain-2
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:19:37Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-domain-2/INVENTORY.yaml
  title: Every legacy class and method (15 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-domain-2
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 15
  methods: 1
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Domain
  body_sha256: 0419d3ca4c8328d1e4afb0a4ec82596b56b626d70d6fc3f91b0ca1fb76c73d75
---

# shared-contracts-domain-2: legacy batch file-transfer domain contract models

This component contains the legacy domain contract/data models from `HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain`.
They are used as in-process DTOs by batch/worker execution code to carry source/destination configuration, file tracking identifiers, delivery/notification settings, and suspension/error information.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel
Represents one delivery destination for a source location.

- Fields:
  - `DestinationId` (`Guid`)
  - `DestinationAddress` (`string`)
  - `DestinationProtocolCode` (`string`)
  - `ContactName` (`string`)
  - `IsOverwriteAtDestination` (`bool`)
  - `IsCompressAtDestination` (`bool`)
  - `IsRenameAtDestination` (`bool`)
  - `RenamePattern` (`string`)
  - `IsArchive` (`bool`)
  - `EncryptionCode` (`string`)
  - `EncryptionPublicKey` (`string`)
  - `EncryptionPrivateKey` (`string`)
  - `EmailId` (`string`)
  - `PrivateKeyPassword` (`string`)
  - `FtpServerDetail` (`FtpServerModel`)
  - `DestinationFileName` (`string`)
  - `FTEMetadataId` (`Guid?`)
  - `ContractName` (`string`)
  - `IsFullyUploaded` (`bool`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.ErrorLogDetailModel
Describes one captured error log entry.

- Fields:
  - `DestinationId` (`Guid`)
  - `EventId` (`Guid`)
  - `FileId` (`Guid`)
  - `ErrorType` (`string`)
  - `ErrorText` (`string`)
  - `StackTrace` (`string`)
  - `MethodName` (`string`)
  - `AssemblyName` (`string`)
  - `ClassName` (`string`)
  - `MachineName` (`string`)
  - `OperatorId` (`string`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileManagementRequestModel
Request DTO used when creating/updating file tracking.

- Fields:
  - `FileID` (`Guid`)
  - `SourceLocationId` (`Guid`)
  - `FileName` (`string`)
  - `FileStatus` (`string`)
  - `FileSize` (`decimal`)
  - `ChecksumCode` (`string`)
  - `OperatorId` (`string`)
  - `TransactionId` (`string`)
  - `GroupId` (`Guid`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileManagementResponseModel
Response DTO returned after file tracking header creation.

- Fields:
  - `FileId` (`Guid`)
  - `FileTrackId` (`Guid`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileToDeliverModel
Represents a file selected for delivery.

- Fields:
  - `SourceLocation` (`SourceLocationModel`)
  - `FileName` (`string`)
  - `FileId` (`Guid`)
  - `SakFileTrack` (`int?`)
  - `FileSize` (`long`)
  - `FileType` (`string`)
  - `GroupId` (`Guid`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FtpServerModel
FTP server profile DTO.

- Fields:
  - `FtpServerId` (`Guid`)
  - `FtpServerName` (`string`)
  - `FtpServerIp` (`string`)
  - `FtpTransferMode` (`string`)
  - `FtpUser` (`string`)
  - `FtpPassword` (`string`)
  - `FtpPort` (`int`)
  - `SSHAuth` (`bool`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FtsDeliveryModel
Delivery record identifier wrapper.

- Fields:
  - `DeliveryId` (`Guid`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FtsNotificationModel
Notification recipient settings for one configuration.

- Fields:
  - `EmailAddress` (`string`)
  - `ContactName` (`string`)
  - `NotifyOnErrorInd` (`bool`)
  - `NotifyOnSuccessInd` (`bool`)
  - `ConfigId` (`Guid`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FtsNotificationQueryModel
Query criteria for selecting notification recipients.

- Fields:
  - `ConfigId` (`int`)
  - `IncludeError` (`bool`)
  - `IncludeSuccess` (`bool`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FtsSuspensionModel
Represents a suspension record for a tracked file.

- Fields:
  - `SakFileTrack` (`Guid`)
  - `SuspensionId` (`Guid`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FtsSuspensionSearchCriteria
Criteria used to search for a suspension.

- Fields:
  - `SourceLocationId` (`Guid`)
  - `ErrorText` (`string`)
  - `StackTrace` (`string`)
  - `OperatorId` (`string`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.GetNotificationSearchCriteria
Criteria used to query notification recipients.

- Fields:
  - `ConfigId` (`Guid`)
  - `NotifyOnErrorInd` (`bool`)
  - `NotifyOnSuccessInd` (`bool`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.PendingFileModel
Information about a file pending/waiting to send.

- Fields:
  - `FileId` (`Guid`)
  - `ConfigId` (`long`)
  - `FileSourceLocationId` (`Guid`)
  - `FileName` (`string`)
  - `FileStatus` (`string`)
  - `FileTrackId` (`long?`)
  - `StatusDate` (`DateTime`)
  - `FileSize` (`decimal`)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel
Describes a source location to retrieve files from, including its schedule and destinations.

- Constructor:
  - Initializes `Destinations` to `new List<DestinationModel>()`.

- Fields:
  - `SourceLocationId` (`Guid`)
  - `ConfigurationId` (`long`)
  - `PreviousConfigurationId` (`int`)
  - `SourceProtocolCode` (`string`)
  - `SourceAddress` (`string`)
  - `SendNotificationsIndicator` (`bool`)
  - `FtpServerDetail` (`FtpServerModel`)
  - `Schedule` (`SourceScheduleModel`)
  - `Destinations` (`List<DestinationModel>`)
  - `IsActive` (`bool`)
  - `IsSuspended` (`bool`)
  - `SuspendedIndChanged` (`DateTime`)
  - `FileMask` (`string`)
  - `SuspendedDate` (`DateTime`)
  - `OwnerEmail` (`string`)
  - `OwnerName` (`string`)
  - `ConfigurationName` (`string`)
  - `ConfigurationDescription` (`string`)
  - `FileDirection` (`string`)

### IsScheduledToRun
- Purpose: indicate whether the source is eligible to run now based on schedule rules.
- Inputs: `Schedule`.
- Logic:
  - If `Schedule` is `null`, return `true`.
  - Otherwise, return whether the current local `DateTime.Now` is within the configured time window and matches either the day-of-week list or day-of-month list.
- Errors:
  - If `Schedule` is not null but `Schedule.DaysToRun` or `Schedule.MonthlyProcessDayToRun` is null, schedule evaluation calls `.Exists(...)` and throws `NullReferenceException`.

### IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)
- Purpose: inclusive time-of-day window check used by schedule evaluation.
- Logic:
  - Return `time.TimeOfDay >= startTime && time.TimeOfDay <= endTime`.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceScheduleModel
Schedule used by `SourceLocationModel`.

- Fields:
  - `DaysToRun` (`List<DayOfWeek>`)
  - `MonthlyProcessDayToRun` (`List<int>`)
  - `StartTime` (`TimeSpan`)
  - `EndTime` (`TimeSpan`)

## Rules implemented

None.

## Not carried over

None.

