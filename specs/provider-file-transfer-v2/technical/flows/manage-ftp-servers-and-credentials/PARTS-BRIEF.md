---
type: Generation Brief
title: 'Brief: split flow manage-ftp-servers-and-credentials into parts'
description: Instructions the tool gave the writer session for splitting this flow into parts.
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
tags:
- api
- flow
- provider-file-transfer-v2
- wcf
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T08:47:43Z
sources:
- id: door-file-transfer-svc-query-service-get-ftp-servers
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L35
  title: GetFtpServers (wcf door)
- id: door-file-transfer-svc-query-service-get-ftp-servers-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L26
  title: GetFtpServers (wcf door)
- id: door-file-transfer-svc-query-service-get-ftp-servers-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L26
  title: GetFtpServers (wcf door)
- id: door-file-transfer-svc-query-service-controller-get-ftp-servers
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L33
  title: GetFtpServers (api door)
- id: door-file-transfer-svc-service-add-ftp-server
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L43
  title: AddFtpServer (wcf door)
- id: door-file-transfer-svc-service-add-ftp-server-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L44
  title: AddFtpServer (wcf door)
- id: door-file-transfer-svc-service-add-ftp-server-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L43
  title: AddFtpServer (wcf door)
- id: door-file-transfer-svc-service-controller-add-ftp-server
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L40
  title: AddFtpServer (api door)
- id: door-file-transfer-svc-service-update-ftp-server
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L49
  title: UpdateFtpServer (wcf door)
- id: door-file-transfer-svc-service-update-ftp-server-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L50
  title: UpdateFtpServer (wcf door)
- id: door-file-transfer-svc-service-update-ftp-server-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L49
  title: UpdateFtpServer (wcf door)
- id: door-file-transfer-svc-service-controller-update-ftp-server
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L46
  title: UpdateFtpServer (api door)
- id: door-file-transfer-svc-service-invalidate-ftp-server
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L61
  title: InvalidateFtpServer (wcf door)
- id: door-file-transfer-svc-service-invalidate-ftp-server-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L56
  title: InvalidateFtpServer (wcf door)
- id: door-file-transfer-svc-service-invalidate-ftp-server-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L61
  title: InvalidateFtpServer (wcf door)
- id: door-file-transfer-svc-service-controller-invalidate-ftp-server
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L52
  title: InvalidateFtpServer (api door)
- id: door-file-transfer-controller-get-ftp-server-details
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L111
  title: GetFtpServerDetails (api door)
- id: door-file-transfer-controller-get-ftpserver-credentials
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L26
  title: GetFTPServerCredentials (api door)
- id: door-file-transfer-controller-add-ftpserver-credentials
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L43
  title: AddFTPServerCredentials (api door)
- id: door-file-transfer-controller-create-ftpserver-credentials
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L52
  title: CreateFTPServerCredentials (api door)
- id: door-file-transfer-controller-update-ftpserver-credentials
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L68
  title: UpdateFTPServerCredentials (api door)
- id: footprint
  resource: /technical/flows/manage-ftp-servers-and-credentials/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:manage-ftp-servers-and-credentials
  generation:
    status: done
    attempts: 2
  rule_prefix: F02
  rules: 63
  doors:
  - file-transfer-svc-query-service-get-ftp-servers
  - file-transfer-svc-query-service-get-ftp-servers-2
  - file-transfer-svc-query-service-get-ftp-servers-3
  - file-transfer-svc-query-service-controller-get-ftp-servers
  - file-transfer-svc-service-add-ftp-server
  - file-transfer-svc-service-add-ftp-server-2
  - file-transfer-svc-service-add-ftp-server-3
  - file-transfer-svc-service-controller-add-ftp-server
  - file-transfer-svc-service-update-ftp-server
  - file-transfer-svc-service-update-ftp-server-2
  - file-transfer-svc-service-update-ftp-server-3
  - file-transfer-svc-service-controller-update-ftp-server
  - file-transfer-svc-service-invalidate-ftp-server
  - file-transfer-svc-service-invalidate-ftp-server-2
  - file-transfer-svc-service-invalidate-ftp-server-3
  - file-transfer-svc-service-controller-invalidate-ftp-server
  - file-transfer-controller-get-ftp-server-details
  - file-transfer-controller-get-ftpserver-credentials
  - file-transfer-controller-add-ftpserver-credentials
  - file-transfer-controller-create-ftpserver-credentials
  - file-transfer-controller-update-ftpserver-credentials
  tables: 1
  procedures: 0
  components:
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  - backend-configure-source-locations-and-destinations-api
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - backend-data-file-transfer
  - data-data-access-entities
  - data-data-access-entities-3
  - shared-contracts-commands
  - shared-contracts-utilities
  - ui-configure-source-locations-and-destinations-screens
  - ui-manage-ftp-servers-and-credentials-screens
  body_sha256: a7316d8d881aac3a901733b28aa295c7a90fe61eb21e6e0c275ca03dfa81abc3
---

# Brief: split flow `manage-ftp-servers-and-credentials` into parts

- Business name: **Manage FTP servers and credentials**
- 21 doors; a part holds at most 12, so at least 2 parts
- Write exactly: `technical/flows/manage-ftp-servers-and-credentials/PARTS.yaml`
- Playbook: `inputs/playbook/parts.md`
- What each door reaches: `technical/doors/<door id>.yaml`; the flow's map: `technical/flows/manage-ftp-servers-and-credentials/TECH.md`

## Doors

| Door id | Kind | Name | Class | Where | Endpoints |
| --- | --- | --- | --- | --- | --- |
| `file-transfer-svc-query-service-get-ftp-servers` | wcf | GetFtpServers | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:35` | - |
| `file-transfer-svc-query-service-get-ftp-servers-2` | wcf | GetFtpServers | HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:26` | - |
| `file-transfer-svc-query-service-get-ftp-servers-3` | wcf | GetFtpServers | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:26` | - |
| `file-transfer-svc-query-service-controller-get-ftp-servers` | api | GetFtpServers | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:33` | POST /FileTransferSvcQueryService |
| `file-transfer-svc-service-add-ftp-server` | wcf | AddFtpServer | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:43` | - |
| `file-transfer-svc-service-add-ftp-server-2` | wcf | AddFtpServer | HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:44` | - |
| `file-transfer-svc-service-add-ftp-server-3` | wcf | AddFtpServer | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:43` | - |
| `file-transfer-svc-service-controller-add-ftp-server` | api | AddFtpServer | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:40` | POST /FileTransferSvcService |
| `file-transfer-svc-service-update-ftp-server` | wcf | UpdateFtpServer | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:49` | - |
| `file-transfer-svc-service-update-ftp-server-2` | wcf | UpdateFtpServer | HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:50` | - |
| `file-transfer-svc-service-update-ftp-server-3` | wcf | UpdateFtpServer | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:49` | - |
| `file-transfer-svc-service-controller-update-ftp-server` | api | UpdateFtpServer | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:46` | POST /FileTransferSvcService |
| `file-transfer-svc-service-invalidate-ftp-server` | wcf | InvalidateFtpServer | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:61` | - |
| `file-transfer-svc-service-invalidate-ftp-server-2` | wcf | InvalidateFtpServer | HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:56` | - |
| `file-transfer-svc-service-invalidate-ftp-server-3` | wcf | InvalidateFtpServer | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:61` | - |
| `file-transfer-svc-service-controller-invalidate-ftp-server` | api | InvalidateFtpServer | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:52` | POST /FileTransferSvcService |
| `file-transfer-controller-get-ftp-server-details` | api | GetFtpServerDetails | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:111` | - |
| `file-transfer-controller-get-ftpserver-credentials` | api | GetFTPServerCredentials | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:26` | - |
| `file-transfer-controller-add-ftpserver-credentials` | api | AddFTPServerCredentials | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:43` | - |
| `file-transfer-controller-create-ftpserver-credentials` | api | CreateFTPServerCredentials | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:52` | - |
| `file-transfer-controller-update-ftpserver-credentials` | api | UpdateFTPServerCredentials | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:68` | - |
