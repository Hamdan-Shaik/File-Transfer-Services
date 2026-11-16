---
type: Test Plan
title: technical/components/shared-common-constants-validation/TESTS
description: The tests that prove the shared-common-constants-validation component, one or more per business rule.
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
  body_sha256: b2ea7488b2f57a2ec2cc2cbcbb02b59828484dabf3bd83a91bd8c0dd5a154ade
---

﻿# Tests: `shared-common-constants-validation`

## Owned rule tests

None.

## Edge-case/unit tests

- `ValidationConstants_exports_expected_constants`
  - Given the C# type `ValidationConstants`
  - When the nested constant groups are referenced
  - Then the constant values match the legacy values, including at minimum:
    - `ValidationConstants.ErrorType.InvalidParameters == "InvalidSearchParameters"`
    - `ValidationConstants.Messages.DuplicatedFtpServerName == "Duplicated FTP Server Name"`
    - `ValidationConstants.Properties.RunSchedule == "Run Schedule"`

## Contract tests

- Validate `technical/components/shared-common-constants-validation/openapi.yaml` parses as OpenAPI 3.x and includes:
  - `paths: {}`
  - `x-no-http-surface: true`

- Validate `technical/components/shared-common-constants-validation/CONTRACT.md`:
  - lists `ValidationConstants` under Offers
  - lists no component dependencies
