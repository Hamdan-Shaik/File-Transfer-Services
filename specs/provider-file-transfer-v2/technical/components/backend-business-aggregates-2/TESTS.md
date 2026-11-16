---
type: Test Plan
title: 'Tests: backend-business-aggregates-2'
description: The tests that prove the backend-business-aggregates-2 component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-aggregates-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:36:00Z
sources:
- id: inventory
  resource: /technical/components/backend-business-aggregates-2/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-aggregates-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 3
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain
  body_sha256: d869075e88d7553ab6d61014ba9d5fb7b6d82f0112d68e5d7e681a14c6dd8c5f
---

# Tests: backend-business-aggregates-2

This component owns no business rules. Tests focus on domain-type parity.

## Contract tests

- openapi_no_http_surface_is_declared
  - Given `technical/components/backend-business-aggregates-2/openapi.yaml`
  - Then `x-no-http-surface` is `true`
  - And `paths` is empty (`{}`)

- contract_offers_expected_types
  - Given `technical/components/backend-business-aggregates-2/CONTRACT.md`
  - Then it lists `FtsSourceLocationState` under `Offers`

## Domain type tests

- fts_source_location_state_constructor_sets_id
  - Given `new FtsSourceLocationState(<id>)`
  - Then `Id` equals `<id>`

- fts_source_location_state_constructor_sets_id_and_version
  - Given `new FtsSourceLocationState(<id>, 7)`
  - Then `Id` equals `<id>`
  - And `Version` equals `7`

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Variation 1: Contract-First (Design-First) - **Recommended for Gainwell**` (contract artifacts are tested as source-of-truth).
- `patterns/application/api-microservices/PTN-APP-API-008-domain-driven-design-pattern.md` section `2. Aggregates: Consistency Boundaries` (aggregate-related types are validated at the domain contract level).

