---
type: Generation Brief
title: 'Brief: component shared-contracts-exceptions'
description: Instructions the tool gave the writer session for the shared-contracts-exceptions component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-exceptions
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:35:59Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-exceptions/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-exceptions
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 4
  methods: 1
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Exceptions
  body_sha256: 01022a3a627fd0c81d61bfbbd1556f6ae9d3f2b0691d4cb3bb51124dcfb444d8
---

# Brief: component `shared-contracts-exceptions`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions (shared tier, 4 classes, 1 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-contracts-exceptions/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-contracts-exceptions/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Contracts/Exceptions`: write `services/fxfer-domain-api/src/Contracts/Exceptions` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
