---
type: Generation Brief
title: 'Brief: flow manage-integration-files'
description: Instructions the tool gave the writer session for this flow.
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
  body_sha256: a568e267dbf189c02b25ff9a68b5ae1624870351a3ee53129e38ddc0a56b287c
---

# Brief: flow `manage-integration-files`

- Business name (from the grouping): **Manage integration file definitions**
- Technical map of what the graph saw: `technical/flows/manage-integration-files/TECH.md` (call chains, data, where it stops)
- Rule-id prefix: **F03** (first rule F03-001)
- Source root (read-only): `C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\legacy-kg\components`. Paths below are relative to it.
- Write exactly these files: `technical/flows/manage-integration-files/FLOW.md`, `technical/flows/manage-integration-files/RULES.md`, `technical/flows/manage-integration-files/DATA.md`, `technical/flows/manage-integration-files/LINKS.md`, and the flow's scenarios `business/manage-integration-files.feature`, the only business document: plain business language, no technical words (the playbook says how)
- Playbook: `inputs/playbook/flow.md`

## Doors (where the flow starts)

| Door id | Kind | Name | Where | Endpoints |
| --- | --- | --- | --- | --- |
| `file-transfer-svc-query-service-get-integration-file` | wcf | GetIntegrationFile | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:50` | - |
| `file-transfer-svc-query-service-get-integration-file-2` | wcf | GetIntegrationFile | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:41` | - |
| `file-transfer-svc-service-update-integration-file` | wcf | UpdateIntegrationFile | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:55` | - |
| `file-transfer-svc-service-update-integration-file-2` | wcf | UpdateIntegrationFile | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:55` | - |

## Code to read (the flow's own methods, by file)

- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs`: BuildUpdateIntegrationFileEvent (80), Handle (54)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateIntegrationFileCommand.cs`: Validate (56), ValidateConfigId (61)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs`: GetIntegrationFile (50), TryRouteQuery (55)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs`: TryRouteCommand (69), UpdateIntegrationFile (55)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs`: BuildUpdateIntegrationFileEvent (85), Handle (59)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs`: GetIntegrationFile (41), TryRouteQuery (46)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs`: TryRouteCommand (69), UpdateIntegrationFile (55)

## Stored procedures (definition found in the graph; check for later redefinitions)

None.

## Tables

- `orm:FTS_INTEGRATION_FILE` (reads)

## Shared pieces (used, owned elsewhere: describe what the flow uses them for, not their insides)

None.

## Calls into code the system does not own (contracts)

- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.CommandServiceBase.TryRoute(HPP.Core.BAS.CQRS.Base.Command)` (1 call sites)
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:71` command=<command>
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.QueryServiceBase.TryRoute<TQuery>(TQuery)` (1 call sites)
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:48` <HPP.Core.BAS.CQRS.Base.Query> query=<query>
- **** : `CommandServiceBase.TryRoute` (1 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:71` None=<command>
- **** : `QueryServiceBase.TryRoute` (1 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:57` None=<query>

## Where the graph's walk stops (it sees nothing these call: keep reading the code from here)

Often a router, a call into a package, reflection or configuration. Follow the code to the data.

- `UpdateIntegrationFileCommand.ValidateConfigId` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateIntegrationFileCommand.cs:61`

## Related flows of this module (share tables with this one)

None.

The full footprint is in `technical/flows/manage-integration-files/FOOTPRINT.yaml`.
