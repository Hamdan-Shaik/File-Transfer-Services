---
type: Interface Contract
title: 'Contract: backend-file-transfer-mgmt-data-access'
description: What the backend-file-transfer-mgmt-data-access component offers other components and what it uses from them.
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
  body_sha256: 373096223c4e7ab13c6c575b38c8fd4093a5c7a92ee5c58e26a29133610188d3
---

# Contract: backend-file-transfer-mgmt-data-access

This component is internal to the `fxfer-transfer-worker` deployable. It exposes in-process data-access operations used by file-transfer management/batch execution logic.

## Offers

### In-process interfaces

- `IDataAccess<TOutput>`
  - `ExecuteAsync(CancellationToken)`

- `IDataAccessFactory`
  - `CreateGetPendingFileListDataAccess(): IDataAccess<List<PendingFileModel>>`
  - `CreateGetSourceLocationsDataAccess(): IDataAccess<List<SourceLocationModel>>`
  - `CreateGetConfigMetaDataDataAccess(Guid fileId): IDataAccess<SourceLocationModel?>`
  - `CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria): IDataAccess<List<FtsNotificationModel>>`
  - `CreateFileManagementDataAccess(FileManagementRequestModel): IDataAccess<FileManagementResponseModel>`
  - `CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus): IDataAccess<Guid>`
  - `CreateFtsFileDeliveryDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, List<DestinationModel> destinations): IDataAccess<Guid>`
  - `CreateErrorLogDataAccess(ErrorLogDetailModel): IDataAccess<int>`
  - `CreateFtsFileExceptionEventDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, ErrorLogDetailModel): IDataAccess<Guid>`
  - `CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria): IDataAccess<FtsSuspensionModel>`
  - `CreateUpdateSourceSuspendStatusDataAccess(Guid sourceLocationId): IDataAccess<bool>`
  - `GetSuspendedLocation(Guid sourceLocationId): IDataAccess<SourceLocationModel?>`

### In-process models

This component uses the in-process contract models already present in the worker deployable (for example `PendingFileModel`, `SourceLocationModel`, `DestinationModel`, `FtsNotificationModel`, `FileManagementRequestModel`, `ErrorLogDetailModel`).

## Uses

### FXFER Domain API (private HTTP)

This component calls `fxfer-domain-api` using tenant-scoped service identity, propagating `X-Correlation-ID` and W3C trace context.

Read/query endpoints used:

- `GET /api/fxfer-domain/v1/transfer-configurations`
- `GET /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}`
- `GET /api/fxfer-domain/v1/source-locations` (optional optimization for pick lists)
- `GET /api/fxfer-domain/v1/file-tracking`
- `GET /api/fxfer-domain/v1/file-tracking/{fileId}`

Domain-write operations required for parity (implemented as domain API commands in the target):

- Create file tracking header (initial status `Transfering`)
- Update file tracking status + status timestamp
- Record a delivery (destinationFileName) and related events (`Renamed` when configured; exactly one of `Archieved` or `Transferred`)
- Record an exception log (with or without an `Exception` event)
- Set suspended state + record suspension diagnostic text (StackTrace truncated to 1000 chars)
- Clear suspended state
- Query notification recipients for a configuration id

All error responses from `fxfer-domain-api` are treated as RFC 7807 Problem Details (`application/problem+json`) and propagated as failures to the caller.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-007-database-per-service-pattern.md` section `Solution Overview` (worker uses service APIs; no direct DB access)
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (Problem Details error contract)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant isolation and tenant-scoped access)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (UUID v4 correlation id; `X-Correlation-ID` propagation)

