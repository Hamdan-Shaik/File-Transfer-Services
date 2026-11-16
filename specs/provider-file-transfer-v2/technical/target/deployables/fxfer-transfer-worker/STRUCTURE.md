---
type: Deployable Structure
title: 'Structure: fxfer-transfer-worker'
description: 'Skeleton of the fxfer-transfer-worker deployable: projects, folders and cross-cutting setup.'
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
  body_sha256: 7354090d46d17424896fcdac2db6f2a7c0256d9540300fbcac06bd70d202c1a7
---

# Structure: `fxfer-transfer-worker`

Deployable path: `workers/fxfer-transfer-worker`

This deployable is a .NET 10 worker that consumes tenant-scoped transfer work items from the Event Backbone, performs protocol I/O (FTP/SFTP/FTPS/UNC/etc as applicable) using tenant-scoped secrets, and records authoritative outcomes by calling `fxfer-domain-api`. It is treated as an edge-touching component and must follow strict payload handling (no payload bytes in logs/traces/events), bounded retries, DLQ quarantine, and durable outcome recording.

## Folder skeleton (new repository)

Create the following skeleton at `workers/fxfer-transfer-worker`:

```text
workers/fxfer-transfer-worker/
  Fxfer.TransferWorker.sln
  README.md
  src/
    Worker/
      Fxfer.TransferWorker.Worker.csproj
      Program.cs
      appsettings.json
      appsettings.Development.json
      Configuration/
        DomainApiClientOptions.cs
        EventBackboneConsumerOptions.cs
        SecretsOptions.cs
        StagingOptions.cs
        TransferExecutionOptions.cs
      Eventing/
        TransferWorkItemConsumer.cs
        TransferWorkItemEnvelope.cs
      Health/
        HealthChecks.cs
    Application/
      (component placement: batch + transfer execution business)
      Helpers/
        (component placement: encryption/event/notification helpers)
    Contracts/
      (component placement: legacy batch contracts)
    Domain/
      Constants/
        (component placement: shared constants)
    Infrastructure/
      Adapters/
        (component placement: domain API adapters)
      DataAccess/
        (component placement: legacy data-access surfaces; replaced by domain API calls)
  test/
    Fxfer.TransferWorker.Tests/
      Fxfer.TransferWorker.Tests.csproj
  Dockerfile
  helm/
    fxfer-transfer-worker/
      Chart.yaml
      values.yaml
      templates/
        deployment.yaml
        service.yaml
        configmap.yaml
        hpa.yaml
```

Notes:

- Component placement locations are defined in `technical/target/architecture.yaml`.
- The worker does not define public business APIs. If an HTTP listener is enabled, it exists only for platform health endpoints.

## Runtime entry point

- `src/Worker/Program.cs` hosts a .NET Generic Host worker process.
- On start-up, the process:
  - configures OpenTelemetry (logs/metrics/traces) and structured logging.
  - configures a work-queue consumer (Event Backbone subscription/queue).
  - dispatches each work item to transfer execution logic in `src/Application/`.
- The worker exposes platform-standard health endpoints (within the runtime boundary):
  - `GET /api/health/live`
  - `GET /api/health/ready`

## Work item consumption (Event Backbone)

- The worker is a competing-consumer worker (scale out by increasing replicas) and must tolerate at-least-once delivery.
- Work items are tenant-scoped. The worker treats `tenant_id` as mandatory and uses it as the routing key for:
  - resolving tenant-scoped endpoint secrets (Secrets Manager + KMS)
  - calling tenant-scoped FXFER domain APIs to read configuration and record outcomes
- Failed work items are retried with bounded backoff. Poison messages are dead-lettered (DLQ) for operator-visible recovery; the worker does not silently drop work.

## Secrets and staging

- Endpoint credentials/certificates are retrieved per tenant using the worker service identity (least privilege) and are held in memory only.
- Transfers use safe staging:
  - tenant-scoped working directories
  - encrypted-at-rest staging
  - strict lifecycle (create  use  cleanup)

## Outcome recording (domain APIs)

- The worker does not write directly to tenant databases.
- The worker records tracking and delivery outcomes through `fxfer-domain-api` command endpoints.
- Outcome writes must be idempotent, since duplicate work item delivery is assumed.

## Observability (logs, metrics, traces)

- OpenTelemetry SDK is configured for traces/metrics/logs and exports via OTLP to the platform OTel Collector.
- Each work item handling run:
  - continues or creates W3C trace context.
  - tags spans/logs with `tenant_id`, `correlation_id`, and `trace_id`.
  - emits per-endpoint dependency spans for external protocol calls and for domain API HTTP calls.
- Payload bytes and secret material are never logged or attached to traces.

## Resilience and idempotency

- External endpoint interactions use explicit timeouts and bounded retries with jitter; repeated failures surface as durable outcomes and/or DLQ quarantine.
- Recording outcomes to `fxfer-domain-api` uses bounded retries paired with command-side idempotency.

## Enterprise-Architecture standards applied

- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel SDK; OTLP export; W3C trace context propagation)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (span/log attribute naming; messaging + dependency semantic conventions)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (correlation id extraction/generation at entry points including message consumers)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` UUID v4 format and propagation posture)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant data isolation; no cross-tenant access)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (tenant-scoped secrets and execution context)
- `patterns/application/event-driven/PTN-APP-EVT-016-messaging-work-queue-pattern.md` section `Executive Summary > Solution Overview` (competing consumers and horizontal scaling via a work queue)
- `patterns/application/event-driven/PTN-APP-EVT-019-messaging-dead-letter-channel-pattern.md` section `Solution Overview` (DLQ posture for failed work items; operational recovery)

