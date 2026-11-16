---
type: Business Rules
title: Query service searches (movement and inbound target)
description: 16 business rules (P2-NNN) of the flow, each with its legacy source lines.
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
  body_sha256: 520d969742a8a17331b401fe3f3fbe0affd69f4c764117e70856b25082f50fdb
---

# Query service searches (movement and inbound target)

## Door routing

**P2-001** The WCF `SearchFileTransferMovement` doors route the incoming `FileTransferTrackingQuery` through the CQRS router (`TryRoute`) and return the routed query object.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:30-33, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55-58, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:21-24, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41-44, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:21-24, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46-49
- Legacy: Each door calls `TryRouteQuery(query)` and casts the result back to `FileTransferTrackingQuery`.
- Decision: keep

**P2-002** The API `SearchFileTransferMovement` door returns HTTP 200 with the result of `IFileTransferSvcQueryService.SearchFileTransferMovement`.
- Doors: file-transfer-svc-query-service-controller-search-file-transfer-movement
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:27-31
- Legacy: The controller calls the query service and wraps the returned query object in `Ok(...)`.
- Decision: keep

**P2-003** The WCF `SearchInboundTarget` doors route the incoming `InboundTargetQuery` through the CQRS router (`TryRoute`) and return the routed query object.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:45-48, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55-58, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:36-39, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41-44, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:36-39, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46-49
- Legacy: Each door calls `TryRouteQuery(query)` and casts the result back to `InboundTargetQuery`.
- Decision: keep

**P2-004** The API `SearchInboundTarget` door returns HTTP 200 with the result of `IFileTransferSvcQueryService.SearchInboundTarget`.
- Doors: file-transfer-svc-query-service-controller-search-inbound-target
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:45-49
- Legacy: The controller calls the query service and wraps the returned query object in `Ok(...)`.
- Decision: keep

## File transfer movement search

**P2-005** When a `FileTransferTrackingQuery.Where` field is provided, file transfer movement results are filtered by exact match (string fields) or by inclusive range (date/size fields).
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:50-82
- Legacy: The query adds `WHERE` predicates only when a corresponding `Where` value is present (`FileId`, `FileName`, `FileStatus`, `FileDirection`, `FileTransportType`, `FileSourceLocationId`, `TradingPartner`, `StartDate`, `EndDate`, `FileSizeFrom`, `FileSizeThrough`).
- âš‘ Flag: `Where.FileId` and `Where.FileSourceLocationId` are parsed with `Guid.TryParse`; if parsing fails but the input string is non-empty, the filter compares to `Guid.Empty`.
- Decision: keep

**P2-006** File transfer movement sorting supports specific sort keys and defaults to ascending `FileName`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:31-47
- Legacy: Sort keys map as `FileId`, `FileName`, `FileStatus`, `FileDirection`, `FileSize`, `CreatedDate`, and `TradingPartnerId` (sorting by `TradingPartner`); when no sort is requested the default is ascending `FileName`.
- Decision: keep

**P2-007** File transfer movement results are built from valid source locations and split into two protocol cases: UNC and non-UNC with an FTP server.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:111-170, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\DataAccessConstants.cs:16-22
- Legacy: The base result set is `UNC (SourceProtocalCode == UNCProtocolCD (3) and IsValid == true) UNION FTP (FtpServerId != null and IsValid == true)` joined to file management; UNC results set `OriginatingServerName` to `string.Empty` and FTP results set it to the joined `FtpServerName`.
- Decision: keep

**P2-008** Each file transfer movement result is enriched with its file events and file exceptions.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:84-108
- Legacy: After the result list is materialized, the executor runs a `FileEventQuery` and a `FileExceptionQuery` per result (using `FileId.ToString()` as input) and assigns `file.Events` and `file.Exceptions`.
- Decision: keep

**P2-009** File event lookups for movement results filter by `FileId` (when provided) and default-sort by `EventId` then `EventCode`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileEventQueryExecutor.cs:23-34, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileEventQueryExecutor.cs:36-46
- Legacy: The executor parses `Where.FileId` with `Guid.TryParse`, filters `FileId` when `Where.FileId != null`, and applies the default order `(EventId ASC, EventCode ASC)`.
- Decision: keep

**P2-010** File event lookups project event fields from `FtsFileEvent` and use the related file management `CreatedDate` as the returned `CreatedDate`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileEventQueryExecutor.cs:53-65
- Legacy: Event results include `EventId`, `FileId`, `EventCD` (as `EventCode`), `EventTrackDescription`, and `CreatedDate = FtsFileManagement.CreatedDate`.
- Decision: keep

**P2-011** File exception lookups for movement results filter by `FileId` (when provided) and default-sort by `ExceptionId` then `ExceptionType`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileExceptionQueryExecutor.cs:23-34, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileExceptionQueryExecutor.cs:36-46
- Legacy: The executor parses `Where.FileId` with `Guid.TryParse`, filters `FileId` when `Where.FileId != null`, and applies the default order `(ExceptionId ASC, ExceptionType ASC)`.
- Decision: keep

**P2-012** File exception lookups project exception fields from `FtsFileException` and use the related file management `CreatedDate` as the returned `CreatedDate`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileExceptionQueryExecutor.cs:53-68
- Legacy: Exception results include `ExceptionId`, `EventId`, `FileId`, `ExceptionType`, `Message`, `StackTrace`, `MethodName`, and `CreatedDate = FtsFileManagement.CreatedDate`.
- Decision: keep

## Inbound target search

**P2-013** `InboundTargetQuery` initializes its `Where` object and defaults `FTEMetadataId` to `Guid.Empty`.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\InboundTargetQuery.cs:27-30, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\InboundTargetQueryParms.cs:18-21
- Legacy: A new `InboundTargetQuery` always has a non-null `Where`, and `Where.FTEMetadataId` starts as `Guid.Empty`.
- Decision: keep

**P2-014** Inbound target results include only valid records and support optional filtering by `FTEMetadataId` and `ContractName`.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\InboundTargetQueryExecutor.cs:31-44
- Legacy: The query applies `inboundTarget.IsValid` and conditionally filters by `FTEMetadataId` (when not `Guid.Empty`) and `ContractName` (when not null/empty), returning `FTEMetadataId`, `FtsInboundTargetId`, `ImportJobName`, and `ContractName`.
- Decision: keep

**P2-015** Inbound target results are de-duplicated to a single record per `FTEMetadataId`, selecting the smallest `FtsInboundTargetId` within each group.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\InboundTargetQueryExecutor.cs:23-28
- Legacy: After building the base query, results are grouped by `FTEMetadataId` and reduced to `OrderBy(FtsInboundTargetId).FirstOrDefault()` per group.
- Decision: keep

**P2-016** Inbound target query validation throws only if validation messages are present, but no messages are added by `Validate()`.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\InboundTargetQuery.cs:45-62
- Legacy: `Validate()` initializes `messages` to an empty list and throws a `BusinessException` only when `messages.Count > 0`.
- Decision: keep
