---
type: Generation Brief
title: 'Brief: component backend-file-transfer-mgmt-business'
description: Instructions the tool gave the writer session for the backend-file-transfer-mgmt-business component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-mgmt-business
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:00:07Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-mgmt-business/INVENTORY.yaml
  title: Every legacy class and method (23 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-mgmt-business
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 23
  methods: 121
  owned_rules: 0
  flows: []
  depends_on:
  - backend-business-helpers-2
  deployable: fxfer-transfer-worker
  folder: src/Application
  body_sha256: ae91ee856b4a485200e690d76fb85fe774b5f77f732a2ee35c91db03e4f7c4f5
---

# Brief: component `backend-file-transfer-mgmt-business`

- HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Business (backend tier, 23 classes, 121 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-file-transfer-mgmt-business/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-file-transfer-mgmt-business/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`), folder `src/Application`: write `workers/fxfer-transfer-worker/src/Application` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-transfer-worker/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

- `backend-business-helpers-2`: `technical/components/backend-business-helpers-2/`

## Flows it takes part in

None.

## Packages it calls (contracts to keep)

- `ICSharpCode.SharpZipLib`
- `Renci.SshNet`
- `edtFTPnetPRO`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
