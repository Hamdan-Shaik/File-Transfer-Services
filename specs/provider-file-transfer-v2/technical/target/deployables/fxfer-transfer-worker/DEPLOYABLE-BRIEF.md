---
type: Generation Brief
title: 'Brief: deployable fxfer-transfer-worker'
description: Instructions the tool gave the writer session for the fxfer-transfer-worker deployable.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-transfer-worker
tags:
- deployable
- provider-file-transfer-v2
- worker
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:23:01Z
sources:
- id: component-backend-batch-business
  resource: /technical/components/backend-batch-business/SPEC.md
  title: Component placed in it
- id: component-backend-business-helpers
  resource: /technical/components/backend-business-helpers/SPEC.md
  title: Component placed in it
- id: component-backend-business-helpers-2
  resource: /technical/components/backend-business-helpers-2/SPEC.md
  title: Component placed in it
- id: component-backend-data-access-batch-data-access
  resource: /technical/components/backend-data-access-batch-data-access/SPEC.md
  title: Component placed in it
- id: component-backend-data-access-batch-data-access-2
  resource: /technical/components/backend-data-access-batch-data-access-2/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (11 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-transfer-worker
  generation:
    status: done
    attempts: 1
  path: workers/fxfer-transfer-worker
  kind: worker
  stack: .NET 10 worker runtime
  components: 11
  depends_on:
  - fxfer-domain-api
  body_sha256: f0491b23c837786d763487aefac81ea49ba3ec29117555015154937d05c6be21
---

# Brief: deployable `fxfer-transfer-worker`

- Playbook: `inputs/playbook/deployable.md`
- Target state: `inputs/target-state.md`
- Architecture: `technical/target/architecture.yaml`, `technical/target/TARGET-ARCHITECTURE.md`
- Write exactly these files in `technical/target/deployables/fxfer-transfer-worker/`: `README.md`, `STRUCTURE.md`, `CONTRACT.md`
- Its path in the new repository: `workers/fxfer-transfer-worker`

## The deployable (from architecture.yaml)

```yaml
name: fxfer-transfer-worker
kind: worker
path: workers/fxfer-transfer-worker
stack: .NET 10 worker runtime
role: Scheduled execution of protocol I/O and recording outcomes via domain APIs.
build: dotnet build workers/fxfer-transfer-worker
depends_on:
- fxfer-domain-api
```

## Components placed in it

- `backend-batch-business` in `src/Application`: `technical/components/backend-batch-business/INVENTORY.yaml`
- `backend-business-helpers` in `src/Application/Helpers`: `technical/components/backend-business-helpers/INVENTORY.yaml`
- `backend-business-helpers-2` in `src/Application/Helpers`: `technical/components/backend-business-helpers-2/INVENTORY.yaml`
- `backend-data-access-batch-data-access` in `src/Infrastructure/DataAccess`: `technical/components/backend-data-access-batch-data-access/INVENTORY.yaml`
- `backend-data-access-batch-data-access-2` in `src/Infrastructure/DataAccess`: `technical/components/backend-data-access-batch-data-access-2/INVENTORY.yaml`
- `backend-file-transfer-mgmt-business` in `src/Application`: `technical/components/backend-file-transfer-mgmt-business/INVENTORY.yaml`
- `backend-file-transfer-mgmt-ctrl` in `src/Infrastructure/Adapters`: `technical/components/backend-file-transfer-mgmt-ctrl/INVENTORY.yaml`
- `backend-file-transfer-mgmt-data-access` in `src/Infrastructure/DataAccess`: `technical/components/backend-file-transfer-mgmt-data-access/INVENTORY.yaml`
- `backend-run-file-transfer-batch-api` in `src/Worker`: `technical/components/backend-run-file-transfer-batch-api/INVENTORY.yaml`
- `shared-common-file-transfer-management-constants` in `src/Domain/Constants`: `technical/components/shared-common-file-transfer-management-constants/INVENTORY.yaml`
- `shared-contracts-batch` in `src/Contracts`: `technical/components/shared-contracts-batch/INVENTORY.yaml`

## Deployables it depends on

- `fxfer-domain-api`: `technical/target/deployables/fxfer-domain-api/`
