---
type: Interface Contract
title: technical/components/backend-business-helpers-2/CONTRACT
description: What the backend-business-helpers-2 component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-helpers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:11:01Z
sources:
- id: inventory
  resource: /technical/components/backend-business-helpers-2/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-helpers-2
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 5
  methods: 28
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Application/Helpers
  body_sha256: 082e95931ac79aef22d227895d440f7db34e76b2257c57547a0c3e05fd8c3367
---

﻿# Contract: `backend-business-helpers-2`

This component is a helper library placed in the `fxfer-transfer-worker` deployable under `src/Application/Helpers`.
It provides utility types used by other worker components for:

- PGP encryption/decryption and stream IO helpers (merged into the shared helper types created by component `backend-business-helpers`).
- Publishing `FileTransferred` integration events.
- Posting notification events and sending notification emails via the legacy notification service proxy.

## Offers

- **Duplicate-file signaling**: `DuplicateFileNameException(string message)`.

- **PGP decryption** (`EncryptionHelper`) (merged into `backend-business-helpers` target type):
  - `DecryptPgpData(string inputData, string outputFile, string privateKey, string password) -> string`
  - `DecryptPgpData(Stream inputStream, string outputFile, Stream privateKeyStream, string passPhrase) -> string`
  - Output behavior parity:
    - file output is written to `outputFile` (or `outputFile + "\\" + <literal filename>` for signed compressed payloads).
    - returned string is a single space by default and is populated only when decrypting `PgpLiteralData` by reading literal data as text.
  - Error behavior parity: exceptions are wrapped to `PgpException` with the legacy message prefix.

- **PGP encryption** (`EncryptionHelper`) (merged into `backend-business-helpers` target type):
  - `EncryptPgpFile(string inputFile, string outputFile, string publicKey, bool armor, bool withIntegrityCheck, out int result) -> void`
  - Uses BouncyCastle PGP with ZIP compression and CAST5 symmetric encryption.

- **Stream utilities** (`IOHelper`) (merged into `backend-business-helpers` target type):
  - `GetStream(string stringData) -> Stream`
  - `GetString(Stream inputStream) -> string`
  - `WriteStream(Stream inputStream, ref byte[] dataBytes) -> void`

- **Integration event publishing** (`EventHelper`):
  - `PublishFileTransferredEvent(FileToDeliverModel fileDeliveryData, string tenantId, TimeSpan elapsedTime) -> void`
  - Event is published only when `fileDeliveryData.SourceLocation.Destinations` includes a destination with `FTEMetadataId.HasValue && FTEMetadataId != Guid.Empty`.

- **Notification service helper** (`NotificationServiceHelper`):
  - `PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId) -> void`
  - `SendEmail(string subject, string emailBody, List<Address> addressList, Address fromAddress, MailPriority priority, List<Attachments> attachmentList) -> void`
  - Maintains `PreviousEvent` and supplies the prior `EventId` as `lastEventId` to the next `PostEvent` call.

## Uses

None (no cross-component contracts declared by this component).

## Data owned

None.

## Enterprise-Architecture standards applied

- `patterns/security/api-microservices/PTN-SEC-API-008-secret-management-pattern.md` section `Executive Summary > Solution Overview` (key material, passphrases, and service endpoints treated as secrets/config and retrieved at runtime)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant-scoped event publishing and notification calls)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (no PII/PHI in telemetry/log payloads when publishing events)