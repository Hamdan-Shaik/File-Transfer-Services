---
type: Generation Brief
title: 'Brief: component backend-bas-file-transfer-svc'
description: Instructions the tool gave the writer session for the backend-bas-file-transfer-svc component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-bas-file-transfer-svc
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:09:36Z
sources:
- id: inventory
  resource: /technical/components/backend-bas-file-transfer-svc/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-bas-file-transfer-svc
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 10
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-soap-facade
  folder: src/LegacyWcf
  body_sha256: 012e6a5877c2aeed19715fd859b04b26edebabbc9dc323aab847df9bdcc364da
---

# Brief: component `backend-bas-file-transfer-svc`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc (backend tier, 5 classes, 10 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-bas-file-transfer-svc/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-bas-file-transfer-svc/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-soap-facade` (`services/fxfer-soap-facade`), folder `src/LegacyWcf`: write `services/fxfer-soap-facade/src/LegacyWcf` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-soap-facade/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
