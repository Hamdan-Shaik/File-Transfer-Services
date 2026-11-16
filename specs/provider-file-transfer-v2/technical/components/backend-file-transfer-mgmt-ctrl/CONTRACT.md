---
type: Interface Contract
title: 'Contract: backend-file-transfer-mgmt-ctrl'
description: What the backend-file-transfer-mgmt-ctrl component offers other components and what it uses from them.
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
  body_sha256: ba75f3b7e5ad0ee0534e6ec0b07f6b8f22ff55c2e421a773a09d98597f850174
---

# Contract: backend-file-transfer-mgmt-ctrl

This component provides worker-internal adapters used by batch execution to query and update integration file definitions.
It preserves legacy BAS FileTransferSvc contract types (`GetIntegrationFileQuery`, `UpdateIntegrationFileCommand`) while sourcing the data/change through `fxfer-domain-api`.

## Offers

### Worker internal interfaces

- `IFileTransferSvcQueryServiceProvider`
  - `GetIntegrationFile(GetIntegrationFileQuery query) -> FtsIntegrationFileModel?`

- `IFileTransferSvcServiceProvider`
  - `UpdateIntegrationFile(UpdateIntegrationFileCommand command) -> IntegrationFileUpdated`

## Uses

### Component dependencies

- `backend-configure-source-locations-and-destinations-api`
  - Uses these `fxfer-domain-api` endpoints:
    - `GET /api/fxfer-domain/v1/integration-files`
    - `PATCH /api/fxfer-domain/v1/integration-files/{integrationFileId}`

### Target shared libraries

- `IFxferDomainApiClient` (OpenAPI-generated client for `fxfer-domain-api`)
- `IRequestContextAccessor` (tenant_id, correlation_id, W3C trace context propagation)

## Data

This component does not own persistence. Through `fxfer-domain-api` it participates in tenant-scoped reads/writes of:

- `orm:FTS_INTEGRATION_FILE`

