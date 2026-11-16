---
type: Interface Contract
title: 'Contract: backend-bas-file-transfer-api'
description: What the backend-bas-file-transfer-api component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-bas-file-transfer-api
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:01:04Z
sources:
- id: inventory
  resource: /technical/components/backend-bas-file-transfer-api/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-bas-file-transfer-api
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 10
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Api
  body_sha256: 6fa68aa75770ce4f8d36766a87c62b38c22c61343409bfb0e1149b26b72ad594
---

# Contract: `backend-bas-file-transfer-api`

This component is the API host wiring/composition root for `fxfer-domain-api` in the target state.
It does not own domain commands/queries/data models; it provides the hosting and middleware foundations that other `src/Api` endpoint components run within.

## Offers

- **API host composition**: the `Program.cs` wiring for ASP.NET Core hosting, service registration, and middleware pipeline.
- **Observability boundary behavior (T-005)**:
  - W3C Trace Context extraction/propagation (`traceparent`, `tracestate`, `baggage`).
  - Correlation id extraction/generation using `X-Correlation-ID`.
  - Telemetry tagging so traces/logs/metrics are taggable with `tenant_id`, `correlation_id`, `trace_id`.

## Uses

None (no cross-component contracts declared by this component).

## Data owned

None.

## Enterprise-Architecture standards applied

- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements`
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions`
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview`
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards`

