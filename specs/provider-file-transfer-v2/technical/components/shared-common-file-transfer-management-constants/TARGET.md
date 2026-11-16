---
type: Target Placement
title: 'shared-common-file-transfer-management-constants: what to build'
description: Where the shared-common-file-transfer-management-constants component goes in the new system and what to build, method by method.
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
  body_sha256: a4c78455887c2033ad1624da5c0c3ca6e03c5c76393888edad8dbdcbb0b5e42d
---

# shared-common-file-transfer-management-constants: what to build

## Where

- Deployable: `fxfer-transfer-worker`
- Folder: `workers/fxfer-transfer-worker/src/Domain/Constants`

## What to create

Create the following types under the `Fxfer.TransferWorker.Domain.Constants` namespace.

### FileTransferManagementConstants

Create `FileTransferManagementConstants` containing the nested groups below.

#### FileTransferManagementConstants.ExceptionMessages

- `AutoCreateDirectoryFailed`: `Remote directory does not exist and it cannot be auto created because it does not start with a / character`
- `DestinationPathIsNotValid`: `Destination: '{0}' path is not valid`
- `ErrorTryingToDeleteFile`: `An error occurred when trying to delete the file, exception thrown: {0}`
- `ErrorRenamingFile`: `An error occurred when trying to rename the file, exception thrown: {0}`
- `FileExixstAtDestination`: `File exists at destination and overwrite was not specified in config`

#### FileTransferManagementConstants.Information

- `AttemptingToConnect`: `Attempting to connect to SFTP server {0}`
- `ConnectionSuccessful`: `Connection established successfully`
- `DestinationFileName`: `Destination file name: {0}`
- `Encrypted`: `ENCRYPTED`
- `FileFullyMoved`: `File is moved to destination`
- `FileWasUnabletoMove`: `File was unable to destination`
- `FullFilePath`: `Full file path: {0}`
- `SFTPConnectionClosed`: `SFTP connection closed`
- `SFTPConnectionEstablished`: `SFTP connection established`

#### FileTransferManagementConstants.EventDistribution

- `AttemptingToDistributeEvent`: `Attempting to distibute {0} event, CommitId: {1}`
- `DistributionOfEventCompleted`: `Distribution of {0} event completed, CommitId: {1}`

#### FileTransferManagementConstants.Paths

- `PathToPrivateKeyFile`: `\\SSHPrivateKey\\SSHPrivateKeyFile`

### MaintainabilityDetails

Create `MaintainabilityDetails` as an attribute type (derive from `System.Attribute`) with these get/set properties:

- `ReviewedOnSprint` (int)
- `ReviewerName` (string)
- `IsApproved` (bool)
- `ApprovedIndex` (int)
- `NeedsAttention` (bool)
- `TechnicalDebtNumber` (int)
- `Notes` (string)

### ErrorLogConstants

Create `ErrorLogConstants` (static constants):

- `ExceptionType`: `ExceptionType`
- `Message`: `Message`
- `MethodName`: `MethodName`
- `StackTrace`: `StackTrace`

### FileDeliveryEventConstants

Create `FileDeliveryEventConstants` (static constants):

- `Renamed`: `R`
- `Transferred`: `T`
- `Archieved`: `A`
- `Exception`: `E`

## Mapping

This inventory has no methods; mapping is class-to-class.

| Legacy | Target | Note |
|---|---|---|
| HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants | Fxfer.TransferWorker.Domain.Constants.FileTransferManagementConstants | Preserve nested class names and string values (including typos) for parity. |
| HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants.ExceptionMessages | Fxfer.TransferWorker.Domain.Constants.FileTransferManagementConstants.ExceptionMessages | Nested static class. |
| HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants.Information | Fxfer.TransferWorker.Domain.Constants.FileTransferManagementConstants.Information | Nested static class. |
| HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants.EventDistribution | Fxfer.TransferWorker.Domain.Constants.FileTransferManagementConstants.EventDistribution | Nested static class. |
| HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants.Paths | Fxfer.TransferWorker.Domain.Constants.FileTransferManagementConstants.Paths | Nested static class. |
| HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Utilities.MaintainabilityDetails | Fxfer.TransferWorker.Domain.Constants.MaintainabilityDetails | Preserved for source-annotation parity; no runtime behavior. |
| HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.ErrorLogConstants | Fxfer.TransferWorker.Domain.Constants.ErrorLogConstants | Preserve constant values. |
| HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.FileDeliveryEventConstants | Fxfer.TransferWorker.Domain.Constants.FileDeliveryEventConstants | Preserve constant values (including `Archieved`). |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `standards/application/api/STD-APP-API-002-microservices-development.md` section `Hexagonal Architecture (Ports and Adapters)`
- `standards/application/api/STD-APP-API-002-microservices-development.md` section `Project Structure`
- `decisions/application/ADR-APP-001-design-patterns-languages.md` section `2. Domain-Driven Design (DDD)`

