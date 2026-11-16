---
type: Flow Specification
title: technical/flows/manage-integration-files/FLOW
description: '# Manage integration file definitions'
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
tags:
- flow
- provider-file-transfer-v2
- wcf
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T06:55:41Z
sources:
- id: door-file-transfer-svc-query-service-get-integration-file
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L50
  title: GetIntegrationFile (wcf door)
- id: door-file-transfer-svc-query-service-get-integration-file-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L41
  title: GetIntegrationFile (wcf door)
- id: door-file-transfer-svc-service-update-integration-file
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L55
  title: UpdateIntegrationFile (wcf door)
- id: door-file-transfer-svc-service-update-integration-file-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L55
  title: UpdateIntegrationFile (wcf door)
- id: src-5
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business/CommandHandlers/FtsIntegrationFileCommandHandler.cs
  title: FtsIntegrationFileCommandHandler.cs (2 methods)
- id: footprint
  resource: /technical/flows/manage-integration-files/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:manage-integration-files
  generation:
    status: needs-review
    attempts: 2
    problems: 1
  rule_prefix: F03
  rules: 13
  doors:
  - file-transfer-svc-query-service-get-integration-file
  - file-transfer-svc-query-service-get-integration-file-2
  - file-transfer-svc-service-update-integration-file
  - file-transfer-svc-service-update-integration-file-2
  tables: 1
  procedures: 0
  components:
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  - backend-configure-source-locations-and-destinations-api
  - shared-contracts-commands
  body_sha256: dd3bcff8f885f893229c6bd7f45efbc776ce06afd7266d0ee80ea2aa5230db2c
---

﻿# Manage integration file definitions

This flow lets another system look up active integration file definitions and record processing outcomes against an existing definition. The lookup returns the stored details for each active definition, and the update records a configuration number, processed status, and whether the definition should remain active.

## Who starts it

Another system that runs or tracks file transfer processing.

## Doors

| Door | Request | Answers |
| --- | --- | --- |
| `file-transfer-svc-query-service-get-integration-file` | `GetIntegrationFileQuery` | `GetIntegrationFileQuery` (with `Results`) |
| `file-transfer-svc-query-service-get-integration-file-2` | `GetIntegrationFileQuery` | `GetIntegrationFileQuery` (with `Results`) |
| `file-transfer-svc-service-update-integration-file` | `UpdateIntegrationFileCommand` | `IntegrationFileUpdated` |
| `file-transfer-svc-service-update-integration-file-2` | `UpdateIntegrationFileCommand` | `IntegrationFileUpdated` |

## Steps

1. A caller asks for integration file definitions, optionally narrowing by a single integration file id and/or a file name fragment. (F03-001, F03-002, F03-006, F03-007)
2. The system returns only active definitions, filtered by the optional criteria. (F03-005, F03-006, F03-007)
3. A caller updates an integration file definition with a configuration number and an active/inactive flag. (F03-003, F03-004, F03-010, F03-011)
4. The system records the update on the stored definition: configuration number, processed status, active flag, operator id, and last-modified time. (F03-012)

## Outcome

- Lookup example: when no criteria are supplied (integration file id is `00000000-0000-0000-0000-000000000000` and the name is empty), the caller receives all active integration file definitions.
- Update example: when a caller supplies an existing integration file id and a configuration number like `123`, the stored definition is marked processed and its configuration number is recorded.

## Errors

- An update request without a configuration number fails with a business validation error. (F03-010)