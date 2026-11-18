---
type: Behavior Specification
title: technical/components/shared-queries-parameters/SPEC
description: 'What the shared-queries-parameters component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 1333a3b9b433483e8866abcdf271af6f2cc396164857414d042a53f0db42d823
---

﻿# shared-queries-parameters: legacy FXFER query parameter types

This component defines the legacy query-parameter (filter) objects used as `Where` criteria by FXFER query contract types.
These types are consumed by query executors/handlers when searching and looking up FXFER configuration and tracking data.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.Parameters.FileEventQueryParms
File-event query filter criteria.

- Properties:
  - `FileId` (`string`): file identifier used to constrain file-event search.
- Data: none.
- Errors: none in this parameter object.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.Parameters.FileExceptionQueryParms
File-exception query filter criteria.

- Properties:
  - `FileId` (`string`): file identifier used to constrain file-exception search.
- Data: none.
- Errors: none in this parameter object.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.Parameters.FileTransferTrackingQueryParms
File transfer tracking search filter criteria.

- Properties:
  - `FileId` (`string`): unique number/identifier of a file.
  - `FileName` (`string`): name of the file.
  - `FileStatus` (`string`): status filter.
  - `FileDirection` (`string`): direction filter (legacy examples: `InBound` / `OutBound`).
  - `FileTransportType` (`string`): protocol used to transfer the file.
  - `FileSourceLocationId` (`string`): source location identifier filter.
  - `StartDate` (`DateTime?`): transfer start timestamp filter.
  - `EndDate` (`DateTime?`): transfer end timestamp filter.
  - `FileSizeFrom` (`decimal?`): minimum file size filter.
  - `FileSizeThrough` (`decimal?`): maximum file size filter.
  - `TradingPartner` (`string`): trading partner filter.
- Data: none.
- Errors: none in this parameter object.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.Parameters.FtpServerQueryParms
FTP server profile search filter criteria.

- Properties:
  - `IsValid` (`bool?`): filter by whether an FTP server record is valid.
  - `FtpServerId` (`Guid`): internal identifier of the FTP server record.
  - `FtpServerName` (`string`): FTP server name.
  - `FtpDescription` (`string`): FTP server description.
  - `FtpUser` (`string`): FTP server username.
  - `SearchOptions` (`List<SearchModeOption>`): match mode options used by legacy query execution.
- Data: none.
- Errors: none in this parameter object.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.Parameters.GetSourceLocationQueryParms
Source location details lookup parameters.

- Properties:
  - `SourceLocationId` (`string`): source location id as a string.
  - `IsActive` (`bool`): legacy request parameter that is not used as a filter for details lookup.
  - `SoureceLocationGuid` (`Guid`, computed): returns `Guid.Parse(SourceLocationId)`.
- Logic:
  - `SoureceLocationGuid` parses `SourceLocationId` via `Guid.Parse` and throws if `SourceLocationId` is not a GUID string.
  - In legacy details lookup execution, `IsActive` is not referenced and does not filter results; the returned model’s `IsActive` value is populated from stored `sourceLocation.IsValid`.
- Data: none (parameter object only).
- Errors:
  - `SoureceLocationGuid` throws exceptions from `Guid.Parse` when `SourceLocationId` is null or not a GUID string.
- Rules: F01-101, F01-104.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.Parameters.InboundTargetQueryParms
Inbound target query filter criteria.

- Constructors:
  - `InboundTargetQueryParms()`: sets `FTEMetadataId = Guid.Empty`.
- Properties:
  - `FTEMetadataId` (`Guid`): key identifying an external file definition record.
  - `ContractName` (`string`): contract name filter.
- Data: none.
- Errors: none in this parameter object.
- Rules: F05-031.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.Parameters.IntegrationFileQueryParms
Integration file lookup query filter criteria.

- Constructors:
  - `IntegrationFileQueryParms()`: sets `FTSIntegrationFileId = Guid.Empty`.
- Properties:
  - `FTSIntegrationFileId` (`Guid`): key identifying an integration file record.
  - `FileName` (`string`): file name filter.
- Data: none.
- Errors: none in this parameter object.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.Parameters.SourceLocationQueryParms
Source location search filter criteria.

- Properties:
  - `SoureceLocationId` (`string`): source location id (legacy note: property is not marked with `DataMember`).
  - `ConfigId` (`int?`): configuration id filter.
  - `ConfigName` (`string`): configuration name filter.
  - `OwnerName` (`string`): owner name filter.
  - `OwnerSubsystem` (`string`): owner subsystem identifier string.
  - `Protocol` (`string`): protocol filter.
  - `IsActive` (`bool`): active flag filter.
  - `OwnerModuleGuid` (`Guid`, computed): returns `Guid.Parse(OwnerSubsystem)`.
- Logic:
  - `OwnerModuleGuid` parses `OwnerSubsystem` via `Guid.Parse`.
- Data: none.
- Errors:
  - `OwnerModuleGuid` throws exceptions from `Guid.Parse` when `OwnerSubsystem` is null or not a GUID string.
- Rules: none.

## Rules implemented
| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-101 | The requested `SourceLocationId` is parsed as a GUID and used to look up `FtsSourceLocation` by id; the lookup returns a source location only when exactly one row matches. | `GetSourceLocationQueryParms.SoureceLocationGuid`; legacy lookup: `GetSourceLocationQueryExecutor.GetSourceLocation` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-104 | The details query does not filter by requested `IsActive` and always returns the source location when the id matches, including the stored active flag. | legacy execution: `GetSourceLocationQueryExecutor.ApplyWhereClause` and `GetSourceLocationQueryExecutor.GetBaseQuery` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F05-031 | `InboundTargetQuery` initializes its `Where` object and defaults `FTEMetadataId` to `Guid.Empty`. | legacy constructors: `InboundTargetQuery..ctor`; `InboundTargetQueryParms..ctor` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |

## Not carried over

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (query-side inputs are read-only filter criteria; these parameter objects must not introduce side effects).
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (invalid identifiers/validation failures are surfaced consistently at the HTTP boundary using a standardized error contract).
- `standards/application/api/STD-APP-API-001-api-design.md` section `3.5 Error Handling` (input parsing/validation failures map to consistent HTTP error responses).