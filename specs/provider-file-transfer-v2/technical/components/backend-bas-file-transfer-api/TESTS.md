---
type: Test Plan
title: 'Tests: backend-bas-file-transfer-api'
description: The tests that prove the backend-bas-file-transfer-api component, one or more per business rule.
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
  body_sha256: 67b5db59bfd09aa8fdb1c481a77711d327bd14ca886595c7fe267685044e632c
---

# Tests: `backend-bas-file-transfer-api`

This component owns no business rules; its tests focus on API-host wiring, observability propagation, and spec/contract validation.

## Target-state requirements

### T-005

- `T_005_emits_and_propagates_otel_tags`
  - Given an inbound HTTP request containing `traceparent` and `X-Correlation-ID` headers and an authenticated identity with a `tenant_id` claim
  - When the request is processed by `fxfer-domain-api`
  - Then the emitted trace/span(s) and logs/metrics are taggable with `tenant_id`, `correlation_id`, and `trace_id`
  - And the service propagates W3C Trace Context end-to-end for outbound calls (where applicable in the request path)

## Composition and wiring

- `program_wires_exception_handling_and_routing_pipeline`
  - Given a request that triggers an exception inside an endpoint
  - When the request is executed
  - Then the API returns the target-state error format and does not leak stack traces in non-development environments

- `di_registration_contains_required_api_services`
  - Given the service provider built by `Program.cs`
  - Then expected API-layer services/middleware can be resolved (exception handling middleware, correlation/tenant middleware)

## Contract tests

- `openapi_yaml_is_valid_openapi_document`
  - Validate `technical/components/backend-bas-file-transfer-api/openapi.yaml` is syntactically valid OpenAPI 3.0.

- `contract_md_is_present_and_declares_no_external_dependencies`
  - Validate `technical/components/backend-bas-file-transfer-api/CONTRACT.md` declares no used components and no owned data.

