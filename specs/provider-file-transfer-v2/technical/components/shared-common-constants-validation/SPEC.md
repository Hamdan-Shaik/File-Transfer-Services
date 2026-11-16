---
type: Behavior Specification
title: technical/components/shared-common-constants-validation/SPEC
description: 'What the shared-common-constants-validation component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-common-constants-validation
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:46:01Z
sources:
- id: inventory
  resource: /technical/components/shared-common-constants-validation/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-common-constants-validation
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 3
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain/Validation
  body_sha256: f6aa35ec7d5000e37fb5e739656b36d0a43f63bd77a2618e69105eebc3c261db
---

﻿# shared-common-constants-validation: validation constant vocabulary (error types/messages/properties)

This component contains the legacy validation constant groups defined under `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.Validation`.
It provides shared string constants used by FXFER validation logic when identifying validation error types, emitting validation messages, and naming the request/property that failed validation.

## ErrorType

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

## Messages

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

## Properties

Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.Validation.Properties`

Validation property-name strings used to label which field failed validation.

- `FtpProxyPort` = `FtpProxyPort`
- `FtpServerId` = `FtpServerId`
- `FtpServerIp` = `FtpServerIp`
- `RunSchedule` = `Run Schedule`

## Rules implemented

None.

## Not carried over

None.
