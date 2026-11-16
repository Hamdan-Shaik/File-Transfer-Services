---
type: Test Plan
title: technical/components/backend-batch-business/TESTS
description: The tests that prove the backend-batch-business component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-batch-business
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T00:36:41Z
sources:
- id: inventory
  resource: /technical/components/backend-batch-business/INVENTORY.yaml
  title: Every legacy class and method (19 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-batch-business
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 19
  methods: 78
  owned_rules: 0
  flows: []
  depends_on:
  - backend-business-helpers
  deployable: fxfer-transfer-worker
  folder: src/Application
  body_sha256: e9a6e10b7bb801c92f65dbfd0a217a016f624b554a6dbdb0c6ada823070aec3f
---

﻿# Tests: `backend-batch-business`

This component owns no business rules.
Tests focus on method-level parity and contract checks for `openapi.yaml` and `CONTRACT.md`.

## Unit tests (method parity)

### FileTransferBatchExecutor
- `FileTransferBatchExecutor_ValidateInputArguments_allows_null_or_empty_args`
  - Given `args == null` or `args.Length == 0`, when validating, then result is true.

- `FileTransferBatchExecutor_ValidateInputArguments_rejects_wrong_arg_count`
  - Given `args.Length != 2`, when validating, then it prints usage and returns false.

- `FileTransferBatchExecutor_ValidateInputArguments_rejects_non_int_non_guid_command_value`
  - Given `args = ["top", "not-a-number"]`, when validating, then it prints usage and returns false.

- `FileTransferBatchExecutor_ParseInputArguments_parses_sakTrack_guid`
  - Given `args = ["sakTrack", <guid>]`, then `sakTrack` is set and other outputs are null.

- `FileTransferBatchExecutor_ParseInputArguments_parses_configId_int`
  - Given `args = ["configId", "123"]`, then `configId == 123`.

- `FileTransferBatchExecutor_ParseInputArguments_parses_top_int`
  - Given `args = ["top", "5"]`, then `topXFailures == 5`.

### FTSProducer
- `FTSProducer_AddPendingFileToDeliveryQueue_only_requeues_after_minutesBetweenRetry`
  - Given a pending file with `StatusDate` less than `minutesBetweenRetry` minutes old, then it returns 0 and does not enqueue.

- `FTSProducer_GetPendingFilesToReProcess_specificFileId_returns_empty_when_file_not_found_or_completed`
  - Given `specificSakFileTracking` that is not found or is Completed, then the method returns an empty list.

- `FTSProducer_IsSourceSetToRun_returns_true_for_active_not_suspended`
  - Given `IsActive == true` and `IsSuspended == false`, then result is true.

### FTSConsumer
- `FTSConsumer_DownloadWorkingFile_reuses_existing_work_file_when_reuseWorkFile_true`
  - Given an existing working-copy file and `reuseWorkFile == true`, then it returns the existing path without calling `DownloadFile`.

- `FTSConsumer_SendNotificationOfDelivery_is_noop_when_SendNotificationsIndicator_false_or_DisableNotifications_true`
  - Given `SendNotificationsIndicator == false` OR `DisableNotifications == true`, then no email/event is emitted.

- `FTSConsumer_ProcessDeliveryQueue_on_DuplicateFileNameException_marks_completed_when_duplicate_rename_active`
  - Given `ActivateDuplicateRenameLogic == true`, then it renames the source using `DuplicateFileExtension` and updates status to `Completed`.

### FileTransferOperations.GetDestinationFileName
- `GetDestinationFileName_expands_DATEDIR_into_DestinationAddress`
  - Given `RenamePattern` contains `<DATEDIR>`, then the destination address is appended with year/month/day using `\` for UNC and `/` for non-UNC.

- `GetDestinationFileName_ORIG_DATE_TIME_pattern_produces_timestamped_name`
  - Given rename pattern `ORIG_<DATE>_<TIME>`, then result contains `_yyyymmdd_hhmmss`.

- `GetDestinationFileName_USER_SAK_CRC32_ORIG_uses_UserId_when_uploadedSFTPUserName_is_empty`
  - Given rename pattern `<USER>_<SAK>_<CRC32>_ORIG` and empty `uploadedSFTPUserName`, then result starts with `UserId_`.

### FileTransferProtocols.UploadToUnc
- `UploadToUnc_throws_DuplicateFileNameException_when_destination_exists_and_overwrite_false`
  - Given destination exists and `IsOverwriteAtDestination == false` and archive deletion is not enabled, then it throws `DuplicateFileNameException`.

### FileTransferProtocols.GetCrc32
- `GetCrc32_returns_empty_string_when_file_does_not_exist`
  - Given a missing file path, then it returns `""`.

## Contract tests

- `openapi_yaml_is_minimal_and_marked_no_http_surface`
  - Assert `x-no-http-surface: true`.
  - Assert `paths` is `{}`.

- `contract_declares_component_dependency_on_backend_business_helpers`
  - Assert `technical/components/backend-batch-business/CONTRACT.md` has a `Uses` section listing `backend-business-helpers`.

## Enterprise-Architecture standards applied

- `patterns/itsm-operations/api-microservices/PTN-OPS-API-004-structured-logging-pattern.md` section `Solution Overview` (tests should validate behavioral outcomes and structured log fields rather than exact unstructured log strings)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Solution Overview` (tests must not embed real credentials/keys; use ephemeral or dummy secret material)
