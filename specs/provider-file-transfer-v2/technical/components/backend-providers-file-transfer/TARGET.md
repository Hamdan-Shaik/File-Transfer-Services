---
type: Target Placement
title: technical/components/backend-providers-file-transfer/TARGET
description: Where the backend-providers-file-transfer component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-providers-file-transfer
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T20:52:42Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-providers-file-transfer/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-providers-file-transfer
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 4
  methods: 73
  owned_rules: 15
  flows: []
  depends_on: []
  deployable: fxfer-bff
  folder: src/Application/Providers
  body_sha256: 58fa4b67eb0073f666a7f1dfc8871d9d69ade2d509ea031d72b4036ca8eccaba
---

﻿# Target: `backend-providers-file-transfer`

## Where

- Deployable: `fxfer-bff` (`services/fxfer-bff`)
- Folder: `services/fxfer-bff/src/Application/Providers`

## What to create

Create provider classes used by the BFF endpoints/controllers to perform file-transfer configuration commands/queries, FTP server credential commands/queries, and file-tracking queries.

The target implementation must:

- Call `fxfer-domain-api` for all authoritative reads/writes (no direct database access).
- Apply the owned legacy mapping rules (phone normalization, FileToEvent address override, Guid.Empty mapping, schedule field defaults, suspended changed date stamping).
- Surface downstream failures as exceptions/typed results that the BFF error middleware maps to Problem Details.

Create the following types in `services/fxfer-bff/src/Application/Providers`:

- `FileTransferCommandProvider` (implements `IFileTransferCommandProvider`)
  - `Task<TransferConfiguration> AddFileTransferBaseConfigurationAsync(TransferConfigurationUpsertRequest request, CancellationToken ct)`
  - `Task<TransferConfiguration> UpdateFileTransferBaseConfigurationAsync(Guid sourceLocationId, TransferConfigurationUpsertRequest request, CancellationToken ct)`
  - `Task InvalidateFileTransferBaseConfigurationAsync(Guid sourceLocationId, CancellationToken ct)`
  - `Task<FtpServer> AddFtpServerAsync(FtpServerUpsertRequest request, CancellationToken ct)`
  - `Task<FtpServer> UpdateFtpServerAsync(Guid ftpServerId, FtpServerUpsertRequest request, CancellationToken ct)`
  - `Task<FtpServer> InvalidateFtpServerAsync(Guid ftpServerId, FtpServerPatchRequest request, CancellationToken ct)`

- `FileTransferQueryProvider` (implements `IFileTransferQueryProvider`)
  - `Task<PagedResult<TransferConfigurationSummary>> SearchFileTransferAsync(TransferConfigurationSearchParams search, CancellationToken ct)`
  - `Task<TransferConfiguration?> GetFileTransferConfigurationInformationAsync(Guid sourceLocationId, CancellationToken ct)`
  - `Task<IReadOnlyList<FtpServerOption>> GetFtpServersOptionsAsync(bool isValid, CancellationToken ct)`
  - `Task<PagedResult<FtpServer>> GetFtpServersAsync(FtpServerSearchParams search, bool removePassword, CancellationToken ct)`
  - `Task<IReadOnlyList<InboundTargetMetadata>> GetMetadataIdsAsync(string? contractName, CancellationToken ct)`
  - `Task<IReadOnlyList<SourceLocationOption>> GetSourceLocationsAsync(bool isActive, CancellationToken ct)`
  - `Task<PagedResult<FileTransferTracking>> SearchFileTransferTrackingAsync(FileTrackingSearchParams search, CancellationToken ct)`
  - `Task<FileTransferTracking?> GetFileTransferTrackingDetailAsync(Guid fileId, CancellationToken ct)`

Target-local helper types in the same folder (minimal, only to avoid untyped parameter bags):

- `TransferConfigurationSearchParams`, `FtpServerSearchParams`, `FileTrackingSearchParams`.

### Legacy-parity mapping rules (apply in provider implementations)

- Phone normalization: when mapping destination contact phone, remove `-` characters when the phone is non-null.
- FileToEvent override: when destination type is `FileToEvent`, overwrite the destination address with the configured `ExternalFTECommonFolder` setting.
- FTP server id mapping: when a destination FTP server id input is empty, send `Guid.Empty` downstream.
- Run-on-this-day mapping: when the UI/contract value is missing, use byte `0`.
- Suspended changed date: when configuration is marked suspended, set `isSuspendedChangedDateUtc = UtcNow`; otherwise send null.
- Update FTP server password: when updating and no new password is provided, fetch existing credentials with `removePassword=false` and reuse the first returned password; if the query returns OK with an empty list, allow the resulting unhandled exception (legacy parity).

## Dependencies

- `FxferDomainApiClient` (typed `HttpClient`) for all downstream `fxfer-domain-api` calls.
- `DataListsClient` for reference-data lookups needed to translate owner subsystem ids to display text when required by the UI.
- `IConfiguration` / options binding for `ExternalFTECommonFolder`.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `FileTransferServiceDataProvider.AddFileTransferBaseConfiguration` | `FileTransferCommandProvider.AddFileTransferBaseConfigurationAsync` | Calls `fxfer-domain-api` create configuration; applies destination mapping rules (F01-027..031). |
| `FileTransferServiceDataProvider.UpdateFileTransferBaseConfiguration` | `FileTransferCommandProvider.UpdateFileTransferBaseConfigurationAsync` | Calls `fxfer-domain-api` update configuration; applies destination mapping rules (F01-027..031). |
| `FileTransferServiceDataProvider.InvalidateFileTransferBaseConfiguration` | `FileTransferCommandProvider.InvalidateFileTransferBaseConfigurationAsync` | In target, invalidation is an explicit domain operation by `sourceLocationId` (destination list handling is downstream). |
| `FileTransferServiceDataProvider.AddFtpServer(RequestModel<FtpServerModel>)` | `FileTransferCommandProvider.AddFtpServerAsync` | Builds downstream create request; preserves legacy semantics for id generation and operator id (F02-051). |
| `FileTransferServiceDataProvider.UpdateFtpServer(RequestModel<FtpServerModel>)` | `FileTransferCommandProvider.UpdateFtpServerAsync` | Reuses password when omitted (F02-056) and preserves the unhandled empty-list failure mode (F02-063). |
| `FileTransferServiceDataProvider.InvalidateFtpServer(RequestModel<FtpServerModel>)` | `FileTransferCommandProvider.InvalidateFtpServerAsync` | Parses id as GUID and maps request context (F02-060). |
| `FileTransferServiceDataProvider.RunAddFtpServer` | `merged into FileTransferCommandProvider.AddFtpServerAsync` | WCF proxy seam replaced by `FxferDomainApiClient` call. |
| `FileTransferServiceDataProvider.RunUpdateFtpServer` | `merged into FileTransferCommandProvider.UpdateFtpServerAsync` | WCF proxy seam replaced by `FxferDomainApiClient` call. |
| `FileTransferServiceDataProvider.RunInvalidateFtpServer` | `merged into FileTransferCommandProvider.InvalidateFtpServerAsync` | WCF proxy seam replaced by `FxferDomainApiClient` call. |
| `FileTransferServiceDataProvider.HandleServiceException<T>` | `drop: centralized Problem Details mapping` | BFF error middleware maps downstream failures into RFC 7807 Problem Details. |
| `FileTransferServiceDataProvider.HandleBusinessException<T>` | `drop: centralized Problem Details mapping` | Same as above. |
| `FileTransferServiceDataProvider.BuildMessage` | `drop: centralized Problem Details mapping` | Legacy UI `ResultModel` message formatting is replaced by standard Problem Details. |
| `FileTransferServiceDataProvider.BuildErrorMessage` | `drop: centralized Problem Details mapping` | Legacy message formatting is not preserved as a distinct helper. |
| `FileTransferServiceDataProvider.HandleException<T>` | `drop: centralized Problem Details mapping` | Same as above. |
| `FileTransferServiceDataProvider.GenerateReponse<T>` | `drop: target uses contract DTOs` | Target does not use `ResponseModel<T>` wrappers. |
| `FileTransferServiceDataProvider.MapToServiceRequestor` | `drop: request context is propagated via headers` | Target uses middleware-derived tenant/user/correlation context. |
| `FileTransferServiceDataProvider.MapToInternalProvider` | `merged into FileTransferCommandProvider.AddFileTransferBaseConfigurationAsync` | Target maps domain response DTO into `TransferConfiguration` contract DTO. |
| `FileTransferServiceDataProvider.AddFtpServer(RequestModel, ResponseModel)` | `merged into FileTransferCommandProvider.AddFtpServerAsync` | Response building becomes direct return of `FtpServer` contract DTO. |
| `FileTransferServiceDataProvider.UpdateFtpServer(RequestModel, ResponseModel)` | `merged into FileTransferCommandProvider.UpdateFtpServerAsync` | Response building becomes direct return of `FtpServer` contract DTO. |
| `FileTransferServiceDataProvider.InvalidateFtpServer(RequestModel, ResponseModel)` | `merged into FileTransferCommandProvider.InvalidateFtpServerAsync` | Response building becomes direct return of `FtpServer` contract DTO. |
| `FileTransferServiceDataProvider.BuildFtpServerModel` | `drop: target uses contract DTOs` | No legacy `ResponseModel` wrapper. |
| `FileTransferServiceDataProvider.BuildAddFtpServerCommand` | `merged into FileTransferCommandProvider.AddFtpServerAsync` | Command object replaced by downstream HTTP request DTO. |
| `FileTransferServiceDataProvider.BuildUpdateFtpServerCommand` | `merged into FileTransferCommandProvider.UpdateFtpServerAsync` | Command object replaced by downstream HTTP request DTO. |
| `FileTransferServiceDataProvider.GetFTPServerPass` | `FileTransferCommandProvider.GetExistingFtpServerPasswordAsync` | Target fetches existing password with `removePassword=false` (F02-056/F02-063 parity). |
| `FileTransferServiceDataProvider.BuildRequest` | `merged into FileTransferCommandProvider.GetExistingFtpServerPasswordAsync` | Request DTO building is internal to the downstream call. |
| `FileTransferServiceDataProvider.BuildCriteria` | `merged into FileTransferCommandProvider.GetExistingFtpServerPasswordAsync` | Search criteria replaced by explicit query parameters. |
| `FileTransferServiceDataProvider.BuildInvalidateFtpServerCommand` | `merged into FileTransferCommandProvider.InvalidateFtpServerAsync` | Command object replaced by downstream HTTP request DTO. |
| `FileTransferServiceDataProvider.HandleFtpServerBusinessExceptions` | `drop: centralized Problem Details mapping` | Domain API validation errors are surfaced via Problem Details. |
| `FileTransferServiceDataProvider.GetErrorMessage` | `drop: centralized Problem Details mapping` | Legacy localized message lookup is replaced by standard error payloads. |
| `FileTransferServiceDataProvider.GetValidationMessage` | `drop: centralized Problem Details mapping` | Same as above. |
| `FileTransferQueryServiceDataProvider.SearchFileTransferTracking` | `FileTransferQueryProvider.SearchFileTransferTrackingAsync` | Calls `fxfer-domain-api` tracking search; applies supported-filter + parse-only behavior (F05-035). |
| `FileTransferQueryServiceDataProvider.GetFileTransferTrackingDetail` | `FileTransferQueryProvider.GetFileTransferTrackingDetailAsync` | Calls `fxfer-domain-api` tracking detail by file id; returns null when not found. |
| `FileTransferQueryServiceDataProvider.BuildWhereQueryParms` | `FileTransferQueryProvider.BuildWhereQueryParms` | Target-local helper to map supported tracking filters; unsupported ignored; parse-only date/size semantics. |
| `FileTransferQueryServiceDataProvider.MapToInternalFileTransferTrackingResultModel` | `merged into FileTransferQueryProvider.SearchFileTransferTrackingAsync` | Target maps downstream tracking DTOs into `FileTransferTracking` contract DTOs. |
| `FileTransferQueryServiceDataProvider.MapToInternalFileTransferTrackingDetailModel` | `merged into FileTransferQueryProvider.GetFileTransferTrackingDetailAsync` | Target builds the contract detail model directly from downstream DTO. |
| `FileTransferQueryServiceDataProvider.MapToInternalFileTransferTrackingInformationModel` | `merged into FileTransferQueryProvider.GetFileTransferTrackingDetailAsync` | Same as above. |
| `FileTransferQueryServiceDataProvider.MapToInternalFileTransferTrackingEvents` | `merged into FileTransferQueryProvider.GetFileTransferTrackingDetailAsync` | Same as above. |
| `FileTransferQueryServiceDataProvider.MapToInternalFileTransferTrackingExceptions` | `merged into FileTransferQueryProvider.GetFileTransferTrackingDetailAsync` | Same as above. |
| `FileTransferQueryServiceDataProvider.BuildServiceSortCriteria` | `merged into FileTransferQueryProvider.SearchFileTransferTrackingAsync` | Target maps `sort` parameters per BFF contract instead of WCF sort criteria objects. |
| `FileTransferQueryServiceDataProvider.BuildServiceSortOrder` | `drop: target uses query parameters` | No WCF enum translation. |
| `FileTransferQueryServiceDataProvider.BuildServicePagingCriteria` | `drop: target uses query parameters` | Pagination uses `page`/`pageSize` per BFF contract. |
| `FileTransferQueryServiceDataProvider.BuildServicePageSizeCriteria` | `drop: target uses query parameters` | Same as above. |
| `FileTransferQueryServiceDataProvider.SearchFileTransfer` | `FileTransferQueryProvider.SearchFileTransferAsync` | Calls `fxfer-domain-api` configuration search; maps to `PagedResult<TransferConfigurationSummary>`. |
| `FileTransferQueryServiceDataProvider.GetFileTransferConfigurationInformation` | `FileTransferQueryProvider.GetFileTransferConfigurationInformationAsync` | Calls `fxfer-domain-api` configuration detail by `sourceLocationId`. |
| `FileTransferQueryServiceDataProvider.GetFtpServers` | `FileTransferQueryProvider.GetFtpServersOptionsAsync` | Uses the `/ftp-servers/options` contract in the BFF surface. |
| `FileTransferQueryServiceDataProvider.GetFTPServerCredentials` | `FileTransferQueryProvider.GetFtpServersAsync` | Uses `/ftp-servers` list/search with `removePassword` control. |
| `FileTransferQueryServiceDataProvider.GetMetadataIds` | `FileTransferQueryProvider.GetMetadataIdsAsync` | Uses BFF `/inbound-target-metadata` lookup. |
| `FileTransferQueryServiceDataProvider.GetServiceQuery` | `drop: explicit query parameters` | Contract name is passed as a query parameter; no query object builder. |
| `FileTransferQueryServiceDataProvider.GetSourceLocations` | `FileTransferQueryProvider.GetSourceLocationsAsync` | Uses BFF `/source-locations` options endpoint. |
| `FileTransferQueryServiceDataProvider.RunService` | `drop: direct HttpClient call` | WCF seam replaced by `FxferDomainApiClient`. |
| `FileTransferQueryServiceDataProvider.MapToInternalFileTransferModel` | `merged into FileTransferQueryProvider.SearchFileTransferAsync` | Target maps downstream search results to `TransferConfigurationSummary`. |
| `FileTransferQueryServiceDataProvider.MapToInternalFileTransferConfigurationModel` | `merged into FileTransferQueryProvider.GetFileTransferConfigurationInformationAsync` | Target maps downstream detail to `TransferConfiguration`. |
| `FileTransferQueryServiceDataProvider.MapToInternalFtpServerModel` | `merged into FileTransferQueryProvider.GetFtpServersOptionsAsync` | Target maps downstream DTOs to `FtpServerOption`. |
| `FileTransferQueryServiceDataProvider.MapToInternalInboundTarget` | `merged into FileTransferQueryProvider.GetMetadataIdsAsync` | Target maps to `InboundTargetMetadata`. |
| `FileTransferQueryServiceDataProvider.MapToInternalSourceLocationModel` | `merged into FileTransferQueryProvider.GetSourceLocationsAsync` | Target maps to `SourceLocationOption`. |
| `FileTransferQueryServiceDataProvider.GetModuleName` | `FileTransferQueryProvider.GetModuleNameAsync` | Replaces reference-data OData query with `DataListsClient` lookup. |
| `FileTransferQueryServiceDataProvider.MapToServiceRequestor` | `drop: request context is propagated via headers` | Target uses middleware-derived tenant/user/correlation context. |
| `FileTransferQueryServiceDataProvider.MapToInternalDestinationModel` | `merged into FileTransferQueryProvider.GetFileTransferConfigurationInformationAsync` | Target maps downstream destination DTOs into `Destination` contract DTOs. |
| `FileTransferQueryServiceDataProvider.BuildSourceLocationQueryParms` | `merged into FileTransferQueryProvider.SearchFileTransferAsync` | Search parameters become explicit query parameters. |
| `FileTransferQueryServiceDataProvider.BuildSourceLocationSortParms` | `merged into FileTransferQueryProvider.SearchFileTransferAsync` | Sort parameters become explicit query parameters. |
| `FileTransferQueryServiceDataProvider.GetFTPServerName` | `drop: target contracts carry names` | Target option contracts include server name; no in-provider id-to-name cache needed. |
| `FileTransferQueryServiceDataProvider.BuildResponseModel<FtpServerModel>` | `drop: target uses contract DTOs` | No legacy `ResponseModel` wrapper. |
| `FileTransferQueryServiceDataProvider.GetFTPServersResponse` | `merged into FileTransferQueryProvider.GetFtpServersAsync` | Target calls downstream list/search and maps directly. |
| `FileTransferQueryServiceDataProvider.BuildQuery` | `drop: explicit query parameters` | Target uses explicit query parameters per BFF contract. |
| `FileTransferQueryServiceDataProvider.BuildSearchOptions` | `drop: explicit match-mode parameters` | Match-mode strings (`contains`/`startsWith`/`endsWith`/`exact`) are mapped at the boundary. |
| `FileTransferQueryServiceDataProvider.GetSearchType` | `drop: explicit match-mode parameters` | Same as above. |
| `FileTransferQueryServiceDataProvider.SetFTPServersPaging` | `drop: explicit query parameters` | Pagination handled by `page`/`pageSize`. |
| `FileTransferQueryServiceDataProvider.SetSortCriteria` | `drop: explicit query parameters` | Sort handled by explicit parameters. |
| `FileTransferQueryServiceDataProvider.PopulateResponseResults` | `drop: target uses PagedResult<T>` | Response shaping is `PagedResult<T>` with `items/total/page/pageSize`. |
| `FileTransferQueryServiceDataProvider.MapResults` | `merged into FileTransferQueryProvider.GetFtpServersAsync` | Mapping performed inline. |
| `FileTransferQueryServiceDataProvider.MapFTPServerModel` | `merged into FileTransferQueryProvider.GetFtpServersAsync` | Mapping performed inline. |
| `FileTransferQueryServiceDataProvider.HandleFaultException` | `drop: centralized Problem Details mapping` | Downstream failures are mapped to Problem Details. |
| `FileTransferQueryServiceDataProvider.HandleException` | `drop: centralized Problem Details mapping` | Downstream failures are mapped to Problem Details. |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-001-backend-for-frontend-pattern.md` section `Executive Summary > Solution Overview` (BFF shapes UI contracts and delegates authoritative state changes to domain services)
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (RFC 7807 Problem Details error contract and correlation support)
- `patterns/application/api-microservices/PTN-APP-API-016-pagination-pattern.md` section `Solution Overview` (standard `page`/`pageSize` pagination envelopes)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Executive Summary > Solution Overview` (correlation id generation/propagation at boundaries and to downstream calls)

