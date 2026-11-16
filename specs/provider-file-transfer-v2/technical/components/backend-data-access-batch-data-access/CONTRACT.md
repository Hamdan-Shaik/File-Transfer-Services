---
type: Interface Contract
title: 'Contract: backend-data-access-batch-data-access'
description: What the backend-data-access-batch-data-access component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-batch-data-access
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:40:28Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-batch-data-access/INVENTORY.yaml
  title: Every legacy class and method (16 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-batch-data-access
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
  body_sha256: 085930250d8c7fa8119697088160552d71c5d875de6399671362ed8a9e31d478
---

# Contract: backend-data-access-batch-data-access

This component is internal to the fxfer-transfer-worker deployable. It exposes in-process data-access operations used by batch execution logic.

## Offers

### In-process interfaces

- IDataAccess<TOutput>
  - ExecuteAsync(CancellationToken)

- IDataAccessFactory
  - CreateGetPendingFileListDataAccess(): IDataAccess<List<PendingFileModel>>
  - CreateGetSourceLocationsDataAccess(): IDataAccess<List<SourceLocationModel>>
  - CreateGetConfigMetaDataDataAccess(Guid fileId): IDataAccess<SourceLocationModel>
  - CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria): IDataAccess<List<FtsNotificationModel>>
  - CreateFileManagementDataAccess(FileManagementRequestModel): IDataAccess<FileManagementResponseModel>
  - CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus): IDataAccess<Guid>
  - CreateFtsFileDeliveryDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId): IDataAccess<Guid>
  - CreateErrorLogDataAccess(ErrorLogDetailModel): IDataAccess<int>
  - CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria): IDataAccess<FtsSuspensionModel>
  - CreateUpdateSourceSuspendStatusDataAccess(Guid sourceLocationId): IDataAccess<bool>

### In-process models

This component uses the in-process contract models already present in the worker deployable (for example PendingFileModel, SourceLocationModel, DestinationModel, FtsNotificationModel, FileManagementRequestModel, ErrorLogDetailModel).

## Uses

### FXFER Domain API (private HTTP)

This component calls fxfer-domain-api using tenant-scoped service identity, propagating X-Correlation-ID and W3C trace context.

- GET /api/fxfer-domain/v1/transfer-configurations
- GET /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}
- GET /api/fxfer-domain/v1/file-tracking
- GET /api/fxfer-domain/v1/file-tracking/{fileId}

The following domain-write operations are required by this component's legacy parity behavior (implemented as domain API commands in the target):

- Record file tracking header (create fileId)
- Update file tracking status
- Record a delivery outcome and related events (Renamed, Archieved, Transferred)
- Record a file exception (error log)
- Set suspended state with diagnostic text
- Clear suspended state
- Query notification recipients for a configuration id

### AWS Secrets Manager

This component resolves credentialSecretRef values from domain configuration into runtime credentials for protocol I/O. Secrets are retrieved per tenant and held in memory only for the duration of the work item handling.


