---
type: Test Plan
title: 'Tests: backend-file-transfer-mgmt-ctrl'
description: The tests that prove the backend-file-transfer-mgmt-ctrl component, one or more per business rule.
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
  body_sha256: 9eacaf6e6d0508194aad937a9c9acc16e1793421dc78d6c282b0aabc943db831
---

# Tests: backend-file-transfer-mgmt-ctrl

This component has no owned business rules. Tests focus on adapter parity and contract correctness.

## Unit tests

### FileTransferSvcQueryServiceProvider.GetIntegrationFile

- Returns null when `fxfer-domain-api` returns no results.
- Returns the first item when `fxfer-domain-api` returns one or more integration files.
- Projects the domain API `IntegrationFileDefinition` fields into the legacy `FtsIntegrationFileModel` fields.
- Uses `query.Where.FTSIntegrationFileId` (when not `Guid.Empty`) and `query.Where.FileName` to build query parameters for `GET /integration-files`.

### FileTransferSvcServiceProvider.UpdateIntegrationFile

- When `command.ConfigId` is null, `command.Validate()` throws the legacy `BusinessException`.
- Calls `PATCH /integration-files/{integrationFileId}` with `integrationFileId = command.FtsIntegrationFileId` and body `{ configId, isValid }`.
- Returns an `IntegrationFileUpdated` event whose `FtsIntegrationFileId` and `LastModifiedDate` reflect the domain API response.
- Bubbles domain API client exceptions to the caller.

## Contract tests

- `technical/components/backend-file-transfer-mgmt-ctrl/openapi.yaml` asserts `x-no-http-surface: true` and `paths: {}`.
- `technical/components/backend-file-transfer-mgmt-ctrl/CONTRACT.md` asserts it names dependency `backend-configure-source-locations-and-destinations-api` and both integration-file endpoints it uses.

