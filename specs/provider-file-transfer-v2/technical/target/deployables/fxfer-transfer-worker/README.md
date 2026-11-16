---
type: Deployable Overview
title: 'Deployable: fxfer-transfer-worker'
description: 'Overview of the fxfer-transfer-worker deployable: where it lives, its stack and the flows it serves.'
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-transfer-worker
tags:
- deployable
- provider-file-transfer-v2
- worker
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:23:01Z
sources:
- id: component-backend-batch-business
  resource: /technical/components/backend-batch-business/SPEC.md
  title: Component placed in it
- id: component-backend-business-helpers
  resource: /technical/components/backend-business-helpers/SPEC.md
  title: Component placed in it
- id: component-backend-business-helpers-2
  resource: /technical/components/backend-business-helpers-2/SPEC.md
  title: Component placed in it
- id: component-backend-data-access-batch-data-access
  resource: /technical/components/backend-data-access-batch-data-access/SPEC.md
  title: Component placed in it
- id: component-backend-data-access-batch-data-access-2
  resource: /technical/components/backend-data-access-batch-data-access-2/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (11 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-transfer-worker
  generation:
    status: done
    attempts: 1
  path: workers/fxfer-transfer-worker
  kind: worker
  stack: .NET 10 worker runtime
  components: 11
  depends_on:
  - fxfer-domain-api
  body_sha256: 91104acb3cdb18097a24195601603045851174fe39e06c06449f6cb8e8e879dd
---

# Deployable: `fxfer-transfer-worker`

Deployable path: `workers/fxfer-transfer-worker`

`fxfer-transfer-worker` is a .NET 10 worker that consumes tenant-scoped FXFER transfer work items, performs protocol-level file I/O against external partner endpoints, and records authoritative outcomes through `fxfer-domain-api`. It is the only FXFER deployable that connects outbound to partner/on-prem endpoints.

## Stack

- .NET 10 worker runtime
- Consumes work items via the platform Event Backbone (SNS/SQS pattern) through an Event Adapter
- Uses Secrets Manager + KMS for tenant-scoped endpoint credentials/certificates

## Dependencies

- Depends on:
  - `fxfer-domain-api` (authoritative configuration reads and outcome recording)
- Depended on by:
  - None directly; outcomes are surfaced through `fxfer-domain-api` (and its projections) to `fxfer-bff`/`fxfer-mfe`.

## Components placed in this deployable

| Component | Folder | What it does |
| --- | --- | --- |
| `backend-run-file-transfer-batch-api` | `src/Worker` | Worker entry point and batch/CLI door for starting transfer processing. |
| `backend-batch-business` | `src/Application` | Legacy-derived transfer execution logic (staging, protocol operations, outcome classification). |
| `backend-file-transfer-mgmt-business` | `src/Application` | Transfer execution logic variant including SFTP support and notification/event helper integration. |
| `backend-business-helpers` | `src/Application/Helpers` | Encryption and IO helper utilities used during transfer processing. |
| `backend-business-helpers-2` | `src/Application/Helpers` | Helper utilities (encryption, event/notification helpers) used during transfer processing. |
| `backend-file-transfer-mgmt-ctrl` | `src/Infrastructure/Adapters` | Adapter surface used to call into domain APIs for integration-file reads/updates. |
| `backend-data-access-batch-data-access` | `src/Infrastructure/DataAccess` | Legacy batch data access surface (tracking/notification/suspension persistence); replaced in target by domain API calls. |
| `backend-data-access-batch-data-access-2` | `src/Infrastructure/DataAccess` | Same legacy batch data access surface (variant namespace/source); replaced in target by domain API calls. |
| `backend-file-transfer-mgmt-data-access` | `src/Infrastructure/DataAccess` | Legacy file-transfer-management data access surface; replaced in target by domain API calls. |
| `shared-common-file-transfer-management-constants` | `src/Domain/Constants` | Shared constants used by transfer processing and outcome mapping. |
| `shared-contracts-batch` | `src/Contracts` | Legacy batch contract models used by the worker internals. |

