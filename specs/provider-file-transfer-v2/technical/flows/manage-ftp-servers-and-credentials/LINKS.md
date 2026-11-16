---
type: Flow Links
title: Links
description: Shared pieces, other flows and external packages the flow relies on.
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
  body_sha256: 81b1920cf200cbf0f1d7dc842cd5f8f61ad8b165cf270daef5cd6908e4db99c9
---

# Links

## Shared pieces used

None.

## Related flows

- `configure-source-locations-and-destinations`: shares `orm:FTP_SERVER` with this flow (server definitions and validity).

## Contracts

- **automapper** 12.0.1: `AutoMapper.IMapperBase.Map<TSource, TDestination>(TSource, TDestination)`
  - Used to map command/event objects into event payloads and projection entities; this flow relies on property-to-property mapping into the destination objects. (BRIEF external calls)

- **hp.hsp.ua3.core.ux.common** 30.0.0
  - `HP.HSP.UA3.Core.UX.Common.ExtensionMethods.IsNullOrEmpty<T>(ICollection<T>)`: used to treat missing search fields as empty. (F02-044)
  - `HP.HSP.UA3.Core.UX.Configuration.Helpers.ContextHelper.InitializeSearchRequest(Kendo.Mvc.UI.DataSourceRequest)`: used to normalize incoming list/search requests before building criteria. (F02-039)

- **hp.hsp.ua3.core.ux.interfaces** 30.0.0
  - `HP.HSP.UA3.Core.UX.Interfaces.ITenantConfigurationManager.Resolve<T>(string)`: used to resolve the file-transfer data providers by component name. (F02-011, F02-039)
  - `HP.HSP.UA3.Core.UX.Interfaces.IUserConfigurationManager.GenerateRequestor()`: used to build the requestor information attached to BAS requests. (F02-012, F02-039)
  - `HP.HSP.UA3.Core.UX.Interfaces.ILocalizationProvider.GetMessage(string, string, string)`: used to retrieve the success message text by locale + content id. (F02-061)

- **hp.hsp.ua3.core.ux.services** 30.0.0
  - `HP.HSP.UA3.Core.UX.Services.Controllers.Base.BaseApiController.FormatResultModel(ModelStateDictionary)`: used to format validation errors returned to the UX client. (F02-049, F02-053)

- **hpp.core.bas.cqrs** 30.0.0
  - `HPP.Core.BAS.CQRS.Base.QueryServiceBase.TryRoute<TQuery>(TQuery)` and `HPP.Core.BAS.CQRS.Base.CommandServiceBase.TryRoute(Command)`: used to route queries/commands to their handlers. (F02-001, F02-002, F02-003, F02-017, F02-019, F02-021)
  - `HPP.Core.BAS.CQRS.Base.ProjectionWriterBase<T>.Add(T)` / `Update(id, Action<T>)`: used to apply events into `FTP_SERVER`. (F02-035, F02-036, F02-037)
  - `HPP.Core.BAS.CQRS.Config.IMessageCodesReadOnly.GetMessage(Guid, string)`: used by `BusinessErrorUtility` to retrieve configuration-based messages by tenant + message code. (F02-027)

- **microsoft.aspnet.webapi.core** 5.3.0
  - `HttpRequestMessageExtensions.CreateResponse<T>(HttpRequestMessage, HttpStatusCode, T)`: used to create UX responses with typed bodies and status codes. (F02-013, F02-014, F02-040, F02-041, F02-049, F02-050, F02-053, F02-055, F02-058, F02-059)

- **Unresolved / implicit calls in the graph output** (listed in the brief as `****`)
  - `?.Find`, `?.ToLower`, `Guid.Equals`, `string.ToString`, plus CQRS base methods referenced without a resolved package name.
  - This flow relies on their normal .NET runtime behaviour for string casing, equality checks, and string conversion. (BRIEF external calls)
