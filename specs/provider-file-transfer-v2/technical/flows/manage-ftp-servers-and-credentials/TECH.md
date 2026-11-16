---
type: Technical Flow Map
title: 'Technical map: Manage FTP servers and credentials'
description: 'How the legacy code implements the flow: call chains, classes, data, outside calls.'
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
  body_sha256: ea51cdb3adbefd620f5b5a1b64b7516e0d201eb5bfbe410cf9769ae6f1b837af
---

# Technical map: Manage FTP servers and credentials

How the legacy code implements the flow `manage-ftp-servers-and-credentials`, as the code graph sees it. Written by the tool from `FOOTPRINT.yaml`, not by AI; the business view is in `FLOW.md` and `RULES.md`.

## Doors

| Door | Kind | Where |
| --- | --- | --- |
| `file-transfer-svc-query-service-get-ftp-servers` (GetFtpServers) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:35` |
| `file-transfer-svc-query-service-get-ftp-servers-2` (GetFtpServers) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:26` |
| `file-transfer-svc-query-service-get-ftp-servers-3` (GetFtpServers) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:26` |
| `file-transfer-svc-query-service-controller-get-ftp-servers` (GetFtpServers) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:33` |
| `file-transfer-svc-service-add-ftp-server` (AddFtpServer) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:43` |
| `file-transfer-svc-service-add-ftp-server-2` (AddFtpServer) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:44` |
| `file-transfer-svc-service-add-ftp-server-3` (AddFtpServer) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:43` |
| `file-transfer-svc-service-controller-add-ftp-server` (AddFtpServer) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:40` |
| `file-transfer-svc-service-update-ftp-server` (UpdateFtpServer) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:49` |
| `file-transfer-svc-service-update-ftp-server-2` (UpdateFtpServer) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:50` |
| `file-transfer-svc-service-update-ftp-server-3` (UpdateFtpServer) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:49` |
| `file-transfer-svc-service-controller-update-ftp-server` (UpdateFtpServer) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:46` |
| `file-transfer-svc-service-invalidate-ftp-server` (InvalidateFtpServer) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:61` |
| `file-transfer-svc-service-invalidate-ftp-server-2` (InvalidateFtpServer) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:56` |
| `file-transfer-svc-service-invalidate-ftp-server-3` (InvalidateFtpServer) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:61` |
| `file-transfer-svc-service-controller-invalidate-ftp-server` (InvalidateFtpServer) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:52` |
| `file-transfer-controller-get-ftp-server-details` (GetFtpServerDetails) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:111` |
| `file-transfer-controller-get-ftpserver-credentials` (GetFTPServerCredentials) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:26` |
| `file-transfer-controller-add-ftpserver-credentials` (AddFTPServerCredentials) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:43` |
| `file-transfer-controller-create-ftpserver-credentials` (CreateFTPServerCredentials) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:52` |
| `file-transfer-controller-update-ftpserver-credentials` (UpdateFTPServerCredentials) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:68` |

## Call chains

### `file-transfer-svc-query-service-get-ftp-servers`

- `FileTransferSvcQueryService.GetFtpServers` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:35`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55`
  - reads table `orm:FTP_SERVER`

### `file-transfer-svc-query-service-get-ftp-servers-2`

- `FileTransferSvcQueryService.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:26`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`
  - reads table `orm:FTP_SERVER`

### `file-transfer-svc-query-service-get-ftp-servers-3`

- `FileTransferSvcQueryService.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:26`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46`
  - reads table `orm:FTP_SERVER`

### `file-transfer-svc-query-service-controller-get-ftp-servers`

- `FileTransferSvcQueryServiceController.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:33`
  - `IFileTransferSvcQueryService.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcQueryService.cs:41`
    - implementation: `FileTransferSvcQueryService.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:26`
      - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`
      - reads table `orm:FTP_SERVER`
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-add-ftp-server`

- `FileTransferSvcService.AddFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:43`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69`
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:64`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33`
        - `FtpServerProjectionWriter.AddFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74`
          - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33` (see above)
          - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33`
            - `FtpServerProjectionWriter.AddFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74`
              - writes table `orm:FTP_SERVER`
            - reads table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44`
        - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:66`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95`
          - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
            - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
              - `BusinessErrorUtility.LoadConfigurationMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:42`
          - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` **- walk stops here**
          - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59`
            - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
            - `FileTransferCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:69`
              - implementation: `AddFtpServerCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:211`
                - `AddFtpServerCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
                    - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateFtpServerIp` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231`
                  - `AddFtpServerCommand.ValidateFtpServerIpPattern` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:242`
                    - reads table `orm:FTP_SERVER` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateProxyPort` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                - reads table `orm:FTP_SERVER` (see above)
            - `FileTransferCommand.ValidateRequiredFields` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92`
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
          - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:106`
            - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116`
              - reads table `orm:FTP_SERVER` (see above)
            - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:111`
              - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
                - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
                  - reads table `orm:FTP_SERVER` (see above)
                - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93`
      - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:104`
        - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:109`
          - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
            - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
      - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
      - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-add-ftp-server-2`

- `FileTransferSvcService.AddFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:44`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:64`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33`
        - `FtpServerProjectionWriter.AddFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74`
          - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33` (see above)
          - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33`
            - `FtpServerProjectionWriter.AddFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74`
              - writes table `orm:FTP_SERVER`
            - reads table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44`
        - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:66`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95`
          - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
            - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
              - `BusinessErrorUtility.LoadConfigurationMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:42`
          - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` **- walk stops here**
          - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59`
            - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
            - `FileTransferCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:69`
              - implementation: `AddFtpServerCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:211`
                - `AddFtpServerCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
                    - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateFtpServerIp` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231`
                  - `AddFtpServerCommand.ValidateFtpServerIpPattern` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:242`
                    - reads table `orm:FTP_SERVER` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateProxyPort` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                - reads table `orm:FTP_SERVER` (see above)
            - `FileTransferCommand.ValidateRequiredFields` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92`
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
          - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:106`
            - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116`
              - reads table `orm:FTP_SERVER` (see above)
            - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:111`
              - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
                - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
                  - reads table `orm:FTP_SERVER` (see above)
                - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93`
      - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:104`
        - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:109`
          - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
            - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
      - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
      - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-add-ftp-server-3`

- `FileTransferSvcService.AddFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:43`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69`
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:64`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33`
        - `FtpServerProjectionWriter.AddFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74`
          - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33` (see above)
          - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33`
            - `FtpServerProjectionWriter.AddFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74`
              - writes table `orm:FTP_SERVER`
            - reads table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44`
        - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:66`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95`
          - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
            - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
              - `BusinessErrorUtility.LoadConfigurationMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:42`
          - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` **- walk stops here**
          - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59`
            - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
            - `FileTransferCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:69`
              - implementation: `AddFtpServerCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:211`
                - `AddFtpServerCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
                    - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateFtpServerIp` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231`
                  - `AddFtpServerCommand.ValidateFtpServerIpPattern` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:242`
                    - reads table `orm:FTP_SERVER` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateProxyPort` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                - reads table `orm:FTP_SERVER` (see above)
            - `FileTransferCommand.ValidateRequiredFields` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92`
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
          - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:106`
            - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116`
              - reads table `orm:FTP_SERVER` (see above)
            - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:111`
              - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
                - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
                  - reads table `orm:FTP_SERVER` (see above)
                - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93`
      - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:104`
        - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:109`
          - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
            - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
      - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
      - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-controller-add-ftp-server`

- `FileTransferSvcServiceController.AddFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:40`
  - `IFileTransferSvcService.AddFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcService.cs:135`
    - implementation: `FileTransferSvcService.AddFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:44`
      - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42`
        - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:64`
          - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42` (see above)
          - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33`
            - `FtpServerProjectionWriter.AddFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74`
              - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33` (see above)
              - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33`
                - `FtpServerProjectionWriter.AddFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74`
                  - writes table `orm:FTP_SERVER`
                - reads table `orm:FTP_SERVER` (see above)
              - writes table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44`
            - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:66`
              - reads table `orm:FTP_SERVER` (see above)
            - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95`
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
                - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
                  - `BusinessErrorUtility.LoadConfigurationMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:42`
              - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` **- walk stops here**
              - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59`
                - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
                - `FileTransferCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:69`
                  - implementation: `AddFtpServerCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:211`
                    - `AddFtpServerCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220`
                    - `AddFtpServerCommand.ValidateFtpServerIp` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231`
                    - `AddFtpServerCommand.ValidateProxyPort` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247`
                    - reads table `orm:FTP_SERVER` (see above)
                - `FileTransferCommand.ValidateRequiredFields` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
                    - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
              - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:106`
                - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116`
                  - reads table `orm:FTP_SERVER` (see above)
                - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:111`
                  - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
                    - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
                    - reads table `orm:FTP_SERVER` (see above)
                  - reads table `orm:FTP_SERVER` (see above)
                - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:33` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93`
          - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:104`
            - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114`
              - reads table `orm:FTP_SERVER` (see above)
            - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:109`
              - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
                - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
                  - reads table `orm:FTP_SERVER` (see above)
                - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
          - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
          - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-update-ftp-server`

- `FileTransferSvcService.UpdateFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:49`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69`
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:74`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:38`
        - writes table `orm:FTP_SERVER`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51`
        - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:76`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95`
          - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
            - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
              - `BusinessErrorUtility.LoadConfigurationMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:42`
          - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` **- walk stops here**
          - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59`
            - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
            - `FileTransferCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:69`
              - implementation: `AddFtpServerCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:211`
                - `AddFtpServerCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
                    - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateFtpServerIp` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231`
                  - `AddFtpServerCommand.ValidateFtpServerIpPattern` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:242`
                    - reads table `orm:FTP_SERVER` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateProxyPort` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                - reads table `orm:FTP_SERVER` (see above)
            - `FileTransferCommand.ValidateRequiredFields` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92`
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
          - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:106`
            - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116`
              - reads table `orm:FTP_SERVER` (see above)
            - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:111`
              - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
                - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
                  - reads table `orm:FTP_SERVER` (see above)
                - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:38`
        - writes table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93`
      - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:104`
        - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:109`
          - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
            - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
      - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
      - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-update-ftp-server-2`

- `FileTransferSvcService.UpdateFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:50`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:74`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:38`
        - writes table `orm:FTP_SERVER`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51`
        - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:76`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95`
          - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
            - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
              - `BusinessErrorUtility.LoadConfigurationMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:42`
          - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` **- walk stops here**
          - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59`
            - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
            - `FileTransferCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:69`
              - implementation: `AddFtpServerCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:211`
                - `AddFtpServerCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
                    - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateFtpServerIp` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231`
                  - `AddFtpServerCommand.ValidateFtpServerIpPattern` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:242`
                    - reads table `orm:FTP_SERVER` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateProxyPort` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                - reads table `orm:FTP_SERVER` (see above)
            - `FileTransferCommand.ValidateRequiredFields` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92`
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
          - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:106`
            - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116`
              - reads table `orm:FTP_SERVER` (see above)
            - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:111`
              - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
                - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
                  - reads table `orm:FTP_SERVER` (see above)
                - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:38`
        - writes table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93`
      - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:104`
        - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:109`
          - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
            - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
      - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
      - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-update-ftp-server-3`

- `FileTransferSvcService.UpdateFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:49`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69`
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:74`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:38`
        - writes table `orm:FTP_SERVER`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51`
        - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:76`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95`
          - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
            - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
              - `BusinessErrorUtility.LoadConfigurationMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:42`
          - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` **- walk stops here**
          - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59`
            - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
            - `FileTransferCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:69`
              - implementation: `AddFtpServerCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:211`
                - `AddFtpServerCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
                    - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateFtpServerIp` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231`
                  - `AddFtpServerCommand.ValidateFtpServerIpPattern` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:242`
                    - reads table `orm:FTP_SERVER` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - reads table `orm:FTP_SERVER` (see above)
                - `AddFtpServerCommand.ValidateProxyPort` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                - reads table `orm:FTP_SERVER` (see above)
            - `FileTransferCommand.ValidateRequiredFields` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92`
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
          - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:106`
            - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116`
              - reads table `orm:FTP_SERVER` (see above)
            - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:111`
              - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
                - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
                  - reads table `orm:FTP_SERVER` (see above)
                - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:38`
        - writes table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93`
      - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:104`
        - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114`
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:109`
          - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
            - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
      - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
      - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-controller-update-ftp-server`

- `FileTransferSvcServiceController.UpdateFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:46`
  - `IFileTransferSvcService.UpdateFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcService.cs:165`
    - implementation: `FileTransferSvcService.UpdateFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:50`
      - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49`
        - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:74`
          - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49` (see above)
          - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:38`
            - writes table `orm:FTP_SERVER`
          - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51`
            - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:76`
              - reads table `orm:FTP_SERVER` (see above)
            - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95`
              - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
                - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
                  - `BusinessErrorUtility.LoadConfigurationMessage` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:42`
              - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` **- walk stops here**
              - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59`
                - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
                - `FileTransferCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:69`
                  - implementation: `AddFtpServerCommand.SetCommandValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:211`
                    - `AddFtpServerCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220`
                    - `AddFtpServerCommand.ValidateFtpServerIp` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231`
                    - `AddFtpServerCommand.ValidateProxyPort` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247`
                    - reads table `orm:FTP_SERVER` (see above)
                - `FileTransferCommand.ValidateRequiredFields` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92`
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
                  - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84`
                    - `BusinessErrorUtility.GetMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:27`
              - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:106`
                - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116`
                  - reads table `orm:FTP_SERVER` (see above)
                - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:111`
                  - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
                    - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
                    - reads table `orm:FTP_SERVER` (see above)
                  - reads table `orm:FTP_SERVER` (see above)
                - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTP_SERVER` (see above)
            - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:49` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - routed to: `FtpServerProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:38`
            - writes table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - `FtpServerCommandHandler.ValidateFtpServerCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93`
          - `FtpServerCommandHandler.IsFtpServerNameDuplicated` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:104`
            - `FtpServerCommandHandler.BuildFtpServerQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114`
              - reads table `orm:FTP_SERVER` (see above)
            - `FtpServerCommandHandler.GetFtpServers` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:109`
              - `IFileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs:14`
                - implementation: `FileTransferReadOnlyRepository.GetFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs:38`
                  - reads table `orm:FTP_SERVER` (see above)
                - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTP_SERVER` (see above)
            - reads table `orm:FTP_SERVER` (see above)
          - `FileTransferCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:84` (see above)
          - `FileTransferCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72` (see above)
          - `FileTransferCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:59` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:51` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-invalidate-ftp-server`

- `FileTransferSvcService.InvalidateFtpServer` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:61`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69`
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:56`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:83`
      - reads table `orm:FTP_SERVER`
    - reads table `orm:FTP_SERVER` (see above)
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:58`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:85`
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-invalidate-ftp-server-2`

- `FileTransferSvcService.InvalidateFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:56`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:56`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:83`
      - reads table `orm:FTP_SERVER`
    - reads table `orm:FTP_SERVER` (see above)
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:58`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:85`
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-invalidate-ftp-server-3`

- `FileTransferSvcService.InvalidateFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:61`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69`
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:56`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:83`
      - reads table `orm:FTP_SERVER`
    - reads table `orm:FTP_SERVER` (see above)
  - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:58`
    - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:85`
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-svc-service-controller-invalidate-ftp-server`

- `FileTransferSvcServiceController.InvalidateFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:52`
  - `IFileTransferSvcService.InvalidateFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcService.cs:186`
    - implementation: `FileTransferSvcService.InvalidateFtpServer` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:56`
      - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:56`
        - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:83`
          - reads table `orm:FTP_SERVER`
        - reads table `orm:FTP_SERVER` (see above)
      - routed to: `FtpServerCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:58`
        - `FtpServerCommandHandler.BuildEvent` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:85`
          - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-controller-get-ftp-server-details`

- `FileTransferController.GetFtpServerDetails` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:111`
  - writes table `orm:FTP_SERVER`

### `file-transfer-controller-get-ftpserver-credentials`

- `FileTransferController.GetFTPServerCredentials` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:26`
  - `FileTransferController.BuildRequest` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:92`
    - `SearchHelper.GetFieldValue` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs:30`
    - reads table `orm:FTP_SERVER`
  - `FileTransferController.BuildResponse` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:116`
    - `FileTransferController.BuildDataSourceResult` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:140`
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-controller-add-ftpserver-credentials`

- `FileTransferController.AddFTPServerCredentials` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:43`
  - `FileTransferController.CreateFTPServerCredentials` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:52`
    - `FileTransferController.CreateRequest` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:107`
      - reads table `orm:FTP_SERVER`
    - `FileTransferController.SetResponse` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:123`
      - `FileTransferController.FtpServerCredentialsSave` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:149`
        - `FileTransferController.GetSuccessMessage` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:180`
        - reads table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - `FileTransferController.UpdateFTPServerCredentials` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:68`
    - `FileTransferController.FormatInvalidResponse` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:187` **- walk stops here**
    - `FileTransferController.SetResponse` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:123` (see above)
    - `FileTransferController.UpdateFtpServer` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:171`
      - `FileTransferController.CreateRequest` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:107` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-controller-create-ftpserver-credentials`

- `FileTransferController.CreateFTPServerCredentials` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:52`
  - `FileTransferController.CreateRequest` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:107`
    - reads table `orm:FTP_SERVER`
  - `FileTransferController.SetResponse` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:123`
    - `FileTransferController.FtpServerCredentialsSave` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:149`
      - `FileTransferController.GetSuccessMessage` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:180`
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

### `file-transfer-controller-update-ftpserver-credentials`

- `FileTransferController.UpdateFTPServerCredentials` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:68`
  - `FileTransferController.FormatInvalidResponse` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:187` **- walk stops here**
  - `FileTransferController.SetResponse` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:123`
    - `FileTransferController.FtpServerCredentialsSave` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:149`
      - `FileTransferController.GetSuccessMessage` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:180`
      - reads table `orm:FTP_SERVER`
    - reads table `orm:FTP_SERVER` (see above)
  - `FileTransferController.UpdateFtpServer` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:171`
    - `FileTransferController.CreateRequest` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:107`
      - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTP_SERVER` (see above)
  - reads table `orm:FTP_SERVER` (see above)

## Classes involved (by namespace; methods walked)

- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc`: `FileTransferSvcQueryService` (2), `FileTransferSvcService` (4)
- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers`: `FtpServerCommandHandler` (10)
- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands`: `AddFtpServerCommand` (5), `FileTransferCommand` (6)
- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities`: `BusinessErrorUtility` (4)
- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers`: `FtpServerProjectionWriter` (3)
- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Repository`: `FileTransferReadOnlyRepository` (1), `IFileTransferReadOnlyRepository` (1)
- `HP.HSP.UA3.FileTransfer.UX.Services.Controller`: `FileTransferController` (14)
- `HP.HSP.UA3.FileTransfer.UX.Services.Helpers`: `SearchHelper` (1)
- `HPP.FileTransfer.BAS.FileTransferApi.Commands`: `FileTransferSvcService` (4)
- `HPP.FileTransfer.BAS.FileTransferApi.Controllers`: `FileTransferSvcQueryServiceController` (1), `FileTransferSvcServiceController` (3)
- `HPP.FileTransfer.BAS.FileTransferApi.Interfaces`: `IFileTransferSvcQueryService` (1), `IFileTransferSvcService` (3)
- `HPP.FileTransfer.BAS.FileTransferApi.Queries`: `FileTransferSvcQueryService` (2)
- `HPP.FileTransfer.BAS.FileTransferSvc`: `FileTransferSvcQueryService` (2), `FileTransferSvcService` (4)
- `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers`: `FtpServerCommandHandler` (10)
- `HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers`: `FtpServerProjectionWriter` (3)
- `HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Repository`: `FileTransferReadOnlyRepository` (1), `IFileTransferReadOnlyRepository` (1)

## Data

- table `orm:FTP_SERVER` (reads, writes)

## Calls into code the system does not own

- **automapper** 12.0.1: `AutoMapper.IMapperBase.Map<TSource, TDestination>(TSource, TDestination)` (3 call sites)
- **hp.hsp.ua3.core.ux.common** 30.0.0: `HP.HSP.UA3.Core.UX.Common.ExtensionMethods.IsNullOrEmpty<T>(System.Collections.Generic.ICollection<T>)` (1 call sites)
- **hp.hsp.ua3.core.ux.common** 30.0.0: `HP.HSP.UA3.Core.UX.Configuration.Helpers.ContextHelper.InitializeSearchRequest(Kendo.Mvc.UI.DataSourceRequest)` (1 call sites)
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ILocalizationProvider.GetMessage(string, string, string)` (1 call sites)
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ITenantConfigurationManager.Resolve<T>(string)` (4 call sites)
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.IUserConfigurationManager.GenerateRequestor()` (3 call sites)
- **hp.hsp.ua3.core.ux.services** 30.0.0: `HP.HSP.UA3.Core.UX.Services.Controllers.Base.BaseApiController.FormatResultModel(System.Web.Http.ModelBinding.ModelStateDictionary)` (2 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.CommandServiceBase.TryRoute(HPP.Core.BAS.CQRS.Base.Command)` (2 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.ProjectionWriterBase<T>.Add(T)` (1 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.ProjectionWriterBase<T>.Update(dynamic, System.Action<T>)` (1 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.QueryServiceBase.TryRoute<TQuery>(TQuery)` (2 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Config.IMessageCodesReadOnly.GetMessage(System.Guid, string)` (1 call sites)
- **microsoft.aspnet.webapi.core** 5.3.0: `System.Net.Http.HttpRequestMessageExtensions.CreateResponse<T>(System.Net.Http.HttpRequestMessage, System.Net.HttpStatusCode, T)` (10 call sites)
- **** : `?.Find` (1 call sites)
- **** : `?.ToLower` (1 call sites)
- **** : `CommandServiceBase.TryRoute` (1 call sites)
- **** : `Guid.Equals` (1 call sites)
- **** : `IMessageCodesReadOnly.GetMessage` (1 call sites)
- **** : `Mapper.DynamicMap` (3 call sites)
- **** : `ProjectionWriterBase<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtpServer>.Add` (1 call sites)
- **** : `ProjectionWriterBase<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtpServer>.Update` (1 call sites)
- **** : `QueryServiceBase.TryRoute` (1 call sites)
- **** : `string.ToString` (1 call sites)

## Where the walk stops

The graph sees nothing these methods call: a router, a call into an unresolved package, reflection, configuration, or simply a leaf. Read the code from here.

- `FileTransferCommand.HandleValidations` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72`
- `FileTransferController.FormatInvalidResponse` at `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:187`

## Shared pieces used (not walked)

None.
