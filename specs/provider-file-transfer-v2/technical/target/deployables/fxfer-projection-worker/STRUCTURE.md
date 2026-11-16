---
type: Deployable Structure
title: 'Structure: fxfer-projection-worker'
description: 'Skeleton of the fxfer-projection-worker deployable: projects, folders and cross-cutting setup.'
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-projection-worker
tags:
- deployable
- projector
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:14:33Z
sources:
- id: component-backend-data-access-event-handlers
  resource: /technical/components/backend-data-access-event-handlers/SPEC.md
  title: Component placed in it
- id: component-backend-data-access-event-handlers-2
  resource: /technical/components/backend-data-access-event-handlers-2/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (2 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-projection-worker
  generation:
    status: done
    attempts: 1
  path: workers/fxfer-projection-worker
  kind: projector
  stack: .NET 10 worker runtime
  components: 2
  depends_on:
  - fxfer-domain-api
  - fxfer-db-schema
  body_sha256: 93e48e1d79c151a9cdb71029b5610dbd7dc9a34c6f6556eb565be81b3d3829ec
---

# Structure: `fxfer-projection-worker`

Deployable path: `workers/fxfer-projection-worker`

This deployable is a .NET 10 worker that runs as a tenant-scoped projector: it consumes FXFER domain events from the Event Backbone and updates query/read-model tables in each tenant's Aurora PostgreSQL database. It is a competing-consumer worker (scale out by increasing consumer replicas) and must tolerate at-least-once delivery semantics via idempotent projection updates.

## Folder skeleton (new repository)

Create the following skeleton at `workers/fxfer-projection-worker`:

```text
workers/fxfer-projection-worker/
  Fxfer.ProjectionWorker.sln
  README.md
  src/
    Worker/
      Fxfer.ProjectionWorker.Worker.csproj
      Program.cs
      appsettings.json
      appsettings.Development.json
      Configuration/
        DomainApiClientOptions.cs
        EventBackboneConsumerOptions.cs
        TenantDatabaseOptions.cs
      Eventing/
        FxferEventConsumer.cs
        FxferEventDispatcher.cs
        FxferEventEnvelope.cs
      Health/
        HealthChecks.cs
    Infrastructure/
      DataAccess/
        FxferProjectionDbContext.cs
        Entities/
      Projections/
        (component placement: projection writers)
  test/
    Fxfer.ProjectionWorker.Tests/
      Fxfer.ProjectionWorker.Tests.csproj
  Dockerfile
  helm/
    fxfer-projection-worker/
      Chart.yaml
      values.yaml
      templates/
        deployment.yaml
        service.yaml
        configmap.yaml
        hpa.yaml
```

Notes:

- `src/Infrastructure/Projections/` is the component placement location defined in `technical/target/architecture.yaml` for `backend-data-access-event-handlers` and `backend-data-access-event-handlers-2`.
- The worker does not define public business APIs. If an HTTP listener is enabled, it exists only for platform health endpoints.

## Runtime entry point

- `src/Worker/Program.cs` hosts a .NET Generic Host worker process.
- The process starts an event consumer loop and dispatches events to projection handlers.
- The worker exposes platform-standard health endpoints (within the runtime boundary):
  - `GET /api/health/live`
  - `GET /api/health/ready`

## Event consumption and dispatch

- The worker consumes messages from the Event Backbone via the platform Event Adapter subscription for FXFER domain events.
- For each message, the worker:
  - extracts the canonical envelope fields (`tenant_id`, `event_name`, `event_version`, `event_id`, `occurred_at`, `correlation_id`, `trace_id`, `producer`) from message attributes/headers (not only JSON body).
  - establishes logging/tracing scope using `tenant_id`, `correlation_id`, and `trace_id`.
  - dispatches to an event-specific handler based on `event_name` and `event_version`.
- Failures are handled by bounded retry (where safe) and dead-lettering through the queue's DLQ configuration. The worker does not silently drop events.

## Tenant resolution and database routing

- Tenant context comes from the message envelope `tenant_id` and is treated as mandatory.
- The worker uses `tenant_id` to resolve tenant-scoped database credentials/connection information (Secrets Manager + KMS) and connects to exactly one tenant database per processed event.
- Projection persistence uses EF Core + Npgsql. The worker does not run schema migrations on startup; schema rollout is owned by `fxfer-db-schema`.

## Observability (logs, metrics, traces)

- OpenTelemetry SDK is configured for traces/metrics/logs and exports via OTLP to the platform OTel Collector.
- Message handling creates/continues trace context using W3C Trace Context and maps `correlation_id` + `tenant_id` into span/log attributes.
- All logs are structured and include `trace_id` and `span_id` when emitted within an active trace context.

## Resilience and idempotency

- Event handling is idempotent: projection updates use `event_id` (and domain ids) to avoid double-applying changes under at-least-once delivery.
- Outbound HTTP calls to `fxfer-domain-api` (when used for enrichment reads) use bounded retries with jitter and timeouts; failures are observable and lead to retry/DLQ rather than infinite loops.

## Enterprise-Architecture standards applied

- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel SDK; OTLP export; W3C trace context propagation)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (span/log attribute naming; messaging semantic conventions)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (extract/generate correlation ids at entry points including message consumers)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` UUID v4 format and propagation posture)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant data isolation and auditability)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (database-per-tenant; tenant-scoped credentials)
- `patterns/application/event-driven/PTN-APP-EVT-017-messaging-publish-subscribe-pattern.md` section `Solution Overview` (pub-sub event distribution with subscriber-owned queues; subscriber failure isolation)
- `patterns/application/event-driven/PTN-APP-EVT-016-messaging-work-queue-pattern.md` section `Executive Summary > Solution Overview` (competing consumers and horizontal scaling via queue consumption)
- `patterns/application/event-driven/PTN-APP-EVT-019-messaging-dead-letter-channel-pattern.md` section `Solution Overview` (DLQ posture for failed messages; operational visibility and recovery path)

