---
type: Test Plan
title: 'Tests: shared-contracts-queries'
description: The tests that prove the shared-contracts-queries component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-queries
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:46:37Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-contracts-queries/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-queries
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 5
  owned_rules: 1
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Queries
  body_sha256: d9713ec4f2e454662bb4f740c79c442acb9fe22e80e4e2c8e2086f54dc290962
---

# Tests: `shared-contracts-queries`

This component owns one validation business rule (F01-100). Tests also validate contract artifacts and the presence of required query contract types.

## Rule tests

- `F01_100_requires_non_empty_source_location_id`
  - Given `GetSourceLocationQuery.Where.SourceLocationId` is null or empty
  - When `GetSourceLocationQuery.Validate()` is called
  - Then it throws `FileTransferValidationException` with validation message key `FtpSoureceLocationId`

## Method tests

- `get_integration_file_query_validate_is_noop`
  - Given a `GetIntegrationFileQuery` with default `Where`
  - When `Validate()` is called
  - Then it does not throw

- `inbound_target_query_validate_is_noop`
  - Given an `InboundTargetQuery` with default `Where`
  - When `Validate()` is called
  - Then it does not throw

## Contract tests

- `openapi_no_http_surface_is_declared`
  - Given `technical/components/shared-contracts-queries/openapi.yaml`
  - Then `x-no-http-surface` is `true`
  - And `paths` is empty (`{}`)

- `contract_md_is_present_and_declares_no_external_dependencies`
  - Given `technical/components/shared-contracts-queries/CONTRACT.md`
  - Then it declares `Uses: None`
  - And `Data owned: None`

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Variation 1: Contract-First (Design-First) - **Recommended for Gainwell**` (contract artifacts are validated and tested as the source of truth).
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (validation errors are asserted consistently; callers can rely on stable error codes/messages).

