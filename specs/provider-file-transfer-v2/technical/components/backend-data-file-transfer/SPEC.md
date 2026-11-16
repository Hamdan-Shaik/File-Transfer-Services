---
type: Behavior Specification
title: technical/components/backend-data-file-transfer/SPEC
description: 'What the backend-data-file-transfer component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 28d9bfff8bdeba61de8773b3637f2a94fe51af6319f13b9a5e7e41c9249c8e51
---

﻿# backend-data-file-transfer: FXFER BFF contract models + search-field helper

This component contains the legacy UX/BFF-facing data models used to represent file-transfer configurations and FTP server credentials in the File Transfer UX.
It is used by UX service/controller code that builds search requests and returns DTOs to the UI.

## AddFileTransferConfigurationModel
Represents the UI-facing payload used to add a file-transfer configuration, including schedule flags and a destination list.

- Constructor initialization (always non-null):
  - `DestinationDetails` = empty list
  - `DestinationProtocol`, `FTPServer`, `FileDirection`, `OwnerSubsystem`, `ProtocolType`, `RunOnThisDay`, `RunSchedule`, `TimeWindowStart`, `TimeWindowEnd` = new list-item objects
- Fields (selected):
  - `ConfigId` (int, serialized as XML attribute `id`)
  - `ConfigName`, `Description`, `Address`, `TradingPartner`
  - `FtsSourceLocationId` (string GUID in practice)
  - Schedule booleans: `RunOnMonday`…`RunOnSunday`, `RunOnFirstDay`, `RunOnLastDay`
  - State flags: `IsActive`, `Suspended`, `Suspend`, `SendNotification`
  - Datalist-backed selection fields (legacy `[ListItem]` attribute binds to these datalists):
    - `DestinationProtocol`: `FileTransfer.DataList.ProtocolType`
    - `FTPServer`: `FileTransfer.DataList.FTPServer`
    - `FileDirection`: `FileTransfer.DataList.FileDirection`
    - `OwnerSubsystem`: `FileTransfer.DataList.OwnerModule`
    - `ProtocolType`: `FileTransfer.DataList.ProtocolType`
    - `RunOnThisDay`: `FileTransfer.DataList.RunOnThisDay`
    - `RunSchedule`: `FileTransfer.DataList.RunSchedule`
    - `TimeWindowStart`, `TimeWindowEnd`: `FileTransfer.DataList.TimeWindow`

## DestinationConfigurationModel
Represents one destination row within a transfer configuration.

- Constructor initialization (always non-null):
  - `FTPServer`, `DestinationProtocol`, `RenamePattern`, `DestinationType`, `ContractName` = new list-item objects
- Fields:
  - Flags: `Active`, `CompressAtDestination`, `DestinationIsArchive`, `OverwriteAtDestination`, `RenameAtDestination`, `IsAddedUpdated`
  - Identifiers/positioning: `DestinationId`, `DestinationLineNumber`
  - Endpoint: `DestinationAddress`
  - Contacts: `ContactName`, `ContactEmail`, `ContactPhone`
  - Status: `Status`
  - Datalist-backed selection fields:
    - `DestinationProtocol`: `FileTransfer.DataList.ProtocolType`
    - `DestinationType`: `FileTransfer.DataList.DestinationType`
    - `RenamePattern`: `FileTransfer.DataList.RenamePattern`
    - `ContractName`: `FileTransfer.DataList.Contracts`
  - Inbound target metadata: `FTEMetadataId` (nullable GUID), `ImportJobName`

## FileTransferConfigurationModel
Represents the UI-facing payload used to view/edit an existing file-transfer configuration.

- Constructor initialization (always non-null):
  - `DestinationDetail` = new destination model
  - `DestinationDetails` = empty list
  - `DestinationProtocol`, `FTPServer`, `FileDirection`, `OwnerSubsystem`, `ProtocolType`, `RunOnThisDay`, `RunSchedule`, `TimeWindowStart`, `TimeWindowEnd` = new list-item objects
- Fields: same shape as `AddFileTransferConfigurationModel` plus:
  - `DestinationDetail` (single destination object) and `DestinationDetails` (list)
  - Destination-side metadata: `ContractName`, `FTEMetadataId`

## FileTransferConfigurationSearchCriteriaModel
Represents the UI-facing search criteria for finding transfer configurations.

- Constructor initialization (always non-null):
  - `OwnerModule`, `Protocol` = new list-item objects
- Fields:
  - `ConfigId`, `ConfigName`, `OwnerName`
  - Datalist-backed selection fields:
    - `OwnerModule`: `FileTransfer.DataList.OwnerModule`
    - `Protocol`: `FileTransfer.DataList.ProtocolType`

## FtpServerModel
Represents an FTP server credential/profile record as used by the UX.

- Fields:
  - `FtpServerId` (string GUID in practice)
  - `FtpServerName`, `FtpServerIp`, `FtpTransferMode`, `FtpUser`
  - `FtpPassword`, `FtpPasswordConfirmation`
  - `FtpDescription`
  - `FtpPort` (short)
  - Proxy: `FtpProxyAddress`, `FtpProxyPort` (nullable short)
  - Audit: `CreationDate`, `LastModifiedDate`
  - Flags: `IsValid` (nullable bool), `RemovePassword` (nullable bool)
  - `OperatorId`

## FtpServerSearchCriteriaModel
Represents the (limited) search criteria used to filter FTP server records.

- Fields:
  - `FtpServerName`
  - `IsValid` (nullable bool)

## InboundTargetModel
Represents an inbound-target metadata record.

- Fields:
  - `FTEMetadataId` (GUID)
  - `ImportJobName`

## SourceLocationModel
Represents a source-location list option.

- Fields:
  - `SourceLocationId` (string GUID in practice)
  - `ConfigName`
  - `Address`

## FileTransferEnumerations
Legacy placeholder type (no members).

## MaintainabilityDetails
Legacy attribute type used to annotate code with maintainability review metadata.

## SearchHelper
Helper for extracting a search-field value from a search-criteria object.

### GetFieldValue(SearchCriteriaModel searchCriteria, string fieldName)
- Purpose: Return the search-field value (as a string) for the first matching field name.
- Inputs:
  - `searchCriteria.SearchFields`: a list of search-field entries
  - `fieldName`: the field name to match (exact match)
- Logic:
  - If `SearchFields` is null/empty, return `string.Empty`.
  - Otherwise, find the first entry where `FieldName == fieldName`.
  - If a match is found, return `FieldValue.ToString()`.
  - If no match is found, return `string.Empty`.
- Data: none.
- Errors: If a matching field exists but its `FieldValue` is null, calling `ToString()` throws a null-reference exception (legacy behavior).
- Rules: F02-042, F02-044.

## Rules implemented
| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F02-042 | Missing FTP server search fields are treated as empty strings. | `SearchHelper.GetFieldValue` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-044 | Field lookup returns the first matching field value as string; missing returns empty string. | `SearchHelper.GetFieldValue` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |

## Not carried over
- `FileTransferEnumerations`: empty placeholder type; no behavior or referenced members.
- `MaintainabilityDetails`: code-review metadata attribute; not part of runtime behavior in the target.
