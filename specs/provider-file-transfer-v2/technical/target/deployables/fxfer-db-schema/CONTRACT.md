---
type: Interface Contract
title: 'Contract: fxfer-db-schema'
description: Endpoints, events, data and interfaces the fxfer-db-schema deployable exposes.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-db-schema
tags:
- deployable
- provider-file-transfer-v2
- schema
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T15:50:16Z
sources:
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (0 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-db-schema
  generation:
    status: done
    attempts: 1
  path: schema/fxfer-db
  kind: schema
  stack: PostgreSQL migrations (Flyway)
  components: 0
  depends_on: []
  body_sha256: a033974a34a26400f0832641fd3e72dc256f217e440f768519c58f503b5d0bc1
---

# Contract: `fxfer-db-schema`

Deployable path: `schema/fxfer-db`

This deployable owns the FXFER relational schema that is applied to each tenant's Aurora PostgreSQL database via Flyway.

## Tenancy and access

- Tenancy model: database-per-tenant. A single database contains data for exactly one tenant; tables do not include `tenant_id` columns.
- Primary writers/readers: `fxfer-domain-api` (authoritative command/query persistence).
- Projection writer: `fxfer-projection-worker` may write tenant-scoped read models/projections in the same database (per target architecture).
- Other deployables do not connect directly to the database; they rely on domain APIs and events.

## Secrets and sensitive values

Endpoint credentials and private keys are not stored as plaintext in this database; the schema stores references to per-tenant secrets managed in AWS Secrets Manager + KMS. Any column ending with `_secret_ref` is a Secrets Manager identifier/ARN (string) resolved by runtime components.

## Tables (logical contract)

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

- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (data isolation; encryption; audit trail expectations for stored operational records)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (database-per-tenant isolation boundary; no cross-tenant keys)
- `patterns/application/api-microservices/PTN-APP-API-007-database-per-service-pattern.md` section `Solution Overview` (database is owned by FXFER domain boundary; other deployables rely on APIs/events rather than cross-deployable DB access)

Recommended constraints/indexes:
- PK: `ftp_server_id`.
- Case-insensitive uniqueness for active servers: unique on `lower(ftp_server_name)` where `is_valid = true`.

### `fts_source_location`

Stores pickup/source-location configuration (legacy: `FTS_SOURCE_LOCATION`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `fts_source_location_id` | `uuid` | PK |
| `config_id` | `integer` | Business identifier used for lookup/update; unique within a tenant |
| `config_name` | `varchar(50)` | Required |
| `source_protocol_cd` | `varchar(4)` | Required |
| `source_address` | `varchar(248)` | Required |
| `source_file_mask` | `varchar(50)` | Required |
| `ftp_server_id` | `uuid` | Optional FK to `ftp_server.ftp_server_id` |
| `send_notifications_ind` | `boolean` | Required |
| `owner_name` | `varchar(50)` | Required (legacy) |
| `owner_module` | `uuid` | Required (legacy) |
| `owner_phone` | `varchar(50)` | Optional |
| `owner_email` | `varchar(50)` | Optional |
| `description` | `varchar(200)` | Optional |
| `run_on_monday_ind` | `boolean` | Required |
| `run_on_tuesday_ind` | `boolean` | Required |
| `run_on_wednesday_ind` | `boolean` | Required |
| `run_on_thursday_ind` | `boolean` | Required |
| `run_on_friday_ind` | `boolean` | Required |
| `run_on_saturday_ind` | `boolean` | Required |
| `run_on_sunday_ind` | `boolean` | Required |
| `run_on_first_day_of_month_ind` | `boolean` | Required |
| `run_on_last_day_of_month_ind` | `boolean` | Required |
| `run_on_this_day_of_month` | `smallint` | Optional; 1-31 when used |
| `start_time_24hhmm` | `varchar(4)` | Required; 24h time `HHMM` |
| `end_time_24hhmm` | `varchar(4)` | Required; 24h time `HHMM` |
| `file_direction` | `varchar(50)` | Optional |
| `trading_partner` | `varchar(10)` | Optional |
| `is_valid` | `boolean` | Required |
| `is_suspended` | `boolean` | Required |
| `is_suspended_changed_ts` | `timestamptz` | Optional |
| `operator_id` | `varchar(200)` | Required |
| `created_ts` | `timestamptz` | Required |
| `last_modified_ts` | `timestamptz` | Required |

Recommended constraints/indexes:
- PK: `fts_source_location_id`.
- Unique: `config_id`.
- FK: `ftp_server_id` references `ftp_server(ftp_server_id)`.

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

Recommended constraints/indexes:
- PK: `destination_id`.
- FK: `fts_source_location_id` references `fts_source_location(fts_source_location_id)`.
- FK: `ftp_server_id` references `ftp_server(ftp_server_id)`.

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

### `fts_notification`

Stores notification recipients and conditions for a source location (legacy: `FTS_NOTIFICATION`).

| Column | Type | Constraints / Notes |
| --- | --- | --- |
| `notification_id` | `uuid` | PK |
| `fts_source_location_id` | `uuid` | Required FK to `fts_source_location.fts_source_location_id` |
| `email_address` | `varchar(254)` | Required |
| `contact_name` | `varchar(50)` | Optional |
| `contact_phone` | `varchar(15)` | Optional |
| `notify_on_error_ind` | `boolean` | Required |
| `notify_on_success_ind` | `boolean` | Required |
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

