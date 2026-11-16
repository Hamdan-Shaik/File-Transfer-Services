---
type: Generation Brief
title: 'Brief: rules of part manage-ftp-server-credentials of flow manage-ftp-servers-and-credentials'
description: Instructions the tool gave the writer session for one part of this flow.
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
  body_sha256: 5a56841d4ec9befead88f3be223820fe4c487b593e26c62da0bb3f2177e114a9
---

# Brief: rules of part `manage-ftp-server-credentials` of flow `manage-ftp-servers-and-credentials`

- Part: **Manage FTP server credentials**, of the flow **Manage FTP servers and credentials**
- Rule-id prefix: **P3** (first rule P3-001); the tool renumbers them into the flow later
- Source root (read-only): `C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\legacy-kg\components`. Paths below are relative to it.
- Write exactly: `technical/flows/manage-ftp-servers-and-credentials/parts/manage-ftp-server-credentials/RULES.md`
- Playbook: `inputs/playbook/part.md`
- The other parts of the flow: `technical/flows/manage-ftp-servers-and-credentials/PARTS.yaml` (leave their doors to them)

## Doors (where the flow starts)

| Door id | Kind | Name | Where | Endpoints |
| --- | --- | --- | --- | --- |
| `file-transfer-controller-get-ftpserver-credentials` | api | GetFTPServerCredentials | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:26` | - |
| `file-transfer-controller-add-ftpserver-credentials` | api | AddFTPServerCredentials | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:43` | - |
| `file-transfer-controller-create-ftpserver-credentials` | api | CreateFTPServerCredentials | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:52` | - |
| `file-transfer-controller-update-ftpserver-credentials` | api | UpdateFTPServerCredentials | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:68` | - |

## Code to read (the flow's own methods, by file)

- `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs`: AddFTPServerCredentials (43), BuildDataSourceResult (140), BuildRequest (92), BuildResponse (116), CreateFTPServerCredentials (52), CreateRequest (107), FormatInvalidResponse (187), FtpServerCredentialsSave (149), GetFTPServerCredentials (26), GetSuccessMessage (180), SetResponse (123), UpdateFTPServerCredentials (68), UpdateFtpServer (171)
- `ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs`: GetFieldValue (30)

## Stored procedures (definition found in the graph; check for later redefinitions)

None.

## Tables

- `orm:FTP_SERVER` (reads)

## Shared pieces (used, owned elsewhere: describe what the flow uses them for, not their insides)

None.

## Calls into code the system does not own (contracts)

- **hp.hsp.ua3.core.ux.common** 30.0.0: `HP.HSP.UA3.Core.UX.Common.ExtensionMethods.IsNullOrEmpty<T>(System.Collections.Generic.ICollection<T>)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs:33` <HP.HSP.UA3.Core.UX.Data.Messaging.SearchFieldModel> 
- **hp.hsp.ua3.core.ux.common** 30.0.0: `HP.HSP.UA3.Core.UX.Configuration.Helpers.ContextHelper.InitializeSearchRequest(Kendo.Mvc.UI.DataSourceRequest)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:34` request=<request>
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ILocalizationProvider.GetMessage(string, string, string)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:182` localeId=<this.ContextHelper.UserConfigurationManager.UserProfile.LocalCode>, contentId=<contentId>
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ITenantConfigurationManager.Resolve<T>(string)` (3 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:31` <HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer.IFileTransferQueryServiceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:55` <HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer.IFileTransferServiceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.FileTransferServiceDataProvider>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:73` <HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer.IFileTransferServiceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.FileTransferServiceDataProvider>
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.IUserConfigurationManager.GenerateRequestor()` (2 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:103` 
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:111` 
- **hp.hsp.ua3.core.ux.services** 30.0.0: `HP.HSP.UA3.Core.UX.Services.Controllers.Base.BaseApiController.FormatResultModel(System.Web.Http.ModelBinding.ModelStateDictionary)` (2 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:65` modelState=<this.ModelState>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:88` modelState=<this.ModelState>
- **microsoft.aspnet.webapi.core** 5.3.0: `System.Net.Http.HttpRequestMessageExtensions.CreateResponse<T>(System.Net.Http.HttpRequestMessage, System.Net.HttpStatusCode, T)` (8 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:40` <System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.UX.Data.FileTransfer.FtpServerModel>> statusCode='404', value=<new List<FtpServerModel>()>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:119` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<ftpServerResult.Result>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:120` <Kendo.Mvc.UI.DataSourceResult> statusCode='200', value=<this.BuildDataSourceResult(ftpServerResult)>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:65` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<this.FormatResultModel(this.ModelState)>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:85` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<this.FormatInvalidResponse(processorResult.Result.Messages[0].Message)>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:88` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<this.FormatResultModel(this.ModelState)>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:136` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<serviceResponse.Result>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:168` <HP.HSP.UA3.FileTransfer.UX.Data.FileTransfer.FtpServerModel> statusCode='200', value=<ftpServerModel>

## Where the graph's walk stops (it sees nothing these call: keep reading the code from here)

Often a router, a call into a package, reflection or configuration. Follow the code to the data.

- `FileTransferController.FormatInvalidResponse` at `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:187`
