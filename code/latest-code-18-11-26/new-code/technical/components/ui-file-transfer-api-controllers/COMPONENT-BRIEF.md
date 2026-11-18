---
type: Generation Brief
title: 'Brief: component ui-file-transfer-api-controllers'
description: Instructions the tool gave the writer session for the ui-file-transfer-api-controllers component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/ui-file-transfer-api-controllers
tags:
- component
- provider-file-transfer-v2
- ui
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T00:17:30Z
sources:
- id: inventory
  resource: /technical/components/ui-file-transfer-api-controllers/INVENTORY.yaml
  title: Every legacy class and method (1 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:ui-file-transfer-api-controllers
  generation:
    status: done
    attempts: 1
  tier: ui
  classes: 1
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-bff
  folder: src/Api
  body_sha256: 70ed5f2e72c732d451dc48fb236099440944cc0c39ee42afcfcc4eebb8113911
---

# Brief: component `ui-file-transfer-api-controllers`

- HPP.FileTransfer.BAS.FileTransferApi.Controllers (ui tier, 1 classes, 0 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/ui-file-transfer-api-controllers/INVENTORY.yaml`
- Write exactly these files in `technical/components/ui-file-transfer-api-controllers/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-bff` (`services/fxfer-bff`), folder `src/Api`: write `services/fxfer-bff/src/Api` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-bff/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (0)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

None: it serves the components that own rules.

## Target-state requirements it owns (2)

No legacy code does these; the target state asks for them. Specify each in SPEC.md under its id, with the section it comes from, and give each a test in TESTS.md.

- **T-001** Derive tenant scope from verified identity context (JWT claim tenant_id) and never from request bodies or caller-supplied headers. (target state: 10.1 Entry Point and Trust Boundaries)
- **T-002** Implement the BFF as stateless bearer-JWT APIs (no server-side sessions/cookies) and enforce strict CORS plus Origin/Referer checks for state-changing requests. (target state: 0.5 Decision Status (UI authentication + CSRF posture))
