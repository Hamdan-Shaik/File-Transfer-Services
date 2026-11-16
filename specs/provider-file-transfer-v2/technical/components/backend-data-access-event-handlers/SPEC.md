---
type: Behavior Specification
title: technical/components/backend-data-access-event-handlers/SPEC
description: 'What the backend-data-access-event-handlers component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 5a6359f518bab2fc133b47e49151995331ab89f93c8d7619650ca10ae8c419d9
---

﻿# backend-data-access-event-handlers: projection writers for configuration read models

This component updates configuration-related projection/read-model tables in response to events.
It is called by the projection worker’s event-dispatch layer and relies on EF Core persistence
plus AutoMapper `DynamicMap(...)` for event-to-entity transforms.

## FtpServerProjectionWriter
Writes FTP server projection rows.

### Handle(FtpServerAdded @event)
- Purpose: Create a new `FTP_SERVER` projection row for the added FTP server.
- Inputs: `@event` (`FtpServerAdded`) including `FtpServerId`, server connection fields, `IsValid`, `OperatorId`.
- Logic:
  - Calls `AddFtpServer(@event)`.
- Data: writes `orm:FTP_SERVER`.
- Errors: Any exception from AutoMapper mapping, `Add(...)` persistence, or the underlying DB context propagates to the caller.
- Rules: F02-035.

### Handle(FtpServerUpdated @event)
- Purpose: Update an existing `FTP_SERVER` projection row.
- Inputs: `@event` (`FtpServerUpdated`) including `FtpServerId` and the server connection fields.
- Logic:
  - Calls `Update(@event.FtpServerId, ...)` (from `ProjectionWriterBase`).
  - In the update callback, sets these fields on the existing row:
    - `FtpDescription`, `FtpServerIp`, `FtpServerName`, `FtpPassword`, `FtpPort`, `FtpProxyAddress`, `FtpProxyPort`, `FtpTransferMode`, `FtpUser`, `UseSSHAuth`.
    - `LastModifiedDate = DateTime.UtcNow`.
    - `OperatorId = @event.OperatorId`.
- Data: reads/writes `orm:FTP_SERVER`.
- Errors: Any exception from `Update(...)` / DB context persistence propagates to the caller.
- Rules: F02-036.

### Handle(FtpServerInvalidated @event)
- Purpose: Mark an `FTP_SERVER` projection row as valid/invalid.
- Inputs: `@event` (`FtpServerInvalidated`) including `FtpServerId`, `IsValid`, `OperatorId`.
- Logic:
  - Calls `Update(@event.FtpServerId, ...)`.
  - In the update callback:
    - `IsValid = @event.IsValid`.
    - `OperatorId = @event.OperatorId`.
    - `LastModifiedDate = DateTime.UtcNow`.
- Data: reads/writes `orm:FTP_SERVER`.
- Errors: Any exception from `Update(...)` / DB context persistence propagates to the caller.
- Rules: F02-037.

### AddFtpServer(FtpServerAdded @event)
- Purpose: Build and persist the new `FtpServer` entity used by `Handle(FtpServerAdded)`.
- Inputs: `@event` (`FtpServerAdded`).
- Logic:
  - Creates `new FtpServer()`.
  - Uses AutoMapper via `Mapper.DynamicMap(@event, newFtpServer)` to map event fields to entity fields.
  - Sets timestamps:
    - `CreationDate = DateTime.UtcNow`.
    - `LastModifiedDate = DateTime.UtcNow`.
  - Calls `Add(newFtpServer)` (from `ProjectionWriterBase`).
- Data: writes `orm:FTP_SERVER`.
- Errors: Any exception from AutoMapper mapping, `Add(...)`, or DB persistence propagates to the caller.
- Rules: F02-035.

## FtsInboundTargetProjectionWriter
Writes inbound-target projection rows.

### Handle(FtsInboundTargetAdded ftsInboundTargetAdded)
- Purpose: Create a new `FTS_INBOUND_TARGET` projection row.
- Inputs: `ftsInboundTargetAdded` (`FtsInboundTargetAdded`) including `FTEMetadataId`, `ImportJobName`, `ContractName`, `OperatorId`, `IsValid`.
- Logic:
  - Calls `AddFtsInboundTarget(ftsInboundTargetAdded)`.
  - Returns immediately after the call.
- Data: writes `orm:FTS_INBOUND_TARGET`.
- Errors: Any exception from mapping, `Add(...)`, or DB persistence propagates to the caller.
- Rules: None.

### Handle(FtsInboundTargetInvalidated ftsInboundTargetInvalidated)
- Purpose: Invalidate an existing `FTS_INBOUND_TARGET` projection row.
- Inputs: `ftsInboundTargetInvalidated` (`FtsInboundTargetInvalidated`) including `FtsInboundTargetId`.
- Logic:
  - Calls `Update(ftsInboundTargetInvalidated.FtsInboundTargetId, ...)`.
  - In the update callback:
    - `IsValid = false` (hard-coded; does not use the event’s `IsValid` field).
    - `LastModifiedDate = DateTime.UtcNow`.
    - `OperatorId = ftsInboundTargetInvalidated.UserID` (from the CQRS base `Event` type).
- Data: reads/writes `orm:FTS_INBOUND_TARGET`.
- Errors: Any exception from `Update(...)` / DB persistence propagates to the caller.
- Rules: None.

### AddFtsInboundTarget(FtsInboundTargetAdded ftsInboundTargetAdded)
- Purpose: Persist a mapped inbound-target entity.
- Inputs: `ftsInboundTargetAdded` (`FtsInboundTargetAdded`).
- Logic:
  - Calls `MapFtsInboundTarget(ftsInboundTargetAdded)`.
  - Calls `Add(mappedEntity)` (from `ProjectionWriterBase`).
- Data: writes `orm:FTS_INBOUND_TARGET`.
- Errors: Any exception from mapping or persistence propagates to the caller.
- Rules: None.

### MapFtsInboundTarget(FtsInboundTargetAdded ftsInboundTargetAdded)
- Purpose: Build an `FtsInboundTarget` entity instance from the event.
- Inputs: `ftsInboundTargetAdded` (`FtsInboundTargetAdded`).
- Logic:
  - Creates `new FtsInboundTarget()`.
  - Uses AutoMapper via `Mapper.DynamicMap(ftsInboundTargetAdded, ftsInboundTarget)`.
  - Sets/overrides fields explicitly:
    - `FtsInboundTargetId = Guid.NewGuid()` (ignores `ftsInboundTargetAdded.FtsInboundTargetId`).
    - `FTEMetadataId = ftsInboundTargetAdded.FTEMetadataId`.
    - `ImportJobName = ftsInboundTargetAdded.ImportJobName`.
    - `ContractName = ftsInboundTargetAdded.ContractName`.
    - `OperatorId = ftsInboundTargetAdded.OperatorId`.
    - `CreationDate = DateTime.UtcNow`.
    - `LastModifiedDate = DateTime.UtcNow`.
  - Returns the entity.
- Data: none directly (mapping only).
- Errors: Any exception from AutoMapper mapping propagates to the caller.
- Rules: None.

## FtsIntegrationFileProjectionWriter
Writes integration-file projection rows.

### Handle(IntegrationFileAdded integrationFileAdded)
- Purpose: Create one or more `FTS_INTEGRATION_FILE` projection rows for a newly observed integration file.
- Inputs: `integrationFileAdded` (`IntegrationFileAdded`) including `GroupInfo` list, `FileName`, `ETFOutboundMetadataId`, `OperatorId`.
- Logic:
  - Calls `AddFtsIntegrationFile(integrationFileAdded)`.
  - Returns immediately after the call.
- Data: writes `orm:FTS_INTEGRATION_FILE`.
- Errors: Any exception from mapping, LINQ filtering, `Add(...)`, or DB persistence propagates to the caller.
- Rules: None.

### Handle(IntegrationFileUpdated integrationFileUpdated)
- Purpose: Mark a specific `FTS_INTEGRATION_FILE` projection row as processed and update validity/config linkage.
- Inputs: `integrationFileUpdated` (`IntegrationFileUpdated`) including `FtsIntegrationFileId`, `ConfigId`, `IsValid`, `OperatorId`.
- Logic:
  - Calls `Update(integrationFileUpdated.FtsIntegrationFileId, ...)`.
  - In the update callback:
    - `ConfigId = integrationFileUpdated.ConfigId`.
    - `IsProcessed = true` (hard-coded).
    - `IsValid = integrationFileUpdated.IsValid`.
    - `LastModifiedDate = DateTime.UtcNow`.
    - `OperatorId = integrationFileUpdated.OperatorId`.
- Data: reads/writes `orm:FTS_INTEGRATION_FILE`.
- Errors: Any exception from `Update(...)` / DB persistence propagates to the caller.
- Rules: F03-012, F03-013.

### AddFtsIntegrationFile(IntegrationFileAdded integrationFileAdded)
- Purpose: Persist one `FtsIntegrationFile` entity per qualifying group entry.
- Inputs: `integrationFileAdded` (`IntegrationFileAdded`) including `GroupInfo`.
- Logic:
  - Iterates `integrationFileAdded.GroupInfo.Where(e => e.NumberOfRecords > 0)`.
  - For each qualifying `groupInfo`, calls `MapFtsIntegrationFile(integrationFileAdded, groupInfo)` and `Add(mappedEntity)`.
- Data: writes `orm:FTS_INTEGRATION_FILE`.
- Errors: Any exception from `GroupInfo` enumeration, mapping, or persistence propagates to the caller.
- Rules: None.

### MapFtsIntegrationFile(IntegrationFileAdded integrationFileAdded, GroupInfo groupInfo)
- Purpose: Build an `FtsIntegrationFile` entity instance for one group.
- Inputs:
  - `integrationFileAdded` (`IntegrationFileAdded`).
  - `groupInfo` (`GroupInfo`).
- Logic:
  - Creates `new FtsIntegrationFile()`.
  - Uses AutoMapper via `Mapper.DynamicMap(integrationFileAdded, ftsIntegrationFile)`.
  - Sets/overrides fields explicitly:
    - `FtsIntegrationFileId = Guid.NewGuid()` (ignores any inbound id).
    - `GroupId = groupInfo.GroupId`.
    - `ConfigId = null`.
    - `FileName = integrationFileAdded.FileName`.
    - `ETFOutboundMetadataDefnId = integrationFileAdded.ETFOutboundMetadataId`.
    - `IsProcessed = false`.
    - `CreationDate = DateTime.UtcNow`.
    - `LastModifiedDate = DateTime.UtcNow`.
    - `IsValid = true`.
    - `OperatorId = integrationFileAdded.OperatorId`.
  - Returns the entity.
- Data: none directly (mapping only).
- Errors: Any exception from AutoMapper mapping propagates to the caller.
- Rules: None.

## FtsSourceLocationProjectionWriter
Writes source-location and destination projection rows.

### Handle(FtsSourceLocationAdded @event)
- Purpose: Insert a new source location row and insert destination rows linked to it.
- Inputs: `@event` (`FtsSourceLocationAdded`) including source-location fields, destination list `DestinationAddress`, and flags including `IsValid` and `IsSuspended`.
- Logic:
  - Generates a new source location id and stores it in the instance field `ftsCommonSourceLocationId`:
    - `ftsCommonSourceLocationId = Guid.NewGuid()`.
  - Calls `Add(new FtsSourceLocation { ... })` with:
    - `FtsSourceLocationId = ftsCommonSourceLocationId`.
    - `FtpServerId = null` when `@event.FtpServerId` is empty, otherwise `Guid.Parse(@event.FtpServerId)`.
    - `OwnerModule = Guid.Parse(@event.OwnerModule)`.
    - `IsValid = true` (hard-coded).
    - `CreationDate = DateTime.UtcNow`, `LastModifiedDate = DateTime.UtcNow`.
    - Remaining fields copied directly from the event.
  - Iterates `@event.DestinationAddress`:
    - If `@event.IsValid == false`, forces each inbound destination object to `dest.IsValid = false` before insert.
    - Calls `AddDestination(dest, null)`.
  - Calls `contextBase.SaveChanges()`.
- Data: writes `orm:FTS_SOURCE_LOCATION`, `orm:FTS_DESTINATION`.
- Errors:
  - `Guid.Parse(@event.FtpServerId)` and `Guid.Parse(@event.OwnerModule)` throw if the strings are not valid GUIDs.
  - A null `@event.DestinationAddress` throws during `foreach`.
  - Any exception from `Add(...)`, `AddDestination(...)`, or `SaveChanges()` propagates to the caller.
- Rules: F01-048, F01-049, F01-050, F01-051, F01-052, F01-053, F01-054.

### Handle(FtsSourceLocationUpdated @event)
- Purpose: Update a source location row and upsert destination rows.
- Inputs: `@event` (`FtsSourceLocationUpdated`) including `FtsSourceLocationsId`, source-location fields, destination list `DestinationAddress`, and flags including `IsValid`.
- Logic:
  - Parses the source location id and stores it in the instance field:
    - `ftsCommonSourceLocationId = new Guid(@event.FtsSourceLocationsId)`.
  - Calls `Update(Guid.Parse(@event.FtsSourceLocationsId), ...)`.
  - In the update callback, assigns all persisted fields from the event, including:
    - `FtpServerId = null` when `@event.FtpServerId` is empty, otherwise `Guid.Parse(@event.FtpServerId)`.
    - `OwnerModule = Guid.Parse(@event.OwnerModule)`.
    - `IsSuspended`, `IsSuspendedChangedDate`, `IsValid`.
    - `LastModifiedDate = DateTime.UtcNow`.
    - `OperatorId = @event.OperatorId`.
  - If `@event.FtsSourceLocationsId.ToString() != null` (always true when `FtsSourceLocationsId` is non-null):
    - Calls `AddDestinationAddress(@event.DestinationAddress, @event.UserID)` (no null-guard on `DestinationAddress`).
    - If `@event.IsValid == false`:
      - Loads all destination rows where `FtsSourceLocationId == ftsCommonSourceLocationId`.
      - For each destination row, sets `IsValid = false` and `OperatorId = @event.OperatorId`.
      - Calls `contextBase.SaveChanges()`.
- Data: reads/writes `orm:FTS_SOURCE_LOCATION`, `orm:FTS_DESTINATION`.
- Errors:
  - `Guid.Parse(...)` throws if `@event.FtsSourceLocationsId`, `@event.FtpServerId` (when present), or `@event.OwnerModule` are not valid GUID strings.
  - A null `@event.DestinationAddress` throws during `foreach` inside `AddDestinationAddress(...)`.
  - Any exception from `Update(...)`, destination upsert logic, or `SaveChanges()` propagates to the caller.
- Rules: F01-073, F01-074, F01-075.

### Handle(FtsSourceLocationInvalidated @event)
- Purpose: Mark the source location row invalid and invalidate its destination rows.
- Inputs: `@event` (`FtsSourceLocationInvalidated`) including `FtsSourceLocationsId`, `OperatorId`, optional `DestinationAddress`, and `UserID`.
- Logic:
  - Parses the source location id and stores it in the instance field:
    - `ftsCommonSourceLocationId = new Guid(@event.FtsSourceLocationsId)`.
  - Calls `Update(Guid.Parse(@event.FtsSourceLocationsId), ...)`.
  - In the update callback:
    - `OperatorId = @event.OperatorId`.
    - `IsValid = false`.
    - Does not update `LastModifiedDate`.
  - If `@event.DestinationAddress != null`, calls `AddDestinationAddress(@event.DestinationAddress, @event.UserID)`.
  - Loads all destination rows where `FtsSourceLocationId == sourceLocationId` and sets:
    - `IsValid = false`.
    - `OperatorId = @event.OperatorId`.
  - Calls `contextBase.SaveChanges()`.
- Data: reads/writes `orm:FTS_SOURCE_LOCATION`, `orm:FTS_DESTINATION`.
- Errors:
  - `Guid.Parse(@event.FtsSourceLocationsId)` throws if the string is not a valid GUID.
  - Any exception from `Update(...)`, destination update queries, or `SaveChanges()` propagates to the caller.
- Rules: F01-076, F01-077.

### AddDestinationAddress(List<FtsDestinationLocationAdded> destinationAddres, string userId)
- Purpose: Upsert destination rows based on an inbound list.
- Inputs:
  - `destinationAddres`: list of `FtsDestinationLocationAdded`.
  - `userId`: inbound user id; not used by insert/update logic.
- Logic:
  - Queries all existing destination ids from `contextBase.Set<FtsDestinationLocations>()` as `existingDestination` (not filtered by source location id).
  - For each `destAddress` in `destinationAddres`:
    - Looks up `existingDestination.Where(x => x.DestinationId == destAddress.DestinationId).FirstOrDefault()`.
    - If found, calls `UpdateDestination(destAddress, userId)`.
    - Otherwise, calls `AddDestination(destAddress, userId)`.
  - Calls `contextBase.SaveChanges()`.
- Data: reads/writes `orm:FTS_DESTINATION`.
- Errors:
  - A null `destinationAddres` throws during `foreach`.
  - Any exception from EF Core queries, insert/update logic, or `SaveChanges()` propagates to the caller.
- Rules: F01-078.

### AddDestination(FtsDestinationLocationAdded dest, string userId)
- Purpose: Insert one destination row linked to the current `ftsCommonSourceLocationId`.
- Inputs: `dest` (`FtsDestinationLocationAdded`); `userId` is not used.
- Logic:
  - Calls `contextBase.Set<FtsDestinationLocations>().Add(new FtsDestinationLocations { ... })`.
  - Inserts with:
    - `DestinationId = Guid.NewGuid()` (ignores `dest.DestinationId`).
    - `FtsSourceLocationId = ftsCommonSourceLocationId`.
    - Destination fields copied from `dest` including `DestinationAddress`, `DestinationProtocol`, destination flags, contact fields, `RenamePattern`, `FTEMetadataId`, `DestinationType`, `ContractName`.
    - `CreationDate = DateTime.UtcNow`, `LastModifiedDate = DateTime.UtcNow`.
    - `IsValid = dest.IsValid`.
- Data: writes `orm:FTS_DESTINATION`.
- Errors: Any exception from entity creation or EF Core `Add(...)` propagates.
- Rules: F01-052, F01-053, F01-078.

### UpdateDestination(FtsDestinationLocationAdded dest, string userId)
- Purpose: Update an existing destination row by id.
- Inputs: `dest` (`FtsDestinationLocationAdded`); `userId` is not used.
- Logic:
  - Loads one row with `DestinationId == dest.DestinationId`.
  - Updates fields from `dest` and sets `LastModifiedDate = DateTime.UtcNow`.
  - Does not check for a null result before field assignment (null result causes a null-reference exception).
- Data: reads/writes `orm:FTS_DESTINATION`.
- Errors:
  - If no destination exists for `dest.DestinationId`, dereferencing `destAddress` throws.
  - Any exception from EF Core queries or field updates propagates.
- Rules: F01-078.

## Rules implemented
| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-048 | New source location insert generates a new GUID id and sets creation/modified UTC timestamps. | `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationAdded)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-049 | New source location is always inserted with `IsValid=true` (ignores event `IsValid`). | `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationAdded)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-050 | Empty `FtpServerId` is stored as null; otherwise parsed as GUID. | `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationAdded)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-051 | When source location event is invalid, each destination insert forces `IsValid=false`. | `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationAdded)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-052 | Each inserted destination gets a new GUID id and links to the new source location id. | `FtsSourceLocationProjectionWriter.AddDestination` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-053 | Destination insert copies destination flags/contact fields and sets creation/modified UTC timestamps. | `FtsSourceLocationProjectionWriter.AddDestination` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-054 | After inserting source location and destinations, changes are persisted via `SaveChanges()`. | `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationAdded)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-073 | Source location update sets fields from event, sets `LastModifiedDate=UtcNow`, and stores validity/suspension/operator fields. | `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationUpdated)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-074 | Source location update calls `AddDestinationAddress(...)` when `FtsSourceLocationsId` is non-null (no null-check on destination list). | `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationUpdated)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-075 | When source location update sets `IsValid=false`, all linked destinations are marked invalid and store `OperatorId`. | `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationUpdated)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-076 | Source location invalidation marks the source location invalid and stores `OperatorId` (no last-modified update). | `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationInvalidated)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-077 | Source location invalidation optionally upserts destinations then invalidates all linked destinations and stores `OperatorId`. | `FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationInvalidated)` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-078 | Destination upsert checks inbound `DestinationId`: updates when exists, else inserts new destination with generated id linked to the source. | `FtsSourceLocationProjectionWriter.AddDestinationAddress` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F02-035 | FTP server add creates a new row with `CreationDate` and `LastModifiedDate` set to `UtcNow`. | `FtpServerProjectionWriter.AddFtpServer` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-036 | FTP server update updates fields and sets `LastModifiedDate=UtcNow` and stores `OperatorId`. | `FtpServerProjectionWriter.Handle(FtpServerUpdated)` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-037 | FTP server invalidation sets `IsValid`, stores `OperatorId`, and sets `LastModifiedDate=UtcNow`. | `FtpServerProjectionWriter.Handle(FtpServerInvalidated)` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F03-012 | Integration file update stores `ConfigId`, marks processed, stores active flag and operator id, and sets last-modified to `UtcNow`. | `FtsIntegrationFileProjectionWriter.Handle(IntegrationFileUpdated)` | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |
| F03-013 | Inactive integration file definitions no longer appear in later lookups (writer persists `IsValid` flag used by query filtering). | `FtsIntegrationFileProjectionWriter.Handle(IntegrationFileUpdated)` | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |

## Not carried over
- None.
