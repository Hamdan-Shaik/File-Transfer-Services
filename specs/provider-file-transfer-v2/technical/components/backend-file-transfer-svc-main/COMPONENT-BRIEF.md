---
type: Generation Brief
title: 'Brief: component backend-file-transfer-svc-main'
description: Instructions the tool gave the writer session for the backend-file-transfer-svc-main component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-svc-main
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T20:16:25Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-svc-main/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-svc-main
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 14
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-soap-facade
  folder: src/Host
  body_sha256: d67b90d954d1c16e6f047d85998e66145414582bf9ca8751955be1bfc39d40cb
---

# Brief: component `backend-file-transfer-svc-main`

- HPP.FileTransfer.BAS.FileTransferSvc.Main (backend tier, 5 classes, 14 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-file-transfer-svc-main/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-file-transfer-svc-main/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-soap-facade` (`services/fxfer-soap-facade`), folder `src/Host`: write `services/fxfer-soap-facade/src/Host` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-soap-facade/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
