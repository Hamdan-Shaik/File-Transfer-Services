---
type: Generation Brief
title: 'Brief: component backend-file-transfer-constants-localization'
description: Instructions the tool gave the writer session for the backend-file-transfer-constants-localization component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-constants-localization
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:44:39Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-constants-localization/INVENTORY.yaml
  title: Every legacy class and method (7 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-constants-localization
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 7
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants/localization
  body_sha256: cf9682e7e5a956448ee483f787fa5e0598363180237dfab9b8395146bf655096
---

# Brief: component `backend-file-transfer-constants-localization`

- HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization (backend tier, 7 classes, 0 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-file-transfer-constants-localization/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-file-transfer-constants-localization/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-mfe` (`apps/fxfer-mfe`), folder `src/legacy-constants/localization`: write `apps/fxfer-mfe/src/legacy-constants/localization` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-mfe/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
