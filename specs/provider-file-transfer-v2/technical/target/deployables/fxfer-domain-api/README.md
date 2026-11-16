---
type: Deployable Overview
title: 'Deployable: fxfer-domain-api'
description: 'Overview of the fxfer-domain-api deployable: where it lives, its stack and the flows it serves.'
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
  body_sha256: bd517fbf11a0941fac7b98a267c52de3c3ead867552458cc9d7f546205a3f7f8
---

# Deployable: `fxfer-domain-api`

`fxfer-domain-api` is the authoritative FXFER Domain API service for configuration and tracking. It enforces FXFER domain validation and persists authoritative state to the per-tenant Aurora PostgreSQL database.

- Path in the new repository: `services/fxfer-domain-api`
- Kind: `dotnet-service`
- Stack: .NET 10 (ASP.NET Core; AOT preferred)
- Build: `dotnet build services/fxfer-domain-api`

## Dependencies

- Depends on: `fxfer-db-schema`.
- Depended on by: `fxfer-bff`, `fxfer-transfer-worker`, `fxfer-soap-facade`, `fxfer-projection-worker`.

## Components

| Component | Folder | What it does |
| --- | --- | --- |
| `backend-bas-file-transfer-api` | `src/Api` | API host composition and API surface wiring for FXFER domain command/query endpoints (legacy WCF surfaces map here). |
| `backend-configure-source-locations-and-destinations-api` | `src/Api` | Configuration-focused API doors for FTP server profiles, source locations, inbound targets, and integration file definitions. |
| `backend-business-aggregates` | `src/Domain` | Domain aggregates/entities representing FXFER configuration and tracking concepts. |
| `backend-business-aggregates-2` | `src/Domain` | Additional domain aggregates/entities for FXFER. |
| `backend-business-profiles` | `src/Domain/Profiles` | Domain-level profiles/mappings (legacy mapping utilities) used by command/query handlers. |
| `backend-business-command-handlers` | `src/Application/Commands` | Command handlers for authoritative writes (create/update/invalidate) for configuration aggregates. |
| `backend-business-command-handlers-2` | `src/Application/Commands` | Additional command handlers (legacy implementation parity). |
| `backend-data-access-query-handlers` | `src/Application/Queries` | Query executors for tracking and configuration read/search operations. |
| `backend-data-access-query-handlers-2` | `src/Application/Queries` | Additional query executors (legacy implementation parity). |
| `data-data-access-entities` | `src/Infrastructure/DataAccess` | EF Core entities / data-access models for command-side persistence. |
| `data-data-access-entities-2` | `src/Infrastructure/DataAccess` | Additional entities and mappings. |
| `data-data-access-entities-3` | `src/Infrastructure/DataAccess` | Additional entities and mappings. |
| `shared-common-common-constants` | `src/Domain/Constants` | Shared constant values used by domain logic and validations. |
| `shared-common-constants-validation` | `src/Domain/Validation` | Shared validation constants/patterns used at command boundaries. |
| `shared-contracts-commands` | `src/Contracts/Commands` | Command DTOs for configuration write operations. |
| `shared-contracts-queries` | `src/Contracts/Queries` | Query DTOs for read/search operations. |
| `shared-queries-parameters` | `src/Contracts/Queries` | Query parameter DTOs for filtering/paging. |
| `shared-queries-parameters-2` | `src/Contracts/Queries` | Additional query parameter / helper contracts. |
| `shared-contracts-domain` | `src/Contracts/Domain` | Domain read models returned by query endpoints (configuration + tracking). |
| `shared-contracts-domain-2` | `src/Contracts/Domain` | Additional domain read models. |
| `shared-contracts-events` | `src/Contracts/Events` | Event payload contracts for FXFER domain events. |
| `shared-contracts-exceptions` | `src/Contracts/Exceptions` | Shared exception contracts and validation error vocabulary. |
| `shared-exceptions-file-transfer-exception-constants` | `src/Contracts/Exceptions` | Exception constant definitions used by validation and error mapping. |
| `shared-contracts-utilities` | `src/Contracts/Utilities` | Shared utility contracts used across commands/queries (e.g., error utilities). |
| `shared-file-transfer-svc-data-access` | `src/Infrastructure/DataAccess/Constants` | Data-access constants for FXFER persistence and queries. |
| `shared-file-transfer-svc-data-access-2` | `src/Infrastructure/DataAccess/Constants` | Additional data-access constants. |

## Enterprise-Architecture standards applied

- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant isolation; encryption; auditability expectations for tenant-scoped state)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (database-per-tenant routing driven by verified tenant context)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Variation 1: Centralized JWT with API Gateway Validation` (edge validation posture; service still validates claims for defense in depth)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Component 3: JWT Validation Middleware (API Services)` (JWT validation middleware in ASP.NET Core services)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Best Practice 2: Validate All JWT Claims` (issuer/audience/signature/expiry validation; reject invalid tokens)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel SDK/OTLP export; W3C trace context propagation)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (OTel semantic conventions; tenant tagging via custom attributes)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (correlation id generation/extraction at entry points)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` header and UUID v4 format)

