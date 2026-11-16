---
type: Test Plan
title: 'Tests: backend-data-file-transfer-constants'
description: The tests that prove the backend-data-file-transfer-constants component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-file-transfer-constants
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:29:46Z
sources:
- id: inventory
  resource: /technical/components/backend-data-file-transfer-constants/INVENTORY.yaml
  title: Every legacy class and method (16 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-file-transfer-constants
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 16
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants
  body_sha256: 538063e60586adf4351431b8ce06a245dc031ce654253aac3551715764888d52
---

# Tests: `backend-data-file-transfer-constants`

## Owned rule tests

None.

## Edge-case/unit tests

- `FileTransferConstantsBackend_exports_expected_constants`
  - Given the TypeScript module `apps/fxfer-mfe/src/legacy-constants/file-transfer-constants-backend.ts`
  - When `FILE_TRANSFER_CONSTANTS_BACKEND` is imported
  - Then the exported object matches the expected constant values (snapshot or explicit assertions), including at minimum:
    - `BusinessValidationExceptions.DuplicatedFTPServerName == DuplicatedFtpServerName`
    - `Ioc.ComponentNames.FileTransferDataProvider == FileTransferDataProvider`
    - `Localization.DataLists.FTPServer == FileTransfer.DataList.FTPServer`
    - `Localization.Messages.SaveSuccess == FileTransfer.Msg.SaveSuccess`
    - `MVC.Actions.SearchFTPServerCredentials == SearchFTPServerCredentials`
    - `WebApi.Actions.GetFtsServerDetails == GetFtpServerDetails`
    - `Xml.URI.NamespaceFileTransfer == urn:hp.com:hsp:ua3:FileTransfer:schemas`

## Contract tests

- Validate `technical/components/backend-data-file-transfer-constants/openapi.yaml` parses as OpenAPI 3.x and includes:
  - `paths: {}`
  - `x-no-http-surface: true`

- Validate `technical/components/backend-data-file-transfer-constants/CONTRACT.md`:
  - lists `FILE_TRANSFER_CONSTANTS_BACKEND` under Offers
  - lists no component dependencies

