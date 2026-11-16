---
type: Target Placement
title: technical/components/backend-data-file-transfer/TARGET
description: Where the backend-data-file-transfer component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-file-transfer
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:21:12Z
sources:
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-data-file-transfer/INVENTORY.yaml
  title: Every legacy class and method (11 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-file-transfer
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 11
  methods: 1
  owned_rules: 2
  flows:
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-bff
  folder: src/Contracts
  body_sha256: 21e5bb6292b3e5c4f4e3a10d03149c4dbb75632730e496eb56973cf9fac6dcd0
---

﻿# Target: `backend-data-file-transfer`

## Where

- Deployable: `fxfer-bff` (`services/fxfer-bff`)
- Folder: `services/fxfer-bff/src/Contracts`

## What to create

Create BFF contract DTOs (request/response types) for FTP server profiles and transfer configurations.

- FTP server profile contracts (match `technical/target/deployables/fxfer-bff/CONTRACT.md`):
  - `services/fxfer-bff/src/Contracts/FtpServers/FtpServerUpsertRequest.cs`
    - Fields: `FtpServerName`, `FtpDescription`, `FtpServerIp`, `FtpTransferMode`, `FtpUser`, `FtpPassword`, `FtpPasswordConfirmation`, `FtpPort`, `FtpProxyAddress`, `FtpProxyPort`, `UseSshAuth`, `IsValid`, `OperatorId`.
  - `services/fxfer-bff/src/Contracts/FtpServers/FtpServerPatchRequest.cs`
    - Fields: optional updates for the FTP server profile; `OperatorId` required.
  - `services/fxfer-bff/src/Contracts/FtpServers/FtpServer.cs`
    - Fields: `FtpServerId`, profile fields (excluding passwords), timestamps.
  - `services/fxfer-bff/src/Contracts/FtpServers/FtpServerOption.cs`
    - Fields: `FtpServerId`, `FtpServerName`.

- Transfer configuration contracts (match `technical/target/deployables/fxfer-bff/CONTRACT.md`):
  - `services/fxfer-bff/src/Contracts/TransferConfigurations/TransferConfigurationUpsertRequest.cs`
    - Fields: `ConfigName`, `Description`, `FtpServerId`, `Address`, `Mask`, `Protocol`, `FileDirection`, `OwnerName`, `OwnerEmail`, `OwnerPhoneNumber`, `OwnerSubsystem`, `TradingPartner`, `SendNotification`, schedule fields, `StartWindow`, `EndWindow`, `IsActive`, `Suspended`, `Destinations`, `OperatorId`.
  - `services/fxfer-bff/src/Contracts/TransferConfigurations/TransferConfigurationPatchRequest.cs`
    - Fields: `IsActive`, `Suspended`, `OperatorId`.
  - `services/fxfer-bff/src/Contracts/TransferConfigurations/TransferConfigurationSummary.cs`
    - Fields: `FtsSourceLocationId`, `ConfigId`, `ConfigName`, `Protocol`, `Address`, `OwnerName`, `TradingPartner`, `IsActive`, `Suspended`.
  - `services/fxfer-bff/src/Contracts/TransferConfigurations/TransferConfiguration.cs`
    - Fields: summary fields plus full configuration fields and timestamps; includes `Destinations`.
  - `services/fxfer-bff/src/Contracts/TransferConfigurations/DestinationUpsert.cs`
    - Fields: destination fields per BFF contract, including `FteMetadataId`, `ContractName`, `ImportJobName`.
  - `services/fxfer-bff/src/Contracts/TransferConfigurations/Destination.cs`
    - Fields: same as `DestinationUpsert` plus timestamps.

- Common contract DTOs used by the above contracts:
  - `services/fxfer-bff/src/Contracts/Common/PagedResult.cs`
    - Generic container: `Items`, `Total`, `Page`, `PageSize`.
  - `services/fxfer-bff/src/Contracts/Common/SourceLocationOption.cs`
    - Fields: `FtsSourceLocationId`, `ConfigName`, `Address`.
  - `services/fxfer-bff/src/Contracts/Common/InboundTargetMetadata.cs`
    - Fields: `FteMetadataId`, `ImportJobName`.

- Search-field helper (legacy-parity; implements F02-042 and F02-044):
  - `services/fxfer-bff/src/Contracts/Common/SearchField.cs`
    - Fields: `FieldName` (string), `FieldValue` (object).
  - `services/fxfer-bff/src/Contracts/Common/SearchHelper.cs`
    - `public static string GetFieldValue(IReadOnlyList<SearchField> searchFields, string fieldName)`
      - If `searchFields` is null/empty, return `string.Empty`.
      - Otherwise, return `FieldValue.ToString()` for the first element where `FieldName == fieldName`.
      - If there is no match, return `string.Empty`.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `AddFileTransferConfigurationModel` | `TransferConfigurationUpsertRequest` | UI list-item selection objects are replaced by primitive fields (`uuid`/`string`) per BFF schema. |
| `DestinationConfigurationModel` | `DestinationUpsert` and `Destination` | UI-only fields (line-number/status/edit flags) are dropped; destination payload is per BFF schema. |
| `FileTransferConfigurationModel` | `TransferConfiguration` | List-item wrappers are replaced by the schema fields (`protocol`, `fileDirection`, `ownerSubsystem`, etc.). |
| `FileTransferConfigurationSearchCriteriaModel` | `drop: represented as query parameters on GET /transfer-configurations` | Target uses explicit query parameters rather than a search-criteria DTO. |
| `FtpServerModel` | `FtpServerUpsertRequest` (write) and `FtpServer` (read) | Target does not return passwords; request DTO carries password fields when creating/updating. |
| `FtpServerSearchCriteriaModel` | `drop: represented as query parameters on GET /ftp-servers` | Target uses explicit query parameters rather than a search-criteria DTO. |
| `InboundTargetModel` | `InboundTargetMetadata` | Same field intent; naming follows BFF schema (`FteMetadataId`). |
| `SourceLocationModel` | `SourceLocationOption` | Renamed to reflect UI dropdown option contract; `SourceLocationId` becomes `FtsSourceLocationId`. |
| `FileTransferEnumerations` | `drop: empty legacy placeholder` | No members and no behavior. |
| `MaintainabilityDetails` | `drop: code-review metadata attribute` | Not part of runtime behavior in the target. |
| `SearchHelper` | `SearchHelper` | Kept as a small utility in `src/Contracts/Common` because controller code depends on its legacy semantics. |
| `SearchHelper.GetFieldValue(SearchCriteriaModel, string)` | `SearchHelper.GetFieldValue(IReadOnlyList<SearchField>, string)` | Keep the same match/empty-string semantics; input shape changes to a target-local `SearchField` list. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-001-backend-for-frontend-pattern.md` section `Executive Summary > Solution Overview` (BFF shapes contracts for UI needs; does not become the domain authority)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `1. OpenAPI Specification Document` (contract DTOs match the OpenAPI-described schemas and are governed as contract artifacts)