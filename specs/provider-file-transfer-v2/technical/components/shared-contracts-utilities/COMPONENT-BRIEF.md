---
type: Generation Brief
title: 'Brief: component shared-contracts-utilities'
description: Instructions the tool gave the writer session for the shared-contracts-utilities component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-utilities
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:55:00Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-utilities/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-utilities
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 4
  methods: 6
  owned_rules: 0
  flows:
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Utilities
  body_sha256: 05565c1f31bd92cb3a49a69844e0aa40df1f51d19025e7ea4e0cd480b2cb55f6
---

# Brief: component `shared-contracts-utilities`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities (shared tier, 4 classes, 6 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-contracts-utilities/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-contracts-utilities/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Contracts/Utilities`: write `services/fxfer-domain-api/src/Contracts/Utilities` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`

## Packages it calls (contracts to keep)

- `hpp.core.bas.cqrs`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
