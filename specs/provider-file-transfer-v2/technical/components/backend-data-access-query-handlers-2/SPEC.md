---
type: Behavior Specification
title: technical/components/backend-data-access-query-handlers-2/SPEC
description: 'What the backend-data-access-query-handlers-2 component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-query-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T01:10:43Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-query-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-query-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 8
  methods: 38
  owned_rules: 0
  flows: []
  depends_on:
  - shared-contracts-queries
  - shared-queries-parameters-2
  deployable: fxfer-domain-api
  folder: src/Application/Queries
  body_sha256: 2fc8a87f788ebf03862a12532abef6ec1bc95fbd4887a3df1ca3ddd9ec9bccbb
---

﻿# backend-data-access-query-handlers-2: legacy query executors for configuration + tracking read models

This component executes read-only queries against FXFER database tables for configuration and file-transfer tracking.
It is called by legacy BAS query-service doors and relies on the CQRS `QueryExecutorBase<TModel, TQuery>` helpers for dynamic where/sort/paging composition.

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
- Rules: none.

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
- Rules: none.

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
- Rules: none.

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
- Rules: none.

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
- Rules: none.

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
- Rules: none.

## FileTransferTrackingQueryExecutor
Executes `FileTransferTrackingQuery` and returns `FileTransferTrackingModel` rows aggregated from UNC and FTP source locations.

### ApplyPaging(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
- Purpose: Apply legacy paging to the file-transfer-tracking query.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Delegates to `QueryExecutorBase.ApplyPaging(baseQuery, query)`.
- Data: none.
- Errors: Any exception thrown by the base paging helper propagates.
- Rules: none.

### ApplySorting(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
- Purpose: Apply sorting to file-transfer-tracking results.
- Inputs: `baseQuery`, `query.SortCriteria`.
- Logic:
  - Builds a dynamic sort definition supporting sort keys:
    - `FileId`, `FileName`, `FileStatus`, `FileDirection`, `FileSize`, `CreatedDate`, `TradingPartnerId`.
  - Default sort: `FileName` ascending.
  - Applies ordering via `baseQuery.OrderBy(dynamicSort, query.SortCriteria)`.
- Data: none.
- Errors: Any exception thrown by `OrderBy(...)` / sort builder propagates.
- Rules: none.

### ApplyWhereClause(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
- Purpose: Filter tracking rows by any provided Where criteria.
- Inputs: `query.Where`.
- Logic:
  - Parses `query.Where.FileId` into `fileId` via `Guid.TryParse` (defaults to `Guid.Empty` on parse failure).
  - Parses `query.Where.FileSourceLocationId` into `sourceLocationId` via `Guid.TryParse` (defaults to `Guid.Empty` on parse failure).
  - Builds a dynamic where-clause with these predicates:
    - When `!string.IsNullOrEmpty(q.Where.FileId)`, add `p.FileId == fileId`.
    - When `!string.IsNullOrEmpty(q.Where.FileName)`, add `p.FileName == query.Where.FileName`.
    - When `!string.IsNullOrEmpty(q.Where.FileStatus)`, add `p.FileStatus == query.Where.FileStatus`.
    - When `!string.IsNullOrEmpty(q.Where.FileDirection)`, add `p.FileDirection == query.Where.FileDirection`.
    - When `!string.IsNullOrEmpty(q.Where.FileTransportType)`, add `p.FileTransportType == query.Where.FileTransportType`.
    - When `!string.IsNullOrEmpty(q.Where.FileSourceLocationId)`, add `p.FileSourceLocationId == sourceLocationId`.
    - When `!string.IsNullOrEmpty(q.Where.TradingPartner)`, add `p.TradingPartner == query.Where.TradingPartner`.
    - When `q.Where.StartDate != null`, add `p.CreatedDate >= query.Where.StartDate`.
    - When `q.Where.EndDate != null`, add `p.CreatedDate <= query.Where.EndDate`.
    - When `q.Where.FileSizeFrom > 0M`, add `p.FileSize >= query.Where.FileSizeFrom`.
    - When `q.Where.FileSizeThrough > 0M`, add `p.FileSize <= query.Where.FileSizeThrough`.
  - Applies filtering via `baseQuery.Where(whereClause, query)`.
- Data: none.
- Errors:
  - If `query.Where` is null, accessing `query.Where.*` throws `NullReferenceException`.
- Rules: none.

### AssignResults(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
- Purpose: Materialize results and enrich each returned row with file events and file exceptions.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Sets `query.Results = baseQuery.ToList()`.
  - For each `file` in `query.Results`:
    - Create `FileEventQuery` with `Where.FileId = file.FileId.ToString()`.
    - Create `FileExceptionQuery` with `Where.FileId = file.FileId.ToString()`.
    - Execute nested queries using new executor instances (no DI):
      - `file.Events = new FileEventQueryExecutor().Execute(this.databaseContext, fileEventQuery).Results`
      - `file.Exceptions = new FileExceptionQueryExecutor().Execute(this.databaseContext, fileExceptionQuery).Results`
- Data:
  - Reads `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_SOURCE_LOCATION`, `orm:FTP_SERVER` (base tracking rows).
  - Reads `orm:FTS_FILE_EVENT` and `orm:FTS_FILE_EXCEPTION` (per-result enrichment).
- Errors: Any exception from materialization or nested query execution propagates.
- Rules: none.

### GetBaseQuery(IDbContextBase databaseContext, FileTransferTrackingQuery query)
- Purpose: Build the base query for tracking results.
- Inputs: `databaseContext`, `query`.
- Logic:
  - Stores `this.databaseContext = databaseContext` for later reuse by `AssignResults`.
  - Materializes tracking rows into memory, then unions the lists:
    - `GetFileTransferTrackingUNC(databaseContext)`
    - `GetFileTransferTrackingFTPProtocls(databaseContext)`
  - Returns `result.AsQueryable()`.
- Data: reads `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_SOURCE_LOCATION`, `orm:FTP_SERVER`.
- Errors: Any exception from EF query execution/materialization propagates.
- Rules: none.

### GetFileTransferTrackingUNC(IDbContextBase databaseContext)
- Purpose: Read tracking rows for UNC source locations.
- Inputs: `databaseContext`.
- Logic:
  - Joins `FtsFileManagement` to `FtsSourceLocation` filtered to `SourceProtocalCode == DataAccessConstants.FileStatusConstant.UNCProtocolCD`.
  - Projects each row into `FileTransferTrackingModel` with `OriginatingServerName = string.Empty`.
  - Materializes and returns `result.ToList()`.
- Data: reads `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_SOURCE_LOCATION`.
- Errors: Any exception from EF query execution/materialization propagates.
- Rules: none.

### GetFileTransferTrackingFTPProtocls(IDbContextBase databaseContext)
- Purpose: Read tracking rows for non-UNC (FTP/SFTP/...) source locations that have an FTP server configured.
- Inputs: `databaseContext`.
- Logic:
  - Joins `FtsFileManagement` to `FtsSourceLocation` filtered to `FtpServerId != null`.
  - Joins `FtsSourceLocation` to `FtpServer` on `FtpServerId`.
  - Projects each row into `FileTransferTrackingModel` with `OriginatingServerName = fs.FtpServerName`.
  - Materializes and returns `result.ToList()`.
- Data: reads `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_SOURCE_LOCATION`, `orm:FTP_SERVER`.
- Errors: Any exception from EF query execution/materialization propagates.
- Rules: none.

## GetFtpServerQueryExecutor
Executes `FtpServerQuery` and returns FTP server profile rows as `FtpServerModel`.

### ApplySorting(IQueryable<FtpServerModel> baseQuery, FtpServerQuery query)
- Purpose: Legacy no-op sort hook.
- Inputs: `baseQuery`, `query`.
- Logic: Returns `baseQuery` unchanged.
- Data: none.
- Errors: none.
- Rules: none.

### AssignResults(IQueryable<FtpServerModel> baseQuery, FtpServerQuery query)
- Purpose: Materialize FTP server results into `query.Results`.
- Inputs: `baseQuery`, `query`.
- Logic: Sets `query.Results = baseQuery.ToList()`.
- Data: reads `orm:FTP_SERVER`.
- Errors: Any exception from query execution/materialization propagates.
- Rules: none.

### GetBaseQuery(IDbContextBase context, FtpServerQuery query)
- Purpose: Build the base query for FTP server lookups.
- Inputs: `context`, `query.Where.IsValid`, `query.Where.FtpServerName`.
- Logic:
  - Filters `FtpServer` rows with:
    - `(query.Where.IsValid == null) || identifier.IsValid == query.Where.IsValid`
    - `string.IsNullOrEmpty(query.Where.FtpServerName) || identifier.FtpServerName.ToUpper() == query.Where.FtpServerName.ToUpper()`
  - Projects rows into `FtpServerModel`, including `FtpPassword`.
- Data: reads `orm:FTP_SERVER`.
- Errors:
  - If `query.Where` is null, accessing `query.Where.*` throws `NullReferenceException`.
  - If `identifier.FtpServerName` is null, `identifier.FtpServerName.ToUpper()` throws `NullReferenceException`.
- Rules: none.

## GetSourceLocationQueryExecutor
Executes `GetSourceLocationQuery` to return one source-location record with its destination list attached.

### ApplySorting(IQueryable<SourceLocationModel> baseQuery, GetSourceLocationQuery query)
- Purpose: Legacy no-op sort hook.
- Inputs: `baseQuery`, `query`.
- Logic: Returns `baseQuery` unchanged.
- Data: none.
- Errors: none.
- Rules: none.

### ApplyWhereClause(IQueryable<SourceLocationModel> baseQuery, GetSourceLocationQuery query)
- Purpose: Legacy no-op filter hook.
- Inputs: `baseQuery`, `query`.
- Logic: Returns `baseQuery` unchanged.
- Data: none.
- Errors: none.
- Rules: none.

### AssignResults(IQueryable<SourceLocationModel> baseQuery, GetSourceLocationQuery query)
- Purpose: Materialize results and attach destination-address list to the first result.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Calls `GetDestinationAddressObject(baseQuery.First().FtsSourceLocationId)`.
  - Sets `query.Results = baseQuery.ToList()`.
  - Sets `query.Results.First().DestinationAddress = destinationRecords`.
- Data: reads `orm:FTS_DESTINATION` and `orm:FTS_INBOUND_TARGET` (loaded during `GetBaseQuery`).
- Errors:
  - If `baseQuery` is empty, `baseQuery.First()` throws `InvalidOperationException`.
- Rules: none.

### GetBaseQuery(IDbContextBase databaseContext, GetSourceLocationQuery query)
- Purpose: Load required lookup tables and map a source location to `SourceLocationModel`.
- Inputs: `databaseContext`, `query`.
- Logic:
  - Calls `query.Validate()`.
  - Loads *all* destination-location rows into memory: `this.dest = databaseContext.Set<FtsDestinationLocations>().ToList()`.
  - Loads *all* inbound-target rows into memory: `this.inboundTarget = databaseContext.Set<FtsInboundTarget>().ToList()`.
  - Retrieves a source location via `GetSourceLocation(databaseContext, query)`.
  - When a source location is found:
    - Optionally loads FTP server details via `GetFTPServer(databaseContext, sourceLocation)`.
    - Adds a `SourceLocationModel` with fields mapped from the entity, including:
      - `FtpServerName = ftpServerDetails != null ? ftpServerDetails.FtpServerName : null`
      - `Protocol = sourceLocation.SourceProtocalCode`
      - Day-of-week scheduling flags and time windows.
  - Returns `returnResult.AsQueryable()`.
- Data: reads `orm:FTS_DESTINATION`, `orm:FTS_INBOUND_TARGET`, `orm:FTS_SOURCE_LOCATION`, `orm:FTP_SERVER`.
- Errors: Any exception from validation, EF query execution, or materialization propagates.
- Rules: none.

### GetDestinationAddressObject(Guid soureceLocationId)
- Purpose: Build a destination list for a given source location id.
- Inputs: `soureceLocationId` (Guid).
- Logic:
  - Filters `this.dest` to destination rows where `FtsSourceLocationId == soureceLocationId`.
  - Orders destinations by `CreationDate` descending.
  - For each destination row, creates `FtsDestinationLocationAdded` including:
    - `ImportJobName = GetImportJobName(destination.FTEMetadataId)`.
- Data: reads the preloaded `this.dest` and `this.inboundTarget` collections.
- Errors: Any exception from mapping (including `GetImportJobName`) propagates.
- Rules: none.

### GetSourceLocation(IDbContextBase context, GetSourceLocationQuery query)
- Purpose: Lookup the source location row by id.
- Inputs: `context`, `query.Where.SoureceLocationGuid`.
- Logic:
  - Loads `locations = context.Set<FtsSourceLocation>().Where(s => s.FtsSourceLocationId == query.Where.SoureceLocationGuid).ToList()`.
  - Returns `locations.First()` only when `locations.Count == 1`; otherwise returns null.
- Data: reads `orm:FTS_SOURCE_LOCATION`.
- Errors:
  - If `query.Where` is null, accessing `query.Where.SoureceLocationGuid` throws `NullReferenceException`.
- Rules: none.

### GetFTPServer(IDbContextBase context, FtsSourceLocation sourceLocation)
- Purpose: Optionally load FTP server details for a source location.
- Inputs: `sourceLocation.FtpServerId`.
- Logic:
  - When `sourceLocation.FtpServerId != null`, returns `context.Set<FtpServer>().Where(f => f.FtpServerId == sourceLocation.FtpServerId).FirstOrDefault()`.
  - Otherwise returns null.
- Data: reads `orm:FTP_SERVER`.
- Errors: Any exception from EF query translation/execution propagates.
- Rules: none.

### GetImportJobName(Guid? fteMetadataId)
- Purpose: Resolve the inbound-target `ImportJobName` for a destination row.
- Inputs: `fteMetadataId` (Guid?).
- Logic:
  - Initialize `importName = string.Empty`.
  - When `fteMetadataId != null`, assigns:
    - `importName = this.inboundTarget.FirstOrDefault(f => f.FTEMetadataId == fteMetadataId).ImportJobName`.
  - Returns `importName`.
- Data: reads the preloaded `this.inboundTarget` collection.
- Errors:
  - When `fteMetadataId != null` and there is no matching inbound-target row, `FirstOrDefault(...)` returns null and `.ImportJobName` throws `NullReferenceException`.
- Rules: none.

## InboundTargetQueryExecutor
Executes `InboundTargetQuery` and projects `FtsInboundTarget` rows into `FtsInboundTargetModel` results.

### ApplySorting(IQueryable<FtsInboundTargetModel> baseQuery, InboundTargetQuery query)
- Purpose: Legacy no-op sort hook.
- Inputs: `baseQuery`, `query`.
- Logic: Returns `baseQuery` unchanged.
- Data: none.
- Errors: none.
- Rules: none.

### AssignResults(IQueryable<FtsInboundTargetModel> baseQuery, InboundTargetQuery query)
- Purpose: Materialize results, de-duplicated by `FTEMetadataId`.
- Inputs: `baseQuery`, `query`.
- Logic:
  - Groups `baseQuery` by `FTEMetadataId`.
  - For each group, orders rows by `FtsInboundTargetId` ascending and selects the first row.
  - Assigns `query.Results = ...ToList()`.
- Data: reads `orm:FTS_INBOUND_TARGET`.
- Errors: Any exception from query execution/materialization propagates.
- Rules: none.

### GetBaseQuery(IDbContextBase context, InboundTargetQuery query)
- Purpose: Build the base query for inbound-target lookups.
- Inputs: `query.Where.FTEMetadataId`, `query.Where.ContractName`.
- Logic:
  - Calls `query.Validate()`.
  - Filters `FtsInboundTarget` rows with:
    - When `query.Where.FTEMetadataId != Guid.Empty`, apply equality; otherwise no filter.
    - When `query.Where.ContractName` is not null/empty, apply equality; otherwise no filter.
    - Always require `inboundTarget.IsValid`.
  - Projects each row into `FtsInboundTargetModel` (four fields).
- Data: reads `orm:FTS_INBOUND_TARGET`.
- Errors: Any exception from validation or EF query translation/execution propagates.
- Rules: none.

## IntegrationFileQueryExecutor
Executes `GetIntegrationFileQuery` and projects `FtsIntegrationFile` rows into `FtsIntegrationFileModel` results.

### ApplySorting(IQueryable<FtsIntegrationFileModel> baseQuery, GetIntegrationFileQuery query)
- Purpose: Legacy no-op sort hook.
- Inputs: `baseQuery`, `query`.
- Logic: Returns `baseQuery` unchanged.
- Data: none.
- Errors: none.
- Rules: none.

### AssignResults(IQueryable<FtsIntegrationFileModel> baseQuery, GetIntegrationFileQuery query)
- Purpose: Materialize integration-file results.
- Inputs: `baseQuery`, `query`.
- Logic: Sets `query.Results = baseQuery.ToList()`.
- Data: reads `orm:FTS_INTEGRATION_FILE`.
- Errors: Any exception from query execution/materialization propagates.
- Rules: none.

### GetBaseQuery(IDbContextBase context, GetIntegrationFileQuery query)
- Purpose: Build the base query for integration-file lookups.
- Inputs: `query.Where.FTSIntegrationFileId`, `query.Where.FileName`.
- Logic:
  - Calls `query.Validate()`.
  - Filters `FtsIntegrationFile` rows with:
    - When `FTSIntegrationFileId != Guid.Empty`, apply equality; otherwise no filter.
    - When `FileName` is not null/empty, apply `integrationFile.FileName.Contains(query.Where.FileName)`; otherwise no filter.
    - Always require `integrationFile.IsValid`.
  - Projects each row into `FtsIntegrationFileModel` (six fields).
- Data: reads `orm:FTS_INTEGRATION_FILE`.
- Errors: Any exception from validation or EF query translation/execution propagates.
- Rules: none.

## SearchSourceLocationQueryExecutor
Executes `SourceLocationQuery` and projects `FtsSourceLocation` rows into `SourceLocationModel` results.

### ApplySorting(IQueryable<SourceLocationModel> baseQuery, SourceLocationQuery query)
- Purpose: Apply sorting to source-location search results.
- Inputs: `baseQuery`, `query.SortCriteria`.
- Logic:
  - Builds a dynamic sort definition supporting sort keys: `ConfigId`, `ConfigName`, `SourceAddress`, `SourceFileMask`, `OwnerName`, `ModuleName`, `Status`.
  - Default sort: `ConfigId` ascending, then `IsActive` descending.
  - Applies ordering via `baseQuery.OrderBy(dynamicSort, query.SortCriteria)`.
- Data: none.
- Errors: Any exception thrown by `OrderBy(...)` / sort builder propagates.
- Rules: none.

### ApplyWhereClause(IQueryable<SourceLocationModel> basequery, SourceLocationQuery query)
- Purpose: Apply optional filters to source-location search results.
- Inputs: `query.Where`.
- Logic:
  - Builds a dynamic where clause with these predicates:
    - When `q.Where.ConfigId > 0`, add `p.ConfigId == query.Where.ConfigId`.
    - When `!string.IsNullOrEmpty(q.Where.OwnerName)`, add `p.OwnerName.ToUpper() == query.Where.OwnerName.ToUpper()`.
    - When `!string.IsNullOrEmpty(q.Where.ConfigName)`, add `p.ConfigName.ToUpper() == query.Where.ConfigName.ToUpper()`.
    - When `!string.IsNullOrEmpty(q.Where.OwnerSubsystem)`, add `p.OwnerSubsystem == query.Where.OwnerModuleGuid`.
    - When `!string.IsNullOrEmpty(q.Where.Protocol)`, add `p.Protocol == query.Where.Protocol`.
    - When `q.Where.IsActive` is true, add `p.IsActive == query.Where.IsActive`.
      - Note: this condition cannot be used to filter inactive records (when `IsActive` is false, no filter is applied).
  - Applies filtering via `basequery.Where(whereClause, query)`.
- Data: none.
- Errors:
  - If `query.Where` is null, accessing `query.Where.*` throws `NullReferenceException`.
  - If `p.OwnerName` or `p.ConfigName` is null, calling `.ToUpper()` throws `NullReferenceException` during predicate evaluation.
- Rules: none.

### AssignResults(IQueryable<SourceLocationModel> baseQuery, SourceLocationQuery query)
- Purpose: Materialize the search results into `query.Results`.
- Inputs: `baseQuery`, `query`.
- Logic: Sets `query.Results = baseQuery.ToList()`.
- Data: reads `orm:FTS_SOURCE_LOCATION`.
- Errors: Any exception from query execution/materialization propagates.
- Rules: none.

### GetBaseQuery(IDbContextBase context, SourceLocationQuery query)
- Purpose: Build the base projection used for searching source locations.
- Inputs: `context`.
- Logic:
  - Projects each `FtsSourceLocation` row into `SourceLocationModel`.
- Data: reads `orm:FTS_SOURCE_LOCATION`.
- Errors: Any exception from EF query translation/execution propagates.
- Rules: none.

## Rules implemented

None (this component owns no business rules).

## Not carried over

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (query handlers execute optimized reads; this component implements query-side executors only).
- `patterns/application/api-microservices/PTN-APP-API-016-pagination-pattern.md` section `Solution Overview` (the legacy paging behavior is preserved; target query endpoints still conform to standardized pagination expectations).
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (queries are executed against tenant-scoped databases; tenant identity is not supplied through query payloads).
- `strategy/white-papers/operations/enterprise-kubernetes-strategy.md` section `2.3.4 Secrets Management` (FTP credentials appear in legacy projections; target state must represent credentials via secret references and avoid exposing plaintext secrets).
