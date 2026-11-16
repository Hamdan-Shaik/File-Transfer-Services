---
type: Data Access
title: technical/flows/manage-integration-files/DATA
description: Tables, stored procedures and data the flow reads and writes.
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
  body_sha256: 962f862f351599ae90697d35b7639ff878d97e01bd8346375b3a781a06d36c02
---

﻿# Data: Manage integration file definitions

## Tables

### `FTS_INTEGRATION_FILE` (`orm:FTS_INTEGRATION_FILE`)

- Reads (lookup): returns only rows where `IS_VALID` is true, and optionally narrows by id and/or file name.
  - Optional id filter: when an id is provided and is not `00000000-0000-0000-0000-000000000000`, the lookup matches `FTS_INTEGRATION_FILE_ID` exactly.
  - Optional name filter: when a name fragment is provided, the lookup matches rows whose `FILE_NAME` contains that fragment.
- Writes (update): updates an existing row identified by `FTS_INTEGRATION_FILE_ID` and sets:
  - `CONFIG_ID` (from the update)
  - `IS_PROCESSED` = true
  - `IS_VALID` (from the update)
  - `LAST_MODIFIED_TS` = current UTC time
  - `OPERATOR_ID` (from the update requestor)

Key columns used by this flow:

- `FTS_INTEGRATION_FILE_ID`: the integration file id
- `GROUP_ID`: group id
- `ETF_OUTBOUND_METADATA_DEFN_ID`: outbound metadata definition id
- `CONFIG_ID`: configuration id
- `FILE_NAME`: file name
- `IS_PROCESSED`: processed flag
- `OPERATOR_ID`: operator id
- `IS_VALID`: active flag
- `CREATED_TS`: created time (not returned by this lookup)
- `LAST_MODIFIED_TS`: last modified time (not returned by this lookup)

## Stored procedures

None.

## Answers

### Lookup answer: `GetIntegrationFileQuery`

- `Where`
  - `FTSIntegrationFileId`: the integration file id to look up; default is empty id (no id filtering)
  - `FileName`: a fragment to match within the stored file name
- `Results[]` items are `FtsIntegrationFileModel` objects populated with:
  - `FtsIntegrationFileId`
  - `GroupId`
  - `ETFOutboundMetadataDefnId`
  - `ConfigId`
  - `FileName`
  - `IsProcessed`

Fields that exist on `FtsIntegrationFileModel` but are not populated by this lookup (and therefore remain defaults):

- `IsValid`
- `CreationDate`
- `LastModifiedDate`

### Update answer: `IntegrationFileUpdated`

- Emitted with:
  - `FtsIntegrationFileId`
  - `FileName` (provided by the caller)
  - `ConfigId`
  - `IsValid`
  - `OperatorId`

The projection that applies the update does not use the event's `FileName` value when writing `FTS_INTEGRATION_FILE`.