---
type: Deployable Structure
title: 'Structure: fxfer-mfe'
description: 'Skeleton of the fxfer-mfe deployable: projects, folders and cross-cutting setup.'
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-mfe
tags:
- deployable
- mfe
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:53:38Z
sources:
- id: component-backend-data-file-transfer-constants
  resource: /technical/components/backend-data-file-transfer-constants/SPEC.md
  title: Component placed in it
- id: component-backend-file-transfer-constants-localization
  resource: /technical/components/backend-file-transfer-constants-localization/SPEC.md
  title: Component placed in it
- id: component-backend-localization-labels
  resource: /technical/components/backend-localization-labels/SPEC.md
  title: Component placed in it
- id: component-shared-data-file-transfer-constants
  resource: /technical/components/shared-data-file-transfer-constants/SPEC.md
  title: Component placed in it
- id: component-ui-configure-source-locations-and-destinations-screens
  resource: /technical/components/ui-configure-source-locations-and-destinations-screens/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (6 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-mfe
  generation:
    status: done
    attempts: 1
  path: apps/fxfer-mfe
  kind: mfe
  stack: Node.js + Next.js 15 + React 19
  components: 6
  depends_on:
  - fxfer-bff
  body_sha256: a583e00106ee00db6c3da1723e11d77775ddefb8477beec37695034380d499b4
---

# Structure: `fxfer-mfe`

Deployable path (new repository): `apps/fxfer-mfe`

This file describes the skeleton required to create and build the FXFER operator micro-frontend as a Provider Plus micro-frontend remote.

## Stack

- Node.js + Next.js 15 (Pages Router)
- React 19
- TypeScript
- Micro-frontend composition: Module Federation (shell-owned top-level routing; remote-owned internal routing)
- Hosting target: static assets to S3 + CloudFront

## Skeleton (at `apps/fxfer-mfe/`)

Required top-level files:

- `apps/fxfer-mfe/package.json`
- `apps/fxfer-mfe/package-lock.json`
- `apps/fxfer-mfe/tsconfig.json`
- `apps/fxfer-mfe/next.config.(js|mjs|ts)` (Next.js build + Module Federation remote output)
- `apps/fxfer-mfe/public/` (static assets only)
- `apps/fxfer-mfe/src/`

Required source layout:

- `apps/fxfer-mfe/src/pages/`
  - Pages Router entrypoints for the remote (shell mount route prefix is `/fxfer/*`).
  - Keep routing URL-driven (querystring for search state) so browser back/forward restores prior results.
- `apps/fxfer-mfe/src/screens/`
  - `apps/fxfer-mfe/src/screens/manage-ftp-servers-and-credentials/` (component placement)
  - `apps/fxfer-mfe/src/screens/configure-source-locations-and-destinations/` (component placement)
- `apps/fxfer-mfe/src/legacy-constants/` (component placement)
  - `apps/fxfer-mfe/src/legacy-constants/localization/` (component placement)
- `apps/fxfer-mfe/src/api/`
  - BFF client wrapper (fetch-based) targeting `/api/file-transfer/v1`.
- `apps/fxfer-mfe/src/auth/`
  - Shell integration wrapper for obtaining the operator access token (JWT) and user context.
- `apps/fxfer-mfe/src/observability/`
  - Correlation-id + trace context propagation helpers for browser → BFF calls.
- `apps/fxfer-mfe/src/components/`
  - Shared presentational components built on the platform UI kit.
- `apps/fxfer-mfe/src/styles/`

Build output (static hosting):

- `apps/fxfer-mfe/out/` (static export output uploaded to S3)
- `apps/fxfer-mfe/out/remoteEntry.js` (Module Federation remote entrypoint, location may vary by platform baseline)

## Cross-cutting setup (target-state requirements)

### Authentication and tenant context

- Authentication is owned by the shared shell; `fxfer-mfe` does not implement login/logout routes.
- Every browser → BFF request includes `Authorization: Bearer <JWT>`; the JWT contains the `tenant_id` claim used as the authoritative tenant selector.
- Do not persist long-lived tokens in `localStorage`; use the platform shell’s session mechanism.

### API communication

- All backend integration is `UI → fxfer-bff` only; no direct calls from the browser to domain APIs.
- Base path: `/api/file-transfer/v1`.
- Writes include `Idempotency-Key` so operator retries are safe.

### Observability (correlation + tracing)

- Propagate `X-Correlation-ID` on every browser → BFF call (generate UUID v4 when not provided by the shell).
- Propagate W3C Trace Context headers (`traceparent`, `tracestate`, `baggage`) so backend spans join to user actions.
- Use OpenTelemetry instrumentation conventions and tag tenant context consistently (custom attributes prefixed `gainwell.`).

### Error handling (UI contract)

- Treat BFF errors as RFC 7807 Problem Details (`application/problem+json`).
- Error UIs must display a safe summary plus `correlation_id` for support without leaking secrets/PII.

### Security headers and static hosting posture

- Assets are published as static content to S3 and served through CloudFront.
- Configure strict CSP appropriate for React/Next.js and restrict `connect-src` to only approved BFF + telemetry endpoints.
- Shell loads the Module Federation remote using Subresource Integrity (SRI) where supported by the platform baseline.

### Accessibility

- Screens meet WCAG 2.2 AA with keyboard navigation and screen-reader compatible semantics.
- Prefer platform UI kit components for consistent accessibility behaviors.

### Tests

- Unit/component tests: Jest + React Testing Library.
- E2E tests: Playwright (run as part of the UI quality gate for key operator flows).

## Enterprise-Architecture standards applied

- `strategy/white-papers/engineering/enterprise-micro-frontend-architecture.md` section `4.1.3 Routing and Navigation` (shell owns top-level routing; route ownership and deep linking)
- `strategy/white-papers/engineering/enterprise-micro-frontend-architecture.md` section `5.4.3 Module Federation Configuration` (Module Federation remote `remoteEntry.js` + exposed modules)
- `strategy/white-papers/engineering/enterprise-micro-frontend-architecture.md` section `5.6.5 Supply Chain Security` (SRI, dependency scanning, CSP posture for CDN-served remotes)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` sections `2.1 Mandatory Requirements`, `3.6 Semantic Conventions` (W3C trace context; semantic and custom attribute conventions)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` sections `Solution Overview`, `Correlation ID Format Standards` (`X-Correlation-ID` propagation; UUID v4)
- `standards/application/accessibility/STD-APP-ACC-001-accessibility.md` sections `2.1 WCAG 2.2 AA Compliance`, `3.3 Keyboard Navigation` (accessibility conformance and operability)
- `standards/technology/nextjs/STD-TEC-NXT-006-testing.md` sections `2.1 Jest and React Testing Library`, `2.2 Playwright for E2E Testing` (testing toolchain)

