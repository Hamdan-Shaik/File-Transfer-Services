---
type: Test Plan
title: technical/components/backend-file-transfer-mgmt-business/TESTS
description: The tests that prove the backend-file-transfer-mgmt-business component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-mgmt-business
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:00:07Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-mgmt-business/INVENTORY.yaml
  title: Every legacy class and method (23 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-mgmt-business
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 23
  methods: 121
  owned_rules: 0
  flows: []
  depends_on:
  - backend-business-helpers-2
  deployable: fxfer-transfer-worker
  folder: src/Application
  body_sha256: ee2a5e2b995c571c8779e2b15edcb21c7fd5cc2b323ebf714ded19645a749082
---

﻿# Tests: `backend-file-transfer-mgmt-business`

This component owns no business rules.
Tests focus on method-level parity and contract checks for `openapi.yaml` and `CONTRACT.md`.

## Unit tests (method parity)

### FileTransferBatchExecutor
- `FileTransferBatchExecutor_AssignTenantId_uses_single_arg_as_tenant`
  - Given `args = ["TENANT"]`, then `tenantId == "TENANT"`.

- `FileTransferBatchExecutor_AssignTenantId_uses_third_arg_as_tenant`
  - Given `args = ["top", "5", "TENANT"]`, then `tenantId == "TENANT"`.

- `FileTransferBatchExecutor_ParseInputArguments_parses_sakTrack_guid`
  - Given `args = ["sakTrack", <guid>, "TENANT"]`, then `sakTrack` is set and other outputs are null.

- `FileTransferBatchExecutor_ParseInputArguments_parses_configId_int`
  - Given `args = ["configId", "123", "TENANT"]`, then `configId == 123`.

- `FileTransferBatchExecutor_ParseInputArguments_parses_top_int`
  - Given `args = ["top", "5", "TENANT"]`, then `topXFailures == 5`.

### FTSProducer
- `FTSProducer_AddPendingFileToDeliveryQueue_only_requeues_after_minutesBetweenRetry`
  - Given a pending file with `StatusDate` less than `minutesBetweenRetry` minutes old, then it returns 0 and does not enqueue.

- `FTSProducer_AddSourceFilesToQueue_skips_files_with_duplicate_extension_when_duplicateRenameActive_true`
  - Given `duplicateRenameActive == true` and source files include `*.{DuplicateFileExtension}`, then those files are not enqueued.

- `FTSProducer_AddSourceFilesToQueue_skips_excluded_extensions`
  - Given a file extension present in `ExcludedFileExtensionList`, then it is not enqueued.

- `FTSProducer_GetPendingFilesToReProcess_specificFileId_returns_empty_when_file_not_found_or_completed`
  - Given `specificFileId` that is not found or is Completed, then result is empty.

- `FTSProducer_IsSourceSetToRun_returns_true_for_active_not_suspended`
  - Given `IsActive == true` and `IsSuspended == false`, then result is true.

### FTSConsumer
- `FTSConsumer_DownloadWorkingFile_reuses_existing_work_file_when_reuseWorkFile_true`
  - Given an existing working file and `reuseWorkFile == true`, then it returns the existing path without calling `DownloadFile`.

- `FTSConsumer_ProcessDeliveryQueue_on_DuplicateFileNameException_marks_completed_when_duplicate_rename_active`
  - Given `ActivateDuplicateRenameLogic == true`, then it renames the source using `DuplicateFileExtension`, updates status to `Completed`, and emits a notification with errorType `Duplicate`.

- `FTSConsumer_ProcessDeliveryQueue_on_DuplicateFileNameException_marks_error_when_duplicate_rename_inactive`
  - Given `ActivateDuplicateRenameLogic == false`, then it updates status to `Error` and emits a notification with errorType `Error`.

- `FTSConsumer_DeleteSourceFile_does_not_delete_when_any_destination_not_fully_uploaded`
  - Given any destination has `IsFullyUploaded == false`, then `DeleteSourceFile` does not call `DeleteFile`.

### FileTransferOperations.GetDestinationFileName
- `GetDestinationFileName_expands_DATEDIR_into_DestinationAddress`
  - Given `RenamePattern` contains `<DATEDIR>`, then the destination address is appended with year/month/day using `\` for UNC and `/` for non-UNC.

- `GetDestinationFileName_pattern_1_appends_date_and_time`
  - Given `RenamePattern == "1"`, then the result name includes `_yyyymmdd_hhmmss`.

- `GetDestinationFileName_pattern_2_includes_fileId_and_crc32`
  - Given `RenamePattern == "2"`, then the result name starts with `<fileId>_<crc32>_`.

- `GetDestinationFileName_pattern_3_uses_UserId_when_uploadedSFTPUserName_empty`
  - Given `RenamePattern == "3"` and `uploadedSFTPUserName` is empty, then the result name starts with `UserId_`.

- `GetDestinationFileName_overrides_destination_folder_when_FTEMetadataId_not_null`
  - Given `FTEMetadataId != null`, then `DestinationAddress` is replaced with `ExternalFTECommonFolder` before composing the path.

### FileTransferOperations.CreateZipFile
- `CreateZipFile_returns_input_when_extension_is_zip`
  - Given input ends with `.zip`, then it returns the input path without creating a new zip.

### FileTransferProtocols.UploadToUnc
- `UploadToUnc_throws_DuplicateFileNameException_when_destination_exists_and_overwrite_false`
  - Given destination exists and `IsOverwriteAtDestination == false` and archive deletion is not enabled, then it throws `DuplicateFileNameException`.

- `UploadToUnc_writes_temp_then_moves_to_final`
  - Given a new destination, then it creates `<dest>.ftsUpload` and then moves to `<dest>`.

### FileTransferProtocols.RenameFileFromUNC
- `RenameFileFromUNC_deconflicts_by_appending_numeric_suffix`
  - Given `<name><ext>` already exists, then it renames to `<name>.1<ext>`, then `<name>.2<ext>`, etc.

### PatternMatcher.StrictMatchPattern
- `StrictMatchPattern_supports_star_and_question_mark`
  - Given `expression = "ABC*.txt"`, name `"ABC123.txt"` matches; given `"ABC?.txt"`, name `"ABCD.txt"` matches.

## Contract tests

- `openapi_yaml_is_minimal_and_marked_no_http_surface`
  - Assert `x-no-http-surface: true`.
  - Assert `paths` is `{}`.

- `contract_declares_component_dependency_on_backend_business_helpers_2`
  - Assert `technical/components/backend-file-transfer-mgmt-business/CONTRACT.md` lists `backend-business-helpers-2` under Uses.

## Enterprise-Architecture standards applied

- `patterns/itsm-operations/api-microservices/PTN-OPS-API-004-structured-logging-pattern.md` section `Solution Overview` (tests validate observable outcomes and structured fields, not exact unstructured log strings)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Solution Overview` (tests must not embed real credentials/keys; use dummy secret material)
