---
type: Test Plan
title: 'Tests: ui-file-transfer-api-controllers'
description: The tests that prove the ui-file-transfer-api-controllers component, one or more per business rule.
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
  body_sha256: 8e4945ffd4db64af893db3f3ad50b98cbb5863ef68418c7431f626b3acc6b512
---

# Tests: `ui-file-transfer-api-controllers`

This component owns no business rules; its tests focus on BFF entry-point trust-boundary enforcement and spec/contract validation.

## Target-state requirements

### T-001

- `T_001_derives_tenant_scope_from_jwt_claim_only`
  - Given an authenticated request with a validated JWT containing `tenant_id=tenant-a`
  - And a request body and/or headers attempting to supply a different tenant value
  - When the request is processed by `fxfer-bff`
  - Then the resolved tenant scope is `tenant-a`
  - And the request is rejected if the `tenant_id` claim is missing/empty

### T-002

- `T_002_bff_is_stateless_bearer_jwt_and_enforces_cors_and_origin_validation`
  - Given a browser request to a state-changing endpoint with `Authorization: Bearer <JWT>`
  - When `Origin`/`Referer` is not in the allow-list
  - Then `fxfer-bff` returns `403 Forbidden` (Problem Details)
  - And when `Origin`/`Referer` is allow-listed, the request proceeds without creating any server-side session

## Composition and wiring

- `program_wires_authentication_tenant_context_and_origin_validation`
  - Given the service provider built by `services/fxfer-bff/src/Api/Program.cs`
  - Then required middleware/services are registered and ordered so that tenant resolution occurs after authentication and before endpoint execution

## Contract tests

- `openapi_yaml_is_valid_openapi_document`
  - Validate `technical/components/ui-file-transfer-api-controllers/openapi.yaml` is syntactically valid OpenAPI 3.0.

- `contract_md_declares_no_external_dependencies`
  - Validate `technical/components/ui-file-transfer-api-controllers/CONTRACT.md` declares no used components and no owned data.

