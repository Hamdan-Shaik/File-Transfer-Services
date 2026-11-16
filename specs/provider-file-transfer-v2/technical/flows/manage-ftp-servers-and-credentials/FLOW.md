---
type: Flow Specification
title: Manage FTP servers and credentials
description: This flow supports listing FTP servers (for selection and review) and maintaining FTP server definitions (add, update, invalidate).
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
  body_sha256: 8fd619070ebc9eb58dc85e563fbd7fa2498d5df44c8cdc4554f72065e6412070
---

# Manage FTP servers and credentials

This flow supports listing FTP servers (for selection and review) and maintaining FTP server definitions (add, update, invalidate). It also supports listing and saving FTP server credentials from the UX layer.

## Who starts it

- Integrations and internal callers using the FileTransferSvc query/service doors.
- Signed-in staff using the File Transfer UX.

## Doors

| Door | Request | Answers |
| --- | --- | --- |
| `file-transfer-svc-query-service-get-ftp-servers` | `FtpServerQuery` | `FtpServerQuery` (routed result) |
| `file-transfer-svc-query-service-get-ftp-servers-2` | `FtpServerQuery` | `FtpServerQuery` (routed result) |
| `file-transfer-svc-query-service-get-ftp-servers-3` | `FtpServerQuery` | `FtpServerQuery` (routed result) |
| `file-transfer-svc-query-service-controller-get-ftp-servers` | POST `/FileTransferSvcQueryService` with `FtpServerQuery` | Always `200 OK` with `GetFtpServers(query)` result |
| `file-transfer-svc-service-add-ftp-server` | `AddFtpServerCommand` | `FtpServerAdded` (routed result) |
| `file-transfer-svc-service-add-ftp-server-2` | `AddFtpServerCommand` | `FtpServerAdded` (routed result) |
| `file-transfer-svc-service-add-ftp-server-3` | `AddFtpServerCommand` | `FtpServerAdded` (routed result) |
| `file-transfer-svc-service-controller-add-ftp-server` | POST `/FileTransferSvcService` with `AddFtpServerCommand` | Always `200 OK` with `AddFtpServer(command)` result |
| `file-transfer-svc-service-update-ftp-server` | `UpdateFtpServerCommand` | `FtpServerUpdated` (routed result) |
| `file-transfer-svc-service-update-ftp-server-2` | `UpdateFtpServerCommand` | `FtpServerUpdated` (routed result) |
| `file-transfer-svc-service-update-ftp-server-3` | `UpdateFtpServerCommand` | `FtpServerUpdated` (routed result) |
| `file-transfer-svc-service-controller-update-ftp-server` | POST `/FileTransferSvcService` with `UpdateFtpServerCommand` | Always `200 OK` with `UpdateFtpServer(command)` result |
| `file-transfer-svc-service-invalidate-ftp-server` | `InvalidateFtpServerCommand` | `FtpServerInvalidated` (routed result) |
| `file-transfer-svc-service-invalidate-ftp-server-2` | `InvalidateFtpServerCommand` | `FtpServerInvalidated` (routed result) |
| `file-transfer-svc-service-invalidate-ftp-server-3` | `InvalidateFtpServerCommand` | `FtpServerInvalidated` (routed result) |
| `file-transfer-svc-service-controller-invalidate-ftp-server` | POST `/FileTransferSvcService` with `InvalidateFtpServerCommand` | Always `200 OK` with `InvalidateFtpServer(command)` result |
| `file-transfer-controller-get-ftp-server-details` | UX action `GetFtpServerDetails()` | `200` with list-items of server name + id, or `400` with errors |
| `file-transfer-controller-get-ftpserver-credentials` | UX action `GetFTPServerCredentials(DataSourceRequest request)` | `404` with empty list when no data; else `400` on error; else `200` with `DataSourceResult` |
| `file-transfer-controller-add-ftpserver-credentials` | UX action `AddFTPServerCredentials(FtpServerModel ftpServerModel)` | Delegates to create or update |
| `file-transfer-controller-create-ftpserver-credentials` | UX action `CreateFTPServerCredentials(FtpServerModel ftpServerModel)` | `400` on invalid model; else `200` on OK add; else `400` with service result |
| `file-transfer-controller-update-ftpserver-credentials` | UX action `UpdateFTPServerCredentials(FtpServerModel ftpServerModel)` | `400` on invalid model; else `200` on OK update/invalidate; else `400` with service result |

## Steps

1. A caller requests FTP servers via the BAS query doors; the query is routed and executed against `FTP_SERVER`. The legacy query door supports richer filtering and applies a default A-to-Z ordering by server name; the newer query doors only support validity and exact-name filtering and do not apply sorting. (F02-001, F02-002, F02-003, F02-005, F02-006, F02-007, F02-008, F02-009)
2. The UX requests FTP server details for UI selection; the UX provider always requests valid servers and maps results into `Name`/`ID` list items. (F02-010, F02-011, F02-012, F02-013, F02-014, F02-015, F02-016)
3. A caller adds an FTP server; the command is routed, validated, and a `FtpServerAdded` event is produced and projected into `FTP_SERVER` with timestamps. (F02-017, F02-018, F02-023, F02-025, F02-028, F02-029, F02-030, F02-062, F02-031, F02-032, F02-035)
4. A caller updates an FTP server; the update command shares the add validation rules, produces a `FtpServerUpdated` event, and updates `FTP_SERVER` and timestamps. (F02-019, F02-020, F02-024, F02-062, F02-033, F02-036)
5. A caller invalidates an FTP server; the invalidate command produces a `FtpServerInvalidated` event and updates `IsValid` to false and timestamps. (F02-021, F02-022, F02-034, F02-037)
6. Staff list and save FTP server credentials via the UX controller; listing uses paging/sorting and removes passwords by default, and saves return localized success messages on OK status. (F02-038, F02-039, F02-041, F02-045, F02-046, F02-049, F02-050, F02-054, F02-056, F02-063, F02-057, F02-058, F02-059, F02-061)

## Outcome

- Listing FTP servers returns `FtpServerModel` rows such as:

  - `FtpServerId`: `2b9f6e1b-2d23-4e4f-9a1a-2c2f51c8cfe0`
  - `FtpServerName`: `ACME-FTP`
  - `FtpServerIp`: `10.20.30.40`
  - `FtpUser`: `ftpsvc`
  - `FtpTransferMode`: `PASSIVE`
  - `IsValid`: `true`

- Creating or updating credentials returns a model echoing `FtpServerId` and the incoming `Editor` object and includes a localized success message when the save succeeds.

## Errors

- Add/update validation failures collect per-field errors and ultimately fail command execution with business error code `E` and message `Command Execution Error`. (F02-026, F02-027)
- The UX credentials save actions return a formatted error result when the submitted data is invalid or when the BAS service reports an error. (F02-049, F02-053, F02-055)
