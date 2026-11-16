---
type: Target Placement
title: 'Target: backend-business-aggregates'
description: Where the backend-business-aggregates component goes in the new system and what to build, method by method.
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
  body_sha256: 11279f6e9a8aed7ebc7ab4635e04f4f8cd958fc382c2c37ccef6dcba9e3775b0
---

# Target: backend-business-aggregates

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Domain`

## What to build

### Domain aggregate state

Create:

- `services/fxfer-domain-api/src/Domain/Aggregates/FtsSourceLocationState.cs`
  - Type: `public sealed class FtsSourceLocationState`
  - Purpose: represent the current values of a File Transfer Source Location configuration record.
  - Constructors:
    - `FtsSourceLocationState()`
    - `FtsSourceLocationState(string id)` sets `Id = id`.
    - `FtsSourceLocationState(string id, int version)` sets `Id = id` and `Version = version`.
  - Properties (types match legacy):
    - `string Id`
    - `int ConfigId`
    - `string ConfigName`
    - `string Description`
    - `string FtpServerId`
    - `string Address`
    - `string Mask`
    - `int Version`
    - `string Protocol`
    - `string FileDirection`
    - `string OwnerEmail`
    - `string OwnerName`
    - `string OwnerPhone`
    - `string OwnerModule`
    - `string TradingPartner`
    - `bool RunOnMonday`
    - `bool RunOnTuesday`
    - `bool RunOnWed`
    - `bool RunOnThrus`
    - `bool RunOnFri`
    - `bool RunOnSat`
    - `bool RunOnSun`
    - `bool RunOnFrstDayOfMonthInd`
    - `bool RunOnLastDayOfMonthInd`
    - `byte RunOnThisDayOfMonth`
    - `string StartTime`
    - `string EndTime`
    - `string OperatorId`
    - `bool IsValid`
    - `DateTime EffectiveDate`
    - `bool SendNotifications`
    - `DateTime EndDate`
    - `DateTime LastModifiedDate`
    - `DateTime CreationDate`

### AutoMapper profiles (legacy parity)

Create:

- `services/fxfer-domain-api/src/Domain/Profiles/InboundTargetProfile.cs`
  - Type: `public sealed class InboundTargetProfile : Profile`
  - In the profile constructor, register:
    - `FTEMetadataUpdated` → `AddFtsInboundTargetCommand`:
      - `Requestor.IdentifierId` ← `src.UserID`
      - `ContractName` ← `src.ModuleName`
      - `ImportJobName` ← `src.ImportJobName`
      - `FTEMetadataId` ← `src.FTEMetadataId`
      - For all members: map only when the source member value is not null (legacy uses `!IsSourceValueNull`).
    - `FTEMetadataUpdated` → base `Command` as `AddFtsInboundTargetCommand`.
    - `FTEMetadataInvalidated` → `InvalidateFtsInboundTargetCommand`:
      - `Requestor.IdentifierId` ← `src.UserID`
      - `FTEMetadataId` ← `src.FTEMetadataId`
      - For all members: map only when the source member value is not null (legacy uses `!IsSourceValueNull`).
    - `FTEMetadataInvalidated` → base `Command` as `InvalidateFtsInboundTargetCommand`.

- `services/fxfer-domain-api/src/Domain/Profiles/IntegrationFileProfile.cs`
  - Type: `public sealed class IntegrationFileProfile : Profile`
  - In the profile constructor, register:
    - `ExtractFileGenerated` → `AddIntegrationFileCommand`:
      - `Requestor.IdentifierId` ← `src.UserID`
      - `ETFOutboundMetadataDefnId` ← `src.MetadataId`
      - `FileName` ← `src.FileDirectory`
      - `GroupInfo` ← `SetGroupInfo(src)`
      - For all members: map only when the source member value is not null (legacy uses `!IsSourceValueNull`).
    - `ExtractFileGenerated` → base `Command` as `AddIntegrationFileCommand`.
    - `SummaryReportGenerated` → `AddIntegrationFileCommand`:
      - `Requestor.IdentifierId` ← `src.UserID`
      - `ETFOutboundMetadataDefnId` ← `src.FTEMetadataId`
      - `FileName` ← `src.FileName`
      - `GroupInfo` ← `SetGroupInfo(src)`
      - For all members: map only when the source member value is not null (legacy uses `!IsSourceValueNull`).
    - `SummaryReportGenerated` → base `Command` as `AddIntegrationFileCommand`.
  - Keep the legacy helper methods (private) with the same behavior:
    - `SetGroupInfo(ExtractFileGenerated)` builds a list by copying each `GroupInfo` entry; it throws when `extractFileGenerated.GroupInfo` is null.
    - `SetGroupInfo(SummaryReportGenerated)` returns a single-item list.

### Dispatcher types

Do not create `CommandDispatcher` or `QueryDispatcher` in the target. The legacy types are empty subclasses used for legacy CQRS base discovery.

## Mapping (legacy → target)

| Legacy | Target | Note |
| --- | --- | --- |
| InboundTargetProfile.Configure() | merged into `InboundTargetProfile` constructor | AutoMapper v13+ uses constructor-based configuration. |
| InboundTargetProfile.MapFTEMetadataUpdatedEvent() | merged into `InboundTargetProfile` constructor | Mapping definition registered during profile construction. |
| InboundTargetProfile.MapFTEMetadataInvalidatedEvent() | merged into `InboundTargetProfile` constructor | Mapping definition registered during profile construction. |
| IntegrationFileProfile.Configure() | merged into `IntegrationFileProfile` constructor | AutoMapper v13+ uses constructor-based configuration. |
| IntegrationFileProfile.MapExtractFileGeneratedEvent() | merged into `IntegrationFileProfile` constructor | Mapping definition registered during profile construction. |
| IntegrationFileProfile.MapSummaryReportGeneratedEvent() | merged into `IntegrationFileProfile` constructor | Mapping definition registered during profile construction. |
| IntegrationFileProfile.SetGroupInfo(ExtractFileGenerated) | `IntegrationFileProfile.SetGroupInfo(ExtractFileGenerated)` | Keep behavior (copy list; throws if GroupInfo is null). |
| IntegrationFileProfile.SetGroupInfo(SummaryReportGenerated) | `IntegrationFileProfile.SetGroupInfo(SummaryReportGenerated)` | Keep behavior (single-item list). |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-008-domain-driven-design-pattern.md` section `2. Aggregates: Consistency Boundaries` (aggregate state is represented as a domain type in the Domain layer).
- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Command Side Components` (commands are mapped and dispatched on the write path; command/query separation is preserved).

