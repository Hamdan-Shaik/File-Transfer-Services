---
type: Target Placement
title: technical/components/backend-data-access-query-handlers-2/TARGET
description: Where the backend-data-access-query-handlers-2 component goes in the new system and what to build, method by method.
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
  body_sha256: 46f95fc6a7bf7b94e5d63de92b714800bd8064e1d007c3b0353a72ef005c6665
---

﻿# Target: backend-data-access-query-handlers-2

Where: `services/fxfer-domain-api/src/Application/Queries`

## What to create

Create query-execution classes in `services/fxfer-domain-api/src/Application/Queries` that implement the legacy query behaviors described in `technical/components/backend-data-access-query-handlers-2/SPEC.md`.

Implement each query executor as a DI-friendly class that:

- Uses tenant-scoped `FxferDbContext` (EF Core + Npgsql) for data access.
- Uses async query APIs (`ToListAsync(ct)` etc).
- Performs reads only (no side effects).

Create these target types:

- `FileEventQueryExecutor`
  - `Task<IReadOnlyList<FileEventModel>> ExecuteAsync(FxferDbContext dbContext, FileEventQuery query, CancellationToken ct)`

- `FileExceptionQueryExecutor`
  - `Task<IReadOnlyList<FileExceptionModel>> ExecuteAsync(FxferDbContext dbContext, FileExceptionQuery query, CancellationToken ct)`

- `FileTransferTrackingQueryExecutor`
  - `Task<IReadOnlyList<FileTransferTrackingModel>> ExecuteAsync(FxferDbContext dbContext, FileTransferTrackingQuery query, CancellationToken ct)`
  - Must preserve the legacy in-memory behavior:
    - Materialize UNC and FTP-protocol tracking lists first.
    - Union the lists in memory.
    - Apply where/sort/paging against the in-memory results.
  - Must preserve enrichment: after materializing the paged results, query and assign `Events` and `Exceptions` for each returned file.
  - Private helpers (keep legacy behavior):
    - `Task<List<FileTransferTrackingModel>> GetFileTransferTrackingUncAsync(FxferDbContext dbContext, CancellationToken ct)`
    - `Task<List<FileTransferTrackingModel>> GetFileTransferTrackingFtpProtocolsAsync(FxferDbContext dbContext, CancellationToken ct)`

- `GetFtpServerQueryExecutor`
  - `Task<IReadOnlyList<FtpServerModel>> ExecuteAsync(FxferDbContext dbContext, FtpServerQuery query, CancellationToken ct)`
  - Must preserve legacy filtering semantics (including `.ToUpper()` comparisons).
  - Security posture: in the target repository, FTP credentials are represented using secret references; map the legacy `FtpPassword` projection to the target credential reference field used by `fxfer-domain-api` (per `technical/target/deployables/fxfer-domain-api/CONTRACT.md`).

- `GetSourceLocationQueryExecutor`
  - `Task<IReadOnlyList<SourceLocationModel>> ExecuteAsync(FxferDbContext dbContext, GetSourceLocationQuery query, CancellationToken ct)`
  - Must preserve eager-load semantics:
    - Load *all* destination rows and *all* inbound-target rows into memory before mapping destinations.
  - Must preserve assignment behavior:
    - Destination list is assigned onto the first result only.
    - When the base query result is empty, preserve the legacy exception behavior (`First()` throws).
  - Private helpers (keep legacy behavior):
    - `Task<FtsSourceLocation?> GetSourceLocationAsync(FxferDbContext dbContext, GetSourceLocationQuery query, CancellationToken ct)` (returns a row only when exactly one match exists)
    - `Task<FtpServer?> GetFtpServerAsync(FxferDbContext dbContext, FtsSourceLocation sourceLocation, CancellationToken ct)`
    - `string GetImportJobName(Guid? fteMetadataId, List<FtsInboundTarget> inboundTargets)`
    - `List<FtsDestinationLocationAdded> GetDestinationAddressObject(Guid sourceLocationId, List<FtsDestinationLocations> destinations, List<FtsInboundTarget> inboundTargets)`

- `InboundTargetQueryExecutor`
  - `Task<IReadOnlyList<FtsInboundTargetModel>> ExecuteAsync(FxferDbContext dbContext, InboundTargetQuery query, CancellationToken ct)`
  - Must preserve legacy de-duplication behavior (group by `FTEMetadataId`, choose lowest `FtsInboundTargetId`).

- `IntegrationFileQueryExecutor`
  - `Task<IReadOnlyList<FtsIntegrationFileModel>> ExecuteAsync(FxferDbContext dbContext, GetIntegrationFileQuery query, CancellationToken ct)`

- `SearchSourceLocationQueryExecutor`
  - `Task<IReadOnlyList<SourceLocationModel>> ExecuteAsync(FxferDbContext dbContext, SourceLocationQuery query, CancellationToken ct)`
  - Must preserve legacy filter semantics (including the `IsActive` filter that only applies when `IsActive` is true).

Notes:

- Preserve the legacy string-comparison and GUID-parsing behaviors exactly (`Guid.TryParse`, null/empty checks, `.ToUpper()` comparisons).
- Preserve the legacy ordering behaviors (including the cases where no explicit ordering is applied).

## Legacy → Target mapping

| Legacy method | Target method | Note |
| --- | --- | --- |
| `FileEventQueryExecutor.ApplyPaging(...)` | `FileEventQueryExecutor.ExecuteAsync(...)` | Paging logic is applied inside the target executor method. |
| `FileEventQueryExecutor.ApplySorting(...)` | `FileEventQueryExecutor.ExecuteAsync(...)` | Applies default sort `EventId ASC, EventCode ASC`. |
| `FileEventQueryExecutor.ApplyWhereClause(...)` | `FileEventQueryExecutor.ExecuteAsync(...)` | Applies `FileId` filter when `Where.FileId != null` (invalid GUID parses as `Guid.Empty`). |
| `FileEventQueryExecutor.AssignResults(...)` | `FileEventQueryExecutor.ExecuteAsync(...)` | Materialization done via `ToListAsync`. |
| `FileEventQueryExecutor.GetBaseQuery(...)` | `FileEventQueryExecutor.ExecuteAsync(...)` | Join/projection preserved. |
| `FileExceptionQueryExecutor.ApplyPaging(...)` | `FileExceptionQueryExecutor.ExecuteAsync(...)` | Paging logic is applied inside the target executor method. |
| `FileExceptionQueryExecutor.ApplySorting(...)` | `FileExceptionQueryExecutor.ExecuteAsync(...)` | Applies default sort `ExceptionId ASC, ExceptionType ASC`. |
| `FileExceptionQueryExecutor.ApplyWhereClause(...)` | `FileExceptionQueryExecutor.ExecuteAsync(...)` | Applies `FileId` filter when `Where.FileId != null` (invalid GUID parses as `Guid.Empty`). |
| `FileExceptionQueryExecutor.AssignResults(...)` | `FileExceptionQueryExecutor.ExecuteAsync(...)` | Materialization done via `ToListAsync`. |
| `FileExceptionQueryExecutor.GetBaseQuery(...)` | `FileExceptionQueryExecutor.ExecuteAsync(...)` | Join/projection preserved. |
| `FileTransferTrackingQueryExecutor.ApplyPaging(...)` | `FileTransferTrackingQueryExecutor.ExecuteAsync(...)` | Paging logic is applied after the UNC/FTP union is materialized into memory. |
| `FileTransferTrackingQueryExecutor.ApplySorting(...)` | `FileTransferTrackingQueryExecutor.ExecuteAsync(...)` | Sort keys + default `FileName ASC` preserved. |
| `FileTransferTrackingQueryExecutor.ApplyWhereClause(...)` | `FileTransferTrackingQueryExecutor.ExecuteAsync(...)` | Optional filters preserved (including date and file-size bounds). |
| `FileTransferTrackingQueryExecutor.AssignResults(...)` | `FileTransferTrackingQueryExecutor.ExecuteAsync(...)` | Materialization + per-file enrichment preserved. |
| `FileTransferTrackingQueryExecutor.GetBaseQuery(...)` | `FileTransferTrackingQueryExecutor.ExecuteAsync(...)` | UNC/FTP queries are executed first, then combined and treated as in-memory queryable. |
| `FileTransferTrackingQueryExecutor.GetFileTransferTrackingUNC(...)` | `FileTransferTrackingQueryExecutor.GetFileTransferTrackingUncAsync(...)` | UNC join/projection preserved (`OriginatingServerName = ""`). |
| `FileTransferTrackingQueryExecutor.GetFileTransferTrackingFTPProtocls(...)` | `FileTransferTrackingQueryExecutor.GetFileTransferTrackingFtpProtocolsAsync(...)` | FTP join/projection preserved (`OriginatingServerName = FtpServerName`). |
| `GetFtpServerQueryExecutor.ApplySorting(...)` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | No explicit ordering preserved. |
| `GetFtpServerQueryExecutor.AssignResults(...)` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | Materialization done via `ToListAsync`. |
| `GetFtpServerQueryExecutor.GetBaseQuery(...)` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | Filtering + projection preserved (including `.ToUpper()` comparisons). |
| `GetSourceLocationQueryExecutor.ApplySorting(...)` | `GetSourceLocationQueryExecutor.ExecuteAsync(...)` | No-op sorting preserved. |
| `GetSourceLocationQueryExecutor.ApplyWhereClause(...)` | `GetSourceLocationQueryExecutor.ExecuteAsync(...)` | No-op filtering preserved. |
| `GetSourceLocationQueryExecutor.AssignResults(...)` | `GetSourceLocationQueryExecutor.ExecuteAsync(...)` | Preserves destination list assignment onto the first result (and exception behavior on empty). |
| `GetSourceLocationQueryExecutor.GetBaseQuery(...)` | `GetSourceLocationQueryExecutor.ExecuteAsync(...)` | Preserves eager-load + mapping behavior and `query.Validate()` call. |
| `GetSourceLocationQueryExecutor.GetDestinationAddressObject(Guid)` | `GetSourceLocationQueryExecutor.GetDestinationAddressObject(...)` | Destination ordering + mapping preserved. |
| `GetSourceLocationQueryExecutor.GetSourceLocation(...)` | `GetSourceLocationQueryExecutor.GetSourceLocationAsync(...)` | Preserves “exactly one row” lookup semantics. |
| `GetSourceLocationQueryExecutor.GetFTPServer(...)` | `GetSourceLocationQueryExecutor.GetFtpServerAsync(...)` | Preserves optional FTP server lookup semantics. |
| `GetSourceLocationQueryExecutor.GetImportJobName(Guid?)` | `GetSourceLocationQueryExecutor.GetImportJobName(...)` | Preserves null/empty return and null-reference behavior when inbound-target row is missing. |
| `InboundTargetQueryExecutor.ApplySorting(...)` | `InboundTargetQueryExecutor.ExecuteAsync(...)` | No-op sorting preserved. |
| `InboundTargetQueryExecutor.AssignResults(...)` | `InboundTargetQueryExecutor.ExecuteAsync(...)` | Preserves group-by de-dup selection (`min(FtsInboundTargetId)`). |
| `InboundTargetQueryExecutor.GetBaseQuery(...)` | `InboundTargetQueryExecutor.ExecuteAsync(...)` | Preserves filters (`FTEMetadataId`, `ContractName`, `IsValid == true`). |
| `IntegrationFileQueryExecutor.ApplySorting(...)` | `IntegrationFileQueryExecutor.ExecuteAsync(...)` | No explicit ordering preserved. |
| `IntegrationFileQueryExecutor.AssignResults(...)` | `IntegrationFileQueryExecutor.ExecuteAsync(...)` | Materialization done via `ToListAsync`. |
| `IntegrationFileQueryExecutor.GetBaseQuery(...)` | `IntegrationFileQueryExecutor.ExecuteAsync(...)` | Preserves filters (`FTSIntegrationFileId`, `FileName.Contains`, `IsValid == true`). |
| `SearchSourceLocationQueryExecutor.ApplySorting(...)` | `SearchSourceLocationQueryExecutor.ExecuteAsync(...)` | Preserves sort keys + default ordering. |
| `SearchSourceLocationQueryExecutor.ApplyWhereClause(...)` | `SearchSourceLocationQueryExecutor.ExecuteAsync(...)` | Preserves optional filters (including `IsActive` filtering only when `IsActive == true`). |
| `SearchSourceLocationQueryExecutor.AssignResults(...)` | `SearchSourceLocationQueryExecutor.ExecuteAsync(...)` | Materialization done via `ToListAsync`. |
| `SearchSourceLocationQueryExecutor.GetBaseQuery(...)` | `SearchSourceLocationQueryExecutor.ExecuteAsync(...)` | Base projection preserved. |

## Dependencies

- Depends on component `shared-contracts-queries` for query request contract types and their `Validate()` behavior.
- Depends on component `shared-queries-parameters-2` for `GetIntegrationFileQuery` and related DTOs.
- Uses tenant-scoped `FxferDbContext` + EF Core for persistence.
- Uses the entity types placed by `data-data-access-entities*` components for LINQ queries.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (query handlers execute optimized queries against read models; queries are read-only).
- `patterns/application/api-microservices/PTN-APP-API-016-pagination-pattern.md` section `Solution Overview` (collection reads support standardized, bounded pagination; legacy paging behavior is preserved in the query contracts).
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (tenant isolation via database-per-tenant routing; query handlers never accept tenant identity from request payloads).
- `strategy/white-papers/operations/enterprise-kubernetes-strategy.md` section `2.3.4 Secrets Management` (credentials are represented using secret references; avoid returning plaintext secrets).
