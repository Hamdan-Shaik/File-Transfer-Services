---
type: Interface Contract
title: 'Contract: backend-bas-file-transfer-svc'
description: What the backend-bas-file-transfer-svc component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-bas-file-transfer-svc
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:09:36Z
sources:
- id: inventory
  resource: /technical/components/backend-bas-file-transfer-svc/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-bas-file-transfer-svc
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 10
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-soap-facade
  folder: src/LegacyWcf
  body_sha256: 70622b30db3891fd4597b15c5407ee25fbbe79b4c7051c3774d9f83f575f7f4d
---

# Contract: backend-bas-file-transfer-svc

This component is legacy WCF host wiring and legacy contract references for the File Transfer SOAP surface. In the target repository, it is treated as a **parity reference** under services/fxfer-soap-facade/src/LegacyWcf and does not expose a separately deployable runtime surface.

## Offers

### Legacy SOAP contract references (parity)

These interfaces define the legacy WCF SOAP operation surface (signatures + legacy ServiceContract namespaces), for parity comparison and migration validation:

- IFileTransferSvcService (namespace: HP.HSP.FileTransferSvc)
  - Command operations:
    - FileTransferAdd(FtsSourceLocationAddCommand) : FtsSourceLocationAdded
    - UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand) : FtsSourceLocationUpdated
    - InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand) : FtsSourceLocationInvalidated
    - AddFtpServer(AddFtpServerCommand) : FtpServerAdded
    - UpdateFtpServer(UpdateFtpServerCommand) : FtpServerUpdated
    - InvalidateFtpServer(InvalidateFtpServerCommand) : FtpServerInvalidated
    - UpdateIntegrationFile(UpdateIntegrationFileCommand) : IntegrationFileUpdated
  - Availability operations (via IServiceAvailability):
    - IsServiceAvailable() : boolean
    - IsServiceHealthy(tenantId: string) : boolean

- IFileTransferSvcQueryService (namespace: HP.HSP.FileTransferSvcQueryService)
  - Query operations:
    - SearchSourceLocations(SourceLocationQuery) : SourceLocationQuery
    - GetFtpServers(FtpServerQuery) : FtpServerQuery
    - GetSourceLocations(GetSourceLocationQuery) : GetSourceLocationQuery
    - SearchFileTransferMovement(FileTransferTrackingQuery) : FileTransferTrackingQuery
    - SearchInboundTarget(InboundTargetQuery) : InboundTargetQuery
    - GetIntegrationFile(GetIntegrationFileQuery) : GetIntegrationFileQuery
  - Availability operations (via IServiceAvailability):
    - IsServiceAvailable() : boolean
    - IsServiceHealthy(tenantId: string) : boolean

### Legacy fault behavior (hosting semantics)

The legacy host converts unhandled exceptions into SOAP faults:

- If the thrown exception implements IFaultGenerator and GenerateFault() returns a non-null FaultException, that fault is re-thrown (passthrough).
- Otherwise the exception is wrapped as a ServiceException and returned as FaultException<ServiceException> with FaultReason = ServiceException.Reason.

## Uses

None (no component-to-component dependencies declared for this component).

## Data

This component itself does not own persistence. Through the registered File Transfer DbContext / writers it participates in reads from:

- FTP_SERVER
- FTS_INBOUND_TARGET
- FTS_INTEGRATION_FILE
- FTS_SUSPENSION

