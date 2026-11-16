---
type: Target Placement
title: 'Target: backend-file-transfer-mgmt-data-access'
description: Where the backend-file-transfer-mgmt-data-access component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-mgmt-data-access
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:58:48Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-mgmt-data-access/INVENTORY.yaml
  title: Every legacy class and method (18 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-mgmt-data-access
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 18
  methods: 54
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Infrastructure/DataAccess
  body_sha256: 670dcd27f31a61245902df3231c886a7122999b97394fcaf47d95eb76d28fd92
---

# Target: backend-file-transfer-mgmt-data-access

## Where

Deployable: fxfer-transfer-worker

Target folder: workers/fxfer-transfer-worker/src/Infrastructure/DataAccess

## What to create

Create these types in `workers/fxfer-transfer-worker/src/Infrastructure/DataAccess`. These are internal worker data-access adapters; they do not access tenant databases directly.

- Interface `IDataAccess<TOutput>`
  - `Task<TOutput> ExecuteAsync(CancellationToken cancellationToken)`
  - Purpose: common execution shape for worker data-access operations.

- Abstract class `DataAccessBase<TOutput>` : `IDataAccess<TOutput>`
  - Constructor inputs:
    - `IFxferDomainApiClient domainApiClient`
    - `IRequestContextAccessor requestContextAccessor` (tenant_id, correlation_id, W3C trace context)
  - `ExecuteAsync` delegates to `ExecuteQueryAsync`.
  - Protected abstract `Task<TOutput> ExecuteQueryAsync(CancellationToken cancellationToken)`.
  - Note: legacy `CreateDbSession`/`CreateDbContext` and transaction plumbing are not carried over (no direct DB access).

- Interface `IDataAccessFactory`
  - Factory methods returning `IDataAccess<T>` for each legacy operation.

- Class `DataAccessFactory` : `IDataAccessFactory`
  - Stores `IFxferDomainApiClient` + `IRequestContextAccessor` and creates instances of each concrete data-access class.

- Static class `FileStatusConstant`
  - `Completed = 1`, `Error = 2`, `Transfering = 3`, `UNCProtocolCD = 3`.

- Static class `ProtocolCodeConstant`
  - `FTP = 1`, `SFTP = 2`, `UNC = 3`, `FTPS = 4`.

- Class `CreateFileManagementDataAccess` : `DataAccessBase<FileManagementResponseModel>`
  - Constructor input: `FileManagementRequestModel`.
  - `ExecuteQueryAsync`: calls `fxfer-domain-api` command to create a tracking header for the file and returns the created `fileId`.
  - Parity: sets initial status to `FileStatusConstant.Transfering`.

- Class `UpdateFileManagementStatusDataAccess` : `DataAccessBase<Guid>`
  - Constructor inputs: `fileId`, `fileStatus`.
  - `ExecuteQueryAsync`: calls `fxfer-domain-api` command to update the file status and status timestamp.
  - Parity: no-op when the file id is not found.

- Class `GetPendingFileListDataAccess` : `DataAccessBase<List<PendingFileModel>>`
  - `ExecuteQueryAsync`: calls `fxfer-domain-api` query endpoints and projects to `PendingFileModel`.
  - Parity filter:
    - include files whose `fileStatus` is not `Completed`
    - also include files whose `fileStatus` is `Completed` and `fileStatusTs > (UtcNow - 1 minute)`

- Class `GetConfigMetaDataDataAccess` : `DataAccessBase<SourceLocationModel?>`
  - Constructor input: `fileId`.
  - `ExecuteQueryAsync`:
    - Calls `GET /api/fxfer-domain/v1/file-tracking/{fileId}` to obtain `fileSourceLocationId` and the file’s recorded delivery events.
    - Calls `GET /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}` to obtain the configuration including destinations.
    - Filters destinations using the legacy rule: exclude destinations already delivered for this `fileId`, except archive destinations.
    - Returns null when the file id cannot be resolved to a source location.

- Class `GetSourceLocationsDataAccess` : `DataAccessBase<List<SourceLocationModel>>`
  - `ExecuteQueryAsync`: calls `fxfer-domain-api` configuration query endpoints and maps schedule fields into the in-process `SourceLocationModel` shape used by the worker.
  - Parity: retains weekly/monthly/time-window parsing rules (including the legacy `RunOnThisDayOfMonth` conversion behavior).

- Class `QueryFtsNotificationDataAccess` : `DataAccessBase<List<FtsNotificationModel>>`
  - Constructor input: `GetNotificationSearchCriteria`.
  - `ExecuteQueryAsync`: queries `fxfer-domain-api` for notification recipients for `ConfigId`.
  - Parity selection:
    - when both NotifyOnErrorInd and NotifyOnSuccessInd are true: recipients where both are true
    - when only NotifyOnErrorInd is true: recipients where NotifyOnErrorInd is true
    - else: recipients where NotifyOnSuccessInd is true

- Class `CreateFtsFileDeliveryDataAccess` : `DataAccessBase<Guid>`
  - Constructor inputs: fileId, destinationId, destinationFileName, operatorId, destinations.
  - `ExecuteQueryAsync`: calls `fxfer-domain-api` command endpoints to record a delivery row and event(s).
  - Parity:
    - when the destination config indicates rename, record a `Renamed` event (id not returned)
    - record exactly one of `Archieved` or `Transferred` and return its event id
    - return `Guid.Empty` when no destination config matches destinationId

- Class `CreateFtsFileExceptionEventDataAccess` : `DataAccessBase<Guid>`
  - Constructor inputs: fileId, destinationId, destinationFileName, operatorId, errorLog.
  - `ExecuteQueryAsync`: records an `Exception` event and an exception log entry and returns the created event id.

- Class `CreateErrorLogDataAccess` : `DataAccessBase<int>`
  - Constructor input: `ErrorLogDetailModel`.
  - `ExecuteQueryAsync`: records an exception log entry without creating a tracking event.
  - Parity: truncates ErrorType/ErrorText/MethodName/StackTrace to the legacy max lengths (2000/2000/2000/4000) before sending.

- Class `FtsSuspensionDataAccess` : `DataAccessBase<FtsSuspensionModel>`
  - Constructor input: `FtsSuspensionSearchCriteria`.
  - `ExecuteQueryAsync`: calls `fxfer-domain-api` command endpoint(s) to suspend the source location and record a suspension diagnostic.
  - Parity: truncates StackTrace to 1000 chars.

- Class `UpdateSourceSuspendStatusDataAccess` : `DataAccessBase<bool>`
  - Constructor input: `sourceLocationId` (note: legacy accepted an EF entity reference).
  - `ExecuteQueryAsync`: clears suspended status via `fxfer-domain-api` (no direct DB write).

- Class `GetSuspendedLocationDataAccess` : `DataAccessBase<SourceLocationModel?>`
  - Constructor input: `sourceLocationId`.
  - `ExecuteQueryAsync`: fetches one transfer configuration by id and maps it to `SourceLocationModel`.

## Mapping

| Legacy | Target | Note |
|---|---|---|
| CreateErrorLogDataAccess.ExecuteQuery(IDbContextBase) | CreateErrorLogDataAccess.ExecuteQueryAsync | Records exception log via domain API command |
| CreateErrorLogDataAccess.SetLimitedValue() | CreateErrorLogDataAccess.SetLimitedValue | Retained as helper (truncate to max lengths) |
| CreateErrorLogDataAccess.Truncate(string, int) | CreateErrorLogDataAccess.Truncate | Retained helper |
| CreateErrorLogDataAccess.GetMaxLength(string) | CreateErrorLogDataAccess.GetMaxLength | Retained helper (uses legacy hard-coded lengths: 2000/2000/2000/4000) |
| CreateFileManagementDataAccess.ExecuteQuery(IDbContextBase) | CreateFileManagementDataAccess.ExecuteQueryAsync | Creates tracking header via domain API |
| CreateFileManagementDataAccess.CreateFileManagementEntityData(IDbContextBase) | CreateFileManagementDataAccess.ExecuteQueryAsync | merged: entity construction replaced by domain API request DTO |
| CreateFtsFileDeliveryDataAccess.ExecuteQuery(IDbContextBase) | CreateFtsFileDeliveryDataAccess.ExecuteQueryAsync | Records delivery + event(s) via domain API |
| CreateFtsFileDeliveryDataAccess.AddFileArchievedOrTransferredEvent(IDbContextBase) | CreateFtsFileDeliveryDataAccess.ExecuteQueryAsync | merged: outcome event selection happens client-side; persistence is domain API command |
| CreateFtsFileDeliveryDataAccess.UpdateFileEventData(ref IDbContextBase, ref FtsFileEvent, string) | CreateFtsFileDeliveryDataAccess.ExecuteQueryAsync | merged: event creation is via domain API command |
| CreateFtsFileExceptionEventDataAccess.ExecuteQuery(IDbContextBase) | CreateFtsFileExceptionEventDataAccess.ExecuteQueryAsync | Records exception event + exception log via domain API |
| CreateFtsFileExceptionEventDataAccess.FtsFileExceptionEvent(IDbContextBase) | CreateFtsFileExceptionEventDataAccess.ExecuteQueryAsync | merged |
| CreateFtsFileExceptionEventDataAccess.AddErrorLog(IDbContextBase) | CreateFtsFileExceptionEventDataAccess.ExecuteQueryAsync | merged |
| CreateFtsFileExceptionEventDataAccess.SetLimitedValue() | CreateFtsFileExceptionEventDataAccess.SetLimitedValue | Retained helper |
| CreateFtsFileExceptionEventDataAccess.Truncate(string, int) | CreateFtsFileExceptionEventDataAccess.Truncate | Retained helper |
| CreateFtsFileExceptionEventDataAccess.GetMaxLength(string) | CreateFtsFileExceptionEventDataAccess.GetMaxLength | Retained helper |
| DataAccessBase.Execute() | DataAccessBase.ExecuteAsync | Async execution shape; no DB transaction |
| DataAccessBase.ExecuteWithNoTransaction() | drop: not needed | Target has no DB transaction abstraction |
| DataAccessBase.CreateDbSession() | drop: no direct DB access | Uses `IFxferDomainApiClient` |
| DataAccessBase.CreateDbContext(IDbSession) | drop: no direct DB access | Uses `IFxferDomainApiClient` |
| DataAccessBase.ExecuteQuery(IDbContextBase) | DataAccessBase.ExecuteQueryAsync | Renamed async template method |
| DataAccessFactory.CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria) | DataAccessFactory.CreateFtsSuspensionDataAccess | Returns `FtsSuspensionDataAccess` |
| DataAccessFactory.CreateGetPendingFileListDataAccess() | DataAccessFactory.CreateGetPendingFileListDataAccess | Returns `GetPendingFileListDataAccess` |
| DataAccessFactory.CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria) | DataAccessFactory.CreateQueryFtsNotificationDataAccess | Returns `QueryFtsNotificationDataAccess` |
| DataAccessFactory.CreateGetSourceLocationsDataAccess() | DataAccessFactory.CreateGetSourceLocationsDataAccess | Returns `GetSourceLocationsDataAccess` |
| DataAccessFactory.CreateErrorLogDataAccess(ErrorLogDetailModel) | DataAccessFactory.CreateErrorLogDataAccess | Returns `CreateErrorLogDataAccess` |
| DataAccessFactory.CreateGetConfigMetaDataDataAccess(Guid) | DataAccessFactory.CreateGetConfigMetaDataDataAccess | Returns `GetConfigMetaDataDataAccess` |
| DataAccessFactory.CreateUpdateFileManagementStatusDataAccess(Guid, string) | DataAccessFactory.CreateUpdateFileManagementStatusDataAccess | Returns `UpdateFileManagementStatusDataAccess` |
| DataAccessFactory.CreateUpdateSourceSuspendStatusDataAccess(FtsSourceLocation) | DataAccessFactory.CreateUpdateSourceSuspendStatusDataAccess(Guid) | Signature change: accept `sourceLocationId` instead of EF entity |
| DataAccessFactory.GetSuspendedLocation(Guid) | DataAccessFactory.GetSuspendedLocation | Returns `GetSuspendedLocationDataAccess` (mapped DTO, not EF entity) |
| DataAccessFactory.CreateFtsFileDeliveryDataAccess(Guid,Guid,string,string,List<DestinationModel>) | DataAccessFactory.CreateFtsFileDeliveryDataAccess | Returns `CreateFtsFileDeliveryDataAccess` |
| DataAccessFactory.CreateFileManagementDataAccess(FileManagementRequestModel) | DataAccessFactory.CreateFileManagementDataAccess | Returns `CreateFileManagementDataAccess` |
| DataAccessFactory.CreateFtsFileExceptionEventDataAccess(Guid,Guid,string,string,ErrorLogDetailModel) | DataAccessFactory.CreateFtsFileExceptionEventDataAccess | Returns `CreateFtsFileExceptionEventDataAccess` |
| FtsSuspensionDataAccess.ExecuteQuery(IDbContextBase) | FtsSuspensionDataAccess.ExecuteQueryAsync | Calls domain API command to suspend source location |
| FtsSuspensionDataAccess.CreateSourceLocationEntityData(IDbContextBase) | FtsSuspensionDataAccess.ExecuteQueryAsync | merged |
| FtsSuspensionDataAccess.LimitLength(string, int) | FtsSuspensionDataAccess.LimitLength | Retained helper (max 1000 chars) |
| GetConfigMetaDataDataAccess.ExecuteQuery(IDbContextBase) | GetConfigMetaDataDataAccess.ExecuteQueryAsync | Uses file-tracking + transfer-configuration queries |
| GetConfigMetaDataDataAccess.SetSoureceLocationId(IDbContextBase) | GetConfigMetaDataDataAccess.ExecuteQueryAsync | merged: sourceLocationId comes from file-tracking header |
| GetConfigMetaDataDataAccess.GetDestinations(IDbContextBase) | GetConfigMetaDataDataAccess.ExecuteQueryAsync | merged: destination filtering uses domain API + tracking events |
| GetPendingFileListDataAccess.ExecuteQuery(IDbContextBase) | GetPendingFileListDataAccess.ExecuteQueryAsync | Uses file-tracking search and client-side filter |
| GetSourceLocationsDataAccess.ExecuteQuery(IDbContextBase) | GetSourceLocationsDataAccess.ExecuteQueryAsync | Uses transfer-configuration queries |
| GetSourceLocationsDataAccess.GetSourceLocationData(FtsSourceLocation,FtpServer) | GetSourceLocationsDataAccess.GetSourceLocationData | Retained mapping helper |
| GetSourceLocationsDataAccess.GetSourceLocationModel(FtsSourceLocation) | GetSourceLocationsDataAccess.GetSourceLocationModel | Retained mapping helper |
| GetSourceLocationsDataAccess.UpdateWeeklyScheduleData(ref SourceLocationModel,FtsSourceLocation) | GetSourceLocationsDataAccess.UpdateWeeklyScheduleData | Retained schedule parsing |
| GetSourceLocationsDataAccess.UpdateMonthlyScheduleData(ref SourceLocationModel,FtsSourceLocation) | GetSourceLocationsDataAccess.UpdateMonthlyScheduleData | Retained legacy monthly parsing |
| GetSourceLocationsDataAccess.UpdateScheduleStartTime(ref SourceLocationModel,FtsSourceLocation) | GetSourceLocationsDataAccess.UpdateScheduleStartTime | Retained HHmm parsing |
| GetSourceLocationsDataAccess.UpdateScheduleEndTime(ref SourceLocationModel,FtsSourceLocation) | GetSourceLocationsDataAccess.UpdateScheduleEndTime | Retained HHmm parsing |
| GetSourceLocationsDataAccess.UpdateFtpServerData(ref SourceLocationModel, FtpServer, string) | GetSourceLocationsDataAccess.UpdateFtpServerData | In target, populates worker credential model from domain API secret refs |
| GetSuspendedLocation.ExecuteQuery(IDbContextBase) | GetSuspendedLocationDataAccess.ExecuteQueryAsync | Replaced EF entity fetch with domain API configuration fetch |
| QueryFtsNotificationDataAccess.ExecuteQuery(IDbContextBase) | QueryFtsNotificationDataAccess.ExecuteQueryAsync | Reads notification recipients via domain API |
| QueryFtsNotificationDataAccess.NotifyErrorSuccess(IDbContextBase) | QueryFtsNotificationDataAccess.ExecuteQueryAsync | merged |
| QueryFtsNotificationDataAccess.NotifyErrorInd(IDbContextBase) | QueryFtsNotificationDataAccess.ExecuteQueryAsync | merged |
| QueryFtsNotificationDataAccess.NotifyOnSuccessInd(IDbContextBase) | QueryFtsNotificationDataAccess.ExecuteQueryAsync | merged |
| UpdateFileManagementStatusDataAccess.ExecuteQuery(IDbContextBase) | UpdateFileManagementStatusDataAccess.ExecuteQueryAsync | Updates status via domain API |
| UpdateSourceSuspendStatusDataAccess.ExecuteQuery(IDbContextBase) | UpdateSourceSuspendStatusDataAccess.ExecuteQueryAsync | Clears suspension via domain API |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-007-database-per-service-pattern.md` section `Solution Overview` (worker must not directly access a shared database; use service APIs)
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (RFC 7807 Problem Details for errors returned by domain API)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (domain API is OpenAPI-first; clients generated from contract)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant isolation and tenant-scoped request context)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` UUID v4 propagation)

<!-- END -->
