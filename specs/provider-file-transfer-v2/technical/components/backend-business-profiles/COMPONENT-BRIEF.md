---
type: Generation Brief
title: 'Brief: component backend-business-profiles'
description: Instructions the tool gave the writer session for the backend-business-profiles component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-profiles
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:23:32Z
sources:
- id: inventory
  resource: /technical/components/backend-business-profiles/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-profiles
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 6
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain/Profiles
  body_sha256: c8d1df834637c3014ee099addfa5f3e785b90c96a203e37b14b8a90ee47a3b49
---

# Brief: component `backend-business-profiles`

- HPP.FileTransfer.BAS.FileTransferSvc.Business.Profiles (backend tier, 6 classes, 6 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-business-profiles/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-business-profiles/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Domain/Profiles`: write `services/fxfer-domain-api/src/Domain/Profiles` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Packages it calls (contracts to keep)

- `automapper`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.
