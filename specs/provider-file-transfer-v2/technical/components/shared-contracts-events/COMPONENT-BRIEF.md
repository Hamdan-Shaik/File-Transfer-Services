---
type: Generation Brief
title: 'Brief: component shared-contracts-events'
description: Instructions the tool gave the writer session for the shared-contracts-events component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-events
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:28:25Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-events/INVENTORY.yaml
  title: Every legacy class and method (15 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-events
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 15
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Events
  body_sha256: ba48a4e76983735f6ab984aa874e043d12cc0bf5828282de437ff85fa9fd0f8a
---

# Brief: component `shared-contracts-events`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events (shared tier, 15 classes, 0 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-contracts-events/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-contracts-events/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Contracts/Events`: write `services/fxfer-domain-api/src/Contracts/Events` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.

## Target-state requirements it owns (1)

No legacy code does these; the target state asks for them. Specify each in SPEC.md under its id, with the section it comes from, and give each a test in TESTS.md.

- **T-006** Publish tenant-aware, schema-versioned domain events using the canonical event envelope (tenant_id, correlation_id, trace_id, producer, etc.). (target state: 7.2 Event Types)
