---
type: Target Placement
title: 'Target: shared-contracts-domain-2'
description: Where the shared-contracts-domain-2 component goes in the new system and what to build, method by method.
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
  body_sha256: 27411e132eac6db079daf8ac795cc7aedae2766e3d28af8a59b124fc02d54e2b
---

# Target: `shared-contracts-domain-2`

## Where

- Deployable: `fxfer-domain-api`
- Folder: `services/fxfer-domain-api/src/Contracts/Domain`

## What to create

Create in-process DTO/contract types representing the legacy batch file-transfer management domain models in `HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain`.

These types are used by worker/batch-oriented components (data access and helper logic) to carry configuration, tracking identifiers, and notification/error payloads.

- Keep field names and CLR types identical to legacy (including legacy casing such as `FileID`).
- Sensitive fields (`FtpServerModel.FtpPassword`, `DestinationModel.EncryptionPrivateKey`, `DestinationModel.PrivateKeyPassword`) are treated as runtime-only values and must not be written to logs or serialized into HTTP responses.

### SourceLocationModel schedule logic parity

Implement the legacy schedule evaluation behavior on `SourceLocationModel`:

- `IsScheduledToRun` returns `true` when `Schedule` is `null`.
- Otherwise, it evaluates the current local `DateTime.Now` and returns `true` only when:
  - `DateTime.Now.TimeOfDay` is inclusively between `Schedule.StartTime` and `Schedule.EndTime`, and
  - either `Schedule.DaysToRun` contains `DateTime.Now.DayOfWeek` or `Schedule.MonthlyProcessDayToRun` contains `DateTime.Now.Day`.
- `IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)` remains as a private helper performing the inclusive time-of-day check.

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

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `1. OpenAPI Specification Document` (OpenAPI documents are treated as versioned contract artifacts, even when the contract is empty/no business surface)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Solution Overview` (secrets are sourced from AWS Secrets Manager / external secrets management; not stored in Git)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Benefits` (treat password/private key fields as runtime-only values; never in Git/logs)

