---
type: Interface Contract
title: technical/components/backend-data-access-event-handlers/CONTRACT
description: What the backend-data-access-event-handlers component offers other components and what it uses from them.
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
  body_sha256: faa22e434f6de7c2c50f46e0e75d96f90eb40be9396690480574e4d9d54e491f
---

﻿# Contract: backend-data-access-event-handlers

This component is internal to `fxfer-projection-worker` and does not expose business HTTP endpoints. Its contract is defined by the event payloads it can project and the projection tables it maintains.

This component’s target implementation is merged into the projection writer types created by component `technical/components/backend-data-access-event-handlers-2/`.

## Provided behavior

- Upserts configuration read models (FTP servers, inbound targets, integration files, source locations, destinations) into the tenant-scoped projection database.
- Applies invalidation/disable semantics by setting `is_valid=false` on the affected projection rows.
- Preserves legacy UA3 behaviors that are rule-owned by this component (for example: source location inserts always write `is_valid=true`; destination inserts ignore inbound `destination_id` and always generate a new id).

## Consumed events

This component is invoked by the worker’s dispatcher in response to `Fxfer.ConfigurationChanged` (v1) events (as defined in `technical/target/deployables/fxfer-projection-worker/CONTRACT.md`).

Dispatch rules:

- `aggregate_type=ftp_server`
  - `change_kind=created` → `HandleFtpServerCreatedAsync(...)`
  - `change_kind=updated` → `HandleFtpServerUpdatedAsync(...)`
  - `change_kind=invalidated` → `HandleFtpServerInvalidatedAsync(...)`
- `aggregate_type=inbound_target`
  - `change_kind=created` → `HandleInboundTargetCreatedAsync(...)`
  - `change_kind=invalidated` → `HandleInboundTargetInvalidatedAsync(...)`
- `aggregate_type=integration_file`
  - `change_kind=created` → `HandleIntegrationFileCreatedAsync(...)`
  - `change_kind=updated` → `HandleIntegrationFileUpdatedAsync(...)`
- `aggregate_type=transfer_configuration`
  - `change_kind=created` → `HandleSourceLocationCreatedAsync(...)`
  - `change_kind=updated` → `HandleSourceLocationUpdatedAsync(...)`
  - `change_kind=invalidated` → `HandleSourceLocationInvalidatedAsync(...)`

### Payloads required by this component

The canonical `Fxfer.ConfigurationChanged` event payload may not contain enough detail to build full projection rows. When that is the case, the dispatcher must obtain the current aggregate snapshot (e.g., via `fxfer-domain-api`) and pass the snapshot to the writer as the payload described below.

#### `FtpServerProjectionPayload`

Required fields:

- `ftp_server_id` (uuid)
- `ftp_server_name` (string)
- `ftp_description` (string)
- `ftp_server_host` (string) (legacy name: `FtpServerIp`)
- `ftp_transfer_mode` (string)
- `ftp_user` (string)
- `ftp_password_secret_ref` (string)
- `ftp_port` (number)
- `ftp_proxy_address` (string, nullable)
- `ftp_proxy_port` (number, nullable)
- `use_ssh_auth` (boolean)
- `is_valid` (boolean)
- `operator_id` (string)

Notes:

- The legacy projection stored plaintext `FtpPassword`. The target projection stores only `ftp_password_secret_ref`.

#### `InboundTargetProjectionPayload`

Required fields:

- `fts_inbound_target_id` (uuid) (legacy behavior ignores this value for inserts)
- `fte_metadata_id` (uuid)
- `import_job_name` (string)
- `contract_name` (string)
- `operator_id` (string)
- `is_valid` (boolean)

#### `IntegrationFileCreatedProjectionPayload`

Required fields:

- `fts_integration_file_id` (uuid) (legacy behavior ignores this value for inserts)
- `group_info[]`:
  - `group_id` (uuid)
  - `number_of_records` (integer)
- `config_id` (integer, nullable) (legacy behavior ignores this value for inserts)
- `file_name` (string)
- `etf_outbound_metadata_defn_id` (uuid)
- `operator_id` (string)

#### `IntegrationFileUpdatedProjectionPayload`

Required fields:

- `fts_integration_file_id` (uuid)
- `config_id` (integer, nullable)
- `operator_id` (string)
- `is_valid` (boolean)

#### `SourceLocationProjectionPayload`

Required fields:

- `source_location_id` (uuid string)
- `config_id` (integer)
- `config_name` (string)
- `description` (string)
- `ftp_server_id` (uuid string, nullable)
- `address` (string)
- `mask` (string)
- `protocol` (string)
- `file_direction` (string)
- Owner/contact:
  - `owner_email` (string)
  - `owner_name` (string)
  - `owner_phone` (string)
  - `owner_module` (uuid string)
  - `trading_partner` (string)
- Scheduling:
  - `run_on_monday` / `run_on_tuesday` / `run_on_wed` / `run_on_thrus` / `run_on_fri` / `run_on_sat` / `run_on_sun` (boolean)
  - `run_on_first_day_of_month_ind` / `run_on_last_day_of_month_ind` (boolean)
  - `run_on_this_day_of_month` (number)
  - `start_time` (string)
  - `end_time` (string)
- Status:
  - `send_notifications` (boolean)
  - `is_valid` (boolean) (legacy create behavior ignores this and stores `is_valid=true`)
  - `is_suspended` (boolean)
  - `is_suspended_changed_date` (date-time, nullable)
- `operator_id` (string)
- `destinations[]` (list of `DestinationProjectionPayload`)

#### `SourceLocationInvalidatedProjectionPayload`

Required fields:

- `source_location_id` (uuid string)
- `operator_id` (string)
- `destinations[]` (nullable list of `DestinationProjectionPayload`)

#### `DestinationProjectionPayload`

Required fields:

- `destination_id` (uuid) (legacy insert behavior ignores this value)
- `destination_address` (string)
- `destination_protocol` (string)
- `ftp_server_id` (uuid)
- `rename_pattern` (string)
- `overwrite_at_destination` / `compress_at_destination` / `rename_at_destination` / `destination_archive` (boolean)
- `contact_name` / `contact_phone` / `contact_email` (string)
- `is_valid` (boolean)
- `operator_id` (string)
- `fte_metadata_id` (uuid, nullable)
- `destination_type` (string)
- `contract_name` (string)

## Data written

This component writes the following tenant-scoped projection tables (names per `technical/target/deployables/fxfer-projection-worker/CONTRACT.md`):

- `ftp_server`
- `fts_inbound_target`
- `fts_integration_file`
- `fts_source_location`
- `fts_destination`

## Dependencies

- Uses `FxferProjectionDbContext` + EF Core for persistence.
- Uses AutoMapper for selected event/snapshot → entity mapping.
- Relies on the worker envelope (`FxferEventEnvelope`) for tenant-scoped context, correlation, and tracing scope.
- Merged into the target types owned by component `backend-data-access-event-handlers-2`.

## Enterprise-Architecture standards applied

- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies`
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements`
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions`
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview`
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards`
- `patterns/application/event-driven/PTN-APP-EVT-016-messaging-work-queue-pattern.md` section `Executive Summary > Solution Overview`
- `patterns/application/event-driven/PTN-APP-EVT-017-messaging-publish-subscribe-pattern.md` section `Solution Overview`
- `patterns/application/event-driven/PTN-APP-EVT-019-messaging-dead-letter-channel-pattern.md` section `Solution Overview`
- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Read Model Projection Architecture`
