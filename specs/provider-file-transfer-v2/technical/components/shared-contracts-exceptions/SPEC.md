---
type: Behavior Specification
title: technical/components/shared-contracts-exceptions/SPEC
description: 'What the shared-contracts-exceptions component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-exceptions
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:35:59Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-exceptions/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-exceptions
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 4
  methods: 1
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Exceptions
  body_sha256: bc9145ea71b92d4a1e48a6d74cf655c471c5c14fda1ee157f7cb84800f789b59
---

﻿# shared-contracts-exceptions: legacy exception and validation contracts

This component provides shared exception/validation contract types used by the legacy File Transfer service.
It defines (a) the list of excluded file extensions, (b) constant strings used as validation field names/codes,
and (c) a validation exception type that can be converted into a WCF fault payload.

## ExcludedFileExtensions
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.ExcludedFileExtensions`

Container type for `ExcludedFileExtensionList`, an enum of disallowed file extensions (used by validation to reject unsafe uploads).

### ExcludedFileExtensionList
- Purpose: identify executable/script-like extensions that are not permitted.
- Inputs: none.
- Logic: enum members are the disallowed extensions.
- Data: none.
- Errors: none.
- Rules: none.

Enum members (exact): `exe`, `dll`, `bat`, `cmd`, `vbs`, `ps1`, `jar`, `app`, `com`, `scr`, `msi`, `js`, `wsf`, `reg`, `hta`, `pif`, `vb`, `py`, `rb`, `sh`, `cpl`, `msc`.

## FileTransferExceptionConstants
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferExceptionConstants`

Container for constant groups used when producing validation and fault information.

### Validation
- Purpose: provide stable field/code keys and default messages for validation scenarios.
- Constants:
  - `Address = "Address"`
  - `ConfigId = "ConfigId"`
  - `ConfigIdNotFound = "ConfigIdNotFound"`
  - `ConfigName = "ConfigName"`
  - `Mask = "Mask"`
  - `Message = "Business Error occurred processing the command."`
  - `MissingRequiredSearchCriteria = "MissingSearchCriteria"`
  - `OwnerName = "OwnerName"`
  - `OwnerSubsystem = "OwnerSubsystem"`
  - `ProcessingSystem = "FileTransfer"`
  - `FtpSoureceLocationId = "FtpSoureceLocationId"`
  - `FtpServerId = "FtpServerId"`
  - `IsSuspendedChangedDate = "IsSuspendedChangedDate"`
  - `OperatorId = "OperatorId"`

### Fields
- Purpose: provide stable legacy field name strings (used for validation error association).
- Constants:
  - `ConfigId = "ConfigId"`
  - `ConfigName = "ConfigName"`
  - `MissingRequiredSearchCriteria = "MissingSearchCriteria"`
  - `OwnerName = "OwnerName"`
  - `OwnerSubsystem = "OwnerSubsystem"`
  - `SoureceLocationId = "SoureceLocationId"`
  - `FtsSourceLocationSeq = "FTS_SOURCE_LOCATION_SEQ"`

### FaultExceptions
- Purpose: provide stable log prefixes/messages for SOAP fault scenarios.
- Constants:
  - `CustomFault = "BAS File Transfer Service Application Error Custom Fault "`
  - `ApplicationError = "BAS File Transfer Service Application Error"`

## FileTransferValidationException
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferValidationException`

Exception type used for validation failures, carrying an optional `FieldName` and a `ValidationErrorMessage`. It implements the legacy `IFaultGenerator` interface so the legacy WCF host can translate it into a custom typed SOAP fault.

### GenerateFault()
- Purpose: convert this validation exception into a typed WCF fault.
- Inputs: none (uses `FieldName` and `ValidationErrorMessage`).
- Logic:
  - Create `new FileTransfertBASValidationException(FieldName, ValidationErrorMessage)`.
  - Return `new FaultException<FileTransfertBASValidationException>(faultDetail, new FaultReason(ValidationErrorMessage))`.
- Data: none.
- Errors: none.
- Rules: none.

## FileTransfertBASValidationException
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransfertBASValidationException`

Serializable (DataContract) payload used as the typed fault detail for validation faults.

- `FieldName`: string (DataMember)
- `ValidationErrorMessage`: string (DataMember)

## Rules implemented

None.

## Not carried over

- `FileTransferValidationException.GenerateFault()`: legacy WCF `FaultException<>` plumbing; target validation errors are surfaced as RFC 7807 Problem Details (HTTP) or translated by the SOAP facade.
