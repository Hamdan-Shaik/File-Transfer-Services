---
type: Generation Brief
title: 'Brief: deployable fxfer-projection-worker'
description: Instructions the tool gave the writer session for the fxfer-projection-worker deployable.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-projection-worker
tags:
- deployable
- projector
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:14:33Z
sources:
- id: component-backend-data-access-event-handlers
  resource: /technical/components/backend-data-access-event-handlers/SPEC.md
  title: Component placed in it
- id: component-backend-data-access-event-handlers-2
  resource: /technical/components/backend-data-access-event-handlers-2/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (2 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-projection-worker
  generation:
    status: done
    attempts: 1
  path: workers/fxfer-projection-worker
  kind: projector
  stack: .NET 10 worker runtime
  components: 2
  depends_on:
  - fxfer-domain-api
  - fxfer-db-schema
  body_sha256: 6eb7cc4aed4cc4072d5558e09441a042e48893ab95fcea1adaf0704c8b8916a4
---

# Brief: deployable `fxfer-projection-worker`

- Playbook: `inputs/playbook/deployable.md`
- Target state: `inputs/target-state.md`
- Architecture: `technical/target/architecture.yaml`, `technical/target/TARGET-ARCHITECTURE.md`
- Write exactly these files in `technical/target/deployables/fxfer-projection-worker/`: `README.md`, `STRUCTURE.md`, `CONTRACT.md`
- Its path in the new repository: `workers/fxfer-projection-worker`

## The deployable (from architecture.yaml)

```yaml
name: fxfer-projection-worker
kind: projector
path: workers/fxfer-projection-worker
stack: .NET 10 worker runtime
role: Builds/updates read models from FXFER domain events.
build: dotnet build workers/fxfer-projection-worker
depends_on:
- fxfer-domain-api
- fxfer-db-schema
```

## Components placed in it

- `backend-data-access-event-handlers` in `src/Infrastructure/Projections`: `technical/components/backend-data-access-event-handlers/INVENTORY.yaml`
- `backend-data-access-event-handlers-2` in `src/Infrastructure/Projections`: `technical/components/backend-data-access-event-handlers-2/INVENTORY.yaml`

## Deployables it depends on

- `fxfer-domain-api`: `technical/target/deployables/fxfer-domain-api/`
- `fxfer-db-schema`: `technical/target/deployables/fxfer-db-schema/`
