---
type: Generation Brief
title: 'Brief: component ui-manage-ftp-servers-and-credentials-screens'
description: Instructions the tool gave the writer session for the ui-manage-ftp-servers-and-credentials-screens component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/ui-manage-ftp-servers-and-credentials-screens
tags:
- component
- provider-file-transfer-v2
- ui
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:47:47Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/ui-manage-ftp-servers-and-credentials-screens/INVENTORY.yaml
  title: Every legacy class and method (1 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:ui-manage-ftp-servers-and-credentials-screens
  generation:
    status: done
    attempts: 2
  tier: ui
  classes: 1
  methods: 25
  owned_rules: 51
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - search-and-track-file-transfers
  depends_on:
  - backend-data-file-transfer
  deployable: fxfer-mfe
  folder: src/screens/manage-ftp-servers-and-credentials
  body_sha256: e61cd07ff193b7f49b2909a1a0049c07ed61152d36dfed7a54211454c4601965
---

# Brief: component `ui-manage-ftp-servers-and-credentials-screens`

- Screens of the flow manage-ftp-servers-and-credentials (ui tier, 1 classes, 25 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/ui-manage-ftp-servers-and-credentials-screens/INVENTORY.yaml`
- Write exactly these files in `technical/components/ui-manage-ftp-servers-and-credentials-screens/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-mfe` (`apps/fxfer-mfe`), folder `src/screens/manage-ftp-servers-and-credentials`: write `apps/fxfer-mfe/src/screens/manage-ftp-servers-and-credentials` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-mfe/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

- `backend-data-file-transfer`: `technical/components/backend-data-file-transfer/`

## Flows it takes part in

- `configure-source-locations-and-destinations`: `technical/flows/configure-source-locations-and-destinations/` (FLOW.md, RULES.md) and its scenarios `business/configure-source-locations-and-destinations.feature`
- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`
- `search-and-track-file-transfers`: `technical/flows/search-and-track-file-transfers/` (FLOW.md, RULES.md) and its scenarios `business/search-and-track-file-transfers.feature`

## Packages it calls (contracts to keep)

- `hp.hsp.ua3.core.ux.common`
- `hp.hsp.ua3.core.ux.interfaces`
- `hp.hsp.ua3.core.ux.services`
- `microsoft.aspnet.webapi.core`

## Business rules it owns (51)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-023** The `AddFileTransferBaseConfiguration` HTTP endpoint rejects invalid models with HTTP 400.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:23-28, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:65-68
- Legacy: If `ModelState.IsValid` is false, returns `Request.CreateResponse(HttpStatusCode.BadRequest, FormatResultModel(ModelState))`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-024** When the owner phone number is present, the HTTP endpoint removes hyphens (`-`) before processing.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:30-33
- Legacy: `model.OwnerPhoneNumber = model.OwnerPhoneNumber.Replace("-", string.Empty)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-025** When `FtsSourceLocationId` is empty, the HTTP endpoint calls the provider "add" operation; otherwise it calls the provider "update" operation.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:53-60
- Legacy: Uses `string.IsNullOrEmpty(model.FtsSourceLocationId)` to choose `processor.AddFileTransferBaseConfiguration(request)` vs `processor.UpdateFileTransferBaseConfiguration(request)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-026** The HTTP endpoint returns HTTP 200 with the submitted model when the provider result status is `OK`; otherwise it returns HTTP 400 with the provider `ResultModel`.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:74-82
- Legacy: If `processorResult.StatusCode == ...OK`, returns `Request.CreateResponse(HttpStatusCode.OK, model)`; else calls `SetProcessorResults(...)` and returns `Request.CreateResponse(HttpStatusCode.BadRequest, processorResult)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-083** The UX `GetSourceLocations` HTTP endpoint is a GET endpoint that requires an anti-forgery token.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:24-27
- Legacy: The method is decorated with `[HttpGet]` and `[ValidateAntiForgeryTokenAttribute]`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-084** The UX `GetSourceLocations` HTTP endpoint resolves the file transfer query-service data provider from tenant configuration using the component name `FileTransferQueryServiceDataProvider`.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:31-34
- Legacy: Sets `this.FileDataProvider = TenantConfigurationManager.Resolve<IFileTransferQueryServiceDataProvider>(...FileTransferQueryServiceDataProvider)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-085** The UX `GetSourceLocations` HTTP endpoint calls `GetSourceLocations` with a request that contains the generated requestor and a null data payload.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:35-41
- Legacy: Creates `new RequestModel<string> { Requestor = GenerateRequestor(), Data = null }` and passes it to `FileDataProvider.GetSourceLocations(request)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-086** When the provider response status is `OK`, the UX `GetSourceLocations` HTTP endpoint returns HTTP 200 with a list of list-items; otherwise it returns HTTP 400 with the provider `ResultModel`.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:43-68
- Legacy: On `StatusCodeType.OK`, returns `Request.CreateResponse(HttpStatusCode.OK, listItems)`; otherwise returns `Request.CreateResponse(HttpStatusCode.BadRequest, resultModel)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-087** Each returned source location becomes a list-item whose display name is `<ConfigName> - <Address>` and whose id is the source location id string.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:47-55
- Legacy: Adds `new ListItemModel { DisplayFormat = ...Name, Name = sourceLocation.ConfigName + " - " + sourceLocation.Address, ID = sourceLocation.SourceLocationId.ToString() }`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-088** When the UX provider `GetSourceLocations` is called, it always queries the WCF service `SearchSourceLocations` with `IsActive = true`.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:360-376, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:390-393
- Legacy: Builds a `SourceLocationQuery` whose `Where` is `new SourceLocationQueryParms { IsActive = true }`, then calls `serviceProxy.SearchSourceLocations(serviceQuery)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-089** When the WCF `SearchSourceLocations` call returns results, the UX provider maps each service `SourceLocationModel` to an internal `SourceLocationModel` with `SourceLocationId` set to the service `FtsSourceLocationId` GUID formatted as a string.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:395-401, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:557-569
- Legacy: Iterates `serviceResponse.Results` and maps via `MapToInternalSourceLocationModel`, which maps `SourceLocationId = src.FtsSourceLocationId.ToString()`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-090** The UX provider sorts the returned source locations by `ConfigName` ascending.
- Doors: file-transfer-controller-get-source-locations
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:403
- Legacy: Calls `response.Data.Sort((a1, a2) => a1.ConfigName.CompareTo(a2.ConfigName))`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-010** The UX door for viewing FTP server details is an HTTP `GET` endpoint that requires an anti-forgery token.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:111-113
- Legacy: The action `GetFtpServerDetails()` is decorated with `[HttpGet]` and `[ValidateAntiForgeryTokenAttribute]`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-011** The UX controller resolves the file-transfer query data provider from tenant configuration using the `FileTransferQueryServiceDataProvider` component name.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:119-121
- Legacy: Calls `TenantConfigurationManager.Resolve<IFileTransferQueryServiceDataProvider>(FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider)` and assigns it to `this.FileDataProvider`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-012** To fetch FTP servers for the UI, the UX controller creates a request with a generated requestor and `Data = null`, then calls `FileDataProvider.GetFtpServers`.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:123-130
- Legacy: Sets `Requestor = UserConfigurationManager.GenerateRequestor()` and `Data = null`, then calls `GetFtpServers(request)` and captures `resp.Result`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-013** When the provider result status is `OK`, the UX controller returns `200 OK` with a list of items where `Name` is the FTP server name and `ID` is the FTP server GUID as a string.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:131-151
- Legacy: Builds `ListItemModel` items with `DisplayFormat = Name`, `Name = dataList.FtpServerName`, and `ID = dataList.FtpServerId.ToString()`, then returns them as the HTTP response body.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-014** When the provider result status is not `OK` (or the provider data is null), the UX controller returns `400 BadRequest` with the provider `ResultModel`.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:152-155
- Legacy: Returns `Request.CreateResponse<ResultModel>(HttpStatusCode.BadRequest, resultModel)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-015** The UX data provider always queries the BAS service for FTP servers with `Where.IsValid = true`.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:225-233
- Legacy: Constructs a `FileTransferSvcQueryService.FtpServerQuery` with `Where.IsValid = true` and does not populate additional `Where` filters.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-016** The UX data provider calls the BAS `GetFtpServers` service via a WCF channel, maps each returned result item to an internal `FtpServerModel`, and formats errors on exceptions.
- Doors: file-transfer-controller-get-ftp-server-details
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:237-265, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:529-543
- Legacy: Uses `ChannelFactory<IFileTransferSvcQueryService>` and `serviceProxy.GetFtpServers(serviceQuery)`; on `FaultException<ServiceException>` or general `Exception`, logs the exception and replaces `response.Result` via `FormatErrorResponseResult(...)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-038** When `FtpServerId` is empty on an Add request, the request is treated as a create; otherwise it is treated as an update.
- Doors: file-transfer-controller-add-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:47-49
- Legacy: `AddFTPServerCredentials` calls `CreateFTPServerCredentials` when `ftpServerModel.FtpServerId` is null/empty, else calls `UpdateFTPServerCredentials`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-039** A Get request builds a `SearchCriteriaModel` from the Kendo `DataSourceRequest`, builds a `RequestModel<FtpServerModel>` from that criteria, and queries for FTP server credentials.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:30-36
- Legacy: The controller resolves `IFileTransferQueryServiceDataProvider`, calls `ContextHelper.InitializeSearchRequest(request)`, then calls `GetFTPServerCredentials(requestModel, searchCriteria)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-040** When the query response is missing (null) or has a null `Data` list, the Get request returns HTTP 404 with an empty list.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:38-40
- Legacy: The controller returns `NotFound` with `new List<FtpServerModel>()` unless both `ftpServerResponse` and `ftpServerResponse.Data` are non-null.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-041** When the query response result status is `Error`, the Get request returns HTTP 400 with the `ResultModel`; otherwise it returns HTTP 200 with a Kendo `DataSourceResult` containing `Data` and `Total`.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:118-120, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:140-146
- Legacy: `BuildResponse` maps `StatusCodeType.Error` to `BadRequest` and otherwise returns `OK` with `BuildDataSourceResult`, which sets `Data` and `Total`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-043** When the search request contains a non-empty `IsValid` field value, the server filters to `IsValid = true`; otherwise it does not set an `IsValid` filter.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:101-102
- Legacy: `BuildRequest` sets `IsValid` to `true` when the `IsValid` search field value is not empty, else sets it to null.
- Decision: keep
- âš‘ Flag: the incoming `IsValid` search field value is not parsed; any non-empty value forces `IsValid=true` and there is no way to request `IsValid=false` via this code path.

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-045** When querying FTP server credentials, passwords are removed from results by default.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:858-860, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:995
- Legacy: If `request.Data.RemovePassword` is null, the provider treats it as `true` and maps each result with `FtpPassword = string.Empty`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-046** When paging and sorting criteria are present, the query uses the provided page size and page number, and (when multiple sorts are provided) only the last sort criterion is used.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:924-938, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:941-951
- Legacy: `SetFTPServersPaging` sets `PageSize` and `CurrentPage` from `PagingCriteria`; `SetSortCriteria` uses `SortCriteria[Count - 1]` and maps direction to Asc/Desc.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-047** Each search field's `SearchMode` is translated into a service-side filter type (Contains, StartsWith, EndsWith, or EqualTo), defaulting to EqualTo.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:889-905, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:908-921
- Legacy: `BuildSearchOptions` creates one `SearchModeOption` per `SearchFieldModel`, and `GetSearchType` maps modes with a default of `EqualTo`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-048** The Get credentials response reports `TotalRowsAvailable` based on the service-reported row count, not just the count of returned results.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:955-960
- Legacy: `PopulateResponseResults` sets `PagingResults.RowCount = Results.Count` and `PagingResults.TotalRowsAvailable = queryResults.RowCount`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-049** When the create request model state is invalid, the create request returns HTTP 400 with the formatted model state errors.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:58-65
- Legacy: `CreateFTPServerCredentials` checks `ModelState.IsValid` and otherwise returns `BadRequest` with `FormatResultModel(ModelState)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-050** When an add succeeds (service result status `OK`), the create request returns HTTP 200 with a localized success message stored in `Editor.SavedMessage` and `Editor.SaveActionText` set to `Success`.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:130-137, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:149-168
- Legacy: `SetResponse` routes `StatusCodeType.OK` to `FtpServerCredentialsSave`, which sets `Editor.SavedMessage` and `Editor.SaveActionText` and returns `OK`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-053** When the update request model state is invalid, the update request returns HTTP 400 with the formatted model state errors.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:76-88
- Legacy: `UpdateFTPServerCredentials` checks `ModelState.IsValid` and otherwise returns `BadRequest` with `FormatResultModel(ModelState)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-054** Update requests with `IsValid=true` call the Update operation; update requests with `IsValid=false` call the Invalidate operation.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:175-177
- Legacy: `UpdateFtpServer` calls `DataProvider.UpdateFtpServer` when `IsValid.Value` is true, else calls `DataProvider.InvalidateFtpServer`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-055** When an update/invalidate service response has no `Data`, the update request returns HTTP 400 with a single error message taken from the first service message.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:80-86, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:187-198
- Legacy: When `processorResult.Data` is null, the controller returns `BadRequest` with `FormatInvalidResponse(processorResult.Result.Messages[0].Message)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-057** Update/invalidate success messages differ: a valid update uses the Edit FTP server credentials success message, and an invalidation uses the Invalidate FTP server credentials success message.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:158-162
- Legacy: `FtpServerCredentialsSave` chooses the content id based on `ftpServerModel.IsValid.Value` for the edit action.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-061** Success messages are retrieved from the localization provider using the current user's locale code and the selected content id, and the returned message `Text` is used.
- Doors: file-transfer-controller-create-ftpserver-credentials, file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:180-185
- Legacy: `GetSuccessMessage` calls `LocalizationProvider.GetMessage(userLocaleCode, contentId).Text`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-004** The tracking search results endpoint requires an anti-forgery token and binds a Kendo `DataSourceRequest` from the query.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:24-27
- Legacy: Exposes `[HttpGet]` `SearchFileTransferTracking(DataSourceRequest request)` with `[ValidateAntiForgeryTokenAttribute]` and a `DataSourceRequestModelBinder`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-005** Tracking search requests are sent to the query provider using the current requestor and the initialized search criteria from the incoming `DataSourceRequest`.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:30-34, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:36-40
- Legacy: Builds `RequestModel<SearchCriteriaModel>` with `Requestor=GenerateRequestor()` and `Data=InitializeSearchRequest(request)`, resolves `IFileTransferQueryServiceDataProvider` by component name, then calls `SearchFileTransferTracking(searchRequest)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-006** When the provider result status is OK, the endpoint returns HTTP 200 with a Kendo `DataSourceResult` whose `Total` defaults to 0 when paging results are missing.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:41-52, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:62
- Legacy: If `searchResponse.Result.StatusCode == OK`, returns `200 OK` with `Data=searchResponse.Data` and `Total = (PagingResults != null ? PagingResults.TotalRowsAvailable : 0)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-007** When the provider result status is OK but the provider returns no data, the endpoint still returns HTTP 200 with an empty result set and total 0.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:45-60, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:62
- Legacy: Uses `new List<TrackingSearchResultModel>()` and `Total=0` when `searchResponse.Data == null`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-008** When the provider result status is not OK, the endpoint returns HTTP 400 with the provider `ResultModel`.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:41-43, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:64-67
- Legacy: Returns `BadRequest` with `resultModel` when `resultModel.StatusCode != OK`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-009** The file transfer search endpoint requires an anti-forgery token and binds a Kendo `DataSourceRequest` from the query.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:28-31
- Legacy: Exposes `[HttpGet]` `SearchFileTransfer(DataSourceRequest requestor)` with `[ValidateAntiForgeryTokenAttribute]` and a `DataSourceRequestModelBinder`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-010** File transfer search requests are sent to the query provider using the current requestor and the initialized search criteria from the incoming `DataSourceRequest`.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:34-43
- Legacy: Resolves `IFileTransferQueryServiceDataProvider`, builds `RequestModel<SearchCriteriaModel>` with `Requestor=GenerateRequestor()` and `Data=InitializeSearchRequest(requestor)`, then calls `SearchFileTransfer(request)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-011** The file transfer search endpoint returns HTTP 200 only when the provider status is OK and the provider returns a non-null data list; otherwise it returns HTTP 400.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:44-58
- Legacy: Returns `200 OK` with `DataSourceResult(Data=response1.Data, Total=response1.PagingResults.TotalRowsAvailable)` when `StatusCode == OK && response1.Data != null`, else `400 BadRequest` with `resultModel`.
- Decision: keep
- ⚑ Flag: If the provider returns `StatusCode == OK` but `Data == null`, the endpoint still responds with HTTP 400.

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-012** The data list lookup endpoint requires an anti-forgery token and routes the request through an internal `GetDataListData` helper that returns list items via an out parameter.
- Doors: file-transfer-controller-get-data-list-data
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:63-73
- Legacy: Exposes `[HttpGet] GetDataListData(contentID, startsWith, localeId)`, then calls `GetDataListData(contentID, startsWith, localeId, out listItems)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-013** When the internal processor result is valid, the data list lookup endpoint returns HTTP 200 with the list items as the response body.
- Doors: file-transfer-controller-get-data-list-data
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:74-81
- Legacy: If `processorResult.IsValid`, returns `200 OK` with `listItems`.
- Decision: keep
- ⚑ Flag: The method creates a `DataSourceResult` but returns the raw `List<ListItemModel>` instead of the `DataSourceResult`.

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-014** When the internal processor result is not valid, the data list lookup endpoint returns HTTP 400 with the processor result payload.
- Doors: file-transfer-controller-get-data-list-data
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:74-85
- Legacy: If `!processorResult.IsValid`, returns `400 BadRequest` with `processorResult`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-015** The internal data list selector treats a missing `dataListType` as an empty string and only populates results for the `MODULE` type.
- Doors: file-transfer-controller-get-data-list-data-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:90-106
- Legacy: Normalizes `dataListType` to `string.Empty` when null/empty; `switch (dataListType.ToUpper())` only handles `case "MODULE": GetModules(out listItems)`.
- Decision: keep
- ⚑ Flag: `contentID` and `localeId` are accepted but not used in the internal selector.

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-016** The module data list is fetched as active modules ordered by module name ascending, and each item uses `ModuleName` as `Name` and `ModuleID` as `ID`.
- Doors: file-transfer-controller-get-data-list-data-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:187-205
- Legacy: Queries reference data with `Module?$filter=IsActive%20eq%20true&$orderby=ModuleName%20asc` and maps each row to `ListItemModel(DisplayFormat=Name, Name=<ModuleName>, ID=<ModuleID>)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-017** The FTE metadata lookup endpoint requires an anti-forgery token and calls the query provider for metadata IDs using the current requestor and the supplied contract name.
- Doors: file-transfer-controller-get-ftemetadatas
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:160-175
- Legacy: Resolves `IFileTransferQueryServiceDataProvider`, builds `RequestModel<SearchCriteriaModel>(Requestor=GenerateRequestor(), Data=null)`, and calls `GetMetadataIds(request, contractName)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-018** The FTE metadata lookup endpoint returns HTTP 200 whenever the provider response includes a non-null data list (including an empty list); it returns HTTP 404 with an empty list only when the provider response or its data list is null.
- Doors: file-transfer-controller-get-ftemetadatas
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:176-179
- Legacy: Returns `200 OK` with `metadatas.Data` when `metadatas != null && metadatas.Data != null` (without checking `metadatas.Result.StatusCode`), else `404 NotFound` with `new List<InboundTargetModel>()`.
- Review: GAP-001
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-036** The file transfer configuration search provider builds a `SourceLocationQuery` only when the incoming search criteria includes paging criteria; otherwise it calls the query service with a null query.
- Doors: file-transfer-controller-search-file-transfer
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:86-110
- Legacy: The provider initializes `serviceQuery = null`, sets it only when `searchCriteria != null && searchCriteria.PagingCriteria != null`, and still calls `SearchSourceLocations(serviceQuery)`.
- Review: GAP-003
- Decision: keep

