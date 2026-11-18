---
type: Target Placement
title: technical/components/backend-data-access-event-handlers-2/TARGET
description: Where the backend-data-access-event-handlers-2 component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-event-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:06:12Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-event-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-event-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 4
  methods: 18
  owned_rules: 0
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-projection-worker
  folder: src/Infrastructure/Projections
  body_sha256: 990ab1fae1f2da9fe9cd7e2ae33898d5f868fbf8a9d089b263890ad8063a75c2
---

﻿# Target: backend-data-access-event-handlers-2

Where: `workers/fxfer-projection-worker/src/Infrastructure/Projections/`

## What to create

Create projection writer classes in `workers/fxfer-projection-worker/src/Infrastructure/Projections/` that update the tenant-scoped projection tables described in `technical/target/deployables/fxfer-projection-worker/CONTRACT.md`.

- `FtpServerProjectionWriter`
  - Public handlers:
    - `Task HandleFtpServerCreatedAsync(FxferEventEnvelope envelope, FtpServerProjectionPayload payload, CancellationToken ct)`
    - `Task HandleFtpServerUpdatedAsync(FxferEventEnvelope envelope, FtpServerProjectionPayload payload, CancellationToken ct)`
    - `Task HandleFtpServerInvalidatedAsync(FxferEventEnvelope envelope, FtpServerProjectionPayload payload, CancellationToken ct)`
- `FtsInboundTargetProjectionWriter`
  - Public handlers:
    - `Task HandleInboundTargetCreatedAsync(FxferEventEnvelope envelope, InboundTargetProjectionPayload payload, CancellationToken ct)`
    - `Task HandleInboundTargetInvalidatedAsync(FxferEventEnvelope envelope, InboundTargetProjectionPayload payload, CancellationToken ct)`
- `FtsIntegrationFileProjectionWriter`
  - Public handlers:
    - `Task HandleIntegrationFileCreatedAsync(FxferEventEnvelope envelope, IntegrationFileCreatedProjectionPayload payload, CancellationToken ct)`
    - `Task HandleIntegrationFileUpdatedAsync(FxferEventEnvelope envelope, IntegrationFileUpdatedProjectionPayload payload, CancellationToken ct)`
- `FtsSourceLocationProjectionWriter`
  - Public handlers:
    - `Task HandleSourceLocationCreatedAsync(FxferEventEnvelope envelope, SourceLocationProjectionPayload payload, CancellationToken ct)`
    - `Task HandleSourceLocationUpdatedAsync(FxferEventEnvelope envelope, SourceLocationProjectionPayload payload, CancellationToken ct)`
    - `Task HandleSourceLocationInvalidatedAsync(FxferEventEnvelope envelope, SourceLocationInvalidatedProjectionPayload payload, CancellationToken ct)`
  - Private helpers (retain legacy behavior):
    - `Task AddDestinationAddressAsync(List<DestinationProjectionPayload> destinations, string userId, CancellationToken ct)`
    - `void AddDestination(DestinationProjectionPayload dest, string userId)`
    - `Task UpdateDestinationAsync(DestinationProjectionPayload dest, string userId, CancellationToken ct)`

Notes:

- Tenant isolation and database routing are handled outside these classes by the worker’s envelope processing (`tenant_id` → tenant DB connection). Projection tables in the tenant DB do not include `tenant_id` columns.
- These writers should use `FxferProjectionDbContext` (from `workers/fxfer-projection-worker/src/Infrastructure/DataAccess/FxferProjectionDbContext.cs`) and EF Core async APIs.
- Time values set by these writers should use `DateTime.UtcNow` to match legacy.
- Secrets: the target projection schema stores password references (e.g. `ftp_password_secret_ref`) rather than plaintext passwords; payloads for FTP server projections must therefore carry a secret reference, not the secret value.

## Legacy → Target mapping

| Legacy method | Target method | Notes |
| --- | --- | --- |
| `FtpServerProjectionWriter.Handle(FtpServerAdded)` | `FtpServerProjectionWriter.HandleFtpServerCreatedAsync(...)` | Use payload fields equivalent to legacy properties; timestamps set to UTC now. |
| `FtpServerProjectionWriter.Handle(FtpServerUpdated)` | `FtpServerProjectionWriter.HandleFtpServerUpdatedAsync(...)` | Update the same set of fields as legacy. |
| `FtpServerProjectionWriter.Handle(FtpServerInvalidated)` | `FtpServerProjectionWriter.HandleFtpServerInvalidatedAsync(...)` | Sets `is_valid` and updates `operator_id` and `last_modified_ts`. |
| `FtpServerProjectionWriter.AddFtpServer(FtpServerAdded)` | (inlined) | In target, creation logic is implemented inside `HandleFtpServerCreatedAsync(...)`. |
| `FtsInboundTargetProjectionWriter.Handle(FtsInboundTargetAdded)` | `FtsInboundTargetProjectionWriter.HandleInboundTargetCreatedAsync(...)` | Preserve legacy behavior: ignore payload inbound_target_id and generate a new id. |
| `FtsInboundTargetProjectionWriter.Handle(FtsInboundTargetInvalidated)` | `FtsInboundTargetProjectionWriter.HandleInboundTargetInvalidatedAsync(...)` | Preserve legacy behavior: `is_valid=false` hard-coded and `operator_id` derived from the envelope’s user/operator id. |
| `FtsInboundTargetProjectionWriter.AddFtsInboundTarget(...)` | (inlined) | In target, creation logic is implemented inside `HandleInboundTargetCreatedAsync(...)`. |
| `FtsInboundTargetProjectionWriter.MapFtsInboundTarget(...)` | (optional helper) | Keep as a private helper if it keeps the code smaller/clearer. |
| `FtsIntegrationFileProjectionWriter.Handle(IntegrationFileAdded)` | `FtsIntegrationFileProjectionWriter.HandleIntegrationFileCreatedAsync(...)` | Preserve legacy behavior: one row per `group_info` with `number_of_records > 0`, each with a new id. |
| `FtsIntegrationFileProjectionWriter.Handle(IntegrationFileUpdated)` | `FtsIntegrationFileProjectionWriter.HandleIntegrationFileUpdatedAsync(...)` | Preserve legacy behavior: set `is_processed=true` and update config_id/is_valid/operator_id. |
| `FtsIntegrationFileProjectionWriter.AddFtsIntegrationFile(...)` | (inlined) | Implement the group iteration inside `HandleIntegrationFileCreatedAsync(...)`. |
| `FtsIntegrationFileProjectionWriter.MapFtsIntegrationFile(...)` | (optional helper) | Keep as a private helper if it keeps the code smaller/clearer. |
| `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationAdded)` | `FtsSourceLocationProjectionWriter.HandleSourceLocationCreatedAsync(...)` | Preserve legacy behavior: generate a new `fts_source_location_id` and use it for all destinations created. |
| `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationUpdated)` | `FtsSourceLocationProjectionWriter.HandleSourceLocationUpdatedAsync(...)` | Preserve legacy behavior including unconditional destination upsert call and invalidation cascade when `is_valid=false`. |
| `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationInvalidated)` | `FtsSourceLocationProjectionWriter.HandleSourceLocationInvalidatedAsync(...)` | Preserve legacy behavior: invalidate source location and invalidate all destinations for it. |
| `FtsSourceLocationProjectionWriter.AddDestinationAddress(...)` | `FtsSourceLocationProjectionWriter.AddDestinationAddressAsync(...)` | Preserve legacy behavior: existence check by `destination_id` against all destination ids. |
| `FtsSourceLocationProjectionWriter.AddDestination(...)` | `FtsSourceLocationProjectionWriter.AddDestination(...)` | Preserve legacy behavior: generate a new `destination_id` for inserts (ignore input id). |
| `FtsSourceLocationProjectionWriter.UpdateDestination(...)` | `FtsSourceLocationProjectionWriter.UpdateDestinationAsync(...)` | Preserve legacy behavior: load by destination_id then update fields; null result should fail as legacy does. |

## Enterprise-Architecture standards applied

- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (database-per-tenant routing; projection writes are tenant-scoped)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel scopes per envelope: tenant_id/correlation_id/trace_id)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (messaging semantic conventions for spans emitted by the worker around handler execution)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (propagate correlation id across async event handling)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (UUID v4 correlation id naming and format)
- `patterns/application/event-driven/PTN-APP-EVT-017-messaging-publish-subscribe-pattern.md` section `Solution Overview` (subscriber-owned queue; independent scaling)
- `patterns/application/event-driven/PTN-APP-EVT-016-messaging-work-queue-pattern.md` section `Solution Overview` (competing-consumer worker semantics)
- `patterns/application/event-driven/PTN-APP-EVT-019-messaging-dead-letter-channel-pattern.md` section `Solution Overview` (DLQ posture for failed projection events)

