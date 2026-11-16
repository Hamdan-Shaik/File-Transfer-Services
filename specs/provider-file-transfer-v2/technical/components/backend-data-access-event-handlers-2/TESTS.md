---
type: Test Plan
title: technical/components/backend-data-access-event-handlers-2/TESTS
description: The tests that prove the backend-data-access-event-handlers-2 component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-event-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:06:12Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-event-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-event-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 4
  methods: 18
  owned_rules: 0
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-projection-worker
  folder: src/Infrastructure/Projections
  body_sha256: 1cf28f101feb787aff226e9756e490c79e0e1ac29299ac55680ebffe5b1c0bc6
---

﻿# Tests: backend-data-access-event-handlers-2

This component has no standalone HTTP surface; tests focus on projection writes and event-handling semantics.

## Unit tests (projection writers)

Create unit tests using an EF Core test database (SQLite in-memory or EF Core InMemory provider) that verify row-level effects.

### FtpServerProjectionWriter

- `HandleFtpServerCreatedAsync(...)`
  - Inserts exactly one `ftp_server` row.
  - Sets `creation_ts` and `last_modified_ts` to UTC “now” (use a controllable clock in the test harness if available; otherwise assert non-default and monotonic).
- `HandleFtpServerUpdatedAsync(...)`
  - Updates the expected fields and updates `last_modified_ts`.
- `HandleFtpServerInvalidatedAsync(...)`
  - Sets `is_valid` to the payload value and updates `operator_id`.

### FtsInboundTargetProjectionWriter

- `HandleInboundTargetCreatedAsync(...)`
  - Inserts one `fts_inbound_target` row.
  - Generates a new id for the inserted row (does not reuse the payload’s id), matching legacy behavior.
- `HandleInboundTargetInvalidatedAsync(...)`
  - Sets `is_valid=false` regardless of payload.
  - Sets `operator_id` from the envelope user/operator id (legacy uses `event.UserID`).

### FtsIntegrationFileProjectionWriter

- `HandleIntegrationFileCreatedAsync(...)`
  - When `group_info` includes records with `number_of_records > 0`, inserts one row per qualifying group.
  - Does not insert rows for groups with `number_of_records <= 0`.
  - Each inserted row has a distinct generated id (does not reuse the payload’s `fts_integration_file_id`).
  - Each inserted row sets `config_id = null`, `is_processed=false`, `is_valid=true`.
- `HandleIntegrationFileUpdatedAsync(...)`
  - Updates exactly one row by id and sets `is_processed=true`, `config_id`, `is_valid`, `operator_id`.

### FtsSourceLocationProjectionWriter

- `HandleSourceLocationCreatedAsync(...)`
  - Inserts one source location row with a generated source location id.
  - Inserts one destination row per payload destination and sets each destination’s `fts_source_location_id` to the generated source location id.
  - When source location `is_valid=false`, all inserted destination rows are also forced to `is_valid=false`.
- `HandleSourceLocationUpdatedAsync(...)`
  - Updates source location fields.
  - Performs destination upserts:
    - If a destination id exists, updates that destination row.
    - If it does not exist, inserts a new destination row with a generated destination id (ignores payload destination id), matching legacy.
  - When source location `is_valid=false`, marks all destination rows for that source location invalid.
- `HandleSourceLocationInvalidatedAsync(...)`
  - Marks the source location invalid.
  - Marks all destinations for that source location invalid.
- Destination edge cases
  - `UpdateDestinationAsync(...)` should throw if no row exists for the destination id (legacy throws due to `null` dereference).

## Contract tests

- `openapi.yaml` contains `x-no-http-surface: true` and has no defined paths.

## Idempotency and redelivery

Idempotency is expected to be enforced by the projection worker infrastructure (dedup by `(tenant_id, event_id)` per `inputs/target-state.md`). Add one integration-style test at the dispatcher level (outside this component) to verify duplicate delivery does not apply the same payload twice.

