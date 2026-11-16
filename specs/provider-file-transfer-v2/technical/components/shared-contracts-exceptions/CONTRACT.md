---
type: Interface Contract
title: technical/components/shared-contracts-exceptions/CONTRACT
description: What the shared-contracts-exceptions component offers other components and what it uses from them.
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
  body_sha256: c111df728bd5745692b4c0d28655b766664401fc7b3306473adbbd71479f4f90
---

﻿# Contract: `shared-contracts-exceptions`

This component provides shared exception/validation contract types and constant strings used across FXFER to represent validation failures in a consistent, machine-readable way.

## Offers

- `ExcludedFileExtensions.ExcludedFileExtensionList` (disallowed extension enum)
- `FileTransferExceptionConstants.Validation` (validation keys/codes and default message)
- `FileTransferExceptionConstants.Fields` (legacy field-name constants)
- `FileTransferExceptionConstants.FaultExceptions` (legacy fault log prefixes)
- `FileTransferValidationException` (exception carrying `FieldName` and `ValidationErrorMessage`)
- `FileTransfertBASValidationException` (validation error detail payload DTO)

## Uses

None (no component dependencies).

## Data owned

None (contracts/constants only; no storage).

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (standardize validation errors into a consistent, machine-readable structure)
