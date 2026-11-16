---
type: Interface Contract
title: technical/target/deployables/fxfer-bff/CONTRACT
description: Endpoints, events, data and interfaces the fxfer-bff deployable exposes.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-bff
tags:
- bff
- deployable
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:45:39Z
sources:
- id: component-backend-data-file-transfer
  resource: /technical/components/backend-data-file-transfer/SPEC.md
  title: Component placed in it
- id: component-backend-data-file-transfer-tracking
  resource: /technical/components/backend-data-file-transfer-tracking/SPEC.md
  title: Component placed in it
- id: component-backend-providers-file-transfer
  resource: /technical/components/backend-providers-file-transfer/SPEC.md
  title: Component placed in it
- id: component-ui-file-transfer-api-controllers
  resource: /technical/components/ui-file-transfer-api-controllers/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (4 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-bff
  generation:
    status: done
    attempts: 1
  path: services/fxfer-bff
  kind: bff
  stack: .NET 10 (Native AOT)
  components: 4
  depends_on:
  - fxfer-domain-api
  body_sha256: 367eb03277f9491711fd0d2ba9eb4be40e8455362c7101040542ee7b2da1a3b5
---

﻿# Contract: `fxfer-bff`

Deployable path: `services/fxfer-bff`

This deployable exposes the browser-facing, tenant-scoped HTTP API used by the FXFER micro-frontend. It normalizes browser request context (auth/tenant/correlation) and calls `fxfer-domain-api` for all authoritative domain validation and persistence.

## Tenancy and request context

- `tenant_id` is mandatory for all tenant-scoped operations.
- `tenant_id` is derived from validated identity context (JWT claims) and is not accepted from request bodies or caller-provided headers as the source of truth.

## Authentication

- All endpoints require `Authorization: Bearer <JWT>`.
- The edge API gateway validates JWTs; the BFF still validates required claims/expiry/issuer/audience for defense in depth.

## Observability headers

Required propagation:

- `X-Correlation-ID`: optional on request; if absent the BFF generates a UUID v4 and returns it.
- W3C Trace Context: `traceparent`, `tracestate`, `baggage`.

## Idempotency

- For write endpoints where operator retries are expected, the BFF accepts `Idempotency-Key` and forwards it downstream.

## Base paths and versioning

- All BFF API routes use URI major versioning.
- BFF base prefix (browser-facing): `/api/file-transfer/v1`
- Platform health endpoints:
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
| `message` | `string` | Safe message text or localizable-safe message key |

## HTTP endpoints

Unless otherwise stated, all request/response bodies are JSON (`application/json`).

### Health

#### `GET /api/health/live`

- Purpose: process liveness.
- `200 OK`: `HealthResponse`

#### `GET /api/health/ready`

- Purpose: dependency readiness (downstream connectivity and configuration readiness).
- `200 OK`: `HealthResponse`
- `503 Service Unavailable`: Problem Details

### Transfer configurations

#### `GET /api/file-transfer/v1/transfer-configurations`

- Purpose: list/search transfer configurations.
- Query parameters (all optional unless stated):
  - `configId`: `int32`
  - `configName`: `string`
  - `ownerName`: `string`
  - `ownerSubsystem`: `uuid`
  - `protocol`: `string`
  - `isActive`: `boolean`
  - `page`: `int32` (default `1`)
  - `pageSize`: `int32` (default `50`; max bounded)
- `200 OK`: `PagedResult<TransferConfigurationSummary>`

#### `POST /api/file-transfer/v1/transfer-configurations`

- Purpose: create a new transfer configuration.
- Request: `TransferConfigurationUpsertRequest`
- `201 Created`: `TransferConfiguration`
- `400 Bad Request`: Problem Details (validation)

#### `GET /api/file-transfer/v1/transfer-configurations/{sourceLocationId}`

- Purpose: get configuration detail including destination list.
- Path parameters:
  - `sourceLocationId`: `uuid`
- `200 OK`: `TransferConfiguration`
- `404 Not Found`: Problem Details

#### `PUT /api/file-transfer/v1/transfer-configurations/{sourceLocationId}`

- Purpose: update an existing configuration.
- Request: `TransferConfigurationUpsertRequest`
- `200 OK`: `TransferConfiguration`
- `400 Bad Request`: Problem Details
- `404 Not Found`: Problem Details

#### `PATCH /api/file-transfer/v1/transfer-configurations/{sourceLocationId}`

- Purpose: soft-disable/enable, suspend/unsuspend, and other state toggles.
- Request: `TransferConfigurationPatchRequest`
- `200 OK`: `TransferConfiguration`
- `400 Bad Request`: Problem Details
- `404 Not Found`: Problem Details

### FTP server profiles

#### `GET /api/file-transfer/v1/ftp-servers`

- Purpose: list/search FTP server profiles.
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

#### `POST /api/file-transfer/v1/ftp-servers`

- Purpose: create an FTP server profile.
- Request: `FtpServerUpsertRequest`
- `201 Created`: `FtpServer`
- `400 Bad Request`: Problem Details

#### `PUT /api/file-transfer/v1/ftp-servers/{ftpServerId}`

- Purpose: update an FTP server profile.
- Request: `FtpServerUpsertRequest`
- `200 OK`: `FtpServer`
- `400 Bad Request`: Problem Details
- `404 Not Found`: Problem Details

#### `PATCH /api/file-transfer/v1/ftp-servers/{ftpServerId}`

- Purpose: partial update (notably: deactivate/invalidate; keep existing credential secret reference when password is omitted).
- Request: `FtpServerPatchRequest`
- `200 OK`: `FtpServer`
- `400 Bad Request`: Problem Details
- `404 Not Found`: Problem Details

#### `GET /api/file-transfer/v1/ftp-servers/options`

- Purpose: lightweight options list for dropdowns.
- Query parameters:
  - `isValid`: `boolean` (default `true`)
- `200 OK`: `array<FtpServerOption>`

### File tracking

#### `GET /api/file-transfer/v1/file-tracking`

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

#### `GET /api/file-transfer/v1/file-tracking/{fileId}`

- Purpose: get tracking detail for one file (header + events + exceptions).
- `200 OK`: `FileTransferTracking`
- `404 Not Found`: Problem Details

### Source location options

#### `GET /api/file-transfer/v1/source-locations`

- Purpose: lightweight list of active source locations for pick lists.
- Query parameters:
  - `isActive`: `boolean` (default `true`)
- `200 OK`: `array<SourceLocationOption>`

### Data lists

#### `GET /api/file-transfer/v1/datalists`

- Purpose: UI pick list lookup.
- Query parameters:
  - `contentId`: `string` (required)
  - `startsWith`: `string` (optional)
  - `localeId`: `string` (optional)
- `200 OK`: `array<DataListItem>`

### Inbound target metadata

#### `GET /api/file-transfer/v1/inbound-target-metadata`

- Purpose: lookup inbound target metadata identifiers and import job names for a contract.
- Query parameters:
  - `contractName`: `string` (optional)
- `200 OK`: `array<InboundTargetMetadata>`

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

Secrets posture: the BFF does not return passwords and does not log or trace passwords. When a password is provided, the BFF stores it in Secrets Manager and calls `fxfer-domain-api` using a secret reference.

| Field | Type | Notes |
| --- | --- | --- |
| `ftpServerName` | `string` | Required |
| `ftpDescription` | `string` | Required |
| `ftpServerIp` | `string` | Required |
| `ftpTransferMode` | `string` | Required |
| `ftpUser` | `string` | Required |
| `ftpPassword` | `string` | Required on create; optional on update (omit to keep existing) |
| `ftpPasswordConfirmation` | `string` | Required when `ftpPassword` is present |
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
| `ftpPassword` | `string` | Optional (omit to keep existing) |
| `ftpPasswordConfirmation` | `string` | Required when `ftpPassword` is present |
| `ftpPort` | `int32` | Optional |
| `ftpProxyAddress` | `string` | Optional |
| `ftpProxyPort` | `int32` | Optional |
| `useSshAuth` | `boolean` | Optional |
| `isValid` | `boolean` | Optional (set `false` to deactivate) |
| `operatorId` | `string` | Required |

### `FtpServer`

| Field | Type |
| --- | --- |
| `ftpServerId` | `uuid` |
| `ftpServerName` | `string` |
| `ftpDescription` | `string` |
| `ftpServerIp` | `string` |
| `ftpTransferMode` | `string` |
| `ftpUser` | `string` |
| `ftpPort` | `int32` |
| `ftpProxyAddress` | `string` |
| `ftpProxyPort` | `int32` |
| `useSshAuth` | `boolean` |
| `isValid` | `boolean` |
| `operatorId` | `string` |
| `creationDate` | `date-time` |
| `lastModifiedDate` | `date-time` |

### `FtpServerOption`

| Field | Type |
| --- | --- |
| `ftpServerId` | `uuid` |
| `ftpServerName` | `string` |

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

### `DataListItem`

| Field | Type |
| --- | --- |
| `id` | `string` |
| `name` | `string` |

### `InboundTargetMetadata`

| Field | Type |
| --- | --- |
| `fteMetadataId` | `uuid` |
| `importJobName` | `string` |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-001-backend-for-frontend-pattern.md` section `Solution Overview` (BFF composes/shapes responses without becoming the domain authority)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (OpenAPI 3.1 contract-first API governance)
- `patterns/application/api-microservices/PTN-APP-API-016-pagination-pattern.md` section `Solution Overview` (standard pagination envelopes for collection endpoints)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Best Practice 2: Validate All JWT Claims` (validate issuer/audience/signature/expiry and required claims)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (W3C trace context propagation; OTLP export to Collector)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` standard header and UUID v4 format)
