---
type: Target Placement
title: technical/components/backend-data-access-batch-data-access-2/TARGET
description: Where the backend-data-access-batch-data-access-2 component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-batch-data-access-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:55:48Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-batch-data-access-2/INVENTORY.yaml
  title: Every legacy class and method (16 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-batch-data-access-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 16
  methods: 47
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Infrastructure/DataAccess
  body_sha256: de9e6e406647b82d8c35394349a8ee9aad5d5413bd3734bdc69c22d57e92a7d9
---

﻿# Target: backend-data-access-batch-data-access-2

## Where

Deployable: fxfer-transfer-worker

Target folder: workers/fxfer-transfer-worker/src/Infrastructure/DataAccess

## What to create

Create these types in workers/fxfer-transfer-worker/src/Infrastructure/DataAccess. These are internal worker data-access adapters; they do not access tenant databases directly.

- Interface IDataAccess<TOutput>
  - Task<TOutput> ExecuteAsync(CancellationToken cancellationToken)
  - Purpose: common execution shape for worker data-access operations.

- Abstract class DataAccessBase<TOutput> : IDataAccess<TOutput>
  - Constructor accepts IFxferDomainApiClient and IRequestContextAccessor (correlation_id, trace context, tenant context).
  - ExecuteAsync delegates to ExecuteQueryAsync with domain API client and request context.
  - Protected abstract Task<TOutput> ExecuteQueryAsync(CancellationToken cancellationToken).
  - Note: legacy CreateDbSession/CreateDbContext are not carried over (no direct DB access).

- Interface IDataAccessFactory
  - Factory methods returning IDataAccess<T> for each legacy operation (pending list, source locations, tracking writes, notifications).

- Class DataAccessFactory : IDataAccessFactory
  - Creates instances of each concrete DataAccess class.

- Class CreateErrorLogDataAccess : DataAccessBase<int>
  - Constructor input: ErrorLogDetailModel.
  - ExecuteQueryAsync: calls fxfer-domain-api to record a file exception for the given fileId.
  - String truncation: uses the legacy entity max lengths (ExceptionType 2000, Message 2000, MethodName 2000, StackTrace 4000) before sending.

- Class CreateFileManagementDataAccess : DataAccessBase<FileManagementResponseModel>
  - Constructor input: FileManagementRequestModel.
  - ExecuteQueryAsync: calls fxfer-domain-api to create a tracking header for the file and returns the created fileId.

- Class CreateFtsFileDeliveryDataAccess : DataAccessBase<Guid>
  - Constructor input: fileId, destinationId, destinationFileName, operatorId.
  - ExecuteQueryAsync: calls fxfer-domain-api to record a delivery for the fileId/destinationId.
  - Event behavior parity:
    - If destinationFileName differs from the tracked fileName, records a Renamed event.
    - Records either Archieved or Transferred based on the destination configuration DestIsArchive.

- Class FtsSuspensionDataAccess : DataAccessBase<FtsSuspensionModel>
  - Constructor input: FtsSuspensionSearchCriteria.
  - ExecuteQueryAsync: calls fxfer-domain-api to set the source location to suspended and record the suspension diagnostic.

- Class GetConfigMetaDataDataAccess : DataAccessBase<SourceLocationModel>
  - Constructor input: fileId.
  - ExecuteQueryAsync:
    - Calls GET /api/fxfer-domain/v1/file-tracking/{fileId} to obtain fileSourceLocationId and current fileName.
    - Calls GET /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId} to obtain the configuration including destinations.
    - Filters destinations using the legacy rule: exclude destinations already delivered for this fileId, except destinations that are archive destinations.
      - Destination delivery presence is inferred from file-tracking events that include destinationId.
    - For protocol types that require credentials, resolves credentialSecretRef using AWS Secrets Manager and populates the in-process model used by the worker.

- Class GetPendingFileListDataAccess : DataAccessBase<List<PendingFileModel>>
  - ExecuteQueryAsync:
    - Calls GET /api/fxfer-domain/v1/file-tracking (paged) and projects to PendingFileModel.
    - Applies the legacy filter:
      - include all files whose fileStatus is not Completed
      - also include files whose fileStatus is Completed and createdDate is within the last 1 minute
    - StatusDate in PendingFileModel is mapped from createdDate.

- Class GetSourceLocationsDataAccess : DataAccessBase<List<SourceLocationModel>>
  - ExecuteQueryAsync:
    - Calls GET /api/fxfer-domain/v1/transfer-configurations (paged) and then GET /transfer-configurations/{sourceLocationId} as needed to obtain full configuration detail.
    - Maps schedule fields to the legacy in-process SourceLocationModel shape used by the worker.
    - For non-UNC protocols, resolves credentialSecretRef via AWS Secrets Manager and populates the in-process model used by the worker.

- Class QueryFtsNotificationDataAccess : DataAccessBase<List<FtsNotificationModel>>
  - Constructor input: GetNotificationSearchCriteria.
  - ExecuteQueryAsync:
    - Calls fxfer-domain-api to obtain the notification recipients for the configuration id.
    - Applies legacy selection:
      - when both NotifyOnErrorInd and NotifyOnSuccessInd are true: recipients where both are true
      - when only NotifyOnErrorInd is true: recipients where NotifyOnErrorInd is true
      - else: recipients where NotifyOnSuccessInd is true

- Class UpdateFileManagementStatusDataAccess : DataAccessBase<Guid>
  - Constructor input: fileId, fileStatus.
  - ExecuteQueryAsync: calls fxfer-domain-api to update the tracking status for the file id.
  - Timestamps: the domain API remains authoritative for status-change timestamps.

- Class UpdateSourceSuspendStatusDataAccess : DataAccessBase<bool>
  - Constructor input: sourceLocationId.
  - ExecuteQueryAsync: calls fxfer-domain-api to clear suspended state and returns whether the operation changed state.

Also carry over these constants for in-process parity:

- DataAccessConstants.FileStatusConstant
  - Completed = 1
  - Error = 2
  - Transfering = 3
- ProtocolCodeConstant
  - FTP = 1
  - SFTP = 2
  - UNC = 3
  - FTPS = 4

## Mapping

| Legacy | Target | Note |
|---|---|---|
| CreateErrorLogDataAccess.ExecuteQuery(IDbContextBase) | CreateErrorLogDataAccess.ExecuteQueryAsync | Records a file exception via fxfer-domain-api |
| CreateErrorLogDataAccess.GetMaxLength(string) | CreateErrorLogDataAccess.GetMaxLength | Uses constant max lengths (no EF reflection) |
| CreateErrorLogDataAccess.SetLimitedValue() | CreateErrorLogDataAccess.SetLimitedValue | Truncates fields before sending |
| CreateErrorLogDataAccess.Truncate(string, int) | CreateErrorLogDataAccess.Truncate | Same truncation rules |
| CreateFileManagementDataAccess.CreateFileManagementEntityData(IDbContextBase) | CreateFileManagementDataAccess.BuildCreateRequest | Builds the domain API create-tracking request |
| CreateFileManagementDataAccess.ExecuteQuery(IDbContextBase) | CreateFileManagementDataAccess.ExecuteQueryAsync | Creates tracking header via fxfer-domain-api |
| CreateFtsFileDeliveryDataAccess.AddDestinationFileRenamedEvent(IDbContextBase) | CreateFtsFileDeliveryDataAccess.ExecuteQueryAsync | merged: delivery command encapsulates event recording |
| CreateFtsFileDeliveryDataAccess.AddFileArchievedOrTransferredEvent(IDbContextBase) | CreateFtsFileDeliveryDataAccess.ExecuteQueryAsync | merged: delivery command encapsulates event recording |
| CreateFtsFileDeliveryDataAccess.ExecuteQuery(IDbContextBase) | CreateFtsFileDeliveryDataAccess.ExecuteQueryAsync | Records delivery outcome via fxfer-domain-api |
| CreateFtsFileDeliveryDataAccess.UpdateFileEventData(ref IDbContextBase, ref FtsFileEvent, string) | CreateFtsFileDeliveryDataAccess.ExecuteQueryAsync | merged |
| DataAccessBase<TdacOutput>.CreateDbContext(IDbSession) | drop: DataAccessBase<TOutput>.CreateDbContext | No direct DB access in target |
| DataAccessBase<TdacOutput>.CreateDbSession() | drop: DataAccessBase<TOutput>.CreateDbSession | No direct DB access in target |
| DataAccessBase<TdacOutput>.Execute() | DataAccessBase<TOutput>.ExecuteAsync | Async execution shape in worker |
| DataAccessBase<TdacOutput>.ExecuteQuery(IDbContextBase) | DataAccessBase<TOutput>.ExecuteQueryAsync | Async abstract query method |
| DataAccessFactory.CreateErrorLogDataAccess(ErrorLogDetailModel) | DataAccessFactory.CreateErrorLogDataAccess | Returns CreateErrorLogDataAccess |
| DataAccessFactory.CreateFileManagementDataAccess(FileManagementRequestModel) | DataAccessFactory.CreateFileManagementDataAccess | Returns CreateFileManagementDataAccess |
| DataAccessFactory.CreateFtsFileDeliveryDataAccess(Guid, Guid, string, string) | DataAccessFactory.CreateFtsFileDeliveryDataAccess | Returns CreateFtsFileDeliveryDataAccess |
| DataAccessFactory.CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria) | DataAccessFactory.CreateFtsSuspensionDataAccess | Returns FtsSuspensionDataAccess |
| DataAccessFactory.CreateGetConfigMetaDataDataAccess(Guid) | DataAccessFactory.CreateGetConfigMetaDataDataAccess | Returns GetConfigMetaDataDataAccess |
| DataAccessFactory.CreateGetPendingFileListDataAccess() | DataAccessFactory.CreateGetPendingFileListDataAccess | Returns GetPendingFileListDataAccess |
| DataAccessFactory.CreateGetSourceLocationsDataAccess() | DataAccessFactory.CreateGetSourceLocationsDataAccess | Returns GetSourceLocationsDataAccess |
| DataAccessFactory.CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria) | DataAccessFactory.CreateQueryFtsNotificationDataAccess | Returns QueryFtsNotificationDataAccess |
| DataAccessFactory.CreateUpdateFileManagementStatusDataAccess(Guid, string) | DataAccessFactory.CreateUpdateFileManagementStatusDataAccess | Returns UpdateFileManagementStatusDataAccess |
| DataAccessFactory.CreateUpdateSourceSuspendStatusDataAccess(Guid) | DataAccessFactory.CreateUpdateSourceSuspendStatusDataAccess | Returns UpdateSourceSuspendStatusDataAccess |
| FtsSuspensionDataAccess.AddEntitySourceLocation(IDbContextBase, FtsSuspension) | FtsSuspensionDataAccess.ExecuteQueryAsync | merged into domain API command |
| FtsSuspensionDataAccess.CreateSourceLocationEntityData() | FtsSuspensionDataAccess.ExecuteQueryAsync | merged into domain API command |
| FtsSuspensionDataAccess.ExecuteQuery(IDbContextBase) | FtsSuspensionDataAccess.ExecuteQueryAsync | Uses fxfer-domain-api suspension command |
| FtsSuspensionDataAccess.GetSourceLocation(IDbContextBase) | FtsSuspensionDataAccess.ExecuteQueryAsync | merged |
| FtsSuspensionDataAccess.UpdateSourceLocation(IDbContextBase, FtsSourceLocation) | FtsSuspensionDataAccess.ExecuteQueryAsync | merged |
| GetConfigMetaDataDataAccess.ExecuteQuery(IDbContextBase) | GetConfigMetaDataDataAccess.ExecuteQueryAsync | Uses file-tracking + transfer-configuration queries |
| GetConfigMetaDataDataAccess.GetDestinations(IDbContextBase) | GetConfigMetaDataDataAccess.ExecuteQueryAsync | merged: destination filtering uses domain API response + tracking events |
| GetConfigMetaDataDataAccess.SetSoureceLocationId(IDbContextBase) | GetConfigMetaDataDataAccess.ExecuteQueryAsync | merged: sourceLocationId comes from file-tracking |
| GetPendingFileListDataAccess.ExecuteQuery(IDbContextBase) | GetPendingFileListDataAccess.ExecuteQueryAsync | Uses file-tracking search and client-side filter |
| GetSourceLocationsDataAccess.ExecuteQuery(IDbContextBase) | GetSourceLocationsDataAccess.ExecuteQueryAsync | Uses transfer-configurations queries |
| GetSourceLocationsDataAccess.GetSourceLocationData(FtsSourceLocation, FtpServer) | GetSourceLocationsDataAccess.GetSourceLocationData | Retained mapping helper |
| GetSourceLocationsDataAccess.GetSourceLocationModel(FtsSourceLocation) | GetSourceLocationsDataAccess.GetSourceLocationModel | Retained mapping helper |
| GetSourceLocationsDataAccess.UpdateFtpServerData(ref SourceLocationModel, FtpServer, string) | GetSourceLocationsDataAccess.UpdateFtpServerData | In target, resolves credentialSecretRef via Secrets Manager |
| GetSourceLocationsDataAccess.UpdateMonthlyScheduleData(ref SourceLocationModel, FtsSourceLocation) | GetSourceLocationsDataAccess.UpdateMonthlyScheduleData | Retained schedule-mapping rules |
| GetSourceLocationsDataAccess.UpdateScheduleEndTime(ref SourceLocationModel, FtsSourceLocation) | GetSourceLocationsDataAccess.UpdateScheduleEndTime | Retained schedule-mapping rules |
| GetSourceLocationsDataAccess.UpdateScheduleStartTime(ref SourceLocationModel, FtsSourceLocation) | GetSourceLocationsDataAccess.UpdateScheduleStartTime | Retained schedule-mapping rules |
| GetSourceLocationsDataAccess.UpdateWeeklyScheduleData(ref SourceLocationModel, FtsSourceLocation) | GetSourceLocationsDataAccess.UpdateWeeklyScheduleData | Retained schedule-mapping rules |
| QueryFtsNotificationDataAccess.ExecuteQuery(IDbContextBase) | QueryFtsNotificationDataAccess.ExecuteQueryAsync | Reads notification recipients via fxfer-domain-api |
| QueryFtsNotificationDataAccess.NotifyErrorInd(IDbContextBase) | QueryFtsNotificationDataAccess.ExecuteQueryAsync | merged: selection done in ExecuteQueryAsync |
| QueryFtsNotificationDataAccess.NotifyErrorSuccess(IDbContextBase) | QueryFtsNotificationDataAccess.ExecuteQueryAsync | merged: selection done in ExecuteQueryAsync |
| QueryFtsNotificationDataAccess.NotifyOnSuccessInd(IDbContextBase) | QueryFtsNotificationDataAccess.ExecuteQueryAsync | merged: selection done in ExecuteQueryAsync |
| UpdateFileManagementStatusDataAccess.ExecuteQuery(IDbContextBase) | UpdateFileManagementStatusDataAccess.ExecuteQueryAsync | Updates status via fxfer-domain-api |
| UpdateSourceSuspendStatusDataAccess.ExecuteQuery(IDbContextBase) | UpdateSourceSuspendStatusDataAccess.ExecuteQueryAsync | Clears suspension via fxfer-domain-api |

## Enterprise-Architecture standards applied

- patterns/application/api-microservices/PTN-APP-API-007-database-per-service-pattern.md section Solution Overview (worker must not directly access a shared database; use service APIs)
- standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md section 2.1 Data Protection Requirements (tenant isolation; audit trail posture)
- patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md section Solution Overview (generate/extract at entry points and propagate)
- patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md section Correlation ID Format Standards (X-Correlation-ID UUID v4; tenant-prefixed format when required)
- strategy/white-papers/operations/enterprise-kubernetes-strategy.md section 2.3.4 Secrets Management (store credentials in AWS Secrets Manager; audited access and rotation)


