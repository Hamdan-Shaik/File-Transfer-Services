---
type: Behavior Specification
title: technical/components/shared-contracts-events/SPEC
description: 'What the shared-contracts-events component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-events
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:28:25Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-events/INVENTORY.yaml
  title: Every legacy class and method (15 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-events
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 15
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Events
  body_sha256: fbc4ebf175a389b8750fcb8bae7685acee2d2e4ac0b4b89ff9c74d2323071361
---

﻿# shared-contracts-events: legacy FXFER domain event DTOs

This component is the legacy set of event contract DTOs under `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events`.
These types are published/consumed by legacy CQRS/eventing infrastructure to represent file-transfer configuration changes and file transfer outcomes.

The inventory contains no business-logic methods; behavior is expressed as event payload shape (properties) and a small amount of inherited `Event.ModelType` identity.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FileTransferEvents
Base event type for File Transfer Services events.

- Properties:
  - `ModelType` (string): overridden to return `CommonConstants.ModuleName`.
  - `StartTime` (DateTime): event start timestamp.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FileTransferAdd
Event indicating a file transfer configuration/source-location was added (legacy naming uses `FtsSourceLocationsID`).

- Inherits: `FileTransferEvents`.
- Properties:
  - `FtsSourceLocationsID` (string): source location identifier.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FileTransferred
Event emitted when a file is transferred.

- Inherits: `FileTransferEvents`.
- Properties:
  - `GroupId` (Guid): group identifier.
  - `FTEMetadataId` (Guid): metadata identifier.
  - `FileId` (Guid): transferred file identifier.
  - `SourceLocationId` (Guid): source location identifier.
  - `FileName` (string): transferred file name.
  - `ConfigurationId` (long): sequential configuration identifier.
  - `ConfigurationName` (string): configuration name.
  - `SourceProtocolCode` (string): protocol code used for transfer.
  - `SourceAddressPath` (string): source path.
  - `DestinationAddressPath` (string): destination path.
  - `SourceMask` (string): mask used to select files.
  - `OwnerModule` (string): owner module.
  - `ModuleContract` (string): module contract.
  - `ConfigurationDescription` (string): configuration description.
  - `TransferDate` (DateTime): transfer timestamp.
  - `FileDirection` (string): direction (inbound/outbound).
  - `FileSize` (long): file size in bytes.
  - `FileType` (string): file type.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtpServerAdded
FTP server profile payload used to persist an FTP server record.

- Inherits: `FileTransferEvents`.
- Properties:
  - `FtpServerId` (Guid): internal identifier.
  - `FtpServerName` (string): unique name.
  - `FtpDescription` (string): description.
  - `FtpServerIp` (string): IP address.
  - `FtpTransferMode` (string): transfer mode (inbound/outbound; stored as 1-char code).
  - `FtpUser` (string): username.
  - `FtpPassword` (string): password (raw secret material in legacy contract).
  - `FtpPort` (short): port.
  - `FtpProxyAddress` (string): proxy address.
  - `FtpProxyPort` (short?): proxy port.
  - `UseSSHAuth` (bool): whether SSH authentication is used.
  - `IsValid` (bool): validity flag.
  - `OperatorId` (string): operator identifier.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtpServerUpdated
FTP server update event.

- Inherits: `FtpServerAdded`.
- Properties: none (payload is identical to `FtpServerAdded`).

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtpServerInvalidated
FTP server invalidation event.

- Inherits: `FtpServerUpdated`.
- Properties: none (payload is identical to `FtpServerAdded`).

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsSourceLocationEvents
Base event type for FTS source location-related events.

- Properties:
  - `ModelType` (string): overridden to return `"FileTransfer"` (and marked `[IgnoreDataMember]`).
  - `FtsSourceLocationsId` (string): source location identifier.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsSourceLocationAdded
Source location configuration creation event (includes destination list).

- Inherits: `FtsSourceLocationEvents`.
- Properties:
  - `ConfigId` (int)
  - `ConfigName` (string)
  - `Description` (string)
  - `FtpServerId` (string)
  - `Address` (string)
  - `Mask` (string)
  - `Protocol` (string)
  - `FileDirection` (string)
  - `OwnerEmail` (string)
  - `OwnerName` (string)
  - `OwnerPhone` (string)
  - `OwnerModule` (string)
  - `TradingPartner` (string)
  - `OperatorId` (string)
  - Schedule flags (bool): `RunOnMonday`, `RunOnTuesday`, `RunOnWed`, `RunOnThrus`, `RunOnFri`, `RunOnSat`, `RunOnSun`, `RunOnFrstDayOfMonthInd`, `RunOnLastDayOfMonthInd`
  - `RunOnThisDayOfMonth` (byte)
  - `StartTime` (string): run start time.
  - `EndTime` (string): run end time.
  - `IsValid` (bool)
  - `IsSuspended` (bool)
  - `IsSuspendedChangedDate` (DateTime?)
  - `SendNotifications` (bool)
  - `LastModifiedDate` (DateTime)
  - `CreationDate` (DateTime)
  - `DestinationAddress` (List<FtsDestinationLocationAdded>): destination entries.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsSourceLocationUpdated
Source location configuration update event (includes destination list).

- Inherits: `FtsSourceLocationEvents`.
- Properties: same payload shape as `FtsSourceLocationAdded`.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsSourceLocationInvalidated
Source location invalidation event.

- Inherits: `FtsSourceLocationEvents`.
- Properties:
  - `ConfigId` (long)
  - `OperatorId` (string)
  - `DestinationAddress` (List<FtsDestinationLocationAdded>)

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsDestinationLocationAdded
Destination location event (destination entry under a source location configuration).

- Inherits: `FtsSourceLocationEvents`.
- Properties:
  - `DestinationId` (Guid)
  - `SourceLocationId` (Guid)
  - `DestinationAddress` (string)
  - `DestinationProtocol` (string)
  - `FtpServerId` (Guid)
  - `RenamePattern` (string)
  - `OverWriteAtDestination` (bool)
  - `CompressAtDestination` (bool)
  - `RenameAtDestination` (bool)
  - `DestinationArchive` (bool)
  - `ContactName` (string)
  - `ContactPhone` (string)
  - `ContactEmail` (string)
  - `IsValid` (bool)
  - `LastModifiedDate` (DateTime)
  - `CreationDate` (DateTime)
  - `OperatorId` (string)
  - `FTEMetadataId` (Guid?)
  - `DestinationType` (string)
  - `ContractName` (string)
  - `ImportJobName` (string)

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsInboundTargetAdded
Inbound target creation event.

- Inherits: `FileTransferEvents`.
- Properties:
  - `FtsInboundTargetId` (Guid)
  - `FTEMetadataId` (Guid)
  - `ImportJobName` (string)
  - `ContractName` (string)
  - `OperatorId` (string)
  - `IsValid` (bool)

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsInboundTargetInvalidated
Inbound target invalidation event.

- Inherits: `FtsInboundTargetAdded`.
- Properties:
  - `LastModifiedDate` (DateTime)

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.IntegrationFileAdded
Integration file creation event.

- Inherits: `FileTransferEvents`.
- Properties:
  - `FtsIntegrationFileId` (Guid)
  - `GroupInfo` (List<GroupInfo>)
  - `ConfigId` (int?)
  - `FileName` (string)
  - `ETFOutboundMetadataId` (Guid)
  - `IsProcessed` (bool)
  - `OperatorId` (string)
  - `IsValid` (bool)

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.IntegrationFileUpdated
Integration file update event.

- Inherits: `IntegrationFileAdded`.
- Properties:
  - `LastModifiedDate` (DateTime)

## Rules implemented

None.

## Target-state requirements

### T-006 Publish tenant-aware, schema-versioned domain events using the canonical event envelope (tenant_id, correlation_id, trace_id, producer, etc.).
- Purpose: Publish FXFER domain events that can be routed, audited, and traced end-to-end without inspecting event payloads.
- Inputs:
  - Tenant context providing `tenant_id` (derived from verified identity context).
  - Correlation context: `correlation_id` and W3C `trace_id`.
  - Event identity: `event_name`, `event_version`, `event_id`, and `occurred_at`.
  - Producer identity: `producer`.
- Logic:
  - Every published domain event carries the canonical envelope fields.
  - Canonical envelope values are also published as message headers/attributes so routing and observability do not require payload inspection.
  - Event schemas are versioned using `event_version` (e.g., `v1`) and payload shapes are treated as immutable per version.
  - Event payloads follow the data minimization posture: no secrets and no raw file payload bytes.
- Data:
  - Events are emitted from the FXFER domain boundary and published via the deployable’s outbox-to-event-adapter flow.
- Errors:
  - Publish failures must not lose events; events remain pending until successfully published by the outbox publisher.
- Target-state section: `inputs/target-state.md` section `7.2 Event Types`.

## Not carried over

None.
