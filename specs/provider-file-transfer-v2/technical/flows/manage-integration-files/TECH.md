---
type: Technical Flow Map
title: 'Technical map: Manage integration file definitions'
description: 'How the legacy code implements the flow: call chains, classes, data, outside calls.'
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
  body_sha256: 5cff191c51c14798dc681cb71bd929e26456752274d8a8f0c0f7c296ce62c2d3
---

# Technical map: Manage integration file definitions

How the legacy code implements the flow `manage-integration-files`, as the code graph sees it. Written by the tool from `FOOTPRINT.yaml`, not by AI; the business view is in `FLOW.md` and `RULES.md`.

## Doors

| Door | Kind | Where |
| --- | --- | --- |
| `file-transfer-svc-query-service-get-integration-file` (GetIntegrationFile) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:50` |
| `file-transfer-svc-query-service-get-integration-file-2` (GetIntegrationFile) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:41` |
| `file-transfer-svc-service-update-integration-file` (UpdateIntegrationFile) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:55` |
| `file-transfer-svc-service-update-integration-file-2` (UpdateIntegrationFile) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:55` |

## Call chains

### `file-transfer-svc-query-service-get-integration-file`

- `FileTransferSvcQueryService.GetIntegrationFile` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:50`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55`

### `file-transfer-svc-query-service-get-integration-file-2`

- `FileTransferSvcQueryService.GetIntegrationFile` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:41`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46`

### `file-transfer-svc-service-update-integration-file`

- `FileTransferSvcService.UpdateIntegrationFile` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:55`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69`
  - routed to: `FtsIntegrationFileCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs:54`
    - `FtsIntegrationFileCommandHandler.BuildUpdateIntegrationFileEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs:80`
      - reads table `orm:FTS_INTEGRATION_FILE`
    - `UpdateIntegrationFileCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateIntegrationFileCommand.cs:56`
      - `UpdateIntegrationFileCommand.ValidateConfigId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateIntegrationFileCommand.cs:61` **- walk stops here**
  - routed to: `FtsIntegrationFileCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs:59`
    - `UpdateIntegrationFileCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateIntegrationFileCommand.cs:56` (see above)
    - `FtsIntegrationFileCommandHandler.BuildUpdateIntegrationFileEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs:85`
      - reads table `orm:FTS_INTEGRATION_FILE` (see above)

### `file-transfer-svc-service-update-integration-file-2`

- `FileTransferSvcService.UpdateIntegrationFile` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:55`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69`
  - routed to: `FtsIntegrationFileCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs:54`
    - `FtsIntegrationFileCommandHandler.BuildUpdateIntegrationFileEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs:80`
      - reads table `orm:FTS_INTEGRATION_FILE`
    - `UpdateIntegrationFileCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateIntegrationFileCommand.cs:56`
      - `UpdateIntegrationFileCommand.ValidateConfigId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateIntegrationFileCommand.cs:61` **- walk stops here**
  - routed to: `FtsIntegrationFileCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs:59`
    - `UpdateIntegrationFileCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateIntegrationFileCommand.cs:56` (see above)
    - `FtsIntegrationFileCommandHandler.BuildUpdateIntegrationFileEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsIntegrationFileCommandHandler.cs:85`
      - reads table `orm:FTS_INTEGRATION_FILE` (see above)

## Classes involved (by namespace; methods walked)

- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc`: `FileTransferSvcQueryService` (2), `FileTransferSvcService` (2)
- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers`: `FtsIntegrationFileCommandHandler` (2)
- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands`: `UpdateIntegrationFileCommand` (2)
- `HPP.FileTransfer.BAS.FileTransferSvc`: `FileTransferSvcQueryService` (2), `FileTransferSvcService` (2)
- `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers`: `FtsIntegrationFileCommandHandler` (2)

## Data

- table `orm:FTS_INTEGRATION_FILE` (reads)

## Calls into code the system does not own

- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.CommandServiceBase.TryRoute(HPP.Core.BAS.CQRS.Base.Command)` (1 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.QueryServiceBase.TryRoute<TQuery>(TQuery)` (1 call sites)
- **** : `CommandServiceBase.TryRoute` (1 call sites)
- **** : `QueryServiceBase.TryRoute` (1 call sites)

## Where the walk stops

The graph sees nothing these methods call: a router, a call into an unresolved package, reflection, configuration, or simply a leaf. Read the code from here.

- `UpdateIntegrationFileCommand.ValidateConfigId` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateIntegrationFileCommand.cs:61`

## Shared pieces used (not walked)

None.
