---
type: Business Rules
title: technical/flows/manage-ftp-servers-and-credentials/parts/maintain-ftp-servers/RULES
description: 21 business rules (P2-NNN) of the flow, each with its legacy source lines.
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
  body_sha256: 42b4a01bcd13a39816a8f1b5e8a61f360145e932da2e449ee48fabff0968a266
---

﻿# Rules: maintain-ftp-servers

## Doors

**P2-001** The WCF/service entrypoints for AddFtpServer route the given command to the command router and return the routed result cast to `FtpServerAdded`.
- Doors: file-transfer-svc-service-add-ftp-server, file-transfer-svc-service-add-ftp-server-2, file-transfer-svc-service-add-ftp-server-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:43-46, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69-72, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:44-47, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64-67, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:43-46, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69-72
- Legacy: The door method returns `TryRouteCommand(command) as FtpServerAdded`, and `TryRouteCommand` delegates to `TryRoute`.
- Decision: keep

**P2-002** The HTTP controller endpoint for AddFtpServer responds `200 OK` containing the result of calling the service’s `AddFtpServer(command)`.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:40-44
- Legacy: The controller wraps `fileTransferSvcService.AddFtpServer(command)` in `Ok(...)`.
- Decision: keep

**P2-003** The WCF/service entrypoints for UpdateFtpServer route the given command to the command router and return the routed result cast to `FtpServerUpdated`.
- Doors: file-transfer-svc-service-update-ftp-server, file-transfer-svc-service-update-ftp-server-2, file-transfer-svc-service-update-ftp-server-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:49-52, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69-72, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:50-53, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64-67, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:49-52, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69-72
- Legacy: The door method returns `TryRouteCommand(command) as FtpServerUpdated`, and `TryRouteCommand` delegates to `TryRoute`.
- Decision: keep

**P2-004** The HTTP controller endpoint for UpdateFtpServer responds `200 OK` containing the result of calling the service’s `UpdateFtpServer(command)`.
- Doors: file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:46-50
- Legacy: The controller wraps `fileTransferSvcService.UpdateFtpServer(command)` in `Ok(...)`.
- Decision: keep

**P2-005** The WCF/service entrypoints for InvalidateFtpServer route the given command to the command router and return the routed result cast to `FtpServerInvalidated`.
- Doors: file-transfer-svc-service-invalidate-ftp-server, file-transfer-svc-service-invalidate-ftp-server-2, file-transfer-svc-service-invalidate-ftp-server-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:61-64, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69-72, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:56-59, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64-67, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:61-64, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69-72
- Legacy: The door method returns `TryRouteCommand(command) as FtpServerInvalidated`, and `TryRouteCommand` delegates to `TryRoute`.
- Decision: keep

**P2-006** The HTTP controller endpoint for InvalidateFtpServer responds `200 OK` containing the result of calling the service’s `InvalidateFtpServer(command)`.
- Doors: file-transfer-svc-service-controller-invalidate-ftp-server
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:52-56
- Legacy: The controller wraps `fileTransferSvcService.InvalidateFtpServer(command)` in `Ok(...)`.
- Decision: keep

## Add FTP server

**P2-007** Handling an AddFtpServer command validates the command and fails the request if validations are present.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:42-47, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:93-101, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:44-49, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:95-103
- Legacy: The handler calls `ValidateFtpServerCommand(command)`, which calls `command.Validate()` and may throw during `HandleValidations()`.
- Decision: keep

**P2-008** For AddFtpServer and UpdateFtpServer, the command type for update inherits the add command type, so both operations share the same add-command validation rules.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateFtpServerCommand.cs:14-15, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateFtpServerCommand.cs:14-15
- Legacy: `UpdateFtpServerCommand : AddFtpServerCommand`.
- Decision: keep

**P2-009** AddFtpServer/UpdateFtpServer treat these string properties as required (null or empty values are considered missing): `FtpServerName`, `FtpDescription`, `FtpServerIp`, `FtpTransferMode`, `FtpUser`, `FtpPassword`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:51-53, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:63-65, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:75-77, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:87-89, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:99-101, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:111-113
- Legacy: These properties are annotated with `[Required(AllowEmptyStrings = false)]` and are checked by `ValidateRequiredFields()`.
- Decision: keep

**P2-010** When required-field validation fails, one validation entry is added per failing property with message text `Missing field: <PropertyName>`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92-101, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92-100, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:46-47, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39-40
- Legacy: The code uses `Validator.TryValidateObject(...)` and formats each message as `"Missing field: {memberName}"`.
- Decision: keep

**P2-011** If any validations are present after validation runs, the command fails by throwing `BusinessException` with message `Command Execution Error` and business error code `E`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72-80, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72-80, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:21-22, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39-40, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:20-21, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:33-34
- Legacy: `HandleValidations()` throws a `BusinessException(validations, "Command Execution Error", "E")` when `Validations.Any()` is true.
- Decision: keep

**P2-012** AddFtpServer/UpdateFtpServer reject an empty GUID FTP server id (`FtpServerId == Guid.Empty`).
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220-228, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:219-226
- Legacy: `ValidateFtpServerId()` adds a validation when `FtpServerId.Equals(Guid.Empty)`.
- Decision: keep

**P2-013** AddFtpServer/UpdateFtpServer validate `FtpServerIp` against the regex pattern `\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231-245, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:229-242, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:14-17, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:13-16
- Legacy: When `FtpServerIp` is non-empty, `ValidateFtpServerIp()` calls `Regex.IsMatch(FtpServerIp, IpAddressPattern)` and adds a validation on mismatch.
- Decision: keep

**P2-014** If `FtpProxyAddress` is provided (non-empty), AddFtpServer/UpdateFtpServer require `FtpProxyPort` to be present and greater than 0.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247-255, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:244-251
- Legacy: `ValidateProxyPort()` adds a validation when `!string.IsNullOrEmpty(FtpProxyAddress)` and `(!FtpProxyPort.HasValue || FtpProxyPort <= 0)`.
- Decision: keep

**P2-015** AddFtpServer/UpdateFtpServer reject a duplicate FTP server name by checking for an existing valid FTP server record with the same `FtpServerName` but a different `FtpServerId`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:97-107, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:114-123, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:99-109, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:116-125, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:95-108, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetFtpServerQueryExecutor.cs:30-33
- Legacy: The handler queries FTP servers with `IsValid = true` and `FtpServerName = command.FtpServerName`, then considers it a duplicate when any returned row has `FtpServerId != command.FtpServerId`.
- Decision: keep
- ⚑ Flag: The two query executors compare `FtpServerName` case-insensitively, but only the HP.HSP.UA3 executor trims whitespace during comparison.

**P2-016** AddFtpServer returns an event populated from the command and forces `IsValid = true`, with `OperatorId` set to the requestor identifier.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:64-71, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:66-73
- Legacy: The handler maps command fields onto `FtpServerAdded`, then sets `ftpServerAdded.IsValid = true` and `ftpServerAdded.OperatorId = command.Requestor.IdentifierId`.
- Decision: keep

## Update FTP server

**P2-017** UpdateFtpServer returns an event populated from the command, with `OperatorId` set to the requestor identifier.
- Doors: file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:74-81, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:76-83
- Legacy: The handler maps command fields onto `FtpServerUpdated`, then sets `ftpServerUpdated.OperatorId = command.Requestor.IdentifierId`.
- Decision: keep

## Invalidate FTP server

**P2-018** InvalidateFtpServer does not run command validation; it returns an event containing `FtpServerId` and forces `IsValid = false`, with `OperatorId` set to the requestor identifier.
- Doors: file-transfer-svc-service-controller-invalidate-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:56-59, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:83-91, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:58-61, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:85-93, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\InvalidateFtpServerCommand.cs:14-15, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\InvalidateFtpServerCommand.cs:14-15
- Legacy: The handler returns `new FtpServerInvalidated { FtpServerId = command.FtpServerId, IsValid = false, OperatorId = command.Requestor.IdentifierId }` and does not call `command.Validate()`.
- Decision: keep

## Projection (FTP_SERVER write model)

**P2-019** When a `FtpServerAdded` event is handled, a new FTP server row is created with `CreationDate` and `LastModifiedDate` set to `DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74-82, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74-82
- Legacy: The projection writer maps the event to a new `FtpServer` entity, sets both timestamps to `DateTime.UtcNow`, and calls `Add(newFtpServer)`.
- Decision: keep

**P2-020** When a `FtpServerUpdated` event is handled, the existing FTP server row is updated by id and `LastModifiedDate` is set to `DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:39-57, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:39-57
- Legacy: The projection writer updates all FTP server fields from the event (including credentials and proxy fields), sets `LastModifiedDate = DateTime.UtcNow`, and sets `OperatorId = @event.OperatorId`.
- Decision: keep

**P2-021** When a `FtpServerInvalidated` event is handled, the existing FTP server row is updated by id by setting `IsValid` (to false), `OperatorId`, and `LastModifiedDate = DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-invalidate-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:60-69, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:60-69
- Legacy: The projection writer sets `IsValid = @event.IsValid`, sets `OperatorId = @event.OperatorId`, and sets `LastModifiedDate = DateTime.UtcNow`.
- Decision: keep
