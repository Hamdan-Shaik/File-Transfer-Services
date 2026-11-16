---
type: Generation Brief
title: 'Brief: component shared-contracts-batch'
description: Instructions the tool gave the writer session for the shared-contracts-batch component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-batch
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:02:24Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-batch/INVENTORY.yaml
  title: Every legacy class and method (15 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-batch
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 15
  methods: 1
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Contracts
  body_sha256: 6f092d2809aeab07cce2bb7423e77a99b56da1cf57ac373230094b3c68c5d9cb
---

# Brief: component `shared-contracts-batch`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch (shared tier, 15 classes, 1 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-contracts-batch/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-contracts-batch/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`), folder `src/Contracts`: write `workers/fxfer-transfer-worker/src/Contracts` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-transfer-worker/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
