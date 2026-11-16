---
type: Generation Brief
title: 'Brief: component backend-data-access-batch-data-access-2'
description: Instructions the tool gave the writer session for the backend-data-access-batch-data-access-2 component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-batch-data-access-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:55:48Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-batch-data-access-2/INVENTORY.yaml
  title: Every legacy class and method (16 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-batch-data-access-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 16
  methods: 47
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Infrastructure/DataAccess
  body_sha256: 2e1879f258640c422f0e7a93b182cc0fcd7c0790c616260585456f1ba293a01e
---

# Brief: component `backend-data-access-batch-data-access-2`

- HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.BatchDataAccess (backend tier, 16 classes, 47 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-data-access-batch-data-access-2/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-data-access-batch-data-access-2/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`), folder `src/Infrastructure/DataAccess`: write `workers/fxfer-transfer-worker/src/Infrastructure/DataAccess` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-transfer-worker/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Packages it calls (contracts to keep)

- `hpp.core.bas.cqrs`
- `microsoft.entityframeworkcore`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
