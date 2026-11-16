---
type: Test Plan
title: 'Tests: backend-business-profiles'
description: The tests that prove the backend-business-profiles component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-profiles
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:23:32Z
sources:
- id: inventory
  resource: /technical/components/backend-business-profiles/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-profiles
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 6
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain/Profiles
  body_sha256: 9a05f9d665d5cb505806c3f2a5b0e3630b760113f3fe232523272c8d4cafc343
---

# Tests: backend-business-profiles

This component owns no business rules. Tests focus on mapping-profile behavior and contract artifacts.

## Contract tests

- openapi_no_http_surface_is_declared
  - Given `technical/components/backend-business-profiles/openapi.yaml`
  - Then `x-no-http-surface` is `true`
  - And `paths` is empty (`{}`)

- contract_offers_expected_profiles
  - Given `technical/components/backend-business-profiles/CONTRACT.md`
  - Then it lists `FtpServerCommandHandlerProfile`, `FtpServerProjectionWriterProfile`, `FtsInboundTargetProjectionWriterProfile`, `FtsIntegrationFileProjectionWriterProfile`, `InboundTargetProfile`, and `IntegrationFileProfile` under `Offers`

## Mapping profile tests

- ftp_server_command_handler_profile_maps_add_command_to_ftp_server_added_event
  - Given an `AddFtpServerCommand` with representative non-null fields
  - When mapping to `FtpServerAdded`
  - Then the event fields match the legacy mapping

- ftp_server_command_handler_profile_does_not_overwrite_destination_members_when_source_member_is_null
  - Given an `AddFtpServerCommand` where an optional member is null
  - And an existing destination `FtpServerAdded` instance with that destination member pre-populated
  - When mapping with `Map(source, destination)`
  - Then the pre-populated destination value remains unchanged (legacy behavior: `Condition(srs => srs != null)`)

- ftp_server_command_handler_profile_maps_update_command_to_ftp_server_updated_event
  - Given an `UpdateFtpServerCommand` with representative non-null fields
  - When mapping to `FtpServerUpdated`
  - Then the event fields match the legacy mapping

- ftp_server_projection_writer_profile_maps_ftp_server_added_to_ftp_server_entity
  - Given a `FtpServerAdded` event with representative fields
  - When mapping to `FtpServer`
  - Then the entity fields match the legacy mapping

- fts_inbound_target_projection_writer_profile_maps_inbound_target_added_to_entity
  - Given a `FtsInboundTargetAdded` event with representative fields
  - When mapping to `FtsInboundTarget`
  - Then the entity fields match the legacy mapping

- fts_integration_file_projection_writer_profile_maps_integration_file_added_to_entity
  - Given an `IntegrationFileAdded` event with representative fields
  - When mapping to `FtsIntegrationFile`
  - Then the entity fields match the legacy mapping

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
- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Command Flow (Write Path)` (command-side mapping is validated as part of the write path).

