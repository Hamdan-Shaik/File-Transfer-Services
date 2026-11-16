---
type: Flow Links
title: technical/flows/manage-integration-files/LINKS
description: Shared pieces, other flows and external packages the flow relies on.
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
  body_sha256: 8437a669cc981f545f1325aab6e3cc61dd02a0e85e472c23dbb7ed6b53139229
---

﻿# Links: Manage integration file definitions

## Shared pieces used

None.

## Related flows

None.

## Contracts

- `hpp.core.bas.cqrs` 30.0.0: `HPP.Core.BAS.CQRS.Base.QueryServiceBase.TryRoute<TQuery>(TQuery)`
  - Used by `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:48` with `query=<query>`.
  - This flow relies on the router returning the same query object with `Results` populated.

- `hpp.core.bas.cqrs` 30.0.0: `HPP.Core.BAS.CQRS.Base.CommandServiceBase.TryRoute(HPP.Core.BAS.CQRS.Base.Command)`
  - Used by `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:71` with `command=<command>`.
  - This flow relies on the router returning an `IntegrationFileUpdated` event for a successful update.

- (package not identified in the graph): `QueryServiceBase.TryRoute`
  - Used by `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:57` with `query=<query>`.
  - This flow relies on the router returning the same query object with `Results` populated.

- (package not identified in the graph): `CommandServiceBase.TryRoute`
  - Used by `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:71` with `command=<command>`.
  - This flow relies on the router returning an `IntegrationFileUpdated` event for a successful update.