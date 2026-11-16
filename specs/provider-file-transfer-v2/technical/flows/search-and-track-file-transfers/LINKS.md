---
type: Flow Links
title: 'Links: Search and track file transfers'
description: Shared pieces, other flows and external packages the flow relies on.
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
  body_sha256: 9b6589cfd688c2123eed21693d31d67fb531c89a99dcd9c70449826e93c38359
---

# Links: Search and track file transfers

## Shared pieces used

None.

## Related flows

None.

## Contracts

### `hp.hsp.ua3.core.ux.common` 30.0.0

- `HP.HSP.UA3.Core.UX.Configuration.Helpers.ContextHelper.BuildMvcUrl(string, string, string)`
  - Used to build the cancel navigation target with `controller = Home` and `action = MyHome`. (F05-001)
  - This flow relies on receiving a usable navigation URL string.
- `HP.HSP.UA3.Core.UX.Configuration.Helpers.ContextHelper.InitializeSearchRequest(Kendo.Mvc.UI.DataSourceRequest)`
  - Used to translate the incoming search request into `SearchCriteriaModel` for the provider calls. (F05-005, F05-010)
  - This flow relies on the returned criteria matching the caller’s filters, ordering, and paging.

### `hp.hsp.ua3.core.ux.interfaces` 30.0.0

- `HP.HSP.UA3.Core.UX.Interfaces.IUserConfigurationManager.GenerateRequestor()`
  - Used to populate `RequestModel.Requestor` for provider calls. (F05-005, F05-010, F05-017)
  - This flow relies on the requestor representing the current signed-in user and tenant.
- `HP.HSP.UA3.Core.UX.Interfaces.ITenantConfigurationManager.Resolve<T>(string)`
  - Used to resolve `IFileTransferQueryServiceDataProvider` and `IReferenceDataProvider` by configured component names. (F05-005, F05-010, F05-016, F05-017)
  - This flow relies on each resolved provider being present and compatible with the expected interface.
- `HP.HSP.UA3.Core.UX.Interfaces.ILocalizationProvider.GetLabel(string, string, string)`
  - Used to localize the “Search” label using the user’s locale code. (F05-002)
  - This flow relies on the returned label having a `.Text` value.
- `HP.HSP.UA3.Core.UX.Interfaces.IReferenceDataProvider.GetReferenceData(string)`
  - Used to retrieve active modules via the query `Module?$filter=IsActive%20eq%20true&$orderby=ModuleName%20asc`. (F05-016)
  - This flow relies on each returned row having `ModuleName` and `ModuleID` keys.

### `hp.hsp.ua3.core.ux.presentation` 30.0.0

- `HP.HSP.UA3.Core.UX.Presentation.Controllers.Base.BaseViewController.InitializeEditor(...)`
  - Used to initialize the tracking search editor with `entityLabelName = FileTransfer`, `step = 1`, `stepCount = 1`, and a cancel URL pointing to My Home. (F05-001)
- `HP.HSP.UA3.Core.UX.Presentation.Controllers.Base.BaseViewController.View(object)`
  - Used to render the tracking search page with the criteria model. (F05-003)

### `hpp.core.bas.cqrs` 30.0.0

- `HPP.Core.BAS.CQRS.Base.QueryServiceBase.TryRoute<TQuery>(TQuery)`
  - Used by the query-service doors to route `FileTransferTrackingQuery` and `InboundTargetQuery` to their handlers. (F05-019, F05-021)
  - This flow relies on the routed query returning with its `Results` populated.

### `microsoft.aspnet.webapi.core` 5.3.0

- `System.Net.Http.HttpRequestMessageExtensions.CreateResponse<T>(HttpRequestMessage, HttpStatusCode, T)`
  - Used to return OK / bad-request / not-found answers for the UX service doors. (F05-006, F05-008, F05-011, F05-013, F05-014, F05-018)

### `****`

- `QueryServiceBase.TryRoute`
  - Used by `FileTransferSvcQueryService.TryRouteQuery(...)` in the legacy WCF implementation. (F05-019, F05-021)
