---
type: Test Plan
title: technical/components/shared-common-common-constants/TESTS
description: The tests that prove the shared-common-common-constants component, one or more per business rule.
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
  body_sha256: 10e6fb5e012bf223b2a6896f6657c1e430a889f58d00ad477d20a11133418f2c
---

﻿# Tests: `shared-common-common-constants`

## Owned rule tests

None.

## Edge-case/unit tests

- `CommonConstants_exposes_expected_constant_values`
  - Given the C# type `CommonConstants` in `services/fxfer-domain-api/src/Domain/Constants/CommonConstants.cs`
  - When the type is referenced by domain/validation logic
  - Then the constants match the expected legacy values, including at minimum:
    - `CommonConstants.FtpServerCode == "1"`
    - `CommonConstants.ModuleName == "FileTransfer"`
    - `CommonConstants.RegexPatterns.IpAddressPattern == @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b"`
    - `CommonConstants.Validation.BusinessErrorCode == "E"`
    - `CommonConstants.Validation.ErrorType.InvalidParameters == "InvalidSearchParameters"`
    - `CommonConstants.Validation.Messages.MissingConfigId == "Missing ConfigId"`
    - `CommonConstants.Validation.Properties.RunSchedule == "Run Schedule"`
    - `CommonConstants.SortFields.FtpServer.FtpServerName == "FtpServerName"`

## Contract tests

- Validate `technical/components/shared-common-common-constants/openapi.yaml` parses as OpenAPI 3.x and includes:
  - `paths: {}`
  - `x-no-http-surface: true`

- Validate `technical/components/shared-common-common-constants/CONTRACT.md`:
  - lists `CommonConstants` under Offers
  - lists no component dependencies
