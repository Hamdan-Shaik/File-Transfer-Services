---
type: Behavior Specification
title: technical/components/shared-contracts-batch/SPEC
description: 'What the shared-contracts-batch component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-batch
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:02:24Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-batch/INVENTORY.yaml
  title: Every legacy class and method (15 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-batch
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 15
  methods: 1
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Contracts
  body_sha256: d1089262cf44be58616e3070c40b9af5ca2293214ecc40f3596993e1e74f6f70
---

﻿# shared-contracts-batch: legacy batch contract models (in-process)

This component contains the legacy batch-oriented contract/data models from `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch`.
They are used as in-process DTOs by the batch/worker execution code to carry transfer configuration, file tracking identifiers, delivery/notification details, and suspension/error information.

## DestinationModel
Represents one delivery destination for a source location.

- Fields:
  - `DestinationId` (`Guid`)
  - `DestinationAddress` (`string`)
  - `DestinationProtocolCode` (`string`)
  - `IsOverwriteAtDestination` (`bool`)
  - `IsCompressAtDestination` (`bool`)
  - `IsRenameAtDestination` (`bool`)
  - `RenamePattern` (`string`)
  - `IsArchive` (`bool`)
  - `EncryptionCode` (`string`)
  - `EncryptionPublicKey` (`string`)
  - `EncryptionPrivateKey` (`string`)
  - `PrivateKeyPassword` (`string`)
  - `FtpServerDetail` (`FtpServerModel`)
  - `DestinationFileName` (`string`)

## ErrorLogDetailModel
Describes an error log entry associated to a file.

- Fields:
  - `FileId` (`Guid`)
  - `ErrorType` (`string`)
  - `ErrorText` (`string`)
  - `StackTrace` (`string`)
  - `MethodName` (`string`)
  - `AssemblyName` (`string`)
  - `ClassName` (`string`)
  - `MachineName` (`string`)
  - `OperatorId` (`string`)

## FileManagementRequestModel
Describes the request payload used by the batch runtime when creating/updating file tracking.

- Fields:
  - `FileID` (`Guid`)
  - `SourceLocationId` (`Guid`)
  - `FileName` (`string`)
  - `FileStatus` (`string`)
  - `FileSize` (`decimal`)
  - `ChecksumCode` (`string`)
  - `OperatorId` (`string`)
  - `TransactionId` (`string`)

## FileManagementResponseModel
Describes the response payload returned after file tracking header creation.

- Fields:
  - `FileId` (`Guid`)
  - `FileTrackId` (`Guid`)

## FileToDeliverModel
Represents a file selected for delivery, including its source configuration and identifiers.

- Fields:
  - `SourceLocation` (`SourceLocationModel`)
  - `FileName` (`string`)
  - `FileId` (`Guid`)
  - `SakFileTrack` (`long?`)

## FtpServerModel
Describes an FTP server profile used by a source location and/or destination.

- Fields:
  - `FtpServerId` (`Guid`)
  - `FtpServerName` (`string`)
  - `FtpServerIp` (`string`)
  - `FtpTransferMode` (`string`)
  - `FtpUser` (`string`)
  - `FtpPassword` (`string`)

## FtsDeliveryModel
Delivery record identifier wrapper.

- Fields:
  - `DeliveryId` (`Guid`)

## FtsNotificationModel
Notification recipient settings for one configuration.

- Fields:
  - `EmailAddress` (`string`)
  - `ContactName` (`string`)
  - `NotifyOnErrorInd` (`bool`)
  - `NotifyOnSuccessInd` (`bool`)
  - `ConfigId` (`Guid`)

## FtsNotificationQueryModel
Query criteria for selecting notification recipients.

- Fields:
  - `ConfigId` (`int`)
  - `IncludeError` (`bool`)
  - `IncludeSuccess` (`bool`)

## FtsSuspensionModel
Represents a suspension record for a tracked file.

- Fields:
  - `SakFileTrack` (`Guid`)
  - `SuspensionId` (`Guid`)

## FtsSuspensionSearchCriteria
Criteria used to search for a suspension (source + diagnostic text).

- Fields:
  - `SourceLocationId` (`Guid`)
  - `ErrorText` (`string`)
  - `StackTrace` (`string`)

## GetNotificationSearchCriteria
Criteria used to query notification recipients for a configuration.

- Fields:
  - `ConfigId` (`Guid`)
  - `NotifyOnErrorInd` (`bool`)
  - `NotifyOnSuccessInd` (`bool`)

## PendingFileModel
Information about a file that is pending/waiting to send.

- Fields:
  - `FileId` (`Guid`)
  - `ConfigId` (`int`)
  - `FileSourceLocationId` (`Guid`)
  - `FileName` (`string`)
  - `FileStatus` (`string`)
  - `FileTrackId` (`int?`)
  - `StatusDate` (`DateTime`)

## SourceLocationModel
Describes a source location that the batch runtime can retrieve files from, including its schedule and destinations.

- Constructor:
  - Initializes `Destinations` to `new List<DestinationModel>()`.

- Fields:
  - `SourceLocationId` (`Guid`)
  - `ConfigurationId` (`long`)
  - `PreviousConfigurationId` (`int`)
  - `SourceProtocolCode` (`string`)
  - `SourceAddress` (`string`)
  - `SendNotificationsIndicator` (`bool`)
  - `PreviousConfigId` (`int`)
  - `FtpServerDetail` (`FtpServerModel`)
  - `Schedule` (`SourceScheduleModel`)
  - `Destinations` (`List<DestinationModel>`)
  - `IsActive` (`bool`)
  - `IsSuspended` (`bool`)
  - `SuspendedIndChanged` (`DateTime`)
  - `FileMask` (`string`)
  - `SuspendedDate` (`DateTime`)

### IsScheduledToRun
- Purpose: Indicate whether the source is eligible to run now based on schedule rules.
- Inputs: `Schedule`.
- Logic:
  - If `Schedule` is `null`, return `true`.
  - Otherwise, evaluate `IsNowInScheduledTime` and return `true` when it is `true`; else `false`.
- Errors:
  - If `Schedule` is not null but `Schedule.DaysToRun` or `Schedule.MonthlyProcessDayToRun` is null, schedule evaluation attempts to call `.Exists(...)` and throws a `NullReferenceException`.

### MinsToWaitBeforeRetrySuspendedConfig
- Purpose: Provide the retry wait duration (in minutes) used when determining whether a suspended source can be retried.
- Inputs: application configuration key `MinsToWaitBeforeRetrySuspendedConfig`.
- Logic:
  - Reads `System.Configuration.ConfigurationManager.AppSettings["MinsToWaitBeforeRetrySuspendedConfig"]`.
  - Converts it to `int` and multiplies by `-1`.
  - Returns the resulting negative minute count (for use with `DateTime.AddMinutes(negative)`).
- Errors:
  - If the app setting is missing/null, `.ToString()` throws, causing the property getter to throw.

### IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)
- Purpose: Determine whether `time.TimeOfDay` falls within the start/end window.
- Inputs:
  - `time`: a `DateTime` whose `TimeOfDay` is evaluated.
  - `startTime`, `endTime`: inclusive bounds.
- Logic:
  - Return `time.TimeOfDay >= startTime && time.TimeOfDay <= endTime`.
- Data: none.
- Errors: none.

## SourceScheduleModel
Schedule used by `SourceLocationModel` to decide whether a location is within its execution window.

- Fields:
  - `DaysToRun` (`List<DayOfWeek>`)
  - `MonthlyProcessDayToRun` (`List<int>`)
  - `StartTime` (`TimeSpan`)
  - `EndTime` (`TimeSpan`)

## Rules implemented

None.
