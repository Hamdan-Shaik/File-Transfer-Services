---
type: Business Rules
title: technical/flows/manage-integration-files/RULES
description: 13 business rules (F03-NNN) of the flow, each with its legacy source lines.
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
tags:
- flow
- provider-file-transfer-v2
- wcf
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T06:55:41Z
sources:
- id: door-file-transfer-svc-query-service-get-integration-file
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L50
  title: GetIntegrationFile (wcf door)
- id: door-file-transfer-svc-query-service-get-integration-file-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L41
  title: GetIntegrationFile (wcf door)
- id: door-file-transfer-svc-service-update-integration-file
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L55
  title: UpdateIntegrationFile (wcf door)
- id: door-file-transfer-svc-service-update-integration-file-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L55
  title: UpdateIntegrationFile (wcf door)
- id: src-5
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business/CommandHandlers/FtsIntegrationFileCommandHandler.cs
  title: FtsIntegrationFileCommandHandler.cs (2 methods)
- id: footprint
  resource: /technical/flows/manage-integration-files/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:manage-integration-files
  generation:
    status: needs-review
    attempts: 2
    problems: 1
  rule_prefix: F03
  rules: 13
  doors:
  - file-transfer-svc-query-service-get-integration-file
  - file-transfer-svc-query-service-get-integration-file-2
  - file-transfer-svc-service-update-integration-file
  - file-transfer-svc-service-update-integration-file-2
  tables: 1
  procedures: 0
  components:
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  - backend-configure-source-locations-and-destinations-api
  - shared-contracts-commands
  body_sha256: 130b0cf03ce49568bd6785ce8892d14cf2c9c620486a8ca10e83edd175a15887
---

﻿# Rules: Manage integration file definitions

## Sources

| Label | Source |
| --- | --- |
| `SVCQ-UA3` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs` |
| `SVCQ-HPP` | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs` |
| `SVCC-UA3` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs` |
| `SVCC-HPP` | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs` |
| `Q-UA3` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\GetIntegrationFileQuery.cs` |
| `QPARMS-UA3` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\IntegrationFileQueryParms.cs` |
| `QEX-UA3` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\IntegrationFileQueryExecutor.cs` |
| `MODEL` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Domain\FtsIntegrationFileModel.cs` |
| `UPDCMD` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateIntegrationFileCommand.cs` |
| `CMDH-UA3` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs` |
| `PROJ-UA3` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsIntegrationFileProjectionWriter.cs` |
| `ENTITY` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Entities\FtsIntegrationFile.cs` |
| `COMMON` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs` |
| `EXCONST` | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs` |

## Doors and routing

**F03-001** A request to look up integration file definitions (door `file-transfer-svc-query-service-get-integration-file`) is routed through the File Transfer query router.
- Doors: file-transfer-svc-query-service-get-integration-file
- Source: SVCQ-UA3:50-58
- Legacy: The door calls `TryRouteQuery(query)` which calls `TryRoute(query)` in the CQRS base.
- Decision: keep

**F03-002** A request to look up integration file definitions (door `file-transfer-svc-query-service-get-integration-file-2`) is routed through the File Transfer query router.
- Doors: file-transfer-svc-query-service-get-integration-file-2
- Source: SVCQ-HPP:41-49
- Legacy: The door calls `TryRouteQuery(query)` which calls `TryRoute(query)` in the CQRS base.
- Decision: keep

**F03-003** A request to update an integration file definition (door `file-transfer-svc-service-update-integration-file`) is routed through the File Transfer command router.
- Doors: file-transfer-svc-service-update-integration-file
- Source: SVCC-UA3:55-72
- Legacy: The door calls `TryRouteCommand(command)` which calls `TryRoute(command)` in the CQRS base.
- Decision: keep

**F03-004** A request to update an integration file definition (door `file-transfer-svc-service-update-integration-file-2`) is routed through the File Transfer command router.
- Doors: file-transfer-svc-service-update-integration-file-2
- Source: SVCC-HPP:55-72
- Legacy: The door calls `TryRouteCommand(command)` which calls `TryRoute(command)` in the CQRS base.
- Decision: keep

## Lookup rules (integration file definitions)

**F03-005** Only active integration file definitions are returned by the lookup.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:31-35
- Legacy: The lookup includes `integrationFile.IsValid` in its filter, excluding inactive definitions.
- Decision: keep

**F03-006** When a specific integration file id is supplied, only that exact definition is returned; when no id is supplied, the lookup does not filter by id.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:31-33, QPARMS-UA3:18-21
- Legacy: The default id is `Guid.Empty`, and the lookup applies an equality filter only when the supplied id is not empty.
- Decision: keep

**F03-007** When a file name fragment is supplied, the lookup returns only definitions whose stored file name contains that fragment; when no fragment is supplied, the lookup does not filter by name.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:33-34
- Legacy: The lookup applies `FileName.Contains(fragment)` only when the fragment is not empty.
- Decision: keep

**F03-008** The lookup does not apply any explicit ordering to the returned definitions.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:18-21
- Legacy: Sorting returns the query as-is, so ordering is whatever the underlying store returns.
- Decision: keep
- ⚑ Flag: callers cannot rely on a stable or meaningful order

**F03-009** Each returned definition includes only: group id, outbound metadata definition id, configuration id, file name, processed flag, and integration file id; other model fields are left at their default values.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:35-43, MODEL:15-129
- Legacy: The lookup constructs `new FtsIntegrationFileModel { ... }` and does not assign `IsValid`, `CreationDate`, or `LastModifiedDate` (among others), so they remain defaults.
- Decision: keep
- ⚑ Flag: consumers may see default values for fields that exist on the model but are not populated by this lookup

## Update rules (recording processing against a definition)

**F03-010** An update request must include a configuration id; when it is missing, the update fails with a business validation error (message `Missing ConfigId`, error type `Command Execution Error`, error code `E`).
- Doors: file-transfer-svc-service-update-integration-file, file-transfer-svc-service-update-integration-file-2
- Source: UPDCMD:61-73, COMMON:21-22, COMMON:39-46, EXCONST:17-46
- Legacy: When `ConfigId` is null, the command throws a `BusinessException` with a validation entry for `ConfigId` and the message `Missing ConfigId`.
- Decision: keep

**F03-011** A successful update emits an `IntegrationFileUpdated` event with the provided integration file id, file name, configuration id, active flag, and the operator id from the requestor.
- Doors: file-transfer-svc-service-update-integration-file, file-transfer-svc-service-update-integration-file-2
- Source: CMDH-UA3:54-58, CMDH-UA3:80-89
- Legacy: The handler calls `Validate()` then builds the event using the command values and `Requestor.IdentifierId`.
- Decision: keep

**F03-012** Applying an integration file update records the configuration id, marks the definition processed, stores the active flag, stores the operator id, and sets last-modified time to the current UTC time.
- Doors: file-transfer-svc-service-update-integration-file, file-transfer-svc-service-update-integration-file-2
- Source: PROJ-UA3:40-52
- Legacy: The projection update sets `ConfigId`, sets `IsProcessed = true`, sets `IsValid`, sets `LastModifiedDate = DateTime.UtcNow`, and sets `OperatorId`.
- Decision: keep
- ⚑ Flag: the update request includes a file name, but this projection does not update the stored file name

**F03-013** When an update marks a definition inactive, that definition no longer appears in later lookups.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2, file-transfer-svc-service-update-integration-file, file-transfer-svc-service-update-integration-file-2
- Source: PROJ-UA3:40-52, QEX-UA3:31-35
- Legacy: The update stores the active/inactive flag, and the lookup returns only definitions where `IsValid` is true.
- Decision: keep