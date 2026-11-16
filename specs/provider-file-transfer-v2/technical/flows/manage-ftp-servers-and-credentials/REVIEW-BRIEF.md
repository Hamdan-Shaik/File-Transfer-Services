---
type: Generation Brief
title: 'Brief: review the rules of flow manage-ftp-servers-and-credentials against the code'
description: Instructions the tool gave the reviewer session for this flow.
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
  body_sha256: 3d9214e4287b6a5811ab23a4f0394e64811e2b1ea1703fe98a1ec01b32bb1dd6
---

# Brief: review the rules of flow `manage-ftp-servers-and-credentials` against the code

- Business name: **Manage FTP servers and credentials**
- The rules to review: `technical/flows/manage-ftp-servers-and-credentials/RULES.md` (and `technical/flows/manage-ftp-servers-and-credentials/FLOW.md`)
- Source root (read-only): `C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\legacy-kg\components`. Paths below are relative to it.
- Write exactly: `technical/flows/manage-ftp-servers-and-credentials/REVIEW.md`
- Playbook: `inputs/playbook/review.md`

## Doors (where the flow starts)

| Door id | Kind | Name | Where | Endpoints |
| --- | --- | --- | --- | --- |
| `file-transfer-svc-query-service-get-ftp-servers` | wcf | GetFtpServers | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:35` | - |
| `file-transfer-svc-query-service-get-ftp-servers-2` | wcf | GetFtpServers | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:26` | - |
| `file-transfer-svc-query-service-get-ftp-servers-3` | wcf | GetFtpServers | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:26` | - |
| `file-transfer-svc-query-service-controller-get-ftp-servers` | api | GetFtpServers | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:33` | POST /FileTransferSvcQueryService |
| `file-transfer-svc-service-add-ftp-server` | wcf | AddFtpServer | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:43` | - |
| `file-transfer-svc-service-add-ftp-server-2` | wcf | AddFtpServer | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:44` | - |
| `file-transfer-svc-service-add-ftp-server-3` | wcf | AddFtpServer | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:43` | - |
| `file-transfer-svc-service-controller-add-ftp-server` | api | AddFtpServer | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:40` | POST /FileTransferSvcService |
| `file-transfer-svc-service-update-ftp-server` | wcf | UpdateFtpServer | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:49` | - |
| `file-transfer-svc-service-update-ftp-server-2` | wcf | UpdateFtpServer | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:50` | - |
| `file-transfer-svc-service-update-ftp-server-3` | wcf | UpdateFtpServer | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:49` | - |
| `file-transfer-svc-service-controller-update-ftp-server` | api | UpdateFtpServer | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:46` | POST /FileTransferSvcService |
| `file-transfer-svc-service-invalidate-ftp-server` | wcf | InvalidateFtpServer | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:61` | - |
| `file-transfer-svc-service-invalidate-ftp-server-2` | wcf | InvalidateFtpServer | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:56` | - |
| `file-transfer-svc-service-invalidate-ftp-server-3` | wcf | InvalidateFtpServer | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:61` | - |
| `file-transfer-svc-service-controller-invalidate-ftp-server` | api | InvalidateFtpServer | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:52` | POST /FileTransferSvcService |
| `file-transfer-controller-get-ftp-server-details` | api | GetFtpServerDetails | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:111` | - |
| `file-transfer-controller-get-ftpserver-credentials` | api | GetFTPServerCredentials | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:26` | - |
| `file-transfer-controller-add-ftpserver-credentials` | api | AddFTPServerCredentials | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:43` | - |
| `file-transfer-controller-create-ftpserver-credentials` | api | CreateFTPServerCredentials | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:52` | - |
| `file-transfer-controller-update-ftpserver-credentials` | api | UpdateFTPServerCredentials | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:68` | - |

## Code to read (the flow's own methods, by file)

- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs`: BuildEvent (64), BuildEvent (74), BuildEvent (83), BuildFtpServerQuery (114), GetFtpServers (109), Handle (42), Handle (49), Handle (56), IsFtpServerNameDuplicated (104), ValidateFtpServerCommand (93)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs`: SetCommandValidations (211), ValidateFtpServerId (220), ValidateFtpServerIp (231), ValidateFtpServerIpPattern (242), ValidateProxyPort (247)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs`: AddValidation (84), HandleValidations (72), SetCommandValidations (69), Validate (59), ValidateRequiredFields (92)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs`: GetMessage (27), LoadConfigurationMessage (42)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs`: AddFtpServer (74), Handle (33), Handle (38)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs`: GetFtpServer (38)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs`: GetFtpServer (14)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs`: GetFtpServers (35), TryRouteQuery (55)
- `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs`: AddFtpServer (43), InvalidateFtpServer (61), TryRouteCommand (69), UpdateFtpServer (49)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs`: AddFtpServer (44), InvalidateFtpServer (56), TryRouteCommand (64), UpdateFtpServer (50)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs`: GetFtpServers (33)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs`: AddFtpServer (40), InvalidateFtpServer (52), UpdateFtpServer (46)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcQueryService.cs`: GetFtpServers (41)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcService.cs`: AddFtpServer (135), InvalidateFtpServer (186), UpdateFtpServer (165)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs`: GetFtpServers (26), TryRouteQuery (41)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs`: BuildEvent (66), BuildEvent (76), BuildEvent (85), BuildFtpServerQuery (116), GetFtpServers (111), Handle (44), Handle (51), Handle (58), IsFtpServerNameDuplicated (106), ValidateFtpServerCommand (95)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs`: AddValidation (84)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs`: GetMessage (27), LoadConfigurationMessage (42)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs`: AddFtpServer (74), Handle (33), Handle (38)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\FileTransferReadOnlyRepository.cs`: GetFtpServer (38)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\Repository\IFileTransferReadOnlyRepository.cs`: GetFtpServer (14)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs`: GetFtpServers (26), TryRouteQuery (46)
- `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs`: AddFtpServer (43), InvalidateFtpServer (61), TryRouteCommand (69), UpdateFtpServer (49)
- `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs`: AddFTPServerCredentials (43), BuildDataSourceResult (140), BuildRequest (92), BuildResponse (116), CreateFTPServerCredentials (52), CreateRequest (107), FormatInvalidResponse (187), FtpServerCredentialsSave (149), GetFTPServerCredentials (26), GetSuccessMessage (180), SetResponse (123), UpdateFTPServerCredentials (68), UpdateFtpServer (171)
- `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs`: GetFtpServerDetails (111)
- `ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs`: GetFieldValue (30)

## Stored procedures (definition found in the graph; check for later redefinitions)

None.

## Tables

- `orm:FTP_SERVER` (reads, writes)

## Shared pieces (used, owned elsewhere: describe what the flow uses them for, not their insides)

None.

## Calls into code the system does not own (contracts)

- **automapper** 12.0.1: `AutoMapper.IMapperBase.Map<TSource, TDestination>(TSource, TDestination)` (3 call sites)
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:69` <HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.AddFtpServerCommand, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtpServerAdded> source=<command>, destination=<ftpServerAdded>
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:77` <HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtpServerAdded, HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtpServer> source=<@event>, destination=<newFtpServer>
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:79` <HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.UpdateFtpServerCommand, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtpServerUpdated> source=<command>, destination=<ftpServerUpdated>
- **hp.hsp.ua3.core.ux.common** 30.0.0: `HP.HSP.UA3.Core.UX.Common.ExtensionMethods.IsNullOrEmpty<T>(System.Collections.Generic.ICollection<T>)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs:33` <HP.HSP.UA3.Core.UX.Data.Messaging.SearchFieldModel> 
- **hp.hsp.ua3.core.ux.common** 30.0.0: `HP.HSP.UA3.Core.UX.Configuration.Helpers.ContextHelper.InitializeSearchRequest(Kendo.Mvc.UI.DataSourceRequest)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:34` request=<request>
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ILocalizationProvider.GetMessage(string, string, string)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:182` localeId=<this.ContextHelper.UserConfigurationManager.UserProfile.LocalCode>, contentId=<contentId>
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ITenantConfigurationManager.Resolve<T>(string)` (4 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:120` <HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer.IFileTransferQueryServiceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:31` <HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer.IFileTransferQueryServiceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:55` <HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer.IFileTransferServiceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.FileTransferServiceDataProvider>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:73` <HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer.IFileTransferServiceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.FileTransferServiceDataProvider>
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.IUserConfigurationManager.GenerateRequestor()` (3 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:125` 
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:103` 
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:111` 
- **hp.hsp.ua3.core.ux.services** 30.0.0: `HP.HSP.UA3.Core.UX.Services.Controllers.Base.BaseApiController.FormatResultModel(System.Web.Http.ModelBinding.ModelStateDictionary)` (2 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:65` modelState=<this.ModelState>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:88` modelState=<this.ModelState>
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.CommandServiceBase.TryRoute(HPP.Core.BAS.CQRS.Base.Command)` (2 call sites)
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:66` command=<command>
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:71` command=<command>
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.ProjectionWriterBase<T>.Add(T)` (1 call sites)
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:81` newInstance=<newFtpServer>
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.ProjectionWriterBase<T>.Update(dynamic, System.Action<T>)` (1 call sites)
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:41` id=<@event.FtpServerId>, updateAction=<e =>
                        {
                            e.FtpDescription = @event.FtpDescription;
                            e.FtpServerIp = @event.FtpSe>
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.QueryServiceBase.TryRoute<TQuery>(TQuery)` (2 call sites)
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:43` <HPP.Core.BAS.CQRS.Base.Query> query=<query>
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:48` <HPP.Core.BAS.CQRS.Base.Query> query=<query>
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Config.IMessageCodesReadOnly.GetMessage(System.Guid, string)` (1 call sites)
  - `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:44` tenantID=<Guid.Parse(requestor.TenantId)>, messageCode=<contentId>
- **microsoft.aspnet.webapi.core** 5.3.0: `System.Net.Http.HttpRequestMessageExtensions.CreateResponse<T>(System.Net.Http.HttpRequestMessage, System.Net.HttpStatusCode, T)` (10 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:150` <System.Collections.Generic.List<HP.HSP.UA3.Core.UX.Data.Common.ListItemModel>> statusCode='200', value=<listItems>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:154` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<resultModel>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:40` <System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.UX.Data.FileTransfer.FtpServerModel>> statusCode='404', value=<new List<FtpServerModel>()>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:119` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<ftpServerResult.Result>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:120` <Kendo.Mvc.UI.DataSourceResult> statusCode='200', value=<this.BuildDataSourceResult(ftpServerResult)>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:65` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<this.FormatResultModel(this.ModelState)>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:85` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<this.FormatInvalidResponse(processorResult.Result.Messages[0].Message)>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:88` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<this.FormatResultModel(this.ModelState)>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:136` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<serviceResponse.Result>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:168` <HP.HSP.UA3.FileTransfer.UX.Data.FileTransfer.FtpServerModel> statusCode='200', value=<ftpServerModel>
- **** : `?.Find` (1 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:33` None=<f => f.LocaleID.ToLower() == requestor.LocaleCode.ToLower()>
- **** : `?.ToLower` (1 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:33` 
- **** : `CommandServiceBase.TryRoute` (1 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:71` None=<command>
- **** : `Guid.Equals` (1 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:106` None=<ftpServerId>
- **** : `IMessageCodesReadOnly.GetMessage` (1 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Utilities\BusinessErrorUtility.cs:44` None=<Guid.Parse(requestor.TenantId)>, None=<contentId>
- **** : `Mapper.DynamicMap` (3 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:67` None=<command>, None=<ftpServerAdded>
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:77` None=<@event>, None=<newFtpServer>
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtpServerCommandHandler.cs:77` None=<command>, None=<ftpServerUpdated>
- **** : `ProjectionWriterBase<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtpServer>.Add` (1 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:81` None=<newFtpServer>
- **** : `ProjectionWriterBase<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtpServer>.Update` (1 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:41` None=<@event.FtpServerId>, None=<e =>
                        {
                            e.FtpDescription = @event.FtpDescription;
                            e.FtpServerIp = @event.FtpSe>
- **** : `QueryServiceBase.TryRoute` (1 call sites)
  - `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:57` None=<query>
- **** : `string.ToString` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:139` 

## Where the graph's walk stops (it sees nothing these call: keep reading the code from here)

Often a router, a call into a package, reflection or configuration. Follow the code to the data.

- `FileTransferCommand.HandleValidations` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72`
- `FileTransferController.FormatInvalidResponse` at `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:187`
