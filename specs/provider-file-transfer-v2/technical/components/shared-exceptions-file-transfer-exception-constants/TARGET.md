---
type: Target Placement
title: 'Target: shared-exceptions-file-transfer-exception-constants'
description: Where the shared-exceptions-file-transfer-exception-constants component goes in the new system and what to build, method by method.
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
  body_sha256: 7066c7b8a359999fa4aa79f2130ba96981ec6d14a73b943dd1dbc0225c7b74e7
---

# Target: `shared-exceptions-file-transfer-exception-constants`

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Contracts/Exceptions`

## What to create

Create a C# contract type under the `Fxfer.DomainApi.Contracts.Exceptions` namespace that preserves the legacy constant-group
structure and values used by validation and SOAP-fault logging.

- `FileTransferExceptionConstants`
  - Provide the nested constant groups mirroring legacy:
    - `Validation` (validation field/code keys and default message)
    - `Fields` (legacy field-name strings)
    - `FaultExceptions` (fault log prefixes/messages)
  - Preserve constant names and values exactly, including legacy misspellings such as `SoureceLocationId` and `FtpSoureceLocationId`.
  - Preserve the trailing space in `FaultExceptions.CustomFault`.

## Mapping

This inventory has no methods; mapping is class-to-class.

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferExceptionConstants.Validation` | `Fxfer.DomainApi.Contracts.Exceptions.FileTransferExceptionConstants.Validation` | Nested constant group; preserve names/values exactly. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferExceptionConstants.Fields` | `Fxfer.DomainApi.Contracts.Exceptions.FileTransferExceptionConstants.Fields` | Nested constant group; preserve names/values exactly. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions.FileTransferExceptionConstants.FaultExceptions` | `Fxfer.DomainApi.Contracts.Exceptions.FileTransferExceptionConstants.FaultExceptions` | Nested constant group; preserve names/values exactly. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (validation keys are preserved so field-level validation details can be surfaced consistently in the target API error format)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (OpenAPI is the contract format for REST APIs; this component declares no HTTP surface in `openapi.yaml`)

