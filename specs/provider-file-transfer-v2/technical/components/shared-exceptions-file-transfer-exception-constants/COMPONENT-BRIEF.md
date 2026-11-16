---
type: Generation Brief
title: 'Brief: component shared-exceptions-file-transfer-exception-constants'
description: Instructions the tool gave the writer session for the shared-exceptions-file-transfer-exception-constants component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-exceptions-file-transfer-exception-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:10:16Z
sources:
- id: inventory
  resource: /technical/components/shared-exceptions-file-transfer-exception-constants/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-exceptions-file-transfer-exception-constants
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
  folder: src/Contracts/Exceptions
  body_sha256: 2d9bde74c93f70704778115a33cd2793d9d894c1d345d3db30cd5876c22056d4
---

# Brief: component `shared-exceptions-file-transfer-exception-constants`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferExceptionConstants (shared tier, 3 classes, 0 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-exceptions-file-transfer-exception-constants/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-exceptions-file-transfer-exception-constants/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
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
