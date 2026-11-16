---
type: Target Placement
title: technical/components/shared-queries-parameters/TARGET
description: Where the shared-queries-parameters component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-queries-parameters
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:38:34Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-queries-parameters/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-queries-parameters
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 0
  owned_rules: 3
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Queries
  body_sha256: 62913e48a4e4fe27411fb030be4bd763205390b9c5eaaa0fe6afe32634a3e68d
---

﻿# Target: `shared-queries-parameters`

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Contracts/Queries`

## What to create

Create the query parameter (filter) types used by `fxfer-domain-api` query endpoints and query handlers.
Implement them as plain C# contract types under `services/fxfer-domain-api/src/Contracts/Queries/Parameters`.

Create the following C# types:

- `FileEventQueryParms`
  - Fields/properties: `FileId` (`string`).

- `FileExceptionQueryParms`
  - Fields/properties: `FileId` (`string`).

- `FileTransferTrackingQueryParms`
  - Fields/properties: `FileId` (`string`), `FileName` (`string`), `FileStatus` (`string`), `FileDirection` (`string`), `FileTransportType` (`string`), `FileSourceLocationId` (`string`), `StartDate` (`DateTime?`), `EndDate` (`DateTime?`), `FileSizeFrom` (`decimal?`), `FileSizeThrough` (`decimal?`), `TradingPartner` (`string`).

- `FtpServerQueryParms`
  - Fields/properties: `IsValid` (`bool?`), `FtpServerId` (`Guid`), `FtpServerName` (`string`), `FtpDescription` (`string`), `FtpUser` (`string`), `SearchOptions` (`List<SearchModeOption>`).

- `GetSourceLocationQueryParms`
  - Fields/properties: `SourceLocationId` (`string`), `IsActive` (`bool`).
  - Provide computed property `SoureceLocationGuid` that returns `Guid.Parse(SourceLocationId)`.
    - Preserve legacy failure behavior: if `SourceLocationId` is not a GUID string, `Guid.Parse` throws.
  - Preserve rule F01-104 by ensuring `IsActive` is not used as a filter by the details lookup behavior (query handler/executor code).

- `InboundTargetQueryParms`
  - Provide a parameterless constructor that sets `FTEMetadataId = Guid.Empty` (rule F05-031).
  - Fields/properties: `FTEMetadataId` (`Guid`), `ContractName` (`string`).

- `IntegrationFileQueryParms`
  - Provide a parameterless constructor that sets `FTSIntegrationFileId = Guid.Empty`.
  - Fields/properties: `FTSIntegrationFileId` (`Guid`), `FileName` (`string`).

- `SourceLocationQueryParms`
  - Fields/properties: `SoureceLocationId` (`string`), `ConfigId` (`int?`), `ConfigName` (`string`), `OwnerName` (`string`), `OwnerSubsystem` (`string`), `Protocol` (`string`), `IsActive` (`bool`).
  - Provide computed property `OwnerModuleGuid` that returns `Guid.Parse(OwnerSubsystem)`.

Property names (including legacy misspellings like `SoureceLocationGuid` / `SoureceLocationId`) are preserved because downstream query execution code refers to them by name.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `FileEventQueryParms` | `FileEventQueryParms` | Preserve property name `FileId`. |
| `FileExceptionQueryParms` | `FileExceptionQueryParms` | Preserve property name `FileId`. |
| `FileTransferTrackingQueryParms` | `FileTransferTrackingQueryParms` | Preserve optional filter fields and types. |
| `FtpServerQueryParms` | `FtpServerQueryParms` | Preserve `SearchOptions` shape for legacy match-mode behavior. |
| `GetSourceLocationQueryParms` | `GetSourceLocationQueryParms` | Preserve `SoureceLocationGuid` parsing via `Guid.Parse(SourceLocationId)` (F01-101) and keep `IsActive` as a non-filtering request parameter (F01-104). |
| `InboundTargetQueryParms` | `InboundTargetQueryParms` | Preserve constructor default `FTEMetadataId = Guid.Empty` (F05-031). |
| `IntegrationFileQueryParms` | `IntegrationFileQueryParms` | Preserve constructor default `FTSIntegrationFileId = Guid.Empty`. |
| `SourceLocationQueryParms` | `SourceLocationQueryParms` | Preserve `OwnerModuleGuid = Guid.Parse(OwnerSubsystem)` semantics. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (query-side DTOs/filters are part of the read-only query path).
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (identifier parsing/validation failures are represented consistently at the API boundary).
- `standards/application/api/STD-APP-API-001-api-design.md` section `3.5 Error Handling` (validation/parsing failures are surfaced via standard error responses).