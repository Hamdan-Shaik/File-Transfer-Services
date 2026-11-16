---
type: Generation Brief
title: 'Brief: component shared-common-common-constants'
description: Instructions the tool gave the writer session for the shared-common-common-constants component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-common-common-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:40:13Z
sources:
- id: inventory
  resource: /technical/components/shared-common-common-constants/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-common-common-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 5
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain/Constants
  body_sha256: 0482288af8803a2ec8270c29f15de43cf4fea558abd907f49327ad2292a6d05a
---

# Brief: component `shared-common-common-constants`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants (shared tier, 5 classes, 0 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-common-common-constants/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-common-common-constants/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Domain/Constants`: write `services/fxfer-domain-api/src/Domain/Constants` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
