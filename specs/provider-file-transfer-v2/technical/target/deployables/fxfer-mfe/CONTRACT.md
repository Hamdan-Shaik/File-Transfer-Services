---
type: Interface Contract
title: 'Contract: fxfer-mfe'
description: Endpoints, events, data and interfaces the fxfer-mfe deployable exposes.
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
  body_sha256: 45a46f9c3a895f4119055cdb1b8d225353e7f8dc55becc4a942cb6f5b2e2985d
---

# Contract: `fxfer-mfe`

Deployable path (new repository): `apps/fxfer-mfe`

This deployable is a UI remote mounted into the Provider Plus shared shell. Its contract is primarily (1) the micro-frontend integration contract with the shared shell, and (2) the browser → BFF call contract it relies on.

## 1) Shared shell integration contract

### Micro-frontend identity

- Remote name: `fxfer` (used by the shell’s Module Federation configuration)
- Remote entrypoint: `remoteEntry.js` served from the micro-frontend CDN origin

### Exposed modules

The remote exposes the shell-loadable modules required for mount and route delegation:

- `./FxferApp` (root React component rendered by the shell)
- `./FxferRoutes` (route table / router component for internal `/fxfer/*` navigation)

### Route ownership

- Shell owns top-level routing and applies route guards before rendering the remote.
- `fxfer-mfe` owns routing under a single reserved prefix: `/fxfer/*`.

Minimum internal route inventory (deep-linkable, authorization-aware):

- `GET /fxfer/source-locations` (Search Source Locations)
- `GET /fxfer/source-locations/{sourceLocationId}` (View Transfer Configuration)
- `GET /fxfer/source-locations/{sourceLocationId}/edit` (Edit Transfer Configuration)
- `GET /fxfer/ftp-servers` (Search Endpoint Credentials)
- `GET /fxfer/ftp-servers/{ftpServerId}` (View/Edit Endpoint Credentials)
- `GET /fxfer/file-tracking` (Search File Transfer Tracking)
- `GET /fxfer/file-tracking/{fileId}` (File Transfer Tracking Detail)

### Shell-provided runtime context

The remote relies on the shared shell to provide:

- Operator identity context (user id and roles/permissions)
- Tenant context via operator JWT `tenant_id` claim (authoritative tenant selector)
- An access token getter for `Authorization: Bearer <JWT>` (no UI-managed sessions)
- Optional feature flag manifest surfaced to the browser (tenant-scoped)

## 2) Browser → BFF call contract (dependency)

`fxfer-mfe` calls `fxfer-bff` only.

### Base path

- BFF base prefix: `/api/file-transfer/v1`

### Required headers

- `Authorization: Bearer <JWT>`
- `X-Correlation-ID: <uuid-v4>` (if absent, BFF may generate and return one)
- `traceparent`, `tracestate`, `baggage` (W3C Trace Context)
- `Idempotency-Key: <uuid-v4>` on state-changing requests where operator retries are expected

### Error format

- Errors are RFC 7807-compatible Problem Details (`application/problem+json`) with safe fields.
- UI behavior must not depend on downstream/internal error shapes.

### Endpoint inventory relied upon by the UI

The UI relies on (at minimum) the following BFF capability areas:

- Transfer configurations: `/transfer-configurations` and `/transfer-configurations/{sourceLocationId}`
- FTP server profiles: `/ftp-servers` and `/ftp-servers/{ftpServerId}` plus `/ftp-servers/options`
- File tracking: `/file-tracking` and `/file-tracking/{fileId}`
- Reference lookups: `/datalists`, `/source-locations`, `/inbound-target-metadata`

The full request/response shapes are governed by the BFF contract (`technical/target/deployables/fxfer-bff/CONTRACT.md`).

## Enterprise-Architecture standards applied

- `strategy/white-papers/engineering/enterprise-micro-frontend-architecture.md` section `4.1.3 Routing and Navigation` (shell delegates internal routing; deep links across remotes)
- `strategy/white-papers/engineering/enterprise-micro-frontend-architecture.md` section `5.4.3 Module Federation Configuration` (remote identity, `remoteEntry.js`, exposed modules)
- `strategy/white-papers/engineering/enterprise-micro-frontend-architecture.md` section `5.6.5 Supply Chain Security` (SRI + CSP posture for CDN-served remotes)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` header and UUID v4 format)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` sections `2.1 Mandatory Requirements`, `3.6 Semantic Conventions` (W3C trace context propagation; custom attribute prefixing)
- `standards/application/accessibility/STD-APP-ACC-001-accessibility.md` section `2.1 WCAG 2.2 AA Compliance` (operator UI accessibility conformance)
