---
type: Target Architecture
title: 'Target Architecture: provider-file-transfer-v2 (MMS-CMS-FXFER)'
description: This module modernizes MMS-CMS-FXFER as the bounded context defined in inputs/target-state.md.
tags:
- architecture
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T14:31:31Z
sources:
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture
- id: components
  resource: /technical/components.yaml
  title: The components it places
cef:
  pack: provider-file-transfer-v2
  item: architecture
  generation:
    status: done
    attempts: 2
  deployables: 7
  body_sha256: d70027447fa59e6235f16df7c26f25f03a72463c1d8e57aa59477cb8a9335e4c
---

# Target Architecture: provider-file-transfer-v2 (MMS-CMS-FXFER)

This module modernizes MMS-CMS-FXFER as the bounded context defined in `inputs/target-state.md`.

## Deployables

| Deployable | Kind | Path | Role |
| --- | --- | --- | --- |
| `fxfer-db-schema` | `schema` | `schema/fxfer-db` | Per-tenant FXFER database schema migrations. |
| `fxfer-domain-api` | `dotnet-service` | `services/fxfer-domain-api` | Authoritative command/query APIs for FXFER configuration and tracking. |
| `fxfer-projection-worker` | `projector` | `workers/fxfer-projection-worker` | Builds/updates read models from FXFER domain events. |
| `fxfer-transfer-worker` | `worker` | `workers/fxfer-transfer-worker` | Scheduled execution of protocol I/O and recording outcomes via domain APIs. |
| `fxfer-soap-facade` | `dotnet-service` | `services/fxfer-soap-facade` | SOAP compatibility facade for legacy consumers (no new SOAP functionality). |
| `fxfer-bff` | `bff` | `services/fxfer-bff` | Browser-facing API facade for the FXFER micro-frontend. |
| `fxfer-mfe` | `mfe` | `apps/fxfer-mfe` | Operator UI for FXFER configuration and tracking. |

## Interactions

- `fxfer-mfe` calls `fxfer-bff` over HTTPS (via API Gateway).
- `fxfer-bff` calls `fxfer-domain-api` only (no direct database access).
- `fxfer-transfer-worker` performs external protocol I/O and calls `fxfer-domain-api` to record authoritative outcomes.
- `fxfer-domain-api` persists authoritative state in per-tenant Aurora PostgreSQL and publishes tenant-scoped domain events (via Event Adapter) to the Event Backbone.
- `fxfer-projection-worker` consumes FXFER domain events to update projections/read models.
- `fxfer-soap-facade` translates legacy SOAP calls to the target command/query APIs.

## Diagram (conceptual)

```mermaid
flowchart LR
  Browser[Operator Browser] -->|HTTPS (OIDC JWT via edge)| GW[API Gateway]
  GW --> BFF[fxfer-bff]
  BFF -->|HTTPS| API[fxfer-domain-api]

  API -->|SQL (tenant-routed)| DB[(Aurora PostgreSQL per tenant)]

  API -->|Outbox/Event Adapter| EB[(Event Backbone)]
  EB --> PRJ[fxfer-projection-worker]
  PRJ -->|SQL (projections)| DB

  TW[fxfer-transfer-worker] -->|protocol I/O| EXT[External partner endpoints]
  TW -->|HTTPS record outcomes| API

  SOAP[fxfer-soap-facade] -->|translate to command/query| API
```

## Uses (shared services / external systems)

The following are used by this module and are not generated here (see `technical/target/architecture.yaml` under `uses:`):

- Platform identity broker (OIDC)
- API Gateway (edge)
- Event Backbone (SNS/SQS pattern)
- Event Adapter
- Tenant Registry + feature flags
- Secrets Manager + KMS
- Aurora PostgreSQL (per-tenant)
- External partner endpoints
- Shared shell (micro-frontend host)

## Component placement

| Component | Deployable | Folder |
| --- | --- | --- |
| `backend-bas-file-transfer-api` | `fxfer-domain-api` | `src/Api` |
| `backend-bas-file-transfer-svc` | `fxfer-soap-facade` | `src/LegacyWcf` |
| `backend-bas-file-transfer-svc-2` | `fxfer-soap-facade` | `src/CoreWcf` |
| `backend-batch-business` | `fxfer-transfer-worker` | `src/Application` |
| `backend-business-aggregates` | `fxfer-domain-api` | `src/Domain` |
| `backend-business-aggregates-2` | `fxfer-domain-api` | `src/Domain` |
| `backend-business-command-handlers` | `fxfer-domain-api` | `src/Application/Commands` |
| `backend-business-command-handlers-2` | `fxfer-domain-api` | `src/Application/Commands` |
| `backend-business-helpers` | `fxfer-transfer-worker` | `src/Application/Helpers` |
| `backend-business-helpers-2` | `fxfer-transfer-worker` | `src/Application/Helpers` |
| `backend-business-profiles` | `fxfer-domain-api` | `src/Domain/Profiles` |
| `backend-configure-source-locations-and-destinations-api` | `fxfer-domain-api` | `src/Api` |
| `backend-data-access-batch-data-access` | `fxfer-transfer-worker` | `src/Infrastructure/DataAccess` |
| `backend-data-access-batch-data-access-2` | `fxfer-transfer-worker` | `src/Infrastructure/DataAccess` |
| `backend-data-access-event-handlers` | `fxfer-projection-worker` | `src/Infrastructure/Projections` |
| `backend-data-access-event-handlers-2` | `fxfer-projection-worker` | `src/Infrastructure/Projections` |
| `backend-data-access-query-handlers` | `fxfer-domain-api` | `src/Application/Queries` |
| `backend-data-access-query-handlers-2` | `fxfer-domain-api` | `src/Application/Queries` |
| `backend-data-file-transfer` | `fxfer-bff` | `src/Contracts` |
| `backend-data-file-transfer-constants` | `fxfer-mfe` | `src/legacy-constants` |
| `backend-data-file-transfer-tracking` | `fxfer-bff` | `src/Contracts/Tracking` |
| `backend-file-transfer-constants-localization` | `fxfer-mfe` | `src/legacy-constants/localization` |
| `backend-file-transfer-mgmt-business` | `fxfer-transfer-worker` | `src/Application` |
| `backend-file-transfer-mgmt-ctrl` | `fxfer-transfer-worker` | `src/Infrastructure/Adapters` |
| `backend-file-transfer-mgmt-data-access` | `fxfer-transfer-worker` | `src/Infrastructure/DataAccess` |
| `backend-file-transfer-svc-main` | `fxfer-soap-facade` | `src/Host` |
| `backend-localization-labels` | `fxfer-mfe` | `src/legacy-constants/localization` |
| `backend-providers-file-transfer` | `fxfer-bff` | `src/Application/Providers` |
| `backend-run-file-transfer-batch-api` | `fxfer-transfer-worker` | `src/Worker` |
| `data-data-access-entities` | `fxfer-domain-api` | `src/Infrastructure/DataAccess` |
| `data-data-access-entities-2` | `fxfer-domain-api` | `src/Infrastructure/DataAccess` |
| `data-data-access-entities-3` | `fxfer-domain-api` | `src/Infrastructure/DataAccess` |
| `shared-common-common-constants` | `fxfer-domain-api` | `src/Domain/Constants` |
| `shared-common-constants-validation` | `fxfer-domain-api` | `src/Domain/Validation` |
| `shared-common-file-transfer-management-constants` | `fxfer-transfer-worker` | `src/Domain/Constants` |
| `shared-contracts-batch` | `fxfer-transfer-worker` | `src/Contracts` |
| `shared-contracts-commands` | `fxfer-domain-api` | `src/Contracts/Commands` |
| `shared-contracts-domain` | `fxfer-domain-api` | `src/Contracts/Domain` |
| `shared-contracts-domain-2` | `fxfer-domain-api` | `src/Contracts/Domain` |
| `shared-contracts-events` | `fxfer-domain-api` | `src/Contracts/Events` |
| `shared-contracts-exceptions` | `fxfer-domain-api` | `src/Contracts/Exceptions` |
| `shared-contracts-queries` | `fxfer-domain-api` | `src/Contracts/Queries` |
| `shared-contracts-utilities` | `fxfer-domain-api` | `src/Contracts/Utilities` |
| `shared-data-file-transfer-constants` | `fxfer-mfe` | `src/legacy-constants` |
| `shared-exceptions-file-transfer-exception-constants` | `fxfer-domain-api` | `src/Contracts/Exceptions` |
| `shared-file-transfer-svc-data-access` | `fxfer-domain-api` | `src/Infrastructure/DataAccess/Constants` |
| `shared-file-transfer-svc-data-access-2` | `fxfer-domain-api` | `src/Infrastructure/DataAccess/Constants` |
| `shared-queries-parameters` | `fxfer-domain-api` | `src/Contracts/Queries` |
| `shared-queries-parameters-2` | `fxfer-domain-api` | `src/Contracts/Queries` |
| `ui-configure-source-locations-and-destinations-screens` | `fxfer-mfe` | `src/screens/configure-source-locations-and-destinations` |
| `ui-file-transfer-api-controllers` | `fxfer-bff` | `src/Api` |
| `ui-manage-ftp-servers-and-credentials-screens` | `fxfer-mfe` | `src/screens/manage-ftp-servers-and-credentials` |

## Build order

Deployables are ordered so dependencies appear first (see `technical/target/architecture.yaml`).

1. `fxfer-db-schema` (schema must exist before services can persist)
2. `fxfer-domain-api` (authoritative write model + event publishing)
3. `fxfer-projection-worker` (depends on domain events and schema)
4. `fxfer-transfer-worker` (records outcomes through domain APIs)
5. `fxfer-soap-facade` (translates legacy consumers into domain APIs)
6. `fxfer-bff` (UI facade depends on domain APIs)
7. `fxfer-mfe` (UI depends on the BFF contract)

Flow build order (from `technical/target/architecture.yaml` `flow_order:`):

1. `manage-ftp-servers-and-credentials` (establish endpoint credentials)
2. `configure-source-locations-and-destinations` (create/update transfer definitions using configured endpoints)
3. `manage-integration-files` (define/manage integration files for configured transfers)
4. `run-file-transfer-batch` (execute transfers; produce outcomes)
5. `search-and-track-file-transfers` (operator tracking/read views depend on run/outcome history)

## Standards applied

The following enterprise-architecture standards are applied and referenced here per requirement:

- `decisions/technology/ADR-TEC-015-source-control-repository-management.md` sections `Decision Summary`, `Technical Context` (bounded-context scoped repo containing multiple deployables)
- `patterns/application/api-microservices/PTN-APP-API-001-backend-for-frontend-pattern.md` section `Solution Overview` (BFF as UI-optimized facade)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` sections `Variation 1: Centralized JWT with API Gateway Validation`, `Component 3: JWT Validation Middleware (API Services)`, `Best Practice 2: Validate All JWT Claims` (stateless JWT posture + claim validation at boundaries)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` sections `2.1 Data Protection Requirements`, `3.1 Tenant Isolation Strategies` (tenant isolation expectations)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` sections `2.1 Mandatory Requirements`, `3.6 Semantic Conventions` (OTel signals and semantic conventions; tenant tagging)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` sections `Solution Overview`, `Correlation ID Format Standards` (correlation_id propagation)
- `standards/technology/nextjs/STD-TEC-NXT-001-coding.md` sections `4.1 App Router vs Pages Router`, `10.0 Deployment and Build Process` (Next.js Pages Router posture and build expectations)
