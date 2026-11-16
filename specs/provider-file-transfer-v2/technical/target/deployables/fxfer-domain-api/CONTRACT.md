---
type: Interface Contract
title: 'Contract: fxfer-domain-api'
description: Endpoints, events, data and interfaces the fxfer-domain-api deployable exposes.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-domain-api
tags:
- deployable
- dotnet-service
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:03:10Z
sources:
- id: component-backend-bas-file-transfer-api
  resource: /technical/components/backend-bas-file-transfer-api/SPEC.md
  title: Component placed in it
- id: component-backend-business-aggregates
  resource: /technical/components/backend-business-aggregates/SPEC.md
  title: Component placed in it
- id: component-backend-business-aggregates-2
  resource: /technical/components/backend-business-aggregates-2/SPEC.md
  title: Component placed in it
- id: component-backend-business-command-handlers
  resource: /technical/components/backend-business-command-handlers/SPEC.md
  title: Component placed in it
- id: component-backend-business-command-handlers-2
  resource: /technical/components/backend-business-command-handlers-2/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (26 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-domain-api
  generation:
    status: done
    attempts: 1
  path: services/fxfer-domain-api
  kind: dotnet-service
  stack: .NET 10 (ASP.NET Core; AOT preferred)
  components: 26
  depends_on:
  - fxfer-db-schema
  body_sha256: bd67074eee90c75433ebaadb7d0134650c433472c095c909143432d5a6508b72
---

# Contract: `fxfer-domain-api`

Deployable path: `services/fxfer-domain-api`

This deployable exposes private, tenant-scoped HTTP APIs used by other FXFER deployables to create/update configuration and to query configuration + tracking state.

## Tenancy and request context

- `tenant_id` is mandatory for all requests that operate on tenant-scoped resources.
- `tenant_id` is derived from validated identity context (JWT claims / service identity) and is not accepted from request bodies or client-controlled headers as the source of truth.

## Authentication

- All endpoints require `Authorization: Bearer <JWT>`.
- The service validates issuer/audience/signature/expiry and required claims (defense in depth).

## Observability headers

Required propagation:

- `X-Correlation-ID`: optional on request; if absent the service generates a UUID v4 and returns it.
- W3C Trace Context: `traceparent`, `tracestate`, `baggage`.

## Base paths and versioning

- All API routes use URI major versioning.
- Domain API base prefix (private): `/api/fxfer-domain/v1`
- Platform health endpoints (public within the runtime boundary):
  - `GET /api/health/live`
  - `GET /api/health/ready`

## Error contract

All error responses use an RFC 7807-compatible Problem Details payload (`application/problem+json`).

Problem Details fields:

| Field | Type | Notes |
| --- | --- | --- |
| `type` | `string` | Stable, URI-like error identifier |
| `title` | `string` | Short, human-readable summary |
| `status` | `int32` | HTTP status code |
| `detail` | `string` | Safe-to-expose detail (no secrets/PII) |
| `instance` | `string` | Request path or request id |
| `correlation_id` | `string` | Mirrors `X-Correlation-ID` |
| `trace_id` | `string` | W3C trace id |
| `errors` | `array<ValidationError>` | Present for validation failures |

`ValidationError`:

| Field | Type | Notes |
| --- | --- | --- |
| `field` | `string` | Field name or JSON pointer |
| `code` | `string` | Stable error code |
| `message` | `string` | Localizable-safe message key or safe message text |

## HTTP endpoints

Unless otherwise stated, all request/response bodies are JSON (`application/json`).

### Health

#### `GET /api/health/live`

- Purpose: process liveness.
- `200 OK`: `HealthResponse`

#### `GET /api/health/ready`

- Purpose: dependency readiness (tenant routing + DB connectivity + event adapter connectivity).
- `200 OK`: `HealthResponse`
- `503 Service Unavailable`: Problem Details

### FTP server profiles

#### `GET /api/fxfer-domain/v1/ftp-servers`

- Purpose: search/list FTP server profiles.
- Query parameters (all optional unless stated):
  - `isValid`: `boolean`
  - `ftpServerId`: `uuid`
  - `ftpServerName`: `string`
  - `ftpDescription`: `string`
  - `nameMatch`: `string` (one of: `contains`, `startsWith`, `endsWith`, `exact`; default `exact`)
  - `descriptionMatch`: `string` (one of: `contains`, `startsWith`, `endsWith`, `exact`; default `exact`)
  - `page`: `int32` (default `1`)
  - `pageSize`: `int32` (default `50`; max bounded)
- `200 OK`: `PagedResult<FtpServer>`

#### `GET /api/fxfer-domain/v1/ftp-servers/{ftpServerId}`

- Purpose: get FTP server details.
- `200 OK`: `FtpServer`
- `404 Not Found`: Problem Details

#### `POST /api/fxfer-domain/v1/ftp-servers`

- Purpose: create an FTP server profile.
- Request: `FtpServerUpsertRequest`
- `201 Created`: `FtpServer` (with `Location` header pointing to `/ftp-servers/{ftpServerId}`)
- `400 Bad Request`: Problem Details (validation)

#### `PUT /api/fxfer-domain/v1/ftp-servers/{ftpServerId}`

- Purpose: update an FTP server profile.
- Request: `FtpServerUpsertRequest`
- `200 OK`: `FtpServer`
- `400 Bad Request`: Problem Details
- `404 Not Found`: Problem Details

#### `PATCH /api/fxfer-domain/v1/ftp-servers/{ftpServerId}`

- Purpose: partial update (notably: deactivate/invalidate; keep existing credential secret reference when omitted).
- Request: `FtpServerPatchRequest`
- `200 OK`: `FtpServer`
- `400 Bad Request`: Problem Details
- `404 Not Found`: Problem Details

### Transfer configurations (source locations + destinations)

In the legacy model a transfer configuration is represented by a source location with one or more destination records.

#### `GET /api/fxfer-domain/v1/transfer-configurations`

- Purpose: search/list configurations.
- Query parameters:
  - `configId`: `int32`
  - `configName`: `string`
  - `ownerName`: `string`
  - `ownerSubsystem`: `uuid`
  - `protocol`: `string`
  - `isActive`: `boolean`
  - `page`: `int32` (default `1`)
  - `pageSize`: `int32` (default `50`; max bounded)
- `200 OK`: `PagedResult<TransferConfigurationSummary>`

#### `GET /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}`

- Purpose: get one configuration including destination list.
- `200 OK`: `TransferConfiguration`
- `404 Not Found`: Problem Details

#### `POST /api/fxfer-domain/v1/transfer-configurations`

- Purpose: create a new configuration.
- Request: `TransferConfigurationUpsertRequest`
- `201 Created`: `TransferConfiguration`
- `400 Bad Request`: Problem Details

#### `PUT /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}`

- Purpose: update an existing configuration.
- Request: `TransferConfigurationUpsertRequest`
- `200 OK`: `TransferConfiguration`
- `400 Bad Request`: Problem Details
- `404 Not Found`: Problem Details

#### `PATCH /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}`

- Purpose: soft-disable/enable, suspend/unsuspend, and other state toggles.
- Request: `TransferConfigurationPatchRequest`
- `200 OK`: `TransferConfiguration`
- `400 Bad Request`: Problem Details
- `404 Not Found`: Problem Details

#### `GET /api/fxfer-domain/v1/source-locations`

- Purpose: lightweight list of active source locations for pick lists.
- Query parameters:
  - `isActive`: `boolean` (default `true`)
- `200 OK`: `array<SourceLocationOption>`

### Integration file definitions

#### `GET /api/fxfer-domain/v1/integration-files`

- Purpose: lookup integration file definitions (active-only by default).
- Query parameters:
  - `integrationFileId`: `uuid`
  - `fileName`: `string`
  - `isValid`: `boolean` (default `true`)
- `200 OK`: `array<IntegrationFileDefinition>`

#### `PATCH /api/fxfer-domain/v1/integration-files/{integrationFileId}`

- Purpose: record processing for an integration file definition (stores configuration number and marks processed; may retire the definition).
- Request: `IntegrationFileProcessingRequest`
- `200 OK`: `IntegrationFileDefinition`
- `400 Bad Request`: Problem Details (missing `configId`)
- `404 Not Found`: Problem Details

### Inbound target metadata

#### `GET /api/fxfer-domain/v1/inbound-targets`

- Purpose: search inbound targets.
- Query parameters:
  - `fteMetadataId`: `uuid`
  - `contractName`: `string`
- `200 OK`: `array<InboundTarget>`

### Tracking

#### `GET /api/fxfer-domain/v1/file-tracking`

- Purpose: search file transfer tracking records.
- Query parameters:
  - `fileId`: `uuid`
  - `fileName`: `string`
  - `fileStatus`: `string`
  - `fileDirection`: `string`
  - `fileTransportType`: `string`
  - `fileSourceLocationId`: `uuid`
  - `startDate`: `date-time`
  - `endDate`: `date-time`
  - `fileSizeFrom`: `decimal`
  - `fileSizeThrough`: `decimal`
  - `tradingPartner`: `string`
  - `page`: `int32` (default `1`)
  - `pageSize`: `int32` (default `50`; max bounded)
- `200 OK`: `PagedResult<FileTransferTracking>`

#### `GET /api/fxfer-domain/v1/file-tracking/{fileId}`

- Purpose: get tracking detail for one file (header + events + exceptions).
- `200 OK`: `FileTransferTracking`
- `404 Not Found`: Problem Details

## Schemas

### `HealthResponse`

| Field | Type | Notes |
| --- | --- | --- |
| `status` | `string` | `ok` or `degraded` |
| `time_utc` | `date-time` | ISO-8601 UTC timestamp |

### `PagedResult<T>`

| Field | Type | Notes |
| --- | --- | --- |
| `items` | `array<T>` | Current page items |
| `total` | `int64` | Total number of matches |
| `page` | `int32` | 1-based page number |
| `pageSize` | `int32` | Page size used |

### `FtpServerUpsertRequest`

Secrets posture: this API does not accept raw passwords; callers provide secret references/handles.

| Field | Type | Notes |
| --- | --- | --- |
| `ftpServerName` | `string` | Required |
| `ftpDescription` | `string` | Required |
| `ftpServerIp` | `string` | Required |
| `ftpTransferMode` | `string` | Required |
| `ftpUser` | `string` | Required |
| `credentialSecretRef` | `string` | Required on create; optional on update (omit to keep existing) |
| `ftpPort` | `int32` | Required; value may be `0` |
| `ftpProxyAddress` | `string` | Optional |
| `ftpProxyPort` | `int32` | Optional; required when `ftpProxyAddress` is present |
| `useSshAuth` | `boolean` | Required |
| `isValid` | `boolean` | Required |
| `operatorId` | `string` | Required |

### `FtpServerPatchRequest`

| Field | Type | Notes |
| --- | --- | --- |
| `ftpServerName` | `string` | Optional |
| `ftpDescription` | `string` | Optional |
| `ftpServerIp` | `string` | Optional |
| `ftpTransferMode` | `string` | Optional |
| `ftpUser` | `string` | Optional |
| `credentialSecretRef` | `string` | Optional (omit to keep existing) |
| `ftpPort` | `int32` | Optional |
| `ftpProxyAddress` | `string` | Optional |
| `ftpProxyPort` | `int32` | Optional |
| `useSshAuth` | `boolean` | Optional |
| `isValid` | `boolean` | Optional (set `false` to deactivate) |
| `operatorId` | `string` | Required |

### `FtpServer`

| Field | Type | Notes |
| --- | --- | --- |
| `ftpServerId` | `uuid` | |
| `ftpServerName` | `string` | |
| `ftpDescription` | `string` | |
| `ftpServerIp` | `string` | |
| `ftpTransferMode` | `string` | |
| `ftpUser` | `string` | |
| `ftpPort` | `int32` | |
| `ftpProxyAddress` | `string` | |
| `ftpProxyPort` | `int32` | |
| `useSshAuth` | `boolean` | |
| `isValid` | `boolean` | |
| `operatorId` | `string` | |
| `creationDate` | `date-time` | |
| `lastModifiedDate` | `date-time` | |

### `TransferConfigurationUpsertRequest`

| Field | Type | Notes |
| --- | --- | --- |
| `configName` | `string` | Required |
| `description` | `string` | Optional |
| `ftpServerId` | `uuid` | Required when `protocol` indicates server-based transfer |
| `address` | `string` | Required |
| `mask` | `string` | Optional |
| `protocol` | `string` | Required |
| `fileDirection` | `string` | Optional |
| `ownerName` | `string` | Required |
| `ownerEmail` | `string` | Required |
| `ownerPhoneNumber` | `string` | Required |
| `ownerSubsystem` | `uuid` | Required |
| `tradingPartner` | `string` | Optional |
| `sendNotification` | `boolean` | Optional |
| `runOnMonday` | `boolean` | Optional |
| `runOnTuesday` | `boolean` | Optional |
| `runOnWednesday` | `boolean` | Optional |
| `runOnThursday` | `boolean` | Optional |
| `runOnFriday` | `boolean` | Optional |
| `runOnSaturday` | `boolean` | Optional |
| `runOnSunday` | `boolean` | Optional |
| `runOnFirstDay` | `boolean` | Optional |
| `runOnLastDay` | `boolean` | Optional |
| `runOnThisDay` | `int32` | Optional; 1-31 when used |
| `startWindow` | `string` | Required; `HHMM` |
| `endWindow` | `string` | Required; `HHMM` |
| `isActive` | `boolean` | Optional |
| `suspended` | `boolean` | Optional |
| `destinations` | `array<DestinationUpsert>` | Required; at least one valid destination |
| `operatorId` | `string` | Required |

### `TransferConfigurationPatchRequest`

| Field | Type | Notes |
| --- | --- | --- |
| `isActive` | `boolean` | Optional |
| `suspended` | `boolean` | Optional |
| `operatorId` | `string` | Required |

### `DestinationUpsert`

| Field | Type | Notes |
| --- | --- | --- |
| `destinationId` | `uuid` | Optional (server may generate) |
| `destinationAddress` | `string` | Required |
| `destinationProtocol` | `string` | Required |
| `ftpServerId` | `uuid` | Optional |
| `renamePattern` | `string` | Optional |
| `overWriteAtDestination` | `boolean` | Optional |
| `compressAtDestination` | `boolean` | Optional |
| `renameAtDestination` | `boolean` | Optional |
| `destinationArchive` | `boolean` | Optional |
| `contactName` | `string` | Optional |
| `contactPhone` | `string` | Optional |
| `contactEmail` | `string` | Optional |
| `isValid` | `boolean` | Optional |
| `fteMetadataId` | `uuid` | Optional |
| `destinationType` | `string` | Optional |
| `contractName` | `string` | Optional |
| `importJobName` | `string` | Optional |
| `operatorId` | `string` | Optional |

### `TransferConfigurationSummary`

| Field | Type |
| --- | --- |
| `ftsSourceLocationId` | `uuid` |
| `configId` | `int32` |
| `configName` | `string` |
| `protocol` | `string` |
| `address` | `string` |
| `ownerName` | `string` |
| `tradingPartner` | `string` |
| `isActive` | `boolean` |
| `suspended` | `boolean` |

### `TransferConfiguration`

| Field | Type |
| --- | --- |
| `ftsSourceLocationId` | `uuid` |
| `configId` | `int32` |
| `configName` | `string` |
| `description` | `string` |
| `ftpServerId` | `uuid` |
| `ftpServerName` | `string` |
| `address` | `string` |
| `mask` | `string` |
| `protocol` | `string` |
| `fileDirection` | `string` |
| `ownerName` | `string` |
| `ownerEmail` | `string` |
| `ownerSubsystem` | `uuid` |
| `ownerPhoneNumber` | `string` |
| `tradingPartner` | `string` |
| `operatorId` | `string` |
| `runOnMonday` | `boolean` |
| `runOnTuesday` | `boolean` |
| `runOnWednesday` | `boolean` |
| `runOnThursday` | `boolean` |
| `runOnFriday` | `boolean` |
| `runOnSaturday` | `boolean` |
| `runOnSunday` | `boolean` |
| `runOnFirstDay` | `boolean` |
| `runOnLastDay` | `boolean` |
| `runOnThisDay` | `int32` |
| `startWindow` | `string` |
| `endWindow` | `string` |
| `sendNotification` | `boolean` |
| `isActive` | `boolean` |
| `validChangeDate` | `date-time` |
| `suspended` | `boolean` |
| `suspendedChangeDate` | `date-time` |
| `lastModifiedDate` | `date-time` |
| `creationDate` | `date-time` |
| `destinations` | `array<Destination>` |

### `Destination`

Same fields as `DestinationUpsert` plus timestamps:

| Field | Type |
| --- | --- |
| `creationDate` | `date-time` |
| `lastModifiedDate` | `date-time` |

### `SourceLocationOption`

| Field | Type |
| --- | --- |
| `ftsSourceLocationId` | `uuid` |
| `configName` | `string` |
| `address` | `string` |

### `IntegrationFileProcessingRequest`

| Field | Type | Notes |
| --- | --- | --- |
| `configId` | `int32` | Required |
| `isValid` | `boolean` | Required; set `false` to retire |

### `IntegrationFileDefinition`

| Field | Type |
| --- | --- |
| `ftsIntegrationFileId` | `uuid` |
| `groupId` | `uuid` |
| `configId` | `int32` |
| `fileName` | `string` |
| `etfOutboundMetadataDefnId` | `uuid` |
| `isProcessed` | `boolean` |
| `isValid` | `boolean` |
| `creationDate` | `date-time` |
| `lastModifiedDate` | `date-time` |

### `InboundTarget`

| Field | Type |
| --- | --- |
| `ftsInboundTargetId` | `uuid` |
| `fteMetadataId` | `uuid` |
| `importJobName` | `string` |
| `contractName` | `string` |
| `isValid` | `boolean` |
| `creationDate` | `date-time` |
| `lastModifiedDate` | `date-time` |

### `FileTransferTracking`

Diagnostic content is redacted/sanitized for non-privileged callers.

| Field | Type |
| --- | --- |
| `fileId` | `uuid` |
| `fileSourceLocationId` | `uuid` |
| `fileName` | `string` |
| `fileStatus` | `string` |
| `fileDirection` | `string` |
| `fileTransportType` | `string` |
| `createdDate` | `date-time` |
| `tradingPartner` | `string` |
| `checkSumCode` | `string` |
| `fileSize` | `decimal` |
| `sourceLocAddress` | `string` |
| `sourceLocConfigName` | `string` |
| `originatingServerName` | `string` |
| `events` | `array<FileEvent>` |
| `exceptions` | `array<FileException>` |

### `FileEvent`

| Field | Type |
| --- | --- |
| `eventId` | `uuid` |
| `fileId` | `uuid` |
| `destinationId` | `uuid` |
| `eventCode` | `string` |
| `eventTrackDescription` | `string` |
| `createdDate` | `date-time` |

### `FileException`

| Field | Type |
| --- | --- |
| `exceptionId` | `uuid` |
| `fileId` | `uuid` |
| `destinationId` | `uuid` |
| `eventId` | `uuid` |
| `exceptionType` | `string` |
| `methodName` | `string` |
| `message` | `string` |
| `stackTrace` | `string` |
| `createdDate` | `date-time` |

## Published events

FXFER publishes tenant-scoped domain events using the platform canonical envelope.

Canonical envelope fields:

| Field | Type |
| --- | --- |
| `tenant_id` | `string` |
| `event_name` | `string` |
| `event_version` | `string` |
| `event_id` | `uuid` |
| `occurred_at` | `date-time` |
| `correlation_id` | `string` |
| `trace_id` | `string` |
| `producer` | `string` |

Event families published by this deployable:

### `Fxfer.ConfigurationChanged` (v1)

Payload (no secrets):

| Field | Type | Notes |
| --- | --- | --- |
| `aggregate_type` | `string` | `ftp_server` / `transfer_configuration` / `integration_file` / `inbound_target` |
| `aggregate_id` | `uuid` | |
| `change_kind` | `string` | `created` / `updated` / `invalidated` / `enabled` / `disabled` |
| `operator_id` | `string` | Actor identifier when available |
| `changed_at` | `date-time` | |

### `Fxfer.SuspensionStateChanged` (v1)

Payload:

| Field | Type |
| --- | --- |
| `source_location_id` | `uuid` |
| `suspended` | `boolean` |
| `reason_code` | `string` |
| `changed_at` | `date-time` |

### `Fxfer.DeliveryOutcomeRecorded` (v1)

Payload (redacted/safe):

| Field | Type |
| --- | --- |
| `file_id` | `uuid` |
| `source_location_id` | `uuid` |
| `destination_id` | `uuid` |
| `outcome_status` | `string` |
| `occurred_at` | `date-time` |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (OpenAPI-first contract governance for service-to-service APIs)
- `patterns/application/api-microservices/PTN-APP-API-016-pagination-pattern.md` section `Solution Overview` (standard pagination envelope for collection endpoints)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Best Practice 2: Validate All JWT Claims` (validate issuer/audience/signature/expiry)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant isolation; audit trails for data access)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (W3C trace context; OTLP export)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` propagation)

