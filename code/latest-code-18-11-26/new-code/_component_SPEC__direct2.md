---
type: Behavior Specification
title: technical/components/backend-data-access-event-handlers-2/SPEC
description: 'What the backend-data-access-event-handlers-2 component does today: every class and method, and the business rules it implements.'
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
  body_sha256: dce2a37d7a52ee9c03da3c3f933e7a95fbede3396fde67b716b02ab83dd54a87
---

ï»¿# backend-data-access-event-handlers-2: projection writers for configuration read models

This component updates projection/read-model tables in response to configuration-related events.
It is called by the event-dispatch layer after configuration commands complete, and it relies on
EF Core persistence plus AutoMapper mapping for some event-to-entity transforms.

## FtpServerProjectionWriter
Writes FTP server read-model rows.

### Handle(FtpServerAdded @event)
- Purpose: Create a new `FTP_SERVER` projection row for the added FTP server.
- Inputs: `@event` (`FtpServerAdded`) including `FtpServerId`, server connection fields, `IsValid`, `OperatorId`.
- Logic:
  - Calls `AddFtpServer(@event)`.
- Data: writes `orm:FTP_SERVER`.
- Errors: Any exception from AutoMapper mapping, `Add(...)` persistence, or the underlying DB context propagates to the caller.
- Rules: None.

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
- Rules: None.

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
- Rules: None.

### AddFtpServer(FtpServerAdded @event)
- Purpose: Build and persist the new `FtpServer` entity used by `Handle(FtpServerAdded)`.
- Inputs: `@event` (`FtpServerAdded`).
- Logic:
  - Creates `new FtpServer()`.
  - Uses AutoMapper via `MapperWrapper.Mapper.Map(@event, newFtpServer)` to map event fields to entity fields.
  - Sets timestamps:
    - `CreationDate = DateTime.UtcNow`.
    - `LastModifiedDate = DateTime.UtcNow`.
  - Calls `Add(newFtpServer)` (from `ProjectionWriterBase`).
- Data: writes `orm:FTP_SERVER`.
- Errors: Any exception from AutoMapper mapping, `Add(...)`, or DB persistence propagates to the caller.
- Rules: None.

## FtsInboundTargetProjectionWriter
Writes inbound-target read-model rows.

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
    - `IsValid = false` (hard-coded; does not use the eventâ€™s `IsValid` field).
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
  - Uses AutoMapper via `MapperWrapper.Mapper.Map(ftsInboundTargetAdded, ftsInboundTarget)`.
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
Writes integration-file read-model rows.

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
- Rules: None.

### AddFtsIntegrationFile(IntegrationFileAdded integrationFileAdded)
- Purpose: Create and persist projection rows from the incoming group list.
- Inputs: `integrationFileAdded` (`IntegrationFileAdded`).
- Logic:
  - Iterates `integrationFileAdded.GroupInfo.Where(e => e.NumberOfRecords > 0)`.
  - For each qualifying `GroupInfo`, calls `MapFtsIntegrationFile(integrationFileAdded, groupInfo)` and then `Add(mappedEntity)`.
  - Groups with `NumberOfRecords <= 0` do not produce projection rows.
- Data: writes `orm:FTS_INTEGRATION_FILE`.
- Errors:
  - If `integrationFileAdded.GroupInfo` is `null`, the `.Where(...)` call throws a `NullReferenceException`.
  - Any exception from mapping or persistence propagates to the caller.
- Rules: None.

### MapFtsIntegrationFile(IntegrationFileAdded integrationFileAdded, GroupInfo groupInfo)
- Purpose: Build an `FtsIntegrationFile` entity instance for one group within an integration file.
- Inputs:
  - `integrationFileAdded` (`IntegrationFileAdded`).
  - `groupInfo` (`GroupInfo`) with `GroupId` and `NumberOfRecords`.
- Logic:
  - Creates `new FtsIntegrationFile()`.
  - Uses AutoMapper via `MapperWrapper.Mapper.Map(integrationFileAdded, ftsIntegrationFile)`.
  - Sets/overrides fields explicitly:
    - `FtsIntegrationFileId = Guid.NewGuid()` (ignores `integrationFileAdded.FtsIntegrationFileId`).
    - `GroupId = groupInfo.GroupId`.
    - `ConfigId = null` (hard-coded, ignoring `integrationFileAdded.ConfigId`).
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
Writes source-location and destination read-model rows.

### Handle(FtsSourceLocationAdded @event)
- Purpose: Create a new source location (legacy entity: `FtsSourceLocation`) and create its destination rows.
- Inputs: `@event` (`FtsSourceLocationAdded`) including source location fields and `DestinationAddress` list.
- Logic:
  - Sets a private field: `ftsCommonSourceLocationId = Guid.NewGuid()`.
  - Calls `Add(new FtsSourceLocation { ... })` (from `ProjectionWriterBase`) with:
    - `FtsSourceLocationId = ftsCommonSourceLocationId`.
    - `Configid = @event.ConfigId`.
    - `ConfigName = @event.ConfigName`.
    - `Description = @event.Description`.
    - `FtpServerId = null` when `@event.FtpServerId` is `null/empty`, else `Guid.Parse(@event.FtpServerId)`.
    - `SourceProtocalCode = @event.Protocol`.
    - `SourceAddress = @event.Address`.
    - `SourceFileMask = @event.Mask`.
    - `FileDirection = @event.FileDirection`.
    - `SendNotificationsIndicator = @event.SendNotifications`.
    - `IsValid = @event.IsValid`.
    - `IsSuspended = @event.IsSuspended`.
    - `IsSuspendedChangedDate = @event.IsSuspendedChangedDate`.
    - `OwnerName = @event.OwnerName`.
    - `OwnerModule = Guid.Parse(@event.OwnerModule)`.
    - `OwnerPhone = @event.OwnerPhone`.
    - `OwnerEmail = @event.OwnerEmail`.
    - Run schedule fields copied from event booleans/byte:
      - `RunOnMondayIndicator`, `RunOnTuesdayIndicator`, `RunOnWedIndicator`, `RunOnThursIndicator`, `RunOnFriIndicator`, `RunOnSatIndicator`, `RunOnSunIndicator`.
      - `RunOnFrstDayOfMonthIndicator`, `RunOnLastDayOfMonthIndicator`, `RunOnThisDayOfMonth`.
    - Window fields: `StartTime24HHMM = @event.StartTime`, `EndTime24HHMM = @event.EndTime`.
    - `TradingPartner = @event.TradingPartner`.
    - `CreationDate = DateTime.UtcNow`, `LastModifiedDate = DateTime.UtcNow`.
    - `OperatorId = @event.OperatorId`.
  - Iterates each `FtsDestinationLocationAdded dest` in `@event.DestinationAddress`:
    - If `@event.IsValid` is `false`, sets `dest.IsValid = false`.
    - Calls `AddDestination(dest, null)`.
  - Calls `contextBase.SaveChanges()`.
- Data: reads/writes `orm:FTS_DESTINATION` and reads/writes the source-location table.
- Errors:
  - `Guid.Parse(@event.FtpServerId)` and `Guid.Parse(@event.OwnerModule)` may throw `FormatException`.
  - If `@event.DestinationAddress` is `null`, the `foreach` throws a `NullReferenceException`.
  - Any DB exceptions from `SaveChanges()` propagate to the caller.
- Rules: None.

### Handle(FtsSourceLocationUpdated @event)
- Purpose: Update an existing source location row and upsert its destination rows.
- Inputs: `@event` (`FtsSourceLocationUpdated`) including `FtsSourceLocationsId` and updated source/destination fields.
- Logic:
  - Sets `ftsCommonSourceLocationId = new Guid(@event.FtsSourceLocationsId)`.
  - Calls `Update(Guid.Parse(@event.FtsSourceLocationsId), ...)`.
  - In the update callback, sets the same set of fields as in the add handler (with timestamps):
    - Updates `Configid`, `ConfigName`, `Description`, `FtpServerId`, `SourceProtocalCode`, `SourceAddress`, `SourceFileMask`, `FileDirection`, `SendNotificationsIndicator`, `IsSuspended`, `IsSuspendedChangedDate`, `IsValid`, owner and schedule fields, window fields, `TradingPartner`, `OperatorId`.
    - Sets `FtsSourceLocationId = ftsCommonSourceLocationId`.
    - Sets `LastModifiedDate = DateTime.UtcNow`.
    - Sets `IsValid` twice (same value each time).
  - If `@event.FtsSourceLocationsId.ToString() != null` (always true for a non-null string):
    - Calls `AddDestinationAddress(@event.DestinationAddress, @event.UserID)`.
    - If `@event.IsValid` is `false`:
      - Loads all destinations where `FtsSourceLocationId == ftsCommonSourceLocationId`.
      - For each loaded destination, sets `IsValid = false` and `OperatorId = @event.OperatorId`.
      - Calls `contextBase.SaveChanges()`.
- Data: reads/writes `orm:FTS_DESTINATION` and reads/writes the source-location table.
- Errors:
  - `new Guid(@event.FtsSourceLocationsId)` / `Guid.Parse(@event.FtsSourceLocationsId)` may throw `FormatException`.
  - If `@event.DestinationAddress` is `null`, `AddDestinationAddress` throws when it enumerates the list.
  - Any DB exceptions propagate.
- Rules: None.

### Handle(FtsSourceLocationInvalidated @event)
- Purpose: Invalidate a source location row and invalidate all of its destination rows.
- Inputs: `@event` (`FtsSourceLocationInvalidated`) including `FtsSourceLocationsId`, `OperatorId`, and optional `DestinationAddress`.
- Logic:
  - Sets `ftsCommonSourceLocationId = new Guid(@event.FtsSourceLocationsId)`.
  - Calls `Update(Guid.Parse(@event.FtsSourceLocationsId), ...)` and in the callback:
    - `OperatorId = @event.OperatorId`.
    - `IsValid = false`.
  - Parses `sourceLocationId = Guid.Parse(@event.FtsSourceLocationsId)`.
  - If `@event.DestinationAddress != null`, calls `AddDestinationAddress(@event.DestinationAddress, @event.UserID)`.
  - Loads all destinations where `FtsSourceLocationId == sourceLocationId` and for each:
    - sets `IsValid = false`.
    - sets `OperatorId = @event.OperatorId`.
  - Calls `contextBase.SaveChanges()`.
- Data: reads/writes `orm:FTS_DESTINATION` and reads/writes the source-location table.
- Errors:
  - `new Guid(...)` / `Guid.Parse(...)` may throw `FormatException`.
  - Any DB exceptions propagate.
- Rules: None.

### AddDestinationAddress(List<FtsDestinationLocationAdded> destinationAddres, string userId)
- Purpose: Upsert destination rows based on the incoming destination list.
- Inputs:
  - `destinationAddres`: a list of `FtsDestinationLocationAdded`.
  - `userId`: passed in by callers, but not used by this method or its callees.
- Logic:
  - Reads all existing destination ids: `existingDestination = from result in contextBase.Set<FtsDestinationLocations>() select new { DestinationId = result.DestinationId }`.
  - For each incoming `destAddress` in `destinationAddres`:
    - Checks whether `destAddress.DestinationId` exists in `existingDestination`.
    - If found, calls `UpdateDestination(destAddress, userId)`.
    - If not found, calls `AddDestination(destAddress, userId)`.
  - Calls `contextBase.SaveChanges()`.
- Data: reads/writes `orm:FTS_DESTINATION`.
- Errors:
  - If `destinationAddres` is `null`, the `foreach` throws a `NullReferenceException`.
  - Any DB exceptions propagate.
- Rules: None.

### AddDestination(FtsDestinationLocationAdded dest, string userId)
- Purpose: Add a new destination row linked to the current `ftsCommonSourceLocationId`.
- Inputs:
  - `dest` (`FtsDestinationLocationAdded`).
  - `userId`: passed in but not used.
- Logic:
  - Calls `contextBase.Set<FtsDestinationLocations>().Add(new FtsDestinationLocations { ... })` with:
    - `DestinationId = Guid.NewGuid()` (ignores `dest.DestinationId`).
    - `FtsSourceLocationId = ftsCommonSourceLocationId`.
    - `DestinationAddress = dest.DestinationAddress`.
    - `DestProtocolCD = dest.DestinationProtocol`.
    - `OverwriteAtDest = dest.OverWriteAtDestination`.
    - `CompressAtDest = dest.CompressAtDestination`.
    - `ReNameAtDest = dest.RenameAtDestination`.
    - `DestIsArchive = dest.DestinationArchive`.
    - `CreationDate = DateTime.UtcNow`, `LastModifiedDate = DateTime.UtcNow`.
    - `IsValid = dest.IsValid`.
    - `FtpServerId = dest.FtpServerId`.
    - `OperatorId = dest.OperatorId`.
    - `RenamePattern = dest.RenamePattern`.
    - `ContactName = dest.ContactName`, `ContactPhone = dest.ContactPhone`, `ContactEmail = dest.ContactEmail`.
    - `FTEMetadataId = dest.FTEMetadataId`.
    - `DestinationType = dest.DestinationType`.
    - `ContractName = dest.ContractName`.
- Data: writes `orm:FTS_DESTINATION`.
- Errors: Any DB exceptions propagate when the caller saves changes.
- Rules: None.

### UpdateDestination(FtsDestinationLocationAdded dest, string userId)
- Purpose: Update an existing destination row.
- Inputs:
  - `dest` (`FtsDestinationLocationAdded`).
  - `userId`: passed in but not used.
- Logic:
  - Loads one row: `destAddress = contextBase.Set<FtsDestinationLocations>().Where(x => x.DestinationId == dest.DestinationId).FirstOrDefault()`.
  - Updates these fields on the loaded row:
    - `DestinationAddress`, `DestProtocolCD`, `OverwriteAtDest`, `CompressAtDest`, `ReNameAtDest`, `DestIsArchive`.
    - `LastModifiedDate = DateTime.UtcNow`.
    - `IsValid`, `FtpServerId`, `OperatorId`.
    - `RenamePattern`, `ContactName`, `ContactPhone`, `ContactEmail`.
    - `FTEMetadataId`, `DestinationType`, `ContractName`.
- Data: reads/writes `orm:FTS_DESTINATION`.
- Errors:
  - If no row exists for `dest.DestinationId`, `destAddress` is `null` and the first field assignment throws a `NullReferenceException`.
  - Any DB exceptions propagate when the caller saves changes.
- Rules: None.

## Rules implemented
| Rule | In short | Where | Flow |
| --- | --- | --- | --- |

## Not carried over
- None.


