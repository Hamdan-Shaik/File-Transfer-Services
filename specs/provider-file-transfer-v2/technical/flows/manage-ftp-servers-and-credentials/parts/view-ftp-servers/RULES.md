---
type: Business Rules
title: technical/flows/manage-ftp-servers-and-credentials/parts/view-ftp-servers/RULES
description: 16 business rules (P1-NNN) of the flow, each with its legacy source lines.
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
  body_sha256: 650478be60a5b896d3d25918aa9f36827afca73dfd3c89f75d1c9f7dac010ebd
---

﻿# Rules: view-ftp-servers

## BAS doors (GetFtpServers)

**P1-001** When the WCF `GetFtpServers` operation is called, it routes the provided `FtpServerQuery` through CQRS routing (`TryRoute`) and returns the routed query result.
- Doors: file-transfer-svc-query-service-get-ftp-servers
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:35-38, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55-58
- Legacy: Calls `TryRouteQuery(query)`, which delegates to `TryRoute(query)` on the CQRS base class, and returns the result cast back to `FtpServerQuery`.
- Decision: keep

**P1-002** When the CoreWCF `GetFtpServers` operation is called in the FileTransfer API query service, it routes the provided `FtpServerQuery` through CQRS routing (`TryRoute`) and returns the routed query result.
- Doors: file-transfer-svc-query-service-get-ftp-servers-2
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:26-29, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41-44
- Legacy: Calls `TryRouteQuery(query)`, which delegates to `TryRoute(query)` on the CQRS base class, and returns the result cast back to `FtpServerQuery`.
- Decision: keep

**P1-003** When the CoreWCF `GetFtpServers` operation is called in the FileTransferSvcCore query service, it routes the provided `FtpServerQuery` through CQRS routing (`TryRoute`) and returns the routed query result.
- Doors: file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:26-29, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46-49
- Legacy: Calls `TryRouteQuery(query)`, which delegates to `TryRoute(query)` on the CQRS base class, and returns the result cast back to `FtpServerQuery`.
- Decision: keep

**P1-004** The HTTP API door for `GetFtpServers` always replies `200 OK` with the result of `IFileTransferSvcQueryService.GetFtpServers`.
- Doors: file-transfer-svc-query-service-controller-get-ftp-servers
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:33-37
- Legacy: Calls `Ok(this.fileTransferSvcQueryService.GetFtpServers(query))` without additional validation or error mapping.
- Decision: keep

## FTP_SERVER query rules

**P1-005** The FTP server search reads from `FTP_SERVER` and filters by validity when `Where.IsValid` is provided.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:44-46
- Legacy: Builds the base query from `context.Set<FtpServer>()` and applies `identifier.IsValid == query.Where.IsValid` unless `query.Where.IsValid` is null.
- Decision: keep

**P1-006** When the caller does not provide sort criteria, FTP server results default to sorting by `FtpServerName` ascending.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:27-35, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:159-168
- Legacy: If `query.SortCriteria` is null or empty, sets a single default sort criteria to `FtpServerName` + `Asc` and orders via a dynamic sort.
- Decision: keep

**P1-007** When the caller supplies a non-empty `Where.FtpServerId`, the search filters to an exact match on that FTP server ID.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:80-93
- Legacy: If `Where.FtpServerId` is not `Guid.Empty`, adds a predicate `p.FtpServerId == new Guid(ftpServerIdValue)` (with default search mode `EqualTo`).
- Decision: keep

**P1-008** When the caller supplies `Where.FtpServerName`, `Where.FtpDescription`, or `Where.FtpUser`, the search applies case-insensitive, trimmed matching using the configured search mode (`Contains`, `StartsWith`, `EndsWith`, `EqualTo`).
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:95-140
- Legacy: Uppercases and trims the input value and compares against `ToUpper().Trim()` on the stored field using the search-mode-specific string operation.
- Decision: keep

**P1-009** Each matching FTP server result projects a fixed set of fields from `FTP_SERVER` into the returned `FtpServerModel`, including credentials fields.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:44-63
- Legacy: Selects columns into `FtpServerModel`, including `FtpPassword`, `FtpUser`, `FtpServerIp`, `FtpServerName`, and proxy/port fields.
- Decision: keep
- ⚑ Flag: `FtpPassword` is selected from `FTP_SERVER` and included in the query result model.

## UX door (GetFtpServerDetails)

**P1-010** The UX door for viewing FTP server details is an HTTP `GET` endpoint that requires an anti-forgery token.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:111-113
- Legacy: The action `GetFtpServerDetails()` is decorated with `[HttpGet]` and `[ValidateAntiForgeryTokenAttribute]`.
- Decision: keep

**P1-011** The UX controller resolves the file-transfer query data provider from tenant configuration using the `FileTransferQueryServiceDataProvider` component name.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:119-121
- Legacy: Calls `TenantConfigurationManager.Resolve<IFileTransferQueryServiceDataProvider>(FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider)` and assigns it to `this.FileDataProvider`.
- Decision: keep

**P1-012** To fetch FTP servers for the UI, the UX controller creates a request with a generated requestor and `Data = null`, then calls `FileDataProvider.GetFtpServers`.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:123-130
- Legacy: Sets `Requestor = UserConfigurationManager.GenerateRequestor()` and `Data = null`, then calls `GetFtpServers(request)` and captures `resp.Result`.
- Decision: keep

**P1-013** When the provider result status is `OK`, the UX controller returns `200 OK` with a list of items where `Name` is the FTP server name and `ID` is the FTP server GUID as a string.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:131-151
- Legacy: Builds `ListItemModel` items with `DisplayFormat = Name`, `Name = dataList.FtpServerName`, and `ID = dataList.FtpServerId.ToString()`, then returns them as the HTTP response body.
- Decision: keep

**P1-014** When the provider result status is not `OK` (or the provider data is null), the UX controller returns `400 BadRequest` with the provider `ResultModel`.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:152-155
- Legacy: Returns `Request.CreateResponse<ResultModel>(HttpStatusCode.BadRequest, resultModel)`.
- Decision: keep

## UX provider (GetFtpServers call into BAS)

**P1-015** The UX data provider always queries the BAS service for FTP servers with `Where.IsValid = true`.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:225-233
- Legacy: Constructs a `FileTransferSvcQueryService.FtpServerQuery` with `Where.IsValid = true` and does not populate additional `Where` filters.
- Decision: keep

**P1-016** The UX data provider calls the BAS `GetFtpServers` service via a WCF channel, maps each returned result item to an internal `FtpServerModel`, and formats errors on exceptions.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:237-265, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:529-543
- Legacy: Uses `ChannelFactory<IFileTransferSvcQueryService>` and `serviceProxy.GetFtpServers(serviceQuery)`; on `FaultException<ServiceException>` or general `Exception`, logs the exception and replaces `response.Result` via `FormatErrorResponseResult(...)`.
- Decision: keep
