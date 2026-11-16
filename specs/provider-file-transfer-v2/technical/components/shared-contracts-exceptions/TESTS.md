---
type: Test Plan
title: technical/components/shared-contracts-exceptions/TESTS
description: The tests that prove the shared-contracts-exceptions component, one or more per business rule.
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
  body_sha256: cb57302d4dd08acd8bdc713b770ddd699cd374ea17c48827bc178e20764f14b0
---

﻿# Tests: `shared-contracts-exceptions`

## Owned rule tests

None.

## Edge-case/unit tests

- `ExcludedFileExtensions_contains_expected_disallowed_extensions`
  - Given `ExcludedFileExtensions.ExcludedFileExtensionList`
  - When the set of enum members is inspected
  - Then it contains at minimum: `exe`, `dll`, `bat`, `cmd`, `ps1`, `js`, `py`, `sh`, `msc`.

- `FileTransferValidationException_sets_properties`
  - Given `new FileTransferValidationException(fieldName: "ConfigId", validationMessage: "MissingSearchCriteria", message: "")`
  - When the exception is inspected
  - Then `FieldName == "ConfigId"` and `ValidationErrorMessage == "MissingSearchCriteria"`.

- `FileTransfertBASValidationException_sets_properties`
  - Given `new FileTransfertBASValidationException(fieldName: "ConfigId", message: "MissingSearchCriteria")`
  - When the object is inspected
  - Then `FieldName == "ConfigId"` and `ValidationErrorMessage == "MissingSearchCriteria"`.

## Contract tests

- Validate `technical/components/shared-contracts-exceptions/openapi.yaml` parses as OpenAPI 3.x and includes:
  - `paths: {}`
  - `x-no-http-surface: true`

- Validate `technical/components/shared-contracts-exceptions/CONTRACT.md`:
  - lists `FileTransferValidationException` and `FileTransfertBASValidationException` under Offers
  - lists no component dependencies under Uses
