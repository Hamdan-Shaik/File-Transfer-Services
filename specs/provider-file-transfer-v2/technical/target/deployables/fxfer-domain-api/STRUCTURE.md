---
type: Deployable Structure
title: 'Structure: fxfer-domain-api'
description: 'Skeleton of the fxfer-domain-api deployable: projects, folders and cross-cutting setup.'
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-domain-api
tags:
- deployable
- dotnet-service
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:03:10Z
sources:
- id: component-backend-bas-file-transfer-api
  resource: /technical/components/backend-bas-file-transfer-api/SPEC.md
  title: Component placed in it
- id: component-backend-business-aggregates
  resource: /technical/components/backend-business-aggregates/SPEC.md
  title: Component placed in it
- id: component-backend-business-aggregates-2
  resource: /technical/components/backend-business-aggregates-2/SPEC.md
  title: Component placed in it
- id: component-backend-business-command-handlers
  resource: /technical/components/backend-business-command-handlers/SPEC.md
  title: Component placed in it
- id: component-backend-business-command-handlers-2
  resource: /technical/components/backend-business-command-handlers-2/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (26 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-domain-api
  generation:
    status: done
    attempts: 1
  path: services/fxfer-domain-api
  kind: dotnet-service
  stack: .NET 10 (ASP.NET Core; AOT preferred)
  components: 26
  depends_on:
  - fxfer-db-schema
  body_sha256: 13f3ceac1ace0f9730fc5b2c759e886db334b8aa73f719322892a7efa7da6b7d
---

# Structure: `fxfer-domain-api`

Deployable path: `services/fxfer-domain-api`

This deployable is an ASP.NET Core service (AOT preferred) that owns the authoritative FXFER configuration + tracking write model and exposes private command/query HTTP APIs for other FXFER deployables (BFF, workers, SOAP facade).

## Folder skeleton (new repository)

Create the following skeleton at `services/fxfer-domain-api`:

```text
services/fxfer-domain-api/
  Fxfer.DomainApi.sln
  README.md
  openapi/
    fxfer-domain-api.v1.yaml
  src/
    Api/
      Fxfer.DomainApi.Api.csproj
      Program.cs
      appsettings.json
      appsettings.Development.json
      Configuration/
        AuthenticationOptions.cs
        TenantResolutionOptions.cs
      Middleware/
        CorrelationIdMiddleware.cs
        TenantContextMiddleware.cs
      Endpoints/
        Commands/
        Queries/
      Health/
        HealthChecks.cs
    Application/
      Commands/
      Queries/
    Domain/
      Aggregates/
      Constants/
      Validation/
      Profiles/
    Contracts/
      Commands/
      Queries/
      Domain/
      Events/
      Exceptions/
      Utilities/
    Infrastructure/
      DataAccess/
        FxferDbContext.cs
        Entities/
        Constants/
      Eventing/
        Outbox/
        EventAdapterClient.cs
  test/
    Fxfer.DomainApi.Tests/
      Fxfer.DomainApi.Tests.csproj
  Dockerfile
  helm/
    fxfer-domain-api/
      Chart.yaml
      values.yaml
      templates/
        deployment.yaml
        service.yaml
        ingress.yaml
        configmap.yaml
        hpa.yaml
```

Notes:

- The `src/` folder layout matches the component placement defined in `technical/target/architecture.yaml`.
- `openapi/fxfer-domain-api.v1.yaml` is the contract-first OpenAPI 3.1 source of truth for the HTTP contract consumed by other deployables.

## Runtime entry point

- `src/Api/Program.cs` hosts the ASP.NET Core pipeline and maps endpoints.
- The service exposes platform-standard health endpoints:
  - `GET /api/health/live`
  - `GET /api/health/ready`

## Authentication and authorization

- Inbound requests require `Authorization: Bearer <JWT>`.
- JWT validation uses ASP.NET Core JWT bearer authentication.
- Authorization is enforced per route (command vs query scopes), with defense-in-depth even when the edge gateway validates tokens.

Tenant context is derived from verified identity context:

- `tenant_id` comes from validated JWT claims (BFF/SOAP facade calls) or from the worker’s service identity token claims.
- `tenant_id` is never accepted from request bodies or caller-provided headers as the source of truth.

## Tenant database routing

- The service is database-per-tenant. Each request resolves `tenant_id` and uses it to obtain a tenant-scoped database connection string/credentials (Secrets Manager + KMS).
- Persistence is implemented with EF Core + Npgsql (read/write) against the schema owned by `fxfer-db-schema`.
- The service does not run schema migrations on startup; schema rollout is handled by the `fxfer-db-schema` deployable.

## Observability (logs, metrics, traces)

- OpenTelemetry SDK is configured for traces/metrics/logs.
- W3C Trace Context is extracted/propagated (`traceparent`, `tracestate`, `baggage`).
- Correlation ID propagation uses `X-Correlation-ID`:
  - if missing on inbound requests, generate a UUID v4 and return it in the response header.
  - include `tenant_id`, `correlation_id`, and `trace_id` attributes on all spans/logs.

## Resilience and timeouts

- Outbound HTTP dependencies (Tenant Registry, Event Adapter, Secrets Manager wrappers where applicable) use bounded retries with jitter and circuit breaking.
- Command endpoints must be idempotent where callers may retry; idempotency keys are accepted via header when required by the BFF/worker.

## Event publishing

- Domain events are published using an outbox pattern to avoid emitting events for writes that are not committed.
- Events use the platform canonical envelope (tenant-scoped) and are published via the Event Adapter to the Event Backbone.
- Event payloads must not include secrets; configuration payloads carry only ids/handles for credentials.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (OpenAPI 3.1 as mandatory contract format; contract-first posture)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Variation 1: Contract-First (Design-First) - **Recommended for Gainwell**` (author OpenAPI before implementation; use it for governance and generation)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Component 3: JWT Validation Middleware (API Services)` (ASP.NET Core JWT validation middleware)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Best Practice 2: Validate All JWT Claims` (issuer/audience/signature/expiry validation)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (tenant isolation via database-per-tenant routing)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel SDK + OTLP export + W3C context propagation)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (span/log attribute naming conventions; custom tenant attribute prefix)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (generate/extract correlation id at boundaries)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` standard header)

