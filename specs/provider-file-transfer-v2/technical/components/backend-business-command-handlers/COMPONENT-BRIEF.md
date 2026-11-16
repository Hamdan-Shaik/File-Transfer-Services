---
type: Generation Brief
title: 'Brief: component backend-business-command-handlers'
description: Instructions the tool gave the writer session for the backend-business-command-handlers component.
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
  body_sha256: 9f91d442e287bb73899c3ef884f4acbb0c4ed2f4956f2c14a9c12ce1df0678d6
---

# Brief: component `backend-business-command-handlers`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers (backend tier, 5 classes, 25 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-business-command-handlers/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-business-command-handlers/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Application/Commands`: write `services/fxfer-domain-api/src/Application/Commands` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

- `backend-business-command-handlers-2`: `technical/components/backend-business-command-handlers-2/`
- `backend-data-access-event-handlers`: `technical/components/backend-data-access-event-handlers/`
- `backend-data-access-event-handlers-2`: `technical/components/backend-data-access-event-handlers-2/`
- `shared-contracts-commands`: `technical/components/shared-contracts-commands/`

## Flows it takes part in

- `configure-source-locations-and-destinations`: `technical/flows/configure-source-locations-and-destinations/` (FLOW.md, RULES.md) and its scenarios `business/configure-source-locations-and-destinations.feature`
- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`
- `manage-integration-files`: `technical/flows/manage-integration-files/` (FLOW.md, RULES.md) and its scenarios `business/manage-integration-files.feature`

## Business rules it owns (12)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-046** When handling an add command, the system allocates a new `ConfigId` from the `FTS_SOURCE_LOCATION_SEQ` database sequence and includes it in the `FtsSourceLocationAdded` event.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:48-56, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:165-168, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:61-63
- Legacy: Calls `contextBase.GetNextSequenceValue("FTS_SOURCE_LOCATION_SEQ")`, casts it to `int`, and assigns it to `FtsSourceLocationAdded.ConfigId`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-047** The add handler copies command fields into the `FtsSourceLocationAdded` event and sets `OperatorId` from the requestor identifier.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:50-84
- Legacy: Populates the event from the command and sets `OperatorId = command.Requestor.IdentifierId`, with `DestinationAddress = command.DestinationAddress`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-069** When handling an update command, if no source location exists for the provided `ConfigId`, the flow throws a `BusinessException` with one entry (`keyName = ConfigId`, `message = ConfigIdNotFound`, `processingSystem = FileTransfer`) and exception message `Business Error occurred processing the command.`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143-163, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139-159, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:29, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:37
- Legacy: Existence is checked by executing a `SourceLocationQuery` filtered only by `ConfigId`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-070** When handling an invalidate command, if no source location exists for the provided `ConfigId`, the flow throws a `BusinessException` with one entry (`keyName = ConfigId`, `message = ConfigIdNotFound`, `processingSystem = FileTransfer`) and exception message `Business Error occurred processing the command.`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143-163, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139-159, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:29, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:37
- Legacy: Existence is checked by executing a `SourceLocationQuery` filtered only by `ConfigId`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-071** When handling an update command for an existing `ConfigId`, the flow creates `FtsSourceLocationUpdated` using the stored source location id (as `FtsSourceLocationsId`) and stored `ConfigId`, while copying the remaining configuration fields and `DestinationAddress` from the command.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:89-127, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:85-123
- Legacy: `OperatorId` is taken from `command.OperatorId` (not the command's `Requestor`).
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-072** When handling an invalidate command for an existing `ConfigId`, the flow creates `FtsSourceLocationInvalidated` using the stored source location id (as `FtsSourceLocationsId`) and stored `ConfigId`, while copying `OperatorId` and `DestinationAddress` from the command.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:130-141, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:126-137
- Legacy: The invalidation event is built from the looked-up source location for `ConfigId`, not from the inbound `FtsSourceLocationId`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-023** Handling an AddFtpServer command validates the command and fails the request if validations are present.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42-47, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93-101, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44-49, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95-103
- Legacy: The handler calls `ValidateFtpServerCommand(command)`, which calls `command.Validate()` and may throw during `HandleValidations()`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-031** AddFtpServer/UpdateFtpServer reject a duplicate FTP server name by checking for an existing valid FTP server record with the same `FtpServerName` but a different `FtpServerId`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:97-107, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114-123, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:99-109, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116-125, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:95-108, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:30-33
- Legacy: The handler queries FTP servers with `IsValid = true` and `FtpServerName = command.FtpServerName`, then considers it a duplicate when any returned row has `FtpServerId != command.FtpServerId`.
- Decision: keep
- ⚑ Flag: The two query executors compare `FtpServerName` case-insensitively, but only the HP.HSP.UA3 executor trims whitespace during comparison.

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-032** AddFtpServer returns an event populated from the command and forces `IsValid = true`, with `OperatorId` set to the requestor identifier.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:64-71, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:66-73
- Legacy: The handler maps command fields onto `FtpServerAdded`, then sets `ftpServerAdded.IsValid = true` and `ftpServerAdded.OperatorId = command.Requestor.IdentifierId`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-033** UpdateFtpServer returns an event populated from the command, with `OperatorId` set to the requestor identifier.
- Doors: file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:74-81, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:76-83
- Legacy: The handler maps command fields onto `FtpServerUpdated`, then sets `ftpServerUpdated.OperatorId = command.Requestor.IdentifierId`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-034** InvalidateFtpServer does not run command validation; it returns an event containing `FtpServerId` and forces `IsValid = false`, with `OperatorId` set to the requestor identifier.
- Doors: file-transfer-svc-service-controller-invalidate-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:56-59, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:83-91, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:58-61, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:85-93, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\InvalidateFtpServerCommand.cs:14-15, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\InvalidateFtpServerCommand.cs:14-15
- Legacy: The handler returns `new FtpServerInvalidated { FtpServerId = command.FtpServerId, IsValid = false, OperatorId = command.Requestor.IdentifierId }` and does not call `command.Validate()`.
- Decision: keep

From `technical/flows/manage-integration-files/RULES.md`:

**F03-011** A successful update emits an `IntegrationFileUpdated` event with the provided integration file id, file name, configuration id, active flag, and the operator id from the requestor.
- Doors: file-transfer-svc-service-update-integration-file, file-transfer-svc-service-update-integration-file-2
- Source: CMDH-UA3:54-58, CMDH-UA3:80-89
- Legacy: The handler calls `Validate()` then builds the event using the command values and `Requestor.IdentifierId`.
- Decision: keep


## Target-state requirements it owns (1)

No legacy code does these; the target state asks for them. Specify each in SPEC.md under its id, with the section it comes from, and give each a test in TESTS.md.

- **T-003** Enforce tenant-scoped authorization and invariants at the domain boundary for all command endpoints (not only in the BFF). (target state: 6.4 API Design Standards)
