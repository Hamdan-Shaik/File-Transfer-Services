---
type: Business Rules
title: Rules
description: 63 business rules (F02-NNN) of the flow, each with its legacy source lines.
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
tags:
- api
- flow
- provider-file-transfer-v2
- wcf
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T08:47:43Z
sources:
- id: door-file-transfer-svc-query-service-get-ftp-servers
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L35
  title: GetFtpServers (wcf door)
- id: door-file-transfer-svc-query-service-get-ftp-servers-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L26
  title: GetFtpServers (wcf door)
- id: door-file-transfer-svc-query-service-get-ftp-servers-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L26
  title: GetFtpServers (wcf door)
- id: door-file-transfer-svc-query-service-controller-get-ftp-servers
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L33
  title: GetFtpServers (api door)
- id: door-file-transfer-svc-service-add-ftp-server
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L43
  title: AddFtpServer (wcf door)
- id: door-file-transfer-svc-service-add-ftp-server-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L44
  title: AddFtpServer (wcf door)
- id: door-file-transfer-svc-service-add-ftp-server-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L43
  title: AddFtpServer (wcf door)
- id: door-file-transfer-svc-service-controller-add-ftp-server
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L40
  title: AddFtpServer (api door)
- id: door-file-transfer-svc-service-update-ftp-server
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L49
  title: UpdateFtpServer (wcf door)
- id: door-file-transfer-svc-service-update-ftp-server-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L50
  title: UpdateFtpServer (wcf door)
- id: door-file-transfer-svc-service-update-ftp-server-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L49
  title: UpdateFtpServer (wcf door)
- id: door-file-transfer-svc-service-controller-update-ftp-server
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L46
  title: UpdateFtpServer (api door)
- id: door-file-transfer-svc-service-invalidate-ftp-server
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L61
  title: InvalidateFtpServer (wcf door)
- id: door-file-transfer-svc-service-invalidate-ftp-server-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L56
  title: InvalidateFtpServer (wcf door)
- id: door-file-transfer-svc-service-invalidate-ftp-server-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L61
  title: InvalidateFtpServer (wcf door)
- id: door-file-transfer-svc-service-controller-invalidate-ftp-server
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L52
  title: InvalidateFtpServer (api door)
- id: door-file-transfer-controller-get-ftp-server-details
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L111
  title: GetFtpServerDetails (api door)
- id: door-file-transfer-controller-get-ftpserver-credentials
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L26
  title: GetFTPServerCredentials (api door)
- id: door-file-transfer-controller-add-ftpserver-credentials
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L43
  title: AddFTPServerCredentials (api door)
- id: door-file-transfer-controller-create-ftpserver-credentials
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L52
  title: CreateFTPServerCredentials (api door)
- id: door-file-transfer-controller-update-ftpserver-credentials
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L68
  title: UpdateFTPServerCredentials (api door)
- id: footprint
  resource: /technical/flows/manage-ftp-servers-and-credentials/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:manage-ftp-servers-and-credentials
  generation:
    status: done
    attempts: 2
  rule_prefix: F02
  rules: 63
  doors:
  - file-transfer-svc-query-service-get-ftp-servers
  - file-transfer-svc-query-service-get-ftp-servers-2
  - file-transfer-svc-query-service-get-ftp-servers-3
  - file-transfer-svc-query-service-controller-get-ftp-servers
  - file-transfer-svc-service-add-ftp-server
  - file-transfer-svc-service-add-ftp-server-2
  - file-transfer-svc-service-add-ftp-server-3
  - file-transfer-svc-service-controller-add-ftp-server
  - file-transfer-svc-service-update-ftp-server
  - file-transfer-svc-service-update-ftp-server-2
  - file-transfer-svc-service-update-ftp-server-3
  - file-transfer-svc-service-controller-update-ftp-server
  - file-transfer-svc-service-invalidate-ftp-server
  - file-transfer-svc-service-invalidate-ftp-server-2
  - file-transfer-svc-service-invalidate-ftp-server-3
  - file-transfer-svc-service-controller-invalidate-ftp-server
  - file-transfer-controller-get-ftp-server-details
  - file-transfer-controller-get-ftpserver-credentials
  - file-transfer-controller-add-ftpserver-credentials
  - file-transfer-controller-create-ftpserver-credentials
  - file-transfer-controller-update-ftpserver-credentials
  tables: 1
  procedures: 0
  components:
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  - backend-configure-source-locations-and-destinations-api
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - backend-data-file-transfer
  - data-data-access-entities
  - data-data-access-entities-3
  - shared-contracts-commands
  - shared-contracts-utilities
  - ui-configure-source-locations-and-destinations-screens
  - ui-manage-ftp-servers-and-credentials-screens
  body_sha256: c074bc2330f41ded7db15a0b6092918eaacf335f53416d1bdb163add3d3faac7
---

# Rules

Written in parts (`PARTS.yaml`); each part's rules follow under its name.

## View FTP servers and details

﻿# Rules: view-ftp-servers

### BAS doors (GetFtpServers)

**F02-001** When the WCF `GetFtpServers` operation is called, it routes the provided `FtpServerQuery` through CQRS routing (`TryRoute`) and returns the routed query result.
- Doors: file-transfer-svc-query-service-get-ftp-servers
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:35-38, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55-58
- Legacy: Calls `TryRouteQuery(query)`, which delegates to `TryRoute(query)` on the CQRS base class, and returns the result cast back to `FtpServerQuery`.
- Decision: keep

**F02-002** When the CoreWCF `GetFtpServers` operation is called in the FileTransfer API query service, it routes the provided `FtpServerQuery` through CQRS routing (`TryRoute`) and returns the routed query result.
- Doors: file-transfer-svc-query-service-get-ftp-servers-2
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:26-29, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41-44
- Legacy: Calls `TryRouteQuery(query)`, which delegates to `TryRoute(query)` on the CQRS base class, and returns the result cast back to `FtpServerQuery`.
- Decision: keep

**F02-003** When the CoreWCF `GetFtpServers` operation is called in the FileTransferSvcCore query service, it routes the provided `FtpServerQuery` through CQRS routing (`TryRoute`) and returns the routed query result.
- Doors: file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:26-29, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46-49
- Legacy: Calls `TryRouteQuery(query)`, which delegates to `TryRoute(query)` on the CQRS base class, and returns the result cast back to `FtpServerQuery`.
- Decision: keep

**F02-004** The HTTP API door for `GetFtpServers` always replies `200 OK` with the result of `IFileTransferSvcQueryService.GetFtpServers`.
- Doors: file-transfer-svc-query-service-controller-get-ftp-servers
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:33-37
- Legacy: Calls `Ok(this.fileTransferSvcQueryService.GetFtpServers(query))` without additional validation or error mapping.
- Decision: keep

### FTP_SERVER query rules

**F02-005** The FTP server search reads from `FTP_SERVER` and filters by validity when `Where.IsValid` is provided.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:44-46
- Legacy: Builds the base query from `context.Set<FtpServer>()` and applies `identifier.IsValid == query.Where.IsValid` unless `query.Where.IsValid` is null.
- Decision: keep

**F02-006** FTP server listing applies different sorting depending on which BAS query door is used: the legacy query door defaults to sorting by `FtpServerName` ascending when no sort is supplied; the newer query doors do not apply sorting.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:27-35, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:159-168, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:18-21
- Legacy: For `file-transfer-svc-query-service-get-ftp-servers`, when `query.SortCriteria` is null/empty the executor sets a single default sort of `FtpServerName` + `Asc` and orders via a dynamic sort; for `file-transfer-svc-query-service-get-ftp-servers-2` and `file-transfer-svc-query-service-get-ftp-servers-3`, the executor's `ApplySorting(...)` returns `baseQuery` without ordering.
- Review: GAP-001
- Review: GAP-004
- Decision: keep

**F02-007** FTP server listing applies different `Where.FtpServerId` filtering depending on which BAS query door is used: the legacy query door applies `Where.FtpServerId` as an exact-match filter; the newer query doors do not filter by FTP server id.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:80-93, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:30-33
- Legacy: For `file-transfer-svc-query-service-get-ftp-servers`, when `Where.FtpServerId` is not `Guid.Empty` and its search mode is `EqualTo`, the executor adds a predicate `p.FtpServerId == new Guid(ftpServerIdValue)`; for `file-transfer-svc-query-service-get-ftp-servers-2` and `file-transfer-svc-query-service-get-ftp-servers-3`, the base query only filters by `Where.IsValid` and `Where.FtpServerName`.
- Review: GAP-002
- Review: GAP-005
- Decision: keep

**F02-008** FTP server listing applies different text filtering depending on which BAS query door is used: the legacy query door supports case-insensitive, trimmed matching for `Where.FtpServerName`, `Where.FtpDescription`, and `Where.FtpUser` using the configured search mode (`Contains`, `StartsWith`, `EndsWith`, `EqualTo`); the newer query doors only apply a case-insensitive exact match on `Where.FtpServerName` and do not filter by description or user.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:95-140, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:30-33
- Legacy: For `file-transfer-svc-query-service-get-ftp-servers`, when a field is non-empty the executor uppercases and trims the input value and applies the search-mode-specific string operation against `ToUpper().Trim()` on the stored field; for `file-transfer-svc-query-service-get-ftp-servers-2` and `file-transfer-svc-query-service-get-ftp-servers-3`, the base query only compares `identifier.FtpServerName.ToUpper() == query.Where.FtpServerName.ToUpper()` when `Where.FtpServerName` is non-empty.
- Review: GAP-003
- Review: GAP-006
- Decision: keep

**F02-009** Each matching FTP server result projects a fixed set of fields from `FTP_SERVER` into the returned `FtpServerModel`, including credentials fields.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:44-63
- Legacy: Selects columns into `FtpServerModel`, including `FtpPassword`, `FtpUser`, `FtpServerIp`, `FtpServerName`, and proxy/port fields.
- Decision: keep
- ⚑ Flag: `FtpPassword` is selected from `FTP_SERVER` and included in the query result model.

### UX door (GetFtpServerDetails)

**F02-010** The UX door for viewing FTP server details is an HTTP `GET` endpoint that requires an anti-forgery token.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:111-113
- Legacy: The action `GetFtpServerDetails()` is decorated with `[HttpGet]` and `[ValidateAntiForgeryTokenAttribute]`.
- Decision: keep

**F02-011** The UX controller resolves the file-transfer query data provider from tenant configuration using the `FileTransferQueryServiceDataProvider` component name.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:119-121
- Legacy: Calls `TenantConfigurationManager.Resolve<IFileTransferQueryServiceDataProvider>(FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider)` and assigns it to `this.FileDataProvider`.
- Decision: keep

**F02-012** To fetch FTP servers for the UI, the UX controller creates a request with a generated requestor and `Data = null`, then calls `FileDataProvider.GetFtpServers`.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:123-130
- Legacy: Sets `Requestor = UserConfigurationManager.GenerateRequestor()` and `Data = null`, then calls `GetFtpServers(request)` and captures `resp.Result`.
- Decision: keep

**F02-013** When the provider result status is `OK`, the UX controller returns `200 OK` with a list of items where `Name` is the FTP server name and `ID` is the FTP server GUID as a string.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:131-151
- Legacy: Builds `ListItemModel` items with `DisplayFormat = Name`, `Name = dataList.FtpServerName`, and `ID = dataList.FtpServerId.ToString()`, then returns them as the HTTP response body.
- Decision: keep

**F02-014** When the provider result status is not `OK` (or the provider data is null), the UX controller returns `400 BadRequest` with the provider `ResultModel`.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:152-155
- Legacy: Returns `Request.CreateResponse<ResultModel>(HttpStatusCode.BadRequest, resultModel)`.
- Decision: keep

### UX provider (GetFtpServers call into BAS)

**F02-015** The UX data provider always queries the BAS service for FTP servers with `Where.IsValid = true`.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:225-233
- Legacy: Constructs a `FileTransferSvcQueryService.FtpServerQuery` with `Where.IsValid = true` and does not populate additional `Where` filters.
- Decision: keep

**F02-016** The UX data provider calls the BAS `GetFtpServers` service via a WCF channel, maps each returned result item to an internal `FtpServerModel`, and formats errors on exceptions.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:237-265, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:529-543
- Legacy: Uses `ChannelFactory<IFileTransferSvcQueryService>` and `serviceProxy.GetFtpServers(serviceQuery)`; on `FaultException<ServiceException>` or general `Exception`, logs the exception and replaces `response.Result` via `FormatErrorResponseResult(...)`.
- Decision: keep

## Add, update, and invalidate FTP servers

﻿# Rules: maintain-ftp-servers

### Doors

**F02-017** The WCF/service entrypoints for AddFtpServer route the given command to the command router and return the routed result cast to `FtpServerAdded`.
- Doors: file-transfer-svc-service-add-ftp-server, file-transfer-svc-service-add-ftp-server-2, file-transfer-svc-service-add-ftp-server-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:43-46, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69-72, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:44-47, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64-67, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:43-46, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69-72
- Legacy: The door method returns `TryRouteCommand(command) as FtpServerAdded`, and `TryRouteCommand` delegates to `TryRoute`.
- Decision: keep

**F02-018** The HTTP controller endpoint for AddFtpServer responds `200 OK` containing the result of calling the service’s `AddFtpServer(command)`.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:40-44
- Legacy: The controller wraps `fileTransferSvcService.AddFtpServer(command)` in `Ok(...)`.
- Decision: keep

**F02-019** The WCF/service entrypoints for UpdateFtpServer route the given command to the command router and return the routed result cast to `FtpServerUpdated`.
- Doors: file-transfer-svc-service-update-ftp-server, file-transfer-svc-service-update-ftp-server-2, file-transfer-svc-service-update-ftp-server-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:49-52, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69-72, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:50-53, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64-67, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:49-52, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69-72
- Legacy: The door method returns `TryRouteCommand(command) as FtpServerUpdated`, and `TryRouteCommand` delegates to `TryRoute`.
- Decision: keep

**F02-020** The HTTP controller endpoint for UpdateFtpServer responds `200 OK` containing the result of calling the service’s `UpdateFtpServer(command)`.
- Doors: file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:46-50
- Legacy: The controller wraps `fileTransferSvcService.UpdateFtpServer(command)` in `Ok(...)`.
- Decision: keep

**F02-021** The WCF/service entrypoints for InvalidateFtpServer route the given command to the command router and return the routed result cast to `FtpServerInvalidated`.
- Doors: file-transfer-svc-service-invalidate-ftp-server, file-transfer-svc-service-invalidate-ftp-server-2, file-transfer-svc-service-invalidate-ftp-server-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:61-64, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69-72, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:56-59, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64-67, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:61-64, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69-72
- Legacy: The door method returns `TryRouteCommand(command) as FtpServerInvalidated`, and `TryRouteCommand` delegates to `TryRoute`.
- Decision: keep

**F02-022** The HTTP controller endpoint for InvalidateFtpServer responds `200 OK` containing the result of calling the service’s `InvalidateFtpServer(command)`.
- Doors: file-transfer-svc-service-controller-invalidate-ftp-server
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:52-56
- Legacy: The controller wraps `fileTransferSvcService.InvalidateFtpServer(command)` in `Ok(...)`.
- Decision: keep

### Add FTP server

**F02-023** Handling an AddFtpServer command validates the command and fails the request if validations are present.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42-47, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93-101, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44-49, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95-103
- Legacy: The handler calls `ValidateFtpServerCommand(command)`, which calls `command.Validate()` and may throw during `HandleValidations()`.
- Decision: keep

**F02-024** For AddFtpServer and UpdateFtpServer, the command type for update inherits the add command type, so both operations share the same add-command validation rules.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateFtpServerCommand.cs:14-15, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateFtpServerCommand.cs:14-15
- Legacy: `UpdateFtpServerCommand : AddFtpServerCommand`.
- Decision: keep

**F02-025** AddFtpServer/UpdateFtpServer treat these string properties as required (null or empty values are considered missing): `FtpServerName`, `FtpDescription`, `FtpServerIp`, `FtpTransferMode`, `FtpUser`, `FtpPassword`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:51-53, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:63-65, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:75-77, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:87-89, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:99-101, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:111-113
- Legacy: These properties are annotated with `[Required(AllowEmptyStrings = false)]` and are checked by `ValidateRequiredFields()`.
- Decision: keep

**F02-026** When required-field validation fails, one validation entry is added per failing property with message text `Missing field: <PropertyName>`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92-101, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92-100, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:46-47, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39-40
- Legacy: The code uses `Validator.TryValidateObject(...)` and formats each message as `"Missing field: {memberName}"`.
- Decision: keep

**F02-027** If any validations are present after validation runs, the command fails by throwing `BusinessException` with message `Command Execution Error` and business error code `E`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72-80, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72-80, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:21-22, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39-40, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:20-21, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:33-34
- Legacy: `HandleValidations()` throws a `BusinessException(validations, "Command Execution Error", "E")` when `Validations.Any()` is true.
- Decision: keep

**F02-028** AddFtpServer/UpdateFtpServer reject an empty GUID FTP server id (`FtpServerId == Guid.Empty`).
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220-228, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:219-226
- Legacy: `ValidateFtpServerId()` adds a validation when `FtpServerId.Equals(Guid.Empty)`.
- Decision: keep

**F02-029** AddFtpServer/UpdateFtpServer validate `FtpServerIp` against the regex pattern `\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231-245, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:229-242, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:14-17, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:13-16
- Legacy: When `FtpServerIp` is non-empty, `ValidateFtpServerIp()` calls `Regex.IsMatch(FtpServerIp, IpAddressPattern)` and adds a validation on mismatch.
- Decision: keep

**F02-030** If `FtpProxyAddress` is provided (non-empty), AddFtpServer/UpdateFtpServer require `FtpProxyPort` to be present and greater than 0.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247-255, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:244-251
- Legacy: `ValidateProxyPort()` adds a validation when `!string.IsNullOrEmpty(FtpProxyAddress)` and `(!FtpProxyPort.HasValue || FtpProxyPort <= 0)`.
- Decision: keep

**F02-062** AddFtpServer/UpdateFtpServer do not validate that `FtpPort` is greater than 0.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:115-126, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:125-137, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:211-216, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:210-215
- Legacy: `FtpPort` is provided on the command as a `short`, but `SetCommandValidations()` only calls `ValidateFtpServerId()`, `ValidateFtpServerIp()`, and `ValidateProxyPort()`.
- Review: GAP-007
- Decision: keep

**F02-031** AddFtpServer/UpdateFtpServer reject a duplicate FTP server name by checking for an existing valid FTP server record with the same `FtpServerName` but a different `FtpServerId`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:97-107, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114-123, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:99-109, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116-125, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:95-108, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:30-33
- Legacy: The handler queries FTP servers with `IsValid = true` and `FtpServerName = command.FtpServerName`, then considers it a duplicate when any returned row has `FtpServerId != command.FtpServerId`.
- Decision: keep
- ⚑ Flag: The two query executors compare `FtpServerName` case-insensitively, but only the HP.HSP.UA3 executor trims whitespace during comparison.

**F02-032** AddFtpServer returns an event populated from the command and forces `IsValid = true`, with `OperatorId` set to the requestor identifier.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:64-71, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:66-73
- Legacy: The handler maps command fields onto `FtpServerAdded`, then sets `ftpServerAdded.IsValid = true` and `ftpServerAdded.OperatorId = command.Requestor.IdentifierId`.
- Decision: keep

### Update FTP server

**F02-033** UpdateFtpServer returns an event populated from the command, with `OperatorId` set to the requestor identifier.
- Doors: file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:74-81, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:76-83
- Legacy: The handler maps command fields onto `FtpServerUpdated`, then sets `ftpServerUpdated.OperatorId = command.Requestor.IdentifierId`.
- Decision: keep

### Invalidate FTP server

**F02-034** InvalidateFtpServer does not run command validation; it returns an event containing `FtpServerId` and forces `IsValid = false`, with `OperatorId` set to the requestor identifier.
- Doors: file-transfer-svc-service-controller-invalidate-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:56-59, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:83-91, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:58-61, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:85-93, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\InvalidateFtpServerCommand.cs:14-15, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\InvalidateFtpServerCommand.cs:14-15
- Legacy: The handler returns `new FtpServerInvalidated { FtpServerId = command.FtpServerId, IsValid = false, OperatorId = command.Requestor.IdentifierId }` and does not call `command.Validate()`.
- Decision: keep

### Projection (FTP_SERVER write model)

**F02-035** When a `FtpServerAdded` event is handled, a new FTP server row is created with `CreationDate` and `LastModifiedDate` set to `DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74-82, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74-82
- Legacy: The projection writer maps the event to a new `FtpServer` entity, sets both timestamps to `DateTime.UtcNow`, and calls `Add(newFtpServer)`.
- Decision: keep

**F02-036** When a `FtpServerUpdated` event is handled, the existing FTP server row is updated by id and `LastModifiedDate` is set to `DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:39-57, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:39-57
- Legacy: The projection writer updates all FTP server fields from the event (including credentials and proxy fields), sets `LastModifiedDate = DateTime.UtcNow`, and sets `OperatorId = @event.OperatorId`.
- Decision: keep

**F02-037** When a `FtpServerInvalidated` event is handled, the existing FTP server row is updated by id by setting `IsValid` (to false), `OperatorId`, and `LastModifiedDate = DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-invalidate-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:60-69, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:60-69
- Legacy: The projection writer sets `IsValid = @event.IsValid`, sets `OperatorId = @event.OperatorId`, and sets `LastModifiedDate = DateTime.UtcNow`.
- Decision: keep

## Manage FTP server credentials


### Add / route

**F02-038** When `FtpServerId` is empty on an Add request, the request is treated as a create; otherwise it is treated as an update.
- Doors: file-transfer-controller-add-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:47-49
- Legacy: `AddFTPServerCredentials` calls `CreateFTPServerCredentials` when `ftpServerModel.FtpServerId` is null/empty, else calls `UpdateFTPServerCredentials`.
- Decision: keep

### Get FTP server credentials

**F02-039** A Get request builds a `SearchCriteriaModel` from the Kendo `DataSourceRequest`, builds a `RequestModel<FtpServerModel>` from that criteria, and queries for FTP server credentials.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:30-36
- Legacy: The controller resolves `IFileTransferQueryServiceDataProvider`, calls `ContextHelper.InitializeSearchRequest(request)`, then calls `GetFTPServerCredentials(requestModel, searchCriteria)`.
- Decision: keep

**F02-040** When the query response is missing (null) or has a null `Data` list, the Get request returns HTTP 404 with an empty list.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:38-40
- Legacy: The controller returns `NotFound` with `new List<FtpServerModel>()` unless both `ftpServerResponse` and `ftpServerResponse.Data` are non-null.
- Decision: keep

**F02-041** When the query response result status is `Error`, the Get request returns HTTP 400 with the `ResultModel`; otherwise it returns HTTP 200 with a Kendo `DataSourceResult` containing `Data` and `Total`.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:118-120, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:140-146
- Legacy: `BuildResponse` maps `StatusCodeType.Error` to `BadRequest` and otherwise returns `OK` with `BuildDataSourceResult`, which sets `Data` and `Total`.
- Decision: keep

**F02-042** When building the FTP server search request, missing search fields (or missing matching field names) are treated as empty strings for `FtpServerName`, `FtpDescription`, and `FtpUser`.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:96-102, ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs:33-36
- Legacy: `BuildRequest` calls `SearchHelper.GetFieldValue(...)`; `GetFieldValue` returns `string.Empty` when there is no criteria or the field is not found.
- Decision: keep

**F02-043** When the search request contains a non-empty `IsValid` field value, the server filters to `IsValid = true`; otherwise it does not set an `IsValid` filter.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:101-102
- Legacy: `BuildRequest` sets `IsValid` to `true` when the `IsValid` search field value is not empty, else sets it to null.
- Decision: keep
- âš‘ Flag: the incoming `IsValid` search field value is not parsed; any non-empty value forces `IsValid=true` and there is no way to request `IsValid=false` via this code path.

**F02-044** A search field lookup returns the first matching field by name and converts its value to a string; if no matching field exists, it returns an empty string.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs:33-36
- Legacy: `GetFieldValue` checks `SearchFields` for emptiness, then `FirstOrDefault(filter => filter.FieldName == fieldName)` and returns `FieldValue.ToString()`.
- Decision: keep

**F02-045** When querying FTP server credentials, passwords are removed from results by default.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:858-860, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:995
- Legacy: If `request.Data.RemovePassword` is null, the provider treats it as `true` and maps each result with `FtpPassword = string.Empty`.
- Decision: keep

**F02-046** When paging and sorting criteria are present, the query uses the provided page size and page number, and (when multiple sorts are provided) only the last sort criterion is used.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:924-938, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:941-951
- Legacy: `SetFTPServersPaging` sets `PageSize` and `CurrentPage` from `PagingCriteria`; `SetSortCriteria` uses `SortCriteria[Count - 1]` and maps direction to Asc/Desc.
- Decision: keep

**F02-047** Each search field's `SearchMode` is translated into a service-side filter type (Contains, StartsWith, EndsWith, or EqualTo), defaulting to EqualTo.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:889-905, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:908-921
- Legacy: `BuildSearchOptions` creates one `SearchModeOption` per `SearchFieldModel`, and `GetSearchType` maps modes with a default of `EqualTo`.
- Decision: keep

**F02-048** The Get credentials response reports `TotalRowsAvailable` based on the service-reported row count, not just the count of returned results.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:955-960
- Legacy: `PopulateResponseResults` sets `PagingResults.RowCount = Results.Count` and `PagingResults.TotalRowsAvailable = queryResults.RowCount`.
- Decision: keep

### Create FTP server credentials

**F02-049** When the create request model state is invalid, the create request returns HTTP 400 with the formatted model state errors.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:58-65
- Legacy: `CreateFTPServerCredentials` checks `ModelState.IsValid` and otherwise returns `BadRequest` with `FormatResultModel(ModelState)`.
- Decision: keep

**F02-050** When an add succeeds (service result status `OK`), the create request returns HTTP 200 with a localized success message stored in `Editor.SavedMessage` and `Editor.SaveActionText` set to `Success`.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:130-137, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:149-168
- Legacy: `SetResponse` routes `StatusCodeType.OK` to `FtpServerCredentialsSave`, which sets `Editor.SavedMessage` and `Editor.SaveActionText` and returns `OK`.
- Decision: keep

**F02-051** When adding a new FTP server, the service command assigns a new server id (`Guid.NewGuid()`) and uses the requestor identifier as the operator id.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:724-738
- Legacy: `BuildAddFtpServerCommand` sets `FtpServerId = Guid.NewGuid()` and `OperatorId = request.Requestor.IdentifierId`.
- Decision: keep

**F02-052** When the add service returns a non-null response, the server responds with a model containing the new `FtpServerId`, preserves the request's `Editor` object, and marks the result status as `OK`.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:650-661
- Legacy: `AddFtpServer` maps `serviceResponse.FtpServerId` into a new `FtpServerModel`, copies `request.Data.Editor`, and sets `response.Result.StatusCode = OK`.
- Decision: keep

### Update / invalidate FTP server credentials

**F02-053** When the update request model state is invalid, the update request returns HTTP 400 with the formatted model state errors.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:76-88
- Legacy: `UpdateFTPServerCredentials` checks `ModelState.IsValid` and otherwise returns `BadRequest` with `FormatResultModel(ModelState)`.
- Decision: keep

**F02-054** Update requests with `IsValid=true` call the Update operation; update requests with `IsValid=false` call the Invalidate operation.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:175-177
- Legacy: `UpdateFtpServer` calls `DataProvider.UpdateFtpServer` when `IsValid.Value` is true, else calls `DataProvider.InvalidateFtpServer`.
- Decision: keep

**F02-055** When an update/invalidate service response has no `Data`, the update request returns HTTP 400 with a single error message taken from the first service message.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:80-86, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:187-198
- Legacy: When `processorResult.Data` is null, the controller returns `BadRequest` with `FormatInvalidResponse(processorResult.Result.Messages[0].Message)`.
- Decision: keep

**F02-056** When updating an FTP server and no new password is provided, the server reuses the existing password fetched from the credentials query with `RemovePassword=false`.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:743-759, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:770-777, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:788-795
- Legacy: `BuildUpdateFtpServerCommand` uses `request.Data.FtpPassword` when non-empty, else calls `GetFTPServerPass`, which queries for the server with `RemovePassword=false` and uses the first returned record's `FtpPassword`.
- Decision: keep

**F02-063** When updating FTP server credentials with no new password, if the password lookup returns `OK` with an empty result list, the update request fails with an unhandled exception.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:444-469, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:743-779
- Legacy: `UpdateFtpServer` calls `BuildUpdateFtpServerCommand(request)` before its try/catch; `GetFTPServerPass` calls `resp.Data.First()` when the credentials query returns `OK` and `resp.Data != null`, without checking whether the list is empty.
- Review: GAP-008
- Decision: keep

**F02-057** Update/invalidate success messages differ: a valid update uses the Edit FTP server credentials success message, and an invalidation uses the Invalidate FTP server credentials success message.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:158-162
- Legacy: `FtpServerCredentialsSave` chooses the content id based on `ftpServerModel.IsValid.Value` for the edit action.
- Decision: keep

**F02-058** When the update service returns a non-null response, the server responds with a model containing `FtpServerId` and `IsValid`, preserves the request's `Editor` object, and marks the result status as `OK`.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:672-684
- Legacy: `UpdateFtpServer` maps `serviceResponse.FtpServerId` and `serviceResponse.IsValid`, copies `request.Data.Editor`, and sets `response.Result.StatusCode = OK`.
- Decision: keep

**F02-059** When the invalidate service returns a non-null response, the server responds with a model containing `FtpServerId` and `IsValid`, preserves the request's `Editor` object, and marks the result status as `OK`.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:695-707
- Legacy: `InvalidateFtpServer` maps `serviceResponse.FtpServerId` and `serviceResponse.IsValid`, copies `request.Data.Editor`, and sets `response.Result.StatusCode = OK`.
- Decision: keep

**F02-060** Invalidate requests build the invalidate command by parsing `FtpServerId` as a GUID and mapping the requestor.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:814-820
- Legacy: `BuildInvalidateFtpServerCommand` sets `FtpServerId = Guid.Parse(request.Data.FtpServerId)` and maps `Requestor`.
- Decision: keep

### Success message localization

**F02-061** Success messages are retrieved from the localization provider using the current user's locale code and the selected content id, and the returned message `Text` is used.
- Doors: file-transfer-controller-create-ftpserver-credentials, file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:180-185
- Legacy: `GetSuccessMessage` calls `LocalizationProvider.GetMessage(userLocaleCode, contentId).Text`.
- Decision: keep
