---
type: Interface Contract
title: 'Contract: fxfer-projection-worker'
description: Endpoints, events, data and interfaces the fxfer-projection-worker deployable exposes.
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
  body_sha256: 2450b4ebf40d9c2373aa1eedcccd2979f1056c090e33e07919004cc462c2cc48
---

# Contract: `fxfer-projection-worker`

Deployable path: `workers/fxfer-projection-worker`

This deployable is an internal FXFER projector. It does not expose business HTTP endpoints; other deployables rely on it only through (a) the correctness and availability of the projections it maintains and (b) its ability to consume FXFER domain events reliably.

## Tenancy and processing context

- Tenancy model: database-per-tenant. A single tenant database contains data for exactly one tenant; projection tables do not include `tenant_id` columns.
- The worker processes events as tenant-scoped work items:
  - `tenant_id` comes from the canonical event envelope and is mandatory.
  - The worker uses `tenant_id` to resolve tenant-scoped database credentials and to select the tenant database connection for projection writes.

## Consumed events

The worker consumes FXFER domain events emitted by `fxfer-domain-api` via the Event Backbone using the platform canonical envelope.

Canonical envelope fields:

| Field | Type | Notes |
| --- | --- | --- |
| `tenant_id` | `string` | Tenant identifier (routing key for DB selection) |
| `event_name` | `string` | Event family name (e.g. `Fxfer.ConfigurationChanged`) |
| `event_version` | `string` | Schema version (e.g. `v1`) |
| `event_id` | `uuid` | Unique event identifier (used for idempotency/dedup) |
| `occurred_at` | `date-time` | UTC timestamp |
| `correlation_id` | `string` | Correlation id (logged + attached to spans) |
| `trace_id` | `string` | W3C trace id (continued/linked by consumer) |
| `producer` | `string` | Producing deployable/service name |

### `Fxfer.ConfigurationChanged` (v1)

Purpose: drive projections for configuration read models (FTP servers, transfer configurations, destinations, integration files, inbound targets).

Payload (no secrets):

| Field | Type | Notes |
| --- | --- | --- |
| `aggregate_type` | `string` | `ftp_server` / `transfer_configuration` / `integration_file` / `inbound_target` |
| `aggregate_id` | `uuid` | Aggregate identifier |
| `change_kind` | `string` | `created` / `updated` / `invalidated` / `enabled` / `disabled` |
| `operator_id` | `string` | Actor identifier when available |
| `changed_at` | `date-time` | UTC timestamp |

Projection behavior:

- Handlers upsert the corresponding read model rows (by id) and update `last_modified_ts`.
- Where the event payload is not sufficient to build the full read model, the worker calls `fxfer-domain-api` query endpoints to fetch the current aggregate snapshot and writes the projection from that snapshot.

### `Fxfer.SuspensionStateChanged` (v1)

Purpose: maintain suspension read models used to show operator-visible suspension state/reasons for transfer configurations.

Payload:

| Field | Type | Notes |
| --- | --- | --- |
| `source_location_id` | `uuid` | Transfer configuration/source location id |
| `suspended` | `boolean` | Current suspended state |
| `reason_code` | `string` | Stable reason code (safe-to-expose) |
| `changed_at` | `date-time` | UTC timestamp |

### `Fxfer.DeliveryOutcomeRecorded` (v1)

Purpose: maintain tracking/history read models for operator search/detail views (file, event, exception, delivery outcome).

Payload (redacted/safe):

| Field | Type | Notes |
| --- | --- | --- |
| `file_id` | `uuid` | File tracking id |
| `source_location_id` | `uuid` | Source location (transfer configuration) id |
| `destination_id` | `uuid` | Destination id |
| `outcome_status` | `string` | Outcome classification code |
| `occurred_at` | `date-time` | UTC timestamp |

## Data written (projection/read models)

The worker writes tenant-scoped projection tables in the FXFER tenant database (schema owned by `fxfer-db-schema`). Endpoint credentials and private keys are not stored as plaintext; only secret references are stored (e.g. columns ending in `_secret_ref`).

Unless otherwise stated, timestamps are stored as `timestamptz` in UTC.

### `ftp_server`

Stores definitions for partner servers/endpoints used for protocol transfers (legacy: `FTP_SERVER`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `ftp_server_id` | `uuid` | PK |
| `ftp_server_name` | `varchar(50)` | Required; case-insensitive uniqueness enforced for active servers |
| `ftp_server_host` | `varchar(50)` | Required; hostname/IP |
| `ftp_transfer_mode` | `varchar(15)` | Required; protocol/mode code |
| `ftp_user` | `varchar(50)` | Required |
| `ftp_password_secret_ref` | `text` | Required; Secrets Manager ref for the password/credential |
| `ftp_description` | `varchar(100)` | Required |
| `ftp_port` | `smallint` | Required; port may be `0` per legacy behavior |
| `ftp_proxy_address` | `varchar(200)` | Optional |
| `ftp_proxy_port` | `smallint` | Optional |
| `is_valid` | `boolean` | Required; legacy uses this as active/inactive |
| `use_ssh_auth` | `boolean` | Required |
| `operator_id` | `varchar(200)` | Required; last operator/user id |
| `created_ts` | `timestamptz` | Required |
| `last_modified_ts` | `timestamptz` | Required |

### `fts_source_location`

Stores transfer configurations / source locations (legacy: `FTS_SOURCE_LOCATION`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `fts_source_location_id` | `uuid` | PK |
| `config_id` | `integer` | Business key; required |
| `config_name` | `varchar(50)` | Required |
| `owner_name` | `varchar(30)` | Optional |
| `owner_subsystem` | `uuid` | Optional |
| `ftp_source_location_id` | `uuid` | Required |
| `ftp_server_id` | `uuid` | Optional FK to `ftp_server.ftp_server_id` |
| `source_address` | `varchar(248)` | Required |
| `source_protocol_cd` | `varchar(4)` | Required |
| `source_file_mask` | `varchar(50)` | Required |
| `source_mask_extension` | `varchar(10)` | Optional |
| `recurse_ind` | `boolean` | Required |
| `create_archive_ind` | `boolean` | Required |
| `archive_address` | `varchar(248)` | Optional |
| `is_archive_at_source_ind` | `boolean` | Required |
| `delete_at_source_ind` | `boolean` | Required |
| `keep_archived_files_ind` | `boolean` | Required |
| `source_file_direction_cd` | `varchar(50)` | Optional |
| `trading_partner` | `varchar(10)` | Optional |
| `run_schedule_cd` | `varchar(1)` | Required; legacy schedule code |
| `run_on_mon_ind` | `boolean` | Required |
| `run_on_tue_ind` | `boolean` | Required |
| `run_on_wed_ind` | `boolean` | Required |
| `run_on_thu_ind` | `boolean` | Required |
| `run_on_fri_ind` | `boolean` | Required |
| `run_on_sat_ind` | `boolean` | Required |
| `run_on_sun_ind` | `boolean` | Required |
| `run_on_first_day_of_month_ind` | `boolean` | Required |
| `run_on_last_day_of_month_ind` | `boolean` | Required |
| `run_on_this_day_of_month` | `smallint` | Optional; 1-31 when used |
| `start_time_24hhmm` | `varchar(4)` | Required; 24h time `HHMM` |
| `end_time_24hhmm` | `varchar(4)` | Required; 24h time `HHMM` |
| `file_direction` | `varchar(50)` | Optional |
| `is_valid` | `boolean` | Required |
| `is_suspended` | `boolean` | Required |
| `is_suspended_changed_ts` | `timestamptz` | Optional |
| `operator_id` | `varchar(200)` | Required |
| `created_ts` | `timestamptz` | Required |
| `last_modified_ts` | `timestamptz` | Required |

### `fts_destination`

Stores delivery destinations attached to a source location (legacy: `FTS_DESTINATION`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `destination_id` | `uuid` | PK |
| `fts_source_location_id` | `uuid` | Required FK to `fts_source_location.fts_source_location_id` |
| `destination_address` | `varchar(248)` | Required |
| `dest_protocol_cd` | `varchar(4)` | Required |
| `overwrite_at_dest_ind` | `boolean` | Required |
| `compress_at_dest_ind` | `boolean` | Required |
| `rename_at_dest_ind` | `boolean` | Required |
| `dest_is_archive_ind` | `boolean` | Required |
| `is_valid` | `boolean` | Required |
| `ftp_server_id` | `uuid` | Optional FK to `ftp_server.ftp_server_id` |
| `rename_pattern` | `varchar(50)` | Optional |
| `contact_name` | `varchar(50)` | Optional |
| `contact_phone` | `varchar(15)` | Optional |
| `contact_email` | `varchar(150)` | Optional |
| `operator_id` | `varchar(200)` | Required |
| `created_ts` | `timestamptz` | Required |
| `last_modified_ts` | `timestamptz` | Required |
| `cde_encryption` | `varchar(8)` | Optional |
| `encryption_public_key` | `text` | Optional; public key material (non-secret) |
| `encryption_private_key_secret_ref` | `text` | Optional; Secrets Manager ref |
| `private_key_password_secret_ref` | `text` | Optional; Secrets Manager ref |
| `external_file_defn_id` | `uuid` | Optional |
| `destination_type` | `varchar(200)` | Optional |
| `contract_name` | `varchar(200)` | Optional |

### `fts_suspension`

Stores suspension/error details for a source location (legacy: `FTS_SUSPENSION`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `suspension_id` | `uuid` | PK |
| `fts_source_location_id` | `uuid` | Required FK to `fts_source_location.fts_source_location_id` |
| `error_text` | `varchar(250)` | Required |
| `stack_trace` | `varchar(1000)` | Required |
| `operator_id` | `varchar(200)` | Required |
| `created_ts` | `timestamptz` | Required |
| `last_modified_ts` | `timestamptz` | Required |

### `fts_inbound_target`

Stores inbound target definitions used for integration file processing (legacy: `FTS_INBOUND_TARGET`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `fts_inbound_target_id` | `uuid` | PK |
| `fte_metadata_id` | `uuid` | Required |
| `import_job_name` | `varchar(200)` | Required |
| `contract_name` | `varchar(200)` | Required |
| `operator_id` | `varchar(200)` | Required |
| `is_valid` | `boolean` | Required |
| `created_ts` | `timestamptz` | Required |
| `last_modified_ts` | `timestamptz` | Required |

### `fts_integration_file`

Stores integration file records (legacy: `FTS_INTEGRATION_FILE`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `fts_integration_file_id` | `uuid` | PK |
| `group_id` | `uuid` | Optional |
| `config_id` | `integer` | Optional; links to `fts_source_location.config_id` by business key |
| `file_name` | `varchar(500)` | Required |
| `etf_outbound_metadata_defn_id` | `uuid` | Required |
| `is_processed` | `boolean` | Required |
| `operator_id` | `varchar(200)` | Required |
| `is_valid` | `boolean` | Required |
| `created_ts` | `timestamptz` | Required |
| `last_modified_ts` | `timestamptz` | Required |

### `fts_file_management`

Stores file tracking records (legacy: `FTS_FILE_MANAGEMENT`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `file_id` | `uuid` | PK |
| `file_source_location_id` | `uuid` | Required FK to `fts_source_location.fts_source_location_id` |
| `file_name` | `varchar(500)` | Required |
| `file_status` | `varchar(50)` | Required |
| `file_status_ts` | `timestamptz` | Required |
| `sak_file_track` | `integer` | Optional |
| `file_size` | `numeric` | Required; bytes |
| `checksum_cd` | `varchar(50)` | Optional |
| `operator_id` | `varchar(200)` | Required |
| `last_modified_ts` | `timestamptz` | Required |
| `created_ts` | `timestamptz` | Required |

### `fts_file_event`

Stores tracking events for a file and destination (legacy: `FTS_FILE_EVENT`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `event_id` | `uuid` | PK |
| `file_id` | `uuid` | Required FK to `fts_file_management.file_id` |
| `destination_id` | `uuid` | Required FK to `fts_destination.destination_id` |
| `event_cd` | `varchar(50)` | Required |
| `event_track_description` | `varchar(500)` | Optional |
| `created_ts` | `timestamptz` | Required |
| `operator_id` | `varchar(200)` | Required |
| `last_modified_ts` | `timestamptz` | Required |

### `fts_file_delivery`

Stores delivery outcomes for a file and destination (legacy: `FTS_FILE_DELIVERY`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `delivery_id` | `uuid` | PK |
| `file_id` | `uuid` | Required FK to `fts_file_management.file_id` |
| `destination_id` | `uuid` | Required FK to `fts_destination.destination_id` |
| `delivery_ts` | `timestamptz` | Required |
| `destination_file_name` | `varchar(500)` | Optional |
| `operator_id` | `varchar(200)` | Required |
| `last_modified_ts` | `timestamptz` | Required |
| `created_ts` | `timestamptz` | Required |

### `fts_file_exception`

Stores exception details for a file and destination (legacy: `FTS_FILE_EXCEPTION`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `exception_id` | `uuid` | PK |
| `file_id` | `uuid` | Required FK to `fts_file_management.file_id` |
| `destination_id` | `uuid` | Required FK to `fts_destination.destination_id` |
| `event_id` | `uuid` | Required FK to `fts_file_event.event_id` |
| `exception_type` | `varchar(200)` | Required |
| `method_name` | `varchar(200)` | Optional |
| `message` | `text` | Required |
| `stack_trace` | `text` | Optional |
| `created_ts` | `timestamptz` | Required |
| `operator_id` | `varchar(200)` | Required |
| `last_modified_ts` | `timestamptz` | Required |

## Enterprise-Architecture standards applied

- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (database-per-tenant routing; no cross-tenant projection writes)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel SDK; OTLP export; W3C trace context propagation)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (messaging semantic conventions and required attributes)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (correlation id propagation across async boundaries)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (UUID v4 correlation id; `X-Correlation-ID` naming)
- `patterns/application/event-driven/PTN-APP-EVT-017-messaging-publish-subscribe-pattern.md` section `Solution Overview` (subscriber-owned queues; independent consumption and scaling)
- `patterns/application/event-driven/PTN-APP-EVT-019-messaging-dead-letter-channel-pattern.md` section `Solution Overview` (DLQ posture for unprocessable events; recovery path)

