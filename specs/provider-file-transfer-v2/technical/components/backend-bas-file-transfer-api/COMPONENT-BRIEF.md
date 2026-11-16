---
type: Generation Brief
title: 'Brief: component backend-bas-file-transfer-api'
description: Instructions the tool gave the writer session for the backend-bas-file-transfer-api component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-bas-file-transfer-api
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:01:04Z
sources:
- id: inventory
  resource: /technical/components/backend-bas-file-transfer-api/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-bas-file-transfer-api
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 10
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Api
  body_sha256: 635ee6beaa9abe6eb4e5562b96e931d4e458986fa6592f10b1efce7a9159dd5a
---

# Brief: component `backend-bas-file-transfer-api`

- HPP.FileTransfer.BAS.FileTransferApi (backend tier, 6 classes, 10 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-bas-file-transfer-api/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-bas-file-transfer-api/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Api`: write `services/fxfer-domain-api/src/Api` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Packages it calls (contracts to keep)

- `autofac`
- `hpp.core.bas.cqrs`
- `swashbuckle.aspnetcore.swagger`
- `swashbuckle.aspnetcore.swaggergen`
- `swashbuckle.aspnetcore.swaggerui`

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.

## Target-state requirements it owns (1)

No legacy code does these; the target state asks for them. Specify each in SPEC.md under its id, with the section it comes from, and give each a test in TESTS.md.

- **T-005** Emit OpenTelemetry traces/metrics/logs from services and workers tagged with tenant_id, correlation_id, and trace_id, and propagate these values end-to-end. (target state: 12.5 Observability)
