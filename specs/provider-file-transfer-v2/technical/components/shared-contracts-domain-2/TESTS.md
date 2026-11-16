---
type: Test Plan
title: 'Tests: shared-contracts-domain-2'
description: The tests that prove the shared-contracts-domain-2 component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-domain-2
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:19:37Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-domain-2/INVENTORY.yaml
  title: Every legacy class and method (15 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-domain-2
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 15
  methods: 1
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Domain
  body_sha256: 70b8334f4def36cb5d858dc66fda691e31645567f1e80c3abc08aca045115cdc
---

# Tests: `shared-contracts-domain-2`

This component has no owned flow rules.
Tests focus on the legacy-parity helper behavior that exists on `SourceLocationModel`, plus contract checks for `CONTRACT.md` and `openapi.yaml`.

## Rule tests

None.

## Unit tests

### SourceLocationModel schedule evaluation

Write unit tests targeting `SourceLocationModel.IsScheduledToRun` behavior.

- `SourceLocationModel_IsScheduledToRun_is_true_when_Schedule_is_null`
  - Given: `model.Schedule = null`.
  - When: read `model.IsScheduledToRun`.
  - Then: result is `true`.

- `SourceLocationModel_IsScheduledToRun_is_false_when_now_is_outside_time_window`
  - Given:
    - `Schedule.StartTime = DateTime.Now.AddHours(1).TimeOfDay`
    - `Schedule.EndTime = DateTime.Now.AddHours(2).TimeOfDay`
    - `Schedule.DaysToRun` contains `DateTime.Now.DayOfWeek`
    - `Schedule.MonthlyProcessDayToRun` contains `DateTime.Now.Day`
  - When: read `model.IsScheduledToRun`.
  - Then: result is `false`.

- `SourceLocationModel_IsScheduledToRun_is_true_when_now_is_in_window_and_today_is_in_DaysToRun`
  - Given:
    - `Schedule.StartTime = DateTime.Now.AddMinutes(-1).TimeOfDay`
    - `Schedule.EndTime = DateTime.Now.AddMinutes(1).TimeOfDay`
    - `Schedule.DaysToRun` contains `DateTime.Now.DayOfWeek`
    - `Schedule.MonthlyProcessDayToRun` does not contain `DateTime.Now.Day`
  - When: read `model.IsScheduledToRun`.
  - Then: result is `true`.

- `SourceLocationModel_IsScheduledToRun_is_true_when_now_is_in_window_and_today_is_in_MonthlyProcessDayToRun`
  - Given:
    - `Schedule.StartTime = DateTime.Now.AddMinutes(-1).TimeOfDay`
    - `Schedule.EndTime = DateTime.Now.AddMinutes(1).TimeOfDay`
    - `Schedule.DaysToRun` does not contain `DateTime.Now.DayOfWeek`
    - `Schedule.MonthlyProcessDayToRun` contains `DateTime.Now.Day`
  - When: read `model.IsScheduledToRun`.
  - Then: result is `true`.

- `SourceLocationModel_IsScheduledToRun_is_false_when_now_is_in_window_but_no_day_matches`
  - Given:
    - `Schedule.StartTime = DateTime.Now.AddMinutes(-1).TimeOfDay`
    - `Schedule.EndTime = DateTime.Now.AddMinutes(1).TimeOfDay`
    - `Schedule.DaysToRun` excludes `DateTime.Now.DayOfWeek`
    - `Schedule.MonthlyProcessDayToRun` excludes `DateTime.Now.Day`
  - When: read `model.IsScheduledToRun`.
  - Then: result is `false`.

### SourceLocationModel time window helper

- `SourceLocationModel_IsTimeOfDayBetween_is_inclusive`
  - Given: `startTime == now.TimeOfDay` and `endTime == now.TimeOfDay`.
  - When: evaluate `IsTimeOfDayBetween(now, startTime, endTime)` via `IsScheduledToRun` schedule setup.
  - Then: the time window check treats bounds as inclusive.

### SourceLocationModel constructor initialization

- `SourceLocationModel_constructor_initializes_Destinations_to_empty_list`
  - Given: `var model = new SourceLocationModel()`.
  - Then: `model.Destinations` is not null and has `Count == 0`.

## Contract tests

### openapi.yaml

- `OpenApi_has_no_http_surface`
  - Assert `x-no-http-surface: true`.
  - Assert `paths` is an empty object.

### CONTRACT.md consistency

- `Contract_lists_all_offered_types`
  - Assert `CONTRACT.md` names each DTO type defined by this component:
    - `DestinationModel`, `ErrorLogDetailModel`, `FileManagementRequestModel`, `FileManagementResponseModel`, `FileToDeliverModel`, `FtpServerModel`, `FtsDeliveryModel`, `FtsNotificationModel`, `FtsNotificationQueryModel`, `FtsSuspensionModel`, `FtsSuspensionSearchCriteria`, `GetNotificationSearchCriteria`, `PendingFileModel`, `SourceLocationModel`, `SourceScheduleModel`.

