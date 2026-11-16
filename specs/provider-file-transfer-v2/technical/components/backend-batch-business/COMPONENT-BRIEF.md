---
type: Generation Brief
title: 'Brief: component backend-batch-business'
description: Instructions the tool gave the writer session for the backend-batch-business component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-batch-business
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T00:36:41Z
sources:
- id: inventory
  resource: /technical/components/backend-batch-business/INVENTORY.yaml
  title: Every legacy class and method (19 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-batch-business
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 19
  methods: 78
  owned_rules: 0
  flows: []
  depends_on:
  - backend-business-helpers
  deployable: fxfer-transfer-worker
  folder: src/Application
  body_sha256: d385e76281e37a88a46190dc5746bec82fb412365be491772467b6755f2802a9
---

# Brief: component `backend-batch-business`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business (backend tier, 19 classes, 78 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-batch-business/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-batch-business/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`), folder `src/Application`: write `workers/fxfer-transfer-worker/src/Application` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-transfer-worker/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

- `backend-business-helpers`: `technical/components/backend-business-helpers/`

## Flows it takes part in

None.

## Packages it calls (contracts to keep)

- `ICSharpCode.SharpZipLib`
- `edtFTPnetPRO`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
