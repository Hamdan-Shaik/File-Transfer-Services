---
type: Generation Brief
title: 'Brief: component backend-business-command-handlers-2'
description: Instructions the tool gave the writer session for the backend-business-command-handlers-2 component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-command-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T06:11:41Z
sources:
- id: inventory
  resource: /technical/components/backend-business-command-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-command-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 25
  owned_rules: 0
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  depends_on:
  - backend-business-command-handlers
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - shared-contracts-commands
  deployable: fxfer-domain-api
  folder: src/Application/Commands
  body_sha256: 3f76d79e40fe19002a991fda25044590c125ac14f2122eec3f1e3416be28ffa1
---

# Brief: component `backend-business-command-handlers-2`

- HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers (backend tier, 5 classes, 25 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-business-command-handlers-2/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-business-command-handlers-2/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Application/Commands`: write `services/fxfer-domain-api/src/Application/Commands` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

- `backend-business-command-handlers`: `technical/components/backend-business-command-handlers/`
- `backend-data-access-event-handlers`: `technical/components/backend-data-access-event-handlers/`
- `backend-data-access-event-handlers-2`: `technical/components/backend-data-access-event-handlers-2/`
- `shared-contracts-commands`: `technical/components/shared-contracts-commands/`

## Flows it takes part in

- `configure-source-locations-and-destinations`: `technical/flows/configure-source-locations-and-destinations/` (FLOW.md, RULES.md) and its scenarios `business/configure-source-locations-and-destinations.feature`
- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`
- `manage-integration-files`: `technical/flows/manage-integration-files/` (FLOW.md, RULES.md) and its scenarios `business/manage-integration-files.feature`

## Packages it calls (contracts to keep)

- `automapper`
- `hpp.core.bas.cqrs`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
