---
type: Deployable Overview
title: technical/target/deployables/fxfer-bff/README
description: 'Overview of the fxfer-bff deployable: where it lives, its stack and the flows it serves.'
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
  body_sha256: e02d9994b99a9ca55f8fe79e28e453093ea7ed95cfe73a20ac696f41cf433f4c
---

﻿# Deployable: `fxfer-bff`

`fxfer-bff` is the browser-facing API facade for the FXFER micro-frontend. It is the first FXFER-controlled trust boundary for UI traffic, shaping responses for the UI and calling `fxfer-domain-api` for all authoritative validation and persistence.

- Path in the new repository: `services/fxfer-bff`
- Kind: `bff`
- Stack: .NET 10 (Native AOT)
- Build: `dotnet build services/fxfer-bff`

## Dependencies

- Depends on: `fxfer-domain-api`.
- Depended on by: `fxfer-mfe`.

## Components

| Component | Folder | What it does |
| --- | --- | --- |
| `backend-data-file-transfer` | `src/Contracts` | UI-facing contract models for transfer configuration and FTP server profile operations. |
| `backend-data-file-transfer-tracking` | `src/Contracts/Tracking` | UI-facing contract models for tracking search and tracking detail screens. |
| `backend-providers-file-transfer` | `src/Application/Providers` | Downstream adapters/clients used by the BFF to call `fxfer-domain-api` and fetch supporting data for screens. |
| `ui-file-transfer-api-controllers` | `src/Api` | BFF HTTP API surface for the FXFER micro-frontend (routes under `/api/file-transfer/v1`). |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-001-backend-for-frontend-pattern.md` section `Solution Overview` (BFF as a UI-optimized facade that shapes/aggregates backend data)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (OpenAPI 3.1 as the contract source of truth for BFF routes)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Variation 1: Contract-First (Design-First) - **Recommended for Gainwell**` (contract-first governance)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Variation 1: Centralized JWT with API Gateway Validation` (edge validation posture for browser traffic)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Best Practice 2: Validate All JWT Claims` (defense-in-depth claim validation and required-claim checks)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (tenant context derived from validated identity; no cross-tenant access)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel SDK + OTLP export + W3C propagation)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` propagation and UUID v4 format)
