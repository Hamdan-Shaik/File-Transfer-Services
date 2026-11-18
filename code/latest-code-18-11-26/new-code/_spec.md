---
type: Behavior Specification
title: technical/components/shared-common-common-constants/SPEC
description: 'What the shared-common-common-constants component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-common-common-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:40:13Z
sources:
- id: inventory
  resource: /technical/components/shared-common-common-constants/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-common-common-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 5
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain/Constants
  body_sha256: 5dff49e48c1710d143e1a1f888d51785cee5111011bd3dacb3b9454d6d57ba78
---

ï»¿# shared-common-common-constants: shared constants for FXFER domain validation + sorting

This component contains the legacy constant groups in `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants`.
It provides shared string constants used by FXFER domain and validation logic for error vocabulary (types/messages/properties), common regex patterns, and sort-field names.

## CommonConstants

Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants`

Top-level constant container.

- `FtpServerCode` = `1`
- `ModuleName` = `FileTransfer`

## CommonConstants.RegexPatterns

Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.RegexPatterns`

Regex pattern constants.

- `IpAddressPattern` = `\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b`
  - Matches four dot-separated 1-3 digit groups.
  - Note: legacy pattern does not enforce numeric range validity (e.g., it can match `999.999.999.999`).

## CommonConstants.Validation

Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.Validation`

Shared validation constants used when emitting business-validation errors.

- `BusinessErrorCode` = `E`
- `ProcessingSystem` = `FileTransfer`

## CommonConstants.Validation.ErrorType

Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.Validation.ErrorType`

Validation error-type identifiers.

- `DuplicatedFtpServerName` = `DuplicatedFtpServerName`
- `InvalidCommand` = `Invalid Command`
- `InvalidFtpProxyPort` = `InvalidFtpProxyPort`
- `InvalidFtpServerId` = `InvalidFtpServerId`
- `InvalidParameters` = `InvalidSearchParameters`
- `InvalidIpAddressFormat` = `InvalidIpAddressFormat`
- `NuGetPackage` = `NuGet package`
- `QueryExecutionError` = `QueryExecutionError`
- `RequiredFields` = `RequiredFields`

## CommonConstants.Validation.Messages

Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.Validation.Messages`

Validation error message strings.

- `CommandError` = `Command Execution Error`
- `DuplicatedFtpServerName` = `Duplicated FTP Server Name`
- `ExcludedExtension` = `Source file mask extension is not Allowed:`
- `FTRepositoryGetResultsError` = `File Transfer Repository error when getting results for`
- `IpAddressInvalidFormat` = `Invalid Format for IP Address`
- `InvalidValue` = `Invalid value for`
- `MissingConfigId` = `Missing ConfigId`
- `MissingField` = `Missing field`

## CommonConstants.Validation.Properties

Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.Validation.Properties`

Validation property-name strings used to label which field failed validation.

- `FtpProxyPort` = `FtpProxyPort`
- `FtpServerId` = `FtpServerId`
- `FtpServerIp` = `FtpServerIp`
- `RunSchedule` = `Run Schedule`

## CommonConstants.SortFields

Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.SortFields`

Container for sort-field name constants used by search/query surfaces.

## CommonConstants.SortFields.FtpServer

Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.SortFields.FtpServer`

Sort field names for FTP server search results.

- `CreationDate` = `CreationDate`
- `FtpDescription` = `FtpDescription`
- `FtpServerId` = `FtpServerId`
- `FtpServerIp` = `FtpServerIp`
- `FtpServerName` = `FtpServerName`
- `FtpUser` = `FtpUser`
- `IsValid` = `IsValid`

## Rules implemented

None.

## Not carried over

None.

