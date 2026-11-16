---
type: Target Placement
title: technical/components/backend-data-access-event-handlers/TARGET
description: Where the backend-data-access-event-handlers component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-event-handlers
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T00:47:50Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-manage-integration-files
  resource: /technical/flows/manage-integration-files/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-data-access-event-handlers/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-event-handlers
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 4
  methods: 18
  owned_rules: 18
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  depends_on:
  - backend-data-access-event-handlers-2
  deployable: fxfer-projection-worker
  folder: src/Infrastructure/Projections
  body_sha256: 81fc1a17d1ffa4ce3059025757800f33bec9f027590450c5e535eb03cc5af497
---

﻿# Target: backend-data-access-event-handlers

Where: `workers/fxfer-projection-worker/src/Infrastructure/Projections/`

## What to create

This component’s legacy code is a duplicate UA3-namespace variant of the projection writers specified by component `backend-data-access-event-handlers-2`. The target repository must contain only one set of projection writer types.

Treat this component as merged into the target types created by `technical/components/backend-data-access-event-handlers-2/TARGET.md`:

- Do not create a second copy of `FtpServerProjectionWriter`, `FtsInboundTargetProjectionWriter`, `FtsIntegrationFileProjectionWriter`, or `FtsSourceLocationProjectionWriter`.
- Ensure the shared target implementations preserve the UA3 legacy behaviors and the business rules owned by this component (listed in `technical/components/backend-data-access-event-handlers/SPEC.md`).

Target projection-writer surface (shared):

- `FtpServerProjectionWriter`
  - `Task HandleFtpServerCreatedAsync(FxferEventEnvelope envelope, FtpServerProjectionPayload payload, CancellationToken ct)`
  - `Task HandleFtpServerUpdatedAsync(FxferEventEnvelope envelope, FtpServerProjectionPayload payload, CancellationToken ct)`
  - `Task HandleFtpServerInvalidatedAsync(FxferEventEnvelope envelope, FtpServerProjectionPayload payload, CancellationToken ct)`
- `FtsInboundTargetProjectionWriter`
  - `Task HandleInboundTargetCreatedAsync(FxferEventEnvelope envelope, InboundTargetProjectionPayload payload, CancellationToken ct)`
  - `Task HandleInboundTargetInvalidatedAsync(FxferEventEnvelope envelope, InboundTargetProjectionPayload payload, CancellationToken ct)`
- `FtsIntegrationFileProjectionWriter`
  - `Task HandleIntegrationFileCreatedAsync(FxferEventEnvelope envelope, IntegrationFileCreatedProjectionPayload payload, CancellationToken ct)`
  - `Task HandleIntegrationFileUpdatedAsync(FxferEventEnvelope envelope, IntegrationFileUpdatedProjectionPayload payload, CancellationToken ct)`
- `FtsSourceLocationProjectionWriter`
  - `Task HandleSourceLocationCreatedAsync(FxferEventEnvelope envelope, SourceLocationProjectionPayload payload, CancellationToken ct)`
  - `Task HandleSourceLocationUpdatedAsync(FxferEventEnvelope envelope, SourceLocationProjectionPayload payload, CancellationToken ct)`
  - `Task HandleSourceLocationInvalidatedAsync(FxferEventEnvelope envelope, SourceLocationInvalidatedProjectionPayload payload, CancellationToken ct)`
  - Private helpers (keep legacy behavior):
    - `Task AddDestinationAddressAsync(List<DestinationProjectionPayload> destinations, string userId, CancellationToken ct)`
    - `void AddDestination(DestinationProjectionPayload dest, string userId)`
    - `Task UpdateDestinationAsync(DestinationProjectionPayload dest, string userId, CancellationToken ct)`

Notes:

- Persistence uses `FxferProjectionDbContext` (EF Core + Npgsql) and async APIs.
- Time values set by these writers use `DateTime.UtcNow` to match legacy rules.
- Idempotency/dedup for at-least-once delivery is enforced by the projection worker infrastructure (dedup by `(tenant_id, event_id)`), not inside these writers.

## Legacy → Target mapping

| Legacy method | Target method | Notes |
| --- | --- | --- |
| `FtpServerProjectionWriter.Handle(FtpServerAdded)` | `FtpServerProjectionWriter.HandleFtpServerCreatedAsync(...)` | Merged into the single target type created by component `backend-data-access-event-handlers-2`. Implements rule F02-035. |
| `FtpServerProjectionWriter.Handle(FtpServerUpdated)` | `FtpServerProjectionWriter.HandleFtpServerUpdatedAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Implements rule F02-036. |
| `FtpServerProjectionWriter.Handle(FtpServerInvalidated)` | `FtpServerProjectionWriter.HandleFtpServerInvalidatedAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Implements rule F02-037. |
| `FtpServerProjectionWriter.AddFtpServer(FtpServerAdded)` | (inlined) | In target, creation logic is implemented inside `HandleFtpServerCreatedAsync(...)`. |
| `FtsInboundTargetProjectionWriter.Handle(FtsInboundTargetAdded)` | `FtsInboundTargetProjectionWriter.HandleInboundTargetCreatedAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Preserve legacy behavior: ignore payload inbound_target_id and generate a new id. |
| `FtsInboundTargetProjectionWriter.Handle(FtsInboundTargetInvalidated)` | `FtsInboundTargetProjectionWriter.HandleInboundTargetInvalidatedAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Preserve legacy behavior: `is_valid=false` hard-coded and `operator_id` derived from the envelope user/operator id. |
| `FtsInboundTargetProjectionWriter.AddFtsInboundTarget(...)` | (inlined) | Creation logic implemented inside `HandleInboundTargetCreatedAsync(...)`. |
| `FtsInboundTargetProjectionWriter.MapFtsInboundTarget(...)` | (optional helper) | Keep as a private helper only if it keeps the code smaller/clearer. |
| `FtsIntegrationFileProjectionWriter.Handle(IntegrationFileAdded)` | `FtsIntegrationFileProjectionWriter.HandleIntegrationFileCreatedAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Preserve legacy behavior: one row per `group_info` with `number_of_records > 0`, each with a new id. |
| `FtsIntegrationFileProjectionWriter.Handle(IntegrationFileUpdated)` | `FtsIntegrationFileProjectionWriter.HandleIntegrationFileUpdatedAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Implements rules F03-012 and F03-013. |
| `FtsIntegrationFileProjectionWriter.AddFtsIntegrationFile(...)` | (inlined) | Implement the group iteration inside `HandleIntegrationFileCreatedAsync(...)`. |
| `FtsIntegrationFileProjectionWriter.MapFtsIntegrationFile(...)` | (optional helper) | Keep as a private helper only if it keeps the code smaller/clearer. |
| `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationAdded)` | `FtsSourceLocationProjectionWriter.HandleSourceLocationCreatedAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Implements rules F01-048 to F01-054. |
| `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationUpdated)` | `FtsSourceLocationProjectionWriter.HandleSourceLocationUpdatedAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Implements rules F01-073 to F01-075. |
| `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationInvalidated)` | `FtsSourceLocationProjectionWriter.HandleSourceLocationInvalidatedAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Implements rules F01-076 and F01-077. |
| `FtsSourceLocationProjectionWriter.AddDestinationAddress(...)` | `FtsSourceLocationProjectionWriter.AddDestinationAddressAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Implements rule F01-078. |
| `FtsSourceLocationProjectionWriter.AddDestination(...)` | `FtsSourceLocationProjectionWriter.AddDestination(...)` | Merged into component `backend-data-access-event-handlers-2`. Preserve legacy behavior: generate a new `destination_id` for inserts (ignore input id). |
| `FtsSourceLocationProjectionWriter.UpdateDestination(...)` | `FtsSourceLocationProjectionWriter.UpdateDestinationAsync(...)` | Merged into component `backend-data-access-event-handlers-2`. Preserve legacy behavior: load by destination_id then update fields; null result should fail as legacy does. |

## Dependencies

- Depends on component `backend-data-access-event-handlers-2` for the shared target type placement (single implementation).
- Uses `FxferProjectionDbContext` + EF Core for persistence.
- Relies on the worker envelope (`FxferEventEnvelope`) for tenant-scoped context, correlation, and tracing scope.

## Enterprise-Architecture standards applied

- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (database-per-tenant routing; projection writes are tenant-scoped)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel scopes per envelope: tenant_id/correlation_id/trace_id)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (messaging semantic conventions for spans emitted by the worker around handler execution)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (propagate correlation id across async event handling)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (UUID v4 correlation id naming and format)
- `patterns/application/event-driven/PTN-APP-EVT-016-messaging-work-queue-pattern.md` section `Executive Summary > Solution Overview` (competing-consumer worker semantics)
- `patterns/application/event-driven/PTN-APP-EVT-017-messaging-publish-subscribe-pattern.md` section `Solution Overview` (subscriber-owned queue; independent scaling)
- `patterns/application/event-driven/PTN-APP-EVT-019-messaging-dead-letter-channel-pattern.md` section `Solution Overview` (DLQ posture for failed projection events)
- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Read Model Projection Architecture` (read-model projections derived from write-side events)
- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Projection Update Pattern` (projection updater responsibilities under eventual consistency)
- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Recommended Requirements` (read/write separation; multiple projections from the same data)
