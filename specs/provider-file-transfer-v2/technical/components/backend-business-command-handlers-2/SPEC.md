---
type: Behavior Specification
title: 'backend-business-command-handlers-2: alternate command handlers for FXFER configuration writes'
description: 'What the backend-business-command-handlers-2 component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-command-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T06:11:41Z
sources:
- id: inventory
  resource: /technical/components/backend-business-command-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-command-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 25
  owned_rules: 0
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  depends_on:
  - backend-business-command-handlers
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - shared-contracts-commands
  deployable: fxfer-domain-api
  folder: src/Application/Commands
  body_sha256: 2a221edf4968244d4a8c619cc36b4a94e82d387869b6d28f5e72e1b3ba6b4564
---

# backend-business-command-handlers-2: alternate command handlers for FXFER configuration writes

This component contains an alternate-namespace implementation of the legacy FXFER configuration mutation command handlers.

The handlers are invoked on the legacy CQRS write path and return legacy CQRS `Event` payloads (or an empty `Event` for no-op cases). They rely on the legacy read-only repository and CQRS infrastructure contracts.

## FtpServerCommandHandler
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtpServerCommandHandler`

Command handler for FTP server profile mutations. The handler validates commands and performs a duplicate-name check before emitting an event.

### Handle(AddFtpServerCommand command)
- Purpose: validate and accept an Add FTP Server mutation.
- Inputs: `command` including FTP server fields and `Requestor.IdentifierId`.
- Logic:
  - Calls `ValidateFtpServerCommand(command)`.
  - Returns `BuildEvent(command)`.
- Data: reads FTP server rows via `IFileTransferReadOnlyRepository.GetFtpServer(...)` (duplicate-name check).
- Errors:
  - Validation failures raised by `command.Validate()` surface to the caller.
  - Throws a legacy `BusinessException` when `command.HandleValidations()` is invoked due to a duplicated name.
- Rules: none.

### Handle(UpdateFtpServerCommand command)
- Purpose: validate and accept an Update FTP Server mutation.
- Inputs: `command` including FTP server fields and `Requestor.IdentifierId`.
- Logic:
  - Calls `ValidateFtpServerCommand(command)` (the legacy code accepts `UpdateFtpServerCommand` here because the update command is compatible with the add-command validation shape).
  - Returns `BuildEvent(command)`.
- Data: reads FTP server rows via `IFileTransferReadOnlyRepository.GetFtpServer(...)` (duplicate-name check).
- Errors:
  - Validation failures raised by `command.Validate()` surface to the caller.
  - Throws a legacy `BusinessException` when `command.HandleValidations()` is invoked due to a duplicated name.
- Rules: none.

### Handle(InvalidateFtpServerCommand command)
- Purpose: accept an invalidate/deactivate mutation without running command validation.
- Inputs: `command.FtpServerId`, `command.Requestor.IdentifierId`.
- Logic: returns `BuildEvent(command)`.
- Data: none.
- Errors: none.
- Rules: none.

### BuildEvent(AddFtpServerCommand command)
- Purpose: build the FTP-server creation event payload.
- Inputs: `command`.
- Logic:
  - Create a new `FtpServerAdded`.
  - Copy command fields into the event (`MapperWrapper.Mapper.Map(command, ftpServerAdded)`).
  - Force `IsValid = true`.
  - Set `OperatorId = command.Requestor.IdentifierId`.
- Data: none.
- Errors: AutoMapper mapping failures surface to the caller.
- Rules: none.

### BuildEvent(UpdateFtpServerCommand command)
- Purpose: build the FTP-server update event payload.
- Inputs: `command`.
- Logic:
  - Create a new `FtpServerUpdated`.
  - Copy command fields into the event (`MapperWrapper.Mapper.Map(command, ftpServerUpdated)`).
  - Set `OperatorId = command.Requestor.IdentifierId`.
- Data: none.
- Errors: AutoMapper mapping failures surface to the caller.
- Rules: none.

### BuildEvent(InvalidateFtpServerCommand command)
- Purpose: build the FTP-server invalidation event payload.
- Inputs: `command`.
- Logic: returns `new FtpServerInvalidated { FtpServerId = command.FtpServerId, IsValid = false, OperatorId = command.Requestor.IdentifierId }`.
- Data: none.
- Errors: none.
- Rules: none.

### ValidateFtpServerCommand(AddFtpServerCommand command)
- Purpose: run command validation and enforce the duplicate-name check.
- Inputs: `command`.
- Logic:
  - Calls `command.Validate()`.
  - When `IsFtpServerNameDuplicated(command.FtpServerName, command.FtpServerId)` is true:
    - Calls `command.AddValidation(command.FtpServerName, "Duplicated FTP Server Name")`.
    - Calls `command.HandleValidations()`.
- Data: reads FTP server rows (duplicate-name check).
- Errors: throws `BusinessException` when validations exist.
- Rules: none.

### IsFtpServerNameDuplicated(string ftpServerName, Guid ftpServerId)
- Purpose: determine whether an active FTP server already exists with the same name but a different id.
- Inputs: `ftpServerName`, `ftpServerId`.
- Logic:
  - Calls `GetFtpServers(BuildFtpServerQuery(ftpServerName))`.
  - Returns true when any returned row has `FtpServerId != ftpServerId`.
- Data: reads FTP server rows.
- Errors: repository exceptions surface to the caller.
- Rules: none.

### GetFtpServers(FtpServerQuery query)
- Purpose: execute a read-only FTP server query.
- Inputs: `query`.
- Logic: returns `fileTransferRepository.GetFtpServer(query)`.
- Data: reads FTP server rows.
- Errors: repository exceptions surface to the caller.
- Rules: none.

### BuildFtpServerQuery(string ftpServerName)
- Purpose: build the query used by the duplicate-name check.
- Inputs: `ftpServerName`.
- Logic: returns `new FtpServerQuery { Where = new FtpServerQueryParms { IsValid = true, FtpServerName = ftpServerName } }`.
- Data: none.
- Errors: none.
- Rules: none.

## FtsInboundTargetCommandHandler
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsInboundTargetCommandHandler`

Command handler for inbound-target profile mutations. It performs an existence check; add/invalidate are implemented as no-ops when the target row is already present/missing.

### Handle(AddFtsInboundTargetCommand addFtsInboundTargetCommand)
- Purpose: accept an inbound-target add only when no existing inbound target exists for the metadata id.
- Inputs: `addFtsInboundTargetCommand.FTEMetadataId`, `addFtsInboundTargetCommand.ImportJobName`, `addFtsInboundTargetCommand.ContractName`, `addFtsInboundTargetCommand.Requestor.IdentifierId`.
- Logic:
  - Query inbound targets via `GetInboundTarget(BuildInboundTargetQuery(addFtsInboundTargetCommand.FTEMetadataId, addFtsInboundTargetCommand.Requestor))`.
  - When query result is null or empty: return `BuildAddFtsInboundTargetEvent(addFtsInboundTargetCommand)`.
  - Otherwise: return a new empty `Event()` (legacy no-op).
- Data: reads inbound target rows via `IFileTransferReadOnlyRepository.GetInboundTarget(...)`.
- Errors: repository exceptions surface to the caller.
- Rules: none.

### Handle(InvalidateFtsInboundTargetCommand invalidateFtsInboundTargetCommand)
- Purpose: invalidate an inbound-target row only when it exists.
- Inputs: `invalidateFtsInboundTargetCommand.FTEMetadataId`, `invalidateFtsInboundTargetCommand.Requestor`.
- Logic:
  - Query inbound targets via `GetInboundTarget(BuildInboundTargetQuery(invalidateFtsInboundTargetCommand.FTEMetadataId, invalidateFtsInboundTargetCommand.Requestor))`.
  - When query result is null or empty: return a new empty `Event()` (legacy no-op).
  - Otherwise: return `BuildInvalidateFtsInboundTargetEvent(inboundTarget)`.
- Data: reads inbound target rows via `IFileTransferReadOnlyRepository.GetInboundTarget(...)`.
- Errors: repository exceptions surface to the caller.
- Rules: none.

### BuildAddFtsInboundTargetEvent(AddFtsInboundTargetCommand addFtsInboundTargetCommand)
- Purpose: build the inbound-target creation event payload.
- Inputs: `addFtsInboundTargetCommand`.
- Logic: returns `new FtsInboundTargetAdded { FTEMetadataId = <command.FTEMetadataId>, ImportJobName = <command.ImportJobName>, ContractName = <command.ContractName>, IsValid = true, OperatorId = <command.Requestor.IdentifierId> }`.
- Data: none.
- Errors: none.
- Rules: none.

### BuildInvalidateFtsInboundTargetEvent(List<FtsInboundTargetModel> inboundTarget)
- Purpose: build the inbound-target invalidation event payload.
- Inputs: `inboundTarget` (must be non-empty).
- Logic: returns `new FtsInboundTargetInvalidated { FtsInboundTargetId = inboundTarget.First().FtsInboundTargetId, IsValid = false }`.
- Data: none.
- Errors: throws if `inboundTarget.First()` is evaluated on an empty list.
- Rules: none.

### BuildInboundTargetQuery(Guid fteMetadataId, RequestorModel requestor)
- Purpose: build the query used by the inbound-target existence checks.
- Inputs: `fteMetadataId`, `requestor`.
- Logic: returns `new InboundTargetQuery { Where = new InboundTargetQueryParms { FTEMetadataId = fteMetadataId }, Requestor = requestor }`.
- Data: none.
- Errors: none.
- Rules: none.

## FtsIntegrationFileCommandHandler
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsIntegrationFileCommandHandler`

Command handler for integration-file mutations. Adds are treated as no-op when group info is missing or does not contain any record-counted group.

### Handle(AddIntegrationFileCommand addIntegrationFileCommand)
- Purpose: accept an integration-file add only when the command includes at least one group entry with records.
- Inputs: `addIntegrationFileCommand.GroupInfo`, `addIntegrationFileCommand.FileName`, `addIntegrationFileCommand.ETFOutboundMetadataDefnId`, `addIntegrationFileCommand.Requestor.IdentifierId`.
- Logic:
  - When `ValidateGroupInfo(addIntegrationFileCommand)` is true: return `BuildAddIntegrationFileEvent(addIntegrationFileCommand)`.
  - Otherwise: return a new empty `Event()` (legacy no-op).
- Data: none.
- Errors: none.
- Rules: none.

### Handle(UpdateIntegrationFileCommand updateIntegrationFileCommand)
- Purpose: validate and accept an integration-file update.
- Inputs: `updateIntegrationFileCommand` including ids/flags and `Requestor.IdentifierId`.
- Logic:
  - Calls `updateIntegrationFileCommand.Validate()`.
  - Returns `BuildUpdateIntegrationFileEvent(updateIntegrationFileCommand)`.
- Data: none.
- Errors: validation failures raised by `updateIntegrationFileCommand.Validate()` surface to the caller.
- Rules: none.

### ValidateGroupInfo(AddIntegrationFileCommand command)
- Purpose: decide whether an AddIntegrationFileCommand should produce an event.
- Inputs: `command`.
- Logic: returns true when `command.GroupInfo != null` and any group entry satisfies `NumberOfRecords > 0` and `GroupId != null`.
- Data: none.
- Errors: none.
- Rules: none.

### BuildAddIntegrationFileEvent(AddIntegrationFileCommand addIntegrationFileCommand)
- Purpose: build the integration-file creation event payload.
- Inputs: `addIntegrationFileCommand`.
- Logic: returns `new IntegrationFileAdded { GroupInfo = <command.GroupInfo>, FileName = <command.FileName>, ETFOutboundMetadataId = <command.ETFOutboundMetadataDefnId>, IsProcessed = false, IsValid = true, OperatorId = <command.Requestor.IdentifierId> }`.
- Data: none.
- Errors: none.
- Rules: none.

### BuildUpdateIntegrationFileEvent(UpdateIntegrationFileCommand updateIntegrationFileCommand)
- Purpose: build the integration-file update event payload.
- Inputs: `updateIntegrationFileCommand`.
- Logic: returns `new IntegrationFileUpdated { FtsIntegrationFileId = <command.FtsIntegrationFileId>, FileName = <command.FileName>, ConfigId = <command.ConfigId>, IsValid = <command.IsValid>, OperatorId = <command.Requestor.IdentifierId> }`.
- Data: none.
- Errors: none.
- Rules: none.

## FtsSourceLocationAddCommandHandler
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsSourceLocationAddCommandHandler`

Command handler for transfer-configuration (source location) mutations.

### Handle(FtsSourceLocationAddCommand command)
- Purpose: validate and accept a new source location / transfer configuration.
- Inputs: `command` including schedule/contact fields, destinations, and `Requestor.IdentifierId`.
- Logic:
  - Calls `command.Validate()`.
  - Allocates `ConfigId` by calling `CreateConfigNumber()` and casting the resulting `long` to an `int`.
  - Returns a new `FtsSourceLocationAdded` event by copying all command fields and setting:
    - `ConfigId = <allocated>`
    - `OperatorId = command.Requestor.IdentifierId`
- Data:
  - Allocates from sequence `FTS_SOURCE_LOCATION_SEQ` via `IDbContextBase.GetNextSequenceValue("FTS_SOURCE_LOCATION_SEQ")`.
- Errors: validation failures raised by `command.Validate()` surface to the caller.
- Rules: none.

### Handle(FtsSourceLocationUpdateCommand command)
- Purpose: validate and accept a source location update.
- Inputs: `command.ConfigId` and updated fields including `OperatorId`.
- Logic:
  - Calls `command.Validate()`.
  - Calls `IsSourceLocationExists(command.ConfigId)`.
  - Returns a new `FtsSourceLocationUpdated` event that:
    - Uses the stored `FtsSourceLocationId` from the queried row (as `FtsSourceLocationsId = <id>.ToString()`).
    - Uses the stored `ConfigId` from the queried row.
    - Copies remaining fields from the command.
    - Sets `OperatorId = command.OperatorId`.
- Data: reads source location rows via `SearchSourceLocationQueryExecutor` filtered only by `ConfigId`.
- Errors:
  - When no row exists for the `ConfigId`, throws `BusinessException` with:
    - Message: `Business Error occurred processing the command.`
    - One validation entry: `keyName = "ConfigId"`, `message = "ConfigIdNotFound"`, `processingSystem = "FileTransfer"`.
- Rules: none.

### Handle(FtsSourceLocationInvalidateCommand command)
- Purpose: validate and accept a source location invalidation.
- Inputs: `command.ConfigId`, `command.OperatorId`, `command.DestinationAddress`.
- Logic:
  - Calls `command.Validate()`.
  - Calls `IsSourceLocationExists(command.ConfigId)`.
  - Returns a new `FtsSourceLocationInvalidated` event that:
    - Uses the stored `FtsSourceLocationId` from the queried row (as `FtsSourceLocationsId = <id>.ToString()`).
    - Uses the stored `ConfigId` from the queried row.
    - Sets `OperatorId = command.OperatorId`.
    - Copies `DestinationAddress` from the command.
- Data: reads source location rows via `SearchSourceLocationQueryExecutor` filtered only by `ConfigId`.
- Errors:
  - When no row exists for the `ConfigId`, throws the same `BusinessException` shape as the update handler.
- Rules: none.

### IsSourceLocationExists(int configId)
- Purpose: load an existing source location by config id or raise the legacy not-found exception.
- Inputs: `configId`.
- Logic:
  - Build a `SourceLocationQuery` with `Where.ConfigId = configId`.
  - Execute `new SearchSourceLocationQueryExecutor().Execute(contextBase, query)`.
  - When `Results.Count == 0`, throw `BusinessException` with message `Business Error occurred processing the command.` and one validation entry (`ConfigId`, `ConfigIdNotFound`, `FileTransfer`).
  - Otherwise return `Results[0]`.
- Data: reads `FtsSourceLocation` rows via the query executor.
- Errors: throws `BusinessException` for not-found.
- Rules: none.

### CreateConfigNumber()
- Purpose: allocate the next transfer configuration number.
- Inputs: none.
- Logic: returns `contextBase.GetNextSequenceValue("FTS_SOURCE_LOCATION_SEQ")`.
- Data: reads sequence `FTS_SOURCE_LOCATION_SEQ`.
- Errors: database/sequence errors surface to the caller.
- Rules: none.

## FtsSourceLocationRepositoryHandler
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsSourceLocationRepositoryHandler`

Legacy base class providing an `EventStoreRepository` property for the legacy CQRS infrastructure.

## Rules implemented

None.

## Not carried over

- `FtsSourceLocationRepositoryHandler`: legacy CQRS base/plumbing; target uses DI + MediatR and does not use legacy `CommandHandler` inheritance for composition.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Command Side Components` (command handlers are part of the write model; validate and apply mutations before publishing events).
- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Prohibited Practices` (target implementation splits the legacy multi-command handler into one handler per command type).