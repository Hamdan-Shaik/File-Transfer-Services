---
type: Behavior Specification
title: 'shared-contracts-domain: legacy FXFER domain contract models'
description: 'What the shared-contracts-domain component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-domain
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:10:36Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-domain/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-domain
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Domain
  body_sha256: f7391529d38bde5a832b30af2c215c4afae9d1c8128d54f19f5d0bd0803a7faa
---

# shared-contracts-domain: legacy FXFER domain contract models

This component is the legacy set of WCF `DataContract`/`DataMember` DTOs used to represent file-transfer configuration and tracking data.
It is consumed by legacy query/command service layers to serialize and return domain state; it contains no business logic methods.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.FileEventModel
File-event DTO returned as part of file tracking detail.

- Properties:
  - `EventId` (Guid): event record identifier.
  - `FileId` (Guid): file identifier the event is associated with.
  - `DestinationId` (Guid): destination identifier the event relates to.
  - `EventCode` (string): event code.
  - `EventTrackDescription` (string): event description text.
  - `CreatedDate` (DateTime): event creation timestamp.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.FileExceptionModel
File-exception DTO returned as part of file tracking detail.

- Properties:
  - `ExceptionId` (Guid): exception record identifier.
  - `FileId` (Guid): file identifier the exception is associated with.
  - `DestinationId` (Guid): destination identifier the exception relates to.
  - `EventId` (Guid): event identifier the exception is associated with.
  - `ExceptionType` (string): exception class/type name.
  - `MethodName` (string): method name where the exception was captured.
  - `Message` (string): exception message.
  - `StackTrace` (string): exception stack trace.
  - `CreatedDate` (DateTime): exception creation timestamp.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.FileTransferTrackingModel
Tracking DTO containing header-level file tracking data plus event and exception lists.

- Properties:
  - `FileId` (Guid): unique identifier of the file being tracked.
  - `FileSourceLocationId` (Guid): source location identifier.
  - `FileName` (string): file name.
  - `FileStatus` (string): file status.
  - `FileDirection` (string): file transfer direction (e.g., inbound/outbound).
  - `FileTransportType` (string): protocol used to transfer the file.
  - `CreatedDate` (DateTime): when the file/tracking record was created.
  - `TradingPartner` (string): trading partner associated with the file.
  - `CheckSumCode` (string): checksum code.
  - `FileSize` (decimal): file size.
  - `SourceLocAddress` (string): source location address.
  - `SourceLocConfigName` (string): source location configuration name.
  - `OriginatingServerName` (string): originating server name.
  - `Events` (List<FileEventModel>): associated tracking events.
  - `Exceptions` (List<FileExceptionModel>): associated exceptions.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.FtpServerModel
FTP server profile DTO.

- Properties:
  - `FtpServerId` (Guid, required; max length 36): internal identifier.
  - `FtpServerName` (string, required; max length 50): unique name.
  - `FtpServerIp` (string, required; max length 15): server IP address.
  - `FtpTransferMode` (string, required; max length 1): transfer direction (inbound/outbound).
  - `FtpUser` (string, required; max length 50): username.
  - `FtpPassword` (string, required; max length 50): password.
  - `FtpDescription` (string, required; max length 100): description.
  - `FtpPort` (short, required; max length 5): server port.
  - `FtpProxyAddress` (string, optional; max length 200): proxy address.
  - `FtpProxyPort` (short?, optional; max length 5): proxy port.
  - `UseSSHAuth` (bool, required): whether SSH authentication is used.
  - `IsValid` (bool, required): whether the profile is valid.
  - `LastModifiedDate` (DateTime): last modified timestamp.
  - `CreationDate` (DateTime): creation timestamp.
  - `OperatorId` (string, required; max length 200): operator identifier who inserted/updated.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.FtsInboundTargetModel
Inbound target metadata DTO.

- Properties:
  - `FtsInboundTargetId` (Guid, required): inbound target identifier.
  - `FTEMetadataId` (Guid, required): metadata identifier.
  - `ImportJobName` (string, required; max length 200): import job name.
  - `ContractName` (string, required; max length 200): contract name.
  - `IsValid` (bool, required): validity flag.
  - `CreationDate` (DateTime, required): created timestamp.
  - `LastModifiedDate` (DateTime, required): last modified timestamp.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.FtsIntegrationFileModel
Integration file definition DTO.

- Properties:
  - `FtsIntegrationFileId` (Guid, required): integration file identifier.
  - `GroupId` (Guid?, required): group identifier.
  - `ConfigId` (int?, optional; max length 5): configuration id.
  - `FileName` (string, required; max length 500): integration file name.
  - `ETFOutboundMetadataDefnId` (Guid, required): outbound metadata definition identifier.
  - `IsProcessed` (bool, required): processed flag.
  - `IsValid` (bool, required): validity flag.
  - `CreationDate` (DateTime, required): created timestamp.
  - `LastModifiedDate` (DateTime, required): last modified timestamp.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.GroupInfo
Group information DTO.

- Properties:
  - `GroupId` (Guid?, max length 36): system-generated group identifier.
  - `NumberOfRecords` (int): number of outbound records in extract file.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.SourceLocationModel
Source location (transfer configuration) DTO.

- Properties:
  - `FtsSourceLocationId` (Guid): source location identifier.
  - `ConfigId` (int): configuration id.
  - `ConfigName` (string): configuration name.
  - `Description` (string): configuration description.
  - `FtpServerId` (Guid?): FTP server id.
  - `FtpServerName` (string): FTP server name.
  - `Address` (string): source location address.
  - `OwnerName` (string): owner name.
  - `OwnerEmail` (string): owner email.
  - `OwnerSubsystem` (Guid): owner subsystem id.
  - `OwnerPhoneNumber` (string): owner phone number.
  - `Protocol` (string): source location protocol.
  - `FileDirection` (string): file direction.
  - `Mask` (string): file mask.
  - `TradingPartner` (string): trading partner.
  - `OperatorId` (string): operator id.
  - `RunOnMonday`/`RunOnTuesday`/`RunOnWednesday`/`RunOnThursday`/`RunOnFriday`/`RunOnSaturday`/`RunOnSunday` (bool): day-of-week run flags.
  - `RunOnFirstDay`/`RunOnLastDay` (bool): first/last day of month flags.
  - `RunOnThisDay` (byte): specific day-of-month value.
  - `StartWindow` (string): start time window.
  - `EndWindow` (string): end time window.
  - `SendNotification` (bool): notification flag.
  - `IsActive` (bool): active flag.
  - `ValidChangeDate` (DateTime): validity change timestamp.
  - `Suspended` (bool): suspended flag.
  - `SuspendedChangeDate` (DateTime): suspended change timestamp.
  - `LastModifiedDate` (DateTime): last modified timestamp.
  - `CreationDate` (DateTime): created timestamp.
  - `DestinationAddress` (List<FtsDestinationLocationAdded>): destination address list.

## Rules implemented

None.

## Not carried over

None.

