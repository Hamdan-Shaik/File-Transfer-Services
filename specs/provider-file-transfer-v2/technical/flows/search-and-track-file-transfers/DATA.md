---
type: Data Access
title: 'Data: Search and track file transfers'
description: Tables, stored procedures and data the flow reads and writes.
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
  body_sha256: 553b5e1ba738aa63d25afa53d6c55e80545221fe160dc1e3e4d0cdefcd88c762
---

# Data: Search and track file transfers

## Tables

The graph used for the brief did not report any tables for this flow.

Tables referenced by the movement and inbound target query executors include:

- `FtsFileManagement` (read)
- `FtsSourceLocation` (read)
- `FtpServer` (read)
- `FtsFileEvent` (read)
- `FtsFileException` (read)
- `FtsInboundTarget` (read)

Reference data used for the module pick list is retrieved via `IReferenceDataProvider.GetReferenceData(...)` (not as a direct table access in this flow). (F05-016)

No writes are performed by this flow.

## Stored procedures

None.

## Door inputs and answer shapes

### `file-transfer-controller-search-file-transfer-tracking`

- Answer: an MVC view model with an empty `TrackingSearchCriteriaModel` and an initialized editor. (F05-001, F05-003)

### `file-transfer-controller-search-file-transfer-tracking-2`

- Input: a Kendo `DataSourceRequest` containing requested filters, ordering, and paging for the tracking search. (F05-004, F05-005)
- The provider maps only supported search fields and applies date/size filters only when their values can be parsed. (F05-035)
- Answer (success): a Kendo `DataSourceResult` with:
  - `Data`: `List<TrackingSearchResultModel>`
  - `Total`: `PagingResults.TotalRowsAvailable` when paging results are present, otherwise `0` (F05-006)
  - When the provider returns no data list, `Data` is an empty list and `Total` is `0` (F05-007)
- Answer (error): a `ResultModel` from the provider. (F05-008)

### `file-transfer-controller-search-file-transfer`

- Input: a Kendo `DataSourceRequest` containing requested filters, ordering, and paging for file transfer configuration search. (F05-009, F05-010)
- The provider builds the underlying `SourceLocationQuery` only when the request includes paging criteria; otherwise it calls the query service with a null query. (F05-036)
- Answer (success): a Kendo `DataSourceResult` with:
  - `Data`: `List<AddFileTransferConfigurationModel>`
  - `Total`: `PagingResults.TotalRowsAvailable` (F05-011)
- Answer (error): a `ResultModel` from the provider, including the case where the provider returns OK but no data list. (F05-011)

### `file-transfer-controller-get-data-list-data`

- Input: `contentID`, `startsWith`, `localeId`.
- Answer (success): `List<ListItemModel>` containing the requested options. (F05-013)
- Answer (error): a `ResultModel` from the internal processor. (F05-014)

### `file-transfer-controller-get-data-list-data-2`

- Input: `contentID`, `dataListType`, `localeId`.
- Answer:
  - When `dataListType` is missing, it is treated as an empty string. (F05-015)
  - Only the `MODULE` type is handled; other values return an empty list. (F05-015)
  - For `MODULE`, the list is active modules ordered by module name ascending, with each item mapping `ModuleName` to `Name` and `ModuleID` to `ID`. (F05-016)

### `file-transfer-controller-get-ftemetadatas`

- Input: `contractName`.
- Answer (success): `List<InboundTargetModel>` from the provider whenever the provider response includes a non-null data list (including an empty list). (F05-018)
- Answer (not found): an empty list only when the provider response or its data list is null (the endpoint does not check the provider status code). (F05-018)

### `file-transfer-svc-query-service-search-file-transfer-movement` / `-2` / `-3` / `file-transfer-svc-query-service-controller-search-file-transfer-movement`

- Input: `FileTransferTrackingQuery`.
  - `Where` supports optional filters for `FileId`, `FileName`, `FileStatus`, `FileDirection`, `FileTransportType`, `FileSourceLocationId`, `TradingPartner`, `StartDate`, `EndDate`, `FileSizeFrom`, `FileSizeThrough`. (F05-023)
  - Sorting keys supported: `FileId`, `FileName`, `FileStatus`, `FileDirection`, `FileSize`, `CreatedDate`, `TradingPartnerId` (sorting by `TradingPartner`), defaulting to ascending `FileName`. (F05-024)
- Answer: the returned query object has a `Results` list of `FileTransferTrackingModel` populated by the executor. (F05-019, F05-020)
  - Results include only valid source locations, split into shared-folder (UNC) and FTP-linked cases; shared-folder results use `OriginatingServerName = string.Empty` and FTP results use the joined FTP server name. (F05-025)
  - Each result includes `Events` and `Exceptions` populated by per-result queries. (F05-026)

### `file-transfer-svc-query-service-search-inbound-target` / `-2` / `-3` / `file-transfer-svc-query-service-controller-search-inbound-target`

- Input: `InboundTargetQuery`.
  - `Where` is always initialized, and `Where.FTEMetadataId` defaults to `Guid.Empty`. (F05-031)
  - Optional filtering by `FTEMetadataId` (when not `Guid.Empty`) and `ContractName` (when not null/empty). (F05-032)
- Answer: the returned query object has a `Results` list populated by the executor. (F05-021, F05-022)
  - Only valid records are included.
  - Results are de-duplicated to one row per `FTEMetadataId` by selecting the smallest `FtsInboundTargetId` per group. (F05-033)
