---
type: Target Placement
title: 'Target: backend-data-access-query-handlers'
description: Where the backend-data-access-query-handlers component goes in the new system and what to build, method by method.
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
  body_sha256: f0e07620f7707eaeb2e4d3a173c4988a7623933d6584d7a2c87ad34aab8fdc36
---

# Target: backend-data-access-query-handlers

Where: `services/fxfer-domain-api/src/Application/Queries`

## What to create

Create query-execution classes in `services/fxfer-domain-api/src/Application/Queries` that implement the legacy query behaviors described in `technical/components/backend-data-access-query-handlers/SPEC.md`.

Implement each query executor as a DI-friendly class that:

- Uses tenant-scoped `FxferDbContext` (EF Core + Npgsql) for data access.
- Uses async query APIs (`ToListAsync(ct)` etc).
- Performs reads only (no side effects).

Create these target types:

- `FileEventQueryExecutor`
  - `Task<IReadOnlyList<FileEventModel>> ExecuteAsync(FxferDbContext dbContext, FileEventQuery query, CancellationToken ct)`
  - Implements rules F05-027 and F05-028.

- `FileExceptionQueryExecutor`
  - `Task<IReadOnlyList<FileExceptionModel>> ExecuteAsync(FxferDbContext dbContext, FileExceptionQuery query, CancellationToken ct)`
  - Implements rules F05-029 and F05-030.

- `FileTransferTrackingQueryExecutor`
  - `Task<IReadOnlyList<FileTransferTrackingModel>> ExecuteAsync(FxferDbContext dbContext, FileTransferTrackingQuery query, CancellationToken ct)`
  - Must preserve the per-result enrichment behavior (rules F05-026..030): after materializing the tracking list, run the file-event and file-exception queries for each returned file and assign them onto the returned models.
  - Private helpers (keep legacy behavior):
    - `Task<List<FileTransferTrackingModel>> GetFileTransferTrackingUncAsync(FxferDbContext dbContext, CancellationToken ct)`
    - `Task<List<FileTransferTrackingModel>> GetFileTransferTrackingFtpProtocolsAsync(FxferDbContext dbContext, CancellationToken ct)`

- `GetFtpServerQueryExecutor`
  - `Task<IReadOnlyList<FtpServerModel>> ExecuteAsync(FxferDbContext dbContext, FtpServerQuery query, CancellationToken ct)`
  - Must preserve legacy door differences described by rules F02-006..008.
  - Security posture: in the target repository, FTP credentials are represented using secret references; map the legacy `FtpPassword` projection to the target credential reference field used by `fxfer-domain-api` (per `technical/target/deployables/fxfer-domain-api/CONTRACT.md`).

- `GetSourceLocationQueryExecutor`
  - `Task<IReadOnlyList<SourceLocationModel>> ExecuteAsync(FxferDbContext dbContext, GetSourceLocationQuery query, CancellationToken ct)`
  - Must preserve eager-loading behavior (rule F01-102): load destination and inbound-target rows into memory before mapping destinations.
  - Must preserve assignment behavior (rule F01-105): the destination list is assigned onto the first result only.
  - Private helpers (keep legacy behavior):
    - `FtsSourceLocation? GetSourceLocation(FxferDbContext dbContext, GetSourceLocationQuery query)`
    - `FtpServer? GetFtpServer(FxferDbContext dbContext, FtsSourceLocation sourceLocation)`
    - `string GetImportJobName(Guid? fteMetadataId, List<FtsInboundTarget> inboundTargets)`
    - `List<FtsDestinationLocationAdded> GetDestinationAddressObject(Guid sourceLocationId, List<FtsDestinationLocations> destinations, List<FtsInboundTarget> inboundTargets)`

- `InboundTargetQueryExecutor`
  - `Task<IReadOnlyList<FtsInboundTargetModel>> ExecuteAsync(FxferDbContext dbContext, InboundTargetQuery query, CancellationToken ct)`

- `IntegrationFileQueryExecutor`
  - `Task<IReadOnlyList<FtsIntegrationFileModel>> ExecuteAsync(FxferDbContext dbContext, GetIntegrationFileQuery query, CancellationToken ct)`

- `SearchSourceLocationQueryExecutor`
  - `Task<IReadOnlyList<SourceLocationModel>> ExecuteAsync(FxferDbContext dbContext, SourceLocationQuery query, CancellationToken ct)`

Notes:

- Preserve the legacy string-comparison and GUID-parsing behaviors exactly (case-insensitive comparisons, `Guid.TryParse` vs `Guid.Parse`, and null/empty checks).
- Preserve the legacy ordering behaviors (including the cases where no explicit ordering is applied).

## Legacy → Target mapping

| Legacy method | Target method | Note |
| --- | --- | --- |
| `FileEventQueryExecutor.ApplyPaging(...)` | `FileEventQueryExecutor.ExecuteAsync(...)` | Paging logic is applied inside the target executor method. |
| `FileEventQueryExecutor.ApplySorting(...)` | `FileEventQueryExecutor.ExecuteAsync(...)` | Applies default sort `EventId ASC, EventCode ASC` (F05-027). |
| `FileEventQueryExecutor.ApplyWhereClause(...)` | `FileEventQueryExecutor.ExecuteAsync(...)` | Applies `FileId` filter when provided (F05-027). |
| `FileEventQueryExecutor.AssignResults(...)` | `FileEventQueryExecutor.ExecuteAsync(...)` | Materialization done via `ToListAsync`. |
| `FileEventQueryExecutor.GetBaseQuery(...)` | `FileEventQueryExecutor.ExecuteAsync(...)` | Join/projection preserved (F05-028). |
| `FileExceptionQueryExecutor.ApplyPaging(...)` | `FileExceptionQueryExecutor.ExecuteAsync(...)` | Paging logic is applied inside the target executor method. |
| `FileExceptionQueryExecutor.ApplySorting(...)` | `FileExceptionQueryExecutor.ExecuteAsync(...)` | Applies default sort `ExceptionId ASC, ExceptionType ASC` (F05-029). |
| `FileExceptionQueryExecutor.ApplyWhereClause(...)` | `FileExceptionQueryExecutor.ExecuteAsync(...)` | Applies `FileId` filter when provided (F05-029). |
| `FileExceptionQueryExecutor.AssignResults(...)` | `FileExceptionQueryExecutor.ExecuteAsync(...)` | Materialization done via `ToListAsync`. |
| `FileExceptionQueryExecutor.GetBaseQuery(...)` | `FileExceptionQueryExecutor.ExecuteAsync(...)` | Join/projection preserved (F05-030). |
| `FileTransferTrackingQueryExecutor.ApplyPaging(...)` | `FileTransferTrackingQueryExecutor.ExecuteAsync(...)` | Paging logic is applied inside the target executor method. |
| `FileTransferTrackingQueryExecutor.ApplySorting(...)` | `FileTransferTrackingQueryExecutor.ExecuteAsync(...)` | Supports legacy sort keys and default `FileName ASC` (F05-024). |
| `FileTransferTrackingQueryExecutor.ApplyWhereClause(...)` | `FileTransferTrackingQueryExecutor.ExecuteAsync(...)` | Applies optional exact/range filters (F05-023). |
| `FileTransferTrackingQueryExecutor.AssignResults(...)` | `FileTransferTrackingQueryExecutor.ExecuteAsync(...)` | Preserves per-row enrichment with events/exceptions (F05-026). |
| `FileTransferTrackingQueryExecutor.GetBaseQuery(...)` | `FileTransferTrackingQueryExecutor.ExecuteAsync(...)` | Preserves UNC/FTP union base query (F05-025). |
| `FileTransferTrackingQueryExecutor.GetFileTransferTrackingFTPProtocls(...)` | `FileTransferTrackingQueryExecutor.GetFileTransferTrackingFtpProtocolsAsync(...)` | Preserves FTP join including `OriginatingServerName` mapping (F05-025). |
| `FileTransferTrackingQueryExecutor.GetFileTransferTrackingUNC(...)` | `FileTransferTrackingQueryExecutor.GetFileTransferTrackingUncAsync(...)` | Preserves UNC mapping (`OriginatingServerName=string.Empty`) (F05-025). |
| `GetFtpServerQueryExecutor.ApplyPaging(...)` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | Paging logic is applied inside the target executor method. |
| `GetFtpServerQueryExecutor.ApplySorting(...)` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | Preserves door-specific ordering semantics (F02-006). |
| `GetFtpServerQueryExecutor.ApplyWhereClause(...)` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | Applies door-specific filtering semantics (F02-007, F02-008). |
| `GetFtpServerQueryExecutor.AssignResults(...)` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | Materialization done via `ToListAsync`. |
| `GetFtpServerQueryExecutor.BuildDynamicSort()` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | Sort-field mapping is implemented directly in the target LINQ ordering logic. |
| `GetFtpServerQueryExecutor.GetBaseQuery(...)` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | Projection fields preserved (F02-005, F02-009). |
| `GetFtpServerQueryExecutor.GetDynamicPredicate(...)` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | Search-mode and match semantics implemented directly in target predicate composition (F02-007, F02-008). |
| `GetFtpServerQueryExecutor.SetDefaultSortCriteria()` | `GetFtpServerQueryExecutor.ExecuteAsync(...)` | Default sort `FtpServerName ASC` preserved for legacy door (F02-006). |
| `GetSourceLocationQueryExecutor.ApplySorting(...)` | `GetSourceLocationQueryExecutor.ExecuteAsync(...)` | No-op sorting preserved. |
| `GetSourceLocationQueryExecutor.ApplyWhereClause(...)` | `GetSourceLocationQueryExecutor.ExecuteAsync(...)` | No-op filtering preserved. |
| `GetSourceLocationQueryExecutor.AssignResults(...)` | `GetSourceLocationQueryExecutor.ExecuteAsync(...)` | Preserves `DestinationAddress` assignment onto first result (F01-105). |
| `GetSourceLocationQueryExecutor.GetBaseQuery(...)` | `GetSourceLocationQueryExecutor.ExecuteAsync(...)` | Preserves eager-load + mapping behavior (F01-015, F01-102, F01-103). |
| `GetSourceLocationQueryExecutor.GetDestinationAddressObject(Guid)` | `GetSourceLocationQueryExecutor.GetDestinationAddressObject(...)` | Preserves destination ordering and mapping (F01-016, F01-106). |
| `GetSourceLocationQueryExecutor.GetFTPServer(...)` | `GetSourceLocationQueryExecutor.GetFtpServer(...)` | Preserves optional FTP server name lookup (F01-103). |
| `GetSourceLocationQueryExecutor.GetImportJobName(Guid?)` | `GetSourceLocationQueryExecutor.GetImportJobName(...)` | Preserves inbound-target lookup semantics (F01-020). |
| `GetSourceLocationQueryExecutor.GetSourceLocation(...)` | `GetSourceLocationQueryExecutor.GetSourceLocation(...)` | Preserves “exactly one row” lookup rule (F01-014). |
| `InboundTargetQueryExecutor.ApplySorting(...)` | `InboundTargetQueryExecutor.ExecuteAsync(...)` | No-op sorting preserved. |
| `InboundTargetQueryExecutor.AssignResults(...)` | `InboundTargetQueryExecutor.ExecuteAsync(...)` | Preserves `FTEMetadataId` de-dup selection (F05-033). |
| `InboundTargetQueryExecutor.GetBaseQuery(...)` | `InboundTargetQueryExecutor.ExecuteAsync(...)` | Preserves filters and projection (F05-032). |
| `IntegrationFileQueryExecutor.ApplySorting(...)` | `IntegrationFileQueryExecutor.ExecuteAsync(...)` | No explicit ordering preserved (F03-008). |
| `IntegrationFileQueryExecutor.AssignResults(...)` | `IntegrationFileQueryExecutor.ExecuteAsync(...)` | Materialization done via `ToListAsync`. |
| `IntegrationFileQueryExecutor.GetBaseQuery(...)` | `IntegrationFileQueryExecutor.ExecuteAsync(...)` | Preserves filters and partial projection (F03-005..007, F03-009). |
| `SearchSourceLocationQueryExecutor.ApplySorting(...)` | `SearchSourceLocationQueryExecutor.ExecuteAsync(...)` | Preserves sort keys and default ordering (F01-098). |
| `SearchSourceLocationQueryExecutor.ApplyWhereClause(...)` | `SearchSourceLocationQueryExecutor.ExecuteAsync(...)` | Preserves optional filters (F01-092..097). |
| `SearchSourceLocationQueryExecutor.AssignResults(...)` | `SearchSourceLocationQueryExecutor.ExecuteAsync(...)` | Materialization done via `ToListAsync` (F01-099). |
| `SearchSourceLocationQueryExecutor.GetBaseQuery(...)` | `SearchSourceLocationQueryExecutor.ExecuteAsync(...)` | Preserves base projection (F01-091). |

## Dependencies

- Depends on component `shared-contracts-queries` for query request types.
- Uses `FxferDbContext` + EF Core for persistence.
- Uses the entity types placed by `data-data-access-entities*` components for LINQ queries.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (query handlers execute optimized reads against read models; queries are read-only)
- `patterns/application/api-microservices/PTN-APP-API-016-pagination-pattern.md` section `Solution Overview` (standard pagination shape and bounded page sizes for collection reads)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (tenant isolation via database-per-tenant routing; query handlers never accept tenant identity from request bodies)
- `strategy/white-papers/operations/enterprise-kubernetes-strategy.md` section `2.3.4 Secrets Management` (credentials are represented using secret references; avoid returning plaintext secrets)

