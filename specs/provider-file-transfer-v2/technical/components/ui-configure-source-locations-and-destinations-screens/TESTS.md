---
type: Test Plan
title: technical/components/ui-configure-source-locations-and-destinations-screens/TESTS
description: The tests that prove the ui-configure-source-locations-and-destinations-screens component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/ui-configure-source-locations-and-destinations-screens
tags:
- component
- provider-file-transfer-v2
- ui
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T00:05:24Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/ui-configure-source-locations-and-destinations-screens/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:ui-configure-source-locations-and-destinations-screens
  generation:
    status: done
    attempts: 2
  tier: ui
  classes: 3
  methods: 23
  owned_rules: 31
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - search-and-track-file-transfers
  depends_on: []
  deployable: fxfer-mfe
  folder: src/screens/configure-source-locations-and-destinations
  body_sha256: 71f30285bd5dedd486f8ba1175fd963babcd945d149c5f9045a3a051963e52bd
---

﻿# Tests: ui-configure-source-locations-and-destinations-screens

This file lists the tests to write to preserve behavior. Tests are described at this component’s level (UI + BFF contract usage) using Jest + React Testing Library, plus contract tests validating `openapi.yaml` and `CONTRACT.md`.

## Rules (unit/component tests)

- F01_001_file_transfer_configuration_view_loads_and_renders_view_model
- F01_002_file_transfer_configuration_edit_defaults_isActive_true_when_id_missing
- F01_003_destination_configuration_edit_panel_renders_posted_model_unmodified
- F01_004_destination_configuration_edit_renders_posted_model_unmodified
- F01_005_get_file_transfer_configuration_builds_requestor_and_data
- F01_006_get_file_transfer_configuration_resolves_provider_by_component_name
- F01_007_get_file_transfer_configuration_replaces_model_only_on_ok_and_non_null_data
- F01_008_provider_status_is_not_found_until_results_present
- F01_009_provider_calls_get_source_locations_with_where_sourceLocationId_equal_requested_string
- F01_010_provider_maps_only_first_result_row
- F01_011_provider_assigns_destination_line_numbers_starting_at_1_in_service_order
- F01_012_empty_sourceLocationId_is_rejected_with_file_transfer_validation_exception
- F01_013_sourceLocationId_is_parsed_with_guid_parse_when_building_soureceLocationGuid
- F01_018_configuration_status_label_is_localized_active_inactive
- F01_019_destination_status_label_localized_and_isAddedUpdated_false
- F01_021_add_file_base_configuration_post_without_id_marks_active_and_renders_add_view
- F01_022_add_file_base_configuration_post_with_id_renders_static_configModel
- F01_037_bff_add_source_location_returns_200_and_body_contains_service_result
- F01_056_bff_update_source_location_returns_200_and_body_contains_service_result
- F01_058_bff_invalidate_source_location_returns_200_and_body_contains_service_result
- F01_080_bff_search_source_locations_returns_200_and_body_contains_query_result
- F01_082_bff_get_source_locations_returns_200_and_body_contains_query_result
- F02_004_bff_get_ftp_servers_returns_200_and_body_contains_query_result
- F02_018_bff_add_ftp_server_returns_200_and_body_contains_service_result
- F02_020_bff_update_ftp_server_returns_200_and_body_contains_service_result
- F02_022_bff_invalidate_ftp_server_returns_200_and_body_contains_service_result
- F05_001_tracking_search_initializes_editor_single_step_create_and_cancel_goes_to_myhome
- F05_002_tracking_search_localizes_search_action_label_from_user_locale
- F05_003_tracking_search_returns_empty_criteria_model
- F05_020_bff_search_file_transfer_movement_returns_200_and_body_contains_query_result
- F05_022_bff_search_inbound_target_returns_200_and_body_contains_query_result

## UI behavior tests (screen-specific)

- SearchSourceLocationsScreen_filters_match_legacy_values_and_open_view_modal
- SourceLocationConfigurationEditModal_requires_at_least_one_active_destination
- SourceLocationConfigurationEditModal_validates_ftp_address_format_for_ftp_protocols
- ViewEditFtpServerCredentialsModal_password_fields_become_required_when_either_password_non_empty
- ViewEditFtpServerCredentialsModal_validates_ip_address_format_for_server_and_proxy
- SearchFileTransferTrackingScreen_initializes_date_fields_to_today_then_clears

## Contract tests

- openapi_yaml_parses_and_lints_minimally (OpenAPI 3.0 document shape; required `x-legacy-endpoints` entries present)
- contract_md_matches_openapi_endpoints (every logical operation in `CONTRACT.md` is represented in `openapi.yaml`)
