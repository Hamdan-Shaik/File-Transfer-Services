---
type: Behavior Specification
title: 'shared-exceptions-file-transfer-exception-constants: legacy exception constant groups'
description: 'What the shared-exceptions-file-transfer-exception-constants component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-exceptions-file-transfer-exception-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:10:16Z
sources:
- id: inventory
  resource: /technical/components/shared-exceptions-file-transfer-exception-constants/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-exceptions-file-transfer-exception-constants
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
  folder: src/Contracts/Exceptions
  body_sha256: 21ab44aeaf789c39d167a247e3b151204e4a016a5172d920697b5bb8970ca37d
---

# shared-exceptions-file-transfer-exception-constants: legacy exception constant groups

This component defines constant string values used by the legacy File Transfer service when producing validation failures
and when logging/raising SOAP fault errors.
It is consumed by other components to keep field keys, validation codes, and fault log prefixes consistent.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferExceptionConstants.Validation

Constant group used for validation field/code keys and a default validation message.

- Constants:
  - `Address`: `Address`
  - `ConfigId`: `ConfigId`
  - `ConfigIdNotFound`: `ConfigIdNotFound`
  - `ConfigName`: `ConfigName`
  - `Mask`: `Mask`
  - `Message`: `Business Error occurred processing the command.`
  - `MissingRequiredSearchCriteria`: `MissingSearchCriteria`
  - `OwnerName`: `OwnerName`
  - `OwnerSubsystem`: `OwnerSubsystem`
  - `ProcessingSystem`: `FileTransfer`
  - `FtpSoureceLocationId`: `FtpSoureceLocationId`
  - `FtpServerId`: `FtpServerId`
  - `IsSuspendedChangedDate`: `IsSuspendedChangedDate`
  - `OperatorId`: `OperatorId`

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferExceptionConstants.Fields

Constant group used for legacy field-name strings (for associating a validation error with a field).

- Constants:
  - `ConfigId`: `ConfigId`
  - `ConfigName`: `ConfigName`
  - `MissingRequiredSearchCriteria`: `MissingSearchCriteria`
  - `OwnerName`: `OwnerName`
  - `OwnerSubsystem`: `OwnerSubsystem`
  - `SoureceLocationId`: `SoureceLocationId`
  - `FtsSourceLocationSeq`: `FTS_SOURCE_LOCATION_SEQ`

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferExceptionConstants.FaultExceptions

Constant group used as stable log prefixes/messages for SOAP fault scenarios.

- Constants:
  - `CustomFault`: `BAS File Transfer Service Application Error Custom Fault `
  - `ApplicationError`: `BAS File Transfer Service Application Error`

## Rules implemented

None.

## Not carried over

None.

