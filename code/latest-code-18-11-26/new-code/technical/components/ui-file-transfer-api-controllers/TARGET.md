---
type: Target Placement
title: 'Target: ui-file-transfer-api-controllers'
description: Where the ui-file-transfer-api-controllers component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/ui-file-transfer-api-controllers
tags:
- component
- provider-file-transfer-v2
- ui
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T00:17:30Z
sources:
- id: inventory
  resource: /technical/components/ui-file-transfer-api-controllers/INVENTORY.yaml
  title: Every legacy class and method (1 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:ui-file-transfer-api-controllers
  generation:
    status: done
    attempts: 1
  tier: ui
  classes: 1
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-bff
  folder: src/Api
  body_sha256: 7dffb49e2188e7e09f8baeca6b7b656f56cde05a84aa85ca4841cbfe72c0190e
---

# Target: `ui-file-transfer-api-controllers`

## Where

- Deployable: `fxfer-bff`
- Folder: `services/fxfer-bff/src/Api`

## What to create

Create the API-layer entry-point wiring in the FXFER BFF needed to enforce tenant trust boundaries and browser security posture across all endpoints.

- `services/fxfer-bff/src/Api/Configuration/TenantResolutionOptions.cs`
  - Define configuration for deriving tenant scope from verified identity context.
  - Do not include configuration knobs that allow tenant_id to be sourced from caller-supplied headers or request bodies.

- `services/fxfer-bff/src/Api/Middleware/TenantContextMiddleware.cs`
  - Resolve `tenant_id` from the validated JWT claim set (`HttpContext.User`).
  - Reject requests where `tenant_id` is missing/empty.
  - Make the resolved `tenant_id` available to downstream endpoint handlers (request-scoped tenant context) and tag logs/traces.

- `services/fxfer-bff/src/Api/Configuration/CorsOptions.cs`
  - Define an explicit allow-list of permitted browser origins for the FXFER UI.

- `services/fxfer-bff/src/Api/Middleware/OriginValidationMiddleware.cs`
  - For state-changing requests, validate `Origin` and/or `Referer` against the configured allow-list.
  - Reject mismatches with a safe Problem Details response.
  - This middleware complements CORS; it does not replace it.

- `services/fxfer-bff/src/Api/Program.cs`
  - Configure JWT bearer authentication (stateless; no sessions/cookies for auth).
  - Configure CORS with explicit allow-list for the FXFER shell/micro-frontend origins.
  - Ensure middleware order supports the trust boundary:
    - authentication
    - tenant resolution (`TenantContextMiddleware`)
    - origin/referer validation for state-changing requests (`OriginValidationMiddleware`)
    - endpoint mapping (endpoint components in `services/fxfer-bff/src/Api/Endpoints`)

## Mapping

This legacy inventory contains no controller action methods; it only provides a concrete controller type that inherits all behavior from the shared CQRS base controller.

| Legacy | Target | Note |
| --- | --- | --- |
| `FileTransferSvcEventServiceController..ctor(IEventDistribution)` | `drop: legacy CQRS event-distribution controller wrapper` | Target BFF uses explicit endpoint modules plus middleware for auth/tenant/origin policies; CQRS EventDistribution controller inheritance is not carried forward. |

## Dependencies

None (no cross-component dependencies by contract).

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-001-backend-for-frontend-pattern.md` section `Solution Overview` (BFF shapes browser-facing API and enforces edge-facing concerns without becoming the domain authority)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Component 3: JWT Validation Middleware (API Services)` (bearer JWT validation middleware posture)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Best Practice 2: Validate All JWT Claims` (issuer/audience/signature/expiry and required-claim validation)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (OpenAPI contract-first posture; endpoint contracts are defined by UI components that consume them)

