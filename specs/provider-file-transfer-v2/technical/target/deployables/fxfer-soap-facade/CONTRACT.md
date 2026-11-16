---
type: Interface Contract
title: 'Contract: fxfer-soap-facade'
description: Endpoints, events, data and interfaces the fxfer-soap-facade deployable exposes.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-soap-facade
tags:
- deployable
- dotnet-service
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:37:46Z
sources:
- id: component-backend-bas-file-transfer-svc
  resource: /technical/components/backend-bas-file-transfer-svc/SPEC.md
  title: Component placed in it
- id: component-backend-bas-file-transfer-svc-2
  resource: /technical/components/backend-bas-file-transfer-svc-2/SPEC.md
  title: Component placed in it
- id: component-backend-file-transfer-svc-main
  resource: /technical/components/backend-file-transfer-svc-main/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (3 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-soap-facade
  generation:
    status: done
    attempts: 1
  path: services/fxfer-soap-facade
  kind: dotnet-service
  stack: .NET 10 (CoreWCF)
  components: 3
  depends_on:
  - fxfer-domain-api
  body_sha256: 5c5a22da0091e71e62862cd92d770b575d9962a9bcbd9fbc27e3e586044566e5
---

# Contract: `fxfer-soap-facade`

Deployable path: `services/fxfer-soap-facade`

This deployable exposes legacy-compatible SOAP operations for FXFER configuration and tracking. It is a compatibility layer: it does not introduce new SOAP functionality and it delegates authoritative processing to `fxfer-domain-api`.

## Tenancy and request context

- All business operations are tenant-scoped.
- `tenant_id` is derived from validated identity context (claims) and is not accepted from caller-controlled payload fields as the source of truth for business operations.

## Transport and bindings

SOAP endpoints are hosted over HTTPS.

Bindings preserved for legacy compatibility:

- `basicHttpsBinding` (primary): HTTPS transport, streamed transfer mode.
- `ws2007FederationHttpBinding` (legacy): `TransportWithMessageCredential` with bearer key; used by legacy callers that require WS-Federation.

Legacy message size limits (carried forward for parity):

- `basicHttpsBinding`: max message size 4,096,000 bytes.
- `ws2007FederationHttpBinding`: max message size 2,048,000 bytes.

## Observability headers

Required propagation on inbound/outbound HTTP (including SOAP):

- `X-Correlation-ID`: optional on request; if absent the service generates a UUID v4 and returns it.
- W3C Trace Context: `traceparent`, `tracestate`, `baggage`.

All OpenTelemetry signals are tagged with `tenant_id`, `correlation_id`, and `trace_id`.

## Base paths and versioning

- SOAP base prefix (versioned): `/soap/file-transfer/v1`
- Platform health endpoints:
  - `GET /api/health/live`
  - `GET /api/health/ready`

## SOAP services

### `FileTransferSvcService` (commands)

- Endpoint: `/soap/file-transfer/v1/FileTransferSvcService.svc`
- ServiceContract namespace: `HP.HSP.FileTransferSvc`

Operations:

| Operation | Request | Response | Faults |
| --- | --- | --- | --- |
| `IsServiceAvailable` | empty | `boolean` | `ServiceException` |
| `IsServiceHealthy` | `tenantId: string` | `boolean` | `ServiceException` |
| `FileTransferAdd` | `FtsSourceLocationAddCommand` | `FtsSourceLocationAdded` | `ServiceException`, `BusinessValidationException` |
| `UpdateFileTransferSourceLocation` | `FtsSourceLocationUpdateCommand` | `FtsSourceLocationUpdated` | `ServiceException`, `BusinessValidationException` |
| `InvalidateFileTransferSourceLocation` | `FtsSourceLocationInvalidateCommand` | `FtsSourceLocationInvalidated` | `ServiceException`, `BusinessValidationException` |
| `AddFtpServer` | `AddFtpServerCommand` | `FtpServerAdded` | `ServiceException`, `BusinessValidationException` |
| `UpdateFtpServer` | `UpdateFtpServerCommand` | `FtpServerUpdated` | `ServiceException`, `BusinessValidationException` |
| `InvalidateFtpServer` | `InvalidateFtpServerCommand` | `FtpServerInvalidated` | `ServiceException`, `BusinessValidationException` |
| `UpdateIntegrationFile` | `UpdateIntegrationFileCommand` | `IntegrationFileUpdated` | `ServiceException`, `BusinessValidationException` |

### `FileTransferSvcQueryService` (queries)

- Endpoint: `/soap/file-transfer/v1/FileTransferSvcQueryService.svc`
- ServiceContract namespace: `HP.HSP.FileTransferSvcQueryService`

Operations:

| Operation | Request | Response | Faults |
| --- | --- | --- | --- |
| `IsServiceAvailable` | empty | `boolean` | `ServiceException` |
| `IsServiceHealthy` | `tenantId: string` | `boolean` | `ServiceException` |
| `SearchSourceLocations` | `SourceLocationQuery` | `SourceLocationQuery` | `ServiceException` |
| `GetFtpServers` | `FtpServerQuery` | `FtpServerQuery` | `ServiceException` |
| `GetSourceLocations` | `GetSourceLocationQuery` | `GetSourceLocationQuery` | `ServiceException` |
| `SearchFileTransferMovement` | `FileTransferTrackingQuery` | `FileTransferTrackingQuery` | `ServiceException` |
| `SearchInboundTarget` | `InboundTargetQuery` | `InboundTargetQuery` | `ServiceException` |
| `GetIntegrationFile` | `GetIntegrationFileQuery` | `GetIntegrationFileQuery` | `ServiceException` |

### `FileTransferSvcEventService` (routing)

- Endpoint: `/soap/file-transfer/v1/FileTransferSvcEventService.svc`
- Purpose: legacy CQRS routing surface used to route a generic `Command` and return a polymorphic `Event`.

Operations:

| Operation | Request | Response | Faults |
| --- | --- | --- | --- |
| `TryRouteCommand` | `Command` | `Event` | `ServiceException` |

## Fault contracts

### `ServiceException`

| Field | Type |
| --- | --- |
| `errorCode` | `StatusCodeType` |
| `errorMessages` | `array<string>` |

`StatusCodeType` enum values:

- `OK` (0)
- `CorruptFile` (9000)
- `Unauthorized` (9010)
- `Duplicate` (9020)
- `NotFound` (9030)
- `Stale` (9040)
- `Error` (9999)

### `BusinessValidationException`

| Field | Type |
| --- | --- |
| `businessMessages` | `array<BusinessExceptionMessage>` |

`BusinessExceptionMessage`:

| Field | Type |
| --- | --- |
| `fieldName` | `string` |
| `messageDefault` | `string` |
| `messageKey` | `string` |
| `messageType` | `MessageTypes` |

`MessageTypes` enum values:

- `Informational` (0)
- `Warning` (1)
- `Error` (2)

## Shared envelope types

### `Command`

| Field | Type | Notes |
| --- | --- | --- |
| `cycleDate` | `date-time?` | Nullable |
| `requestor` | `RequestorModel` | Required for request context |
| `viewTimeStamp` | `date-time` | |

`RequestorModel`:

| Field | Type |
| --- | --- |
| `applicationName` | `string` |
| `browserInfo` | `string` |
| `correlationId` | `string` |
| `identifierId` | `string` |
| `identifierIdType` | `IdentifierIdType` |
| `ipAddress` | `string` |
| `localeCode` | `string` |
| `requestDate` | `date-time` |
| `tenantId` | `string` |

`IdentifierIdType` enum values:

- `User` (0)
- `System` (1)

### `Event`

`Event` is polymorphic; concrete types returned by these services include `FtsSourceLocationAdded`, `FtsSourceLocationUpdated`, `FtsSourceLocationInvalidated`, `FtpServerAdded`, `FtpServerUpdated`, `FtpServerInvalidated`, and `IntegrationFileAdded`/`IntegrationFileUpdated`.

Base fields:

| Field | Type | Notes |
| --- | --- | --- |
| `applicationName` | `string` | |
| `commitId` | `uuid?` | Nullable |
| `correlationId` | `string` | |
| `cycleDate` | `date-time?` | Nullable |
| `eventID` | `string` | |
| `eventSequenceId` | `string` | |
| `inGroupId` | `uuid?` | Nullable |
| `inProxyId` | `uuid?` | Nullable |
| `isLastFromGroup` | `boolean` | |
| `modelID` | `string` | |
| `modelType` | `string` | |
| `timestamp` | `date-time?` | Nullable |
| `userID` | `string` | |
| `userIp` | `string` | |

## Command and event payload types (configuration writes)

### `FtsSourceLocationAddCommand`

| Field | Type |
| --- | --- |
| `address` | `string` |
| `configId` | `int32` |
| `configName` | `string` |
| `creationDate` | `date-time` |
| `description` | `string` |
| `destinationAddress` | `array<FtsDestinationLocationAdded>` |
| `endTime` | `string` |
| `fileDirection` | `string` |
| `ftpServerId` | `string` |
| `ftsSourceLocationId` | `string` |
| `isSuspended` | `boolean` |
| `isSuspendedChangedDate` | `date-time?` |
| `isValid` | `boolean` |
| `lastModifiedDate` | `date-time` |
| `mask` | `string` |
| `operatorId` | `string` |
| `ownerEmail` | `string` |
| `ownerModule` | `string` |
| `ownerName` | `string` |
| `ownerPhone` | `string` |
| `protocol` | `string` |
| `runOnFri` | `boolean` |
| `runOnFrstDayOfMonthInd` | `boolean` |
| `runOnLastDayOfMonthInd` | `boolean` |
| `runOnMonday` | `boolean` |
| `runOnSat` | `boolean` |
| `runOnSun` | `boolean` |
| `runOnThisDayOfMonth` | `uint8` |
| `runOnThrus` | `boolean` |
| `runOnTuesday` | `boolean` |
| `runOnWed` | `boolean` |
| `sendNotifications` | `boolean` |
| `startTime` | `string` |
| `tradingPartner` | `string` |

### `FtsSourceLocationUpdateCommand`

Same shape as `FtsSourceLocationAddCommand`.

### `FtsSourceLocationInvalidateCommand`

| Field | Type |
| --- | --- |
| `configId` | `int32` |
| `destinationAddress` | `array<FtsDestinationLocationAdded>` |
| `ftsSourceLocationId` | `string` |
| `operatorId` | `string` |

### `FtsSourceLocationAdded` / `FtsSourceLocationUpdated`

These concrete `Event` types carry the same fields as `FtsSourceLocationAddCommand`.

### `FtsSourceLocationInvalidated`

| Field | Type |
| --- | --- |
| `configId` | `int64` |
| `destinationAddress` | `array<FtsDestinationLocationAdded>` |
| `operatorId` | `string` |

### `FtsDestinationLocationAdded`

| Field | Type |
| --- | --- |
| `compressAtDestination` | `boolean` |
| `contactEmail` | `string` |
| `contactName` | `string` |
| `contactPhone` | `string` |
| `contractName` | `string` |
| `creationDate` | `date-time` |
| `destinationAddress` | `string` |
| `destinationArchive` | `boolean` |
| `destinationId` | `uuid` |
| `destinationProtocol` | `string` |
| `destinationType` | `string` |
| `fteMetadataId` | `uuid?` |
| `ftpServerId` | `uuid` |
| `importJobName` | `string` |
| `isValid` | `boolean` |
| `lastModifiedDate` | `date-time` |
| `operatorId` | `string` |
| `overWriteAtDestination` | `boolean` |
| `renameAtDestination` | `boolean` |
| `renamePattern` | `string` |
| `sourceLocationId` | `uuid` |

### `AddFtpServerCommand`

| Field | Type |
| --- | --- |
| `creationDate` | `date-time` |
| `ftpDescription` | `string` |
| `ftpPassword` | `string` |
| `ftpPort` | `int16` |
| `ftpProxyAddress` | `string` |
| `ftpProxyPort` | `int16?` |
| `ftpServerId` | `uuid` |
| `ftpServerIp` | `string` |
| `ftpServerName` | `string` |
| `ftpTransferMode` | `string` |
| `ftpUser` | `string` |
| `isValid` | `boolean` |
| `lastModifiedDate` | `date-time` |
| `operatorId` | `string` |
| `useSSHAuth` | `boolean` |

### `UpdateFtpServerCommand`

Same shape as `AddFtpServerCommand`.

### `InvalidateFtpServerCommand`

Same shape as `UpdateFtpServerCommand`.

### `FtpServerAdded`

Same fields as `AddFtpServerCommand` except `creationDate`/`lastModifiedDate` are not present.

### `FtpServerUpdated`

Same shape as `FtpServerAdded`.

### `FtpServerInvalidated`

Same shape as `FtpServerUpdated`.

### `UpdateIntegrationFileCommand`

| Field | Type |
| --- | --- |
| `configId` | `int32?` |
| `ftsIntegrationFileId` | `uuid` |

### `IntegrationFileAdded`

| Field | Type |
| --- | --- |
| `configId` | `int32?` |
| `etfOutboundMetadataId` | `uuid` |
| `fileName` | `string` |
| `ftsIntegrationFileId` | `uuid` |
| `groupInfo` | `array<GroupInfo>` |
| `isProcessed` | `boolean` |
| `isValid` | `boolean` |
| `operatorId` | `string` |

`GroupInfo`:

| Field | Type |
| --- | --- |
| `groupId` | `uuid?` |
| `numberOfRecords` | `int32` |

### `IntegrationFileUpdated`

Same fields as `IntegrationFileAdded` plus:

| Field | Type |
| --- | --- |
| `lastModifiedDate` | `date-time` |

## Query payload types (configuration reads + tracking)

All query request/response types extend the base `Query` envelope.

### `Query` (base)

| Field | Type | Notes |
| --- | --- | --- |
| `byPassCountQuery` | `boolean` | |
| `currentPage` | `int32` | |
| `cycleDate` | `date-time?` | Nullable |
| `pageSize` | `int32` | |
| `requestor` | `RequestorModel` | |
| `rowCount` | `int32` | Populated by service |
| `viewTimeStamp` | `date-time` | |

### `SortCriteria`

| Field | Type |
| --- | --- |
| `fieldName` | `string` |
| `order` | `SortOrder` |

`SortOrder` enum values:

- `Asc` (0)
- `Desc` (1)

### `SearchModeOption`

| Field | Type |
| --- | --- |
| `fieldName` | `string` |
| `filterType` | `SearchMode` |

`SearchMode` enum values:

- `Contains` (0)
- `EndsWith` (1)
- `EqualTo` (2)
- `StartsWith` (3)

### `SourceLocationQuery`

| Field | Type | Notes |
| --- | --- | --- |
| `where` | `SourceLocationQueryParms` | |
| `sortCriteria` | `array<SortCriteria>` | |
| `results` | `array<SourceLocationModel>` | Populated by service |

`SourceLocationQueryParms`:

| Field | Type |
| --- | --- |
| `configId` | `int32?` |
| `configName` | `string` |
| `isActive` | `boolean` |
| `ownerName` | `string` |
| `ownerSubsystem` | `string` |
| `protocol` | `string` |

`SourceLocationModel`:

| Field | Type |
| --- | --- |
| `address` | `string` |
| `configId` | `int32` |
| `configName` | `string` |
| `creationDate` | `date-time` |
| `description` | `string` |
| `destinationAddress` | `array<FtsDestinationLocationAdded>` |
| `endWindow` | `string` |
| `fileDirection` | `string` |
| `ftpServerId` | `uuid?` |
| `ftpServerName` | `string` |
| `ftsSourceLocationId` | `uuid` |
| `isActive` | `boolean` |
| `lastModifiedDate` | `date-time` |
| `mask` | `string` |
| `operatorId` | `string` |
| `ownerEmail` | `string` |
| `ownerName` | `string` |
| `ownerPhoneNumber` | `string` |
| `ownerSubsystem` | `uuid` |
| `protocol` | `string` |
| `runOnFirstDay` | `boolean` |
| `runOnFriday` | `boolean` |
| `runOnLastDay` | `boolean` |
| `runOnMonday` | `boolean` |
| `runOnSaturday` | `boolean` |
| `runOnSunday` | `boolean` |
| `runOnThisDay` | `uint8` |
| `runOnThursday` | `boolean` |
| `runOnTuesday` | `boolean` |
| `runOnWednesday` | `boolean` |
| `sendNotification` | `boolean` |
| `startWindow` | `string` |
| `suspended` | `boolean` |
| `suspendedChangeDate` | `date-time` |
| `tradingPartner` | `string` |
| `validChangeDate` | `date-time` |

### `FtpServerQuery`

| Field | Type | Notes |
| --- | --- | --- |
| `where` | `FtpServerQueryParms` | |
| `sortCriteria` | `array<SortCriteria>` | |
| `results` | `array<FtpServerModel>` | Populated by service |

`FtpServerQueryParms`:

| Field | Type |
| --- | --- |
| `ftpDescription` | `string` |
| `ftpServerId` | `uuid` |
| `ftpServerName` | `string` |
| `ftpUser` | `string` |
| `isValid` | `boolean?` |
| `searchOptions` | `array<SearchModeOption>` |

`FtpServerModel`:

| Field | Type |
| --- | --- |
| `creationDate` | `date-time` |
| `ftpDescription` | `string` |
| `ftpPassword` | `string` |
| `ftpPort` | `int16` |
| `ftpProxyAddress` | `string` |
| `ftpProxyPort` | `int16?` |
| `ftpServerId` | `uuid` |
| `ftpServerIp` | `string` |
| `ftpServerName` | `string` |
| `ftpTransferMode` | `string` |
| `ftpUser` | `string` |
| `isValid` | `boolean` |
| `lastModifiedDate` | `date-time` |
| `operatorId` | `string` |

### `GetSourceLocationQuery`

| Field | Type | Notes |
| --- | --- | --- |
| `where` | `GetSourceLocationQueryParms` | |
| `sortCriteria` | `array<SortCriteria>` | |
| `results` | `array<SourceLocationModel>` | Populated by service |

`GetSourceLocationQueryParms`:

| Field | Type |
| --- | --- |
| `isActive` | `boolean` |
| `sourceLocationId` | `string` |

### `FileTransferTrackingQuery`

| Field | Type | Notes |
| --- | --- | --- |
| `where` | `FileTransferTrackingQueryParms` | |
| `sortCriteria` | `array<SortCriteria>` | |
| `results` | `array<FileTransferTrackingModel>` | Populated by service |

`FileTransferTrackingQueryParms`:

| Field | Type |
| --- | --- |
| `endDate` | `date-time?` |
| `fileDirection` | `string` |
| `fileId` | `string` |
| `fileName` | `string` |
| `fileSizeFrom` | `decimal?` |
| `fileSizeThrough` | `decimal?` |
| `fileSourceLocationId` | `string` |
| `fileStatus` | `string` |
| `fileTransportType` | `string` |
| `startDate` | `date-time?` |
| `tradingPartner` | `string` |

`FileTransferTrackingModel`:

| Field | Type |
| --- | --- |
| `checkSumCode` | `string` |
| `createdDate` | `date-time` |
| `events` | `array<FileEventModel>` |
| `exceptions` | `array<FileExceptionModel>` |
| `fileDirection` | `string` |
| `fileId` | `uuid` |
| `fileName` | `string` |
| `fileSize` | `decimal` |
| `fileSourceLocationId` | `uuid` |
| `fileStatus` | `string` |
| `fileTransportType` | `string` |
| `originatingServerName` | `string` |
| `sourceLocAddress` | `string` |
| `sourceLocConfigName` | `string` |
| `tradingPartner` | `string` |

`FileEventModel`:

| Field | Type |
| --- | --- |
| `createdDate` | `date-time` |
| `destinationId` | `uuid` |
| `eventCode` | `string` |
| `eventId` | `uuid` |
| `eventTrackDescription` | `string` |
| `fileId` | `uuid` |

`FileExceptionModel`:

| Field | Type |
| --- | --- |
| `createdDate` | `date-time` |
| `destinationId` | `uuid` |
| `eventId` | `uuid` |
| `exceptionId` | `uuid` |
| `exceptionType` | `string` |
| `fileId` | `uuid` |
| `message` | `string` |
| `methodName` | `string` |
| `stackTrace` | `string` |

### `InboundTargetQuery`

| Field | Type | Notes |
| --- | --- | --- |
| `where` | `InboundTargetQueryParms` | |
| `sortCriteria` | `array<SortCriteria>` | |
| `results` | `array<FtsInboundTargetModel>` | Populated by service |

`InboundTargetQueryParms`:

| Field | Type |
| --- | --- |
| `contractName` | `string` |
| `fteMetadataId` | `uuid` |

`FtsInboundTargetModel`:

| Field | Type |
| --- | --- |
| `contractName` | `string` |
| `creationDate` | `date-time` |
| `fteMetadataId` | `uuid` |
| `ftsInboundTargetId` | `uuid` |
| `importJobName` | `string` |
| `isValid` | `boolean` |
| `lastModifiedDate` | `date-time` |

### `GetIntegrationFileQuery`

| Field | Type | Notes |
| --- | --- | --- |
| `where` | `IntegrationFileQueryParms` | |
| `sortCriteria` | `array<SortCriteria>` | |
| `results` | `array<FtsIntegrationFileModel>` | Populated by service |

`IntegrationFileQueryParms`:

| Field | Type |
| --- | --- |
| `ftsIntegrationFileId` | `uuid` |
| `fileName` | `string` |

`FtsIntegrationFileModel`:

| Field | Type |
| --- | --- |
| `configId` | `int32?` |
| `creationDate` | `date-time` |
| `etfOutboundMetadataDefnId` | `uuid` |
| `fileName` | `string` |
| `ftsIntegrationFileId` | `uuid` |
| `groupId` | `uuid?` |
| `isProcessed` | `boolean` |
| `isValid` | `boolean` |
| `lastModifiedDate` | `date-time` |

## Enterprise-Architecture standards applied

- `strategy/guidance/security/Application/GUID-SEC-APP-API-SECURITY-2.7.01.md` section `Pattern 1: External API Access with Token-Based Authentication` (token-based auth at the edge)
- `strategy/guidance/security/Application/GUID-SEC-APP-API-SECURITY-2.7.01.md` section `Pattern 2: Internal Service-to-Service API Access (Zero Trust)` (facade-to-domain-api call posture)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel signals and propagation)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` propagation)

