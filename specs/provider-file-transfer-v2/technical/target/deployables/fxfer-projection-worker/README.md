---
type: Deployable Overview
title: 'Deployable: fxfer-projection-worker'
description: 'Overview of the fxfer-projection-worker deployable: where it lives, its stack and the flows it serves.'
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-projection-worker
tags:
- deployable
- projector
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:14:33Z
sources:
- id: component-backend-data-access-event-handlers
  resource: /technical/components/backend-data-access-event-handlers/SPEC.md
  title: Component placed in it
- id: component-backend-data-access-event-handlers-2
  resource: /technical/components/backend-data-access-event-handlers-2/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (2 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-projection-worker
  generation:
    status: done
    attempts: 1
  path: workers/fxfer-projection-worker
  kind: projector
  stack: .NET 10 worker runtime
  components: 2
  depends_on:
  - fxfer-domain-api
  - fxfer-db-schema
  body_sha256: 2f9aad1599a3704813d51f0e57117374e7b17202c1c6a65cdacb45c100633059
---

# Deployable: `fxfer-projection-worker`

Deployable path: `workers/fxfer-projection-worker`

`fxfer-projection-worker` is a .NET 10 worker that consumes tenant-scoped FXFER domain events and updates query/read-model projections in the tenant database. It exists to keep operator-facing reads fast and indexed without forcing the write-side/domain boundary to serve all query shapes directly.

## Stack

- .NET 10 worker runtime
- Consumes events via the platform Event Backbone (SNS/SQS pattern) through an Event Adapter
- Writes projection/read-model tables in per-tenant Aurora PostgreSQL (schema owned by `fxfer-db-schema`)

## Dependencies

- Depends on:
  - `fxfer-domain-api` (event contracts + optional enrichment reads; authoritative producer of FXFER domain events)
  - `fxfer-db-schema` (projection table schema in each tenant database)
- Depended on by:
  - `fxfer-domain-api` query/read endpoints (read models maintained by this worker)

## Components placed in this deployable

| Component | Folder | What it does |
| --- | --- | --- |
| `backend-data-access-event-handlers` | `src/Infrastructure/Projections` | Legacy-derived projection writers for configuration read models (FTP servers, inbound targets, integration files, source locations/destinations/suspensions). |
| `backend-data-access-event-handlers-2` | `src/Infrastructure/Projections` | Same projection-writer surface (variant namespace/source), migrated into the target worker as the baseline projection implementation. |

