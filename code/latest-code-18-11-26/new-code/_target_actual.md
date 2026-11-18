---
type: Target Placement
title: technical/components/shared-contracts-exceptions/TARGET
description: Where the shared-contracts-exceptions component goes in the new system and what to build, method by method.
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
  body_sha256: 2a659e27b678de956bff4df0cf8c6bb92714d83a1e979dc558ec948813a707ea
---

ï»¿# Target: `shared-contracts-exceptions`

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Contracts/Exceptions`

## What to create

Create C# contract types and constants used across the FXFER target codebase to represent validation failures consistently.

- `ExcludedFileExtensions`
  - Provide the enum `ExcludedFileExtensionList` with the same member names as legacy:
    - `exe`, `dll`, `bat`, `cmd`, `vbs`, `ps1`, `jar`, `app`, `com`, `scr`, `msi`, `js`, `wsf`, `reg`, `hta`, `pif`, `vb`, `py`, `rb`, `sh`, `cpl`, `msc`

- `FileTransferExceptionConstants`
  - Provide nested constant groups mirroring legacy:
    - `Validation` (keys/codes and default message)
    - `Fields` (legacy field-name strings)
    - `FaultExceptions` (legacy fault log prefixes)
  - Preserve constant names and values exactly (including legacy misspellings such as `SoureceLocationId` and `FtpSoureceLocationId`).

- `FileTransferValidationException : Exception`
  - Provide the same constructor overloads and property behavior as legacy:
    - `FieldName: string`
    - `ValidationErrorMessage: string`
  - Do not depend on WCF (`System.ServiceModel`) or legacy `IFaultGenerator`.

- `FileTransfertBASValidationException`
  - Provide a simple DTO with the same property names and constructor overloads as legacy:
    - `FieldName: string`
    - `ValidationErrorMessage: string`

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.ExcludedFileExtensions` | `ExcludedFileExtensions` | Preserve enum members for parity with legacy validation behavior. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferExceptionConstants` | `FileTransferExceptionConstants` | Preserve constant names and values exactly. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferValidationException` | `FileTransferValidationException` | Preserve constructors and properties; remove WCF/legacy interface dependencies. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransfertBASValidationException` | `FileTransfertBASValidationException` | Preserve property names for parity with legacy typed validation detail payloads. |
| `FileTransferValidationException.GenerateFault()` | `drop: WCF fault generation is not used in target` | Target validation failures surface as RFC 7807 Problem Details per `technical/target/deployables/fxfer-domain-api/CONTRACT.md`. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (validation failures surfaced as RFC 7807 Problem Details with field-level errors)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (OpenAPI used as the contract format; this component declares no HTTP surface)

