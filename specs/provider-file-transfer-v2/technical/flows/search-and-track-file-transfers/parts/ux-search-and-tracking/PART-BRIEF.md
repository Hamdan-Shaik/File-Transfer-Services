---
type: Generation Brief
title: 'Brief: rules of part ux-search-and-tracking of flow search-and-track-file-transfers'
description: Instructions the tool gave the writer session for one part of this flow.
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController
tags:
- api
- flow
- provider-file-transfer-v2
- wcf
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T09:12:32Z
sources:
- id: door-file-transfer-controller-search-file-transfer-tracking
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.Tracking.cs#L24
  title: SearchFileTransferTracking (api door)
- id: door-file-transfer-controller-search-file-transfer-tracking-2
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.Tracking.cs#L24
  title: SearchFileTransferTracking (api door)
- id: door-file-transfer-controller-search-file-transfer
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L28
  title: SearchFileTransfer (api door)
- id: door-file-transfer-svc-query-service-search-file-transfer-movement
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L30
  title: SearchFileTransferMovement (wcf door)
- id: door-file-transfer-svc-query-service-search-file-transfer-movement-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L21
  title: SearchFileTransferMovement (wcf door)
- id: door-file-transfer-svc-query-service-search-file-transfer-movement-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L21
  title: SearchFileTransferMovement (wcf door)
- id: door-file-transfer-svc-query-service-controller-search-file-transfer-movement
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L27
  title: SearchFileTransferMovement (api door)
- id: door-file-transfer-svc-query-service-search-inbound-target
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L45
  title: SearchInboundTarget (wcf door)
- id: door-file-transfer-svc-query-service-search-inbound-target-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L36
  title: SearchInboundTarget (wcf door)
- id: door-file-transfer-svc-query-service-search-inbound-target-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L36
  title: SearchInboundTarget (wcf door)
- id: door-file-transfer-svc-query-service-controller-search-inbound-target
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L45
  title: SearchInboundTarget (api door)
- id: door-file-transfer-controller-get-ftemetadatas
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L160
  title: GetFTEMetadatas (api door)
- id: door-file-transfer-controller-get-data-list-data
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L63
  title: GetDataListData (api door)
- id: door-file-transfer-controller-get-data-list-data-2
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L90
  title: GetDataListData (api door)
- id: footprint
  resource: /technical/flows/search-and-track-file-transfers/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:search-and-track-file-transfers
  generation:
    status: done
    attempts: 1
  rule_prefix: F05
  rules: 36
  doors:
  - file-transfer-controller-search-file-transfer-tracking
  - file-transfer-controller-search-file-transfer-tracking-2
  - file-transfer-controller-search-file-transfer
  - file-transfer-svc-query-service-search-file-transfer-movement
  - file-transfer-svc-query-service-search-file-transfer-movement-2
  - file-transfer-svc-query-service-search-file-transfer-movement-3
  - file-transfer-svc-query-service-controller-search-file-transfer-movement
  - file-transfer-svc-query-service-search-inbound-target
  - file-transfer-svc-query-service-search-inbound-target-2
  - file-transfer-svc-query-service-search-inbound-target-3
  - file-transfer-svc-query-service-controller-search-inbound-target
  - file-transfer-controller-get-ftemetadatas
  - file-transfer-controller-get-data-list-data
  - file-transfer-controller-get-data-list-data-2
  tables: 0
  procedures: 0
  components:
  - backend-configure-source-locations-and-destinations-api
  - ui-configure-source-locations-and-destinations-screens
  - ui-manage-ftp-servers-and-credentials-screens
  body_sha256: 35a2c7366f896f647277fc527a696b51900bf0820c314fb32f2cb75815b6e883
---

# Brief: rules of part `ux-search-and-tracking` of flow `search-and-track-file-transfers`

- Part: **Search and track file transfers (UX)**, of the flow **Search and track file transfers**
- Rule-id prefix: **P1** (first rule P1-001); the tool renumbers them into the flow later
- Source root (read-only): `C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\legacy-kg\components`. Paths below are relative to it.
- Write exactly: `technical/flows/search-and-track-file-transfers/parts/ux-search-and-tracking/RULES.md`
- Playbook: `inputs/playbook/part.md`
- The other parts of the flow: `technical/flows/search-and-track-file-transfers/PARTS.yaml` (leave their doors to them)

## Doors (where the flow starts)

| Door id | Kind | Name | Where | Endpoints |
| --- | --- | --- | --- | --- |
| `file-transfer-controller-search-file-transfer-tracking` | api | SearchFileTransferTracking | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:24` | GET /FileTransfer |
| `file-transfer-controller-search-file-transfer-tracking-2` | api | SearchFileTransferTracking | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:24` | - |
| `file-transfer-controller-search-file-transfer` | api | SearchFileTransfer | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:28` | - |
| `file-transfer-controller-get-data-list-data` | api | GetDataListData | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:63` | - |
| `file-transfer-controller-get-data-list-data-2` | api | GetDataListData | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:90` | - |
| `file-transfer-controller-get-ftemetadatas` | api | GetFTEMetadatas | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:160` | - |

## Code to read (the flow's own methods, by file)

- `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs`: SearchFileTransferTracking (24)
- `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs`: GetDataListData (63), GetDataListData (90), GetFTEMetadatas (160), GetModules (181), SearchFileTransfer (28)
- `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs`: SearchFileTransferTracking (24)

## Stored procedures (definition found in the graph; check for later redefinitions)

None.

## Tables

None.

## Shared pieces (used, owned elsewhere: describe what the flow uses them for, not their insides)

None.

## Calls into code the system does not own (contracts)

- **hp.hsp.ua3.core.ux.common** 30.0.0: `HP.HSP.UA3.Core.UX.Configuration.Helpers.ContextHelper.BuildMvcUrl(string, string, string)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:36` controller='Home', action='MyHome'
- **hp.hsp.ua3.core.ux.common** 30.0.0: `HP.HSP.UA3.Core.UX.Configuration.Helpers.ContextHelper.InitializeSearchRequest(Kendo.Mvc.UI.DataSourceRequest)` (2 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:33` request=<request>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:41` request=<requestor>
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ILocalizationProvider.GetLabel(string, string, string)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:42` localeId=<this.ContextHelper.UserConfigurationManager.UserProfile.LocalCode>, contentId='Core.Label.Button.Search'
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.IReferenceDataProvider.GetReferenceData(string)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:191` odataQuery=<objDataQuery>
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ITenantConfigurationManager.Resolve<T>(string)` (4 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:37` <HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer.IFileTransferQueryServiceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:35` <HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer.IFileTransferQueryServiceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:189` <HP.HSP.UA3.Core.UX.Interfaces.IReferenceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.DataListReferenceDataProvider>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:165` <HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer.IFileTransferQueryServiceDataProvider> name=<FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider>
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.IUserConfigurationManager.GenerateRequestor()` (3 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:32` 
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:40` 
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:170` 
- **hp.hsp.ua3.core.ux.presentation** 30.0.0: `HP.HSP.UA3.Core.UX.Presentation.Controllers.Base.BaseViewController.InitializeEditor(string, int, int, HP.HSP.UA3.Core.UX.Data.Common.EditModeType, HP.HSP.UA3.Core.UX.Data.Common.EditDirectionType, string, bool, bool, bool, bool)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:30` entityLabelName=<FileTransferConstants.Localization.Entities.FileTransfer>, step='1', stepCount='1', editMode='0', editDirection='0', cancelActionUrl=<this.ContextHelper.BuildMvcUrl(CoreConstants.MVC.Controllers.Home, CoreConstants.MVC.Actions.MyHome)>, isDirtyDataEnabled='False', isResetEnabled='True', isCancelEnabled='True', showSummaryError='True'
- **hp.hsp.ua3.core.ux.presentation** 30.0.0: `HP.HSP.UA3.Core.UX.Presentation.Controllers.Base.BaseViewController.View(object)` (1 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:45` model=<criteria>
- **microsoft.aspnet.webapi.core** 5.3.0: `System.Net.Http.HttpRequestMessageExtensions.CreateResponse<T>(System.Net.Http.HttpRequestMessage, System.Net.HttpStatusCode, T)` (8 call sites)
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:62` <Kendo.Mvc.UI.DataSourceResult> statusCode='200', value=<result>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:66` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<resultModel>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:53` <Kendo.Mvc.UI.DataSourceResult> statusCode='200', value=<result>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:57` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<resultModel>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:80` <System.Collections.Generic.List<HP.HSP.UA3.Core.UX.Data.Common.ListItemModel>> statusCode='200', value=<listItems>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:84` <HP.HSP.UA3.Core.UX.Data.Messaging.ResultModel> statusCode='400', value=<processorResult>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:177` <System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.UX.Data.FileTransfer.InboundTargetModel>> statusCode='200', value=<metadatas.Data>
  - `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:178` <System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.UX.Data.FileTransfer.InboundTargetModel>> statusCode='404', value=<new List<InboundTargetModel>()>
