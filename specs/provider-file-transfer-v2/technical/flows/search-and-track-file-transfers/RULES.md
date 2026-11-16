---
type: Business Rules
title: Rules
description: 36 business rules (F05-NNN) of the flow, each with its legacy source lines.
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController
tags:
- api
- flow
- provider-file-transfer-v2
- wcf
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T09:12:32Z
sources:
- id: door-file-transfer-controller-search-file-transfer-tracking
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.Tracking.cs#L24
  title: SearchFileTransferTracking (api door)
- id: door-file-transfer-controller-search-file-transfer-tracking-2
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.Tracking.cs#L24
  title: SearchFileTransferTracking (api door)
- id: door-file-transfer-controller-search-file-transfer
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L28
  title: SearchFileTransfer (api door)
- id: door-file-transfer-svc-query-service-search-file-transfer-movement
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L30
  title: SearchFileTransferMovement (wcf door)
- id: door-file-transfer-svc-query-service-search-file-transfer-movement-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L21
  title: SearchFileTransferMovement (wcf door)
- id: door-file-transfer-svc-query-service-search-file-transfer-movement-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L21
  title: SearchFileTransferMovement (wcf door)
- id: door-file-transfer-svc-query-service-controller-search-file-transfer-movement
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L27
  title: SearchFileTransferMovement (api door)
- id: door-file-transfer-svc-query-service-search-inbound-target
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L45
  title: SearchInboundTarget (wcf door)
- id: door-file-transfer-svc-query-service-search-inbound-target-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L36
  title: SearchInboundTarget (wcf door)
- id: door-file-transfer-svc-query-service-search-inbound-target-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L36
  title: SearchInboundTarget (wcf door)
- id: door-file-transfer-svc-query-service-controller-search-inbound-target
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L45
  title: SearchInboundTarget (api door)
- id: door-file-transfer-controller-get-ftemetadatas
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L160
  title: GetFTEMetadatas (api door)
- id: door-file-transfer-controller-get-data-list-data
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L63
  title: GetDataListData (api door)
- id: door-file-transfer-controller-get-data-list-data-2
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L90
  title: GetDataListData (api door)
- id: footprint
  resource: /technical/flows/search-and-track-file-transfers/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:search-and-track-file-transfers
  generation:
    status: done
    attempts: 1
  rule_prefix: F05
  rules: 36
  doors:
  - file-transfer-controller-search-file-transfer-tracking
  - file-transfer-controller-search-file-transfer-tracking-2
  - file-transfer-controller-search-file-transfer
  - file-transfer-svc-query-service-search-file-transfer-movement
  - file-transfer-svc-query-service-search-file-transfer-movement-2
  - file-transfer-svc-query-service-search-file-transfer-movement-3
  - file-transfer-svc-query-service-controller-search-file-transfer-movement
  - file-transfer-svc-query-service-search-inbound-target
  - file-transfer-svc-query-service-search-inbound-target-2
  - file-transfer-svc-query-service-search-inbound-target-3
  - file-transfer-svc-query-service-controller-search-inbound-target
  - file-transfer-controller-get-ftemetadatas
  - file-transfer-controller-get-data-list-data
  - file-transfer-controller-get-data-list-data-2
  tables: 0
  procedures: 0
  components:
  - backend-configure-source-locations-and-destinations-api
  - ui-configure-source-locations-and-destinations-screens
  - ui-manage-ftp-servers-and-credentials-screens
  body_sha256: 9defbc4c86ba61a07dea0e0956d08623f374b9ea1108e93212fc869f6d2a90cf
---

# Rules

Written in parts (`PARTS.yaml`); each part's rules follow under its name.

## Search and track file transfers (UX)


### Tracking search page (presentation)

**F05-001** When the File Transfer tracking search page is requested, the editor is initialized as a single-step create flow for the FileTransfer entity and the cancel action navigates to `Home/MyHome`.
- Doors: file-transfer-controller-search-file-transfer-tracking
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:30-40
- Legacy: Calls `InitializeEditor(FileTransfer, 1, 1, Create, Next, BuildMvcUrl(Home, MyHome), false, true, true, true)` (dirty-data disabled; reset enabled; cancel enabled; summary errors shown).
- Decision: keep

**F05-002** The tracking search page uses the user profile locale code to localize the editor Search action label.
- Doors: file-transfer-controller-search-file-transfer-tracking
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:42-43
- Legacy: Sets `criteria.Editor.SaveActionText` from `LocalizationProvider.GetLabel(UserProfile.LocalCode, CoreConstants.Localization.Labels.Buttons.Search).Text`.
- Decision: keep

**F05-003** The tracking search page returns an MVC view with an empty tracking search criteria model.
- Doors: file-transfer-controller-search-file-transfer-tracking
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:28, ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:45-47
- Legacy: Creates `new TrackingSearchCriteriaModel()` and returns `View(criteria)`.
- Decision: keep
### Tracking search results (service)

**F05-004** The tracking search results endpoint requires an anti-forgery token and binds a Kendo `DataSourceRequest` from the query.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:24-27
- Legacy: Exposes `[HttpGet]` `SearchFileTransferTracking(DataSourceRequest request)` with `[ValidateAntiForgeryTokenAttribute]` and a `DataSourceRequestModelBinder`.
- Decision: keep

**F05-005** Tracking search requests are sent to the query provider using the current requestor and the initialized search criteria from the incoming `DataSourceRequest`.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:30-34, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:36-40
- Legacy: Builds `RequestModel<SearchCriteriaModel>` with `Requestor=GenerateRequestor()` and `Data=InitializeSearchRequest(request)`, resolves `IFileTransferQueryServiceDataProvider` by component name, then calls `SearchFileTransferTracking(searchRequest)`.
- Decision: keep

**F05-035** Tracking search criteria mapping sets only supported filter fields, and date/size filters are applied only when their values can be parsed.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.Tracking.cs:36, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.Tracking.cs:126-203
- Legacy: The provider maps `SearchFieldModel.FieldName` values `FileId`, `FileName`, `FileStatus`, `Direction`, `TransportType`, `TradingPartnerId`, and `SourceLocationId` to query parameters as strings; it applies `FromDate`/`ToDate` only when `DateTime.TryParse(...)` succeeds and `FromSize`/`ToSize` only when `decimal.TryParse(...)` succeeds (unsupported field names are ignored).
- Review: GAP-002
- Decision: keep

**F05-006** When the provider result status is OK, the endpoint returns HTTP 200 with a Kendo `DataSourceResult` whose `Total` defaults to 0 when paging results are missing.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:41-52, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:62
- Legacy: If `searchResponse.Result.StatusCode == OK`, returns `200 OK` with `Data=searchResponse.Data` and `Total = (PagingResults != null ? PagingResults.TotalRowsAvailable : 0)`.
- Decision: keep

**F05-007** When the provider result status is OK but the provider returns no data, the endpoint still returns HTTP 200 with an empty result set and total 0.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:45-60, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:62
- Legacy: Uses `new List<TrackingSearchResultModel>()` and `Total=0` when `searchResponse.Data == null`.
- Decision: keep

**F05-008** When the provider result status is not OK, the endpoint returns HTTP 400 with the provider `ResultModel`.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:41-43, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:64-67
- Legacy: Returns `BadRequest` with `resultModel` when `resultModel.StatusCode != OK`.
- Decision: keep
### File transfer search (service)

**F05-009** The file transfer search endpoint requires an anti-forgery token and binds a Kendo `DataSourceRequest` from the query.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:28-31
- Legacy: Exposes `[HttpGet]` `SearchFileTransfer(DataSourceRequest requestor)` with `[ValidateAntiForgeryTokenAttribute]` and a `DataSourceRequestModelBinder`.
- Decision: keep

**F05-010** File transfer search requests are sent to the query provider using the current requestor and the initialized search criteria from the incoming `DataSourceRequest`.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:34-43
- Legacy: Resolves `IFileTransferQueryServiceDataProvider`, builds `RequestModel<SearchCriteriaModel>` with `Requestor=GenerateRequestor()` and `Data=InitializeSearchRequest(requestor)`, then calls `SearchFileTransfer(request)`.
- Decision: keep

**F05-036** The file transfer configuration search provider builds a `SourceLocationQuery` only when the incoming search criteria includes paging criteria; otherwise it calls the query service with a null query.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:86-110
- Legacy: The provider initializes `serviceQuery = null`, sets it only when `searchCriteria != null && searchCriteria.PagingCriteria != null`, and still calls `SearchSourceLocations(serviceQuery)`.
- Review: GAP-003
- Decision: keep

**F05-011** The file transfer search endpoint returns HTTP 200 only when the provider status is OK and the provider returns a non-null data list; otherwise it returns HTTP 400.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:44-58
- Legacy: Returns `200 OK` with `DataSourceResult(Data=response1.Data, Total=response1.PagingResults.TotalRowsAvailable)` when `StatusCode == OK && response1.Data != null`, else `400 BadRequest` with `resultModel`.
- Decision: keep
- ⚑ Flag: If the provider returns `StatusCode == OK` but `Data == null`, the endpoint still responds with HTTP 400.

### Data list lookup (service)

**F05-012** The data list lookup endpoint requires an anti-forgery token and routes the request through an internal `GetDataListData` helper that returns list items via an out parameter.
- Doors: file-transfer-controller-get-data-list-data
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:63-73
- Legacy: Exposes `[HttpGet] GetDataListData(contentID, startsWith, localeId)`, then calls `GetDataListData(contentID, startsWith, localeId, out listItems)`.
- Decision: keep

**F05-013** When the internal processor result is valid, the data list lookup endpoint returns HTTP 200 with the list items as the response body.
- Doors: file-transfer-controller-get-data-list-data
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:74-81
- Legacy: If `processorResult.IsValid`, returns `200 OK` with `listItems`.
- Decision: keep
- ⚑ Flag: The method creates a `DataSourceResult` but returns the raw `List<ListItemModel>` instead of the `DataSourceResult`.

**F05-014** When the internal processor result is not valid, the data list lookup endpoint returns HTTP 400 with the processor result payload.
- Doors: file-transfer-controller-get-data-list-data
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:74-85
- Legacy: If `!processorResult.IsValid`, returns `400 BadRequest` with `processorResult`.
- Decision: keep
### Data list selection (internal)

**F05-015** The internal data list selector treats a missing `dataListType` as an empty string and only populates results for the `MODULE` type.
- Doors: file-transfer-controller-get-data-list-data-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:90-106
- Legacy: Normalizes `dataListType` to `string.Empty` when null/empty; `switch (dataListType.ToUpper())` only handles `case "MODULE": GetModules(out listItems)`.
- Decision: keep
- ⚑ Flag: `contentID` and `localeId` are accepted but not used in the internal selector.

**F05-016** The module data list is fetched as active modules ordered by module name ascending, and each item uses `ModuleName` as `Name` and `ModuleID` as `ID`.
- Doors: file-transfer-controller-get-data-list-data-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:187-205
- Legacy: Queries reference data with `Module?$filter=IsActive%20eq%20true&$orderby=ModuleName%20asc` and maps each row to `ListItemModel(DisplayFormat=Name, Name=<ModuleName>, ID=<ModuleID>)`.
- Decision: keep

### FTE metadata lookup (service)

**F05-017** The FTE metadata lookup endpoint requires an anti-forgery token and calls the query provider for metadata IDs using the current requestor and the supplied contract name.
- Doors: file-transfer-controller-get-ftemetadatas
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:160-175
- Legacy: Resolves `IFileTransferQueryServiceDataProvider`, builds `RequestModel<SearchCriteriaModel>(Requestor=GenerateRequestor(), Data=null)`, and calls `GetMetadataIds(request, contractName)`.
- Decision: keep

**F05-018** The FTE metadata lookup endpoint returns HTTP 200 whenever the provider response includes a non-null data list (including an empty list); it returns HTTP 404 with an empty list only when the provider response or its data list is null.
- Doors: file-transfer-controller-get-ftemetadatas
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:176-179
- Legacy: Returns `200 OK` with `metadatas.Data` when `metadatas != null && metadatas.Data != null` (without checking `metadatas.Result.StatusCode`), else `404 NotFound` with `new List<InboundTargetModel>()`.
- Review: GAP-001
- Decision: keep

## Query service searches (movement and inbound target)


### Door routing

**F05-019** The WCF `SearchFileTransferMovement` doors route the incoming `FileTransferTrackingQuery` through the CQRS router (`TryRoute`) and return the routed query object.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:30-33, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55-58, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:21-24, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41-44, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:21-24, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46-49
- Legacy: Each door calls `TryRouteQuery(query)` and casts the result back to `FileTransferTrackingQuery`.
- Decision: keep

**F05-020** The API `SearchFileTransferMovement` door returns HTTP 200 with the result of `IFileTransferSvcQueryService.SearchFileTransferMovement`.
- Doors: file-transfer-svc-query-service-controller-search-file-transfer-movement
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:27-31
- Legacy: The controller calls the query service and wraps the returned query object in `Ok(...)`.
- Decision: keep

**F05-021** The WCF `SearchInboundTarget` doors route the incoming `InboundTargetQuery` through the CQRS router (`TryRoute`) and return the routed query object.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:45-48, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55-58, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:36-39, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41-44, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:36-39, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46-49
- Legacy: Each door calls `TryRouteQuery(query)` and casts the result back to `InboundTargetQuery`.
- Decision: keep

**F05-022** The API `SearchInboundTarget` door returns HTTP 200 with the result of `IFileTransferSvcQueryService.SearchInboundTarget`.
- Doors: file-transfer-svc-query-service-controller-search-inbound-target
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:45-49
- Legacy: The controller calls the query service and wraps the returned query object in `Ok(...)`.
- Decision: keep

### File transfer movement search

**F05-023** When a `FileTransferTrackingQuery.Where` field is provided, file transfer movement results are filtered by exact match (string fields) or by inclusive range (date/size fields).
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:50-82
- Legacy: The query adds `WHERE` predicates only when a corresponding `Where` value is present (`FileId`, `FileName`, `FileStatus`, `FileDirection`, `FileTransportType`, `FileSourceLocationId`, `TradingPartner`, `StartDate`, `EndDate`, `FileSizeFrom`, `FileSizeThrough`).
- ⚑ Flag: `Where.FileId` and `Where.FileSourceLocationId` are parsed with `Guid.TryParse`; if parsing fails but the input string is non-empty, the filter compares to `Guid.Empty`.
- Decision: keep

**F05-024** File transfer movement sorting supports specific sort keys and defaults to ascending `FileName`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:31-47
- Legacy: Sort keys map as `FileId`, `FileName`, `FileStatus`, `FileDirection`, `FileSize`, `CreatedDate`, and `TradingPartnerId` (sorting by `TradingPartner`); when no sort is requested the default is ascending `FileName`.
- Decision: keep

**F05-025** File transfer movement results are built from valid source locations and split into two protocol cases: UNC and non-UNC with an FTP server.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:111-170, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\DataAccessConstants.cs:16-22
- Legacy: The base result set is `UNC (SourceProtocalCode == UNCProtocolCD (3) and IsValid == true) UNION FTP (FtpServerId != null and IsValid == true)` joined to file management; UNC results set `OriginatingServerName` to `string.Empty` and FTP results set it to the joined `FtpServerName`.
- Decision: keep

**F05-026** Each file transfer movement result is enriched with its file events and file exceptions.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:84-108
- Legacy: After the result list is materialized, the executor runs a `FileEventQuery` and a `FileExceptionQuery` per result (using `FileId.ToString()` as input) and assigns `file.Events` and `file.Exceptions`.
- Decision: keep

**F05-027** File event lookups for movement results filter by `FileId` (when provided) and default-sort by `EventId` then `EventCode`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileEventQueryExecutor.cs:23-34, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileEventQueryExecutor.cs:36-46
- Legacy: The executor parses `Where.FileId` with `Guid.TryParse`, filters `FileId` when `Where.FileId != null`, and applies the default order `(EventId ASC, EventCode ASC)`.
- Decision: keep

**F05-028** File event lookups project event fields from `FtsFileEvent` and use the related file management `CreatedDate` as the returned `CreatedDate`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileEventQueryExecutor.cs:53-65
- Legacy: Event results include `EventId`, `FileId`, `EventCD` (as `EventCode`), `EventTrackDescription`, and `CreatedDate = FtsFileManagement.CreatedDate`.
- Decision: keep

**F05-029** File exception lookups for movement results filter by `FileId` (when provided) and default-sort by `ExceptionId` then `ExceptionType`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileExceptionQueryExecutor.cs:23-34, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileExceptionQueryExecutor.cs:36-46
- Legacy: The executor parses `Where.FileId` with `Guid.TryParse`, filters `FileId` when `Where.FileId != null`, and applies the default order `(ExceptionId ASC, ExceptionType ASC)`.
- Decision: keep

**F05-030** File exception lookups project exception fields from `FtsFileException` and use the related file management `CreatedDate` as the returned `CreatedDate`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileExceptionQueryExecutor.cs:53-68
- Legacy: Exception results include `ExceptionId`, `EventId`, `FileId`, `ExceptionType`, `Message`, `StackTrace`, `MethodName`, and `CreatedDate = FtsFileManagement.CreatedDate`.
- Decision: keep

### Inbound target search

**F05-031** `InboundTargetQuery` initializes its `Where` object and defaults `FTEMetadataId` to `Guid.Empty`.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\InboundTargetQuery.cs:27-30, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\InboundTargetQueryParms.cs:18-21
- Legacy: A new `InboundTargetQuery` always has a non-null `Where`, and `Where.FTEMetadataId` starts as `Guid.Empty`.
- Decision: keep

**F05-032** Inbound target results include only valid records and support optional filtering by `FTEMetadataId` and `ContractName`.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\InboundTargetQueryExecutor.cs:31-44
- Legacy: The query applies `inboundTarget.IsValid` and conditionally filters by `FTEMetadataId` (when not `Guid.Empty`) and `ContractName` (when not null/empty), returning `FTEMetadataId`, `FtsInboundTargetId`, `ImportJobName`, and `ContractName`.
- Decision: keep

**F05-033** Inbound target results are de-duplicated to a single record per `FTEMetadataId`, selecting the smallest `FtsInboundTargetId` within each group.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\InboundTargetQueryExecutor.cs:23-28
- Legacy: After building the base query, results are grouped by `FTEMetadataId` and reduced to `OrderBy(FtsInboundTargetId).FirstOrDefault()` per group.
- Decision: keep

**F05-034** Inbound target query validation throws only if validation messages are present, but no messages are added by `Validate()`.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\InboundTargetQuery.cs:45-62
- Legacy: `Validate()` initializes `messages` to an empty list and throws a `BusinessException` only when `messages.Count > 0`.
- Decision: keep
