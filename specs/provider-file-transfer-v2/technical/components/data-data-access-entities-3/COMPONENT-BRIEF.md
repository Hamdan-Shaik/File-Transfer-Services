---
type: Generation Brief
title: 'Brief: component data-data-access-entities-3'
description: Instructions the tool gave the writer session for the data-data-access-entities-3 component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/data-data-access-entities-3
tags:
- component
- data
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:32:53Z
sources:
- id: inventory
  resource: /technical/components/data-data-access-entities-3/INVENTORY.yaml
  title: Every legacy class and method (14 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:data-data-access-entities-3
  generation:
    status: done
    attempts: 1
  tier: data
  classes: 14
  methods: 8
  owned_rules: 0
  flows:
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Infrastructure/DataAccess
  body_sha256: 7fe6f74721a16eee8734d885eb5cfb2341b073605538116167c620d5f99cc5be
---

# Brief: component `data-data-access-entities-3`

- HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities (data tier, 14 classes, 8 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/data-data-access-entities-3/INVENTORY.yaml`
- Write exactly these files in `technical/components/data-data-access-entities-3/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Infrastructure/DataAccess`: write `services/fxfer-domain-api/src/Infrastructure/DataAccess` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`

## Packages it calls (contracts to keep)

- `hpp.core.api.logger`
- `hpp.core.bas.cqrs`
- `microsoft.entityframeworkcore`
- `microsoft.entityframeworkcore.relational`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
