---
type: Behavior Specification
title: technical/components/backend-business-command-handlers/SPEC
description: 'What the backend-business-command-handlers component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-command-handlers
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:59:42Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-manage-integration-files
  resource: /technical/flows/manage-integration-files/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-business-command-handlers/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-command-handlers
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 25
  owned_rules: 12
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  depends_on:
  - backend-business-command-handlers-2
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - shared-contracts-commands
  deployable: fxfer-domain-api
  folder: src/Application/Commands
  body_sha256: f9a5b2cdf85d444c53a7b0420f6d29170d2a0f97e0c6314d33cfcc4c3f8df891
---

﻿# backend-business-command-handlers: command handlers for FXFER configuration writes

This component contains the legacy command-handler implementations that validate configuration mutation commands and decide whether to emit a configuration-change event (or to perform a no-op).

These handlers are invoked on the write path for FXFER configuration operations (FTP servers, transfer configurations/source locations, inbound targets, and integration files).

## FtpServerCommandHandler
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtpServerCommandHandler`

Command handler for FTP server profile mutations. The handler validates commands and performs a duplicate-name check before emitting an event.

### Handle(AddFtpServerCommand command)
- Purpose: validate and accept an Add FTP Server mutation.
- Inputs: `command` including FTP server fields and `Requestor.IdentifierId`.
- Logic:
  - Calls `ValidateFtpServerCommand(command)`.
  - Returns `BuildEvent(command)`.
- Data: reads `orm:FTP_SERVER` via `IFileTransferReadOnlyRepository.GetFtpServer(...)` (duplicate-name check).
- Errors:
  - Throws `BusinessException` when `command.Validate()` or `command.HandleValidations()` reports validation failures.
- Rules: F02-023, F02-031.

### Handle(UpdateFtpServerCommand command)
- Purpose: validate and accept an Update FTP Server mutation.
- Inputs: `command` including FTP server fields and `Requestor.IdentifierId`.
- Logic:
  - Calls `ValidateFtpServerCommand(command)`.
  - Returns `BuildEvent(command)`.
- Data: reads `orm:FTP_SERVER` via `IFileTransferReadOnlyRepository.GetFtpServer(...)` (duplicate-name check).
- Errors:
  - Throws `BusinessException` when `command.Validate()` or `command.HandleValidations()` reports validation failures.
- Rules: F02-031.

### Handle(InvalidateFtpServerCommand command)
- Purpose: accept an invalidate/deactivate mutation without running command validation.
- Inputs: `command.FtpServerId`, `command.Requestor.IdentifierId`.
- Logic: returns `BuildEvent(command)`.
- Data: none.
- Errors: none.
- Rules: F02-034.

### BuildEvent(AddFtpServerCommand command)
- Purpose: build the FTP-server creation event payload.
- Inputs: `command`.
- Logic:
  - Create a new `FtpServerAdded`.
  - Copy command fields into the event (`Mapper.DynamicMap(command, ftpServerAdded)`).
  - Force `IsValid = true`.
  - Set `OperatorId = command.Requestor.IdentifierId`.
- Data: none.
- Errors: AutoMapper mapping failures surface to the caller.
- Rules: F02-032.

### BuildEvent(UpdateFtpServerCommand command)
- Purpose: build the FTP-server update event payload.
- Inputs: `command`.
- Logic:
  - Create a new `FtpServerUpdated`.
  - Copy command fields into the event (`Mapper.DynamicMap(command, ftpServerUpdated)`).
  - Set `OperatorId = command.Requestor.IdentifierId`.
- Data: none.
- Errors: AutoMapper mapping failures surface to the caller.
- Rules: F02-033.

### BuildEvent(InvalidateFtpServerCommand command)
- Purpose: build the FTP-server invalidation event payload.
- Inputs: `command`.
- Logic: returns `new FtpServerInvalidated { FtpServerId = command.FtpServerId, IsValid = false, OperatorId = command.Requestor.IdentifierId }`.
- Data: none.
- Errors: none.
- Rules: F02-034.

### ValidateFtpServerCommand(AddFtpServerCommand command)
- Purpose: run command validation and enforce the duplicate-name rule.
- Inputs: `command`.
- Logic:
  - Calls `command.Validate()`.
  - When `IsFtpServerNameDuplicated(command.FtpServerName, command.FtpServerId)` is true:
    - Calls `command.AddValidation(command.FtpServerName, CommonConstants.Validation.ErrorType.DuplicatedFtpServerName, CommonConstants.Validation.Messages.DuplicatedFtpServerName)`.
    - Calls `command.HandleValidations()`.
- Data: reads `orm:FTP_SERVER` (duplicate-name check).
- Errors: throws `BusinessException` when validations exist.
- Rules: F02-023, F02-031.

### IsFtpServerNameDuplicated(string ftpServerName, Guid ftpServerId)
- Purpose: determine whether an active FTP server already exists with the same name but a different id.
- Inputs: `ftpServerName`, `ftpServerId`.
- Logic:
  - Calls `GetFtpServers(BuildFtpServerQuery(ftpServerName))`.
  - Returns true when any returned row has `FtpServerId != ftpServerId`.
- Data: reads `orm:FTP_SERVER`.
- Errors: repository exceptions surface to the caller.
- Rules: F02-031.

### GetFtpServers(FtpServerQuery query)
- Purpose: execute a read-only FTP server query.
- Inputs: `query`.
- Logic: returns `fileTransferRepository.GetFtpServer(query)`.
- Data: reads `orm:FTP_SERVER`.
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
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsInboundTargetCommandHandler`

Command handler for inbound-target mutations keyed by `FTEMetadataId`.

### Handle(AddFtsInboundTargetCommand addFtsInboundTargetCommand)
- Purpose: add an inbound target if one does not already exist for the given `FTEMetadataId`.
- Inputs: `FTEMetadataId`, `ImportJobName`, `ContractName`, `Requestor.IdentifierId`.
- Logic:
  - Query existing inbound targets by `FTEMetadataId` (`BuildInboundTargetQuery(addFtsInboundTargetCommand.FTEMetadataId, addFtsInboundTargetCommand.Requestor)`).
  - When the query returns null or an empty list: return `BuildAddFtsInboundTargetEvent(addFtsInboundTargetCommand)`.
  - Otherwise: return `new Event()` (no-op).
- Data: reads `orm:FTS_INBOUND_TARGET` via `IFileTransferReadOnlyRepository.GetInboundTarget(...)`.
- Errors: repository exceptions surface to the caller.
- Rules: none.

### Handle(InvalidateFtsInboundTargetCommand invalidateFtsInboundTargetCommand)
- Purpose: invalidate an existing inbound target for the given `FTEMetadataId`.
- Inputs: `FTEMetadataId` and `Requestor`.
- Logic:
  - Query existing inbound targets by `FTEMetadataId`.
  - When the query returns null or an empty list: return `new Event()` (no-op).
  - Otherwise: return `BuildInvalidateFtsInboundTargetEvent(inboundTarget)`.
- Data: reads `orm:FTS_INBOUND_TARGET`.
- Errors: repository exceptions surface to the caller.
- Rules: none.

### BuildAddFtsInboundTargetEvent(AddFtsInboundTargetCommand addFtsInboundTargetCommand)
- Purpose: build the inbound-target creation event payload.
- Inputs: `addFtsInboundTargetCommand`.
- Logic: returns `new FtsInboundTargetAdded { FTEMetadataId, ImportJobName, ContractName, IsValid = true, OperatorId = Requestor.IdentifierId }`.
- Data: none.
- Errors: none.
- Rules: none.

### BuildInvalidateFtsInboundTargetEvent(List<FtsInboundTargetModel> inboundTarget)
- Purpose: build the inbound-target invalidation event payload.
- Inputs: `inboundTarget` (expected non-empty list).
- Logic: returns `new FtsInboundTargetInvalidated { FtsInboundTargetId = inboundTarget.First().FtsInboundTargetId, IsValid = false }`.
- Data: none.
- Errors:
  - Throws when `inboundTarget` is empty (`First()`), but callers guard against empty lists.
- Rules: none.

### BuildInboundTargetQuery(Guid fteMetadataId, RequestorModel requestor)
- Purpose: build the inbound-target lookup query.
- Inputs: `fteMetadataId`, `requestor`.
- Logic: returns `new InboundTargetQuery { Where = new InboundTargetQueryParms { FTEMetadataId = fteMetadataId }, Requestor = requestor }`.
- Data: none.
- Errors: none.
- Rules: none.

## FtsIntegrationFileCommandHandler
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsIntegrationFileCommandHandler`

Command handler for integration-file mutations.

### Handle(AddIntegrationFileCommand addIntegrationFileCommand)
- Purpose: add integration-file records only when the command includes at least one usable group-info entry.
- Inputs: `addIntegrationFileCommand` including `GroupInfo` list and `Requestor.IdentifierId`.
- Logic:
  - When `ValidateGroupInfo(addIntegrationFileCommand)` is true: return `BuildAddIntegrationFileEvent(addIntegrationFileCommand)`.
  - Otherwise: return `new Event()` (no-op).
- Data: none.
- Errors: none.
- Rules: none.

### Handle(UpdateIntegrationFileCommand updateIntegrationFileCommand)
- Purpose: validate and accept integration-file updates.
- Inputs: `updateIntegrationFileCommand` including `FtsIntegrationFileId`, `FileName`, `ConfigId`, `IsValid`, and `Requestor.IdentifierId`.
- Logic:
  - Calls `updateIntegrationFileCommand.Validate()`.
  - Returns `BuildUpdateIntegrationFileEvent(updateIntegrationFileCommand)`.
- Data: none.
- Errors: throws `BusinessException` when `Validate()` produces validations.
- Rules: F03-011.

### ValidateGroupInfo(AddIntegrationFileCommand command)
- Purpose: decide whether the command contains at least one usable group-info entry.
- Inputs: `command.GroupInfo`.
- Logic: returns true only when `command.GroupInfo != null` and any entry has `NumberOfRecords > 0` and `GroupId != null`.
- Data: none.
- Errors: none.
- Rules: none.

### BuildAddIntegrationFileEvent(AddIntegrationFileCommand addIntegrationFileCommand)
- Purpose: build the integration-file creation event payload.
- Inputs: `addIntegrationFileCommand`.
- Logic: returns `new IntegrationFileAdded { GroupInfo = command.GroupInfo, FileName = command.FileName, ETFOutboundMetadataId = command.ETFOutboundMetadataDefnId, IsProcessed = false, IsValid = true, OperatorId = command.Requestor.IdentifierId }`.
- Data: none.
- Errors: none.
- Rules: none.

### BuildUpdateIntegrationFileEvent(UpdateIntegrationFileCommand updateIntegrationFileCommand)
- Purpose: build the integration-file update event payload.
- Inputs: `updateIntegrationFileCommand`.
- Logic: returns `new IntegrationFileUpdated { FtsIntegrationFileId, FileName, ConfigId, IsValid, OperatorId = Requestor.IdentifierId }`.
- Data: none.
- Errors: none.
- Rules: F03-011.

## FtsSourceLocationAddCommandHandler
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsSourceLocationAddCommandHandler`

Command handler for transfer-configuration (source location + destinations) mutations.

### Handle(FtsSourceLocationAddCommand command)
- Purpose: validate and accept source-location creation.
- Inputs: `command` including configuration fields, schedule flags, destination list, and `Requestor.IdentifierId`.
- Logic:
  - Calls `command.Validate()`.
  - Creates `FtsSourceLocationAdded` with:
    - `ConfigId = (int)CreateConfigNumber()`.
    - Remaining configuration fields copied from the command.
    - `OperatorId = command.Requestor.IdentifierId`.
    - `DestinationAddress = command.DestinationAddress`.
  - Returns the event.
- Data: consumes database sequence `FTS_SOURCE_LOCATION_SEQ` via `contextBase.GetNextSequenceValue(...)`.
- Errors: throws `BusinessException` when command validation fails.
- Rules: F01-046, F01-047.

### Handle(FtsSourceLocationUpdateCommand command)
- Purpose: validate and accept source-location updates, failing when `ConfigId` does not exist.
- Inputs: `command` including `ConfigId` and configuration fields.
- Logic:
  - Calls `command.Validate()`.
  - Calls `IsSourceLocationExists(command.ConfigId)`.
  - Returns `new FtsSourceLocationUpdated` populated with:
    - `FtsSourceLocationsId = sourceLocation.FtsSourceLocationId.ToString()`.
    - `ConfigId = sourceLocation.ConfigId`.
    - Remaining configuration fields copied from the command.
    - `OperatorId = command.OperatorId` (not `Requestor.IdentifierId`).
    - `DestinationAddress = command.DestinationAddress`.
- Data: reads `orm:FTS_SOURCE_LOCATION` via `SearchSourceLocationQueryExecutor`.
- Errors:
  - Throws `BusinessException` when no matching source location exists (from `IsSourceLocationExists`).
- Rules: F01-069, F01-071.

### Handle(FtsSourceLocationInvalidateCommand command)
- Purpose: validate and accept source-location invalidation, failing when `ConfigId` does not exist.
- Inputs: `command.ConfigId`, `command.OperatorId`, `command.DestinationAddress`.
- Logic:
  - Calls `command.Validate()`.
  - Calls `IsSourceLocationExists(command.ConfigId)`.
  - Returns `new FtsSourceLocationInvalidated` populated with:
    - `FtsSourceLocationsId = sourceLocation.FtsSourceLocationId.ToString()`.
    - `ConfigId = sourceLocation.ConfigId`.
    - `OperatorId = command.OperatorId`.
    - `DestinationAddress = command.DestinationAddress`.
- Data: reads `orm:FTS_SOURCE_LOCATION` via `SearchSourceLocationQueryExecutor`.
- Errors:
  - Throws `BusinessException` when no matching source location exists.
- Rules: F01-070, F01-072.

### IsSourceLocationExists(int configId)
- Purpose: load an existing source location by `ConfigId`, throwing a `BusinessException` when none exists.
- Inputs: `configId`.
- Logic:
  - Build `SourceLocationQuery` filtered only by `ConfigId`.
  - Execute `new SearchSourceLocationQueryExecutor().Execute(contextBase, query)`.
  - When `existingSourceLocation.Results.Count == 0`:
    - Create `exceptions = [ new BusinessExceptionMessage(keyName = ConfigId, message = ConfigIdNotFound, processingSystem = FileTransfer) ]` using `FileTransferExceptionConstants.Validation`.
    - Throw `new BusinessException(exceptions, FileTransferExceptionConstants.Validation.Message, FileTransferExceptionConstants.Validation.ProcessingSystem)`.
  - Otherwise return `existingSourceLocation.Results[0]`.
- Data: reads `orm:FTS_SOURCE_LOCATION`.
- Errors: throws `BusinessException` when no row exists.
- Rules: F01-069, F01-070.

### CreateConfigNumber()
- Purpose: allocate the next sequential ConfigId value.
- Inputs: none.
- Logic: returns `contextBase.GetNextSequenceValue(FileTransferExceptionConstants.Fields.FtsSourceLocationSeq)`.
- Data: consumes database sequence `FTS_SOURCE_LOCATION_SEQ`.
- Errors: database/connection errors surface to the caller.
- Rules: F01-046.

## FtsSourceLocationRepositoryHandler
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsSourceLocationRepositoryHandler`

Legacy base class providing an `EventStoreRepository` property for the legacy CQRS infrastructure.

## Rules implemented

| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-046 | Allocate new `ConfigId` from `FTS_SOURCE_LOCATION_SEQ` for adds. | `FtsSourceLocationAddCommandHandler.Handle(FtsSourceLocationAddCommand)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-047 | Copy add-command fields to `FtsSourceLocationAdded`; set `OperatorId` from requestor. | `FtsSourceLocationAddCommandHandler.Handle(FtsSourceLocationAddCommand)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-069 | Update throws `BusinessException` when `ConfigId` not found (query by `ConfigId` only). | `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-070 | Invalidate throws `BusinessException` when `ConfigId` not found (query by `ConfigId` only). | `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-071 | Update event uses stored source-location id + stored `ConfigId`; remaining fields from command. | `FtsSourceLocationAddCommandHandler.Handle(FtsSourceLocationUpdateCommand)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-072 | Invalidate event uses stored source-location id + stored `ConfigId`; `OperatorId`/destinations from command. | `FtsSourceLocationAddCommandHandler.Handle(FtsSourceLocationInvalidateCommand)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F02-023 | AddFtpServer validates and fails on validations. | `FtpServerCommandHandler.Handle(AddFtpServerCommand)` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-031 | Add/Update reject duplicate FTP server names among valid rows, excluding same id. | `FtpServerCommandHandler.ValidateFtpServerCommand` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-032 | AddFtpServer event forces `IsValid=true` and sets `OperatorId` from requestor. | `FtpServerCommandHandler.BuildEvent(AddFtpServerCommand)` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-033 | UpdateFtpServer event sets `OperatorId` from requestor. | `FtpServerCommandHandler.BuildEvent(UpdateFtpServerCommand)` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-034 | InvalidateFtpServer skips validation; forces `IsValid=false` and sets `OperatorId` from requestor. | `FtpServerCommandHandler.Handle(InvalidateFtpServerCommand)` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F03-011 | UpdateIntegrationFile emits `IntegrationFileUpdated` with ids/flags and operator id from requestor. | `FtsIntegrationFileCommandHandler.Handle(UpdateIntegrationFileCommand)` | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |

## Target-state requirements

### T-003 Enforce tenant-scoped authorization and invariants at the domain boundary for all command endpoints (not only in the BFF).
- Purpose: ensure command execution is always tenant-scoped and authorization is enforced within `fxfer-domain-api`.
- Inputs:
  - Verified tenant context (`tenant_id`) derived from JWT/service identity.
  - Caller authorization context (claims/scopes/roles).
- Logic:
  - Command execution requires a resolved tenant context; tenant identity is not accepted from request bodies/query parameters.
  - Command execution is denied when authorization checks fail (defense-in-depth).
  - Domain invariants that rely on tenant isolation (e.g., uniqueness checks, existence checks) are evaluated within the tenant-scoped persistence context.
- Data: tenant-scoped command operations execute only against the tenant database resolved by `tenant_id`.
- Errors:
  - Missing/invalid tenant context results in an authorization failure response.
- Target-state section: `inputs/target-state.md` section `6.4 API Design Standards`.

## Not carried over

- `FtsSourceLocationRepositoryHandler`: legacy CQRS base/plumbing; target uses DI + MediatR and does not use legacy `CommandHandler`/Unity injection.
