---
type: Interface Contract
title: 'Contract: fxfer-transfer-worker'
description: Endpoints, events, data and interfaces the fxfer-transfer-worker deployable exposes.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-transfer-worker
tags:
- deployable
- provider-file-transfer-v2
- worker
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:23:01Z
sources:
- id: component-backend-batch-business
  resource: /technical/components/backend-batch-business/SPEC.md
  title: Component placed in it
- id: component-backend-business-helpers
  resource: /technical/components/backend-business-helpers/SPEC.md
  title: Component placed in it
- id: component-backend-business-helpers-2
  resource: /technical/components/backend-business-helpers-2/SPEC.md
  title: Component placed in it
- id: component-backend-data-access-batch-data-access
  resource: /technical/components/backend-data-access-batch-data-access/SPEC.md
  title: Component placed in it
- id: component-backend-data-access-batch-data-access-2
  resource: /technical/components/backend-data-access-batch-data-access-2/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (11 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-transfer-worker
  generation:
    status: done
    attempts: 1
  path: workers/fxfer-transfer-worker
  kind: worker
  stack: .NET 10 worker runtime
  components: 11
  depends_on:
  - fxfer-domain-api
  body_sha256: 9eb47a88f2a1d83cf2d6d37f7fba4d0a47b65441ee1aa00662074f62e3ae1b87
---

# Contract: `fxfer-transfer-worker`

Deployable path: `workers/fxfer-transfer-worker`

This deployable is an internal FXFER worker. It does not expose business HTTP APIs; other deployables rely on it through its ability to consume transfer work items reliably and to record durable, idempotent outcomes via `fxfer-domain-api`.

## Tenancy and processing context

- Work items are tenant-scoped.
- `tenant_id` is mandatory for processing and is taken from the work item envelope (or equivalent scheduler context).
- The worker must be incapable of processing a work item without an explicit `tenant_id`.

## Authentication

- The worker runs under a service identity.
- Outbound calls to `fxfer-domain-api` use `Authorization: Bearer <JWT>` (service-to-service token issued by the platform identity broker).

## Observability context propagation

Required propagation across work items and outbound calls:

- Correlation id: `X-Correlation-ID` (UUID v4)
- W3C Trace Context: `traceparent`, `tracestate`, `baggage`

The worker maps `tenant_id`, `correlation_id`, and `trace_id` into OpenTelemetry attributes/log fields for every work item handled.

## HTTP surface

The worker does not expose FXFER business endpoints. If an HTTP listener is enabled, it exists only for platform health endpoints (within the runtime boundary):

- `GET /api/health/live`
- `GET /api/health/ready`

## Consumed work items

The worker consumes tenant-scoped transfer execution work items from the Event Backbone (SNS/SQS pattern) via the platform Event Adapter.

Work items follow the platform canonical envelope shape (same as domain events), with tenant-scoped routing and observability fields.

Canonical envelope fields:

| Field | Type | Notes |
| --- | --- | --- |
| `tenant_id` | `string` | Required; tenant identifier used for routing and secrets resolution |
| `event_name` | `string` | Work item family name (see below) |
| `event_version` | `string` | Schema version (e.g. `v1`) |
| `event_id` | `uuid` | Unique work item id (used for dedup/idempotency) |
| `occurred_at` | `date-time` | UTC timestamp |
| `correlation_id` | `string` | Correlation id (logged + attached to spans) |
| `trace_id` | `string` | W3C trace id (continued/linked by consumer) |
| `producer` | `string` | Producing component/service name |

### `Fxfer.TransferWorkItem` (v1)

Purpose: instruct the worker to execute a tenant-scoped unit of transfer work (scheduled or operator-triggered) that references authoritative execution intent maintained by the FXFER domain.

Payload (no secrets; no payload bytes):

| Field | Type | Notes |
| --- | --- | --- |
| `execution_id` | `uuid` | Required; authoritative execution intent id (run/attempt reference) |
| `source_location_id` | `uuid` | Required; transfer configuration/source location to execute |
| `destination_id` | `uuid` | Optional; when present the work item targets one destination attempt |
| `file_identity_key` | `string` | Optional; opaque stable identity used for duplicate prevention |
| `run_mode` | `string` | Required; `scheduled` or `rerun` |
| `requested_by` | `string` | Optional; actor id for privileged rerun initiation |

Processing requirements:

- At-least-once delivery is assumed.
- Work item handling is idempotent by `(tenant_id, execution_id)` (and destination/file identity when applicable).
- Poison messages are dead-lettered; recovery is via controlled redrive tooling.

## CLI surface (privileged rerun mode)

Privileged reruns are performed via a CLI mode targeting a specific tenant and tracked file identifier.

### `rerun` mode

Invocation:

```text
fxfer-transfer-worker rerun --tenant-id <uuid> --file-id <uuid>
```

Inputs:

- `tenant-id`: required; 36-character UUID.
- `file-id`: required; tracked file identifier (UUID).

Outputs:

- Exit code: `0` on success; non-zero on failure.

### Legacy-compatible batch door (parity)

For parity with the legacy batch runner surface described by `run-file-transfer-batch`, the worker retains a batch/CLI door that accepts the following argument patterns:

- One argument: `<tenant-id>` (process all eligible work for the tenant)
- Three arguments: `<any-string> <config-id-or-guid> <tenant-id>` (selective run; the first argument is not validated)

The batch door reports a single numeric result and reports `-1` on failure.

## Published events

None. The worker records outcomes via `fxfer-domain-api`, which publishes domain events (for example `Fxfer.DeliveryOutcomeRecorded`).

## Data owned

None. The worker does not own databases or projection tables.

## Enterprise-Architecture standards applied

- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel SDK; OTLP export; W3C trace context propagation)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (messaging and dependency semantic conventions)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (correlation id propagation across async boundaries)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (UUID v4 correlation id; `X-Correlation-ID` naming)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant isolation expectations)
- `patterns/application/event-driven/PTN-APP-EVT-016-messaging-work-queue-pattern.md` section `Executive Summary > Solution Overview` (work-queue consumption and scaling)
- `patterns/application/event-driven/PTN-APP-EVT-019-messaging-dead-letter-channel-pattern.md` section `Solution Overview` (DLQ posture and recovery)

