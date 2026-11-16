---
type: Test Plan
title: 'Tests: shared-data-file-transfer-constants'
description: The tests that prove the shared-data-file-transfer-constants component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-data-file-transfer-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:02:16Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-data-file-transfer-constants/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-data-file-transfer-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 3
  methods: 0
  owned_rules: 1
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants
  body_sha256: cc98f50cf163fa2fff46e693e2b9258531175c297955e416fa1932dd97f93a50
---

# Tests: `shared-data-file-transfer-constants`

## Owned rule tests

- `F01_017_configuration_run_schedule_id_is_monthly_when_any_day_of_month_field_is_set`
  - Given `RunOnLastDay = true` and `RunOnFirstDay = false` and any `RunOnThisDayId`
  - When run schedule id is inferred
  - Then the result is `FILE_TRANSFER_CONSTANTS_SHARED.Localization.DataListsItems.RunScheduleMonhly`

- `F01_017_configuration_run_schedule_id_is_monthly_when_run_on_this_day_is_non_zero`
  - Given `RunOnLastDay = false` and `RunOnFirstDay = false` and `RunOnThisDayId = 15`
  - When run schedule id is inferred using the legacy condition:
    - monthly when `RunOnLastDay || RunOnFirstDay || (RunOnThisDayId not empty and RunOnThisDayId != RunOnThisDay)`
    - otherwise weekly
  - Then the result is `FILE_TRANSFER_CONSTANTS_SHARED.Localization.DataListsItems.RunScheduleMonhly`

- `F01_017_configuration_run_schedule_id_is_weekly_when_no_day_of_month_fields_are_set`
  - Given `RunOnLastDay = false` and `RunOnFirstDay = false` and `RunOnThisDayId` is empty or equals `FILE_TRANSFER_CONSTANTS_SHARED.Localization.DataListsItems.RunOnThisDay`
  - When run schedule id is inferred
  - Then the result is `FILE_TRANSFER_CONSTANTS_SHARED.Localization.DataListsItems.RunScheduleWeekly`

## Edge-case/unit tests

- `FileTransferConstantsShared_exports_expected_constants`
  - Given the TypeScript module `apps/fxfer-mfe/src/legacy-constants/file-transfer-constants-shared.ts`
  - When `FILE_TRANSFER_CONSTANTS_SHARED` is imported
  - Then the exported object matches the expected constant values (snapshot or explicit assertions), including at minimum:
    - `ModuleName == FileTransfer`
    - `ApplicationSettings.ExternalFTECommonFolder == ExternalFTECommonFolder`
    - `Constants.FTPCode == 1`
    - `Localization.DataListsItems.RunScheduleMonhly == 2`

## Contract tests

- Validate `technical/components/shared-data-file-transfer-constants/openapi.yaml` parses as OpenAPI 3.x and includes:
  - `paths: {}`
  - `x-no-http-surface: true`

- Validate `technical/components/shared-data-file-transfer-constants/CONTRACT.md`:
  - lists `FILE_TRANSFER_CONSTANTS_SHARED` under Offers
  - lists no component dependencies