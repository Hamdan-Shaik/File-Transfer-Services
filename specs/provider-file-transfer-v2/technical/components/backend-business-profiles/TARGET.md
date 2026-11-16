---
type: Target Placement
title: 'Target: backend-business-profiles'
description: Where the backend-business-profiles component goes in the new system and what to build, method by method.
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
  body_sha256: b238445b09ab38f282b94bba6732119b9299a4fa519145c0257c11267675bc1f
---

# Target: backend-business-profiles

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Domain/Profiles`

## What to build

Create the following AutoMapper profiles (legacy parity):

- `services/fxfer-domain-api/src/Domain/Profiles/FtpServerCommandHandlerProfile.cs`
  - Type: `public sealed class FtpServerCommandHandlerProfile : Profile`
  - In the profile constructor, register:
    - `AddFtpServerCommand` -> `FtpServerAdded`
      - For all members: map only when the source member value is not null (legacy uses `srs != null`).
    - `UpdateFtpServerCommand` -> `FtpServerUpdated`
      - For all members: map only when the source member value is not null (legacy uses `srs != null`).

- `services/fxfer-domain-api/src/Domain/Profiles/FtpServerProjectionWriterProfile.cs`
  - Type: `public sealed class FtpServerProjectionWriterProfile : Profile`
  - In the profile constructor, register:
    - `FtpServerAdded` -> `FtpServer`
      - For all members: map only when the source member value is not null (legacy uses `srs != null`).

- `services/fxfer-domain-api/src/Domain/Profiles/FtsInboundTargetProjectionWriterProfile.cs`
  - Type: `public sealed class FtsInboundTargetProjectionWriterProfile : Profile`
  - In the profile constructor, register:
    - `FtsInboundTargetAdded` -> `FtsInboundTarget`
      - For all members: map only when the source member value is not null (legacy uses `srs != null`).

- `services/fxfer-domain-api/src/Domain/Profiles/FtsIntegrationFileProjectionWriterProfile.cs`
  - Type: `public sealed class FtsIntegrationFileProjectionWriterProfile : Profile`
  - In the profile constructor, register:
    - `IntegrationFileAdded` -> `FtsIntegrationFile`
      - For all members: map only when the source member value is not null (legacy uses `srs != null`).

- `services/fxfer-domain-api/src/Domain/Profiles/InboundTargetProfile.cs`
  - Type: `public sealed class InboundTargetProfile : Profile`
  - In the profile constructor, register:
    - `FTEMetadataUpdated` -> `AddFtsInboundTargetCommand`:
      - `Requestor.IdentifierId` <- `src.UserID` (via `new RequestorModel { IdentifierId = src.UserID }`).
      - `ContractName` <- `src.ModuleName`.
      - `ImportJobName` <- `src.ImportJobName`.
      - `FTEMetadataId` <- `src.FTEMetadataId`.
      - For all members: map only when the source member value is not null (legacy uses `srs != null`).
    - `FTEMetadataUpdated` -> base `Command` as `AddFtsInboundTargetCommand`.
    - `FTEMetadataInvalidated` -> `InvalidateFtsInboundTargetCommand`:
      - `Requestor.IdentifierId` <- `src.UserID` (via `new RequestorModel { IdentifierId = src.UserID }`).
      - `FTEMetadataId` <- `src.FTEMetadataId`.
      - For all members: map only when the source member value is not null (legacy uses `srs != null`).
    - `FTEMetadataInvalidated` -> base `Command` as `InvalidateFtsInboundTargetCommand`.

- `services/fxfer-domain-api/src/Domain/Profiles/IntegrationFileProfile.cs`
  - Type: `public sealed class IntegrationFileProfile : Profile`
  - In the profile constructor, register:
    - `ExtractFileGenerated` -> `AddIntegrationFileCommand`:
      - `Requestor.IdentifierId` <- `src.UserID` (via `new RequestorModel { IdentifierId = src.UserID }`).
      - `ETFOutboundMetadataDefnId` <- `src.MetadataId`.
      - `FileName` <- `src.FileDirectory`.
      - `GroupInfo` <- `SetGroupInfo(src)`.
      - For all members: map only when the source member value is not null (legacy uses `srs != null`).
    - `ExtractFileGenerated` -> base `Command` as `AddIntegrationFileCommand`.
    - `SummaryReportGenerated` -> `AddIntegrationFileCommand`:
      - `Requestor.IdentifierId` <- `src.UserID` (via `new RequestorModel { IdentifierId = src.UserID }`).
      - `ETFOutboundMetadataDefnId` <- `src.FTEMetadataId`.
      - `FileName` <- `src.FileName`.
      - `GroupInfo` <- `SetGroupInfo(src)`.
      - For all members: map only when the source member value is not null (legacy uses `srs != null`).
    - `SummaryReportGenerated` -> base `Command` as `AddIntegrationFileCommand`.
  - Keep the legacy helper methods (private) with the same behavior:
    - `SetGroupInfo(ExtractFileGenerated)` builds a list by copying each `GroupInfo` entry; it throws when `extractFileGenerated.GroupInfo` is null.
    - `SetGroupInfo(SummaryReportGenerated)` returns a single-item list.

## Mapping (legacy -> target)

| Legacy | Target | Note |
| --- | --- | --- |
| FtpServerCommandHandlerProfile.FtpServerCommandHandlerProfile() | `FtpServerCommandHandlerProfile` constructor | AutoMapper profile is constructor-configured in both legacy and target. |
| FtpServerProjectionWriterProfile.FtpServerProjectionWriterProfile() | `FtpServerProjectionWriterProfile` constructor | AutoMapper profile is constructor-configured in both legacy and target. |
| FtsInboundTargetProjectionWriterProfile.FtsInboundTargetProjectionWriterProfile() | `FtsInboundTargetProjectionWriterProfile` constructor | AutoMapper profile is constructor-configured in both legacy and target. |
| FtsIntegrationFileProjectionWriterProfile.FtsIntegrationFileProjectionWriterProfile() | `FtsIntegrationFileProjectionWriterProfile` constructor | AutoMapper profile is constructor-configured in both legacy and target. |
| InboundTargetProfile.InboundTargetProfile() | merged into `InboundTargetProfile` constructor | Target keeps constructor-based registration; private helper methods may be kept or inlined. |
| InboundTargetProfile.MapFTEMetadataUpdatedEvent() | merged into `InboundTargetProfile` constructor | Mapping definition registered during profile construction. |
| InboundTargetProfile.MapFTEMetadataInvalidatedEvent() | merged into `InboundTargetProfile` constructor | Mapping definition registered during profile construction. |
| IntegrationFileProfile.IntegrationFileProfile() | merged into `IntegrationFileProfile` constructor | Target keeps constructor-based registration; private helper methods may be kept or inlined. |
| IntegrationFileProfile.MapExtractFileGeneratedEvent() | merged into `IntegrationFileProfile` constructor | Mapping definition registered during profile construction. |
| IntegrationFileProfile.MapSummaryReportGeneratedEvent() | merged into `IntegrationFileProfile` constructor | Mapping definition registered during profile construction. |
| IntegrationFileProfile.SetGroupInfo(ExtractFileGenerated) | `IntegrationFileProfile.SetGroupInfo(ExtractFileGenerated)` | Keep behavior (copy list; throws if GroupInfo is null). |
| IntegrationFileProfile.SetGroupInfo(SummaryReportGenerated) | `IntegrationFileProfile.SetGroupInfo(SummaryReportGenerated)` | Keep behavior (single-item list). |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Command Side Components` (command-side mapping utilities are part of the command-handling stack).
- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Command Flow (Write Path)` (event/command translation is validated as part of the write path).

