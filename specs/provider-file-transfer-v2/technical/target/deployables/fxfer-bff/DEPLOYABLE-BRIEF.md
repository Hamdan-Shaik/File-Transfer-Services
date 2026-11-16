---
type: Generation Brief
title: 'Brief: deployable fxfer-bff'
description: Instructions the tool gave the writer session for the fxfer-bff deployable.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-bff
tags:
- bff
- deployable
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:45:39Z
sources:
- id: component-backend-data-file-transfer
  resource: /technical/components/backend-data-file-transfer/SPEC.md
  title: Component placed in it
- id: component-backend-data-file-transfer-tracking
  resource: /technical/components/backend-data-file-transfer-tracking/SPEC.md
  title: Component placed in it
- id: component-backend-providers-file-transfer
  resource: /technical/components/backend-providers-file-transfer/SPEC.md
  title: Component placed in it
- id: component-ui-file-transfer-api-controllers
  resource: /technical/components/ui-file-transfer-api-controllers/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (4 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-bff
  generation:
    status: done
    attempts: 1
  path: services/fxfer-bff
  kind: bff
  stack: .NET 10 (Native AOT)
  components: 4
  depends_on:
  - fxfer-domain-api
  body_sha256: d93c47052a881a378fcee1d67f7eb3a80db62a53e8fa9da24b91ee084ce0e1e7
---

# Brief: deployable `fxfer-bff`

- Playbook: `inputs/playbook/deployable.md`
- Target state: `inputs/target-state.md`
- Architecture: `technical/target/architecture.yaml`, `technical/target/TARGET-ARCHITECTURE.md`
- Write exactly these files in `technical/target/deployables/fxfer-bff/`: `README.md`, `STRUCTURE.md`, `CONTRACT.md`
- Its path in the new repository: `services/fxfer-bff`

## The deployable (from architecture.yaml)

```yaml
name: fxfer-bff
kind: bff
path: services/fxfer-bff
stack: .NET 10 (Native AOT)
role: Browser-facing API facade for the FXFER micro-frontend.
build: dotnet build services/fxfer-bff
depends_on:
- fxfer-domain-api
```

## Components placed in it

- `backend-data-file-transfer` in `src/Contracts`: `technical/components/backend-data-file-transfer/INVENTORY.yaml`
- `backend-data-file-transfer-tracking` in `src/Contracts/Tracking`: `technical/components/backend-data-file-transfer-tracking/INVENTORY.yaml`
- `backend-providers-file-transfer` in `src/Application/Providers`: `technical/components/backend-providers-file-transfer/INVENTORY.yaml`
- `ui-file-transfer-api-controllers` in `src/Api`: `technical/components/ui-file-transfer-api-controllers/INVENTORY.yaml`

## Deployables it depends on

- `fxfer-domain-api`: `technical/target/deployables/fxfer-domain-api/`
