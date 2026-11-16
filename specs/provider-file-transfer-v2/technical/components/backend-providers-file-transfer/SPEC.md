---
type: Behavior Specification
title: technical/components/backend-providers-file-transfer/SPEC
description: 'What the backend-providers-file-transfer component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-providers-file-transfer
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T20:52:42Z
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
  resource: /technical/components/backend-providers-file-transfer/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-providers-file-transfer
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 4
  methods: 73
  owned_rules: 15
  flows: []
  depends_on: []
  deployable: fxfer-bff
  folder: src/Application/Providers
  body_sha256: aacf97cd0e3d797856eafb039b5fb45dd48aefca09f96af0f020f3147f363562
---

﻿# backend-providers-file-transfer: Legacy UX provider adapters for file transfer

This component contains the legacy UX-layer *data providers* used by the File Transfer UI to call the File Transfer WCF services and to map service DTOs into UI models.
It is called by controller/service code in the UX stack and relies on the shared `BaseServiceDataProvider` plumbing for WCF channel creation and credentials.

## FileTransferServiceDataProvider
Command-oriented provider for file-transfer configuration writes and FTP server credential writes.

### AddFileTransferBaseConfiguration(RequestModel<FileTransferConfigurationModel> request)
- Purpose: Create a new file-transfer base configuration (source location + destinations) by calling the FileTransfer service `FileTransferAdd` operation.
- Inputs:
  - `request.Data`: a `FileTransferConfigurationModel` containing config fields and `DestinationDetails` list.
  - `request.Requestor`: requestor context mapped to the service requestor.
- Logic:
  - Reads tenant application setting `ExternalFTECommonFolder`.
  - Builds `destArray` with one `FtsDestinationLocationAdded` per `request.Data.DestinationDetails`:
    - Copies flags and contact fields.
    - If `destination.ContactPhone != null`, sets `ContactPhone = destination.ContactPhone.Replace("-", string.Empty)`.
    - If `destination.DestinationType.ID == FileToEvent`, overwrites `destination.DestinationAddress = externalFTECommonFolder`.
    - Sets `FtpServerId = (destination.FTPServer.ID == string.Empty) ? Guid.Empty : Guid.Parse(destination.FTPServer.ID)`.
    - Sets `OperatorId` to the current user id (`ContextHelper.UserConfigurationManager.UserProfile.UserId`).
  - Builds `FtsSourceLocationAddCommand` from `request.Data`, including:
    - `RunOnThisDayOfMonth = request.Data.RunOnThisDay.ID != string.Empty ? Convert.ToByte(request.Data.RunOnThisDay.ID) : (byte)0`.
    - `IsSuspendedChangedDate = request.Data.Suspended ? (DateTime?)DateTime.UtcNow : null`.
    - `DestinationAddress = destArray`.
  - Calls WCF: `serviceProxy.FileTransferAdd(cmd)`.
  - If the service response is non-null, maps it to a `FileTransferConfigurationModel` and returns `StatusCode = OK`.
- Data:
  - Reads tenant configuration `ExternalFTECommonFolder`.
  - Writes via service: file-transfer configuration (source location + destinations).
- Errors:
  - `FaultException<ServiceException>` -> `HandleServiceException` (error response).
  - `FaultException<BusinessValidationException>` -> `HandleBusinessException` (error response).
  - Any other `Exception` -> `HandleException` (error response).
- Rules: F01-027, F01-028, F01-029, F01-030, F01-031, F01-032, F01-033.

### UpdateFileTransferBaseConfiguration(RequestModel<FileTransferConfigurationModel> request)
- Purpose: Update an existing file-transfer base configuration by calling the FileTransfer service `UpdateFileTransferSourceLocation` operation.
- Inputs: `request.Data` includes `ConfigId`, `FtsSourceLocationId`, and `DestinationDetails`.
- Logic:
  - Reads tenant application setting `ExternalFTECommonFolder`.
  - Builds `destArray` only from destinations where `IsAddedUpdated == true`.
  - For each included destination:
    - If `DestinationId != null`, parses it into `destinationTempData.DestinationId`.
    - Removes hyphens from `ContactPhone` when non-null.
    - Sets `ContractName` from `destination.ContractName.ID`.
    - If destination type is `FileToEvent`, overwrites `destination.DestinationAddress` with `externalFTECommonFolder`.
    - Sets `FtpServerId` using empty-string -> `Guid.Empty` mapping.
  - Builds `FtsSourceLocationUpdateCommand` mirroring the create command plus `ConfigId` and `FtsSourceLocationId`.
  - Calls WCF: `serviceProxy.UpdateFileTransferSourceLocation(cmd)`.
  - If the service response is non-null, returns `request.Data` with `StatusCode = OK`.
- Data:
  - Reads tenant configuration `ExternalFTECommonFolder`.
  - Writes via service: file-transfer configuration updates.
- Errors: same catch/handler set as `AddFileTransferBaseConfiguration`.
- Rules: F01-027, F01-028, F01-029, F01-030, F01-031, F01-033.

### InvalidateFileTransferBaseConfiguration(RequestModel<FileTransferConfigurationModel> request)
- Purpose: Invalidate (soft-disable) an existing file-transfer base configuration by calling the FileTransfer service `InvalidateFileTransferSourceLocation` operation.
- Inputs: `request.Data` includes `ConfigId`, `FtsSourceLocationId`, and `DestinationDetails`.
- Logic:
  - Builds `destArray` only from destinations where `IsAddedUpdated == true`.
  - For each included destination, maps destination fields and:
    - Removes hyphens from `ContactPhone` when non-null.
    - Sets `FtpServerId` using empty-string -> `Guid.Empty` mapping.
  - Builds `FtsSourceLocationInvalidateCommand` with `ConfigId`, `FtsSourceLocationId`, `OperatorId` (current user id), and `DestinationAddress = destArray`.
  - Calls WCF: `serviceProxy.InvalidateFileTransferSourceLocation(cmd)`.
  - Always returns `request.Data` with `StatusCode = OK` if no exception occurs (no null-response check).
- Data: writes via service: file-transfer configuration invalidation.
- Errors:
  - `FaultException<ServiceException>` -> `HandleServiceException`.
  - Any other `Exception` -> `HandleException`.
- Rules: F01-027, F01-029.

### AddFtpServer(RequestModel<FtpServerModel> request)
- Purpose: Create an FTP server credential/profile record.
- Inputs: `request.Data` includes FTP server fields and an `Editor` object.
- Logic:
  - Initializes an error `ResponseModel<FtpServerModel>` via `BuildFtpServerModel()`.
  - Builds `addFtpServerCommand` via `BuildAddFtpServerCommand(request)`.
  - Calls private `AddFtpServer(request, response)` inside a try/catch.
  - On service/business/other exceptions, logs and sets `response.Result` via legacy `FormatErrorResponseResult(...)` / `HandleFtpServerBusinessExceptions`.
- Data: writes via service: FTP server record.
- Errors: handled in the wrapper method; response status defaults to `Error` unless the internal call sets `OK`.
- Rules: F02-051.

### UpdateFtpServer(RequestModel<FtpServerModel> request)
- Purpose: Update an existing FTP server credential/profile record.
- Inputs: `request.Data.FtpServerId` must be parseable as `Guid`.
- Logic:
  - Initializes an error `ResponseModel<FtpServerModel>`.
  - Builds `updateFtpServerCommand` via `BuildUpdateFtpServerCommand(request)` **before** entering the try/catch.
  - Calls private `UpdateFtpServer(request, response)` inside try/catch.
- Data: writes via service: FTP server record update.
- Errors:
  - Service/business/other exceptions inside the internal call are caught and mapped to `response.Result`.
  - Exceptions thrown while building the update command (notably password lookup failures) are **not** caught here.
- Rules: F02-056, F02-063.

### InvalidateFtpServer(RequestModel<FtpServerModel> request)
- Purpose: Invalidate an FTP server record (mark invalid) via service.
- Inputs: `request.Data.FtpServerId` must be parseable as `Guid`.
- Logic:
  - Initializes an error `ResponseModel<FtpServerModel>`.
  - Builds `invalidateFtpServerCommand` via `BuildInvalidateFtpServerCommand(request)`.
  - Calls private `InvalidateFtpServer(request, response)` inside try/catch.
- Data: writes via service: FTP server invalidation.
- Errors: service/business/other exceptions are caught and mapped to `response.Result`.
- Rules: F02-060.

### RunAddFtpServer(AddFtpServerCommand command, ChannelFactory<IFileTransferSvcService> channelFactory)
- Purpose: Test seam that executes the WCF `AddFtpServer` call.
- Inputs: `command`, `channelFactory`.
- Logic: creates service proxy from factory and calls `serviceProxy.AddFtpServer(command)`.
- Data: writes via service.
- Errors: any thrown exception propagates to caller.

### RunUpdateFtpServer(UpdateFtpServerCommand command, ChannelFactory<IFileTransferSvcService> channelFactory)
- Purpose: Test seam that executes the WCF `UpdateFtpServer` call.
- Logic: creates proxy and calls `serviceProxy.UpdateFtpServer(command)`.

### RunInvalidateFtpServer(InvalidateFtpServerCommand command, ChannelFactory<IFileTransferSvcService> channelFactory)
- Purpose: Test seam that executes the WCF `InvalidateFtpServer` call.
- Logic: creates proxy and calls `serviceProxy.InvalidateFtpServer(command)`.

### HandleServiceException<T>(FaultException<ServiceException> ex)
- Purpose: Convert a service fault into an error `ResponseModel<T>`.
- Logic: logs, creates `GenerateReponse<T>(null, Error)`, then sets `response.Result = FormatErrorResponseResult(ex.Detail.ErrorCode.ToString())`.

### HandleBusinessException<T>(FaultException<BusinessValidationException> ex)
- Purpose: Convert a business-validation fault into an error `ResponseModel<T>`.
- Logic: logs, creates error response, builds a `ResultModel` via `BuildMessage(ex)` and sets status to `Error`.

### BuildMessage(FaultException<BusinessValidationException> ex)
- Purpose: Convert service business messages into a single UI error message.
- Logic:
  - Copies `ex.Detail.BusinessMessages` into a list.
  - Uses `BuildErrorMessage(messagesList)` to build a string.
  - Trims any trailing comma from the built message.
  - Adds one `ResultMessageModel(MessageType.Error, message)` to the result model.

### BuildErrorMessage(List<BusinessExceptionMessage> messagesList)
- Purpose: Format business-exception messages.
- Logic: iterates messages and sets `message = $"{msg.MessageDefault} {msg.FieldName}"` on each iteration, so the returned value reflects the **last** message in the list.

### HandleException<T>(Exception ex)
- Purpose: Convert an unexpected exception into an error `ResponseModel<T>`.
- Logic: logs, creates error response, sets `Result = FormatErrorResponseResult(ex)`.

### GenerateReponse<T>(dynamic data, StatusCodeType statusCode)
- Purpose: Build a `ResponseModel<T>` with `Data = (T)data` and `Result.StatusCode = statusCode`.

### MapToServiceRequestor(RequestorModel internalRequestor)
- Purpose: Map the internal requestor model into the service requestor type.
- Logic: AutoMapper map from `RequestorModel` to `FileTransferConfig.RequestorModel` with `Condition(!IsSourceValueNull)`.

### MapToInternalProvider(FtsSourceLocationAdded serviceProvider)
- Purpose: Map a service `FtsSourceLocationAdded` DTO into a UI `FileTransferConfigurationModel`.
- Logic:
  - Returns null when `serviceProvider` is null.
  - AutoMapper mapping includes:
    - `FTPServer` list-item with `Name = src.FtpServerId`
    - `SourceFileMask = src.Mask`
    - `ProtocolType` list-item with `Name = src.Protocol`
    - `OwnerPhoneNumber = src.OwnerPhone`
    - `OwnerSubsystem` list-item with `Name = src.OwnerModule`
    - Weekday booleans mapped from service fields (`RunOnWed`, `RunOnThrus`, ...)
    - `RunOnThisDay` list-item `Name = src.RunOnThisDayOfMonth.ToString()`
    - `TimeWindowStart/End` list-items from `StartTime`/`EndTime`
    - `SendNotification = src.SendNotifications`
    - `FileDirection` list-item `Name = src.FileDirection`

### AddFtpServer(RequestModel<FtpServerModel> request, ResponseModel<FtpServerModel> response)
- Purpose: Execute the WCF add FTP server command and populate the response.
- Logic:
  - Calls `RunAddFtpServer(addFtpServerCommand, channelFactory)`.
  - If service response is non-null:
    - Sets `response.Data = new FtpServerModel { FtpServerId = serviceResponse.FtpServerId.ToString() }`.
    - Copies `response.Data.Editor = request.Data.Editor`.
    - Sets `response.Result.StatusCode = OK`.
- Rules: F02-052.

### UpdateFtpServer(RequestModel<FtpServerModel> request, ResponseModel<FtpServerModel> response)
- Purpose: Execute the WCF update FTP server command and populate the response.
- Logic:
  - Calls `RunUpdateFtpServer(updateFtpServerCommand, channelFactory)`.
  - If non-null response:
    - Sets `response.Data = new FtpServerModel { FtpServerId = ..., IsValid = serviceResponse.IsValid }`.
    - Copies `Editor`.
    - Sets status `OK`.
- Rules: F02-058.

### InvalidateFtpServer(RequestModel<FtpServerModel> request, ResponseModel<FtpServerModel> response)
- Purpose: Execute the WCF invalidate FTP server command and populate the response.
- Logic: same shape as update, but calls invalidate and maps `FtpServerId` and `IsValid`.
- Rules: F02-059.

### BuildFtpServerModel()
- Purpose: Initialize a `ResponseModel<FtpServerModel>` with `Result.StatusCode = Error`.

### BuildAddFtpServerCommand(RequestModel<FtpServerModel> request)
- Purpose: Build the service `AddFtpServerCommand` from the UI request.
- Logic:
  - Sets `FtpServerId = Guid.NewGuid()`.
  - Sets `OperatorId = request.Requestor.IdentifierId`.
  - Maps password, ports, proxy, transfer mode, etc.
- Rules: F02-051.

### BuildUpdateFtpServerCommand(RequestModel<FtpServerModel> request)
- Purpose: Build the service `UpdateFtpServerCommand`.
- Logic:
  - Parses `ftpServerId = Guid.Parse(request.Data.FtpServerId)`.
  - Determines `ftpServerPass`:
    - If `request.Data.FtpPassword` is non-empty, use it.
    - Else, call `GetFTPServerPass(ftpServerId)`.
  - Builds `UpdateFtpServerCommand` with the resolved password.
- Errors: if `request.Data.FtpServerId` is not a GUID, `Guid.Parse` throws.
- Rules: F02-056, F02-063.

### GetFTPServerPass(Guid ftpServerId)
- Purpose: Fetch the existing password for an FTP server when updating and no new password is provided.
- Logic:
  - Resolves `IFileTransferQueryServiceDataProvider` via IoC.
  - Calls `GetFTPServerCredentials(BuildRequest(ftpServerId), BuildCriteria(ftpServerId))`.
  - If `resp.Result.StatusCode == OK` and `resp.Data != null`, uses `resp.Data.First()` and returns `server.FtpPassword`.
  - Otherwise returns null.
- Errors:
  - If the credentials query returns `OK` and `resp.Data` is a non-null empty list, `First()` throws an unhandled `InvalidOperationException`.
- Rules: F02-056, F02-063.

### BuildRequest(Guid ftpServer)
- Purpose: Build a request model for querying FTP server credentials.
- Logic:
  - Sets `Data = new FtpServerModel { FtpServerId = ftpServer.ToString(), IsValid = true, RemovePassword = false }`.
  - Sets `Requestor = GenerateRequestor()`.
- Rules: F02-056.

### BuildCriteria(Guid ftpServerId)
- Purpose: Build `SearchCriteriaModel` that filters by FTP server id.
- Logic: one search field: `FieldName = FtpServerId`, `SearchMode = EqualTo`, `FieldValue = ftpServerId`.

### BuildInvalidateFtpServerCommand(RequestModel<FtpServerModel> request)
- Purpose: Build the service `InvalidateFtpServerCommand`.
- Logic: sets `FtpServerId = Guid.Parse(request.Data.FtpServerId)` and maps requestor.
- Rules: F02-060.

### HandleFtpServerBusinessExceptions(FaultException<BusinessValidationException> ex)
- Purpose: Convert business-validation messages for FTP server operations.
- Logic:
  - Creates a `ResultModel` with status `Error`.
  - Iterates `ex.Detail.BusinessMessages` and calls `GetErrorMessage(resultModel, err)` for each.

### GetErrorMessage(ResultModel resultModel, BusinessExceptionMessage message)
- Purpose: Map known FTP server validation keys to localized text.
- Logic:
  - For `DuplicatedFTPServerName`, `InvalidIpAddressFormat`, `InvalidFTPProxyPort`: looks up the localized message text and adds it as an error message.
  - Otherwise: adds the raw `message.MessageKey` as the error message.

### GetValidationMessage(string validationMessage, ContextHelper contextHelper)
- Purpose: Resolve a localized validation message.
- Logic: uses `LocalizationProvider.GetMessage(userLocale, validationMessage)`.

## IFileTransferServiceDataProvider
Legacy interface implemented by `FileTransferServiceDataProvider`.

- `AddFileTransferBaseConfiguration(request)`
- `UpdateFileTransferBaseConfiguration(request)`
- `InvalidateFileTransferBaseConfiguration(request)`
- `AddFtpServer(request)`
- `UpdateFtpServer(request)`
- `InvalidateFtpServer(request)`

## FileTransferQueryServiceDataProvider
Query-oriented provider for configuration searches/lookups, FTP server lookups, inbound-target metadata lookups, source-location pick lists, and file-tracking searches/details.

### SearchFileTransferTracking(RequestModel<SearchCriteriaModel> request)
- Purpose: Search file-transfer tracking records.
- Inputs: `request.Data` contains `SearchFields`, `PagingCriteria`, and `SortCriteria`.
- Logic:
  - Builds `FileTransferTrackingQuery` with:
    - `CurrentPage = BuildServicePagingCriteria(pagingCriteria)` (defaults to 1)
    - `PageSize = BuildServicePageSizeCriteria(pagingCriteria)` (defaults to 5)
    - `SortCriteria = BuildServiceSortCriteria(sortCriteria)`
    - `Where = BuildWhereQueryParms(searchFields)`
  - Calls WCF: `SearchFileTransferMovement(trackingQuery)`.
  - When results are non-empty, maps each result with `MapToInternalFileTransferTrackingResultModel`, sets paging row counts, and sets status `OK`.
- Data: reads tracking records via service.
- Errors:
  - On `FaultException<ServiceException>`, formats an error response and sets status `Error`.
  - On other exceptions, formats an error response.
- Rules: F05-035.

### GetFileTransferTrackingDetail(RequestModel<string> request)
- Purpose: Retrieve tracking detail for a single file by file id.
- Inputs: `request.Data` is `fileId`.
- Logic:
  - Builds `FileTransferTrackingQuery` with `Where.FileId = fileId`.
  - Calls `SearchFileTransferMovement`.
  - If a result exists, maps `Results[0]` with `MapToInternalFileTransferTrackingDetailModel`.
  - Sets `response.Result.StatusCode = OK` regardless of whether a result was found.
- Data: reads one tracking record via service.
- Errors: same shape as tracking search.

### BuildWhereQueryParms(List<SearchFieldModel> searchFields)
- Purpose: Map supported UI tracking search fields into service query parameters.
- Inputs: list of search fields.
- Logic:
  - For each supported `FieldName`, sets the corresponding query parameter:
    - `FileId`, `FileName`, `FileStatus`, `Direction`, `TransportType`, `TradingPartnerId`, `SourceLocationId` map directly to string fields.
    - `FromDate` / `ToDate`: applied only when `DateTime.TryParse(...)` succeeds.
    - `FromSize` / `ToSize`: applied only when `decimal.TryParse(...)` succeeds.
  - Unsupported field names are ignored.
- Data: none.
- Errors: if a supported field has `FieldValue == null`, calling `ToString()` will throw (legacy behavior).
- Rules: F05-035.

### MapToInternalFileTransferTrackingResultModel(FileTransferTrackingModel serviceModel)
- Purpose: Map one tracking search result to the UI model and localize its list-item fields.
- Logic: AutoMapper maps direction/transport/status into `ListItemModel { ID = ... }` and maps create date/size/trading partner; then calls `LocalizeObjectProperties`.

### MapToInternalFileTransferTrackingDetailModel(FileTransferTrackingModel serviceModel)
- Purpose: Build detail model containing information header, events list, and exceptions list.
- Logic: delegates to `MapToInternalFileTransferTrackingInformationModel`, `MapToInternalFileTransferTrackingEvents`, `MapToInternalFileTransferTrackingExceptions`.

### MapToInternalFileTransferTrackingInformationModel(FileTransferTrackingModel serviceModel)
- Purpose: Map tracking header information.
- Logic:
  - Maps direction/transport/status into list items.
  - Computes `SourceLocation = SourceLocConfigName + " - " + SourceLocAddress`.
  - Localizes object properties.

### MapToInternalFileTransferTrackingEvents(List<FileEventModel> serviceEvents)
- Purpose: Map tracking events.
- Logic: for each event, maps `CreateDate = CreatedDate` and `Description = EventTrackDescription`, localizes, and adds to list.

### MapToInternalFileTransferTrackingExceptions(List<FileExceptionModel> serviceExceptions)
- Purpose: Map tracking exception records.
- Logic: maps `Date = CreatedDate`, localizes, and adds to list.

### BuildServiceSortCriteria(List<SortCriteriaModel> sortCriteria)
- Purpose: Map supported UI sort fields to service sort criteria for tracking search.
- Logic:
  - Supports (by `SortField`): `FileId`, `FileName`, `FileStatus.FormattedText` -> `FileStatus`, `Direction.FormattedText` -> `FileDirection`, `Size` -> `FileSize`, `CreateDate` -> `CreatedDate`, `TradingPartnerId`.
  - For each supported field, sets `Order` using `BuildServiceSortOrder(sort)`.
  - Unsupported sort fields are ignored.

### BuildServiceSortOrder(SortCriteriaModel sortModel)
- Purpose: Translate sort direction.
- Logic: ascending -> `SortOrder.Asc`, otherwise `SortOrder.Desc`.

### BuildServicePagingCriteria(PagingCriteriaModel pagingCriteria)
- Purpose: Determine service query current page.
- Logic: defaults to `1`; when paging criteria is non-null uses `pagingCriteria.Page`.

### BuildServicePageSizeCriteria(PagingCriteriaModel pagingCriteria)
- Purpose: Determine service query page size.
- Logic: defaults to `5`; when paging criteria is non-null uses `pagingCriteria.RowsPerPage`.

### SearchFileTransfer(RequestModel<SearchCriteriaModel> request)
- Purpose: Search file-transfer configurations (source locations) using paging, sorting, and search fields.
- Logic:
  - Initializes response with empty list, paging `{RowCount=0, TotalRowsAvailable=0}`, and status `OK`.
  - If `searchCriteria` and `searchCriteria.PagingCriteria` are non-null, builds `SourceLocationQuery` with:
    - `CurrentPage = searchCriteria.PagingCriteria.Page`
    - `PageSize = searchCriteria.PagingCriteria.RowsPerPage`
    - `SortCriteria = BuildSourceLocationSortParms(searchCriteria.SortCriteria)`
    - `Where = BuildSourceLocationQueryParms(searchCriteria)`
  - Calls WCF: `SearchSourceLocations(serviceQuery)`.
  - When results are non-empty, maps each using `MapToInternalFileTransferModel`, and sets `PagingResults` from service `RowCount`.
- Data: reads source location configurations via service.
- Errors: service exception -> formatted error result; other exception -> formatted error result.

### GetFileTransferConfigurationInformation(RequestModel<string> request)
- Purpose: Retrieve full configuration detail (source location + destination list) for a specific source location id.
- Logic:
  - Initializes response with empty `FileTransferConfigurationModel` and status `NotFound`.
  - Calls WCF: `GetSourceLocations(GetSourceLocationQuery{ Where.SourceLocationId = request.Data })`.
  - When results are non-empty:
    - Maps `Results[0]` with `MapToInternalFileTransferConfigurationModel`.
    - If `DestinationAddress` is present, maps each destination using `MapToInternalDestinationModel(item, lineNumber)` where `lineNumber` starts at 1.
    - Sets status `OK`.
- Data: reads one configuration via service.

### GetFtpServers(RequestModel<SearchCriteriaModel> request)
- Purpose: Retrieve a list of valid FTP servers.
- Logic:
  - Calls WCF `GetFtpServers` with a query where `IsValid = true`.
  - Maps each result with `MapToInternalFtpServerModel`.

### GetFTPServerCredentials(RequestModel<FtpServerModel> request, SearchCriteriaModel searchCriteria)
- Purpose: Search FTP server records (including credentials) based on criteria and return a paging-aware response.
- Logic:
  - Builds response via `BuildResponseModel<FtpServerModel>()`.
  - Calls `GetFTPServersResponse(request, searchCriteria, response)`.
  - On service fault: `HandleFaultException`.
  - On other exceptions: `HandleException`.

### GetMetadataIds(RequestModel<SearchCriteriaModel> request, string contractName)
- Purpose: Lookup inbound-target metadata (id + import job name) for a given contract.
- Logic:
  - Builds `InboundTargetQuery` using `GetServiceQuery(request, contractName)`.
  - Calls WCF `SearchInboundTarget`.
  - Maps results with `MapToInternalInboundTarget`.

### GetServiceQuery(RequestModel<SearchCriteriaModel> request, string contractName)
- Purpose: Build a service inbound-target query.
- Logic: sets `Where.ContractName = contractName` and `Where.FTEMetadataId = Guid.Empty`.

### GetSourceLocations(RequestModel<string> request)
- Purpose: Return a sorted list of active source locations for pick lists.
- Logic:
  - Calls WCF `SearchSourceLocations` with `Where.IsActive = true`.
  - Maps each result using `MapToInternalSourceLocationModel`.
  - Sorts the resulting list by `ConfigName` ascending.
  - Sets status `OK` (redundantly set to OK even when already OK).
- Errors:
  - Service fault -> formatted error result.
  - General exception -> sets status `Error` and adds a single error message containing `ex.Message` (does not use `FormatErrorResponseResult` here).

### RunService(FtpServerQuery query, IFileTransferSvcQueryService serviceProxy)
- Purpose: Test seam for the WCF `GetFtpServers` query.
- Logic: returns `serviceProxy.GetFtpServers(query)`.

### MapToInternalFileTransferModel(SourceLocationModel serviceModel)
- Purpose: Map a configuration search result (`SourceLocationModel`) to `AddFileTransferConfigurationModel`.
- Logic:
  - Maps owner subsystem/protocol/run-on-this-day/time windows into list items with `ID` set from service values.
  - Maps `Address -> SourceAddress` and `Mask -> SourceFileMask`.
  - Looks up the owner subsystem name via `GetModuleName(model.OwnerSubsystem.ID)`.
  - Sets `Metadata.IsActive` and localizes `Status` label (Active/Inactive) using the current user locale.
  - Localizes object properties.

### MapToInternalFileTransferConfigurationModel(SourceLocationModel serviceModel)
- Purpose: Map a configuration detail record into `FileTransferConfigurationModel`.
- Logic:
  - Maps FTP server name, protocol, owner subsystem, run-on-this-day, time windows, isActive, etc.
  - Uses `AfterMap` to set `dest.FileDirection.ID = src.FileDirection`.
  - Computes `RunSchedule.ID`:
    - If monthly indicators are present (`RunOnLastDay` or `RunOnFirstDay` or a non-empty run-on-this-day that is not the datalist placeholder), sets monthly.
    - Otherwise sets weekly.
  - Localizes `Status` (Active/Inactive) and owner subsystem name.
  - Calls `LocalizeObjectProperties`.

### MapToInternalFtpServerModel(FtpServerModel serviceModel)
- Purpose: Map a WCF `FtpServerModel` into UI `FtpServerModel` (subset of fields).

### MapToInternalInboundTarget(FtsInboundTargetModel serviceModel)
- Purpose: Map inbound target metadata.

### MapToInternalSourceLocationModel(SourceLocationModel serviceModel)
- Purpose: Map source location into UI `SourceLocationModel`.
- Logic: maps `FtsSourceLocationId` to `SourceLocationId` string and localizes properties.

### GetModuleName(string moduleid)
- Purpose: Translate a module id into its module name by querying reference data.
- Logic:
  - Builds OData-like query string `Module?$filter=IsActive eq true&$orderby=ModuleName asc`.
  - Calls `IReferenceDataProvider.GetReferenceData(query)`.
  - Iterates results and when `ModuleID` matches, returns `ModuleName`.

### MapToServiceRequestor(RequestorModel internalRequestor)
- Purpose: Map internal requestor into the query service requestor type.

### MapToInternalDestinationModel(FtsDestinationLocationAdded item, int lineNumber)
- Purpose: Map one destination DTO into `DestinationConfigurationModel`.
- Logic:
  - Maps contract/destination type/protocol to list items.
  - Maps FTP server to a list item with `ID = FtpServerId.ToString()` and `Name = GetFTPServerName(...)`.
  - Maps rename pattern and other flags; sets `IsAddedUpdated = false`.
  - Sets `DestinationLineNumber = lineNumber` and `ImportJobName`.
  - Sets localized `Status` (Active/Inactive).

### BuildSourceLocationQueryParms(SearchCriteriaModel searchCriteria)
- Purpose: Map supported configuration search fields into service where parameters.
- Logic: supports `ConfigId`, `ConfigName`, `OwnerName`, `OwnerSubsystem`, `Protocol`, `IsActive`.

### BuildSourceLocationSortParms(List<SortCriteriaModel> sortCriteria)
- Purpose: Map supported sort fields for configuration search.
- Logic:
  - Supports `ConfigId`, `ConfigName`, `SourceAddress`, `SourceFileMask`, `OwnerName`, `OwnerSubsystem.Name` -> `ModuleName`, `Status`.
  - For unsupported fields, adds default `ConfigId` ascending.
  - If no sort criteria are produced, adds default `ConfigId` ascending.

### GetFTPServerName(string ftpServerId)
- Purpose: Resolve an FTP server id to its display name with a cached list.
- Logic:
  - If cache `ftpServers` is null, calls `GetFtpServers` and builds a list of `{ID, Name}` list items.
  - Returns the cached name for the matching id, else null.

### BuildResponseModel<List<FtpServerModel>>()
- Purpose: Create a standard OK response with an empty list, paging results initialized to 0, and an empty messages list.

### GetFTPServersResponse(RequestModel<FtpServerModel> request, SearchCriteriaModel searchCriteria, ResponseModel<List<FtpServerModel>> response)
- Purpose: Execute the WCF FTP server search and map results into the response.
- Logic:
  - Calls `RunService(BuildQuery(request, searchCriteria), serviceProxy)`.
  - When results are present:
    - Computes `removePassword = request.Data.RemovePassword ?? true`.
    - Calls `PopulateResponseResults(response, queryResults, removePassword)`.

### BuildQuery(RequestModel<FtpServerModel> request, SearchCriteriaModel searchCriteria)
- Purpose: Build a WCF FTP server query.
- Logic:
  - Sets a default `PageSize = 1` then calls `SetFTPServersPaging(searchCriteria, query)` to overwrite paging when criteria is provided.
  - Maps where fields, including:
    - `FtpServerId = request.Data.FtpServerId != null ? new Guid(request.Data.FtpServerId) : Guid.Empty`.
    - `SearchOptions = BuildSearchOptions(searchCriteria)`.

### BuildSearchOptions(SearchCriteriaModel searchCriteria)
- Purpose: Translate search-field match modes into service search mode options.
- Logic: for each search field, adds `SearchModeOption { FieldName, FilterType = GetSearchType(SearchModeType) }`.

### GetSearchType(SearchModeType searchMode)
- Purpose: Map UX search mode type to service search mode enum.
- Logic: Contains/EndsWith/StartsWith map directly; EqualTo is the default.

### SetFTPServersPaging(SearchCriteriaModel searchCriteria, FtpServerQuery query)
- Purpose: Apply sort + paging criteria to the FTP server query.
- Logic:
  - If `searchCriteria.SortCriteria.Count > 0`, sets `query.SortCriteria = SetSortCriteria(searchCriteria)`.
  - Ensures `PagingCriteria` is non-null, then sets `query.PageSize = RowsPerPage` and `query.CurrentPage = Page`.

### SetSortCriteria(SearchCriteriaModel searchCriteria)
- Purpose: Translate sort criteria into a single service sort criterion.
- Logic: uses the **last** element of `searchCriteria.SortCriteria`, and determines asc/desc by `SortDirection.ToString().Contains("Asc")`.

### PopulateResponseResults(ResponseModel<List<FtpServerModel>> response, FtpServerQuery queryResults, bool removePassord)
- Purpose: Set paging counts and map data list.
- Logic: sets `RowCount`, `TotalRowsAvailable`, and maps results via `MapResults`.

### MapResults(List<FtpServerModel> results, bool removePassord)
- Purpose: Map each service FTP server record to UI model.
- Logic: iterates results and calls `MapFTPServerModel(serversList, server, removePassword)`.

### MapFTPServerModel(List<FtpServerModel> serversList, FtpServerModel serverRecord, bool removePassword)
- Purpose: Map a service FTP server record and optionally remove its password.
- Logic:
  - AutoMapper maps all profile fields including `FtpPassword`.
  - Localizes object properties.
  - If `removePassword == true`, sets `model.FtpPassword = string.Empty`; otherwise preserves the mapped password.
  - Adds the mapped model to the destination list.

### HandleFaultException(FaultException<ServiceException> ex, ResultModel result)
- Purpose: Append service fault messages to the given `ResultModel` and mark it error.
- Logic:
  - Adds each string in `ex.Detail.ErrorMessages` as an error message.
  - Also adds `ex.Message` when present.

### HandleException(Exception ex)
- Purpose: Format an exception as a `ResultModel`.
- Logic: logs and returns `FormatErrorResponseResult(ex)`.

## IFileTransferQueryServiceDataProvider
Legacy interface implemented by `FileTransferQueryServiceDataProvider`.

- `SearchFileTransfer(request)`
- `GetFtpServers(request)`
- `GetMetadataIds(request, contractName)`
- `GetFileTransferConfigurationInformation(request)`
- `SearchFileTransferTracking(request)`
- `GetSourceLocations(request)`
- `GetFileTransferTrackingDetail(request)`
- `GetFTPServerCredentials(request, searchCriteria)`

## Rules implemented
| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-027 | Destination contact phone number has hyphens removed when present. | `FileTransferServiceDataProvider.AddFileTransferBaseConfiguration` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-028 | For `FileToEvent` destination type, overwrite address with `ExternalFTECommonFolder` setting. | `FileTransferServiceDataProvider.AddFileTransferBaseConfiguration` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-029 | Destination FTP server id empty string maps to `Guid.Empty`; otherwise parse GUID. | `FileTransferServiceDataProvider.AddFileTransferBaseConfiguration` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-030 | Empty run-on-this-day maps to byte `0`; otherwise convert to byte. | `FileTransferServiceDataProvider.AddFileTransferBaseConfiguration` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-031 | When suspended, set `IsSuspendedChangedDate = UtcNow`; else null. | `FileTransferServiceDataProvider.AddFileTransferBaseConfiguration` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-032 | Build `FtsSourceLocationAddCommand` and call service `FileTransferAdd`. | `FileTransferServiceDataProvider.AddFileTransferBaseConfiguration` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-033 | Map service faults/unexpected exceptions into an error response using standard handlers. | `FileTransferServiceDataProvider.AddFileTransferBaseConfiguration` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F02-051 | New FTP server command assigns `Guid.NewGuid()` and uses requestor id as operator id. | `FileTransferServiceDataProvider.BuildAddFtpServerCommand` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-052 | When add service returns non-null, response contains `FtpServerId`, preserves `Editor`, status `OK`. | `FileTransferServiceDataProvider.AddFtpServer(RequestModel, ResponseModel)` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-056 | Update FTP server reuses existing password when new password is not provided (`RemovePassword=false`). | `FileTransferServiceDataProvider.BuildUpdateFtpServerCommand` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-058 | When update service returns non-null, response contains `FtpServerId` and `IsValid`, preserves `Editor`, status `OK`. | `FileTransferServiceDataProvider.UpdateFtpServer(RequestModel, ResponseModel)` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-059 | When invalidate service returns non-null, response contains `FtpServerId` and `IsValid`, preserves `Editor`, status `OK`. | `FileTransferServiceDataProvider.InvalidateFtpServer(RequestModel, ResponseModel)` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-060 | Invalidate command parses `FtpServerId` as GUID and maps the requestor. | `FileTransferServiceDataProvider.BuildInvalidateFtpServerCommand` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-063 | If password lookup returns `OK` with an empty list, update fails with an unhandled exception. | `FileTransferServiceDataProvider.GetFTPServerPass` | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F05-035 | Tracking search maps only supported filters and applies date/size only when parsable. | `FileTransferQueryServiceDataProvider.BuildWhereQueryParms` | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |

## Target-state requirements

### T-008 Prohibit direct database access from the BFF; all persistence must occur via domain services.
- Purpose: Ensure this component’s target implementation performs all reads/writes via downstream domain APIs and not via direct database/ORM access.
- Data:
  - Reads/writes occur through `fxfer-domain-api` operations.
  - No direct DB connections, SQL, or ORM access is performed from the BFF.
- Errors: Any persistence failure is surfaced as downstream failures (and then normalized at the HTTP boundary by the BFF error middleware).
- Target-state section: `inputs/target-state.md` section `5.1 Purpose and Responsibilities`.
