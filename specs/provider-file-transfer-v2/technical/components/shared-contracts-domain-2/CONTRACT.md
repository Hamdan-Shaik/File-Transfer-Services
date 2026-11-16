---
type: Interface Contract
title: 'Contract: shared-contracts-domain-2'
description: What the shared-contracts-domain-2 component offers other components and what it uses from them.
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
  body_sha256: ec4e30b995a5ff34d2973394c2741fdfa4623c2388c01eed010b8764a534f221
---

# Contract: `shared-contracts-domain-2`

This component provides in-process contract/DTO types for the legacy batch file-transfer management domain models.
It does not define HTTP endpoints; it is consumed by other components as a shared type set.

## Offers

### DTO types

- `DestinationModel`
  - Destination endpoint/protocol fields plus notification/contact fields.
  - Sensitive fields (runtime-only): `EncryptionPrivateKey`, `PrivateKeyPassword`.

- `FtpServerModel`
  - Server endpoint + credential fields.
  - Sensitive fields (runtime-only): `FtpPassword`.

- `SourceLocationModel`
  - Source location identity + protocol/address fields.
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

They are runtime-only values populated from the target system's secrets-resolution mechanism and must not be persisted, emitted to events, or written to logs/traces.

## Enterprise-Architecture standards applied

- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Solution Overview` (external secrets management; eliminate secrets from Git and container images)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Benefits` (`No Secret Sprawl` posture; secrets never in Git)

