---
type: Generation Brief
title: 'Brief: component backend-data-access-event-handlers-2'
description: Instructions the tool gave the writer session for the backend-data-access-event-handlers-2 component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-event-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:06:12Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-event-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-event-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 4
  methods: 18
  owned_rules: 0
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-projection-worker
  folder: src/Infrastructure/Projections
  body_sha256: d4f9c3e692783963030f1f705b98ffa29d53b4ad4d1f4142304df4c12e85e40f
---

# Brief: component `backend-data-access-event-handlers-2`

- HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers (backend tier, 4 classes, 18 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-data-access-event-handlers-2/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-data-access-event-handlers-2/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-projection-worker` (`workers/fxfer-projection-worker`), folder `src/Infrastructure/Projections`: write `workers/fxfer-projection-worker/src/Infrastructure/Projections` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-projection-worker/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

- `configure-source-locations-and-destinations`: `technical/flows/configure-source-locations-and-destinations/` (FLOW.md, RULES.md) and its scenarios `business/configure-source-locations-and-destinations.feature`
- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`

## Packages it calls (contracts to keep)

- `automapper`
- `hpp.core.bas.cqrs`
- `microsoft.entityframeworkcore`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
