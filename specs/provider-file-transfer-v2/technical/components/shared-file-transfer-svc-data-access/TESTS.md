---
type: Test Plan
title: technical/components/shared-file-transfer-svc-data-access/TESTS
description: The tests that prove the shared-file-transfer-svc-data-access component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-file-transfer-svc-data-access
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:14:49Z
sources:
- id: inventory
  resource: /technical/components/shared-file-transfer-svc-data-access/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-file-transfer-svc-data-access
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
  folder: src/Infrastructure/DataAccess/Constants
  body_sha256: 69e2a83c5e80023beb1dbe349bed06a5e21ed32ed89ca45cf2e3ecaa1c935dcf
---

﻿# Tests: `shared-file-transfer-svc-data-access`

## Owned rule tests

None.

## Edge-case/unit tests

- `DataAccessConstants_exposes_expected_constant_values`
  - Given the C# type `DataAccessConstants` in `services/fxfer-domain-api/src/Infrastructure/DataAccess/Constants/DataAccessConstants.cs`
  - When data-access code references the constants
  - Then the constants match the expected legacy values, including at minimum:
    - `DataAccessConstants.DefaultConnection == "DefaultConnection"`
    - `DataAccessConstants.DefaultConnectionProvider == "DefaultConnectionProvider"`
    - `DataAccessConstants.DefaultConnectionString == "DefaultConnectionString"`
    - `DataAccessConstants.FileStatusConstant.Completed == "1"`
    - `DataAccessConstants.FileStatusConstant.Error == "2"`
    - `DataAccessConstants.FileStatusConstant.Transfering == "3"`
    - `DataAccessConstants.FileStatusConstant.UNCProtocolCD == "3"`

- `ErrorLogConstants_exposes_expected_constant_values`
  - Given the C# type `ErrorLogConstants` in `services/fxfer-domain-api/src/Infrastructure/DataAccess/Constants/ErrorLogConstants.cs`
  - When error logging/serialization uses the constants
  - Then the constants match the expected legacy values, including at minimum:
    - `ErrorLogConstants.ExceptionType == "ExceptionType"`
    - `ErrorLogConstants.Message == "Message"`
    - `ErrorLogConstants.MethodName == "MethodName"`
    - `ErrorLogConstants.StackTrace == "StackTrace"`

- `FileDeliveryEventConstants_exposes_expected_constant_values`
  - Given the C# type `FileDeliveryEventConstants` in `services/fxfer-domain-api/src/Infrastructure/DataAccess/Constants/FileDeliveryEventConstants.cs`
  - When file-delivery outcomes are encoded
  - Then the constants match the expected legacy values, including at minimum:
    - `FileDeliveryEventConstants.Renamed == "R"`
    - `FileDeliveryEventConstants.Transferred == "T"`
    - `FileDeliveryEventConstants.Archieved == "A"`

## Contract tests

- Validate `technical/components/shared-file-transfer-svc-data-access/openapi.yaml` parses as OpenAPI 3.x and includes:
  - `paths: {}`
  - `x-no-http-surface: true`

- Validate `technical/components/shared-file-transfer-svc-data-access/CONTRACT.md`:
  - lists `DataAccessConstants`, `ErrorLogConstants`, and `FileDeliveryEventConstants` under Offers
  - lists no component dependencies
