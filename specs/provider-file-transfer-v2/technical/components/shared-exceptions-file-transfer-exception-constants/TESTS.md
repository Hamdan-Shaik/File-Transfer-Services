---
type: Test Plan
title: 'Tests: shared-exceptions-file-transfer-exception-constants'
description: The tests that prove the shared-exceptions-file-transfer-exception-constants component, one or more per business rule.
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
  body_sha256: 54bea183eb760334ba163b5f9bb7735b1854936dac4524a358dd272e1af4fdd0
---

# Tests: `shared-exceptions-file-transfer-exception-constants`

## Owned rule tests

None.

## Edge-case/unit tests

- `FileTransferExceptionConstants_exports_expected_constants`
  - Given the C# type `Fxfer.DomainApi.Contracts.Exceptions.FileTransferExceptionConstants`
  - When the constant values are referenced
  - Then the exported constant groups match the expected legacy values (snapshot or explicit assertions), including at minimum:
    - `Validation.Message` equals `Business Error occurred processing the command.`
    - `Validation.ProcessingSystem` equals `FileTransfer`
    - `Fields.FtsSourceLocationSeq` equals `FTS_SOURCE_LOCATION_SEQ`
    - `FaultExceptions.CustomFault` equals `BAS File Transfer Service Application Error Custom Fault ` (includes trailing space)

## Contract tests

- Validate `technical/components/shared-exceptions-file-transfer-exception-constants/openapi.yaml` parses as OpenAPI 3.x and includes:
  - `paths: {}`
  - `x-no-http-surface: true`

- Validate `technical/components/shared-exceptions-file-transfer-exception-constants/CONTRACT.md`:
  - lists `FileTransferExceptionConstants` under Provides
  - lists no component dependencies under Uses

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-035-contract-testing-pattern.md` section `Solution Overview` (contract tests verify the documented contract artifacts stay consistent)

