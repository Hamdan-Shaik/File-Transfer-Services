---
type: Deployable Structure
title: technical/target/deployables/fxfer-bff/STRUCTURE
description: 'Skeleton of the fxfer-bff deployable: projects, folders and cross-cutting setup.'
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-bff
tags:
- bff
- deployable
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:45:39Z
sources:
- id: component-backend-data-file-transfer
  resource: /technical/components/backend-data-file-transfer/SPEC.md
  title: Component placed in it
- id: component-backend-data-file-transfer-tracking
  resource: /technical/components/backend-data-file-transfer-tracking/SPEC.md
  title: Component placed in it
- id: component-backend-providers-file-transfer
  resource: /technical/components/backend-providers-file-transfer/SPEC.md
  title: Component placed in it
- id: component-ui-file-transfer-api-controllers
  resource: /technical/components/ui-file-transfer-api-controllers/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (4 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-bff
  generation:
    status: done
    attempts: 1
  path: services/fxfer-bff
  kind: bff
  stack: .NET 10 (Native AOT)
  components: 4
  depends_on:
  - fxfer-domain-api
  body_sha256: 4b078fb3944ff377147d5721c36ff381db420f8095a465232944a0a8f5a3973e
---

﻿# Structure: `fxfer-bff`

Deployable path: `services/fxfer-bff`

This deployable is the FXFER Backend-for-Frontend (BFF): an ASP.NET Core HTTP API (Native AOT posture) that serves the FXFER micro-frontend. It is a contract boundary for browser traffic and calls `fxfer-domain-api` for authoritative domain operations (no direct database access).

## Folder skeleton (new repository)

Create the following skeleton at `services/fxfer-bff`:

```text
services/fxfer-bff/
  Fxfer.Bff.sln
  README.md
  openapi/
    fxfer-bff.v1.yaml
  src/
    Api/
      Fxfer.Bff.Api.csproj
      Program.cs
      appsettings.json
      appsettings.Development.json
      Configuration/
        AuthenticationOptions.cs
        TenantResolutionOptions.cs
        CorsOptions.cs
        DownstreamOptions.cs
      Middleware/
        CorrelationIdMiddleware.cs
        TenantContextMiddleware.cs
        OriginValidationMiddleware.cs
        ProblemDetailsMiddleware.cs
      Endpoints/
        V1/
          TransferConfigurationsEndpoints.cs
          FtpServersEndpoints.cs
          FileTrackingEndpoints.cs
          DataListsEndpoints.cs
          InboundTargetMetadataEndpoints.cs
          SourceLocationsEndpoints.cs
      Health/
        HealthChecks.cs
    Application/
      Providers/
        FxferDomainApiClient.cs
        DataListsClient.cs
    Contracts/
      TransferConfigurations/
      FtpServers/
      Tracking/
      Common/
  test/
    Fxfer.Bff.Tests/
      Fxfer.Bff.Tests.csproj
  Dockerfile
  helm/
    fxfer-bff/
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
- `openapi/fxfer-bff.v1.yaml` is the contract-first OpenAPI 3.1 source of truth for `/api/file-transfer/v1`.

## Runtime entry point

- `src/Api/Program.cs` hosts the ASP.NET Core pipeline and maps BFF endpoints under `/api/file-transfer/v1`.
- The service exposes platform-standard health endpoints:
  - `GET /api/health/live`
  - `GET /api/health/ready`

## Authentication, tenancy and browser security

- Inbound requests require `Authorization: Bearer <JWT>`.
- Tenant context is derived from the validated JWT `tenant_id` claim and is never accepted from request bodies or caller-provided tenant headers.
- CORS is allow-listed to approved shell/micro-frontend origins.
- For state-changing requests (POST/PUT/PATCH), validate `Origin`/`Referer` and reject mismatches.

## Downstream calls (to `fxfer-domain-api`)

- All authoritative reads/writes are performed by `fxfer-domain-api`; the BFF does not access databases.
- Outbound HTTP uses typed `HttpClient` with explicit timeouts and bounded retries.
- BFF request context is propagated downstream:
  - `Authorization` (token passthrough or on-behalf-of, per platform identity)
  - `X-Correlation-ID`
  - W3C Trace Context (`traceparent`, `tracestate`, `baggage`)
  - `Idempotency-Key` for write operations where operator retries are expected

## Error normalization

- The BFF normalizes downstream failures into an RFC 7807-compatible Problem Details response (`application/problem+json`).
- Problem Details never includes stack traces, secrets, or PII.

## Observability (logs, metrics, traces)

- OpenTelemetry SDK is configured for traces/metrics/logs.
- W3C Trace Context is extracted/propagated on all inbound/outbound HTTP calls.
- Correlation ID propagation uses `X-Correlation-ID` (UUID v4):
  - if missing on inbound requests, generate one and return it.
  - include `gainwell.tenant.id`, `correlation_id`, and `trace_id` attributes on spans/logs.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-001-backend-for-frontend-pattern.md` section `Solution Overview` (BFF patterns: aggregation/shaping without duplicating domain logic)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (OpenAPI 3.1 contract stored with code)
- `patterns/application/api-microservices/PTN-APP-API-016-pagination-pattern.md` section `Solution Overview` (standard `page`/`pageSize` pagination envelopes for collection endpoints)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Component 3: JWT Validation Middleware (API Services)` (JWT bearer validation middleware)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (OTel semantic conventions + `gainwell.*` namespaced custom attributes)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (generate/extract correlation id at boundaries and propagate)
