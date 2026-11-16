---
type: Shared Pieces List
title: 'provider-file-transfer-v2: shared pieces'
description: A piece is shared when at least 8 of the 77 doors in the whole system (10%) reach it.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T06:21:10Z
cef:
  pack: provider-file-transfer-v2
  body_sha256: 40026dca26945e0f6ac1b64d5d9e6632b0c36e388f64833b0328dc957ec2f89b
---

# provider-file-transfer-v2: shared pieces

A piece is shared when at least 8 of the 77 doors in the whole system (10%) reach it. A flow's footprint lists the shared pieces it uses and stops there; each shared piece gets one spec of its own.

Confirm each piece: is it really shared, and is its home right?

## Home here (21): this module's own code that the rest of the system also uses

These stay inside this module's flows and get their spec in this pack's `technical/` components.

| Piece | Kind | Doors reaching it | Used by flows | Confirm |
| --- | --- | ---: | --- | --- |
| `orm:FTP_SERVER` | Table | 33 (43%) | `file-transfer-svc-query-service-1` | [ ] |
| `orm:FTS_SUSPENSION` | Table | 16 (21%) | `file-transfer-svc-query-service-1` | [ ] |
| `HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService` | Class | 13 (17%) | `file-transfer-svc-query-service-1`, `file-transfer-svc-service-2` | [ ] |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers.FtsSourceLocationProjectionWriter` | Class | 12 (16%) | `file-transfer-svc-query-service-1` | [ ] |
| `HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers.FtsSourceLocationProjectionWriter` | Class | 12 (16%) | `file-transfer-svc-query-service-1` | [ ] |
| `orm:FTS_DESTINATION` | Table | 12 (16%) | `file-transfer-svc-query-service-1` | [ ] |
| `HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService` | Class | 11 (14%) | `file-transfer-svc-query-service-1`, `file-transfer-svc-query-service-11`, `file-transfer-svc-query-service-14`, `file-transfer-svc-query-service-17`, `file-transfer-svc-query-service-5`, `file-transfer-svc-query-service-8`, `file-transfer-svc-query-service-controller-1`, `file-transfer-svc-query-service-controller-2`, `file-transfer-svc-query-service-controller-3`, `file-transfer-svc-query-service-controller-4` | [ ] |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtpServerCommandHandler` | Class | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsSourceLocationAddCommandHandler` | Class | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.AddFtpServerCommand` | Class | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FileTransferCommand` | Class | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsCommand` | Class | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.BusinessErrorUtility` | Class | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Repository.FileTransferReadOnlyRepository` | Class | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Repository.IFileTransferReadOnlyRepository` | Interface | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService` | Class | 8 (10%) | `file-transfer-svc-query-service-1`, `file-transfer-svc-service-1`, `file-transfer-svc-service-4` | [ ] |
| `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtpServerCommandHandler` | Class | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsSourceLocationAddCommandHandler` | Class | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Repository.FileTransferReadOnlyRepository` | Class | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Repository.IFileTransferReadOnlyRepository` | Interface | 8 (10%) | `file-transfer-svc-query-service-1` | [ ] |
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService` | Class | 8 (10%) | `file-transfer-svc-query-service-1`, `file-transfer-svc-service-3`, `file-transfer-svc-service-4` | [ ] |

## From elsewhere (0): plumbing this module uses but does not own

Their specs belong to the modules that own them; flows here only reference them.

| Piece | Kind | Doors reaching it | Used by flows | Confirm |
| --- | --- | ---: | --- | --- |
