---
type: Interface Contract
title: 'Contract: ui-file-transfer-api-controllers'
description: What the ui-file-transfer-api-controllers component offers other components and what it uses from them.
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
  body_sha256: cafd878fbdaa49b5d99a92035114abdb42b5510e2d53e00a9e6ff2473a982af4
---

# Contract: `ui-file-transfer-api-controllers`

This component defines the BFF API-layer entry-point policies for authentication, tenancy resolution, and browser security checks.

## Offers

- Request-scoped tenant context derived from verified identity context (JWT claim `tenant_id`).
- Middleware enforcing stateless bearer-JWT posture (no server-side sessions/cookies for authentication).
- Middleware enforcing browser origin validation for state-changing requests.

## Uses

None (no cross-component dependencies by contract).

## Owned data

None.

