---
type: Test Plan
title: 'Tests: backend-run-file-transfer-batch-api'
description: The tests that prove the backend-run-file-transfer-batch-api component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-run-file-transfer-batch-api
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:02:28Z
sources:
- id: rules-run-file-transfer-batch
  resource: /technical/flows/run-file-transfer-batch/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-run-file-transfer-batch-api/INVENTORY.yaml
  title: Every legacy class and method (2 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-run-file-transfer-batch-api
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 2
  methods: 7
  owned_rules: 14
  flows:
  - run-file-transfer-batch
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Worker
  body_sha256: d9b0477f0c659a086df24e9133487814fb9a4d17dd0acb04859e3f68a4eea2a5
---

# Tests: backend-run-file-transfer-batch-api

## Rules

- `F04_001_initializes_container_resolves_FileTransferExecutor_and_executes`
  - Given any `args`
  - When `run-file-transfer-batch-basic` is executed
  - Then the runtime container is initialized, `FileTransferExecutor` is resolved by name, `Execute(args)` is called, and the returned integer is returned.

- `F04_002_basic_run_records_failure_message_and_returns_minus_1_on_unexpected_exception`
  - Given `FileTransferExecutor` resolution or execution throws an exception with message `boom`
  - When `run-file-transfer-batch-basic` is executed
  - Then a support-visible record is written under source `File Transfer Batch Process` with message `boom`
  - And the result is `-1`.

- `F04_003_managed_run_records_start_and_always_records_end`
  - Given a managed run is started
  - When it succeeds and when it throws unexpectedly
  - Then `Starting FileTransfer Batch Process` is logged
  - And `Ending FileTransfer Batch Process` is always logged.

- `F04_004_managed_run_rejects_non_accepted_argument_patterns`
  - Given managed run details with 0, 2, or 4 values
  - When `run-file-transfer-batch-managed` is executed
  - Then usage guidance is emitted and the result is `-1`.

- `F04_005_one_value_managed_run_sets_processing_all_files_and_uses_args0_as_tenant`
  - Given managed run details of exactly one value containing a valid tenant GUID
  - When `run-file-transfer-batch-managed` is executed
  - Then the tenant id is set from `args[0]` and the run continues.

- `F04_006_one_value_managed_run_rejects_non_guid_tenant_id`
  - Given managed run details of exactly one value that is not a GUID
  - When `run-file-transfer-batch-managed` is executed
  - Then usage guidance is emitted and the result is `-1`.

- `F04_007_three_value_managed_run_uses_args2_as_tenant_id`
  - Given managed run details of three values where `args[2]` is a valid GUID
  - When `run-file-transfer-batch-managed` is executed
  - Then the tenant id is set from `args[2]` and the run continues.

- `F04_008_three_value_managed_run_requires_args1_be_int_or_guid`
  - Given managed run details of three values where `args[1]` is neither an int nor a GUID
  - When `run-file-transfer-batch-managed` is executed
  - Then usage guidance is emitted and the result is `-1`.

- `F04_009_three_value_managed_run_requires_args2_be_guid`
  - Given managed run details of three values where `args[2]` is not a GUID
  - When `run-file-transfer-batch-managed` is executed
  - Then usage guidance is emitted and the result is `-1`.

- `F04_010_three_value_managed_run_does_not_validate_args0_label`
  - Given managed run details of three values with `label = made-up-option`, a valid `commandValue`, and a valid tenant GUID
  - When `run-file-transfer-batch-managed` is executed
  - Then the run is accepted and continues.

- `F04_011_managed_run_loads_tenant_settings_only_when_not_already_loaded`
  - Given tenant settings are not already loaded
  - When `run-file-transfer-batch-managed` is executed
  - Then tenant settings are loaded for the extracted `tenantId` before continuing.
  - And when tenant settings are already loaded, the loader is not called.

- `F04_012_missing_mandatory_setting_key_logs_error_and_causes_failure`
  - Given one required key is missing from loaded tenant settings
  - When `run-file-transfer-batch-managed` is executed
  - Then an error `"{key} is not specified in app.config"` is emitted
  - And the result is `-1`.

- `F04_013_managed_run_initializes_container_resolves_FileTransferExecutor_and_executes_after_checks`
  - Given managed run details are accepted and mandatory keys are present
  - When `run-file-transfer-batch-managed` is executed
  - Then the runtime container is initialized, `FileTransferExecutor` is resolved by name, and `Execute(args)` is called.

- `F04_014_managed_run_records_fatal_and_records_failure_message_and_returns_minus_1_on_unexpected_exception`
  - Given an unexpected exception is thrown during managed execution
  - When `run-file-transfer-batch-managed` is executed
  - Then a fatal log `Exception occurred: ` is recorded with the exception
  - And a support-visible record is written under source `File Transfer Batch Process`
  - And the result is `-1`.

## Target-state requirement

- `T_007_rerun_mode_executes_for_specific_tenant_and_tracked_file`
  - Given a privileged rerun mode call with a valid tenant GUID and tracked file GUID
  - When `rerun-tracked-file` is executed
  - Then `FileTransferExecutor` is invoked in a way that targets only that tracked file for that tenant.

## Contract tests

- `openapi_has_no_http_surface`
  - Assert `openapi.yaml` is valid OpenAPI 3.0 and contains `x-no-http-surface: true` and `paths: {}`.

- `contract_lists_supported_cli_forms_and_dependencies`
  - Assert `CONTRACT.md` documents the three offered commands and the required `FileTransferExecutor` named resolution.