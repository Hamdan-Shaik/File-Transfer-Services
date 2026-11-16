---
type: Generation Brief
title: 'Brief: component data-data-access-entities-2'
description: Instructions the tool gave the writer session for the data-data-access-entities-2 component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/data-data-access-entities-2
tags:
- component
- data
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:22:58Z
sources:
- id: inventory
  resource: /technical/components/data-data-access-entities-2/INVENTORY.yaml
  title: Every legacy class and method (10 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:data-data-access-entities-2
  generation:
    status: done
    attempts: 1
  tier: data
  classes: 10
  methods: 2
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Infrastructure/DataAccess
  body_sha256: 044cbc21af6cdefd60eb34295bf19007e1396ba38553a5a6421f1a53388c2ae8
---

# Brief: component `data-data-access-entities-2`

- HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.Entities (data tier, 10 classes, 2 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/data-data-access-entities-2/INVENTORY.yaml`
- Write exactly these files in `technical/components/data-data-access-entities-2/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Infrastructure/DataAccess`: write `services/fxfer-domain-api/src/Infrastructure/DataAccess` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
