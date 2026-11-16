---
type: Business Rules
title: technical/flows/configure-source-locations-and-destinations/parts/find-source-locations/RULES
description: 28 business rules (P4-NNN) of the flow, each with its legacy source lines.
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController
tags:
- api
- flow
- provider-file-transfer-v2
- wcf
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T08:09:56Z
sources:
- id: door-file-transfer-controller-file-transfer-configuration-view
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L88
  title: FileTransferConfigurationView (api door)
- id: door-file-transfer-controller-file-transfer-configuration-edit
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L113
  title: FileTransferConfigurationEdit (api door)
- id: door-file-transfer-controller-add-file-base-configuration
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L62
  title: AddFileBaseConfiguration (api door)
- id: door-file-transfer-controller-add-file-transfer-base-configuration
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.AddFileTransferBaseConfiguration.cs#L21
  title: AddFileTransferBaseConfiguration (api door)
- id: door-file-transfer-controller-destination-configuration-edit
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L132
  title: DestinationConfigurationEdit (api door)
- id: door-file-transfer-controller-destination-configuration-edit-panel
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L103
  title: DestinationConfigurationEditPanel (api door)
- id: door-file-transfer-svc-service-file-transfer-add
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L25
  title: FileTransferAdd (wcf door)
- id: door-file-transfer-svc-service-file-transfer-add-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L26
  title: FileTransferAdd (wcf door)
- id: door-file-transfer-svc-service-file-transfer-add-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L25
  title: FileTransferAdd (wcf door)
- id: door-file-transfer-svc-service-controller-file-transfer-add
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L22
  title: FileTransferAdd (api door)
- id: door-file-transfer-svc-service-update-file-transfer-source-location
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L31
  title: UpdateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-update-file-transfer-source-location-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L32
  title: UpdateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-update-file-transfer-source-location-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L31
  title: UpdateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-controller-update-file-transfer-source-location
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L28
  title: UpdateFileTransferSourceLocation (api door)
- id: door-file-transfer-svc-service-invalidate-file-transfer-source-location
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L37
  title: InvalidateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-invalidate-file-transfer-source-location-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L38
  title: InvalidateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-invalidate-file-transfer-source-location-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L37
  title: InvalidateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-controller-invalidate-file-transfer-source-location
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L34
  title: InvalidateFileTransferSourceLocation (api door)
- id: door-file-transfer-svc-query-service-get-source-locations
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L40
  title: GetSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-get-source-locations-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L31
  title: GetSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-get-source-locations-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L31
  title: GetSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-controller-get-source-locations
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L39
  title: GetSourceLocations (api door)
- id: door-file-transfer-controller-get-source-locations
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SourceLocation.cs#L24
  title: GetSourceLocations (api door)
- id: door-file-transfer-svc-query-service-search-source-locations
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L25
  title: SearchSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-search-source-locations-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L16
  title: SearchSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-search-source-locations-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L16
  title: SearchSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-controller-search-source-locations
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L21
  title: SearchSourceLocations (api door)
- id: footprint
  resource: /technical/flows/configure-source-locations-and-destinations/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:configure-source-locations-and-destinations
  generation:
    status: done
    attempts: 1
  rule_prefix: F01
  rules: 108
  doors:
  - file-transfer-controller-file-transfer-configuration-view
  - file-transfer-controller-file-transfer-configuration-edit
  - file-transfer-controller-add-file-base-configuration
  - file-transfer-controller-add-file-transfer-base-configuration
  - file-transfer-controller-destination-configuration-edit
  - file-transfer-controller-destination-configuration-edit-panel
  - file-transfer-svc-service-file-transfer-add
  - file-transfer-svc-service-file-transfer-add-2
  - file-transfer-svc-service-file-transfer-add-3
  - file-transfer-svc-service-controller-file-transfer-add
  - file-transfer-svc-service-update-file-transfer-source-location
  - file-transfer-svc-service-update-file-transfer-source-location-2
  - file-transfer-svc-service-update-file-transfer-source-location-3
  - file-transfer-svc-service-controller-update-file-transfer-source-location
  - file-transfer-svc-service-invalidate-file-transfer-source-location
  - file-transfer-svc-service-invalidate-file-transfer-source-location-2
  - file-transfer-svc-service-invalidate-file-transfer-source-location-3
  - file-transfer-svc-service-controller-invalidate-file-transfer-source-location
  - file-transfer-svc-query-service-get-source-locations
  - file-transfer-svc-query-service-get-source-locations-2
  - file-transfer-svc-query-service-get-source-locations-3
  - file-transfer-svc-query-service-controller-get-source-locations
  - file-transfer-controller-get-source-locations
  - file-transfer-svc-query-service-search-source-locations
  - file-transfer-svc-query-service-search-source-locations-2
  - file-transfer-svc-query-service-search-source-locations-3
  - file-transfer-svc-query-service-controller-search-source-locations
  tables: 3
  procedures: 0
  components:
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  - backend-configure-source-locations-and-destinations-api
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - shared-contracts-commands
  - ui-configure-source-locations-and-destinations-screens
  - ui-manage-ftp-servers-and-credentials-screens
  body_sha256: 62d3668e0b452e09e6005df83cd94d9a50d80b65ad486354f976309f85f3c6c9
---

﻿# Find source locations (legacy rules)

## Door wiring

**P4-001** The WCF `SearchSourceLocations` doors route the incoming `SourceLocationQuery` through `TryRouteQuery` and return the routed query result.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:25-28, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:16-19, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:16-19
- Legacy: Each method returns `this.TryRouteQuery(query) as SourceLocationQuery`.
- Decision: keep

**P4-002** The API `SearchSourceLocations` door returns HTTP 200 with the result of `IFileTransferSvcQueryService.SearchSourceLocations`.
- Doors: file-transfer-svc-query-service-controller-search-source-locations
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:21-25
- Legacy: The action returns `Ok(fileTransferSvcQueryService.SearchSourceLocations(query))`.
- Decision: keep

**P4-003** The WCF `GetSourceLocations` doors route the incoming `GetSourceLocationQuery` through `TryRouteQuery` and return the routed query result.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:40-43, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:31-34, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:31-34
- Legacy: Each method returns `this.TryRouteQuery(query) as GetSourceLocationQuery`.
- Decision: keep

**P4-004** The API `GetSourceLocations` door returns HTTP 200 with the result of `IFileTransferSvcQueryService.GetSourceLocations`.
- Doors: file-transfer-svc-query-service-controller-get-source-locations
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:39-43
- Legacy: The action returns `Ok(fileTransferSvcQueryService.GetSourceLocations(query))`.
- Decision: keep

## UX API (HTTP)

**P4-005** The UX `GetSourceLocations` HTTP endpoint is a GET endpoint that requires an anti-forgery token.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:24-27
- Legacy: The method is decorated with `[HttpGet]` and `[ValidateAntiForgeryTokenAttribute]`.
- Decision: keep

**P4-006** The UX `GetSourceLocations` HTTP endpoint resolves the file transfer query-service data provider from tenant configuration using the component name `FileTransferQueryServiceDataProvider`.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:31-34
- Legacy: Sets `this.FileDataProvider = TenantConfigurationManager.Resolve<IFileTransferQueryServiceDataProvider>(...FileTransferQueryServiceDataProvider)`.
- Decision: keep

**P4-007** The UX `GetSourceLocations` HTTP endpoint calls `GetSourceLocations` with a request that contains the generated requestor and a null data payload.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:35-41
- Legacy: Creates `new RequestModel<string> { Requestor = GenerateRequestor(), Data = null }` and passes it to `FileDataProvider.GetSourceLocations(request)`.
- Decision: keep

**P4-008** When the provider response status is `OK`, the UX `GetSourceLocations` HTTP endpoint returns HTTP 200 with a list of list-items; otherwise it returns HTTP 400 with the provider `ResultModel`.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:43-68
- Legacy: On `StatusCodeType.OK`, returns `Request.CreateResponse(HttpStatusCode.OK, listItems)`; otherwise returns `Request.CreateResponse(HttpStatusCode.BadRequest, resultModel)`.
- Decision: keep

**P4-009** Each returned source location becomes a list-item whose display name is `<ConfigName> - <Address>` and whose id is the source location id string.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:47-55
- Legacy: Adds `new ListItemModel { DisplayFormat = ...Name, Name = sourceLocation.ConfigName + " - " + sourceLocation.Address, ID = sourceLocation.SourceLocationId.ToString() }`.
- Decision: keep

## UX provider (WCF client)

**P4-010** When the UX provider `GetSourceLocations` is called, it always queries the WCF service `SearchSourceLocations` with `IsActive = true`.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:360-376, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:390-393
- Legacy: Builds a `SourceLocationQuery` whose `Where` is `new SourceLocationQueryParms { IsActive = true }`, then calls `serviceProxy.SearchSourceLocations(serviceQuery)`.
- Decision: keep

**P4-011** When the WCF `SearchSourceLocations` call returns results, the UX provider maps each service `SourceLocationModel` to an internal `SourceLocationModel` with `SourceLocationId` set to the service `FtsSourceLocationId` GUID formatted as a string.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:395-401, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:557-569
- Legacy: Iterates `serviceResponse.Results` and maps via `MapToInternalSourceLocationModel`, which maps `SourceLocationId = src.FtsSourceLocationId.ToString()`.
- Decision: keep

**P4-012** The UX provider sorts the returned source locations by `ConfigName` ascending.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:403
- Legacy: Calls `response.Data.Sort((a1, a2) => a1.ConfigName.CompareTo(a2.ConfigName))`.
- Decision: keep

## Search source locations (query execution)

**P4-013** Searching source locations reads from `FtsSourceLocation` records and projects them into `SourceLocationModel` results.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:65-79
- Legacy: Selects fields from `FtsSourceLocation` into a `SourceLocationModel` with `ConfigId`, `ConfigName`, `OwnerName`, `OwnerSubsystem`, `Protocol`, `Address`, `Mask`, and `IsActive`.
- Decision: keep

**P4-014** When `ConfigId` is greater than 0, searching source locations returns only source locations whose `ConfigId` equals the requested `ConfigId`.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:43-45
- Legacy: Adds `p => p.ConfigId == query.Where.ConfigId` when `query.Where.ConfigId > 0`.
- Decision: keep

**P4-015** When `OwnerName` is provided, searching source locations filters to exact matches using case-insensitive comparison.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:45-47
- Legacy: Compares `p.OwnerName.ToUpper() == query.Where.OwnerName.ToUpper()`.
- Decision: keep

**P4-016** When `ConfigName` is provided, searching source locations filters to exact matches using case-insensitive comparison.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:47-49
- Legacy: Compares `p.ConfigName.ToUpper() == query.Where.ConfigName.ToUpper()`.
- Decision: keep

**P4-017** When `OwnerSubsystem` is provided, searching source locations parses it as a GUID and filters to source locations whose owner module GUID equals it.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:49-51, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\SourceLocationQueryParms.cs:59-65
- Legacy: Uses `p.OwnerSubsystem == query.Where.OwnerModuleGuid`, where `OwnerModuleGuid` is computed as `Guid.Parse(OwnerSubsystem)`.
- Decision: keep
- ⚑ Flag: `OwnerSubsystem` must be a GUID string; if it is not parseable as a GUID, `Guid.Parse` throws during query construction.

**P4-018** When `Protocol` is provided, searching source locations filters to exact matches of the protocol code.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:51-53
- Legacy: Adds `p => p.Protocol == query.Where.Protocol` when the requested protocol is not empty.
- Decision: keep

**P4-019** When `IsActive` is true, searching source locations filters to active source locations; when `IsActive` is false, no active/inactive filter is applied.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:53-55
- Legacy: Adds `p => p.IsActive == query.Where.IsActive` only when `query.Where.IsActive` is true.
- Decision: keep
- ⚑ Flag: There is no way to request “inactive only” results using `IsActive = false`, because the where-clause is not applied.

**P4-020** Search results can be sorted by configured field names, and default sorting is by `ConfigId` ascending and then `IsActive` descending.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:20-37
- Legacy: Supports sort field names `ConfigId`, `ConfigName`, `SourceAddress`, `SourceFileMask`, `OwnerName`, `ModuleName`, and `Status`; defaults to `ConfigId` asc then `IsActive` desc.
- Decision: keep

**P4-021** Search results are assigned by materializing the filtered and sorted query to a list.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:60-63
- Legacy: Sets `query.Results = baseQuery.ToList()`.
- Decision: keep

## Get source location details (query execution)

**P4-022** Getting source location details requires a non-empty `SourceLocationId`; when it is missing, the query throws a validation exception with message `FtpSoureceLocationId`.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\GetSourceLocationQuery.cs:30-36, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:39
- Legacy: `Validate()` throws `new FileTransferValidationException("FtpSoureceLocationId")` when `Where.SourceLocationId` is empty.
- Decision: keep

**P4-023** The requested `SourceLocationId` is parsed as a GUID and used to look up `FtsSourceLocation` by id; the lookup returns a source location only when exactly one row matches.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\GetSourceLocationQueryParms.cs:30-35, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:130-135
- Legacy: Parses the id via `Guid.Parse(SourceLocationId)` and queries `FtsSourceLocationId == query.Where.SoureceLocationGuid`, then returns `locations.First()` only when `locations.Count == 1`.
- Decision: keep
- ⚑ Flag: If `SourceLocationId` is not a GUID string, `Guid.Parse` throws.

**P4-024** Before building the response, the details query loads all destination locations and inbound targets into memory.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:47-49
- Legacy: Sets `this.dest = ...Set<FtsDestinationLocations>().ToList()` and `this.inboundTarget = ...Set<FtsInboundTarget>().ToList()`.
- Decision: keep

**P4-025** When a source location row is found, the details query returns a single `SourceLocationModel` populated from the source location record and includes `FtpServerName` when the source location has an FTP server id.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:52-88, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:137-142
- Legacy: Builds a `SourceLocationModel` from the `FtsSourceLocation` fields and sets `FtpServerName = ftpServerDetails != null ? ftpServerDetails.FtpServerName : null`.
- Decision: keep

**P4-026** The details query does not filter by requested `IsActive` and always returns the source location when the id matches, including the stored active flag.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\GetSourceLocationQueryParms.cs:24-28, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:31-34, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:52-88
- Legacy: `ApplyWhereClause` returns the base query unchanged; the `IsActive` request parameter is not referenced, and the response model sets `IsActive = sourceLocation.IsValid`.
- Decision: keep

**P4-027** When the details query assigns results, it also populates the first result’s `DestinationAddress` with all destinations linked to the source location.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:36-41
- Legacy: Gets destinations using `baseQuery.First().FtsSourceLocationId`, sets `query.Results = baseQuery.ToList()`, then sets `query.Results.First().DestinationAddress = destinationRecords`.
- Decision: keep
- ⚑ Flag: `baseQuery.First()` throws if no source location record was returned.

**P4-028** Destination records are ordered by creation date descending and mapped into destination DTOs, including an `ImportJobName` looked up by `FTEMetadataId`.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:96-125, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:144-153
- Legacy: Orders destinations by `CreationDate` descending, maps each destination into `FtsDestinationLocationAdded`, and sets `ImportJobName = inboundTarget.FirstOrDefault(...).ImportJobName`.
- Decision: keep
- ⚑ Flag: When there is no inbound-target row matching `FTEMetadataId`, `FirstOrDefault(...)` returns null and `...ImportJobName` throws.
