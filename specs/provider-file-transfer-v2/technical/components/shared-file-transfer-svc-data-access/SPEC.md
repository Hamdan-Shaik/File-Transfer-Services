---
type: Behavior Specification
title: technical/components/shared-file-transfer-svc-data-access/SPEC
description: 'What the shared-file-transfer-svc-data-access component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 8ae8251e705f189c2d4dcb9abe385e8d8b7edc0a3296b1c15135a8993682b622
---

﻿# shared-file-transfer-svc-data-access: legacy data-access constants (FileTransferSvc)

This component contains legacy constant groups used by the File Transfer service data-access layer for configuration keys and fixed code values.
The constants are referenced by data-access code when resolving connection/configuration settings, recording error-log fields, and encoding file-delivery event outcomes.

## DataAccessConstants
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.DataAccessConstants`

Constants for data-access configuration keys and file status codes.

- `DefaultConnection` = `DefaultConnection`
- `DefaultConnectionProvider` = `DefaultConnectionProvider`
- `DefaultConnectionString` = `DefaultConnectionString`

- Nested group: `FileStatusConstant`
  - `Completed` = `1`
  - `Error` = `2`
  - `Transfering` = `3`
  - `UNCProtocolCD` = `3`

## ErrorLogConstants
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.ErrorLogConstants`

Constants representing the canonical property names used when capturing/serializing error log details.

- `ExceptionType` = `ExceptionType`
- `Message` = `Message`
- `MethodName` = `MethodName`
- `StackTrace` = `StackTrace`

## FileDeliveryEventConstants
Legacy type: `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.FileDeliveryEventConstants`

Constants representing file-delivery event codes.

- `Renamed` = `R`
- `Transferred` = `T`
- `Archieved` = `A`

## Rules implemented

None.

## Not carried over

None.
