---
type: Behavior Specification
title: 'backend-data-access-query-handlers: legacy query executors for configuration + tracking read models'
description: 'What the backend-data-access-query-handlers component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-query-handlers
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T01:03:24Z
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
  resource: /technical/components/backend-data-access-query-handlers/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-query-handlers
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 8
  methods: 43
  owned_rules: 37
  flows: []
  depends_on:
  - shared-contracts-queries
  deployable: fxfer-domain-api
  folder: src/Application/Queries
  body_sha256: 3bb7c385571c37cbdffac09b1ab0450bb86467828d4b3e5e3579344e96063bd0
---

# backend-data-access-query-handlers: legacy query executors for configuration + tracking read models

This component executes read-only queries against the FXFER database tables used for configuration and file-transfer tracking.
It is called by legacy BAS query-service doors and relies on the CQRS `QueryExecutorBase<TModel, TQuery>` helpers for dynamic where/sort/paging.

## FileEventQueryExecutor
Executes `FileEventQuery` and projects `FtsFileEvent` rows into `FileEventModel` results.

### ApplyPaging(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
- Purpose: Apply legacy paging to the file-event query.
- Inputs: `baseQuery` (queryable results), `query` (`FileEventQuery`).
- Logic:
  - Delegates to `QueryExecutorBase.ApplyPaging(baseQuery, query)`.
- Data: none.
- Errors: Any exception thrown by the base paging helper propagates.
- Rules: none.

### ApplySorting(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
- Purpose: Apply sorting to file-event results.
- Inputs: `baseQuery`, `query.SortCriteria`.
- Logic:
  - Builds a dynamic sort definition that supports sort key `EVENT_ID`.
  - Default sort (when caller does not specify sort criteria):
    - `EventId` ascending, then `EventCode` ascending.
  - Applies ordering via `baseQuery.OrderBy(dynamicSort, query.SortCriteria)`.
- Data: none.
- Errors: Any exception thrown by `OrderBy(...)` / sort builder propagates.
- Rules: F05-027.

### ApplyWhereClause(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
- Purpose: Filter file-event results by `FileId` when provided.
- Inputs: `query.Where.FileId` (string).
- Logic:
  - Parses `query.Where.FileId` with `Guid.TryParse(...)` into `fileId` (defaults to `Guid.Empty` on parse failure).
  - Builds a dynamic where-clause:
    - When `q.Where.FileId != null`, add predicate `p.FileId == fileId`.
  - Applies filtering via `baseQuery.Where(whereClause, query)`.
- Data: none.
- Errors:
  - If `query.Where` is null, accessing `query.Where.FileId` throws `NullReferenceException`.
- Rules: F05-027.

### AssignResults(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
- Purpose: Materialize the queryable results into `query.Results`.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Sets `query.Results = baseQuery.ToList()`.
- Data: reads `orm:FTS_FILE_EVENT`, `orm:FTS_FILE_MANAGEMENT` (via the composed query).
- Errors: Any exception from query execution/materialization propagates.
- Rules: none.

### GetBaseQuery(IDbContextBase databaseContext, FileEventQuery query)
- Purpose: Build the base query for file-event lookups.
- Inputs: `databaseContext` (EF context), `query`.
- Logic:
  - Joins `FtsFileManagement` to `FtsFileEvent` on `FileId`.
  - Projects each joined row into `new FileEventModel { ... }`:
    - `EventId = fe.EventId`
    - `FileId = fe.FileId`
    - `EventCode = fe.EventCD`
    - `CreatedDate = fm.CreatedDate`
    - `EventTrackDescription = fe.EventTrackDescription`
- Data: reads `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_FILE_EVENT`.
- Errors: Any exception thrown by the underlying LINQ provider/EF query translation propagates.
- Rules: F05-028.

## FileExceptionQueryExecutor
Executes `FileExceptionQuery` and projects `FtsFileException` rows into `FileExceptionModel` results.

### ApplyPaging(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
- Purpose: Apply legacy paging to the file-exception query.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Delegates to `QueryExecutorBase.ApplyPaging(baseQuery, query)`.
- Data: none.
- Errors: Any exception thrown by the base paging helper propagates.
- Rules: none.

### ApplySorting(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
- Purpose: Apply sorting to file-exception results.
- Inputs: `baseQuery`, `query.SortCriteria`.
- Logic:
  - Builds a dynamic sort definition that supports sort key `EXCEPTION_ID`.
  - Default sort (when caller does not specify sort criteria):
    - `ExceptionId` ascending, then `ExceptionType` ascending.
  - Applies ordering via `baseQuery.OrderBy(dynamicSort, query.SortCriteria)`.
- Data: none.
- Errors: Any exception thrown by `OrderBy(...)` / sort builder propagates.
- Rules: F05-029.

### ApplyWhereClause(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
- Purpose: Filter file-exception results by `FileId` when provided.
- Inputs: `query.Where.FileId` (string).
- Logic:
  - Parses `query.Where.FileId` with `Guid.TryParse(...)` into `fileId` (defaults to `Guid.Empty` on parse failure).
  - Builds a dynamic where-clause:
    - When `q.Where.FileId != null`, add predicate `p.FileId == fileId`.
  - Applies filtering via `baseQuery.Where(whereClause, query)`.
- Data: none.
- Errors:
  - If `query.Where` is null, accessing `query.Where.FileId` throws `NullReferenceException`.
- Rules: F05-029.

### AssignResults(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
- Purpose: Materialize the queryable results into `query.Results`.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Sets `query.Results = baseQuery.ToList()`.
- Data: reads `orm:FTS_FILE_EXCEPTION`, `orm:FTS_FILE_MANAGEMENT` (via the composed query).
- Errors: Any exception from query execution/materialization propagates.
- Rules: none.

### GetBaseQuery(IDbContextBase databaseContext, FileExceptionQuery query)
- Purpose: Build the base query for file-exception lookups.
- Inputs: `databaseContext`, `query`.
- Logic:
  - Joins `FtsFileManagement` to `FtsFileException` on `FileId`.
  - Projects each joined row into `new FileExceptionModel { ... }`:
    - `EventId = fe.EventId`
    - `FileId = fe.FileId`
    - `ExceptionType = fe.ExceptionType`
    - `Message = fe.Message`
    - `StackTrace = fe.StackTrace`
    - `MethodName = fe.MethodName`
    - `CreatedDate = fm.CreatedDate`
    - `ExceptionId = fe.ExceptionId`
- Data: reads `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_FILE_EXCEPTION`.
- Errors: Any exception thrown by the underlying LINQ provider/EF query translation propagates.
- Rules: F05-030.

## FileTransferTrackingQueryExecutor
Executes `FileTransferTrackingQuery` to search file movement/tracking headers and enriches each result with file events and file exceptions.

### ApplyPaging(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
- Purpose: Apply legacy paging to the tracking query.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Delegates to `QueryExecutorBase.ApplyPaging(baseQuery, query)`.
- Data: none.
- Errors: Any exception thrown by the base paging helper propagates.
- Rules: none.

### ApplySorting(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
- Purpose: Apply sorting to tracking results.
- Inputs: `baseQuery`, `query.SortCriteria`.
- Logic:
  - Supports sort keys:
    - `FileId`, `FileName`, `FileStatus`, `FileDirection`, `FileSize`, `CreatedDate`, `TradingPartnerId` (sorts by `TradingPartner`).
  - Default sort (when caller does not specify sort criteria): `FileName` ascending.
  - Applies ordering via `baseQuery.OrderBy(dynamicSort, query.SortCriteria)`.
- Data: none.
- Errors: Any exception thrown by `OrderBy(...)` / sort builder propagates.
- Rules: F05-024.

### ApplyWhereClause(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
- Purpose: Apply exact-match and inclusive-range filters to tracking results.
- Inputs: `query.Where` fields.
- Logic:
  - Parses these ids using `Guid.TryParse`:
    - `query.Where.FileId` → `fileId` (defaults to `Guid.Empty` on parse failure)
    - `query.Where.FileSourceLocationId` → `sourceLocationId` (defaults to `Guid.Empty` on parse failure)
  - Builds a dynamic where-clause. A predicate is added only when its input is present:
    - `FileId`: when `Where.FileId` is non-empty, `p.FileId == fileId`
    - `FileName`: when non-empty, `p.FileName == Where.FileName`
    - `FileStatus`: when non-empty, `p.FileStatus == Where.FileStatus`
    - `FileDirection`: when non-empty, `p.FileDirection == Where.FileDirection`
    - `FileTransportType`: when non-empty, `p.FileTransportType == Where.FileTransportType`
    - `FileSourceLocationId`: when non-empty, `p.FileSourceLocationId == sourceLocationId`
    - `TradingPartner`: when non-empty, `p.TradingPartner == Where.TradingPartner`
    - `StartDate`: when non-null, `p.CreatedDate >= Where.StartDate`
    - `EndDate`: when non-null, `p.CreatedDate <= Where.EndDate`
    - `FileSizeFrom`: when `> 0M`, `p.FileSize >= Where.FileSizeFrom`
    - `FileSizeThrough`: when `> 0M`, `p.FileSize <= Where.FileSizeThrough`
  - Applies filtering via `baseQuery.Where(whereClause, query)`.
- Data: none.
- Errors:
  - If `query.Where` is null, accessing `query.Where.*` throws `NullReferenceException`.
- Rules: F05-023.

### AssignResults(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
- Purpose: Materialize results and enrich each result with file events and exceptions.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Sets `query.Results = baseQuery.ToList()`.
  - For each `file` in `query.Results`:
    - Creates `fileEventQuery` with `Where.FileId = file.FileId.ToString()`.
    - Creates `fileExceptionQuery` with `Where.FileId = file.FileId.ToString()`.
    - Executes `new FileEventQueryExecutor().Execute(this.databaseContext, fileEventQuery)` and assigns `file.Events`.
    - Executes `new FileExceptionQueryExecutor().Execute(this.databaseContext, fileExceptionQuery)` and assigns `file.Exceptions`.
- Data: reads `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_SOURCE_LOCATION`, `orm:FTP_SERVER`, `orm:FTS_FILE_EVENT`, `orm:FTS_FILE_EXCEPTION`.
- Errors:
  - Any exception from query execution/materialization or from the per-row enrichment queries propagates.
- Rules: F05-026.

### GetBaseQuery(IDbContextBase databaseContext, FileTransferTrackingQuery query)
- Purpose: Build the base tracking query by combining UNC and FTP protocol cases.
- Inputs: `databaseContext`.
- Logic:
  - Stores `this.databaseContext = databaseContext` for later use by `AssignResults`.
  - Builds the base result set as:
    - `GetFileTransferTrackingUNC(databaseContext)` UNION `GetFileTransferTrackingFTPProtocls(databaseContext)`.
  - Returns `result.AsQueryable()`.
- Data: reads `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_SOURCE_LOCATION`, `orm:FTP_SERVER`.
- Errors: Any exception from query execution/materialization in the helper methods propagates.
- Rules: F05-025.

### GetFileTransferTrackingUNC(IDbContextBase databaseContext)
- Purpose: Build the tracking results for UNC-protocol source locations.
- Inputs: `databaseContext`.
- Logic:
  - Joins `FtsFileManagement` to `FtsSourceLocation` where:
    - `SourceProtocalCode == DataAccessConstants.FileStatusConstant.UNCProtocolCD`
    - `IsValid == true`
  - Projects `FileTransferTrackingModel` fields from file management and source location.
  - Sets `OriginatingServerName = string.Empty`.
  - Materializes to `List<FileTransferTrackingModel>` via `ToList()`.
- Data: reads `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_SOURCE_LOCATION`.
- Errors: Any exception from query execution/materialization propagates.
- Rules: F05-025.

### GetFileTransferTrackingFTPProtocls(IDbContextBase databaseContext)
- Purpose: Build the tracking results for non-UNC source locations that have an FTP server id.
- Inputs: `databaseContext`.
- Logic:
  - Joins `FtsFileManagement` to `FtsSourceLocation` where:
    - `FtpServerId != null`
    - `IsValid == true`
  - Joins the source location to `FtpServer` on `FtpServerId`.
  - Projects `FileTransferTrackingModel` fields.
  - Sets `OriginatingServerName = fs.FtpServerName`.
  - Materializes to `List<FileTransferTrackingModel>` via `ToList()`.
- Data: reads `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_SOURCE_LOCATION`, `orm:FTP_SERVER`.
- Errors: Any exception from query execution/materialization propagates.
- Rules: F05-025.

## GetFtpServerQueryExecutor
Executes `FtpServerQuery` against `FTP_SERVER` rows.

### ApplyPaging(IQueryable<FtpServerModel> baseQuery, FtpServerQuery query)
- Purpose: Apply legacy paging to the FTP server query.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Delegates to `QueryExecutorBase.ApplyPaging(baseQuery, query)`.
- Data: none.
- Errors: Any exception thrown by the base paging helper propagates.
- Rules: none.

### ApplySorting(IQueryable<FtpServerModel> baseQuery, FtpServerQuery query)
- Purpose: Apply sorting to FTP server results.
- Inputs: `query.SortCriteria`.
- Logic:
  - When `query.SortCriteria` is null or empty:
    - Sets `query.SortCriteria = SetDefaultSortCriteria()`.
    - Default criteria: sort by `CommonConstants.SortFields.FtpServer.FtpServerName` ascending.
  - Applies ordering via `baseQuery.OrderBy(BuildDynamicSort(), query.SortCriteria)`.
- Data: none.
- Errors: Any exception from sort helper execution propagates.
- Rules: F02-006.

### AssignResults(IQueryable<FtpServerModel> baseQuery, FtpServerQuery query)
- Purpose: Materialize the queryable results into `query.Results`.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Sets `query.Results = baseQuery.ToList()`.
- Data: reads `orm:FTP_SERVER` (via the composed query).
- Errors: Any exception from query execution/materialization propagates.
- Rules: none.

### GetBaseQuery(IDbContextBase context, FtpServerQuery query)
- Purpose: Build the base query for FTP server lookups.
- Inputs: `context`, `query.Where.IsValid`.
- Logic:
  - Builds the base query from `context.Set<FtpServer>()`.
  - Applies validity filter:
    - include all when `query.Where.IsValid == null`
    - else filter `identifier.IsValid == query.Where.IsValid`.
  - Projects each row into a `FtpServerModel` including credential fields:
    - `FtpPassword`, `FtpUser`, `FtpServerIp`, `FtpServerName` and proxy/port fields.
- Data: reads `orm:FTP_SERVER`.
- Errors:
  - If `query.Where` is null, accessing `query.Where.IsValid` throws `NullReferenceException`.
- Rules: F02-005, F02-009.

### ApplyWhereClause(IQueryable<FtpServerModel> baseQuery, FtpServerQuery query)
- Purpose: Apply optional id/name/description/user predicates.
- Inputs: `query.Where` including `SearchOptions`.
- Logic:
  - Filters via `baseQuery.Where(GetDynamicPredicate(query), query)`.
- Data: none.
- Errors:
  - If `query.Where` is null, accessing `query.Where.*` throws `NullReferenceException`.
- Rules: F02-007, F02-008.

### GetDynamicPredicate(FtpServerQuery query)
- Purpose: Build the dynamic predicate for FTP server filtering.
- Inputs: `query.Where` (`FtpServerQueryParms`).
- Logic:
  - Reads search-mode strings via `SearchModeHelper.GetSearchMode(fieldName, query.Where.SearchOptions)` for:
    - `FtpServerId`, `FtpServerName`, `FtpDescription`, `FtpUser`.
  - `FtpServerId` filter:
    - If `query.Where.FtpServerId != Guid.Empty` AND the id search mode is `EqualTo`, adds `p.FtpServerId == new Guid(ftpServerIdValue)`.
  - Text filters (name/description/user):
    - When the corresponding input is non-empty, uppercases and trims the input value.
    - Applies one of these comparisons to the stored field (also uppercased + trimmed) depending on the configured search mode:
      - `Contains`, `StartsWith`, `EndsWith`, `EqualTo`.
  - Calls `whereClause.Build()` and returns the built builder.
- Data: none.
- Errors:
  - If `FtpServerId` is non-empty and the predicate is added, `new Guid(ftpServerIdValue)` can throw when the id value is not a valid GUID string.
- Rules: F02-007, F02-008.

### BuildDynamicSort()
- Purpose: Build the dynamic sorter for FTP server results.
- Inputs: none.
- Logic:
  - Supports sort fields:
    - `FtpServerName` (uppercased)
    - `FtpDescription` (uppercased)
    - `FtpServerIp`
    - `FtpUser` (uppercased)
    - `CreationDate`
    - `IsValid`
- Data: none.
- Errors: Any exception from sort builder creation propagates.
- Rules: none.

### SetDefaultSortCriteria()
- Purpose: Provide default sort criteria when none is supplied.
- Inputs: none.
- Logic:
  - Returns a one-item `List<SortCriteria>` with:
    - `FieldName = CommonConstants.SortFields.FtpServer.FtpServerName`
    - `Order = SortOrder.Asc`
- Data: none.
- Errors: none.
- Rules: F02-006.

## GetSourceLocationQueryExecutor
Executes `GetSourceLocationQuery` to return one source-location configuration including linked destination records.

### ApplySorting(IQueryable<SourceLocationModel> baseQuery, GetSourceLocationQuery query)
- Purpose: Apply sorting to details lookup results.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Returns `baseQuery` unchanged.
- Data: none.
- Errors: none.
- Rules: none.

### ApplyWhereClause(IQueryable<SourceLocationModel> baseQuery, GetSourceLocationQuery query)
- Purpose: Apply filtering to details lookup results.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Returns `baseQuery` unchanged.
- Data: none.
- Errors: none.
- Rules: none.

### AssignResults(IQueryable<SourceLocationModel> baseQuery, GetSourceLocationQuery query)
- Purpose: Assign the details result list and populate the first result’s destination list.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Calls `baseQuery.First()` and reads its `FtsSourceLocationId`.
  - Calls `GetDestinationAddressObject(sourceLocationId)` to build destination DTOs.
  - Sets `query.Results = baseQuery.ToList()`.
  - Sets `query.Results.First().DestinationAddress = destinationRecords`.
- Data: reads `orm:FTS_SOURCE_LOCATION`, `orm:FTS_DESTINATION`, `orm:FTS_INBOUND_TARGET`.
- Errors:
  - If `baseQuery` is empty, `baseQuery.First()` throws `InvalidOperationException`.
  - If `query.Results` is empty, `query.Results.First()` throws `InvalidOperationException`.
- Rules: F01-105.

### GetBaseQuery(IDbContextBase databaseContext, GetSourceLocationQuery query)
- Purpose: Build the details lookup result for one source location id.
- Inputs: `databaseContext`, `query`.
- Logic:
  - Calls `query.Validate()`.
  - Loads supporting tables into memory:
    - `this.dest = databaseContext.Set<FtsDestinationLocations>().ToList()`.
    - `this.inboundTarget = databaseContext.Set<FtsInboundTarget>().ToList()`.
  - Calls `GetSourceLocation(databaseContext, query)`.
  - When the returned `sourceLocation` is non-null:
    - Calls `GetFTPServer(databaseContext, sourceLocation)`.
    - Adds one `SourceLocationModel` populated from the `FtsSourceLocation` row, including:
      - `FtpServerName = ftpServerDetails != null ? ftpServerDetails.FtpServerName : null`
      - `Suspended = sourceLocation.IsSuspended`
      - `IsActive = sourceLocation.IsValid`
  - Returns the in-memory list as `IQueryable`.
- Data: reads `orm:FTS_SOURCE_LOCATION`, `orm:FTS_DESTINATION`, `orm:FTS_INBOUND_TARGET`, `orm:FTP_SERVER`.
- Errors:
  - `query.Validate()` can throw validation exceptions for invalid/missing id input.
  - Accessing `query.Where.SoureceLocationGuid` inside `GetSourceLocation(...)` can throw exceptions from `Guid.Parse`.
- Rules: F01-015, F01-102, F01-103.

### GetDestinationAddressObject(Guid soureceLocationId)
- Purpose: Build destination DTOs linked to the source location.
- Inputs: `soureceLocationId`.
- Logic:
  - Filters in-memory destination list: `dest.Where(d => d.FtsSourceLocationId == soureceLocationId)`.
  - Orders destinations by `CreationDate` descending.
  - For each `FtsDestinationLocations destination`, creates `new FtsDestinationLocationAdded { ... }` including:
    - Destination address/protocol fields.
    - Validity flag `IsValid`.
    - `FTEMetadataId`.
    - `ImportJobName = GetImportJobName(destination.FTEMetadataId)`.
- Data: reads `orm:FTS_DESTINATION`, `orm:FTS_INBOUND_TARGET` (in-memory list).
- Errors: Any exception thrown during projection/population propagates.
- Rules: F01-016, F01-106.

### GetSourceLocation(IDbContextBase context, GetSourceLocationQuery query)
- Purpose: Look up the source-location entity row by id.
- Inputs: `query.Where.SoureceLocationGuid`.
- Logic:
  - Loads `locations = context.Set<FtsSourceLocation>().Where(s => s.FtsSourceLocationId == query.Where.SoureceLocationGuid).ToList()`.
  - Returns `locations.First()` only when `locations != null && locations.Count == 1`.
  - Otherwise returns `null`.
- Data: reads `orm:FTS_SOURCE_LOCATION`.
- Errors:
  - Accessing `query.Where.SoureceLocationGuid` can throw exceptions from `Guid.Parse` when the source id is null or not a GUID string.
- Rules: F01-014.

### GetFTPServer(IDbContextBase context, FtsSourceLocation sourceLocation)
- Purpose: Look up FTP server details for a source location.
- Inputs: `sourceLocation.FtpServerId`.
- Logic:
  - When `sourceLocation.FtpServerId != null`, queries `context.Set<FtpServer>()` for that id and returns the first match.
  - When `sourceLocation.FtpServerId == null`, returns `null`.
- Data: reads `orm:FTP_SERVER`.
- Errors: Any exception thrown by query execution propagates.
- Rules: F01-103.

### GetImportJobName(Guid? fteMetadataId)
- Purpose: Resolve destination `ImportJobName` from the inbound-target list.
- Inputs: `fteMetadataId`.
- Logic:
  - Initializes `importName = string.Empty`.
  - When `fteMetadataId != null`:
    - Looks up `this.inboundTarget.FirstOrDefault(f => f.FTEMetadataId == fteMetadataId)`.
    - Reads `.ImportJobName` from the returned object and assigns it to `importName`.
  - Returns `importName`.
- Data: reads `orm:FTS_INBOUND_TARGET` (in-memory list).
- Errors:
  - When `fteMetadataId != null` and no inbound-target row matches, `.FirstOrDefault(...)` returns null and `.ImportJobName` throws `NullReferenceException`.
- Rules: F01-020, F01-106.

## InboundTargetQueryExecutor
Executes `InboundTargetQuery` to list inbound targets filtered by `FTEMetadataId` and/or `ContractName`.

### ApplySorting(IQueryable<FtsInboundTargetModel> baseQuery, InboundTargetQuery query)
- Purpose: Apply sorting to inbound-target results.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Returns `baseQuery` unchanged.
- Data: none.
- Errors: none.
- Rules: none.

### AssignResults(IQueryable<FtsInboundTargetModel> baseQuery, InboundTargetQuery query)
- Purpose: Materialize and de-duplicate inbound-target results by metadata id.
- Inputs: `baseQuery`.
- Logic:
  - Materializes results by:
    - Grouping by `FTEMetadataId`.
    - Selecting `OrderBy(FtsInboundTargetId).FirstOrDefault()` per group.
    - Converting to list.
- Data: reads `orm:FTS_INBOUND_TARGET` (via the composed query).
- Errors: Any exception from query execution/materialization propagates.
- Rules: F05-033.

### GetBaseQuery(IDbContextBase context, InboundTargetQuery query)
- Purpose: Build the base query for inbound-target search.
- Inputs: `query.Where.FTEMetadataId`, `query.Where.ContractName`.
- Logic:
  - Calls `query.Validate()`.
  - Filters `context.Set<FtsInboundTarget>()`:
    - When `query.Where.FTEMetadataId != Guid.Empty`, requires `inboundTarget.FTEMetadataId == query.Where.FTEMetadataId`.
    - When `query.Where.ContractName` is non-empty, requires `inboundTarget.ContractName == query.Where.ContractName`.
    - Always requires `inboundTarget.IsValid` (valid only).
  - Projects `FtsInboundTargetModel` with fields: `FTEMetadataId`, `FtsInboundTargetId`, `ImportJobName`, `ContractName`.
- Data: reads `orm:FTS_INBOUND_TARGET`.
- Errors:
  - Any exception from `query.Validate()` propagates.
  - If `query.Where` is null, accessing `query.Where.*` throws `NullReferenceException`.
- Rules: F05-032.

## IntegrationFileQueryExecutor
Executes `GetIntegrationFileQuery` to look up integration file definitions.

### ApplySorting(IQueryable<FtsIntegrationFileModel> baseQuery, GetIntegrationFileQuery query)
- Purpose: Apply sorting to integration file results.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Returns `baseQuery` unchanged (no explicit ordering).
- Data: none.
- Errors: none.
- Rules: F03-008.

### AssignResults(IQueryable<FtsIntegrationFileModel> baseQuery, GetIntegrationFileQuery query)
- Purpose: Materialize the queryable results into `query.Results`.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Sets `query.Results = baseQuery.ToList()`.
- Data: reads `orm:FTS_INTEGRATION_FILE` (via the composed query).
- Errors: Any exception from query execution/materialization propagates.
- Rules: none.

### GetBaseQuery(IDbContextBase context, GetIntegrationFileQuery query)
- Purpose: Build the base query for integration-file lookup.
- Inputs: `query.Where`.
- Logic:
  - Calls `query.Validate()`.
  - Filters `context.Set<FtsIntegrationFile>()`:
    - When `query.Where.FTSIntegrationFileId != Guid.Empty`, requires `integrationFile.FtsIntegrationFileId == query.Where.FTSIntegrationFileId`.
    - When `query.Where.FileName` is non-empty, requires `integrationFile.FileName.Contains(query.Where.FileName)`.
    - Always requires `integrationFile.IsValid` (active only).
  - Projects each match into `FtsIntegrationFileModel` assigning only:
    - `GroupId`, `ETFOutboundMetadataDefnId`, `ConfigId`, `FileName`, `IsProcessed`, `FtsIntegrationFileId`.
  - Does not assign other fields on `FtsIntegrationFileModel` (they remain default values).
- Data: reads `orm:FTS_INTEGRATION_FILE`.
- Errors:
  - Any exception from `query.Validate()` propagates.
  - If `query.Where` is null, accessing `query.Where.*` throws `NullReferenceException`.
- Rules: F03-005, F03-006, F03-007, F03-009.

## SearchSourceLocationQueryExecutor
Executes `SourceLocationQuery` to search configuration/source-location records.

### ApplySorting(IQueryable<SourceLocationModel> baseQuery, SourceLocationQuery query)
- Purpose: Apply sorting to source-location search results.
- Inputs: `query.SortCriteria`.
- Logic:
  - Supports sort keys:
    - `ConfigId`, `ConfigName`, `SourceAddress`, `SourceFileMask`, `OwnerName`, `ModuleName`, `Status`.
  - Default sort: `ConfigId` ascending, then `IsActive` descending.
  - Applies ordering via `baseQuery.OrderBy(dynamicSort, query.SortCriteria)`.
- Data: none.
- Errors: Any exception thrown by `OrderBy(...)` / sort builder propagates.
- Rules: F01-098.

### ApplyWhereClause(IQueryable<SourceLocationModel> basequery, SourceLocationQuery query)
- Purpose: Apply optional search filters.
- Inputs: `query.Where` fields.
- Logic:
  - Builds a dynamic where-clause:
    - When `ConfigId > 0`: `p.ConfigId == query.Where.ConfigId`
    - When `OwnerName` non-empty: `p.OwnerName.ToUpper() == query.Where.OwnerName.ToUpper()`
    - When `ConfigName` non-empty: `p.ConfigName.ToUpper() == query.Where.ConfigName.ToUpper()`
    - When `OwnerSubsystem` non-empty: `p.OwnerSubsystem == query.Where.OwnerModuleGuid`
    - When `Protocol` non-empty: `p.Protocol == query.Where.Protocol`
    - When `IsActive` is `true`: `p.IsActive == true` (no filter when `IsActive` is `false`)
  - Applies filtering via `basequery.Where(whereClause, query)`.
- Data: none.
- Errors:
  - Accessing `query.Where.OwnerModuleGuid` can throw exceptions from `Guid.Parse` when `OwnerSubsystem` is null or not a GUID string.
- Rules: F01-092, F01-093, F01-094, F01-095, F01-096, F01-097.

### AssignResults(IQueryable<SourceLocationModel> baseQuery, SourceLocationQuery query)
- Purpose: Materialize the queryable results into `query.Results`.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Sets `query.Results = baseQuery.ToList()`.
- Data: reads `orm:FTS_SOURCE_LOCATION`.
- Errors: Any exception from query execution/materialization propagates.
- Rules: F01-099.

### GetBaseQuery(IDbContextBase context, SourceLocationQuery query)
- Purpose: Build the base query for source-location search.
- Inputs: `context`.
- Logic:
  - Projects from `context.Set<FtsSourceLocation>()` into `SourceLocationModel` selecting:
    - `FtsSourceLocationId`, `ConfigId`, `ConfigName`, `OwnerName`, `OwnerSubsystem`, `Protocol`, `Address`, `Mask`, `IsActive`.
- Data: reads `orm:FTS_SOURCE_LOCATION`.
- Errors: Any exception thrown by the underlying LINQ provider/EF query translation propagates.
- Rules: F01-091.

## Rules implemented
| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-014 | Source location details lookup returns a source location only when exactly one row matches the parsed source location GUID. | `GetSourceLocationQueryExecutor.GetSourceLocation` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-015 | Details result maps `IsActive` from `FTS_SOURCE_LOCATION.IS_VALID` and `Suspended` from `FTS_SOURCE_LOCATION.IS_SUSPENDED`. | `GetSourceLocationQueryExecutor.GetBaseQuery` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-016 | Destination list is ordered by destination `CreationDate` descending. | `GetSourceLocationQueryExecutor.GetDestinationAddressObject` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-020 | Destination `ImportJobName` is resolved from the first inbound target row matching `FTEMetadataId`. | `GetSourceLocationQueryExecutor.GetImportJobName` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-091 | Source location search reads from `FtsSourceLocation` and projects to `SourceLocationModel`. | `SearchSourceLocationQueryExecutor.GetBaseQuery` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-092 | When `ConfigId > 0`, source location search filters by exact `ConfigId`. | `SearchSourceLocationQueryExecutor.ApplyWhereClause` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-093 | When `OwnerName` is provided, source location search filters by case-insensitive exact match. | `SearchSourceLocationQueryExecutor.ApplyWhereClause` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-094 | When `ConfigName` is provided, source location search filters by case-insensitive exact match. | `SearchSourceLocationQueryExecutor.ApplyWhereClause` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-095 | When `OwnerSubsystem` is provided, source location search parses it as a GUID and filters by `OwnerModuleGuid`. | `SearchSourceLocationQueryExecutor.ApplyWhereClause` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-096 | When `Protocol` is provided, source location search filters by exact protocol code. | `SearchSourceLocationQueryExecutor.ApplyWhereClause` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-097 | When `IsActive` is true, source location search filters by `IsActive=true`; when false, no active/inactive filter is applied. | `SearchSourceLocationQueryExecutor.ApplyWhereClause` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-098 | Source location search supports configured sort keys and defaults to `ConfigId ASC` then `IsActive DESC`. | `SearchSourceLocationQueryExecutor.ApplySorting` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-099 | Source location search assigns results by materializing the query to a list. | `SearchSourceLocationQueryExecutor.AssignResults` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-102 | Details lookup eagerly loads all destination locations and inbound targets into memory before building the response. | `GetSourceLocationQueryExecutor.GetBaseQuery` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-103 | When a source location is found, details lookup returns one `SourceLocationModel` and includes `FtpServerName` when a server id is present. | `GetSourceLocationQueryExecutor.GetBaseQuery`; `GetSourceLocationQueryExecutor.GetFTPServer` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-105 | Details lookup assigns results then populates the first result’s `DestinationAddress` list. | `GetSourceLocationQueryExecutor.AssignResults` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-106 | Destination records are ordered and mapped to DTOs including `ImportJobName` derived by `FTEMetadataId`. | `GetSourceLocationQueryExecutor.GetDestinationAddressObject`; `GetSourceLocationQueryExecutor.GetImportJobName` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F02-005 | FTP server search reads from `FTP_SERVER` and filters by validity when `Where.IsValid` is provided. | `GetFtpServerQueryExecutor.GetBaseQuery` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-006 | FTP server listing sorts differently by door: the legacy door defaults to `FtpServerName ASC`, while newer doors apply no explicit ordering. | `GetFtpServerQueryExecutor.ApplySorting` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-007 | FTP server listing filters by `FtpServerId` only for the legacy door (exact-match when search mode is `EqualTo`); newer doors do not filter by id. | `GetFtpServerQueryExecutor.GetDynamicPredicate` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-008 | FTP server listing text filtering differs by door: legacy door supports search-mode matching for name/description/user; newer doors filter by case-insensitive exact `FtpServerName` only. | `GetFtpServerQueryExecutor.GetDynamicPredicate` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-009 | FTP server results project a fixed set of fields including credential-related fields. | `GetFtpServerQueryExecutor.GetBaseQuery` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F03-005 | Integration file lookup returns only active (`IsValid=true`) definitions. | `IntegrationFileQueryExecutor.GetBaseQuery` | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |
| F03-006 | Integration file lookup filters by exact id only when id is provided (non-empty); otherwise no id filter is applied. | `IntegrationFileQueryExecutor.GetBaseQuery` | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |
| F03-007 | Integration file lookup filters by `FileName.Contains(fragment)` only when fragment is provided; otherwise no name filter is applied. | `IntegrationFileQueryExecutor.GetBaseQuery` | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |
| F03-008 | Integration file lookup applies no explicit ordering to results. | `IntegrationFileQueryExecutor.ApplySorting` | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |
| F03-009 | Integration file lookup populates only a fixed subset of model fields; other fields remain default values. | `IntegrationFileQueryExecutor.GetBaseQuery` | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |
| F05-023 | Tracking search filters by exact match (string fields) and inclusive ranges (date/size fields) when filters are provided. | `FileTransferTrackingQueryExecutor.ApplyWhereClause` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-024 | Tracking search supports configured sort keys and defaults to `FileName ASC`. | `FileTransferTrackingQueryExecutor.ApplySorting` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-025 | Tracking base results include only valid source locations and are the union of UNC vs FTP protocol cases, mapping `OriginatingServerName` accordingly. | `FileTransferTrackingQueryExecutor.GetBaseQuery`; `FileTransferTrackingQueryExecutor.GetFileTransferTrackingUNC`; `FileTransferTrackingQueryExecutor.GetFileTransferTrackingFTPProtocls` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-026 | Each tracking result is enriched with its file events and file exceptions. | `FileTransferTrackingQueryExecutor.AssignResults` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-027 | File event lookups filter by `FileId` when provided and default-sort by `EventId` then `EventCode`. | `FileEventQueryExecutor.ApplySorting`; `FileEventQueryExecutor.ApplyWhereClause` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-028 | File event lookups project event fields and set `CreatedDate` from the related file management `CreatedDate`. | `FileEventQueryExecutor.GetBaseQuery` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-029 | File exception lookups filter by `FileId` when provided and default-sort by `ExceptionId` then `ExceptionType`. | `FileExceptionQueryExecutor.ApplySorting`; `FileExceptionQueryExecutor.ApplyWhereClause` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-030 | File exception lookups project exception fields and set `CreatedDate` from the related file management `CreatedDate`. | `FileExceptionQueryExecutor.GetBaseQuery` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-032 | Inbound target search includes only valid records and supports optional filtering by `FTEMetadataId` and `ContractName`. | `InboundTargetQueryExecutor.GetBaseQuery` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-033 | Inbound target results are de-duplicated to one record per `FTEMetadataId`, selecting the smallest `FtsInboundTargetId` per group. | `InboundTargetQueryExecutor.AssignResults` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |

## Not carried over
- None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (query-side logic is read-only and optimized for reads)
- `patterns/application/api-microservices/PTN-APP-API-016-pagination-pattern.md` section `Solution Overview` (collection reads are paged and bounded)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (queries execute within tenant-scoped DB context)

