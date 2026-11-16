---
type: Data Access
title: Data
description: Tables, stored procedures and data the flow reads and writes.
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
  body_sha256: 30ba5aa5e46bfa2d9888fadde721291d191cd538847b13b2feebc2f6bb71521c
---

# Data

## Stored procedures

None.

## Tables

### `orm:FTP_SERVER`

- Reads (list/search): (F02-005, F02-007, F02-008, F02-009)
- Reads (dup check): (F02-031)
- Writes (create): (F02-035)
- Writes (update): (F02-036)
- Writes (invalidate): (F02-037)

Key columns (as used by this flow):

- `FtpServerId` (identifier) (F02-007, F02-028)
- `IsValid` (active flag) (F02-005, F02-032, F02-034, F02-037)
- `FtpServerName` (display/name) (F02-006, F02-008, F02-031)
- `FtpDescription` (description) (F02-008)
- `FtpServerIp` (host/IP) (F02-009, F02-029)
- `FtpTransferMode` (transfer mode) (F02-025)
- `FtpUser` (username) (F02-008, F02-025)
- `FtpPassword` (password) (F02-009, F02-025, F02-036)
- `FtpPort` (server port) (F02-062)
- `FtpProxyAddress` (proxy host) (F02-030, F02-036)
- `FtpProxyPort` (proxy port) (F02-030, F02-036)
- `CreationDate` (UTC) (F02-035)
- `LastModifiedDate` (UTC) (F02-035, F02-036, F02-037)
- `OperatorId` (who changed it) (F02-032, F02-033, F02-034, F02-036, F02-037)

## Answer shapes

### BAS FTP server listing (`GetFtpServers`)

Returned item shape: `FtpServerModel`. This flow projects these fields from `FTP_SERVER`: (F02-009)

- `FtpServerId`
- `FtpServerName`
- `FtpDescription`
- `FtpServerIp`
- `FtpTransferMode`
- `FtpUser`
- `FtpPassword`
- `FtpPort`
- `FtpProxyAddress`
- `FtpProxyPort`
- `IsValid`

### UX server list items (`GetFtpServerDetails`)

Returned item shape: `ListItemModel`. (F02-013)

- `Name`: FTP server name
- `ID`: FTP server id as a string

### UX credentials listing (`GetFTPServerCredentials`)

Returned item shape: `DataSourceResult`. (F02-041)

- `Data`: list of FTP server rows
- `Total`: total row count

Credential field handling:

- `FtpPassword` is blanked when `RemovePassword` is not explicitly set. (F02-045)

### UX save responses (create/update/invalidate)

Returned item shape: `FtpServerModel`. On success it includes: (F02-050, F02-052, F02-058, F02-059)

- `FtpServerId`: created/updated server id
- `IsValid`: updated server validity on update/invalidate
- `Editor`: echoed from the request and populated with a success message when OK
