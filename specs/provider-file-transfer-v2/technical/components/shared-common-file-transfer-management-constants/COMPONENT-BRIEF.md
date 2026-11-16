---
type: Generation Brief
title: 'Brief: component shared-common-file-transfer-management-constants'
description: Instructions the tool gave the writer session for the shared-common-file-transfer-management-constants component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-common-file-transfer-management-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:54:12Z
sources:
- id: inventory
  resource: /technical/components/shared-common-file-transfer-management-constants/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-common-file-transfer-management-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Domain/Constants
  body_sha256: d192d97550c2eead1e684c354a11f17d371d0dcfabc895f660214ebf3a55c173
---

# Brief: component `shared-common-file-transfer-management-constants`

- HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Common.FileTransferManagementConstants (shared tier, 8 classes, 0 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-common-file-transfer-management-constants/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-common-file-transfer-management-constants/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`), folder `src/Domain/Constants`: write `workers/fxfer-transfer-worker/src/Domain/Constants` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-transfer-worker/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
