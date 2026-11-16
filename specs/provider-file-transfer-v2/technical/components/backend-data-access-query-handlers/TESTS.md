---
type: Test Plan
title: 'Tests: backend-data-access-query-handlers'
description: The tests that prove the backend-data-access-query-handlers component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-query-handlers
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T01:03:24Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-manage-integration-files
  resource: /technical/flows/manage-integration-files/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-data-access-query-handlers/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-query-handlers
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 8
  methods: 43
  owned_rules: 37
  flows: []
  depends_on:
  - shared-contracts-queries
  deployable: fxfer-domain-api
  folder: src/Application/Queries
  body_sha256: cdbd3626fdffbbc47930353d83b7a4d43b3ed53f1c32bdd9290e6cec9a64ca8a
---

# Tests: backend-data-access-query-handlers

This component has no standalone HTTP surface; tests focus on query execution semantics, filtering/sorting behavior, and owned business rules.

Create unit tests using an EF Core test database (SQLite in-memory or EF Core InMemory provider) and seed minimal rows required for each query.

## Unit tests (owned rules)

### Source location details query rules (F01-014, F01-015, F01-016, F01-020, F01-102, F01-103, F01-105, F01-106)

- `F01_014_returns_null_when_zero_or_multiple_source_location_rows_match`
  - Given `GetSourceLocationQuery.Where.SourceLocationId` and the DB contains 0 matching rows, when executed, then the executor returns no source location.
  - Given 2 matching rows for the same id (data anomaly), when executed, then the executor returns no source location.

- `F01_015_details_model_maps_is_active_and_suspended_from_source_location_row`
  - Given a matching `fts_source_location` row with `is_valid` and `is_suspended`, when executed, then the returned `SourceLocationModel` has `IsActive` and `Suspended` set from those fields.

- `F01_016_destinations_are_ordered_by_creation_date_desc`
  - Given multiple destination rows linked to the source location with distinct `creation_date`, when executed, then `DestinationAddress` is ordered newest-first.

- `F01_020_import_job_name_is_taken_from_first_inbound_target_matching_fte_metadata_id`
  - Given a destination with a non-null `fte_metadata_id` and multiple inbound-target rows for that metadata id, when executed, then destination `ImportJobName` equals the first match in the in-memory list ordering.

- `F01_102_details_query_eager_loads_destinations_and_inbound_targets`
  - Given the details query is executed, then destination/inbound-target rows are loaded into memory before mapping destinations (verify via one DB query per table or by isolating the mapping method inputs).

- `F01_103_ftp_server_name_is_included_only_when_source_location_has_ftp_server_id`
  - Given a source location with `ftp_server_id=null`, when executed, then `FtpServerName` is null.
  - Given a source location with a non-null ftp server id, when executed, then `FtpServerName` equals the matched FTP server row’s name.

- `F01_105_destination_address_is_assigned_on_the_first_result_only`
  - Given a details query that returns one result, when executed, then `Results.First().DestinationAddress` is populated.

- `F01_106_destination_dto_fields_include_import_job_name_lookup_by_fte_metadata_id`
  - Given destinations linked to the source location, when executed, then destination DTOs include `ImportJobName` derived from inbound targets and preserve ordering.

### Source location search query rules (F01-091..F01-099)

- `F01_091_search_projects_source_locations_into_source_location_model`
  - Given `fts_source_location` rows, when executed, then each result includes `ConfigId`, `ConfigName`, `OwnerName`, `OwnerSubsystem`, `Protocol`, `Address`, `Mask`, `IsActive`.

- `F01_092_when_config_id_gt_zero_search_filters_to_that_config_id`
  - Given `Where.ConfigId=123`, when executed, then all results have `ConfigId==123`.

- `F01_093_owner_name_filter_is_case_insensitive_exact_match`
  - Given `Where.OwnerName` set to a value with different casing than stored, when executed, then matching rows are returned.

- `F01_094_config_name_filter_is_case_insensitive_exact_match`
  - Given `Where.ConfigName` set to a value with different casing than stored, when executed, then matching rows are returned.

- `F01_095_owner_subsystem_is_parsed_as_guid_and_filtered_by_owner_module_guid`
  - Given `Where.OwnerSubsystem` set to a valid GUID string, when executed, then results are filtered to the matching `OwnerSubsystem` GUID value.

- `F01_096_protocol_filter_is_exact_match`
  - Given `Where.Protocol` is set, when executed, then only rows with that protocol code are returned.

- `F01_097_is_active_filter_is_applied_only_when_is_active_true`
  - Given `Where.IsActive=true`, when executed, then only active rows are returned.
  - Given `Where.IsActive=false`, when executed, then both active and inactive rows may be returned (no filter).

- `F01_098_default_sort_is_config_id_asc_then_is_active_desc`
  - Given no sort is requested, when executed, then results are ordered by `ConfigId` ascending and `IsActive` descending.

- `F01_099_results_are_materialized_to_a_list`
  - Given a query that returns N rows, when executed, then the returned list contains exactly N rows.

### FTP server query rules (F02-005..F02-009)

- `F02_005_is_valid_filter_is_applied_only_when_where_is_valid_is_not_null`
  - Given `Where.IsValid=null`, when executed, then both valid and invalid rows may be returned.
  - Given `Where.IsValid=true`, when executed, then only valid rows are returned.

- `F02_006_legacy_door_defaults_sort_by_ftp_server_name_ascending`
  - Given the legacy executor is used and no sort criteria is provided, when executed, then results are sorted by `FtpServerName` ascending.

- `F02_007_legacy_door_filters_by_ftp_server_id_only_for_equal_to_mode`
  - Given `Where.FtpServerId` is non-empty and the id search mode is `EqualTo`, when executed, then only the matching row is returned.

- `F02_008_legacy_door_applies_search_mode_matching_for_name_description_and_user`
  - Given `Contains` / `StartsWith` / `EndsWith` / `EqualTo` search modes for `FtpServerName`, `FtpDescription`, and `FtpUser`, when executed, then string matching is case-insensitive and uses `ToUpper().Trim()` semantics.

- `F02_009_ftp_server_results_project_credential_related_fields`
  - Given a matching row, when executed, then the returned model includes username, host/ip, ports/proxy fields, and the credential reference field used by the target system.

### Integration file lookup rules (F03-005..F03-009)

- `F03_005_only_active_integration_file_definitions_are_returned`
  - Given both valid and invalid integration file rows, when executed, then only valid rows are returned.

- `F03_006_id_filter_is_applied_only_when_id_is_not_empty`
  - Given `Where.FTSIntegrationFileId=Guid.Empty`, when executed, then results are not filtered by id.
  - Given a specific id, when executed, then only that row is returned.

- `F03_007_file_name_fragment_filter_uses_contains_when_fragment_is_provided`
  - Given `Where.FileName='abc'`, when executed, then only rows whose stored `FileName` contains `abc` are returned.

- `F03_008_lookup_applies_no_explicit_ordering`
  - Given multiple matching rows, when executed, then the test asserts only set equality (do not assert ordering).

- `F03_009_lookup_populates_only_subset_of_model_fields`
  - Given a matching row, when executed, then `GroupId`, `ETFOutboundMetadataDefnId`, `ConfigId`, `FileName`, `IsProcessed`, `FtsIntegrationFileId` are populated and other model fields remain default values.

### Tracking search rules (F05-023..F05-026)

- `F05_023_filters_apply_only_when_where_fields_are_present_and_ranges_are_inclusive`
  - Given combinations of `Where` fields (`FileName`, `FileStatus`, `TradingPartner`, date and size ranges), when executed, then filters apply only for provided fields and date/size ranges are inclusive.
  - Include a case where `Where.FileId` is non-empty but not parseable as a GUID; verify the filter uses `Guid.Empty`.

- `F05_024_sorting_supports_configured_keys_and_defaults_to_file_name_ascending`
  - Given no sort is requested, when executed, then results are ordered by `FileName` ascending.

- `F05_025_base_results_union_unc_and_ftp_cases_and_use_only_valid_source_locations`
  - Given valid and invalid source locations, when executed, then only valid source locations contribute results.
  - Verify UNC rows have `OriginatingServerName==string.Empty` and FTP rows have `OriginatingServerName==FtpServerName`.

- `F05_026_each_tracking_result_is_enriched_with_events_and_exceptions`
  - Given N tracking results, when executed, then each result has `Events` and `Exceptions` populated from per-file lookups.

### File event lookup rules (F05-027..F05-028)

- `F05_027_file_event_lookup_filters_by_file_id_when_provided_and_defaults_sort_by_event_id_then_event_code`
  - Given `Where.FileId` is set, when executed, then results are filtered by `FileId`.
  - Given an invalid GUID string that is non-null, verify the filter uses `Guid.Empty`.

- `F05_028_file_event_lookup_projects_fields_and_maps_created_date_from_file_management_created_date`
  - Given joined file management + file event rows, when executed, then event `CreatedDate` equals the related file management `CreatedDate`.

### File exception lookup rules (F05-029..F05-030)

- `F05_029_file_exception_lookup_filters_by_file_id_when_provided_and_defaults_sort_by_exception_id_then_exception_type`
  - Given `Where.FileId` is set, when executed, then results are filtered by `FileId`.
  - Given an invalid GUID string that is non-null, verify the filter uses `Guid.Empty`.

- `F05_030_file_exception_lookup_projects_fields_and_maps_created_date_from_file_management_created_date`
  - Given joined file management + file exception rows, when executed, then exception `CreatedDate` equals the related file management `CreatedDate`.

### Inbound target lookup rules (F05-032..F05-033)

- `F05_032_inbound_target_results_include_only_valid_rows_and_support_optional_filters`
  - Given mixed valid/invalid rows, when executed, then only valid rows are returned.
  - Given `Where.FTEMetadataId != Guid.Empty`, verify results are filtered by that id.
  - Given `Where.ContractName` is set, verify results are filtered by exact contract name.

- `F05_033_inbound_target_results_are_deduped_by_fte_metadata_id_using_smallest_inbound_target_id`
  - Given multiple rows with the same `FTEMetadataId`, when executed, then only one result is returned for that group and it has the smallest `FtsInboundTargetId`.

## Unit tests (non-rule legacy parity)

- Details lookup throws when `AssignResults` calls `baseQuery.First()` on an empty query (parity with legacy `First()` behavior).
- Destination `ImportJobName` lookup throws when `FTEMetadataId` is non-null but no inbound target exists for that metadata id (legacy null dereference parity).

## Contract tests

- `openapi.yaml` contains `x-no-http-surface: true` and has no defined paths.
- `CONTRACT.md` names dependency on component `shared-contracts-queries`.

