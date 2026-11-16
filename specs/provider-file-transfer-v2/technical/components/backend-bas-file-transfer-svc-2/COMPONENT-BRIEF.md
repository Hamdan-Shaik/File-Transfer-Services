---
type: Generation Brief
title: 'Brief: component backend-bas-file-transfer-svc-2'
description: Instructions the tool gave the writer session for the backend-bas-file-transfer-svc-2 component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-bas-file-transfer-svc-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:18:17Z
sources:
- id: inventory
  resource: /technical/components/backend-bas-file-transfer-svc-2/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-bas-file-transfer-svc-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 10
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-soap-facade
  folder: src/CoreWcf
  body_sha256: 666d5e3baf8d9e8e8134b7c685abbe04f53cdabfa434737c7e972442f08a86ea
---

# Brief: component `backend-bas-file-transfer-svc-2`

- HPP.FileTransfer.BAS.FileTransferSvc (backend tier, 6 classes, 10 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-bas-file-transfer-svc-2/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-bas-file-transfer-svc-2/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-soap-facade` (`services/fxfer-soap-facade`), folder `src/CoreWcf`: write `services/fxfer-soap-facade/src/CoreWcf` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-soap-facade/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Packages it calls (contracts to keep)

- `autofac`
- `corewcf.primitives`
- `hpp.core.bas.cqrs`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
