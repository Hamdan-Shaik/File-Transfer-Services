---
type: Behavior Specification
title: 'shared-common-file-transfer-management-constants: shared constants for FXFER transfer worker'
description: 'What the shared-common-file-transfer-management-constants component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-common-file-transfer-management-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:54:12Z
sources:
- id: inventory
  resource: /technical/components/shared-common-file-transfer-management-constants/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-common-file-transfer-management-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Domain/Constants
  body_sha256: 8dbe8c4171e3dc45646df674b55f8cf3346c7272b08699af6d8e4062a8275ddc
---

# shared-common-file-transfer-management-constants: shared constants for FXFER transfer worker

This component defines string constants and a metadata attribute used by legacy FXFER batch/file-transfer code.
It is consumed by other components for log/message text, error-field keys, and event-type codes.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants
Container for nested constant groups used by SFTP transfer execution and event distribution logging.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants.ExceptionMessages
Error/exception message templates.

- `AutoCreateDirectoryFailed`: `Remote directory does not exist and it cannot be auto created because it does not start with a / character`
- `DestinationPathIsNotValid`: `Destination: '{0}' path is not valid` (`{0}` is the destination path)
- `ErrorTryingToDeleteFile`: `An error occurred when trying to delete the file, exception thrown: {0}` (`{0}` is the exception)
- `ErrorRenamingFile`: `An error occurred when trying to rename the file, exception thrown: {0}` (`{0}` is the exception)
- `FileExixstAtDestination`: `File exists at destination and overwrite was not specified in config`

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants.Information
Informational log/message templates.

- `AttemptingToConnect`: `Attempting to connect to SFTP server {0}` (`{0}` is the server host/name)
- `ConnectionSuccessful`: `Connection established successfully`
- `DestinationFileName`: `Destination file name: {0}` (`{0}` is the destination file name)
- `Encrypted`: `ENCRYPTED`
- `FileFullyMoved`: `File is moved to destination`
- `FileWasUnabletoMove`: `File was unable to destination`
- `FullFilePath`: `Full file path: {0}` (`{0}` is the full file path)
- `SFTPConnectionClosed`: `SFTP connection closed`
- `SFTPConnectionEstablished`: `SFTP connection established`

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants.EventDistribution
Event distribution log templates.

- `AttemptingToDistributeEvent`: `Attempting to distibute {0} event, CommitId: {1}` (`{0}` is event name/type; `{1}` is commit id)
- `DistributionOfEventCompleted`: `Distribution of {0} event completed, CommitId: {1}` (`{0}` is event name/type; `{1}` is commit id)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants.Paths
Path fragments used to locate SSH private key material.

- `PathToPrivateKeyFile`: `\\SSHPrivateKey\\SSHPrivateKeyFile`

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Utilities.MaintainabilityDetails
An attribute type used to annotate legacy source with maintainability metadata (review sprint, reviewer, approval flags, and notes).
Its properties do not affect runtime behavior.

- Properties:
  - `ReviewedOnSprint` (int)
  - `ReviewerName` (string)
  - `IsApproved` (bool)
  - `ApprovedIndex` (int)
  - `NeedsAttention` (bool)
  - `TechnicalDebtNumber` (int)
  - `Notes` (string)

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.ErrorLogConstants
String keys used when writing structured error-log entries.

- `ExceptionType`: `ExceptionType`
- `Message`: `Message`
- `MethodName`: `MethodName`
- `StackTrace`: `StackTrace`

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.FileDeliveryEventConstants
Single-character codes used to categorize file-delivery outcomes.

- `Renamed`: `R`
- `Transferred`: `T`
- `Archieved`: `A`
- `Exception`: `E`

## Rules implemented

None.

## Not carried over

None.

