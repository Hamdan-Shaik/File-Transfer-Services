---
type: Business Rules
title: Search and track file transfers — UX search and tracking (rules)
description: 18 business rules (P1-NNN) of the flow, each with its legacy source lines.
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
  body_sha256: 78e4a0749ec45ebc847f67aedf8524067193cb6791d50dfee7a931885d76a134
---

# Search and track file transfers — UX search and tracking (rules)

## Tracking search page (presentation)

**P1-001** When the File Transfer tracking search page is requested, the editor is initialized as a single-step create flow for the FileTransfer entity and the cancel action navigates to `Home/MyHome`.
- Doors: file-transfer-controller-search-file-transfer-tracking
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:30-40
- Legacy: Calls `InitializeEditor(FileTransfer, 1, 1, Create, Next, BuildMvcUrl(Home, MyHome), false, true, true, true)` (dirty-data disabled; reset enabled; cancel enabled; summary errors shown).
- Decision: keep

**P1-002** The tracking search page uses the user profile locale code to localize the editor “Search” action label.
- Doors: file-transfer-controller-search-file-transfer-tracking
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:42-43
- Legacy: Sets `criteria.Editor.SaveActionText` from `LocalizationProvider.GetLabel(UserProfile.LocalCode, CoreConstants.Localization.Labels.Buttons.Search).Text`.
- Decision: keep

**P1-003** The tracking search page returns an MVC view with an empty tracking search criteria model.
- Doors: file-transfer-controller-search-file-transfer-tracking
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:28, ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:45-47
- Legacy: Creates `new TrackingSearchCriteriaModel()` and returns `View(criteria)`.
- Decision: keep
## Tracking search results (service)

**P1-004** The tracking search results endpoint requires an anti-forgery token and binds a Kendo `DataSourceRequest` from the query.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:24-27
- Legacy: Exposes `[HttpGet]` `SearchFileTransferTracking(DataSourceRequest request)` with `[ValidateAntiForgeryTokenAttribute]` and a `DataSourceRequestModelBinder`.
- Decision: keep

**P1-005** Tracking search requests are sent to the query provider using the current requestor and the initialized search criteria from the incoming `DataSourceRequest`.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:30-34, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:36-40
- Legacy: Builds `RequestModel<SearchCriteriaModel>` with `Requestor=GenerateRequestor()` and `Data=InitializeSearchRequest(request)`, resolves `IFileTransferQueryServiceDataProvider` by component name, then calls `SearchFileTransferTracking(searchRequest)`.
- Decision: keep

**P1-006** When the provider result status is OK, the endpoint returns HTTP 200 with a Kendo `DataSourceResult` whose `Total` defaults to 0 when paging results are missing.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:41-52, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:62
- Legacy: If `searchResponse.Result.StatusCode == OK`, returns `200 OK` with `Data=searchResponse.Data` and `Total = (PagingResults != null ? PagingResults.TotalRowsAvailable : 0)`.
- Decision: keep

**P1-007** When the provider result status is OK but the provider returns no data, the endpoint still returns HTTP 200 with an empty result set and total 0.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:45-60, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:62
- Legacy: Uses `new List<TrackingSearchResultModel>()` and `Total=0` when `searchResponse.Data == null`.
- Decision: keep

**P1-008** When the provider result status is not OK, the endpoint returns HTTP 400 with the provider `ResultModel`.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:41-43, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:64-67
- Legacy: Returns `BadRequest` with `resultModel` when `resultModel.StatusCode != OK`.
- Decision: keep
## File transfer search (service)

**P1-009** The file transfer search endpoint requires an anti-forgery token and binds a Kendo `DataSourceRequest` from the query.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:28-31
- Legacy: Exposes `[HttpGet]` `SearchFileTransfer(DataSourceRequest requestor)` with `[ValidateAntiForgeryTokenAttribute]` and a `DataSourceRequestModelBinder`.
- Decision: keep

**P1-010** File transfer search requests are sent to the query provider using the current requestor and the initialized search criteria from the incoming `DataSourceRequest`.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:34-43
- Legacy: Resolves `IFileTransferQueryServiceDataProvider`, builds `RequestModel<SearchCriteriaModel>` with `Requestor=GenerateRequestor()` and `Data=InitializeSearchRequest(requestor)`, then calls `SearchFileTransfer(request)`.
- Decision: keep

**P1-011** The file transfer search endpoint returns HTTP 200 only when the provider status is OK and the provider returns a non-null data list; otherwise it returns HTTP 400.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:44-58
- Legacy: Returns `200 OK` with `DataSourceResult(Data=response1.Data, Total=response1.PagingResults.TotalRowsAvailable)` when `StatusCode == OK && response1.Data != null`, else `400 BadRequest` with `resultModel`.
- Decision: keep
- ⚑ Flag: If the provider returns `StatusCode == OK` but `Data == null`, the endpoint still responds with HTTP 400.

## Data list lookup (service)

**P1-012** The data list lookup endpoint requires an anti-forgery token and routes the request through an internal `GetDataListData` helper that returns list items via an out parameter.
- Doors: file-transfer-controller-get-data-list-data
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:63-73
- Legacy: Exposes `[HttpGet] GetDataListData(contentID, startsWith, localeId)`, then calls `GetDataListData(contentID, startsWith, localeId, out listItems)`.
- Decision: keep

**P1-013** When the internal processor result is valid, the data list lookup endpoint returns HTTP 200 with the list items as the response body.
- Doors: file-transfer-controller-get-data-list-data
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:74-81
- Legacy: If `processorResult.IsValid`, returns `200 OK` with `listItems`.
- Decision: keep
- ⚑ Flag: The method creates a `DataSourceResult` but returns the raw `List<ListItemModel>` instead of the `DataSourceResult`.

**P1-014** When the internal processor result is not valid, the data list lookup endpoint returns HTTP 400 with the processor result payload.
- Doors: file-transfer-controller-get-data-list-data
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:74-85
- Legacy: If `!processorResult.IsValid`, returns `400 BadRequest` with `processorResult`.
- Decision: keep
## Data list selection (internal)

**P1-015** The internal data list selector treats a missing `dataListType` as an empty string and only populates results for the `MODULE` type.
- Doors: file-transfer-controller-get-data-list-data-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:90-106
- Legacy: Normalizes `dataListType` to `string.Empty` when null/empty; `switch (dataListType.ToUpper())` only handles `case "MODULE": GetModules(out listItems)`.
- Decision: keep
- ⚑ Flag: `contentID` and `localeId` are accepted but not used in the internal selector.

**P1-016** The module data list is fetched as active modules ordered by module name ascending, and each item uses `ModuleName` as `Name` and `ModuleID` as `ID`.
- Doors: file-transfer-controller-get-data-list-data-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:187-205
- Legacy: Queries reference data with `Module?$filter=IsActive%20eq%20true&$orderby=ModuleName%20asc` and maps each row to `ListItemModel(DisplayFormat=Name, Name=<ModuleName>, ID=<ModuleID>)`.
- Decision: keep

## FTE metadata lookup (service)

**P1-017** The FTE metadata lookup endpoint requires an anti-forgery token and calls the query provider for metadata IDs using the current requestor and the supplied contract name.
- Doors: file-transfer-controller-get-ftemetadatas
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:160-175
- Legacy: Resolves `IFileTransferQueryServiceDataProvider`, builds `RequestModel<SearchCriteriaModel>(Requestor=GenerateRequestor(), Data=null)`, and calls `GetMetadataIds(request, contractName)`.
- Decision: keep

**P1-018** The FTE metadata lookup endpoint returns HTTP 200 with the provider data when present; otherwise it returns HTTP 404 with an empty list.
- Doors: file-transfer-controller-get-ftemetadatas
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:176-179
- Legacy: Returns `200 OK` with `metadatas.Data` when `metadatas != null && metadatas.Data != null`, else `404 NotFound` with `new List<InboundTargetModel>()`.
- Decision: keep