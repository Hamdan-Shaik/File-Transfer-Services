---
type: Behavior Specification
title: 'backend-file-transfer-mgmt-ctrl: Integration file service wrappers'
description: 'What the backend-file-transfer-mgmt-ctrl component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-mgmt-ctrl
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T06:39:37Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-mgmt-ctrl/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-mgmt-ctrl
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 4
  methods: 2
  owned_rules: 0
  flows: []
  depends_on:
  - backend-configure-source-locations-and-destinations-api
  deployable: fxfer-transfer-worker
  folder: src/Infrastructure/Adapters
  body_sha256: f191e31dc1332e4518404d9b5fcf805dbeeb723bbbd9bbdeee784d282c7fae76
---

# backend-file-transfer-mgmt-ctrl: Integration file service wrappers

This component is a thin legacy wrapper around the BAS File Transfer service door used by the batch File Transfer Management worker logic.
It provides two methods used by the batch runtime to look up an integration file definition and to record an integration-file update.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Ctrl.FileTransferSvcQueryServiceProvider
Wrapper for `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService` used by batch to read integration-file configuration.

### GetIntegrationFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.GetIntegrationFileQuery)
- Purpose: Fetch the first matching integration file definition.
- Inputs: `query` (legacy CQRS query; commonly populated with `Where.FileName` and/or `Where.FTSIntegrationFileId`).
- Logic:
  - Calls `FileTransferSvcQueryService.GetIntegrationFile(query)`.
  - If the response is null, or `response.Results` is null, or `response.Results.Count == 0`, returns null.
  - Otherwise returns `response.Results.FirstOrDefault()`.
- Data: Reads `orm:FTS_INTEGRATION_FILE` (through the called service door).
- Errors: Exceptions thrown by the service call bubble up to the caller.
- Rules: None.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Ctrl.FileTransferSvcServiceProvider
Wrapper for `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService` used by batch to update integration-file state.

### UpdateIntegrationFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.UpdateIntegrationFileCommand)
- Purpose: Update an integration file record and return the legacy `IntegrationFileUpdated` event.
- Inputs: `command` (legacy CQRS command; includes `FtsIntegrationFileId`, `ConfigId`, and `IsValid`).
- Logic: Calls `FileTransferSvcService.UpdateIntegrationFile(command)` and returns the result.
- Data: Writes `orm:FTS_INTEGRATION_FILE` (through the called service door).
- Errors: Exceptions thrown by routing/handlers bubble up to the caller.
- Rules: None.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Ctrl.IFileTransferSvcQueryServiceProvider
Interface used by batch code to access `GetIntegrationFile`.

### GetIntegrationFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.GetIntegrationFileQuery)
- Purpose: Query for an integration file definition and return a single model.
- Inputs: `query`.
- Logic: Implementations return the first matching record, or null when there are no matches.
- Data: Reads `orm:FTS_INTEGRATION_FILE`.
- Errors: Implementation-defined; legacy behavior allows exceptions to bubble.
- Rules: None.

## HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Ctrl.IFileTransferSvcServiceProvider
Interface used by batch code to access `UpdateIntegrationFile`.

### UpdateIntegrationFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.UpdateIntegrationFileCommand)
- Purpose: Update an integration file record.
- Inputs: `command`.
- Logic: Delegates to the underlying integration-file update operation.
- Data: Writes `orm:FTS_INTEGRATION_FILE`.
- Errors: Implementation-defined; legacy behavior allows exceptions to bubble.
- Rules: None.

## Rules implemented

None (this component owns no business rules).

## Not carried over

None.

