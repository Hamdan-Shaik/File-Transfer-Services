---
type: Behavior Specification
title: 'backend-business-profiles: AutoMapper profiles for commands, events, and projection entities'
description: 'What the backend-business-profiles component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-profiles
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:23:32Z
sources:
- id: inventory
  resource: /technical/components/backend-business-profiles/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-profiles
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 6
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain/Profiles
  body_sha256: 6bd8da87bf3010e714491c2a2b29877ef51f864e3d335a31e01f209e7a64146d
---

# backend-business-profiles: AutoMapper profiles for commands, events, and projection entities

This component contains AutoMapper `Profile` classes used by the legacy BAS FileTransferSvc to translate between command DTOs, integration/domain events, and projection-writer persistence entities.

## FtpServerCommandHandlerProfile
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.Profiles.FtpServerCommandHandlerProfile`

AutoMapper profile mapping FTP server commands into FTP server domain events.

### FtpServerCommandHandlerProfile()
- Purpose: register command-to-event mappings used by the FTP server command handler.
- Inputs: none.
- Logic:
  - Registers mapping `AddFtpServerCommand` -> `FtpServerAdded`.
  - Registers mapping `UpdateFtpServerCommand` -> `FtpServerUpdated`.
  - For both mappings, applies a per-member condition: map only when the source member value is not null (`srs != null`).
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

## FtpServerProjectionWriterProfile
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.Profiles.FtpServerProjectionWriterProfile`

AutoMapper profile mapping FTP server events into the FTP server projection entity.

### FtpServerProjectionWriterProfile()
- Purpose: register event-to-entity mappings used by the FTP server projection writer.
- Inputs: none.
- Logic:
  - Registers mapping `FtpServerAdded` -> `FtpServer`.
  - Applies a per-member condition: map only when the source member value is not null (`srs != null`).
- Data: none (mapping only; persistence is performed by projection-writer code).
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

## FtsInboundTargetProjectionWriterProfile
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.Profiles.FtsInboundTargetProjectionWriterProfile`

AutoMapper profile mapping inbound-target events into the inbound-target projection entity.

### FtsInboundTargetProjectionWriterProfile()
- Purpose: register event-to-entity mappings used by the inbound-target projection writer.
- Inputs: none.
- Logic:
  - Registers mapping `FtsInboundTargetAdded` -> `FtsInboundTarget`.
  - Applies a per-member condition: map only when the source member value is not null (`srs != null`).
- Data: none (mapping only; persistence is performed by projection-writer code).
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

## FtsIntegrationFileProjectionWriterProfile
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.Profiles.FtsIntegrationFileProjectionWriterProfile`

AutoMapper profile mapping integration-file events into the integration-file projection entity.

### FtsIntegrationFileProjectionWriterProfile()
- Purpose: register event-to-entity mappings used by the integration-file projection writer.
- Inputs: none.
- Logic:
  - Registers mapping `IntegrationFileAdded` -> `FtsIntegrationFile`.
  - Applies a per-member condition: map only when the source member value is not null (`srs != null`).
- Data: none (mapping only; persistence is performed by projection-writer code).
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

## InboundTargetProfile
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.Profiles.InboundTargetProfile`

AutoMapper profile mapping Events Distributor inbound-target metadata events into FXFER commands.

### InboundTargetProfile()
- Purpose: register inbound-target event-to-command mappings.
- Inputs: none.
- Logic:
  - Calls `MapFTEMetadataUpdatedEvent()`.
  - Calls `MapFTEMetadataInvalidatedEvent()`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

### MapFTEMetadataUpdatedEvent()
- Purpose: register mapping `FTEMetadataUpdated` -> `AddFtsInboundTargetCommand`.
- Inputs: none (registers mapping definitions).
- Logic:
  - Maps fields:
    - `Requestor.IdentifierId` <- `src.UserID` (via `new RequestorModel { IdentifierId = src.UserID }`).
    - `ContractName` <- `src.ModuleName`.
    - `ImportJobName` <- `src.ImportJobName`.
    - `FTEMetadataId` <- `src.FTEMetadataId`.
  - Applies a per-member condition: map only when the source member value is not null (`srs != null`).
  - Registers polymorphic mapping: `FTEMetadataUpdated` -> base `Command` as `AddFtsInboundTargetCommand`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

### MapFTEMetadataInvalidatedEvent()
- Purpose: register mapping `FTEMetadataInvalidated` -> `InvalidateFtsInboundTargetCommand`.
- Inputs: none (registers mapping definitions).
- Logic:
  - Maps fields:
    - `Requestor.IdentifierId` <- `src.UserID` (via `new RequestorModel { IdentifierId = src.UserID }`).
    - `FTEMetadataId` <- `src.FTEMetadataId`.
  - Applies a per-member condition: map only when the source member value is not null (`srs != null`).
  - Registers polymorphic mapping: `FTEMetadataInvalidated` -> base `Command` as `InvalidateFtsInboundTargetCommand`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

## IntegrationFileProfile
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.Profiles.IntegrationFileProfile`

AutoMapper profile mapping Event-To-File and Events Distributor events into `AddIntegrationFileCommand`.

### IntegrationFileProfile()
- Purpose: register integration-file event-to-command mappings.
- Inputs: none.
- Logic:
  - Calls `MapExtractFileGeneratedEvent()`.
  - Calls `MapSummaryReportGeneratedEvent()`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

### MapExtractFileGeneratedEvent()
- Purpose: register mapping `ExtractFileGenerated` -> `AddIntegrationFileCommand`.
- Inputs: none (registers mapping definitions).
- Logic:
  - Maps fields:
    - `Requestor.IdentifierId` <- `src.UserID` (via `new RequestorModel { IdentifierId = src.UserID }`).
    - `ETFOutboundMetadataDefnId` <- `src.MetadataId`.
    - `FileName` <- `src.FileDirectory`.
    - `GroupInfo` <- `SetGroupInfo(src)`.
  - Applies a per-member condition: map only when the source member value is not null (`srs != null`).
  - Registers polymorphic mapping: `ExtractFileGenerated` -> base `Command` as `AddIntegrationFileCommand`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

### MapSummaryReportGeneratedEvent()
- Purpose: register mapping `SummaryReportGenerated` -> `AddIntegrationFileCommand`.
- Inputs: none (registers mapping definitions).
- Logic:
  - Maps fields:
    - `Requestor.IdentifierId` <- `src.UserID` (via `new RequestorModel { IdentifierId = src.UserID }`).
    - `ETFOutboundMetadataDefnId` <- `src.FTEMetadataId`.
    - `FileName` <- `src.FileName`.
    - `GroupInfo` <- `SetGroupInfo(src)`.
  - Applies a per-member condition: map only when the source member value is not null (`srs != null`).
  - Registers polymorphic mapping: `SummaryReportGenerated` -> base `Command` as `AddIntegrationFileCommand`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

### SetGroupInfo(ExtractFileGenerated extractFileGenerated)
- Purpose: convert `extractFileGenerated.GroupInfo` entries into a `List<GroupInfo>`.
- Inputs: extractFileGenerated.
- Logic:
  - Create an empty `List<GroupInfo>`.
  - For each `grpInfo` in `extractFileGenerated.GroupInfo`, append `new GroupInfo { GroupId = grpInfo.GroupId, NumberOfRecords = grpInfo.NumberOfRecords }`.
- Data: none.
- Errors: throws `NullReferenceException` when `extractFileGenerated.GroupInfo` is null (foreach over null).
- Rules: none.

### SetGroupInfo(SummaryReportGenerated summaryReportGenerated)
- Purpose: create a single-item `GroupInfo` list for the summary-report event.
- Inputs: summaryReportGenerated.
- Logic: returns `[new GroupInfo { GroupId = summaryReportGenerated.GroupId, NumberOfRecords = summaryReportGenerated.NumberOfRecords }]`.
- Data: none.
- Errors: none.
- Rules: none.

## Rules implemented

None.

## Not carried over

None.

