---
type: Behavior Specification
title: 'backend-business-aggregates: aggregate state + event-to-command mapping profiles'
description: 'What the backend-business-aggregates component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-aggregates
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:26:50Z
sources:
- id: inventory
  resource: /technical/components/backend-business-aggregates/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-aggregates
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 8
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain
  body_sha256: c31120244c74b157bc683fbd8007ec30a0fab1fa09cd29b4c8a9af7ef82d039d
---

# backend-business-aggregates: aggregate state + event-to-command mapping profiles

This component contains a File Transfer aggregate-state type and AutoMapper profiles that translate inbound integration events into File Transfer command DTOs.

## FtsSourceLocationState
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.Aggregates.FtsSourceLocationState`

Aggregate state object representing the current values of a File Transfer Source Location configuration record.

## CommandDispatcher
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandDispatcher`

Concrete type inheriting legacy `CommandDispatcherBase` with no additional behavior.

## QueryDispatcher
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.QueryDispatcher`

Concrete type inheriting legacy `QueryDispatcherBase` with no additional behavior.

## InboundTargetProfile
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.Profiles.InboundTargetProfile`

AutoMapper profile mapping Events Distributor inbound-target events into commands.

### Configure()
- Purpose: register inbound-target event-to-command mappings.
- Inputs: none.
- Logic:
  - Calls `MapFTEMetadataUpdatedEvent()`.
  - Calls `MapFTEMetadataInvalidatedEvent()`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

### MapFTEMetadataUpdatedEvent()
- Purpose: register mapping `FTEMetadataUpdated` → `AddFtsInboundTargetCommand`.
- Inputs: none (registers mapping definitions).
- Logic:
  - Maps fields:
    - `Requestor.IdentifierId` ← `src.UserID`
    - `ContractName` ← `src.ModuleName`
    - `ImportJobName` ← `src.ImportJobName`
    - `FTEMetadataId` ← `src.FTEMetadataId`
  - Applies a per-member condition: map only when the source value is not null (`!IsSourceValueNull`).
  - Registers polymorphic mapping: `FTEMetadataUpdated` → base `Command` as `AddFtsInboundTargetCommand`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

### MapFTEMetadataInvalidatedEvent()
- Purpose: register mapping `FTEMetadataInvalidated` → `InvalidateFtsInboundTargetCommand`.
- Inputs: none (registers mapping definitions).
- Logic:
  - Maps fields:
    - `Requestor.IdentifierId` ← `src.UserID`
    - `FTEMetadataId` ← `src.FTEMetadataId`
  - Applies a per-member condition: map only when the source value is not null (`!IsSourceValueNull`).
  - Registers polymorphic mapping: `FTEMetadataInvalidated` → base `Command` as `InvalidateFtsInboundTargetCommand`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

## IntegrationFileProfile
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.Profiles.IntegrationFileProfile`

AutoMapper profile mapping Event-To-File and Events Distributor events into `AddIntegrationFileCommand`.

### Configure()
- Purpose: register integration-file event-to-command mappings.
- Inputs: none.
- Logic:
  - Calls `MapExtractFileGeneratedEvent()`.
  - Calls `MapSummaryReportGeneratedEvent()`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

### MapExtractFileGeneratedEvent()
- Purpose: register mapping `ExtractFileGenerated` → `AddIntegrationFileCommand`.
- Inputs: none (registers mapping definitions).
- Logic:
  - Maps fields:
    - `Requestor.IdentifierId` ← `src.UserID`
    - `ETFOutboundMetadataDefnId` ← `src.MetadataId`
    - `FileName` ← `src.FileDirectory`
    - `GroupInfo` ← `SetGroupInfo(src)`
  - Applies a per-member condition: map only when the source value is not null (`!IsSourceValueNull`).
  - Registers polymorphic mapping: `ExtractFileGenerated` → base `Command` as `AddIntegrationFileCommand`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

### MapSummaryReportGeneratedEvent()
- Purpose: register mapping `SummaryReportGenerated` → `AddIntegrationFileCommand`.
- Inputs: none (registers mapping definitions).
- Logic:
  - Maps fields:
    - `Requestor.IdentifierId` ← `src.UserID`
    - `ETFOutboundMetadataDefnId` ← `src.FTEMetadataId`
    - `FileName` ← `src.FileName`
    - `GroupInfo` ← `SetGroupInfo(src)`
  - Applies a per-member condition: map only when the source value is not null (`!IsSourceValueNull`).
  - Registers polymorphic mapping: `SummaryReportGenerated` → base `Command` as `AddIntegrationFileCommand`.
- Data: none.
- Errors: AutoMapper configuration exceptions surface during mapper initialization.
- Rules: none.

### SetGroupInfo(ExtractFileGenerated extractFileGenerated)
- Purpose: convert `extractFileGenerated.GroupInfo` entries into `List<GroupInfo>`.
- Inputs: extractFileGenerated.
- Logic: for each `grpInfo` in `extractFileGenerated.GroupInfo`, append `new GroupInfo { GroupId = grpInfo.GroupId, NumberOfRecords = grpInfo.NumberOfRecords }`.
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

- `CommandDispatcher`: target command routing does not use legacy `CommandDispatcherBase` subclasses.
- `QueryDispatcher`: target query routing does not use legacy `QueryDispatcherBase` subclasses.

