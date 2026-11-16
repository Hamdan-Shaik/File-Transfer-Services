---
type: Test Plan
title: 'Tests: shared-queries-parameters-2'
description: The tests that prove the shared-queries-parameters-2 component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-queries-parameters-2
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:47:34Z
sources:
- id: inventory
  resource: /technical/components/shared-queries-parameters-2/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-queries-parameters-2
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 3
  methods: 2
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Queries
  body_sha256: 980a4a965141de1546a7dddbd6a846a5d8003f11770fc89a47f6f29c19eac5b2
---

# Tests: shared-queries-parameters-2

This component owns no business rules. Tests validate default-value behavior for the contract types and validate the contract artifacts.

## Method tests

- integration_file_query_parms_defaults_integration_file_id_to_empty_guid
  - Given an IntegrationFileQueryParms created via the default constructor
  - Then FTSIntegrationFileId == Guid.Empty

- get_integration_file_query_constructor_initializes_where
  - Given a GetIntegrationFileQuery created via the default constructor
  - Then Where is not null
  - And Where.FTSIntegrationFileId == Guid.Empty

- get_integration_file_query_validate_is_noop
  - Given a GetIntegrationFileQuery with default Where
  - When Validate() is called
  - Then it does not throw

## Contract tests

- openapi_no_http_surface_is_declared
  - Given 	echnical/components/shared-queries-parameters-2/openapi.yaml
  - Then x-no-http-surface is 	rue
  - And paths is empty ({})

- contract_md_is_present_and_declares_no_external_dependencies
  - Given 	echnical/components/shared-queries-parameters-2/CONTRACT.md
  - Then it declares Uses: None
  - And Data owned: None

## Enterprise-Architecture standards applied

- patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md section Variation 1: Contract-First (Design-First) - **Recommended for Gainwell** (contract artifacts are validated and tested as the source of truth).
- patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md section Solution Overview (validation/parsing behavior is asserted consistently so callers can rely on stable error handling).

