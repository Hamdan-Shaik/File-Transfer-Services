---
type: Interface Contract
title: 'Contract: backend-file-transfer-svc-main'
description: What the backend-file-transfer-svc-main component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-svc-main
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T20:16:25Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-svc-main/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-svc-main
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 14
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-soap-facade
  folder: src/Host
  body_sha256: e192248398e5b25f367d964251af39139d2d5c850f86819ed18ac5ea94f9eca8
---

# Contract: `backend-file-transfer-svc-main`

This component is host/bootstrap code for the `fxfer-soap-facade` deployable. It does not own business data or persistence; it wires runtime concerns: SOAP endpoint hosting, startup initialization, and host-level observability.

## Provides

- Host entry point (`Program.Main`) for `services/fxfer-soap-facade/src/Host`.
- CoreWCF endpoint mapping for the SOAP services (base prefix `/soap/file-transfer/v1`).
- Host logging facade (`ServiceEventSource`) for startup and request lifecycle events.
- Best-effort tenant startup probe (`Startup.InitializeTenants`) driven from configured tenant ids.

## Uses

- Host configuration keys:
  - `InstrumentationKey` (optional): used to configure the chosen telemetry exporter/backend.
- OpenTelemetry context propagation:
  - W3C Trace Context headers (`traceparent`, `tracestate`, `baggage`).
  - `X-Correlation-ID` (propagate or generate).

## Does not provide

- No REST/HTTP business API surface (SOAP only; health endpoints are deployable-level concerns).
- No domain commands/queries beyond what the hosted SOAP services expose.

## Data ownership

- None.

## Dependencies

- None (per brief).

## Enterprise-Architecture standards applied

- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements`
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards`
