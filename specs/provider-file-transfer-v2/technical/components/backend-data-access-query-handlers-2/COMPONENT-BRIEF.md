---
type: Generation Brief
title: 'Brief: component backend-data-access-query-handlers-2'
description: Instructions the tool gave the writer session for the backend-data-access-query-handlers-2 component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-query-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T01:10:43Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-query-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-query-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 8
  methods: 38
  owned_rules: 0
  flows: []
  depends_on:
  - shared-contracts-queries
  - shared-queries-parameters-2
  deployable: fxfer-domain-api
  folder: src/Application/Queries
  body_sha256: c32d7408eab17b92ea1d87bfe795c9e401e592524c78e67c7b0baf48e9c4be51
---

# Brief: component `backend-data-access-query-handlers-2`

- HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.QueryHandlers (backend tier, 8 classes, 38 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-data-access-query-handlers-2/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-data-access-query-handlers-2/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Application/Queries`: write `services/fxfer-domain-api/src/Application/Queries` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

- `shared-contracts-queries`: `technical/components/shared-contracts-queries/`
- `shared-queries-parameters-2`: `technical/components/shared-queries-parameters-2/`

## Flows it takes part in

None.

## Packages it calls (contracts to keep)

- `hpp.core.bas.cqrs`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
