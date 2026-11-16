---
type: Generation Brief
title: 'Brief: component shared-file-transfer-svc-data-access-2'
description: Instructions the tool gave the writer session for the shared-file-transfer-svc-data-access-2 component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-file-transfer-svc-data-access-2
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:28:54Z
sources:
- id: inventory
  resource: /technical/components/shared-file-transfer-svc-data-access-2/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-file-transfer-svc-data-access-2
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 3
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Infrastructure/DataAccess/Constants
  body_sha256: 7a4c693c0b384b052f5c9f07ec8a51cc125f409a982a9eafa0ca40eb589d51de
---

# Brief: component `shared-file-transfer-svc-data-access-2`

- HPP.FileTransfer.BAS.FileTransferSvc.DataAccess (shared tier, 3 classes, 0 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-file-transfer-svc-data-access-2/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-file-transfer-svc-data-access-2/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Infrastructure/DataAccess/Constants`: write `services/fxfer-domain-api/src/Infrastructure/DataAccess/Constants` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
