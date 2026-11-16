---
type: Business Rules
title: Manage FTP server credentials - rules
description: 24 business rules (P3-NNN) of the flow, each with its legacy source lines.
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
  body_sha256: 8d673b276c01b61534c960fe0b53c57289ba7e41ddef95d43138466431125916
---

# Manage FTP server credentials - rules

## Add / route

**P3-001** When `FtpServerId` is empty on an Add request, the request is treated as a create; otherwise it is treated as an update.
- Doors: file-transfer-controller-add-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:47-49
- Legacy: `AddFTPServerCredentials` calls `CreateFTPServerCredentials` when `ftpServerModel.FtpServerId` is null/empty, else calls `UpdateFTPServerCredentials`.
- Decision: keep

## Get FTP server credentials

**P3-002** A Get request builds a `SearchCriteriaModel` from the Kendo `DataSourceRequest`, builds a `RequestModel<FtpServerModel>` from that criteria, and queries for FTP server credentials.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:30-36
- Legacy: The controller resolves `IFileTransferQueryServiceDataProvider`, calls `ContextHelper.InitializeSearchRequest(request)`, then calls `GetFTPServerCredentials(requestModel, searchCriteria)`.
- Decision: keep

**P3-003** When the query response is missing (null) or has a null `Data` list, the Get request returns HTTP 404 with an empty list.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:38-40
- Legacy: The controller returns `NotFound` with `new List<FtpServerModel>()` unless both `ftpServerResponse` and `ftpServerResponse.Data` are non-null.
- Decision: keep

**P3-004** When the query response result status is `Error`, the Get request returns HTTP 400 with the `ResultModel`; otherwise it returns HTTP 200 with a Kendo `DataSourceResult` containing `Data` and `Total`.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:118-120, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:140-146
- Legacy: `BuildResponse` maps `StatusCodeType.Error` to `BadRequest` and otherwise returns `OK` with `BuildDataSourceResult`, which sets `Data` and `Total`.
- Decision: keep

**P3-005** When building the FTP server search request, missing search fields (or missing matching field names) are treated as empty strings for `FtpServerName`, `FtpDescription`, and `FtpUser`.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:96-102, ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs:33-36
- Legacy: `BuildRequest` calls `SearchHelper.GetFieldValue(...)`; `GetFieldValue` returns `string.Empty` when there is no criteria or the field is not found.
- Decision: keep

**P3-006** When the search request contains a non-empty `IsValid` field value, the server filters to `IsValid = true`; otherwise it does not set an `IsValid` filter.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:101-102
- Legacy: `BuildRequest` sets `IsValid` to `true` when the `IsValid` search field value is not empty, else sets it to null.
- Decision: keep
- âš‘ Flag: the incoming `IsValid` search field value is not parsed; any non-empty value forces `IsValid=true` and there is no way to request `IsValid=false` via this code path.

**P3-007** A search field lookup returns the first matching field by name and converts its value to a string; if no matching field exists, it returns an empty string.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs:33-36
- Legacy: `GetFieldValue` checks `SearchFields` for emptiness, then `FirstOrDefault(filter => filter.FieldName == fieldName)` and returns `FieldValue.ToString()`.
- Decision: keep

**P3-008** When querying FTP server credentials, passwords are removed from results by default.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:858-860, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:995
- Legacy: If `request.Data.RemovePassword` is null, the provider treats it as `true` and maps each result with `FtpPassword = string.Empty`.
- Decision: keep

**P3-009** When paging and sorting criteria are present, the query uses the provided page size and page number, and (when multiple sorts are provided) only the last sort criterion is used.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:924-938, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:941-951
- Legacy: `SetFTPServersPaging` sets `PageSize` and `CurrentPage` from `PagingCriteria`; `SetSortCriteria` uses `SortCriteria[Count - 1]` and maps direction to Asc/Desc.
- Decision: keep

**P3-010** Each search field's `SearchMode` is translated into a service-side filter type (Contains, StartsWith, EndsWith, or EqualTo), defaulting to EqualTo.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:889-905, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:908-921
- Legacy: `BuildSearchOptions` creates one `SearchModeOption` per `SearchFieldModel`, and `GetSearchType` maps modes with a default of `EqualTo`.
- Decision: keep

**P3-011** The Get credentials response reports `TotalRowsAvailable` based on the service-reported row count, not just the count of returned results.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:955-960
- Legacy: `PopulateResponseResults` sets `PagingResults.RowCount = Results.Count` and `PagingResults.TotalRowsAvailable = queryResults.RowCount`.
- Decision: keep

## Create FTP server credentials

**P3-012** When the create request model state is invalid, the create request returns HTTP 400 with the formatted model state errors.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:58-65
- Legacy: `CreateFTPServerCredentials` checks `ModelState.IsValid` and otherwise returns `BadRequest` with `FormatResultModel(ModelState)`.
- Decision: keep

**P3-013** When an add succeeds (service result status `OK`), the create request returns HTTP 200 with a localized success message stored in `Editor.SavedMessage` and `Editor.SaveActionText` set to `Success`.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:130-137, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:149-168
- Legacy: `SetResponse` routes `StatusCodeType.OK` to `FtpServerCredentialsSave`, which sets `Editor.SavedMessage` and `Editor.SaveActionText` and returns `OK`.
- Decision: keep

**P3-014** When adding a new FTP server, the service command assigns a new server id (`Guid.NewGuid()`) and uses the requestor identifier as the operator id.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:724-738
- Legacy: `BuildAddFtpServerCommand` sets `FtpServerId = Guid.NewGuid()` and `OperatorId = request.Requestor.IdentifierId`.
- Decision: keep

**P3-015** When the add service returns a non-null response, the server responds with a model containing the new `FtpServerId`, preserves the request's `Editor` object, and marks the result status as `OK`.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:650-661
- Legacy: `AddFtpServer` maps `serviceResponse.FtpServerId` into a new `FtpServerModel`, copies `request.Data.Editor`, and sets `response.Result.StatusCode = OK`.
- Decision: keep

## Update / invalidate FTP server credentials

**P3-016** When the update request model state is invalid, the update request returns HTTP 400 with the formatted model state errors.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:76-88
- Legacy: `UpdateFTPServerCredentials` checks `ModelState.IsValid` and otherwise returns `BadRequest` with `FormatResultModel(ModelState)`.
- Decision: keep

**P3-017** Update requests with `IsValid=true` call the Update operation; update requests with `IsValid=false` call the Invalidate operation.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:175-177
- Legacy: `UpdateFtpServer` calls `DataProvider.UpdateFtpServer` when `IsValid.Value` is true, else calls `DataProvider.InvalidateFtpServer`.
- Decision: keep

**P3-018** When an update/invalidate service response has no `Data`, the update request returns HTTP 400 with a single error message taken from the first service message.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:80-86, ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:187-198
- Legacy: When `processorResult.Data` is null, the controller returns `BadRequest` with `FormatInvalidResponse(processorResult.Result.Messages[0].Message)`.
- Decision: keep

**P3-019** When updating an FTP server and no new password is provided, the server reuses the existing password fetched from the credentials query with `RemovePassword=false`.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:743-759, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:770-777, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:788-795
- Legacy: `BuildUpdateFtpServerCommand` uses `request.Data.FtpPassword` when non-empty, else calls `GetFTPServerPass`, which queries for the server with `RemovePassword=false` and uses the first returned record's `FtpPassword`.
- Decision: keep

**P3-020** Update/invalidate success messages differ: a valid update uses the Edit FTP server credentials success message, and an invalidation uses the Invalidate FTP server credentials success message.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:158-162
- Legacy: `FtpServerCredentialsSave` chooses the content id based on `ftpServerModel.IsValid.Value` for the edit action.
- Decision: keep

**P3-021** When the update service returns a non-null response, the server responds with a model containing `FtpServerId` and `IsValid`, preserves the request's `Editor` object, and marks the result status as `OK`.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:672-684
- Legacy: `UpdateFtpServer` maps `serviceResponse.FtpServerId` and `serviceResponse.IsValid`, copies `request.Data.Editor`, and sets `response.Result.StatusCode = OK`.
- Decision: keep

**P3-022** When the invalidate service returns a non-null response, the server responds with a model containing `FtpServerId` and `IsValid`, preserves the request's `Editor` object, and marks the result status as `OK`.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:695-707
- Legacy: `InvalidateFtpServer` maps `serviceResponse.FtpServerId` and `serviceResponse.IsValid`, copies `request.Data.Editor`, and sets `response.Result.StatusCode = OK`.
- Decision: keep

**P3-023** Invalidate requests build the invalidate command by parsing `FtpServerId` as a GUID and mapping the requestor.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:814-820
- Legacy: `BuildInvalidateFtpServerCommand` sets `FtpServerId = Guid.Parse(request.Data.FtpServerId)` and maps `Requestor`.
- Decision: keep

## Success message localization

**P3-024** Success messages are retrieved from the localization provider using the current user's locale code and the selected content id, and the returned message `Text` is used.
- Doors: file-transfer-controller-create-ftpserver-credentials, file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:180-185
- Legacy: `GetSuccessMessage` calls `LocalizationProvider.GetMessage(userLocaleCode, contentId).Text`.
- Decision: keep
