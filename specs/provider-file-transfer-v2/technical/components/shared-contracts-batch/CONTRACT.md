---
type: Interface Contract
title: technical/components/shared-contracts-batch/CONTRACT
description: What the shared-contracts-batch component offers other components and what it uses from them.
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
  body_sha256: d326a34fb611b61f5999099b21335ee389015ee0fffe9a8298337830d7212866
---

﻿# Contract: `shared-contracts-batch`

This component provides worker-local in-process contract/DTO types for FXFER batch execution.
It does not define HTTP endpoints; it is consumed by other worker components (application + data-access) as a shared type set.

## Offers

### Batch DTO types

- `DestinationModel`
  - Destination identity + endpoint/protocol fields.
  - Sensitive fields (runtime-only): `EncryptionPrivateKey`, `PrivateKeyPassword`.

- `FtpServerModel`
  - Server endpoint + credential fields.
  - Sensitive fields (runtime-only): `FtpPassword`.

- `SourceLocationModel`
  - Source location identity + protocol/address.
  - Schedule + destination list:
    - `Schedule` (`SourceScheduleModel`)
    - `Destinations` (`List<DestinationModel>`, initialized to empty list by constructor)
  - Eligibility helpers:
    - `IsScheduledToRun` schedule evaluation based on `DateTime.Now`.

- `SourceScheduleModel`
  - Weekly schedule list `DaysToRun` and monthly schedule list `MonthlyProcessDayToRun`.
  - Time window bounds `StartTime`/`EndTime`.

- File tracking + delivery + notification + suspension DTOs:
  - `FileManagementRequestModel`, `FileManagementResponseModel`
  - `PendingFileModel`, `FileToDeliverModel`
  - `FtsDeliveryModel`
  - `FtsNotificationModel`, `FtsNotificationQueryModel`, `GetNotificationSearchCriteria`
  - `ErrorLogDetailModel`
  - `FtsSuspensionModel`, `FtsSuspensionSearchCriteria`

## Uses

None.

## Data owned

None.

## Notes on sensitive fields

The following DTO fields may carry secret material (passwords/private keys) at runtime and are treated as sensitive:

- `FtpServerModel.FtpPassword`
- `DestinationModel.EncryptionPrivateKey`
- `DestinationModel.PrivateKeyPassword`

They are runtime-only values populated from the worker's secrets-resolution mechanism and must not be persisted, emitted to events, or written to logs/traces.

## Enterprise-Architecture standards applied

- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Solution Overview` (external secrets management; eliminate secrets from Git and container images)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Benefits` (`No Secret Sprawl` posture; secrets never in Git)
