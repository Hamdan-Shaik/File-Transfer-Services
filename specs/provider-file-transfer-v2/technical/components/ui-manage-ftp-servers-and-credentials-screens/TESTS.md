---
type: Test Plan
title: 'Tests: ui-manage-ftp-servers-and-credentials-screens'
description: The tests that prove the ui-manage-ftp-servers-and-credentials-screens component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/ui-manage-ftp-servers-and-credentials-screens
tags:
- component
- provider-file-transfer-v2
- ui
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:47:47Z
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
  resource: /technical/components/ui-manage-ftp-servers-and-credentials-screens/INVENTORY.yaml
  title: Every legacy class and method (1 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:ui-manage-ftp-servers-and-credentials-screens
  generation:
    status: done
    attempts: 2
  tier: ui
  classes: 1
  methods: 25
  owned_rules: 51
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - search-and-track-file-transfers
  depends_on:
  - backend-data-file-transfer
  deployable: fxfer-mfe
  folder: src/screens/manage-ftp-servers-and-credentials
  body_sha256: f3d19790c5b4fb74fa7ba95ca1cb5a5cedfc9879f7dad880ac0b92669f49f961
---

# Tests: ui-manage-ftp-servers-and-credentials-screens

This file lists the tests to write to preserve behavior. Tests are described at this component level (UI-side request mapping plus BFF contract usage) using Jest plus React Testing Library, plus contract tests validating `openapi.yaml` and `CONTRACT.md`.

## Rules (unit or component tests)

- F01_023_upsert_base_configuration_rejects_invalid_model_with_400
- F01_024_upsert_base_configuration_strips_hyphens_from_owner_phone_number
- F01_025_upsert_base_configuration_calls_add_when_source_location_id_missing_else_update
- F01_026_upsert_base_configuration_returns_200_echo_model_on_ok_else_400
- F01_083_get_source_locations_is_get_and_requires_csrf_posture
- F01_084_get_source_locations_resolves_query_provider_by_component_name
- F01_085_get_source_locations_builds_request_with_generated_requestor_and_null_data
- F01_086_get_source_locations_returns_200_list_items_on_ok_else_400
- F01_087_get_source_locations_item_name_is_configName_dash_address_and_id_is_source_location_id
- F01_088_get_source_locations_provider_queries_active_only
- F01_089_get_source_locations_provider_maps_service_guid_to_string_sourceLocationId
- F01_090_get_source_locations_provider_sorts_by_config_name_ascending
- F02_010_get_ftp_server_details_is_get_and_requires_csrf_posture
- F02_011_get_ftp_server_details_resolves_query_provider_by_component_name
- F02_012_get_ftp_server_details_builds_request_with_generated_requestor_and_null_data
- F02_013_get_ftp_server_details_returns_200_list_items_with_name_and_guid_string_id_on_ok
- F02_014_get_ftp_server_details_returns_400_when_status_not_ok_or_data_null
- F02_015_get_ftp_server_details_provider_queries_isValid_true
- F02_016_get_ftp_server_details_provider_maps_service_rows_and_formats_exceptions
- F02_038_save_credentials_dispatches_create_when_ftpServerId_empty_else_update
- F02_039_search_credentials_builds_search_criteria_from_grid_and_queries_provider
- F02_040_search_credentials_returns_404_with_empty_list_when_response_or_data_is_null
- F02_041_search_credentials_returns_400_on_error_status_else_200_with_data_and_total
- F02_043_search_credentials_any_non_empty_isValid_filter_forces_isValid_true
- F02_045_search_credentials_passwords_are_removed_from_results_by_default
- F02_046_search_credentials_uses_page_size_and_page_number_and_last_sort_only
- F02_047_search_credentials_translates_search_mode_to_filter_type_default_equal
- F02_048_search_credentials_total_rows_available_uses_service_reported_total
- F02_049_create_credentials_invalid_model_state_returns_400_with_formatted_errors
- F02_050_create_credentials_success_sets_editor_savedMessage_and_saveActionText_success
- F02_053_update_credentials_invalid_model_state_returns_400_with_formatted_errors
- F02_054_update_credentials_isValid_true_calls_update_isValid_false_calls_invalidate
- F02_055_update_credentials_null_data_returns_400_with_first_service_message_only
- F02_057_update_credentials_success_message_differs_for_update_vs_invalidate
- F02_061_success_messages_use_localization_provider_user_locale_and_message_text
- F05_004_tracking_search_requires_csrf_posture_and_binds_grid_request
- F05_005_tracking_search_sends_requestor_and_initialized_search_criteria
- F05_006_tracking_search_total_defaults_to_0_when_paging_results_missing
- F05_007_tracking_search_returns_200_with_empty_list_and_total_0_when_data_null
- F05_008_tracking_search_returns_400_when_status_not_ok
- F05_009_file_transfer_search_requires_csrf_posture_and_binds_grid_request
- F05_010_file_transfer_search_sends_requestor_and_initialized_search_criteria
- F05_011_file_transfer_search_returns_200_only_when_status_ok_and_data_non_null_else_400
- F05_012_data_list_lookup_requires_csrf_posture_and_routes_through_internal_selector
- F05_013_data_list_lookup_returns_200_with_list_items_when_result_valid
- F05_014_data_list_lookup_returns_400_with_result_when_invalid
- F05_015_data_list_selector_treats_missing_dataListType_as_empty_and_populates_only_for_module
- F05_016_modules_list_queries_active_modules_ordered_by_name_and_maps_moduleName_and_moduleId
- F05_017_fte_metadata_lookup_requires_csrf_posture_and_calls_provider_with_requestor_and_contract
- F05_018_fte_metadata_lookup_returns_200_when_data_list_non_null_else_404_with_empty_list
- F05_036_file_transfer_search_provider_builds_query_only_when_paging_criteria_present

## Contract tests

- openapi_yaml_parses_and_lints_minimally (OpenAPI 3.0 shape; required x-legacy-endpoints entries present)
- contract_md_matches_openapi_endpoints (every logical operation in CONTRACT.md is represented in openapi.yaml)

