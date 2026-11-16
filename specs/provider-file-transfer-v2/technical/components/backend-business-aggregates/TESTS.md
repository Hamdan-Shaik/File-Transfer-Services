---
type: Test Plan
title: 'Tests: backend-business-aggregates'
description: The tests that prove the backend-business-aggregates component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-aggregates
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:26:50Z
sources:
- id: inventory
  resource: /technical/components/backend-business-aggregates/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-aggregates
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 8
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain
  body_sha256: 18e70afe461f38251b15349ce0c34c5ee1999f12c03727f273cde39ef963a85e
---

# Tests: backend-business-aggregates

This component owns no business rules. Tests focus on domain-type parity and mapping-profile behavior.

## Contract tests

- openapi_no_http_surface_is_declared
  - Given `technical/components/backend-business-aggregates/openapi.yaml`
  - Then `x-no-http-surface` is `true`
  - And `paths` is empty (`{}`)

- contract_offers_expected_types
  - Given `technical/components/backend-business-aggregates/CONTRACT.md`
  - Then it lists `FtsSourceLocationState`, `InboundTargetProfile`, and `IntegrationFileProfile` under `Offers`

## Domain type tests

- fts_source_location_state_constructor_sets_id
  - Given `new FtsSourceLocationState(<id>)`
  - Then `Id` equals `<id>`

- fts_source_location_state_constructor_sets_id_and_version
  - Given `new FtsSourceLocationState(<id>, 7)`
  - Then `Id` equals `<id>`
  - And `Version` equals `7`

## Mapping profile tests

- inbound_target_profile_maps_fte_metadata_updated_to_add_ftsinboundtarget_command
  - Given an `FTEMetadataUpdated` event with `UserID`, `ModuleName`, `ImportJobName`, `FTEMetadataId`
  - When mapping to `AddFtsInboundTargetCommand`
  - Then the command fields match the legacy mapping (including `Requestor.IdentifierId`)

- inbound_target_profile_maps_fte_metadata_invalidated_to_invalidate_ftsinboundtarget_command
  - Given an `FTEMetadataInvalidated` event with `UserID`, `FTEMetadataId`
  - When mapping to `InvalidateFtsInboundTargetCommand`
  - Then the command fields match the legacy mapping (including `Requestor.IdentifierId`)

- integration_file_profile_maps_extract_file_generated_to_add_integration_file_command
  - Given an `ExtractFileGenerated` event with `UserID`, `MetadataId`, `FileDirectory`, and `GroupInfo[]`
  - When mapping to `AddIntegrationFileCommand`
  - Then `ETFOutboundMetadataDefnId` equals `MetadataId`
  - And `FileName` equals `FileDirectory`
  - And `GroupInfo` contains one entry per source entry (same `GroupId` and `NumberOfRecords`)

- integration_file_profile_set_group_info_throws_when_group_info_is_null
  - Given an `ExtractFileGenerated` event with `GroupInfo = null`
  - When mapping (or calling `SetGroupInfo`)
  - Then the mapping throws (legacy behavior: foreach over null)

- integration_file_profile_maps_summary_report_generated_to_add_integration_file_command
  - Given a `SummaryReportGenerated` event with `UserID`, `FTEMetadataId`, `FileName`, `GroupId`, `NumberOfRecords`
  - When mapping to `AddIntegrationFileCommand`
  - Then `ETFOutboundMetadataDefnId` equals `FTEMetadataId`
  - And `FileName` equals `FileName`
  - And `GroupInfo` contains exactly one entry with `GroupId` and `NumberOfRecords`

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Variation 1: Contract-First (Design-First) - **Recommended for Gainwell**` (contract artifacts are tested as source-of-truth).
- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Command Flow (Write Path)` (event-to-command mapping is validated on the command/write path).

