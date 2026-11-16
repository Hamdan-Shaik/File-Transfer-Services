---
type: Target Placement
title: technical/components/shared-contracts-events/TARGET
description: Where the shared-contracts-events component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-events
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:28:25Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-events/INVENTORY.yaml
  title: Every legacy class and method (15 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-events
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 15
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Events
  body_sha256: c3c64b39b1a885ac80883a53cf1d618868832f5f1f2f3f0f744632ce48065df8
---

﻿# Target: `shared-contracts-events`

## Where

- Deployable: `fxfer-domain-api`
- Folder: `services/fxfer-domain-api/src/Contracts/Events`

## What to create

Create the target-state event contract types used by `fxfer-domain-api` when publishing FXFER domain events via the outbox + Event Adapter flow.

Create the following C# types in `services/fxfer-domain-api/src/Contracts/Events`:

- `FxferEventEnvelope` containing the canonical envelope fields:
  - `tenant_id`, `event_name`, `event_version`, `event_id`, `occurred_at`, `correlation_id`, `trace_id`, `producer`.
- `FxferDomainEvent<TPayload>` representing an emitted event instance with:
  - `Envelope` (`FxferEventEnvelope`)
  - `Payload` (`TPayload`)

Create schema-versioned payload types for the event families declared by the deployable contract (`technical/target/deployables/fxfer-domain-api/CONTRACT.md`):

- `FxferConfigurationChangedV1`
  - `aggregate_type` (`ftp_server` / `transfer_configuration` / `integration_file` / `inbound_target`)
  - `aggregate_id` (uuid)
  - `change_kind` (`created` / `updated` / `invalidated` / `enabled` / `disabled`)
  - `operator_id` (string; optional when unknown)
  - `changed_at` (date-time)

- `FxferSuspensionStateChangedV1`
  - `source_location_id` (uuid)
  - `suspended` (boolean)
  - `reason_code` (string)
  - `changed_at` (date-time)

- `FxferDeliveryOutcomeRecordedV1`
  - `file_id` (uuid)
  - `source_location_id` (uuid)
  - `destination_id` (uuid)
  - `outcome_status` (string)
  - `occurred_at` (date-time)

Envelope values must be published in message headers/attributes (not only in the JSON body) so routing and observability do not require payload inspection.

Payload minimization posture for all FXFER domain events:

- Payloads must not include secrets (e.g., raw passwords).
- Payloads must not include raw provider file contents.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `FileTransferEvents` | `FxferEventEnvelope`, `FxferDomainEvent<TPayload>` | Legacy `ModelType` identity is replaced by the canonical envelope + producer identity.
| `FileTransferAdd` | `FxferConfigurationChangedV1` | Merged into configuration-change event publishing for `transfer_configuration` with `change_kind=created`.
| `FileTransferred` | `FxferDeliveryOutcomeRecordedV1` | Target payload is minimized; legacy name/path/mask/module fields are not carried in the published event payload.
| `FtpServerAdded` | `FxferConfigurationChangedV1` | `aggregate_type=ftp_server`, `change_kind=created`; raw password is not carried.
| `FtpServerUpdated` | `FxferConfigurationChangedV1` | `aggregate_type=ftp_server`, `change_kind=updated`.
| `FtpServerInvalidated` | `FxferConfigurationChangedV1` | `aggregate_type=ftp_server`, `change_kind=invalidated`.
| `FtsSourceLocationEvents` | `FxferConfigurationChangedV1` / `FxferSuspensionStateChangedV1` | Legacy `ModelType` identity is not carried; tenant-scoping uses the canonical envelope.
| `FtsSourceLocationAdded` | `FxferConfigurationChangedV1` | `aggregate_type=transfer_configuration`, `change_kind=created`.
| `FtsSourceLocationUpdated` | `FxferConfigurationChangedV1` | `aggregate_type=transfer_configuration`, `change_kind=updated`.
| `FtsSourceLocationInvalidated` | `FxferConfigurationChangedV1` | `aggregate_type=transfer_configuration`, `change_kind=invalidated`.
| `FtsDestinationLocationAdded` | `FxferConfigurationChangedV1` | Destination is a configuration sub-entity; target event publishes as `transfer_configuration` with `change_kind=updated`.
| `FtsInboundTargetAdded` | `FxferConfigurationChangedV1` | `aggregate_type=inbound_target`, `change_kind=created`.
| `FtsInboundTargetInvalidated` | `FxferConfigurationChangedV1` | `aggregate_type=inbound_target`, `change_kind=invalidated`.
| `IntegrationFileAdded` | `FxferConfigurationChangedV1` | `aggregate_type=integration_file`, `change_kind=created`.
| `IntegrationFileUpdated` | `FxferConfigurationChangedV1` | `aggregate_type=integration_file`, `change_kind=updated`.

## Dependencies

None.

## Enterprise-Architecture standards applied

- `standards/application/event-driven/STD-APP-EVT-002-dotnet-kafka-producer.md` section `Mandatory Requirements` requirement `[REQ-EVT-002-006] - Event Metadata Requirements` (correlation id, timestamp, version, producer)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant isolation and tenant scoping)
- `strategy/guidance/security/GUID-SEC-PHI-001-phi-data-classification-and-handling.md` section `Kafka Event Payload PHI` (schema governance for sensitive fields; payload not logged)
- `strategy/guidance/security/GUID-SEC-PHI-001-phi-data-classification-and-handling.md` section `Log Redaction` (log envelope identifiers; omit payload from logs)
