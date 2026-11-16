---
type: Test Plan
title: 'Tests: backend-file-transfer-mgmt-data-access'
description: The tests that prove the backend-file-transfer-mgmt-data-access component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-mgmt-data-access
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:58:48Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-mgmt-data-access/INVENTORY.yaml
  title: Every legacy class and method (18 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-mgmt-data-access
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 18
  methods: 54
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Infrastructure/DataAccess
  body_sha256: 2b38c258377840b975db7a6a181aa9ccfa0bccc8f56c47bd85bc17febaba47de
---

# Tests: backend-file-transfer-mgmt-data-access

This component owns no business rules. Tests focus on legacy-parity edge cases for mapping/filtering logic and on ensuring domain-API interactions are correctly formed.

## Unit tests (data-access adapters)

- `CreateFileManagementDataAccess_creates_tracking_header_with_transfering_status`
- `UpdateFileManagementStatusDataAccess_is_noop_when_file_not_found`

- `GetPendingFileListDataAccess_includes_non_completed_files`
- `GetPendingFileListDataAccess_includes_completed_files_completed_within_last_minute`
- `GetPendingFileListDataAccess_excludes_completed_files_older_than_last_minute`

- `GetConfigMetaDataDataAccess_returns_null_when_file_tracking_not_found`
- `GetConfigMetaDataDataAccess_filters_out_already_delivered_destinations_except_archive_destinations`

- `GetSourceLocationsDataAccess_parses_weekly_schedule_indicators`
- `GetSourceLocationsDataAccess_parses_monthly_schedule_first_and_last_day`
- `GetSourceLocationsDataAccess_parses_start_and_end_time_hhmm_or_defaults_to_midnight`
- `GetSourceLocationsDataAccess_monthly_run_on_this_day_of_month_uses_legacy_conversion_behavior`

- `QueryFtsNotificationDataAccess_when_both_flags_true_returns_recipients_with_both_flags_true`
- `QueryFtsNotificationDataAccess_when_error_only_returns_error_recipients`
- `QueryFtsNotificationDataAccess_when_success_only_returns_success_recipients`

- `CreateFtsFileDeliveryDataAccess_returns_empty_guid_when_destination_not_in_list`
- `CreateFtsFileDeliveryDataAccess_records_renamed_event_when_is_rename_at_destination_true`
- `CreateFtsFileDeliveryDataAccess_records_archieved_when_is_archive_true_otherwise_transferred`

- `CreateFtsFileExceptionEventDataAccess_records_exception_event_and_exception_log_and_sets_event_id_on_input_model`

- `CreateErrorLogDataAccess_truncates_exception_fields_to_legacy_max_lengths`

- `FtsSuspensionDataAccess_truncates_stack_trace_to_1000_chars`
- `UpdateSourceSuspendStatusDataAccess_returns_false_when_domain_api_reports_not_found`

Test strategy:

- Use a fake/stub `IFxferDomainApiClient` (or an in-memory HTTP test server) and assert requests/payloads rather than performing real network calls.
- Ensure `X-Correlation-ID` and W3C trace context are propagated from `IRequestContextAccessor` into outgoing domain API requests.

## Contract tests

- `openapi_yaml_has_x_no_http_surface_true`
- `contract_lists_all_domain_api_dependencies_used_by_data_access_classes`

