---
type: Interface Contract
title: 'Contract: backend-run-file-transfer-batch-api'
description: What the backend-run-file-transfer-batch-api component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-run-file-transfer-batch-api
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:02:28Z
sources:
- id: rules-run-file-transfer-batch
  resource: /technical/flows/run-file-transfer-batch/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-run-file-transfer-batch-api/INVENTORY.yaml
  title: Every legacy class and method (2 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-run-file-transfer-batch-api
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 2
  methods: 7
  owned_rules: 14
  flows:
  - run-file-transfer-batch
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Worker
  body_sha256: 4cc90df618a01674f23d6d852ecbb1a0475bc6d8c5b402e2121f05a550241cd2
---

# Contract: backend-run-file-transfer-batch-api

This component provides the CLI/batch entry surface for starting file-transfer batch execution.

## Offers

### Command: run-file-transfer-batch-basic

- Input: `string[] args` (any number of values; not validated)
- Output: integer process result (the resolved batch executor result), or `-1` on unexpected failure.

### Command: run-file-transfer-batch-managed

Accepted run detail patterns:

- 1-value form: `<tenantId>`
  - `tenantId` must be a GUID in standard 36-character format.
  - Meaning: process all files for the tenant.

- 3-value form: `<label> <commandValue> <tenantId>`
  - `label` is not validated.
  - `commandValue` must be either an integer or a GUID in standard 36-character format.
  - `tenantId` must be a GUID in standard 36-character format.

- Output: integer process result (the resolved batch executor result) or `-1`.
- Side effects within this component:
  - emits usage guidance when the run details are not accepted.
  - emits an error message `"{key} is not specified in app.config"` when a mandatory key is missing.

### Command: rerun-tracked-file (T-007)

- Input:
  - `tenantId` (GUID)
  - `trackedFileId` (GUID)
- Output: integer process result (the resolved batch executor result) or `-1`.

## Uses

### Batch executor resolution

- Requires a registered `IBatchExecutor<int>` keyed/named `FileTransferExecutor`.

### Tenant settings loader

- Requires a tenant settings source that can be loaded (once) for a provided `tenantId` and then queried for required keys.

## Data owned

None.