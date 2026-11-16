---
type: Deployable Overview
title: 'Deployable: fxfer-soap-facade'
description: 'Overview of the fxfer-soap-facade deployable: where it lives, its stack and the flows it serves.'
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-soap-facade
tags:
- deployable
- dotnet-service
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:37:46Z
sources:
- id: component-backend-bas-file-transfer-svc
  resource: /technical/components/backend-bas-file-transfer-svc/SPEC.md
  title: Component placed in it
- id: component-backend-bas-file-transfer-svc-2
  resource: /technical/components/backend-bas-file-transfer-svc-2/SPEC.md
  title: Component placed in it
- id: component-backend-file-transfer-svc-main
  resource: /technical/components/backend-file-transfer-svc-main/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (3 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-soap-facade
  generation:
    status: done
    attempts: 1
  path: services/fxfer-soap-facade
  kind: dotnet-service
  stack: .NET 10 (CoreWCF)
  components: 3
  depends_on:
  - fxfer-domain-api
  body_sha256: 053f0338d26ecd78b2c59f682af81af38cde8359cbd18470414604e9a2052976
---

# Deployable: `fxfer-soap-facade`

`fxfer-soap-facade` is a SOAP compatibility service for legacy consumers of FXFER. It exposes the legacy SOAP operations (no new SOAP functionality) and translates those calls into the target `fxfer-domain-api` command/query HTTP APIs.

- Path in the new repository: `services/fxfer-soap-facade`
- Kind: `dotnet-service`
- Stack: .NET 10 (CoreWCF)
- Build: `dotnet build services/fxfer-soap-facade`

## Dependencies

- Depends on: `fxfer-domain-api`.
- Depended on by: legacy SOAP consumers (outside this repo boundary).

## Components

| Component | Folder | What it does |
| --- | --- | --- |
| `backend-bas-file-transfer-svc` | `src/LegacyWcf` | Legacy WCF/IIS-era SOAP contract surface (service and query interfaces + svc host artifacts) used as the parity reference for the compatibility facade. |
| `backend-bas-file-transfer-svc-2` | `src/CoreWcf` | CoreWCF-compatible SOAP contract surface and wiring (service and query interfaces + startup) for .NET 10 hosting. |
| `backend-file-transfer-svc-main` | `src/Host` | The deployable host that exposes SOAP endpoints + health endpoints and calls `fxfer-domain-api` to execute commands and queries. |

## Enterprise-Architecture standards applied

- `strategy/guidance/security/Application/GUID-SEC-APP-API-SECURITY-2.7.01.md` section `Pattern 1: External API Access with Token-Based Authentication` (legacy consumers authenticate via token-based patterns at an edge gateway; the facade treats all calls as authenticated requests)
- `strategy/guidance/security/Application/GUID-SEC-APP-API-SECURITY-2.7.01.md` section `Pattern 2: Internal Service-to-Service API Access (Zero Trust)` (facade-to-domain-api calls use zero-trust, token-based service-to-service patterns)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel traces/metrics/logs; W3C trace context propagation)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (consistent attribute naming; tenant/correlation tagging)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` propagation across ingress and egress)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-004-structured-logging-pattern.md` section `Mandatory Structured Logging Requirements` (JSON structured logging with correlation fields; sensitive-data masking)

