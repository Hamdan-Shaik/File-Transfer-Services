---
type: Generation Brief
title: 'Brief: component backend-data-file-transfer-constants'
description: Instructions the tool gave the writer session for the backend-data-file-transfer-constants component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-file-transfer-constants
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:29:46Z
sources:
- id: inventory
  resource: /technical/components/backend-data-file-transfer-constants/INVENTORY.yaml
  title: Every legacy class and method (16 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-file-transfer-constants
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 16
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants
  body_sha256: 730a9cb6dccc427831191598357087479e29ac140be798d23c88910d8dd4db4c
---

# Brief: component `backend-data-file-transfer-constants`

- HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants (backend tier, 16 classes, 0 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-data-file-transfer-constants/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-data-file-transfer-constants/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-mfe` (`apps/fxfer-mfe`), folder `src/legacy-constants`: write `apps/fxfer-mfe/src/legacy-constants` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-mfe/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
