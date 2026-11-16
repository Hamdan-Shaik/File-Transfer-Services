---
type: Interface Contract
title: technical/components/shared-contracts-events/CONTRACT
description: What the shared-contracts-events component offers other components and what it uses from them.
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
  body_sha256: fbb2394779d162a08a43fc6ea5458c8c3a1184c65583c96e48c4cc13af5f9296
---

﻿# Contract: `shared-contracts-events`

This component defines the target-state event contract types used by `fxfer-domain-api` when publishing FXFER tenant-scoped domain events.
It includes the canonical envelope shape and the schema-versioned payload DTOs for the event families listed in `technical/target/deployables/fxfer-domain-api/CONTRACT.md`.

## Offers

- **Canonical event envelope**: `FxferEventEnvelope` (`tenant_id`, `event_name`, `event_version`, `event_id`, `occurred_at`, `correlation_id`, `trace_id`, `producer`).
- **Event wrapper**: `FxferDomainEvent<TPayload>` with `Envelope` + `Payload`.
- **Event payloads (v1)**:
  - `FxferConfigurationChangedV1`
  - `FxferSuspensionStateChangedV1`
  - `FxferDeliveryOutcomeRecordedV1`

Security posture applied to published events:

- Envelope fields are also published in message headers/attributes for routing and observability.
- Event payloads do not include secrets (no raw passwords/keys/certificates).
- Event payloads do not include raw provider file contents.

## Uses

None.

## Data owned

None.

## Enterprise-Architecture standards applied

- `standards/application/event-driven/STD-APP-EVT-002-dotnet-kafka-producer.md` section `Mandatory Requirements` requirement `[REQ-EVT-002-006] - Event Metadata Requirements`
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements`
- `strategy/guidance/security/GUID-SEC-PHI-001-phi-data-classification-and-handling.md` section `Kafka Event Payload PHI`
- `strategy/guidance/security/GUID-SEC-PHI-001-phi-data-classification-and-handling.md` section `Log Redaction`
