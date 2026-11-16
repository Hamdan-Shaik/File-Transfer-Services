---
type: Test Plan
title: technical/components/shared-queries-parameters/TESTS
description: The tests that prove the shared-queries-parameters component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-queries-parameters
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:38:34Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-queries-parameters/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-queries-parameters
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 0
  owned_rules: 3
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Queries
  body_sha256: c78c3a7e7d2a04a32fb525cdd505a1e65a1605dc9f60ca7f13054d67035e1f97
---

﻿# Tests: `shared-queries-parameters`

This component owns three business rules (F01-101, F01-104, F05-031). Tests also validate the contract artifacts and the presence of required parameter types.

## Rule tests

- `F01_101_parses_source_location_id_as_guid_for_details_lookup`
  - Given `GetSourceLocationQueryParms.SourceLocationId` is a non-GUID string
  - When `GetSourceLocationQueryParms.SoureceLocationGuid` is evaluated
  - Then it throws the `Guid.Parse` exception surfaced by .NET for invalid GUID strings

  - Given `GetSourceLocationQueryParms.SourceLocationId` is a valid GUID string
  - When `SoureceLocationGuid` is evaluated
  - Then it equals `Guid.Parse(SourceLocationId)`

- `F01_104_details_lookup_does_not_filter_by_requested_is_active`
  - Given a stored source location exists for the requested id and has `IsValid = false`
  - And the request parameters set `GetSourceLocationQueryParms.IsActive = true`
  - When the source location details lookup is executed
  - Then the record is still returned
  - And the returned model’s `IsActive` reflects the stored active flag (`sourceLocation.IsValid`), not the requested filter value

- `F05_031_inbound_target_query_params_default_fte_metadata_id_to_empty_guid`
  - Given `new InboundTargetQueryParms()`
  - Then `FTEMetadataId == Guid.Empty`

## Contract tests

- `openapi_no_http_surface_is_declared`
  - Given `technical/components/shared-queries-parameters/openapi.yaml`
  - Then `x-no-http-surface` is `true`
  - And `paths` is empty (`{}`)

- `contract_md_is_present_and_declares_no_external_dependencies`
  - Given `technical/components/shared-queries-parameters/CONTRACT.md`
  - Then it declares `Uses: None`
  - And `Data owned: None`

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Variation 1: Contract-First (Design-First) - **Recommended for Gainwell**` (contract artifacts are validated and tested as the source of truth).
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (validation/parsing errors are asserted consistently; callers can rely on stable error handling).