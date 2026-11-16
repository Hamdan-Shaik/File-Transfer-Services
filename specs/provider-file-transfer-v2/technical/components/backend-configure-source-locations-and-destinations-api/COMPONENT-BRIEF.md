---
type: Generation Brief
title: 'Brief: component backend-configure-source-locations-and-destinations-api'
description: Instructions the tool gave the writer session for the backend-configure-source-locations-and-destinations-api component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-configure-source-locations-and-destinations-api
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T06:28:48Z
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
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-configure-source-locations-and-destinations-api/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-configure-source-locations-and-destinations-api
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 49
  owned_rules: 22
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  - search-and-track-file-transfers
  depends_on:
  - backend-bas-file-transfer-api
  - backend-bas-file-transfer-svc
  - backend-bas-file-transfer-svc-2
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  deployable: fxfer-domain-api
  folder: src/Api
  body_sha256: 00080918f9e4ef2f2de32037e345c275a80b71367abfc783dcaadc66e5787ca3
---

# Brief: component `backend-configure-source-locations-and-destinations-api`

- API of the flow configure-source-locations-and-destinations (backend tier, 6 classes, 49 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-configure-source-locations-and-destinations-api/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-configure-source-locations-and-destinations-api/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Api`: write `services/fxfer-domain-api/src/Api` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

- `backend-bas-file-transfer-api`: `technical/components/backend-bas-file-transfer-api/`
- `backend-bas-file-transfer-svc`: `technical/components/backend-bas-file-transfer-svc/`
- `backend-bas-file-transfer-svc-2`: `technical/components/backend-bas-file-transfer-svc-2/`
- `backend-business-command-handlers`: `technical/components/backend-business-command-handlers/`
- `backend-business-command-handlers-2`: `technical/components/backend-business-command-handlers-2/`

## Flows it takes part in

- `configure-source-locations-and-destinations`: `technical/flows/configure-source-locations-and-destinations/` (FLOW.md, RULES.md) and its scenarios `business/configure-source-locations-and-destinations.feature`
- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`
- `manage-integration-files`: `technical/flows/manage-integration-files/` (FLOW.md, RULES.md) and its scenarios `business/manage-integration-files.feature`
- `search-and-track-file-transfers`: `technical/flows/search-and-track-file-transfers/` (FLOW.md, RULES.md) and its scenarios `business/search-and-track-file-transfers.feature`

## Packages it calls (contracts to keep)

- `hpp.core.bas.cqrs`

## Business rules it owns (22)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-034** The FileTransfer WCF service `FileTransferAdd` operation routes the incoming add command through the CQRS router and returns the resulting event as `FtsSourceLocationAdded`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:25-28
- Legacy: Returns `TryRouteCommand(command) as FtsSourceLocationAdded`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-035** The FileTransfer API-hosted service `FileTransferAdd` operation routes the incoming add command through the CQRS router and returns the resulting event as `FtsSourceLocationAdded`.
- Doors: file-transfer-svc-service-file-transfer-add-2
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:26-29
- Legacy: Returns `TryRouteCommand(command) as FtsSourceLocationAdded`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-036** The FileTransfer core service `FileTransferAdd` operation routes the incoming add command through the CQRS router and returns the resulting event as `FtsSourceLocationAdded`.
- Doors: file-transfer-svc-service-file-transfer-add-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:25-28
- Legacy: Returns `TryRouteCommand(command) as FtsSourceLocationAdded`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-055** When `UpdateFileTransferSourceLocation` is called through the WCF service surfaces, the flow routes the `FtsSourceLocationUpdateCommand` through `TryRouteCommand(...)` and returns the routed result cast as `FtsSourceLocationUpdated`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:31, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:32, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:31
- Legacy: The WCF methods are thin wrappers; they do no validation or mapping beyond `as FtsSourceLocationUpdated`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-057** When `InvalidateFileTransferSourceLocation` is called through the WCF service surfaces, the flow routes the `FtsSourceLocationInvalidateCommand` through `TryRouteCommand(...)` and returns the routed result cast as `FtsSourceLocationInvalidated`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:37, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:38, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:37
- Legacy: The WCF methods are thin wrappers; they do no validation or mapping beyond `as FtsSourceLocationInvalidated`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-079** The WCF `SearchSourceLocations` doors route the incoming `SourceLocationQuery` through `TryRouteQuery` and return the routed query result.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:25-28, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:16-19, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:16-19
- Legacy: Each method returns `this.TryRouteQuery(query) as SourceLocationQuery`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-081** The WCF `GetSourceLocations` doors route the incoming `GetSourceLocationQuery` through `TryRouteQuery` and return the routed query result.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:40-43, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:31-34, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:31-34
- Legacy: Each method returns `this.TryRouteQuery(query) as GetSourceLocationQuery`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-107** When applying a `FtsSourceLocationUpdated` event, the flow assumes `FtsSourceLocationsId` is a non-empty GUID string and parses it using `new Guid(...)` and `Guid.Parse(...)`; when it is missing or not a GUID string, the projection update throws and the source location is not updated.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Review: GAP-001
- Source: bas\\FileTransferSvc\\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\\EventHandlers\\FtsSourceLocationProjectionWriter.cs:91-94, bas\\FileTransferSvc\\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\\EventHandlers\\FtsSourceLocationProjectionWriter.cs:91-94
- Legacy: The handler performs the GUID parsing before calling `Update(...)` and does not guard against null or malformed values.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-108** When applying a `FtsSourceLocationInvalidated` event, the flow assumes `FtsSourceLocationsId` is a non-empty GUID string and parses it using `new Guid(...)` and `Guid.Parse(...)`; when it is missing or not a GUID string, the projection update throws and the source location is not invalidated.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Review: GAP-002
- Source: bas\\FileTransferSvc\\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\\EventHandlers\\FtsSourceLocationProjectionWriter.cs:145-148, bas\\FileTransferSvc\\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\\EventHandlers\\FtsSourceLocationProjectionWriter.cs:145-148
- Legacy: The handler performs the GUID parsing before calling `Update(...)` and does not guard against null or malformed values.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-001** When the WCF `GetFtpServers` operation is called, it routes the provided `FtpServerQuery` through CQRS routing (`TryRoute`) and returns the routed query result.
- Doors: file-transfer-svc-query-service-get-ftp-servers
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:35-38, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55-58
- Legacy: Calls `TryRouteQuery(query)`, which delegates to `TryRoute(query)` on the CQRS base class, and returns the result cast back to `FtpServerQuery`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-002** When the CoreWCF `GetFtpServers` operation is called in the FileTransfer API query service, it routes the provided `FtpServerQuery` through CQRS routing (`TryRoute`) and returns the routed query result.
- Doors: file-transfer-svc-query-service-get-ftp-servers-2
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:26-29, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41-44
- Legacy: Calls `TryRouteQuery(query)`, which delegates to `TryRoute(query)` on the CQRS base class, and returns the result cast back to `FtpServerQuery`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-003** When the CoreWCF `GetFtpServers` operation is called in the FileTransferSvcCore query service, it routes the provided `FtpServerQuery` through CQRS routing (`TryRoute`) and returns the routed query result.
- Doors: file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:26-29, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46-49
- Legacy: Calls `TryRouteQuery(query)`, which delegates to `TryRoute(query)` on the CQRS base class, and returns the result cast back to `FtpServerQuery`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-017** The WCF/service entrypoints for AddFtpServer route the given command to the command router and return the routed result cast to `FtpServerAdded`.
- Doors: file-transfer-svc-service-add-ftp-server, file-transfer-svc-service-add-ftp-server-2, file-transfer-svc-service-add-ftp-server-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:43-46, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69-72, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:44-47, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64-67, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:43-46, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69-72
- Legacy: The door method returns `TryRouteCommand(command) as FtpServerAdded`, and `TryRouteCommand` delegates to `TryRoute`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-019** The WCF/service entrypoints for UpdateFtpServer route the given command to the command router and return the routed result cast to `FtpServerUpdated`.
- Doors: file-transfer-svc-service-update-ftp-server, file-transfer-svc-service-update-ftp-server-2, file-transfer-svc-service-update-ftp-server-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:49-52, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69-72, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:50-53, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64-67, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:49-52, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69-72
- Legacy: The door method returns `TryRouteCommand(command) as FtpServerUpdated`, and `TryRouteCommand` delegates to `TryRoute`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-021** The WCF/service entrypoints for InvalidateFtpServer route the given command to the command router and return the routed result cast to `FtpServerInvalidated`.
- Doors: file-transfer-svc-service-invalidate-ftp-server, file-transfer-svc-service-invalidate-ftp-server-2, file-transfer-svc-service-invalidate-ftp-server-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:61-64, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69-72, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:56-59, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64-67, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:61-64, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69-72
- Legacy: The door method returns `TryRouteCommand(command) as FtpServerInvalidated`, and `TryRouteCommand` delegates to `TryRoute`.
- Decision: keep

From `technical/flows/manage-integration-files/RULES.md`:

**F03-001** A request to look up integration file definitions (door `file-transfer-svc-query-service-get-integration-file`) is routed through the File Transfer query router.
- Doors: file-transfer-svc-query-service-get-integration-file
- Source: SVCQ-UA3:50-58
- Legacy: The door calls `TryRouteQuery(query)` which calls `TryRoute(query)` in the CQRS base.
- Decision: keep

From `technical/flows/manage-integration-files/RULES.md`:

**F03-002** A request to look up integration file definitions (door `file-transfer-svc-query-service-get-integration-file-2`) is routed through the File Transfer query router.
- Doors: file-transfer-svc-query-service-get-integration-file-2
- Source: SVCQ-HPP:41-49
- Legacy: The door calls `TryRouteQuery(query)` which calls `TryRoute(query)` in the CQRS base.
- Decision: keep

From `technical/flows/manage-integration-files/RULES.md`:

**F03-003** A request to update an integration file definition (door `file-transfer-svc-service-update-integration-file`) is routed through the File Transfer command router.
- Doors: file-transfer-svc-service-update-integration-file
- Source: SVCC-UA3:55-72
- Legacy: The door calls `TryRouteCommand(command)` which calls `TryRoute(command)` in the CQRS base.
- Decision: keep

From `technical/flows/manage-integration-files/RULES.md`:

**F03-004** A request to update an integration file definition (door `file-transfer-svc-service-update-integration-file-2`) is routed through the File Transfer command router.
- Doors: file-transfer-svc-service-update-integration-file-2
- Source: SVCC-HPP:55-72
- Legacy: The door calls `TryRouteCommand(command)` which calls `TryRoute(command)` in the CQRS base.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-019** The WCF `SearchFileTransferMovement` doors route the incoming `FileTransferTrackingQuery` through the CQRS router (`TryRoute`) and return the routed query object.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:30-33, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55-58, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:21-24, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41-44, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:21-24, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46-49
- Legacy: Each door calls `TryRouteQuery(query)` and casts the result back to `FileTransferTrackingQuery`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-021** The WCF `SearchInboundTarget` doors route the incoming `InboundTargetQuery` through the CQRS router (`TryRoute`) and return the routed query object.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:45-48, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55-58, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:36-39, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41-44, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:36-39, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46-49
- Legacy: Each door calls `TryRouteQuery(query)` and casts the result back to `InboundTargetQuery`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-034** Inbound target query validation throws only if validation messages are present, but no messages are added by `Validate()`.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\InboundTargetQuery.cs:45-62
- Legacy: `Validate()` initializes `messages` to an empty list and throws a `BusinessException` only when `messages.Count > 0`.
- Decision: keep

