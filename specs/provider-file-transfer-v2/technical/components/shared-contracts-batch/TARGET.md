---
type: Target Placement
title: technical/components/shared-contracts-batch/TARGET
description: Where the shared-contracts-batch component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-batch
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:02:24Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-batch/INVENTORY.yaml
  title: Every legacy class and method (15 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-batch
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 15
  methods: 1
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Contracts
  body_sha256: b1ad8f81351413824bd9da1aa9af94e1f5439cd280f48f9e2e2517de106ee173
---

﻿# Target: `shared-contracts-batch`

## Where

- Deployable: `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`)
- Folder: `workers/fxfer-transfer-worker/src/Contracts`

## What to create

Create worker-local contract/DTO types representing the legacy batch contract models in `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Batch`.

- Namespace: `Fxfer.TransferWorker.Contracts.Batch`
- Files (one type per file under `workers/fxfer-transfer-worker/src/Contracts/Batch/`):
  - `DestinationModel.cs`
  - `ErrorLogDetailModel.cs`
  - `FileManagementRequestModel.cs`
  - `FileManagementResponseModel.cs`
  - `FileToDeliverModel.cs`
  - `FtpServerModel.cs`
  - `FtsDeliveryModel.cs`
  - `FtsNotificationModel.cs`
  - `FtsNotificationQueryModel.cs`
  - `FtsSuspensionModel.cs`
  - `FtsSuspensionSearchCriteria.cs`
  - `GetNotificationSearchCriteria.cs`
  - `PendingFileModel.cs`
  - `SourceLocationModel.cs`
  - `SourceScheduleModel.cs`

### DTO field parity

- Keep field names and CLR types identical to legacy (including legacy casing such as `FileID`).
- `FtpServerModel.FtpPassword`, `DestinationModel.EncryptionPrivateKey`, and `DestinationModel.PrivateKeyPassword` are treated as sensitive runtime-only values.

### SourceLocationModel schedule logic parity

Implement the legacy schedule evaluation logic on `SourceLocationModel`:

- `IsScheduledToRun` returns `true` when `Schedule` is `null`, otherwise returns whether the current local `DateTime.Now` is within schedule time window and matches either:
  - `Schedule.DaysToRun` containing `DateTime.Now.DayOfWeek`, or
  - `Schedule.MonthlyProcessDayToRun` containing `DateTime.Now.Day`.
- `IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)` is a private helper returning an inclusive time-of-day check.

### MinsToWaitBeforeRetrySuspendedConfig parity

Legacy `SourceLocationModel.MinsToWaitBeforeRetrySuspendedConfig` reads `MinsToWaitBeforeRetrySuspendedConfig` from `ConfigurationManager.AppSettings` and returns it multiplied by `-1`.

In the target worker, keep the same observable behavior by:

- Defining `SourceLocationModel.MinsToWaitBeforeRetrySuspendedConfig` as an `int` property containing the already-negated minute count (for legacy-parity use with `DateTime.AddMinutes(negative)`).
- Setting this property from worker configuration (for example `TransferExecutionOptions.MinsToWaitBeforeRetrySuspendedConfig`) when building/populating `SourceLocationModel` instances.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `DestinationModel` | `DestinationModel` | In-process DTO for destination configuration, including encryption fields and `FtpServerDetail`. |
| `ErrorLogDetailModel` | `ErrorLogDetailModel` | In-process DTO for error logging payload. |
| `FileManagementRequestModel` | `FileManagementRequestModel` | In-process DTO for file tracking create/update payload. |
| `FileManagementResponseModel` | `FileManagementResponseModel` | In-process DTO for file tracking create response ids. |
| `FileToDeliverModel` | `FileToDeliverModel` | In-process DTO combining `SourceLocationModel` + file identifiers. |
| `FtpServerModel` | `FtpServerModel` | In-process DTO; contains runtime-only password. |
| `FtsDeliveryModel` | `FtsDeliveryModel` | In-process DTO wrapper for delivery id. |
| `FtsNotificationModel` | `FtsNotificationModel` | In-process DTO for notification recipients. |
| `FtsNotificationQueryModel` | `FtsNotificationQueryModel` | In-process DTO for recipient filtering criteria. |
| `FtsSuspensionModel` | `FtsSuspensionModel` | In-process DTO for suspension ids. |
| `FtsSuspensionSearchCriteria` | `FtsSuspensionSearchCriteria` | In-process DTO for suspension search criteria. |
| `GetNotificationSearchCriteria` | `GetNotificationSearchCriteria` | In-process DTO for notification query criteria. |
| `PendingFileModel` | `PendingFileModel` | In-process DTO for pending-file summary rows. |
| `SourceLocationModel` | `SourceLocationModel` | In-process DTO for source location configuration + schedule evaluation helpers. |
| `SourceLocationModel.IsTimeOfDayBetween(DateTime, TimeSpan, TimeSpan)` | `SourceLocationModel.IsTimeOfDayBetween(DateTime, TimeSpan, TimeSpan)` | Keep inclusive start/end comparisons (private helper used by `IsScheduledToRun`). |
| `SourceScheduleModel` | `SourceScheduleModel` | In-process DTO for schedule data. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (OpenAPI documents are treated as versioned contract artifacts, even when the contract is empty/no business surface)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Solution Overview` (secrets are sourced from AWS Secrets Manager / external secrets management; not stored in Git)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Benefits` (secrets never in Git/container images; treat password/private key fields as runtime-only values)
