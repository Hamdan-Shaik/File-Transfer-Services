---
type: Generation Brief
title: 'Brief: deployable fxfer-mfe'
description: Instructions the tool gave the writer session for the fxfer-mfe deployable.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-mfe
tags:
- deployable
- mfe
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:53:38Z
sources:
- id: component-backend-data-file-transfer-constants
  resource: /technical/components/backend-data-file-transfer-constants/SPEC.md
  title: Component placed in it
- id: component-backend-file-transfer-constants-localization
  resource: /technical/components/backend-file-transfer-constants-localization/SPEC.md
  title: Component placed in it
- id: component-backend-localization-labels
  resource: /technical/components/backend-localization-labels/SPEC.md
  title: Component placed in it
- id: component-shared-data-file-transfer-constants
  resource: /technical/components/shared-data-file-transfer-constants/SPEC.md
  title: Component placed in it
- id: component-ui-configure-source-locations-and-destinations-screens
  resource: /technical/components/ui-configure-source-locations-and-destinations-screens/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (6 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-mfe
  generation:
    status: done
    attempts: 1
  path: apps/fxfer-mfe
  kind: mfe
  stack: Node.js + Next.js 15 + React 19
  components: 6
  depends_on:
  - fxfer-bff
  body_sha256: 36a65fc4df3726ecb295e58526c872d4588d8fa017f3835f4c320971208d2272
---

# Brief: deployable `fxfer-mfe`

- Playbook: `inputs/playbook/deployable.md`
- Target state: `inputs/target-state.md`
- Architecture: `technical/target/architecture.yaml`, `technical/target/TARGET-ARCHITECTURE.md`
- Write exactly these files in `technical/target/deployables/fxfer-mfe/`: `README.md`, `STRUCTURE.md`, `CONTRACT.md`
- Its path in the new repository: `apps/fxfer-mfe`

## The deployable (from architecture.yaml)

```yaml
name: fxfer-mfe
kind: mfe
path: apps/fxfer-mfe
stack: Node.js + Next.js 15 + React 19
role: Operator UI for FXFER configuration and tracking.
build: npm --prefix apps/fxfer-mfe ci && npm --prefix apps/fxfer-mfe run build
depends_on:
- fxfer-bff
```

## Components placed in it

- `backend-data-file-transfer-constants` in `src/legacy-constants`: `technical/components/backend-data-file-transfer-constants/INVENTORY.yaml`
- `backend-file-transfer-constants-localization` in `src/legacy-constants/localization`: `technical/components/backend-file-transfer-constants-localization/INVENTORY.yaml`
- `backend-localization-labels` in `src/legacy-constants/localization`: `technical/components/backend-localization-labels/INVENTORY.yaml`
- `shared-data-file-transfer-constants` in `src/legacy-constants`: `technical/components/shared-data-file-transfer-constants/INVENTORY.yaml`
- `ui-configure-source-locations-and-destinations-screens` in `src/screens/configure-source-locations-and-destinations`: `technical/components/ui-configure-source-locations-and-destinations-screens/INVENTORY.yaml`
- `ui-manage-ftp-servers-and-credentials-screens` in `src/screens/manage-ftp-servers-and-credentials`: `technical/components/ui-manage-ftp-servers-and-credentials-screens/INVENTORY.yaml`

## Deployables it depends on

- `fxfer-bff`: `technical/target/deployables/fxfer-bff/`
