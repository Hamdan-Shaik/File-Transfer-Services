---
type: Generation Brief
title: 'Brief: deployable fxfer-soap-facade'
description: Instructions the tool gave the writer session for the fxfer-soap-facade deployable.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-soap-facade
tags:
- deployable
- dotnet-service
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:37:46Z
sources:
- id: component-backend-bas-file-transfer-svc
  resource: /technical/components/backend-bas-file-transfer-svc/SPEC.md
  title: Component placed in it
- id: component-backend-bas-file-transfer-svc-2
  resource: /technical/components/backend-bas-file-transfer-svc-2/SPEC.md
  title: Component placed in it
- id: component-backend-file-transfer-svc-main
  resource: /technical/components/backend-file-transfer-svc-main/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (3 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-soap-facade
  generation:
    status: done
    attempts: 1
  path: services/fxfer-soap-facade
  kind: dotnet-service
  stack: .NET 10 (CoreWCF)
  components: 3
  depends_on:
  - fxfer-domain-api
  body_sha256: a7e9061970c02b9ef9115fc4d24761cfa9ae64552a6b1f1d62bda4b91a7e4107
---

# Brief: deployable `fxfer-soap-facade`

- Playbook: `inputs/playbook/deployable.md`
- Target state: `inputs/target-state.md`
- Architecture: `technical/target/architecture.yaml`, `technical/target/TARGET-ARCHITECTURE.md`
- Write exactly these files in `technical/target/deployables/fxfer-soap-facade/`: `README.md`, `STRUCTURE.md`, `CONTRACT.md`
- Its path in the new repository: `services/fxfer-soap-facade`

## The deployable (from architecture.yaml)

```yaml
name: fxfer-soap-facade
kind: dotnet-service
path: services/fxfer-soap-facade
stack: .NET 10 (CoreWCF)
role: SOAP compatibility facade for legacy consumers (no new SOAP functionality).
build: dotnet build services/fxfer-soap-facade
depends_on:
- fxfer-domain-api
```

## Components placed in it

- `backend-bas-file-transfer-svc` in `src/LegacyWcf`: `technical/components/backend-bas-file-transfer-svc/INVENTORY.yaml`
- `backend-bas-file-transfer-svc-2` in `src/CoreWcf`: `technical/components/backend-bas-file-transfer-svc-2/INVENTORY.yaml`
- `backend-file-transfer-svc-main` in `src/Host`: `technical/components/backend-file-transfer-svc-main/INVENTORY.yaml`

## Deployables it depends on

- `fxfer-domain-api`: `technical/target/deployables/fxfer-domain-api/`
