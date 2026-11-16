---
type: Generation Brief
title: 'Brief: component backend-data-access-query-handlers'
description: Instructions the tool gave the writer session for the backend-data-access-query-handlers component.
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
  body_sha256: 3ca9131114a39dbc2c74f792f12ab21336a9e0831c25277dcb3aac075cf3ea20
---

# Brief: component `backend-data-access-query-handlers`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.QueryHandlers (backend tier, 8 classes, 43 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-data-access-query-handlers/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-data-access-query-handlers/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Application/Queries`: write `services/fxfer-domain-api/src/Application/Queries` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

- `shared-contracts-queries`: `technical/components/shared-contracts-queries/`

## Flows it takes part in

None.

## Business rules it owns (37)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-014** A source location is returned only when there is exactly one `FtsSourceLocation` row matching the parsed source location GUID; otherwise the lookup returns no `FtsSourceLocation`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:130-135
- Legacy: The executor returns `null` when there are zero or more than one matching rows.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-015** When a source location is found, the returned model includes `IsActive` from `FTS_SOURCE_LOCATION.IS_VALID` and `Suspended` from `FTS_SOURCE_LOCATION.IS_SUSPENDED`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:83-88
- Legacy: The “suspended” value comes from the source location row.
- Decision: keep
- ⚑ Flag: The brief mentions a read of `FTS_SUSPENSION`, but this query path populates “suspended” from `FTS_SOURCE_LOCATION.IS_SUSPENDED` instead.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-016** Destination addresses returned with the source location are ordered by destination `CreationDate` descending (newest first).
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:98-104
- Legacy: Destination ordering is determined by the destination creation timestamp, not by destination id.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-020** When resolving `ImportJobName` for a destination with a non-null `FTEMetadataId`, the service looks up the first inbound target record with a matching `FTEMetadataId` and reads its `ImportJobName`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:144-152
- Legacy: The lookup is performed in-memory from an eagerly-loaded list of inbound targets.
- Decision: keep
- ⚑ Flag: If an inbound target is not found for a given non-null `FTEMetadataId`, the code dereferences `.ImportJobName` on a null object.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-091** Searching source locations reads from `FtsSourceLocation` records and projects them into `SourceLocationModel` results.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:65-79
- Legacy: Selects fields from `FtsSourceLocation` into a `SourceLocationModel` with `ConfigId`, `ConfigName`, `OwnerName`, `OwnerSubsystem`, `Protocol`, `Address`, `Mask`, and `IsActive`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-092** When `ConfigId` is greater than 0, searching source locations returns only source locations whose `ConfigId` equals the requested `ConfigId`.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:43-45
- Legacy: Adds `p => p.ConfigId == query.Where.ConfigId` when `query.Where.ConfigId > 0`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-093** When `OwnerName` is provided, searching source locations filters to exact matches using case-insensitive comparison.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:45-47
- Legacy: Compares `p.OwnerName.ToUpper() == query.Where.OwnerName.ToUpper()`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-094** When `ConfigName` is provided, searching source locations filters to exact matches using case-insensitive comparison.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:47-49
- Legacy: Compares `p.ConfigName.ToUpper() == query.Where.ConfigName.ToUpper()`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-095** When `OwnerSubsystem` is provided, searching source locations parses it as a GUID and filters to source locations whose owner module GUID equals it.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:49-51, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\SourceLocationQueryParms.cs:59-65
- Legacy: Uses `p.OwnerSubsystem == query.Where.OwnerModuleGuid`, where `OwnerModuleGuid` is computed as `Guid.Parse(OwnerSubsystem)`.
- Decision: keep
- ⚑ Flag: `OwnerSubsystem` must be a GUID string; if it is not parseable as a GUID, `Guid.Parse` throws during query construction.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-096** When `Protocol` is provided, searching source locations filters to exact matches of the protocol code.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:51-53
- Legacy: Adds `p => p.Protocol == query.Where.Protocol` when the requested protocol is not empty.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-097** When `IsActive` is true, searching source locations filters to active source locations; when `IsActive` is false, no active/inactive filter is applied.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:53-55
- Legacy: Adds `p => p.IsActive == query.Where.IsActive` only when `query.Where.IsActive` is true.
- Decision: keep
- ⚑ Flag: There is no way to request “inactive only” results using `IsActive = false`, because the where-clause is not applied.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-098** Search results can be sorted by configured field names, and default sorting is by `ConfigId` ascending and then `IsActive` descending.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:20-37
- Legacy: Supports sort field names `ConfigId`, `ConfigName`, `SourceAddress`, `SourceFileMask`, `OwnerName`, `ModuleName`, and `Status`; defaults to `ConfigId` asc then `IsActive` desc.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-099** Search results are assigned by materializing the filtered and sorted query to a list.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:60-63
- Legacy: Sets `query.Results = baseQuery.ToList()`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-102** Before building the response, the details query loads all destination locations and inbound targets into memory.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:47-49
- Legacy: Sets `this.dest = ...Set<FtsDestinationLocations>().ToList()` and `this.inboundTarget = ...Set<FtsInboundTarget>().ToList()`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-103** When a source location row is found, the details query returns a single `SourceLocationModel` populated from the source location record and includes `FtpServerName` when the source location has an FTP server id.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:52-88, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:137-142
- Legacy: Builds a `SourceLocationModel` from the `FtsSourceLocation` fields and sets `FtpServerName = ftpServerDetails != null ? ftpServerDetails.FtpServerName : null`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-105** When the details query assigns results, it also populates the first result’s `DestinationAddress` with all destinations linked to the source location.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:36-41
- Legacy: Gets destinations using `baseQuery.First().FtsSourceLocationId`, sets `query.Results = baseQuery.ToList()`, then sets `query.Results.First().DestinationAddress = destinationRecords`.
- Decision: keep
- ⚑ Flag: `baseQuery.First()` throws if no source location record was returned.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-106** Destination records are ordered by creation date descending and mapped into destination DTOs, including an `ImportJobName` looked up by `FTEMetadataId`.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:96-125, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:144-153
- Legacy: Orders destinations by `CreationDate` descending, maps each destination into `FtsDestinationLocationAdded`, and sets `ImportJobName = inboundTarget.FirstOrDefault(...).ImportJobName`.
- Decision: keep
- ⚑ Flag: When there is no inbound-target row matching `FTEMetadataId`, `FirstOrDefault(...)` returns null and `...ImportJobName` throws.

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-005** The FTP server search reads from `FTP_SERVER` and filters by validity when `Where.IsValid` is provided.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:44-46
- Legacy: Builds the base query from `context.Set<FtpServer>()` and applies `identifier.IsValid == query.Where.IsValid` unless `query.Where.IsValid` is null.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-006** FTP server listing applies different sorting depending on which BAS query door is used: the legacy query door defaults to sorting by `FtpServerName` ascending when no sort is supplied; the newer query doors do not apply sorting.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:27-35, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:159-168, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:18-21
- Legacy: For `file-transfer-svc-query-service-get-ftp-servers`, when `query.SortCriteria` is null/empty the executor sets a single default sort of `FtpServerName` + `Asc` and orders via a dynamic sort; for `file-transfer-svc-query-service-get-ftp-servers-2` and `file-transfer-svc-query-service-get-ftp-servers-3`, the executor's `ApplySorting(...)` returns `baseQuery` without ordering.
- Review: GAP-001
- Review: GAP-004
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-007** FTP server listing applies different `Where.FtpServerId` filtering depending on which BAS query door is used: the legacy query door applies `Where.FtpServerId` as an exact-match filter; the newer query doors do not filter by FTP server id.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:80-93, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:30-33
- Legacy: For `file-transfer-svc-query-service-get-ftp-servers`, when `Where.FtpServerId` is not `Guid.Empty` and its search mode is `EqualTo`, the executor adds a predicate `p.FtpServerId == new Guid(ftpServerIdValue)`; for `file-transfer-svc-query-service-get-ftp-servers-2` and `file-transfer-svc-query-service-get-ftp-servers-3`, the base query only filters by `Where.IsValid` and `Where.FtpServerName`.
- Review: GAP-002
- Review: GAP-005
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-008** FTP server listing applies different text filtering depending on which BAS query door is used: the legacy query door supports case-insensitive, trimmed matching for `Where.FtpServerName`, `Where.FtpDescription`, and `Where.FtpUser` using the configured search mode (`Contains`, `StartsWith`, `EndsWith`, `EqualTo`); the newer query doors only apply a case-insensitive exact match on `Where.FtpServerName` and do not filter by description or user.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:95-140, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:30-33
- Legacy: For `file-transfer-svc-query-service-get-ftp-servers`, when a field is non-empty the executor uppercases and trims the input value and applies the search-mode-specific string operation against `ToUpper().Trim()` on the stored field; for `file-transfer-svc-query-service-get-ftp-servers-2` and `file-transfer-svc-query-service-get-ftp-servers-3`, the base query only compares `identifier.FtpServerName.ToUpper() == query.Where.FtpServerName.ToUpper()` when `Where.FtpServerName` is non-empty.
- Review: GAP-003
- Review: GAP-006
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-009** Each matching FTP server result projects a fixed set of fields from `FTP_SERVER` into the returned `FtpServerModel`, including credentials fields.
- Doors: file-transfer-svc-query-service-get-ftp-servers, file-transfer-svc-query-service-get-ftp-servers-2, file-transfer-svc-query-service-get-ftp-servers-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:44-63
- Legacy: Selects columns into `FtpServerModel`, including `FtpPassword`, `FtpUser`, `FtpServerIp`, `FtpServerName`, and proxy/port fields.
- Decision: keep
- ⚑ Flag: `FtpPassword` is selected from `FTP_SERVER` and included in the query result model.

From `technical/flows/manage-integration-files/RULES.md`:

**F03-005** Only active integration file definitions are returned by the lookup.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:31-35
- Legacy: The lookup includes `integrationFile.IsValid` in its filter, excluding inactive definitions.
- Decision: keep

From `technical/flows/manage-integration-files/RULES.md`:

**F03-006** When a specific integration file id is supplied, only that exact definition is returned; when no id is supplied, the lookup does not filter by id.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:31-33, QPARMS-UA3:18-21
- Legacy: The default id is `Guid.Empty`, and the lookup applies an equality filter only when the supplied id is not empty.
- Decision: keep

From `technical/flows/manage-integration-files/RULES.md`:

**F03-007** When a file name fragment is supplied, the lookup returns only definitions whose stored file name contains that fragment; when no fragment is supplied, the lookup does not filter by name.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:33-34
- Legacy: The lookup applies `FileName.Contains(fragment)` only when the fragment is not empty.
- Decision: keep

From `technical/flows/manage-integration-files/RULES.md`:

**F03-008** The lookup does not apply any explicit ordering to the returned definitions.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:18-21
- Legacy: Sorting returns the query as-is, so ordering is whatever the underlying store returns.
- Decision: keep
- ⚑ Flag: callers cannot rely on a stable or meaningful order

From `technical/flows/manage-integration-files/RULES.md`:

**F03-009** Each returned definition includes only: group id, outbound metadata definition id, configuration id, file name, processed flag, and integration file id; other model fields are left at their default values.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:35-43, MODEL:15-129
- Legacy: The lookup constructs `new FtsIntegrationFileModel { ... }` and does not assign `IsValid`, `CreationDate`, or `LastModifiedDate` (among others), so they remain defaults.
- Decision: keep
- ⚑ Flag: consumers may see default values for fields that exist on the model but are not populated by this lookup

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-023** When a `FileTransferTrackingQuery.Where` field is provided, file transfer movement results are filtered by exact match (string fields) or by inclusive range (date/size fields).
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:50-82
- Legacy: The query adds `WHERE` predicates only when a corresponding `Where` value is present (`FileId`, `FileName`, `FileStatus`, `FileDirection`, `FileTransportType`, `FileSourceLocationId`, `TradingPartner`, `StartDate`, `EndDate`, `FileSizeFrom`, `FileSizeThrough`).
- ⚑ Flag: `Where.FileId` and `Where.FileSourceLocationId` are parsed with `Guid.TryParse`; if parsing fails but the input string is non-empty, the filter compares to `Guid.Empty`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-024** File transfer movement sorting supports specific sort keys and defaults to ascending `FileName`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:31-47
- Legacy: Sort keys map as `FileId`, `FileName`, `FileStatus`, `FileDirection`, `FileSize`, `CreatedDate`, and `TradingPartnerId` (sorting by `TradingPartner`); when no sort is requested the default is ascending `FileName`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-025** File transfer movement results are built from valid source locations and split into two protocol cases: UNC and non-UNC with an FTP server.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:111-170, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\DataAccessConstants.cs:16-22
- Legacy: The base result set is `UNC (SourceProtocalCode == UNCProtocolCD (3) and IsValid == true) UNION FTP (FtpServerId != null and IsValid == true)` joined to file management; UNC results set `OriginatingServerName` to `string.Empty` and FTP results set it to the joined `FtpServerName`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-026** Each file transfer movement result is enriched with its file events and file exceptions.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileTransferTrackingQueryExecutor.cs:84-108
- Legacy: After the result list is materialized, the executor runs a `FileEventQuery` and a `FileExceptionQuery` per result (using `FileId.ToString()` as input) and assigns `file.Events` and `file.Exceptions`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-027** File event lookups for movement results filter by `FileId` (when provided) and default-sort by `EventId` then `EventCode`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileEventQueryExecutor.cs:23-34, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileEventQueryExecutor.cs:36-46
- Legacy: The executor parses `Where.FileId` with `Guid.TryParse`, filters `FileId` when `Where.FileId != null`, and applies the default order `(EventId ASC, EventCode ASC)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-028** File event lookups project event fields from `FtsFileEvent` and use the related file management `CreatedDate` as the returned `CreatedDate`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileEventQueryExecutor.cs:53-65
- Legacy: Event results include `EventId`, `FileId`, `EventCD` (as `EventCode`), `EventTrackDescription`, and `CreatedDate = FtsFileManagement.CreatedDate`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-029** File exception lookups for movement results filter by `FileId` (when provided) and default-sort by `ExceptionId` then `ExceptionType`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileExceptionQueryExecutor.cs:23-34, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileExceptionQueryExecutor.cs:36-46
- Legacy: The executor parses `Where.FileId` with `Guid.TryParse`, filters `FileId` when `Where.FileId != null`, and applies the default order `(ExceptionId ASC, ExceptionType ASC)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-030** File exception lookups project exception fields from `FtsFileException` and use the related file management `CreatedDate` as the returned `CreatedDate`.
- Doors: file-transfer-svc-query-service-search-file-transfer-movement, file-transfer-svc-query-service-search-file-transfer-movement-2, file-transfer-svc-query-service-search-file-transfer-movement-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\FileExceptionQueryExecutor.cs:53-68
- Legacy: Exception results include `ExceptionId`, `EventId`, `FileId`, `ExceptionType`, `Message`, `StackTrace`, `MethodName`, and `CreatedDate = FtsFileManagement.CreatedDate`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-032** Inbound target results include only valid records and support optional filtering by `FTEMetadataId` and `ContractName`.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\InboundTargetQueryExecutor.cs:31-44
- Legacy: The query applies `inboundTarget.IsValid` and conditionally filters by `FTEMetadataId` (when not `Guid.Empty`) and `ContractName` (when not null/empty), returning `FTEMetadataId`, `FtsInboundTargetId`, `ImportJobName`, and `ContractName`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-033** Inbound target results are de-duplicated to a single record per `FTEMetadataId`, selecting the smallest `FtsInboundTargetId` within each group.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\InboundTargetQueryExecutor.cs:23-28
- Legacy: After building the base query, results are grouped by `FTEMetadataId` and reduced to `OrderBy(FtsInboundTargetId).FirstOrDefault()` per group.
- Decision: keep

