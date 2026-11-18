---
type: Behavior Specification
title: 'ui-file-transfer-api-controllers: BFF API entry-point wiring and trust-boundary enforcement'
description: 'What the ui-file-transfer-api-controllers component does today: every class and method, and the business rules it implements.'
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
  body_sha256: ea1066f4de903a8d9766d39ca7de1db84882f532d88714d3aa9d8430722864ec
---

# ui-file-transfer-api-controllers: BFF API entry-point wiring and trust-boundary enforcement

This component represents the legacy API-controller entry point used by the File Transfer UX stack to expose shared CQRS event-distribution HTTP endpoints.
In the target state it becomes the `fxfer-bff` API-layer wiring that enforces authentication/tenancy trust boundaries for all browser-facing endpoints.

## FileTransferSvcEventServiceController
Legacy ASP.NET controller that provides a concrete controller type for File Transfer service event distribution.

- Inherits all HTTP behavior from `EventDistributionBaseApiController` (from the shared `HPP.Core.BAS.CQRS` library).
- Has no controller actions of its own.
- Only responsibility is wiring the `IEventDistribution` dependency into the base controller.

## Rules implemented

None (this component owns no business rules).

## Target-state requirements

### T-001 Derive tenant scope from verified identity context (JWT claim tenant_id) and never from request bodies or caller-supplied headers.
- Purpose: Ensure all BFF operations are tenant-scoped using verified identity context only.
- Inputs:
  - `HttpContext.User` (validated JWT principal).
  - JWT claim: `tenant_id`.
- Logic:
  - Resolve `tenant_id` from the validated JWT claim set.
  - Do not accept `tenant_id` from request bodies, query strings, or caller-supplied headers as the source of truth.
  - Store the resolved `tenant_id` in a request-scoped tenant context for use by endpoint handlers and for tagging logs/traces.
- Data: None.
- Errors:
  - Missing/empty `tenant_id` claim -> reject the request as unauthorized (Problem Details), because tenant-scoped execution cannot be safely determined.
- Target-state section: `inputs/target-state.md` section `10.1 Entry Point and Trust Boundaries`.

### T-002 Implement the BFF as stateless bearer-JWT APIs (no server-side sessions/cookies) and enforce strict CORS plus Origin/Referer checks for state-changing requests.
- Purpose: Ensure the browser-facing API remains stateless and resists CSRF-style cross-site request abuse.
- Inputs:
  - `Authorization: Bearer <JWT>` header.
  - Browser cross-site headers: `Origin` and/or `Referer`.
  - CORS allow-list configuration.
- Logic:
  - Authenticate each request using bearer JWTs; do not use server-side sessions or authentication cookies.
  - Configure CORS with an explicit allow-list of shell/micro-frontend origins.
  - For state-changing requests, validate `Origin`/`Referer` against the allow-list and reject mismatches.
- Data: None.
- Errors:
  - Missing/invalid bearer token -> `401 Unauthorized` (Problem Details).
  - Disallowed origin/referer on state-changing request -> `403 Forbidden` (Problem Details).
- Target-state section: `inputs/target-state.md` section `0.5 Decision Status (UI authentication + CSRF posture)`.

## Not carried over

None.

