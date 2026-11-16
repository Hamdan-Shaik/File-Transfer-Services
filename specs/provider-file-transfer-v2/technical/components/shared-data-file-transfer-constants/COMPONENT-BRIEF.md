---
type: Generation Brief
title: 'Brief: component shared-data-file-transfer-constants'
description: Instructions the tool gave the writer session for the shared-data-file-transfer-constants component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-data-file-transfer-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:02:16Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-data-file-transfer-constants/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-data-file-transfer-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 3
  methods: 0
  owned_rules: 1
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants
  body_sha256: 67c1296f4a75e68b9333fc9d8727e5fc48067fd21eea6b1396348da2ee07c331
---

# Brief: component `shared-data-file-transfer-constants`

- HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants (shared tier, 3 classes, 0 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-data-file-transfer-constants/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-data-file-transfer-constants/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-mfe` (`apps/fxfer-mfe`), folder `src/legacy-constants`: write `apps/fxfer-mfe/src/legacy-constants` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-mfe/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (1)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-017** The configuration run schedule is set to monthly (`RunSchedule.ID = 2`) when any of these are true: `RunOnLastDay`, `RunOnFirstDay`, or `RunOnThisDay.ID` is not empty and not equal to 0; otherwise it is weekly (`RunSchedule.ID = 1`).
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:500-505, ux\HP.HSP.UA3.FileTransfer.UX.Data\FileTransferConstants.cs:95-100
- Legacy: The schedule is inferred from the day-of-month fields and stored as a string id (1 or 2).
- Decision: keep

