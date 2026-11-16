---
type: Test Plan
title: 'Tests: backend-data-access-batch-data-access'
description: The tests that prove the backend-data-access-batch-data-access component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-batch-data-access
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:40:28Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-batch-data-access/INVENTORY.yaml
  title: Every legacy class and method (16 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-batch-data-access
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 16
  methods: 47
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Infrastructure/DataAccess
  body_sha256: 8a8119dcfe857e7cff20a1652c5d5db7035597bcd96e58073376530dfb839c55
---

# Tests: backend-data-access-batch-data-access

Business rules owned: none.

## Unit tests

- CreateErrorLogDataAccess_truncate_returns_null_or_empty_unchanged
  - Given null/empty input, when Truncate, then returns same value

- CreateErrorLogDataAccess_truncate_limits_to_max_length
  - Given value length > max, when Truncate, then returned length equals max and preserves prefix

- CreateErrorLogDataAccess_set_limited_value_applies_legacy_entity_lengths
  - Given ErrorType/Message/MethodName length > 2000 and StackTrace length > 4000, when ExecuteQueryAsync, then payload sent to domain API is truncated to those limits

- GetSourceLocationsDataAccess_update_schedule_start_time_parses_hhmm
  - Given StartTime24HHMM is valid HHMM, when UpdateScheduleStartTime, then StartTime equals expected TimeSpan

- GetSourceLocationsDataAccess_update_schedule_start_time_falls_back_to_midnight
  - Given StartTime24HHMM is null/empty/too-short/non-numeric, when UpdateScheduleStartTime, then StartTime equals 00:00

- GetSourceLocationsDataAccess_update_schedule_end_time_parses_hhmm_and_fallbacks
  - Same cases as start time for EndTime24HHMM

- GetSourceLocationsDataAccess_update_weekly_schedule_data_adds_days_for_true_flags
  - Given each day flag set independently, when UpdateWeeklyScheduleData, then DaysToRun contains the corresponding DayOfWeek

- GetSourceLocationsDataAccess_update_monthly_schedule_data_adds_first_and_last_day
  - Given RunOnFrstDayOfMonthIndicator and RunOnLastDayOfMonthIndicator, when UpdateMonthlyScheduleData, then list contains 1 and DateTime.DaysInMonth(utcYear, utcMonth)

- GetSourceLocationsDataAccess_update_monthly_schedule_data_adds_zero_when_run_on_this_day_is_zero
  - Given RunOnThisDayOfMonth = 0, when UpdateMonthlyScheduleData, then MonthlyProcessDayToRun contains 0 (legacy parity)

- GetPendingFileListDataAccess_filters_pending_and_recently_completed
  - Given domain API file-tracking results containing Completed and non-Completed entries with createdDate values
  - When ExecuteQueryAsync at time T
  - Then includes all non-Completed
  - And includes Completed where createdDate > (T - 1 minute)
  - And excludes Completed where createdDate <= (T - 1 minute)

- GetConfigMetaDataDataAccess_filters_destinations_based_on_delivered_and_archive
  - Given transfer configuration destinations include archive and non-archive, and file-tracking events indicate prior delivery destinationIds
  - When ExecuteQueryAsync
  - Then excludes already-delivered non-archive destinations and includes archive destinations even if already delivered

- CreateFtsFileDeliveryDataAccess_records_renamed_event_only_when_destination_name_differs
  - Given destinationFileName differs from tracked fileName, when ExecuteQueryAsync, then domain API command includes Renamed event
  - Given destinationFileName equals tracked fileName, then Renamed event is not included

- CreateFtsFileDeliveryDataAccess_records_archieved_or_transferred_based_on_destination_flag
  - Given DestIsArchive true, when ExecuteQueryAsync, then Archieved event is recorded
  - Given DestIsArchive false, when ExecuteQueryAsync, then Transferred event is recorded

- QueryFtsNotificationDataAccess_selects_notification_rows_by_flag_combination
  - Given NotifyOnErrorInd and NotifyOnSuccessInd combinations, when ExecuteQueryAsync, then returned rows match the legacy selection rules

- UpdateSourceSuspendStatusDataAccess_returns_false_when_no_state_change
  - Given domain API reports no change, when ExecuteQueryAsync, then returns false

- UpdateFileManagementStatusDataAccess_returns_file_id
  - Given any domain API response, when ExecuteQueryAsync, then returns the fileId input

## Contract tests

- openapi.yaml: asserts x-no-http-surface: true and empty paths.
- CONTRACT.md: asserts the offered in-process operations are present.

