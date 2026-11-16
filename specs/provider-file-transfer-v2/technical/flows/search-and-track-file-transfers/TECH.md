---
type: Technical Flow Map
title: 'Technical map: Search and track file transfers'
description: 'How the legacy code implements the flow: call chains, classes, data, outside calls.'
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
  body_sha256: 0f17191f632d59c1083b7e5bd97dc2bc4b68dce95e18c44fad7a83ab2dcf9ca5
---

# Technical map: Search and track file transfers

How the legacy code implements the flow `search-and-track-file-transfers`, as the code graph sees it. Written by the tool from `FOOTPRINT.yaml`, not by AI; the business view is in `FLOW.md` and `RULES.md`.

## Doors

| Door | Kind | Where |
| --- | --- | --- |
| `file-transfer-controller-search-file-transfer-tracking` (SearchFileTransferTracking) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:24` |
| `file-transfer-controller-search-file-transfer-tracking-2` (SearchFileTransferTracking) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:24` |
| `file-transfer-controller-search-file-transfer` (SearchFileTransfer) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:28` |
| `file-transfer-svc-query-service-search-file-transfer-movement` (SearchFileTransferMovement) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:30` |
| `file-transfer-svc-query-service-search-file-transfer-movement-2` (SearchFileTransferMovement) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:21` |
| `file-transfer-svc-query-service-search-file-transfer-movement-3` (SearchFileTransferMovement) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:21` |
| `file-transfer-svc-query-service-controller-search-file-transfer-movement` (SearchFileTransferMovement) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:27` |
| `file-transfer-svc-query-service-search-inbound-target` (SearchInboundTarget) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:45` |
| `file-transfer-svc-query-service-search-inbound-target-2` (SearchInboundTarget) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:36` |
| `file-transfer-svc-query-service-search-inbound-target-3` (SearchInboundTarget) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:36` |
| `file-transfer-svc-query-service-controller-search-inbound-target` (SearchInboundTarget) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:45` |
| `file-transfer-controller-get-ftemetadatas` (GetFTEMetadatas) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:160` |
| `file-transfer-controller-get-data-list-data` (GetDataListData) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:63` |
| `file-transfer-controller-get-data-list-data-2` (GetDataListData) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:90` |

## Call chains

### `file-transfer-controller-search-file-transfer-tracking`

- `FileTransferController.SearchFileTransferTracking` `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:24`

### `file-transfer-controller-search-file-transfer-tracking-2`

- `FileTransferController.SearchFileTransferTracking` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:24`

### `file-transfer-controller-search-file-transfer`

- `FileTransferController.SearchFileTransfer` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:28`

### `file-transfer-svc-query-service-search-file-transfer-movement`

- `FileTransferSvcQueryService.SearchFileTransferMovement` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:30`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55`

### `file-transfer-svc-query-service-search-file-transfer-movement-2`

- `FileTransferSvcQueryService.SearchFileTransferMovement` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:21`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`

### `file-transfer-svc-query-service-search-file-transfer-movement-3`

- `FileTransferSvcQueryService.SearchFileTransferMovement` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:21`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46`

### `file-transfer-svc-query-service-controller-search-file-transfer-movement`

- `FileTransferSvcQueryServiceController.SearchFileTransferMovement` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:27`
  - `IFileTransferSvcQueryService.SearchFileTransferMovement` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcQueryService.cs:59`
    - implementation: `FileTransferSvcQueryService.SearchFileTransferMovement` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:21`
      - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`

### `file-transfer-svc-query-service-search-inbound-target`

- `FileTransferSvcQueryService.SearchInboundTarget` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:45`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55`

### `file-transfer-svc-query-service-search-inbound-target-2`

- `FileTransferSvcQueryService.SearchInboundTarget` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:36`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`

### `file-transfer-svc-query-service-search-inbound-target-3`

- `FileTransferSvcQueryService.SearchInboundTarget` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:36`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46`

### `file-transfer-svc-query-service-controller-search-inbound-target`

- `FileTransferSvcQueryServiceController.SearchInboundTarget` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:45`
  - `IFileTransferSvcQueryService.SearchInboundTarget` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcQueryService.cs:68`
    - implementation: `FileTransferSvcQueryService.SearchInboundTarget` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:36`
      - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`

### `file-transfer-controller-get-ftemetadatas`

- `FileTransferController.GetFTEMetadatas` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:160`

### `file-transfer-controller-get-data-list-data`

- `FileTransferController.GetDataListData` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:63`
  - `FileTransferController.GetDataListData` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:90`
    - `FileTransferController.GetModules` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:181`

### `file-transfer-controller-get-data-list-data-2`

- `FileTransferController.GetDataListData` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:90`
  - `FileTransferController.GetModules` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:181`

## Classes involved (by namespace; methods walked)

- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc`: `FileTransferSvcQueryService` (3)
- `HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers`: `FileTransferController` (1)
- `HP.HSP.UA3.FileTransfer.UX.Services.Controller`: `FileTransferController` (6)
- `HPP.FileTransfer.BAS.FileTransferApi.Controllers`: `FileTransferSvcQueryServiceController` (2)
- `HPP.FileTransfer.BAS.FileTransferApi.Interfaces`: `IFileTransferSvcQueryService` (2)
- `HPP.FileTransfer.BAS.FileTransferApi.Queries`: `FileTransferSvcQueryService` (3)
- `HPP.FileTransfer.BAS.FileTransferSvc`: `FileTransferSvcQueryService` (3)

## Data

None reached in the graph.

## Calls into code the system does not own

- **hp.hsp.ua3.core.ux.common** 30.0.0: `HP.HSP.UA3.Core.UX.Configuration.Helpers.ContextHelper.BuildMvcUrl(string, string, string)` (1 call sites)
- **hp.hsp.ua3.core.ux.common** 30.0.0: `HP.HSP.UA3.Core.UX.Configuration.Helpers.ContextHelper.InitializeSearchRequest(Kendo.Mvc.UI.DataSourceRequest)` (2 call sites)
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ILocalizationProvider.GetLabel(string, string, string)` (1 call sites)
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.IReferenceDataProvider.GetReferenceData(string)` (1 call sites)
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ITenantConfigurationManager.Resolve<T>(string)` (4 call sites)
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.IUserConfigurationManager.GenerateRequestor()` (3 call sites)
- **hp.hsp.ua3.core.ux.presentation** 30.0.0: `HP.HSP.UA3.Core.UX.Presentation.Controllers.Base.BaseViewController.InitializeEditor(string, int, int, HP.HSP.UA3.Core.UX.Data.Common.EditModeType, HP.HSP.UA3.Core.UX.Data.Common.EditDirectionType, string, bool, bool, bool, bool)` (1 call sites)
- **hp.hsp.ua3.core.ux.presentation** 30.0.0: `HP.HSP.UA3.Core.UX.Presentation.Controllers.Base.BaseViewController.View(object)` (1 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.QueryServiceBase.TryRoute<TQuery>(TQuery)` (2 call sites)
- **microsoft.aspnet.webapi.core** 5.3.0: `System.Net.Http.HttpRequestMessageExtensions.CreateResponse<T>(System.Net.Http.HttpRequestMessage, System.Net.HttpStatusCode, T)` (8 call sites)
- **** : `QueryServiceBase.TryRoute` (1 call sites)

## Where the walk stops

The graph sees nothing these methods call: a router, a call into an unresolved package, reflection, configuration, or simply a leaf. Read the code from here.

Nowhere: every path ends in data, a package call or a leaf the graph understands.

## Shared pieces used (not walked)

None.
