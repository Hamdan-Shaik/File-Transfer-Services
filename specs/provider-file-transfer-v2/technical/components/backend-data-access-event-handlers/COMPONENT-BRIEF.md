---
type: Generation Brief
title: 'Brief: component backend-data-access-event-handlers'
description: Instructions the tool gave the writer session for the backend-data-access-event-handlers component.
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
  body_sha256: 254415b47e4235a9d3cb8a42b3cd25098e802edd9b2056fa3dbfa69615fe482f
---

# Brief: component `backend-data-access-event-handlers`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers (backend tier, 4 classes, 18 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-data-access-event-handlers/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-data-access-event-handlers/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-projection-worker` (`workers/fxfer-projection-worker`), folder `src/Infrastructure/Projections`: write `workers/fxfer-projection-worker/src/Infrastructure/Projections` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-projection-worker/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

- `backend-data-access-event-handlers-2`: `technical/components/backend-data-access-event-handlers-2/`

## Flows it takes part in

- `configure-source-locations-and-destinations`: `technical/flows/configure-source-locations-and-destinations/` (FLOW.md, RULES.md) and its scenarios `business/configure-source-locations-and-destinations.feature`
- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`

## Business rules it owns (18)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-048** When projecting a newly added source location, the writer creates a new GUID id and inserts a `FtsSourceLocation` row with creation/modified timestamps set to `DateTime.UtcNow`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36-74
- Legacy: Sets `ftsCommonSourceLocationId = Guid.NewGuid()` and calls `Add(new FtsSourceLocation { FtsSourceLocationId = ftsCommonSourceLocationId, ..., CreationDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow })`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-049** When projecting a newly added source location, the source location is always stored with `IsValid = true` (regardless of the event `IsValid`).
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:39-53
- Legacy: The inserted `FtsSourceLocation` entity sets `IsValid = true`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-050** When projecting a newly added source location, an empty `FtpServerId` in the event is stored as null; otherwise it is parsed as a GUID.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:45-47
- Legacy: `FtpServerId = string.IsNullOrEmpty(@event.FtpServerId) ? (Guid?)null : Guid.Parse(@event.FtpServerId)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-051** When projecting destinations, if the event is marked invalid (`IsValid == false`), each destination is forced to `IsValid = false` before insert.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:76-84
- Legacy: Before calling `AddDestination(dest, ...)`, checks `if (!@event.IsValid) { dest.IsValid = false; }`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-052** Each destination inserted for a newly added source location gets a new GUID destination id and is linked to the new source location id.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191-199
- Legacy: Inserts `new FtsDestinationLocations { DestinationId = Guid.NewGuid(), FtsSourceLocationId = this.ftsCommonSourceLocationId, ... }`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-053** When inserting a destination, the destination row copies the destination flags and contact fields and sets creation/modified timestamps to `DateTime.UtcNow`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:197-215
- Legacy: Writes destination fields including `OverwriteAtDest`, `CompressAtDest`, `ReNameAtDest`, `DestIsArchive`, `RenamePattern`, `ContactName/Phone/Email`, and timestamps `CreationDate`/`LastModifiedDate` as UTC now.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-054** After inserting the source location and all destinations, the projection writer persists the changes with `SaveChanges()`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:76-87
- Legacy: Calls `contextBase.SaveChanges()` after adding the source location and iterating destinations.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-073** When applying a `FtsSourceLocationUpdated` event, the flow updates the persisted source location identified by `FtsSourceLocationsId` and sets its fields directly from the event, including `IsSuspended`, `IsSuspendedChangedDate`, `IsValid`, and `OperatorId`, and updates `LastModifiedDate` to the current UTC timestamp.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89-132, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89-132, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:100, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:110, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:126
- Legacy: `FtpServerId` is stored as null when the event's `FtpServerId` is empty; otherwise the code parses it as a GUID.
- ⚑ Flag: `OwnerModule` and non-empty `FtpServerId` are parsed using `Guid.Parse(...)` and will throw if the event contains a non-GUID string.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-074** When applying a `FtsSourceLocationUpdated` event, the flow attempts to upsert destination locations by calling `AddDestinationAddress(@event.DestinationAddress, @event.UserID)` whenever `FtsSourceLocationsId` is non-null.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:129-138, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:129-138, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:133, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165-188
- Legacy: The update path does not check whether `DestinationAddress` is null before calling `AddDestinationAddress(...)`.
- ⚑ Flag: A null `DestinationAddress` list will raise an exception during `foreach` in `AddDestinationAddress(...)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-075** When applying a `FtsSourceLocationUpdated` event with `IsValid == false`, the flow marks every destination location row linked to the source location as invalid (`IsValid = false`) and sets each row's `OperatorId` to the event's `OperatorId`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:134-137, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:134-137, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:136
- Legacy: The flow saves changes after updating destination validity.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-076** When applying a `FtsSourceLocationInvalidated` event, the flow marks the source location identified by `FtsSourceLocationsId` as invalid (`IsValid = false`) and stores `OperatorId` from the event.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:143-156, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:143-156
- Legacy: The invalidation projection does not update `LastModifiedDate`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-077** When applying a `FtsSourceLocationInvalidated` event, if `DestinationAddress` is provided the flow upserts destination locations from that list and then marks every destination location row linked to the source location as invalid (`IsValid = false`) and sets each row's `OperatorId` to the event's `OperatorId`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:154-163, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:154-163, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:158-162, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:161
- Legacy: Any destination rows upserted in this step are immediately marked invalid in the same handler.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-078** When upserting destination locations (`AddDestinationAddress(...)`), the flow checks each inbound destination's `DestinationId`: if it matches an existing row the flow updates that row's fields; otherwise it inserts a new row with a freshly generated `DestinationId` and links it to the current source location id.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-controller-update-file-transfer-source-location
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165-188, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191-217, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:195, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:219-241, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:229
- Legacy: The insert path ignores any inbound `DestinationId` value and always generates a new GUID.
- ⚑ Flag: Callers cannot request a specific `DestinationId` for a new destination; the system always assigns one.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-035** When a `FtpServerAdded` event is handled, a new FTP server row is created with `CreationDate` and `LastModifiedDate` set to `DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74-82, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74-82
- Legacy: The projection writer maps the event to a new `FtpServer` entity, sets both timestamps to `DateTime.UtcNow`, and calls `Add(newFtpServer)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-036** When a `FtpServerUpdated` event is handled, the existing FTP server row is updated by id and `LastModifiedDate` is set to `DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:39-57, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:39-57
- Legacy: The projection writer updates all FTP server fields from the event (including credentials and proxy fields), sets `LastModifiedDate = DateTime.UtcNow`, and sets `OperatorId = @event.OperatorId`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-037** When a `FtpServerInvalidated` event is handled, the existing FTP server row is updated by id by setting `IsValid` (to false), `OperatorId`, and `LastModifiedDate = DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-invalidate-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:60-69, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:60-69
- Legacy: The projection writer sets `IsValid = @event.IsValid`, sets `OperatorId = @event.OperatorId`, and sets `LastModifiedDate = DateTime.UtcNow`.
- Decision: keep

From `technical/flows/manage-integration-files/RULES.md`:

**F03-012** Applying an integration file update records the configuration id, marks the definition processed, stores the active flag, stores the operator id, and sets last-modified time to the current UTC time.
- Doors: file-transfer-svc-service-update-integration-file, file-transfer-svc-service-update-integration-file-2
- Source: PROJ-UA3:40-52
- Legacy: The projection update sets `ConfigId`, sets `IsProcessed = true`, sets `IsValid`, sets `LastModifiedDate = DateTime.UtcNow`, and sets `OperatorId`.
- Decision: keep
- ⚑ Flag: the update request includes a file name, but this projection does not update the stored file name

From `technical/flows/manage-integration-files/RULES.md`:

**F03-013** When an update marks a definition inactive, that definition no longer appears in later lookups.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2, file-transfer-svc-service-update-integration-file, file-transfer-svc-service-update-integration-file-2
- Source: PROJ-UA3:40-52, QEX-UA3:31-35
- Legacy: The update stores the active/inactive flag, and the lookup returns only definitions where `IsValid` is true.
- Decision: keep

