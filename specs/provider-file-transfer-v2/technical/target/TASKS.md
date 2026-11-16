---
type: Task Plan
title: 'Code-generation plan: provider-file-transfer-v2'
description: 'The code-generation tasks wave by wave: skeletons, component parts ported from the legacy code, flow wiring.'
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
  body_sha256: b9aae0fc5bea5f4fed158f6943a7d388d5504402ef7db6363e01ee1d129e7dcb
---

# Code-generation plan: provider-file-transfer-v2

105 tasks in 14 waves: 7 skeletons, 84 port tasks for 52 components (1105 legacy methods or classes, 26143 legacy lines), and 5 flow wirings. Everything in a wave depends only on earlier waves; up to 21 tasks can run side by side.

A port task carries over at most 30 methods or 600 legacy lines, from the legacy code itself. The data is in `PLAN.yaml`.

## What a task must show

- **scaffold**: The deployable's skeleton exists at its path in the target-state structure. Its build command succeeds.
- **port**: Every legacy method listed has a ported target method with a real body (no stub, no TODO). Each body keeps the legacy method's branches, calls, messages, constants and data access; only what the target technology forces changes. Every rule listed is cited by id where it is implemented and in a test name. The deployable builds and its tests pass.
- **build**: What the target-state requirement asks for is built where the architecture places it. The requirement id is cited where it is implemented and in a test name. The deployable builds and its tests pass.
- **wire**: The flow runs end to end through its components' contracts. Its business scenarios (business/<flow>.feature) run as end-to-end tests and pass. Every deployable it touches builds.

## Wave 1 (1 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `scaffold:fxfer-db-schema` | skeleton at `schema/fxfer-db` | fxfer-db-schema | - | - | - |

## Wave 2 (2 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `scaffold:fxfer-domain-api` | skeleton at `services/fxfer-domain-api` | fxfer-domain-api | - | - | `scaffold:fxfer-db-schema` |
| `build:T-009` | target-state requirement T-009: Apply an expand/contract discipline for schema migrations; each migration must be repeatable and idempotent per tenant and observable per-tenant. | fxfer-db-schema | - | - | `scaffold:fxfer-db-schema` |

## Wave 3 (21 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `scaffold:fxfer-projection-worker` | skeleton at `workers/fxfer-projection-worker` | fxfer-projection-worker | - | - | `scaffold:fxfer-domain-api`, `scaffold:fxfer-db-schema` |
| `port:backend-bas-file-transfer-api/1` | 12 of backend-bas-file-transfer-api (part 1/1) | fxfer-domain-api | 269 | 0 | `scaffold:fxfer-domain-api` |
| `port:backend-business-aggregates/1` | 11 of backend-business-aggregates (part 1/1) | fxfer-domain-api | 343 | 0 | `scaffold:fxfer-domain-api` |
| `port:backend-business-aggregates-2/1` | 3 of backend-business-aggregates-2 (part 1/1) | fxfer-domain-api | 263 | 0 | `scaffold:fxfer-domain-api` |
| `port:backend-business-profiles/1` | 10 of backend-business-profiles (part 1/1) | fxfer-domain-api | 106 | 0 | `scaffold:fxfer-domain-api` |
| `port:data-data-access-entities/1` | 4 of data-data-access-entities (part 1/4) | fxfer-domain-api | 590 | 0 | `scaffold:fxfer-domain-api` |
| `port:data-data-access-entities-2/1` | 4 of data-data-access-entities-2 (part 1/3) | fxfer-domain-api | 539 | 0 | `scaffold:fxfer-domain-api` |
| `port:data-data-access-entities-3/1` | 4 of data-data-access-entities-3 (part 1/4) | fxfer-domain-api | 580 | 0 | `scaffold:fxfer-domain-api` |
| `port:shared-common-common-constants/1` | 5 of shared-common-common-constants (part 1/1) | fxfer-domain-api | 128 | 13 | `scaffold:fxfer-domain-api` |
| `port:shared-common-constants-validation/1` | 3 of shared-common-constants-validation (part 1/1) | fxfer-domain-api | 30 | 11 | `scaffold:fxfer-domain-api` |
| `port:shared-contracts-domain/1` | 7 of shared-contracts-domain (part 1/2) | fxfer-domain-api | 567 | 1 | `scaffold:fxfer-domain-api` |
| `port:shared-contracts-domain-2/1` | 15 of shared-contracts-domain-2 (part 1/1) | fxfer-domain-api | 411 | 0 | `scaffold:fxfer-domain-api` |
| `port:shared-contracts-events/1` | 9 of shared-contracts-events (part 1/2) | fxfer-domain-api | 502 | 0 | `scaffold:fxfer-domain-api` |
| `port:shared-contracts-exceptions/1` | 4 of shared-contracts-exceptions (part 1/1) | fxfer-domain-api | 116 | 11 | `scaffold:fxfer-domain-api` |
| `port:shared-contracts-queries/1` | 10 of shared-contracts-queries (part 1/1) | fxfer-domain-api | 98 | 1 | `scaffold:fxfer-domain-api` |
| `port:shared-contracts-utilities/1` | 8 of shared-contracts-utilities (part 1/1) | fxfer-domain-api | 89 | 0 | `scaffold:fxfer-domain-api` |
| `port:shared-exceptions-file-transfer-exception-constants/1` | 3 of shared-exceptions-file-transfer-exception-constants (part 1/1) | fxfer-domain-api | 54 | 9 | `scaffold:fxfer-domain-api` |
| `port:shared-file-transfer-svc-data-access/1` | 3 of shared-file-transfer-svc-data-access (part 1/1) | fxfer-domain-api | 27 | 1 | `scaffold:fxfer-domain-api` |
| `port:shared-file-transfer-svc-data-access-2/1` | 3 of shared-file-transfer-svc-data-access-2 (part 1/1) | fxfer-domain-api | 27 | 0 | `scaffold:fxfer-domain-api` |
| `port:shared-queries-parameters/1` | 8 of shared-queries-parameters (part 1/1) | fxfer-domain-api | 298 | 5 | `scaffold:fxfer-domain-api` |
| `port:shared-queries-parameters-2/1` | 4 of shared-queries-parameters-2 (part 1/1) | fxfer-domain-api | 77 | 0 | `scaffold:fxfer-domain-api` |

## Wave 4 (11 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `scaffold:fxfer-transfer-worker` | skeleton at `workers/fxfer-transfer-worker` | fxfer-transfer-worker | - | - | `scaffold:fxfer-domain-api`, `scaffold:fxfer-projection-worker` |
| `port:backend-data-access-event-handlers-2/1` | 18 of backend-data-access-event-handlers-2 (part 1/1) | fxfer-projection-worker | 317 | 8 | `scaffold:fxfer-projection-worker` |
| `port:data-data-access-entities/2` | 4 of data-data-access-entities (part 2/4) | fxfer-domain-api | 516 | 0 | `scaffold:fxfer-domain-api`, `port:data-data-access-entities/1` |
| `port:data-data-access-entities-2/2` | 5 of data-data-access-entities-2 (part 2/3) | fxfer-domain-api | 570 | 0 | `scaffold:fxfer-domain-api`, `port:data-data-access-entities-2/1` |
| `port:data-data-access-entities-3/2` | 4 of data-data-access-entities-3 (part 2/4) | fxfer-domain-api | 516 | 0 | `scaffold:fxfer-domain-api`, `port:data-data-access-entities-3/1` |
| `port:shared-contracts-domain/2` | 1 of shared-contracts-domain (part 2/2) | fxfer-domain-api | 241 | 0 | `scaffold:fxfer-domain-api`, `port:shared-contracts-domain/1` |
| `port:shared-contracts-events/2` | 6 of shared-contracts-events (part 2/2) | fxfer-domain-api | 518 | 0 | `scaffold:fxfer-domain-api`, `port:shared-contracts-events/1` |
| `port:backend-data-access-query-handlers/1` | 25 of backend-data-access-query-handlers (part 1/2) | fxfer-domain-api | 371 | 14 | `scaffold:fxfer-domain-api`, `port:shared-contracts-queries/1` |
| `port:backend-data-access-query-handlers-2/1` | 28 of backend-data-access-query-handlers-2 (part 1/2) | fxfer-domain-api | 383 | 4 | `scaffold:fxfer-domain-api`, `port:shared-contracts-queries/1`, `port:shared-queries-parameters-2/1` |
| `port:shared-contracts-commands/1` | 27 of shared-contracts-commands (part 1/2) | fxfer-domain-api | 299 | 18 | `scaffold:fxfer-domain-api`, `port:shared-contracts-utilities/1` |
| `build:T-005` | target-state requirement T-005: Emit OpenTelemetry traces/metrics/logs from services and workers tagged with tenant_id, correlation_id, and trace_id, and propagate these values end-to-end. | fxfer-domain-api | - | - | `scaffold:fxfer-domain-api`, `port:backend-bas-file-transfer-api/1` |

## Wave 5 (17 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `scaffold:fxfer-soap-facade` | skeleton at `services/fxfer-soap-facade` | fxfer-soap-facade | - | - | `scaffold:fxfer-domain-api`, `scaffold:fxfer-transfer-worker` |
| `port:backend-business-helpers/1` | 10 of backend-business-helpers (part 1/1) | fxfer-transfer-worker | 270 | 0 | `scaffold:fxfer-transfer-worker` |
| `port:backend-business-helpers-2/1` | 29 of backend-business-helpers-2 (part 1/1) | fxfer-transfer-worker | 465 | 0 | `scaffold:fxfer-transfer-worker` |
| `port:backend-data-access-batch-data-access/1` | 29 of backend-data-access-batch-data-access (part 1/2) | fxfer-transfer-worker | 253 | 0 | `scaffold:fxfer-transfer-worker` |
| `port:backend-data-access-batch-data-access-2/1` | 29 of backend-data-access-batch-data-access-2 (part 1/2) | fxfer-transfer-worker | 258 | 0 | `scaffold:fxfer-transfer-worker` |
| `port:backend-file-transfer-mgmt-data-access/1` | 20 of backend-file-transfer-mgmt-data-access (part 1/3) | fxfer-transfer-worker | 260 | 0 | `scaffold:fxfer-transfer-worker` |
| `port:backend-run-file-transfer-batch-api/1` | 7 of backend-run-file-transfer-batch-api (part 1/1) | fxfer-transfer-worker | 150 | 14 | `scaffold:fxfer-transfer-worker` |
| `port:data-data-access-entities/3` | 3 of data-data-access-entities (part 3/4) | fxfer-domain-api | 274 | 0 | `scaffold:fxfer-domain-api`, `port:data-data-access-entities/2` |
| `port:data-data-access-entities-2/3` | 2 of data-data-access-entities-2 (part 3/3) | fxfer-domain-api | 493 | 0 | `scaffold:fxfer-domain-api`, `port:data-data-access-entities-2/2` |
| `port:data-data-access-entities-3/3` | 3 of data-data-access-entities-3 (part 3/4) | fxfer-domain-api | 274 | 0 | `scaffold:fxfer-domain-api`, `port:data-data-access-entities-3/2` |
| `port:shared-common-file-transfer-management-constants/1` | 8 of shared-common-file-transfer-management-constants (part 1/1) | fxfer-transfer-worker | 96 | 0 | `scaffold:fxfer-transfer-worker` |
| `port:shared-contracts-batch/1` | 15 of shared-contracts-batch (part 1/1) | fxfer-transfer-worker | 288 | 0 | `scaffold:fxfer-transfer-worker` |
| `port:backend-data-access-event-handlers/1` | 18 of backend-data-access-event-handlers (part 1/1) | fxfer-projection-worker | 317 | 18 | `scaffold:fxfer-projection-worker`, `port:backend-data-access-event-handlers-2/1` |
| `port:backend-data-access-query-handlers/2` | 18 of backend-data-access-query-handlers (part 2/2) | fxfer-domain-api | 229 | 27 | `scaffold:fxfer-domain-api`, `port:shared-contracts-queries/1`, `port:backend-data-access-query-handlers/1` |
| `port:backend-data-access-query-handlers-2/2` | 10 of backend-data-access-query-handlers-2 (part 2/2) | fxfer-domain-api | 110 | 0 | `scaffold:fxfer-domain-api`, `port:shared-contracts-queries/1`, `port:shared-queries-parameters-2/1`, `port:backend-data-access-query-handlers-2/1` |
| `port:shared-contracts-commands/2` | 14 of shared-contracts-commands (part 2/2) | fxfer-domain-api | 138 | 10 | `scaffold:fxfer-domain-api`, `port:shared-contracts-utilities/1`, `port:shared-contracts-commands/1` |
| `build:T-006` | target-state requirement T-006: Publish tenant-aware, schema-versioned domain events using the canonical event envelope (tenant_id, correlation_id, trace_id, producer, etc.). | fxfer-domain-api | - | - | `scaffold:fxfer-domain-api`, `port:shared-contracts-events/2` |

## Wave 6 (14 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `scaffold:fxfer-bff` | skeleton at `services/fxfer-bff` | fxfer-bff | - | - | `scaffold:fxfer-domain-api`, `scaffold:fxfer-soap-facade` |
| `port:backend-bas-file-transfer-svc/1` | 12 of backend-bas-file-transfer-svc (part 1/1) | fxfer-soap-facade | 302 | 0 | `scaffold:fxfer-soap-facade` |
| `port:backend-bas-file-transfer-svc-2/1` | 12 of backend-bas-file-transfer-svc-2 (part 1/1) | fxfer-soap-facade | 302 | 0 | `scaffold:fxfer-soap-facade` |
| `port:backend-data-access-batch-data-access/2` | 22 of backend-data-access-batch-data-access (part 2/2) | fxfer-transfer-worker | 387 | 1 | `scaffold:fxfer-transfer-worker`, `port:backend-data-access-batch-data-access/1` |
| `port:backend-data-access-batch-data-access-2/2` | 22 of backend-data-access-batch-data-access-2 (part 2/2) | fxfer-transfer-worker | 387 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-data-access-batch-data-access-2/1` |
| `port:backend-file-transfer-mgmt-data-access/2` | 30 of backend-file-transfer-mgmt-data-access (part 2/3) | fxfer-transfer-worker | 383 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-file-transfer-mgmt-data-access/1` |
| `port:backend-file-transfer-svc-main/1` | 15 of backend-file-transfer-svc-main (part 1/1) | fxfer-soap-facade | 171 | 0 | `scaffold:fxfer-soap-facade` |
| `port:data-data-access-entities/4` | 9 of data-data-access-entities (part 4/4) | fxfer-domain-api | 554 | 0 | `scaffold:fxfer-domain-api`, `port:data-data-access-entities/3` |
| `port:data-data-access-entities-3/4` | 9 of data-data-access-entities-3 (part 4/4) | fxfer-domain-api | 554 | 0 | `scaffold:fxfer-domain-api`, `port:data-data-access-entities-3/3` |
| `port:backend-batch-business/1` | 17 of backend-batch-business (part 1/5) | fxfer-transfer-worker | 388 | 1 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers/1` |
| `port:backend-file-transfer-mgmt-business/1` | 7 of backend-file-transfer-mgmt-business (part 1/7) | fxfer-transfer-worker | 78 | 2 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers-2/1` |
| `port:backend-business-command-handlers/1` | 26 of backend-business-command-handlers (part 1/1) | fxfer-domain-api | 290 | 14 | `scaffold:fxfer-domain-api`, `port:backend-data-access-event-handlers/1`, `port:backend-data-access-event-handlers-2/1`, `port:shared-contracts-commands/2` |
| `build:T-004` | target-state requirement T-004: Support idempotent command submission for operator retries (for example via an Idempotency-Key) so resubmission does not create duplicates. | fxfer-domain-api | - | - | `scaffold:fxfer-domain-api`, `port:shared-contracts-commands/2` |
| `build:T-007` | target-state requirement T-007: Provide privileged rerun execution via a CLI mode targeting a specific tenant and tracked file identifier (without introducing a required UI rerun button). | fxfer-transfer-worker | - | - | `scaffold:fxfer-transfer-worker`, `port:backend-run-file-transfer-batch-api/1` |

## Wave 7 (10 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `scaffold:fxfer-mfe` | skeleton at `apps/fxfer-mfe` | fxfer-mfe | - | - | `scaffold:fxfer-bff` |
| `port:backend-data-file-transfer/1` | 11 of backend-data-file-transfer (part 1/1) | fxfer-bff | 541 | 2 | `scaffold:fxfer-bff` |
| `port:backend-data-file-transfer-tracking/1` | 6 of backend-data-file-transfer-tracking (part 1/1) | fxfer-bff | 161 | 0 | `scaffold:fxfer-bff` |
| `port:backend-file-transfer-mgmt-data-access/3` | 8 of backend-file-transfer-mgmt-data-access (part 3/3) | fxfer-transfer-worker | 123 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-file-transfer-mgmt-data-access/2` |
| `port:backend-providers-file-transfer/1` | 15 of backend-providers-file-transfer (part 1/5) | fxfer-bff | 540 | 1 | `scaffold:fxfer-bff` |
| `port:ui-file-transfer-api-controllers/1` | 1 of ui-file-transfer-api-controllers (part 1/1) | fxfer-bff | 9 | 0 | `scaffold:fxfer-bff` |
| `port:backend-batch-business/2` | 27 of backend-batch-business (part 2/5) | fxfer-transfer-worker | 512 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers/1`, `port:backend-batch-business/1` |
| `port:backend-file-transfer-mgmt-business/2` | 30 of backend-file-transfer-mgmt-business (part 2/7) | fxfer-transfer-worker | 553 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers-2/1`, `port:backend-file-transfer-mgmt-business/1` |
| `port:backend-business-command-handlers-2/1` | 26 of backend-business-command-handlers-2 (part 1/1) | fxfer-domain-api | 289 | 11 | `scaffold:fxfer-domain-api`, `port:backend-business-command-handlers/1`, `port:backend-data-access-event-handlers/1`, `port:backend-data-access-event-handlers-2/1`, `port:shared-contracts-commands/2` |
| `build:T-003` | target-state requirement T-003: Enforce tenant-scoped authorization and invariants at the domain boundary for all command endpoints (not only in the BFF). | fxfer-domain-api | - | - | `scaffold:fxfer-domain-api`, `port:backend-business-command-handlers/1` |

## Wave 8 (12 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `port:backend-data-file-transfer-constants/1` | 16 of backend-data-file-transfer-constants (part 1/1) | fxfer-mfe | 373 | 1 | `scaffold:fxfer-mfe` |
| `port:backend-file-transfer-constants-localization/1` | 7 of backend-file-transfer-constants-localization (part 1/1) | fxfer-mfe | 164 | 1 | `scaffold:fxfer-mfe` |
| `port:backend-localization-labels/1` | 4 of backend-localization-labels (part 1/1) | fxfer-mfe | 103 | 0 | `scaffold:fxfer-mfe` |
| `port:backend-providers-file-transfer/2` | 24 of backend-providers-file-transfer (part 2/5) | fxfer-bff | 575 | 0 | `scaffold:fxfer-bff`, `port:backend-providers-file-transfer/1` |
| `port:shared-data-file-transfer-constants/1` | 3 of shared-data-file-transfer-constants (part 1/1) | fxfer-mfe | 369 | 1 | `scaffold:fxfer-mfe` |
| `port:ui-configure-source-locations-and-destinations-screens/1` | 23 of ui-configure-source-locations-and-destinations-screens (part 1/1) | fxfer-mfe | 285 | 14 | `scaffold:fxfer-mfe` |
| `port:backend-batch-business/3` | 24 of backend-batch-business (part 3/5) | fxfer-transfer-worker | 511 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers/1`, `port:backend-batch-business/2` |
| `port:backend-file-transfer-mgmt-business/3` | 28 of backend-file-transfer-mgmt-business (part 3/7) | fxfer-transfer-worker | 527 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers-2/1`, `port:backend-file-transfer-mgmt-business/2` |
| `port:ui-manage-ftp-servers-and-credentials-screens/1` | 25 of ui-manage-ftp-servers-and-credentials-screens (part 1/1) | fxfer-mfe | 492 | 4 | `scaffold:fxfer-mfe`, `port:backend-data-file-transfer/1` |
| `port:backend-configure-source-locations-and-destinations-api/1` | 25 of backend-configure-source-locations-and-destinations-api (part 1/2) | fxfer-domain-api | 100 | 14 | `scaffold:fxfer-domain-api`, `port:backend-bas-file-transfer-api/1`, `port:backend-bas-file-transfer-svc/1`, `port:backend-bas-file-transfer-svc-2/1`, `port:backend-business-command-handlers/1`, `port:backend-business-command-handlers-2/1` |
| `build:T-001` | target-state requirement T-001: Derive tenant scope from verified identity context (JWT claim tenant_id) and never from request bodies or caller-supplied headers. | fxfer-bff | - | - | `scaffold:fxfer-bff`, `port:ui-file-transfer-api-controllers/1` |
| `build:T-002` | target-state requirement T-002: Implement the BFF as stateless bearer-JWT APIs (no server-side sessions/cookies) and enforce strict CORS plus Origin/Referer checks for state-changing requests. | fxfer-bff | - | - | `scaffold:fxfer-bff`, `port:ui-file-transfer-api-controllers/1` |

## Wave 9 (4 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `port:backend-providers-file-transfer/3` | 4 of backend-providers-file-transfer (part 3/5) | fxfer-bff | 154 | 1 | `scaffold:fxfer-bff`, `port:backend-providers-file-transfer/2` |
| `port:backend-batch-business/4` | 21 of backend-batch-business (part 4/5) | fxfer-transfer-worker | 337 | 1 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers/1`, `port:backend-batch-business/3` |
| `port:backend-file-transfer-mgmt-business/4` | 30 of backend-file-transfer-mgmt-business (part 4/7) | fxfer-transfer-worker | 596 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers-2/1`, `port:backend-file-transfer-mgmt-business/3` |
| `port:backend-configure-source-locations-and-destinations-api/2` | 24 of backend-configure-source-locations-and-destinations-api (part 2/2) | fxfer-domain-api | 96 | 14 | `scaffold:fxfer-domain-api`, `port:backend-bas-file-transfer-api/1`, `port:backend-bas-file-transfer-svc/1`, `port:backend-bas-file-transfer-svc-2/1`, `port:backend-business-command-handlers/1`, `port:backend-business-command-handlers-2/1`, `port:backend-configure-source-locations-and-destinations-api/1` |

## Wave 10 (5 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `port:backend-providers-file-transfer/4` | 27 of backend-providers-file-transfer (part 4/5) | fxfer-bff | 578 | 13 | `scaffold:fxfer-bff`, `port:backend-providers-file-transfer/3` |
| `port:backend-batch-business/5` | 1 of backend-batch-business (part 5/5) | fxfer-transfer-worker | 342 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers/1`, `port:backend-batch-business/4` |
| `port:backend-file-transfer-mgmt-business/5` | 3 of backend-file-transfer-mgmt-business (part 5/7) | fxfer-transfer-worker | 43 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers-2/1`, `port:backend-file-transfer-mgmt-business/4` |
| `port:backend-file-transfer-mgmt-ctrl/1` | 4 of backend-file-transfer-mgmt-ctrl (part 1/1) | fxfer-transfer-worker | 18 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-configure-source-locations-and-destinations-api/2` |
| `wire:manage-ftp-servers-and-credentials` | flow `manage-ftp-servers-and-credentials` across 12 components | - | - | - | `port:backend-business-command-handlers/1`, `port:backend-business-command-handlers-2/1`, `port:backend-configure-source-locations-and-destinations-api/2`, `port:backend-data-access-event-handlers/1`, `port:backend-data-access-event-handlers-2/1`, `port:backend-data-file-transfer/1`, `port:data-data-access-entities/4`, `port:data-data-access-entities-3/4`, `port:shared-contracts-commands/2`, `port:shared-contracts-utilities/1`, `port:ui-configure-source-locations-and-destinations-screens/1`, `port:ui-manage-ftp-servers-and-credentials-screens/1`, `build:T-003`, `build:T-004`, `scaffold:fxfer-domain-api`, `scaffold:fxfer-projection-worker`, `scaffold:fxfer-bff`, `scaffold:fxfer-mfe` |

## Wave 11 (3 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `port:backend-providers-file-transfer/5` | 5 of backend-providers-file-transfer (part 5/5) | fxfer-bff | 281 | 2 | `scaffold:fxfer-bff`, `port:backend-providers-file-transfer/4` |
| `port:backend-file-transfer-mgmt-business/6` | 30 of backend-file-transfer-mgmt-business (part 6/7) | fxfer-transfer-worker | 522 | 0 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers-2/1`, `port:backend-file-transfer-mgmt-business/5` |
| `wire:configure-source-locations-and-destinations` | flow `configure-source-locations-and-destinations` across 8 components | - | - | - | `port:backend-business-command-handlers/1`, `port:backend-business-command-handlers-2/1`, `port:backend-configure-source-locations-and-destinations-api/2`, `port:backend-data-access-event-handlers/1`, `port:backend-data-access-event-handlers-2/1`, `port:shared-contracts-commands/2`, `port:ui-configure-source-locations-and-destinations-screens/1`, `port:ui-manage-ftp-servers-and-credentials-screens/1`, `build:T-003`, `build:T-004`, `scaffold:fxfer-domain-api`, `scaffold:fxfer-projection-worker`, `scaffold:fxfer-mfe`, `wire:manage-ftp-servers-and-credentials` |

## Wave 12 (3 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `port:backend-file-transfer-mgmt-business/7` | 9 of backend-file-transfer-mgmt-business (part 7/7) | fxfer-transfer-worker | 433 | 1 | `scaffold:fxfer-transfer-worker`, `port:backend-business-helpers-2/1`, `port:backend-file-transfer-mgmt-business/6` |
| `build:T-008` | target-state requirement T-008: Prohibit direct database access from the BFF; all persistence must occur via domain services. | fxfer-bff | - | - | `scaffold:fxfer-bff`, `port:backend-providers-file-transfer/5` |
| `wire:manage-integration-files` | flow `manage-integration-files` across 4 components | - | - | - | `port:backend-business-command-handlers/1`, `port:backend-business-command-handlers-2/1`, `port:backend-configure-source-locations-and-destinations-api/2`, `port:shared-contracts-commands/2`, `build:T-003`, `build:T-004`, `scaffold:fxfer-domain-api`, `wire:configure-source-locations-and-destinations` |

## Wave 13 (1 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `wire:run-file-transfer-batch` | flow `run-file-transfer-batch` across 1 components | - | - | - | `port:backend-run-file-transfer-batch-api/1`, `build:T-007`, `scaffold:fxfer-transfer-worker`, `wire:manage-integration-files` |

## Wave 14 (1 tasks)

| Task | Carries | Deployable | Legacy lines | Rules | Waits for |
| --- | --- | --- | ---: | ---: | --- |
| `wire:search-and-track-file-transfers` | flow `search-and-track-file-transfers` across 3 components | - | - | - | `port:backend-configure-source-locations-and-destinations-api/2`, `port:ui-configure-source-locations-and-destinations-screens/1`, `port:ui-manage-ftp-servers-and-credentials-screens/1`, `scaffold:fxfer-domain-api`, `scaffold:fxfer-mfe`, `wire:run-file-transfer-batch` |
