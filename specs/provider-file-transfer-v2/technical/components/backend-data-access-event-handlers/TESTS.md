---
type: Test Plan
title: technical/components/backend-data-access-event-handlers/TESTS
description: The tests that prove the backend-data-access-event-handlers component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-event-handlers
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T00:47:50Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-manage-integration-files
  resource: /technical/flows/manage-integration-files/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-data-access-event-handlers/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-event-handlers
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 4
  methods: 18
  owned_rules: 18
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  depends_on:
  - backend-data-access-event-handlers-2
  deployable: fxfer-projection-worker
  folder: src/Infrastructure/Projections
  body_sha256: 165cc79bccfbe7b699465331c37e88cc4346446d5eec1e12ddb3dc6f60648472
---

﻿# Tests: backend-data-access-event-handlers

This component has no standalone HTTP surface; tests focus on projection writes and event-handling semantics.

The target code for this component is merged into the shared projection writer types created by component `technical/components/backend-data-access-event-handlers-2/`. The tests below apply to those shared target implementations.

## Unit tests (owned rules)

Create unit tests using an EF Core test database (SQLite in-memory or EF Core InMemory provider) that verify row-level effects.

### Source location projection rules (F01-048..054, F01-073..078)

- `F01_048_creates_new_source_location_id_and_sets_utc_timestamps`
  - Given a source location create payload, when handled, then the inserted `fts_source_location` has a generated id and `created_ts`/`last_modified_ts` set to UTC “now”.
- `F01_049_source_location_insert_forces_is_valid_true`
  - Given a create payload with `is_valid=false`, when handled, then the inserted `fts_source_location.is_valid` is `true`.
- `F01_050_empty_ftp_server_id_is_stored_as_null`
  - Given a create payload with `ftp_server_id=''` (or null-equivalent), when handled, then stored `ftp_server_id` is null; when non-empty it is parsed as UUID.
- `F01_051_invalid_source_location_forces_destination_is_valid_false_on_insert`
  - Given a create payload with `is_valid=false` and destinations with `is_valid=true`, when handled, then all inserted destination rows are stored with `is_valid=false`.
- `F01_052_destination_inserts_generate_new_destination_id_and_link_to_source_location`
  - Given a create payload with N destinations, when handled, then N destination rows exist, each has a generated `destination_id` and each has `fts_source_location_id` equal to the generated source location id.
- `F01_053_destination_insert_copies_flags_contact_fields_and_sets_utc_timestamps`
  - Given a destination payload with all flag/contact fields set, when inserted, then the destination row copies those fields and sets `created_ts`/`last_modified_ts` to UTC “now”.
- `F01_054_source_location_create_persists_changes`
  - Given a create payload, when handled, then the transaction is persisted (rows are queryable after handler completion).

- `F01_073_source_location_update_sets_fields_and_last_modified_utc_now`
  - Given an existing source location, when updated, then fields are overwritten from the payload and `last_modified_ts` advances.
- `F01_074_update_calls_destination_upsert_without_null_guard`
  - Given an update payload where `destinations=null`, when handled, then the handler throws during iteration (parity with legacy null-foreach behavior).
- `F01_075_update_with_is_valid_false_marks_all_destinations_invalid_and_sets_operator_id`
  - Given a source location update with `is_valid=false`, when handled, then every destination row linked to the source location is marked `is_valid=false` and stores the event’s operator id.
- `F01_076_invalidate_marks_source_location_invalid_and_stores_operator_id`
  - Given an invalidation payload, when handled, then the source location row is updated with `is_valid=false` and `operator_id` from the payload.
- `F01_077_invalidate_optionally_upserts_destinations_then_marks_all_destinations_invalid`
  - Given an invalidation payload with a non-null `destinations` list, when handled, then destinations are upserted first and then every destination for the source location is marked invalid.
- `F01_078_destination_upsert_updates_existing_else_inserts_new_with_generated_id`
  - Given a destination list containing one existing destination id and one non-existing destination id, when upserted, then:
    - the existing destination row is updated by id;
    - the non-existing destination is inserted with a generated id (ignoring the payload id).

### FTP server projection rules (F02-035..037)

- `F02_035_ftp_server_add_sets_creation_and_last_modified_utc_now`
  - Given an FTP server create payload, when handled, then `created_ts` and `last_modified_ts` are set to UTC “now”.
- `F02_036_ftp_server_update_overwrites_fields_and_sets_last_modified_utc_now`
  - Given an existing FTP server, when updated, then all fields are overwritten and `last_modified_ts` advances and `operator_id` is stored.
- `F02_037_ftp_server_invalidate_sets_is_valid_operator_and_last_modified_utc_now`
  - Given an existing FTP server, when invalidated, then `is_valid` equals payload, `operator_id` equals payload, and `last_modified_ts` advances.

### Integration file projection rules (F03-012..013)

- `F03_012_integration_file_update_sets_configid_processed_flag_is_valid_operator_and_last_modified_utc_now`
  - Given an existing integration file row, when updated, then `config_id` is stored, `is_processed=true`, `is_valid` is stored, `operator_id` is stored, and `last_modified_ts` advances.
- `F03_013_inactive_integration_file_is_not_returned_by_active_lookup`
  - Given an integration file updated with `is_valid=false`, when queried by an “active definitions only” query, then it does not appear. (This is an integration-style test spanning projection write + the query-side filter that enforces `is_valid=true`.)

## Unit tests (non-rule legacy parity)

- Inbound target creation ignores payload id and generates a new `fts_inbound_target_id`.
- Inbound target invalidation hard-codes `is_valid=false` and uses operator/user id from the envelope.
- Integration file creation inserts one row per `group_info` where `number_of_records > 0` and sets `config_id=null`, `is_processed=false`, `is_valid=true`.
- Destination update throws if no row exists for the destination id (legacy null dereference parity).

## Contract tests

- `openapi.yaml` contains `x-no-http-surface: true` and has no defined paths.
- `CONTRACT.md` payload requirements match `technical/target/deployables/fxfer-projection-worker/CONTRACT.md` for `Fxfer.ConfigurationChanged` projection.

## Idempotency and redelivery

Idempotency is expected to be enforced by the projection worker infrastructure (dedup by `(tenant_id, event_id)` per `inputs/target-state.md`). Add one integration-style test at the dispatcher level (outside this component) to verify duplicate delivery does not apply the same payload twice.
