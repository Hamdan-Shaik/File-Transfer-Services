---
type: Behavior Specification
title: 'ui-manage-ftp-servers-and-credentials-screens: UX HTTP endpoints backing FTP server + credential maintenance and related lookup/search'
description: 'What the ui-manage-ftp-servers-and-credentials-screens component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 5243bdfd84d0b550e0f8ec4d14d35eea5a7e50fabb082a4e411e7d9a61f5480a
---

# ui-manage-ftp-servers-and-credentials-screens: UX HTTP endpoints backing FTP server + credential maintenance and related lookup/search

This component covers the legacy `FileTransferController` Web API actions used by the UX to (1) upsert file-transfer base configuration, (2) load source-location and FTP-server option lists, (3) browse and save FTP server credentials with paging/filtering, and (4) perform related search/lookup requests (file transfers, tracking results, modules data list, and FTE metadata ids).

In the legacy system these behaviors are implemented as ASP.NET Web API controller actions that resolve tenant-scoped providers from configuration and return `HttpResponseMessage` responses (often shaped for Kendo UI grids). In the target state, these controller actions are not carried over as UI-tier HTTP endpoints; their observable behaviors are implemented as screen-local API calls to the FXFER BFF plus request/response mappers within the `fxfer-mfe` micro-frontend.

## FileTransferController

Legacy ASP.NET Web API controller (partial class) that exposes UX-facing HTTP doors and thin helper methods for request shaping and response formatting.

### AddFileTransferBaseConfiguration(FileTransferConfigurationModel model)
- Purpose: Upsert the submitted file-transfer base configuration by choosing the legacy provider add vs update operation.
- Inputs:
  - `model`: `FileTransferConfigurationModel`.
  - `ModelState`.
- Logic:
  - If `ModelState.IsValid == false`, return `GenerateInvalidModelStateRepsonse()`.
  - If `model.OwnerPhoneNumber != null`, strip hyphens: `model.OwnerPhoneNumber = model.OwnerPhoneNumber.Replace(-, string.Empty)`.
  - Call `GenerateAddFileTransferBaseConfigurationResult(ref model)` to execute the provider operation.
  - Return `GenerateResponse<FileTransferConfigurationModel>(processorResult, model)`.
- Data: Indirect via `IFileTransferServiceDataProvider`.
- Errors:
  - Invalid model state -> `400 BadRequest` with formatted model-state result.
  - Provider status != `OK` -> `400 BadRequest` with provider `ResultModel`.
- Rules: F01-023, F01-024, F01-025, F01-026.

### GenerateAddFileTransferBaseConfigurationResult(ref FileTransferConfigurationModel model)
- Purpose: Resolve the service data provider and execute add vs update based on `FtsSourceLocationId`.
- Inputs:
  - `model` (by ref): must contain `FtsSourceLocationId`.
- Logic:
  - Resolve `IFileTransferServiceDataProvider` using tenant configuration component name `FileTransferServiceDataProvider`.
  - Build `RequestModel<FileTransferConfigurationModel>`:
    - `Requestor = ContextHelper.UserConfigurationManager.GenerateRequestor()`
    - `Data = model`
  - If `string.IsNullOrEmpty(model.FtsSourceLocationId)` call `processor.AddFileTransferBaseConfiguration(request)`; else call `processor.UpdateFileTransferBaseConfiguration(request)`.
  - Return `result.Result`.
- Data: Indirect via provider.
- Errors: Provider exceptions bubble out of provider call; controller does not catch.
- Rules: F01-025.

### GenerateInvalidModelStateRepsonse()
- Purpose: Format invalid model-state and return as HTTP 400.
- Inputs: `ModelState`.
- Logic: `Request.CreateResponse(HttpStatusCode.BadRequest, FormatResultModel(ModelState))`.
- Data: None.
- Errors: None.
- Rules: F01-023.

### GenerateResponse<T>(ResultModel processorResult, dynamic objModelType)
- Purpose: Convert a provider `ResultModel` into an HTTP 200/400 response.
- Inputs:
  - `processorResult`: provider result.
  - `objModelType`: model instance; expected to be castable to `T` and to carry an `Editor` property when errors are applied.
- Logic:
  - If `processorResult.StatusCode == OK`, return `200 OK` with `(T)objModelType`.
  - Else call `SetProcessorResults(processorResult, objModelType.Editor, false)` and return `400 BadRequest` with `processorResult`.
- Data: None.
- Errors:
  - Non-OK provider status -> 400.
  - If `objModelType.Editor` is missing/null, `SetProcessorResults` may throw.
- Rules: F01-026.

### GetSourceLocations()
- Purpose: Return source-location options for the UI as list items.
- Inputs: None.
- Logic:
  - Initialize `listItems = new List<ListItemModel>()`.
  - Resolve `IFileTransferQueryServiceDataProvider` using component name `FileTransferQueryServiceDataProvider` and assign to `FileDataProvider`.
  - Build `RequestModel<string>`:
    - `Requestor = ContextHelper.UserConfigurationManager.GenerateRequestor()`
    - `Data = null`
  - Call `FileDataProvider.GetSourceLocations(request)`.
  - If `resultModel.StatusCode == OK`:
    - When `serviceResponse.Data != null`, map each `SourceLocationModel` to `ListItemModel`.
      - `DisplayFormat = Name`.
      - `Name = ConfigName + (space-hyphen-space) + Address`.
      - `ID = SourceLocationId.ToString()`.
    - Return `200 OK` with `listItems`.
  - Else return `400 BadRequest` with `resultModel`.
- Data: Indirect via `IFileTransferQueryServiceDataProvider`.
- Errors:
  - Provider status != OK -> 400.
- Rules: F01-083, F01-084, F01-085, F01-086, F01-087, F01-088, F01-089, F01-090.

### GetFtpServerDetails()
- Purpose: Return FTP server options for UI selection as list items.
- Inputs: None.
- Logic:
  - Initialize `listItems = new List<ListItemModel>()`.
  - Resolve `IFileTransferQueryServiceDataProvider` using component name `FileTransferQueryServiceDataProvider` and assign to `FileDataProvider`.
  - Build `RequestModel<SearchCriteriaModel>`:
    - `Requestor = ContextHelper.UserConfigurationManager.GenerateRequestor()`
    - `Data = null`
  - Call `FileDataProvider.GetFtpServers(request)`.
  - If `resultModel.StatusCode == OK` and `resp.Data != null`, map each row to a list item.
    - `DisplayFormat = Name`.
    - `Name = FtpServerName`.
    - `ID = FtpServerId.ToString()`.
    - Return `200 OK` with `listItems`.
  - Else return `400 BadRequest` with `resultModel`.
- Data: Indirect; inventory indicates this call ultimately reads `orm:FTP_SERVER`.
- Errors:
  - Provider status != OK -> 400.
  - Provider status OK with null data -> 400.
- Rules: F02-010, F02-011, F02-012, F02-013, F02-014, F02-015, F02-016.

### GetFTPServerCredentials(DataSourceRequest request)
- Purpose: Return FTP server credential rows for a Kendo grid using paging/filtering.
- Inputs:
  - `request`: Kendo `DataSourceRequest` bound from query string.
- Logic:
  - Resolve `IFileTransferQueryServiceDataProvider` using component name `FileTransferQueryServiceDataProvider` and assign to `FileDataProvider`.
  - Build `searchCriteria = ContextHelper.InitializeSearchRequest(request)`.
  - Build `ftpServerRequest = BuildRequest(searchCriteria)`.
  - Call `FileDataProvider.GetFTPServerCredentials(ftpServerRequest, searchCriteria)`.
  - If the response is non-null and `Data` is non-null, return `BuildResponse(ftpServerResponse)`.
  - Else return `404 NotFound` with an empty list.
- Data: Indirect; inventory indicates this call ultimately reads `orm:FTP_SERVER`.
- Errors:
  - Null response or null data -> 404 with empty list.
  - Provider status `Error` -> 400 with `ResultModel` (via `BuildResponse`).
- Rules: F02-039, F02-040, F02-041, F02-043, F02-045, F02-046, F02-047, F02-048.

### BuildRequest(SearchCriteriaModel searchCriteria)
- Purpose: Convert initialized `SearchCriteriaModel` fields into a `RequestModel<FtpServerModel>`.
- Inputs:
  - `searchCriteria`: initialized by `ContextHelper.InitializeSearchRequest`.
- Logic:
  - Create `RequestModel<FtpServerModel>`:
    - `Requestor = ContextHelper.UserConfigurationManager.GenerateRequestor()`.
    - `Data = new FtpServerModel()` with:
      - `FtpServerName = SearchHelper.GetFieldValue(searchCriteria, FtpServerName)`.
      - `FtpDescription = SearchHelper.GetFieldValue(searchCriteria, FtpDescription)`.
      - `FtpUser = SearchHelper.GetFieldValue(searchCriteria, FtpUser)`.
      - `IsValid = true` when the `IsValid` search field is non-empty; otherwise `IsValid = null`.
- Data: None.
- Errors: None.
- Rules: F02-043.

### BuildResponse(ResponseModel<List<FtpServerModel>> ftpServerResult)
- Purpose: Convert provider response into 200/400 for the credentials grid.
- Inputs:
  - `ftpServerResult`: provider response.
- Logic:
  - If `ftpServerResult.Result.StatusCode == Error`, return `400 BadRequest` with the `ResultModel`.
  - Else return `200 OK` with `BuildDataSourceResult(ftpServerResult)`.
- Data: None.
- Errors: Provider `Error` -> 400.
- Rules: F02-041.

### BuildDataSourceResult(ResponseModel<List<FtpServerModel>> ftpServerResult)
- Purpose: Convert a provider response into a Kendo `DataSourceResult`.
- Inputs: provider response with `Data` list and `PagingResults.TotalRowsAvailable`.
- Logic: Return `DataSourceResult { Data = ftpServerResult.Data, Total = ftpServerResult.PagingResults.TotalRowsAvailable }`.
- Data: None.
- Errors: If `PagingResults` is null, this throws.
- Rules: F02-048.

### AddFTPServerCredentials(FtpServerModel ftpServerModel)
- Purpose: Dispatch save request as create vs update based on `FtpServerId`.
- Inputs:
  - `ftpServerModel`: posted credential model.
- Logic:
  - If `FtpServerId` is empty, call `CreateFTPServerCredentials(ftpServerModel)`.
  - Else call `UpdateFTPServerCredentials(ftpServerModel)`.
- Data: Indirect.
- Errors: None directly.
- Rules: F02-038.

### CreateFTPServerCredentials(FtpServerModel addFtpServerCredentials)
- Purpose: Validate posted model and call the BAS add operation.
- Inputs:
  - `addFtpServerCredentials`: posted model.
  - `ModelState`.
- Logic:
  - Resolve `IFileTransferServiceDataProvider` using component name `FileTransferServiceDataProvider` and assign to `DataProvider`.
  - If `ModelState.IsValid`:
    - Call `DataProvider.AddFtpServer(CreateRequest(addFtpServerCredentials))`.
    - Return `SetResponse(Actions.AddFTPServerCredentials, addFtpServerCredentials, null, processorResult)`.
  - Else return `400 BadRequest` with formatted model-state errors.
- Data: Indirect; inventory indicates this ultimately writes `orm:FTP_SERVER`.
- Errors:
  - Invalid model state -> 400 formatted errors.
  - Provider status != OK -> 400 with `ResultModel`.
- Rules: F02-049, F02-050.

### UpdateFTPServerCredentials(FtpServerModel editFtpServerCredentials)
- Purpose: Validate posted model and call update vs invalidate based on `IsValid`.
- Inputs:
  - `editFtpServerCredentials`.
  - `ModelState`.
- Logic:
  - Resolve `IFileTransferServiceDataProvider` using component name `FileTransferServiceDataProvider` and assign to `DataProvider`.
  - If `ModelState.IsValid`:
    - Call `UpdateFtpServer(editFtpServerCredentials)`.
    - If the response `Data` is non-null, return `SetResponse(Actions.EditFTPServerCredentials, editFtpServerCredentials, null, processorResult)`.
    - Else return `400 BadRequest` with a single error message from `processorResult.Result.Messages[0].Message`.
  - Else return `400 BadRequest` with formatted model-state errors.
- Data: Indirect; inventory indicates this ultimately writes `orm:FTP_SERVER`.
- Errors:
  - Invalid model state -> 400 formatted errors.
  - Update/invalidate response has null data -> 400 with first service message only.
  - Provider status != OK -> 400 with `ResultModel` (via `SetResponse`).
- Rules: F02-053, F02-054, F02-055, F02-057.

### UpdateFtpServer(FtpServerModel editFtpServer)
- Purpose: Choose update vs invalidate BAS operation based on `IsValid`.
- Inputs:
  - `editFtpServer` (expects `IsValid` has a value).
- Logic:
  - Build `request = CreateRequest(editFtpServer)`.
  - If `request.Data.IsValid.Value == true` call `DataProvider.UpdateFtpServer(request)`; else call `DataProvider.InvalidateFtpServer(request)`.
- Data: Indirect.
- Errors: If `IsValid` is null, `IsValid.Value` throws.
- Rules: F02-054.

### CreateRequest(FtpServerModel addFtpServer)
- Purpose: Wrap an FTP server model in a request with a generated requestor.
- Inputs: `addFtpServer`.
- Logic: Return `RequestModel<FtpServerModel> { Requestor = GenerateRequestor(), Data = addFtpServer }`.
- Data: None.
- Errors: None.
- Rules: (supports F02-039).

### SetResponse(string action, FtpServerModel ftpServerModel, HttpResponseMessage response, ResponseModel<FtpServerModel> serviceResponse)
- Purpose: Convert add/update/invalidate provider response into either a saved-model 200 or a 400 error.
- Inputs:
  - `action`: one of `Actions.AddFTPServerCredentials` or `Actions.EditFTPServerCredentials`.
  - `ftpServerModel`: originally submitted model.
  - `serviceResponse`: provider response.
- Logic:
  - If `serviceResponse.Result.StatusCode == OK` and `serviceResponse.Data != null`, replace `ftpServerModel` with `serviceResponse.Data`.
  - If `StatusCode == OK`, return `FtpServerCredentialsSave(action, ftpServerModel)`.
  - Else return `400 BadRequest` with `serviceResponse.Result`.
- Data: None.
- Errors: None.
- Rules: F02-050.

### FtpServerCredentialsSave(string action, FtpServerModel ftpServerModel)
- Purpose: Set a localized success message and return the saved model.
- Inputs:
  - `action`.
  - `ftpServerModel` (expects `Editor` and `IsValid` to be non-null in edit path).
- Logic:
  - Determine `contentId`:
    - Add -> `Localization.Messages.AddFtpServerCredentialsSuccess`.
    - Edit -> when `ftpServerModel.IsValid.Value == true` use `EditFtpServerCredentialsSuccess`; otherwise use `InvalidateFtpServerCredentialsSuccess`.
  - Set `ftpServerModel.Editor.SavedMessage = GetSuccessMessage(contentId)`.
  - Set `ftpServerModel.Editor.SaveActionText = Success`.
  - Return `200 OK` with `ftpServerModel`.
- Data: Reads localization messages via `LocalizationProvider`.
- Errors: Missing `Editor` or missing `IsValid` may throw.
- Rules: F02-050, F02-057, F02-061.

### GetSuccessMessage(string contentId)
- Purpose: Retrieve a localized message text for the current user.
- Inputs: `contentId` (localization key).
- Logic: `LocalizationProvider.GetMessage(UserProfile.LocalCode, contentId).Text`.
- Data: Reads localization store.
- Errors: Provider failures bubble.
- Rules: F02-061.

### FormatInvalidResponse(string message)
- Purpose: Wrap a single error message into a `ResultModel`.
- Inputs: `message`.
- Logic: Return `ResultModel` with `Messages = [ ResultMessageModel { Message = message } ]`.
- Data: None.
- Errors: None.
- Rules: F02-055.

### SearchFileTransfer(DataSourceRequest requestor)
- Purpose: Execute file transfer configuration search and return Kendo grid results.
- Inputs:
  - `requestor`: Kendo `DataSourceRequest` bound from query string.
- Logic:
  - Resolve `IFileTransferQueryServiceDataProvider` using component name `FileTransferQueryServiceDataProvider`.
  - Build `RequestModel<SearchCriteriaModel>`:
    - `Requestor = GenerateRequestor()`
    - `Data = ContextHelper.InitializeSearchRequest(requestor)`
  - Call `FileDataProvider.SearchFileTransfer(request)`.
  - If provider status is OK and data list is non-null, return `200 OK` with `DataSourceResult { Data = data, Total = TotalRowsAvailable }`.
  - Else return `400 BadRequest` with the provider `ResultModel`.
- Data: Indirect.
- Errors:
  - Provider status != OK -> 400.
  - Provider status OK but data null -> 400.
- Rules: F05-009, F05-010, F05-011, F05-036.

### SearchFileTransferTracking(DataSourceRequest request)
- Purpose: Execute tracking search and return Kendo grid results.
- Inputs:
  - `request`: Kendo `DataSourceRequest` bound from query string.
- Logic:
  - Build `RequestModel<SearchCriteriaModel>` with generated requestor and `Data = InitializeSearchRequest(request)`.
  - Resolve `IFileTransferQueryServiceDataProvider` using component name `FileTransferQueryServiceDataProvider`.
  - Call `FileDataProvider.SearchFileTransferTracking(searchRequest)`.
  - If provider status is OK:
    - When data list is non-null, return `200 OK` with `DataSourceResult { Data = data, Total = (PagingResults != null ? TotalRowsAvailable : 0) }`.
    - When data list is null, return `200 OK` with `DataSourceResult { Data = empty list, Total = 0 }`.
  - Else return `400 BadRequest` with provider `ResultModel`.
- Data: Indirect.
- Errors: Provider status != OK -> 400.
- Rules: F05-004, F05-005, F05-006, F05-007, F05-008.

### GetDataListData(string contentID, string startsWith, string localeId)
- Purpose: Return a data-list lookup result for the UI.
- Inputs:
  - `contentID`.
  - `startsWith` (treated as dataListType for internal dispatch).
  - `localeId`.
- Logic:
  - Initialize `listItems` empty.
  - Call internal `GetDataListData(contentID, startsWith, localeId, out listItems)`.
  - When internal result is valid, return `200 OK` with `listItems`.
  - Otherwise return `400 BadRequest` with the internal result.
- Data: Indirect via reference-data provider (modules list).
- Errors: Internal result invalid -> 400.
- Rules: F05-012, F05-013, F05-014.

### GetDataListData(string contentID, string dataListType, string localeId, out List<ListItemModel> listItems)
- Purpose: Internal selector that populates list items for a requested data list type.
- Inputs:
  - `dataListType` (nullable/empty allowed).
- Logic:
  - Initialize `result = new ResultModel()` and `listItems` empty.
  - Treat missing `dataListType` as empty string.
  - Only handles type `MODULE` by calling `GetModules(out listItems)`; all other types return empty `listItems`.
- Data: Indirect via `IReferenceDataProvider` when type is `MODULE`.
- Errors: None.
- Rules: F05-015.

### GetModules(out List<ListItemModel> listItems, bool withModuleID = true)
- Purpose: Fetch active modules and map them into list items.
- Inputs: `withModuleID` (present but not used to alter mapping).
- Logic:
  - Query reference data with: `Module?$filter=IsActive%20eq%20true&$orderby=ModuleName%20asc`.
  - Resolve `IReferenceDataProvider` using component name `DataListReferenceDataProvider`.
  - For each reference-data row:
    - `DisplayFormat = Name`.
    - `Name = ModuleName`.
    - `ID = ModuleID`.
- Data: Reads reference data for active modules.
- Errors: None.
- Rules: F05-016.

### GetFTEMetadatas(string contractName)
- Purpose: Return inbound-target metadata ids for a contract name.
- Inputs: `contractName`.
- Logic:
  - Resolve `IFileTransferQueryServiceDataProvider` using component name `FileTransferQueryServiceDataProvider`.
  - Build `RequestModel<SearchCriteriaModel>` with generated requestor and `Data = null`.
  - Call `FileDataProvider.GetMetadataIds(request, contractName)`.
  - Return `200 OK` with `Data` whenever the response and its `Data` list are non-null (even when empty).
  - Otherwise return `404 NotFound` with an empty list.
- Data: Indirect.
- Errors: None.
- Rules: F05-017, F05-018.

## Rules implemented

| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-023 | AddFileTransferBaseConfiguration rejects invalid models with HTTP 400. | FileTransferController.AddFileTransferBaseConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-024 | When owner phone number is present, remove hyphens before processing. | FileTransferController.AddFileTransferBaseConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-025 | When FtsSourceLocationId is empty, call provider add; otherwise call provider update. | FileTransferController.GenerateAddFileTransferBaseConfigurationResult | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-026 | Return 200 with submitted model on OK; otherwise return 400 with ResultModel. | FileTransferController.GenerateResponse | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-083 | GetSourceLocations is a GET endpoint and requires anti-forgery validation. | FileTransferController.GetSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-084 | GetSourceLocations resolves query provider by component name FileTransferQueryServiceDataProvider. | FileTransferController.GetSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-085 | GetSourceLocations calls provider with generated requestor and null data payload. | FileTransferController.GetSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-086 | GetSourceLocations returns 200 list-items on OK; otherwise returns 400 ResultModel. | FileTransferController.GetSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-087 | Each returned source location becomes a list item named ConfigName - Address with id as string. | FileTransferController.GetSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-088 | Provider GetSourceLocations always queries service SearchSourceLocations with IsActive = true. | FileTransferController.GetSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-089 | Provider maps service FtsSourceLocationId GUID to string SourceLocationId. | FileTransferController.GetSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-090 | Provider sorts returned source locations by ConfigName ascending. | FileTransferController.GetSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F02-010 | GetFtpServerDetails is a GET endpoint and requires anti-forgery validation. | FileTransferController.GetFtpServerDetails | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-011 | GetFtpServerDetails resolves query provider by component name FileTransferQueryServiceDataProvider. | FileTransferController.GetFtpServerDetails | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-012 | GetFtpServerDetails builds request with generated requestor and Data = null then calls GetFtpServers. | FileTransferController.GetFtpServerDetails | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-013 | On OK, GetFtpServerDetails returns 200 with items where Name is server name and ID is server GUID string. | FileTransferController.GetFtpServerDetails | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-014 | When status is not OK or data is null, GetFtpServerDetails returns 400 with ResultModel. | FileTransferController.GetFtpServerDetails | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-015 | Data provider GetFtpServers always queries BAS with Where.IsValid = true. | FileTransferController.GetFtpServerDetails | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-016 | Data provider calls BAS GetFtpServers, maps each result to internal model, and formats errors on exceptions. | FileTransferController.GetFtpServerDetails | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-038 | When FtpServerId is empty on an add request, treat it as create; otherwise treat it as update. | FileTransferController.AddFTPServerCredentials | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-039 | Get credentials builds search criteria from grid request, builds RequestModel from criteria, and queries provider. | FileTransferController.GetFTPServerCredentials | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-040 | When response or data list is null, get credentials returns 404 with empty list. | FileTransferController.GetFTPServerCredentials | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-041 | When status is Error return 400 ResultModel; otherwise return 200 with DataSourceResult containing Data and Total. | FileTransferController.BuildResponse | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-043 | Any non-empty IsValid search field value forces filtering to IsValid = true; otherwise do not set IsValid filter. | FileTransferController.BuildRequest | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-045 | When querying credentials, passwords are removed from results by default. | FileTransferController.GetFTPServerCredentials | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-046 | Paging uses provided page size and page number, and only the last sort criterion is used when multiple sorts are provided. | FileTransferController.GetFTPServerCredentials | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-047 | SearchMode is translated into service-side filter type (Contains, StartsWith, EndsWith, EqualTo), defaulting to EqualTo. | FileTransferController.GetFTPServerCredentials | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-048 | TotalRowsAvailable comes from service-reported row count, not just returned row count. | FileTransferController.BuildDataSourceResult | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-049 | When create model state is invalid, return 400 with formatted model state errors. | FileTransferController.CreateFTPServerCredentials | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-050 | On OK add, return 200 with localized success message in Editor.SavedMessage and Editor.SaveActionText = Success. | FileTransferController.FtpServerCredentialsSave | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-053 | When update model state is invalid, return 400 with formatted model state errors. | FileTransferController.UpdateFTPServerCredentials | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-054 | IsValid true calls Update; IsValid false calls Invalidate. | FileTransferController.UpdateFtpServer | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-055 | When update or invalidate response has no Data, return 400 with a single error message from first service message. | FileTransferController.UpdateFTPServerCredentials | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-057 | Success message differs for update vs invalidation. | FileTransferController.FtpServerCredentialsSave | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-061 | Success messages come from localization provider using user locale code and content id, using returned Text. | FileTransferController.GetSuccessMessage | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F05-004 | Tracking search endpoint requires anti-forgery token and binds DataSourceRequest from query. | FileTransferController.SearchFileTransferTracking | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-005 | Tracking search sends requestor and initialized search criteria from DataSourceRequest to provider. | FileTransferController.SearchFileTransferTracking | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-006 | On OK, tracking search returns 200 with DataSourceResult whose Total defaults to 0 when paging results are missing. | FileTransferController.SearchFileTransferTracking | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-007 | On OK with no data, tracking search still returns 200 with empty result set and total 0. | FileTransferController.SearchFileTransferTracking | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-008 | When status is not OK, tracking search returns 400 with ResultModel. | FileTransferController.SearchFileTransferTracking | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-009 | File transfer search endpoint requires anti-forgery token and binds DataSourceRequest from query. | FileTransferController.SearchFileTransfer | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-010 | File transfer search sends requestor and initialized search criteria from DataSourceRequest to provider. | FileTransferController.SearchFileTransfer | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-011 | File transfer search returns 200 only when status OK and data list non-null; otherwise returns 400. | FileTransferController.SearchFileTransfer | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-012 | Data list lookup endpoint requires anti-forgery token and routes through internal GetDataListData helper with out parameter. | FileTransferController.GetDataListData | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-013 | When internal result is valid, data list lookup returns 200 with list items as body. | FileTransferController.GetDataListData | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-014 | When internal result is not valid, data list lookup returns 400 with result payload. | FileTransferController.GetDataListData | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-015 | Missing dataListType is treated as empty string and only MODULE type populates results. | FileTransferController.GetDataListData (internal) | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-016 | Module data list fetches active modules ordered by module name ascending and maps ModuleName to Name and ModuleID to ID. | FileTransferController.GetModules | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-017 | FTE metadata lookup requires anti-forgery token and calls provider GetMetadataIds using generated requestor and supplied contract name. | FileTransferController.GetFTEMetadatas | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-018 | FTE metadata lookup returns 200 whenever data list is non-null (including empty list); returns 404 with empty list only when response or data list is null. | FileTransferController.GetFTEMetadatas | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-036 | Provider builds SourceLocationQuery only when paging criteria is present; otherwise calls query service with null query. | FileTransferController.SearchFileTransfer | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |

## Not carried over

- FileTransferController (ASP.NET Web API controller plumbing): dropped in favor of `fxfer-mfe` screen-local API calls to `fxfer-bff`; behavior is preserved, but controller routing, HttpResponseMessage construction, and ValidateAntiForgeryToken attribute wiring are not carried forward.
