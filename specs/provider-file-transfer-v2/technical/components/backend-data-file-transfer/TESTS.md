---
type: Test Plan
title: technical/components/backend-data-file-transfer/TESTS
description: The tests that prove the backend-data-file-transfer component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-file-transfer
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:21:12Z
sources:
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-data-file-transfer/INVENTORY.yaml
  title: Every legacy class and method (11 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-file-transfer
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 11
  methods: 1
  owned_rules: 2
  flows:
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-bff
  folder: src/Contracts
  body_sha256: f6e1bce5472660878e72f404cf009fd2a525de03ecc52b6c789e114ab42def2b
---

﻿# Tests: `backend-data-file-transfer`

## Owned rule tests

- `F02_042_missing_search_fields_treated_as_empty_strings`
  - Given a search-fields list that does not contain `FtpServerName`, `FtpDescription`, or `FtpUser`
  - When `SearchHelper.GetFieldValue(searchFields, <fieldName>)` is called for each of those names
  - Then the result is `""` (empty string)

- `F02_044_field_lookup_returns_first_match_as_string_or_empty`
  - Given `searchFields = [{ FieldName = "FtpServerName", FieldValue = "ACME" }, { FieldName = "FtpServerName", FieldValue = "SECOND" }]`
  - When `SearchHelper.GetFieldValue(searchFields, "FtpServerName")` is called
  - Then the result is `"ACME"`
  - And when `SearchHelper.GetFieldValue(searchFields, "DoesNotExist")` is called
  - Then the result is `""`

## Edge-case/unit tests

- `SearchHelper_returns_empty_string_when_search_fields_is_empty`
  - Given `searchFields = []`
  - When `GetFieldValue` is called
  - Then the result is `""`

## Contract tests

- Validate `technical/components/backend-data-file-transfer/openapi.yaml` parses as OpenAPI 3.x and includes:
  - `paths: {}`
  - `x-no-http-surface: true`

- Validate `technical/components/backend-data-file-transfer/CONTRACT.md`:
  - lists the DTOs offered by this component
  - lists no component dependencies