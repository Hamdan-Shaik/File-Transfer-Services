---
type: Generation Brief
title: 'Brief: component shared-contracts-domain-2'
description: Instructions the tool gave the writer session for the shared-contracts-domain-2 component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-domain-2
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:19:37Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-domain-2/INVENTORY.yaml
  title: Every legacy class and method (15 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-domain-2
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 15
  methods: 1
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Domain
  body_sha256: 9af99644b1df6629de28d3ee58f11b764f5e8c4cdc9d5e0cfcfd15f251452f36
---

# Brief: component `shared-contracts-domain-2`

- HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain (shared tier, 15 classes, 1 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-contracts-domain-2/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-contracts-domain-2/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Contracts/Domain`: write `services/fxfer-domain-api/src/Contracts/Domain` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
