---
type: Behavior Specification
title: 'ui-configure-source-locations-and-destinations-screens: Configure source locations, destinations, FTP credentials, and tracking screens'
description: 'What the ui-configure-source-locations-and-destinations-screens component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/ui-configure-source-locations-and-destinations-screens
tags:
- component
- provider-file-transfer-v2
- ui
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T00:05:24Z
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
  resource: /technical/components/ui-configure-source-locations-and-destinations-screens/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:ui-configure-source-locations-and-destinations-screens
  generation:
    status: done
    attempts: 2
  tier: ui
  classes: 3
  methods: 23
  owned_rules: 31
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - search-and-track-file-transfers
  depends_on: []
  deployable: fxfer-mfe
  folder: src/screens/configure-source-locations-and-destinations
  body_sha256: 12f5accfd04ad6e3d8d43191aa8ed3efbe28696a710bd6f6f0e94b5ebad201b4
---

# ui-configure-source-locations-and-destinations-screens: Configure source locations, destinations, FTP credentials, and tracking screens

This component covers the legacy UI/controller behaviors used to search, view, add, and edit file-transfer source-location configurations (and their destination rows), plus the related FTP server credential maintenance overlays and file-transfer tracking search/view overlays.

In the legacy system these behaviors are implemented as MVC actions returning Razor views/partials and thin HTTP API controllers returning `200 OK` wrappers around service calls. In the target state, the operator UI is a Next.js micro-frontend that calls the FXFER BFF only; MVC view rendering is replaced by React screen components and BFF calls while keeping the same observable behavior.

## FileTransferController

Legacy MVC controller that returns configuration/tracking views and partial overlays. It also calls the legacy `IFileTransferQueryServiceDataProvider` to load configuration and tracking details.

### SearchSourceLocations()
- Purpose: Render the search page for file transfer configurations (source locations).
- Inputs: None.
- Logic:
  - Creates `FileTransferConfigurationSearchCriteriaModel`.
  - Initializes the model editor using:
    - entity label `FileTransferConstants.Localization.Entities.FileTransfer`
    - `step = 1`, `stepCount = 1`, `editMode = Create`, `editDirection = Next`
    - cancel URL = `BuildMvcUrl(Home, MyHome)`
    - flags = `(isDirtyDataEnabled: false, isResetEnabled: true, isCancelEnabled: true, showSummaryError: true)`.
  - Sets `criteria.Editor.SaveActionText` to the localized label `Buttons.Search` using `UserProfile.LocalCode`.
  - Returns `View(criteria)`.
- Data: None.
- Errors: None.
- Rules: (none owned explicitly).

### AddFileBaseConfiguration(AddFileTransferConfigurationModel addFileTransferConfiguration)
- Purpose: Render the add configuration view for a file transfer source location.
- Inputs:
  - `addFileTransferConfiguration`: posted model (may or may not contain `FtsSourceLocationId`).
- Logic:
  - If static `configModel` is `null`, initializes it to `new AddFileTransferConfigurationModel()`.
  - Sets `configModel.OwnerSubsystemId = configModel.OwnerSubsystem.ID`.
  - Sets `ViewBag.FileTransferViewMode = "add"`.
  - If `addFileTransferConfiguration.FtsSourceLocationId == null`:
    - Sets `addFileTransferConfiguration.IsActive = true`.
    - Returns `View(addFileTransferConfiguration)`.
  - Else:
    - Returns `View(configModel)`.
- Data: None directly (render-only).
- Errors: None.
- Rules: F01-021, F01-022.

### FileTransferConfigurationView(string ftsSourceLocationId)
- Purpose: Render the configuration view overlay for a source location id.
- Inputs:
  - `ftsSourceLocationId`: source location identifier string (no validation in the controller).
- Logic:
  - Creates `FileTransferConfigurationModel` and assigns `FtsSourceLocationId = ftsSourceLocationId`.
  - Calls `GetFileTransferConfiguration(model)` to attempt to load details.
  - Returns view `~/Views/FileTransfer/FileTransferConfigurationView.cshtml` with the resulting model.
- Data: Reads configuration via `GetFileTransferConfiguration`.
- Errors: Provider/service errors do not block rendering; the view is returned with the model as-is.
- Rules: F01-001.

### DestinationConfigurationEditPanel(DestinationConfigurationModel addDestinationConfiguration)
- Purpose: Render destination configuration edit panel.
- Inputs: `addDestinationConfiguration`.
- Logic: Returns `View(addDestinationConfiguration)` without modification.
- Data: None.
- Errors: None.
- Rules: F01-003.

### FileTransferConfigurationEdit(FileTransferConfigurationModel fileTransferConfigModel)
- Purpose: Render configuration edit overlay (add/edit).
- Inputs: `fileTransferConfigModel`.
- Logic:
  - If `fileTransferConfigModel.FtsSourceLocationId == null`, sets `fileTransferConfigModel.IsActive = true`.
  - Else refreshes the model via `GetFileTransferConfiguration(fileTransferConfigModel)`.
  - Returns `View(fileTransferConfigModel)`.
- Data: Reads configuration via `GetFileTransferConfiguration`.
- Errors: Same as `GetFileTransferConfiguration`.
- Rules: F01-002.

### DestinationConfigurationEdit(DestinationConfigurationModel destinationConfiguration)
- Purpose: Render destination configuration edit view.
- Inputs: `destinationConfiguration`.
- Logic: Returns `View(destinationConfiguration)` without modification.
- Data: None.
- Errors: None.
- Rules: F01-004.

### GetFileTransferConfiguration(FileTransferConfigurationModel fileTransferConfigModel)
- Purpose: Load file transfer configuration details for a given `FtsSourceLocationId`.
- Inputs:
  - `fileTransferConfigModel.FtsSourceLocationId`: source location id string.
- Logic:
  - Builds `RequestModel<string>`:
    - `Requestor = ContextHelper.UserConfigurationManager.GenerateRequestor()`
    - `Data = fileTransferConfigModel.FtsSourceLocationId`
  - Resolves `IFileTransferQueryServiceDataProvider` from `TenantConfigurationManager` using component name `FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider`.
  - Calls `provider.GetFileTransferConfigurationInformation(request)`.
  - Replaces `fileTransferConfigModel` with `response.Data` only when `response != null`, `response.Result.StatusCode == OK`, and `response.Data != null`.
  - Returns the resulting `fileTransferConfigModel`.
- Data:
  - Reads configuration information via the query-service data provider. Observable behavior of that provider call (relied upon by this component):
    - Initializes `response.Result.StatusCode = NotFound` and sets it to `OK` only when the service returns at least one result row.
    - Calls query service `GetSourceLocations` with `Where.SourceLocationId` equal to the requested source location id string.
    - When the service returns multiple source-location results, maps only the first result row.
    - When destination addresses are present, appends each destination to `DestinationDetails` in service-provided order, assigning 1-based sequential `DestinationLineNumber` starting at 1.
    - Configuration status label is localized as “Active” when `IsActive == true`, otherwise localized as “Inactive”.
    - Each destination is marked `IsAddedUpdated = false` and destination status label is localized as “Active” when `Active == true`, otherwise “Inactive”.
    - Service-side validation relied upon:
      - Requires a non-empty `Where.SourceLocationId`; empty/missing id throws `FileTransferValidationException`.
      - Parses `Where.SourceLocationId` via `Guid.Parse(...)` when building `SoureceLocationGuid`.
- Errors:
  - Provider/service errors result in a non-`OK` status or null data; the controller method returns the in-flight model unchanged.
- Rules: F01-005, F01-006, F01-007, F01-008, F01-009, F01-010, F01-011, F01-012, F01-013, F01-018, F01-019.

### SearchFTPServerCredentials()
- Purpose: Render the FTP server credentials search view.
- Inputs: None.
- Logic: Returns `View(BuildFTPServerModel())`.
- Data: None directly.
- Errors: None.
- Rules: (none owned explicitly).

### AddFTPServerCredentials(FtpServerModel ftpServerModel)
- Purpose: Render the FTP server credentials view/edit partial overlay.
- Inputs: `ftpServerModel`.
- Logic: Returns `PartialView("~/Views/FileTransfer/ViewEditFTPServerCredentials.cshtml", ftpServerModel)`.
- Data: None.
- Errors: None.
- Rules: (none owned explicitly).

### BuildFTPServerModel()
- Purpose: Create the search criteria model for FTP server credential search.
- Inputs: None.
- Logic:
  - Creates `FtpServerSearchCriteriaModel`.
  - Initializes editor with:
    - entity label `FileTransferConstants.Localization.Entities.FileTransfer`
    - `step = 1`, `stepCount = 1`, `editMode = Create`, `editDirection = Next`
    - cancel URL = `BuildMvcUrl(Home, MyHome)`
    - flags = `(isDirtyDataEnabled: false, isResetEnabled: true, showSummaryError: false, isCancelEnabled: true)`.
  - Sets `Editor.SaveActionText` to localized `Buttons.Search` using `UserProfile.LocalCode`.
- Data: None.
- Errors: None.
- Rules: (none owned explicitly).

### SearchFileTransferTracking()
- Purpose: Render the file transfer tracking search page.
- Inputs: None.
- Logic:
  - Creates `TrackingSearchCriteriaModel`.
  - Initializes editor as single-step create flow:
    - `InitializeEditor(FileTransfer, 1, 1, Create, Next, BuildMvcUrl(Home, MyHome), false, true, true, true)`.
  - Uses `UserProfile.LocalCode` to set `criteria.Editor.SaveActionText` to localized `Buttons.Search`.
  - Returns `View(criteria)`.
- Data: None.
- Errors: None.
- Rules: F05-001, F05-002, F05-003.

### FileTransferTrackingView(string fileId)
- Purpose: Render tracking details overlay for a file id.
- Inputs: `fileId` string.
- Logic:
  - Creates `TrackingDetailModel trackingData`.
  - If `fileId` is not null/empty:
    - Resolves `IFileTransferQueryServiceDataProvider` by component name `FileTransferQueryServiceDataProvider`.
    - Builds `RequestModel<string>` with `Requestor` from user context and `Data = fileId`.
    - Calls `provider.GetFileTransferTrackingDetail(request)`.
    - Replaces `trackingData` only when result status is `OK` and data non-null.
  - Creates overlay editor parameters with:
    - a Cancel button labeled with localized `Buttons.Cancel` and postprocess `cancelFileTranferTrackingView`
    - title localized `Titles.FileTransferTracking`
    - close function `processFileTransfertrackingClose`
    - help icon shown, dirty-data enabled, summary errors shown
  - Assigns `trackingData.Editor = InitializeOverlayEditor(parameters)` and returns `View(trackingData)`.
- Data: Reads tracking detail via provider.
- Errors: Failures leave `trackingData` as empty model.
- Rules: (none owned explicitly).

## FileTransferSvcQueryServiceController

Legacy ASP.NET Core API controller. All actions are `[HttpPost]` and always respond `200 OK` containing the underlying query service return value.

### SearchSourceLocations(SourceLocationQuery query)
- Purpose: Search source locations.
- Inputs: `SourceLocationQuery`.
- Logic: `Ok(fileTransferSvcQueryService.SearchSourceLocations(query))`.
- Errors: No explicit validation or error mapping.
- Rules: F01-080.

### SearchFileTransferMovement(FileTransferTrackingQuery query)
- Purpose: Search file transfer movement.
- Inputs: `FileTransferTrackingQuery`.
- Logic: `Ok(fileTransferSvcQueryService.SearchFileTransferMovement(query))`.
- Rules: F05-020.

### GetFtpServers(FtpServerQuery query)
- Purpose: Get FTP servers.
- Inputs: `FtpServerQuery`.
- Logic: `Ok(fileTransferSvcQueryService.GetFtpServers(query))`.
- Rules: F02-004.

### GetSourceLocations(GetSourceLocationQuery query)
- Purpose: Get source location(s) for configuration details.
- Inputs: `GetSourceLocationQuery`.
- Logic: `Ok(fileTransferSvcQueryService.GetSourceLocations(query))`.
- Rules: F01-082.

### SearchInboundTarget(InboundTargetQuery query)
- Purpose: Search inbound targets.
- Inputs: `InboundTargetQuery`.
- Logic: `Ok(fileTransferSvcQueryService.SearchInboundTarget(query))`.
- Rules: F05-022.

## FileTransferSvcServiceController

Legacy ASP.NET Core API controller. All actions are `[HttpPost]` and always respond `200 OK` containing the underlying service return value.

### FileTransferAdd(FtsSourceLocationAddCommand command)
- Purpose: Add source location.
- Inputs: `FtsSourceLocationAddCommand`.
- Logic: `Ok(fileTransferSvcService.FileTransferAdd(command))`.
- Rules: F01-037.

### UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand command)
- Purpose: Update source location.
- Inputs: `FtsSourceLocationUpdateCommand`.
- Logic: `Ok(fileTransferSvcService.UpdateFileTransferSourceLocation(command))`.
- Rules: F01-056.

### InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand command)
- Purpose: Invalidate source location.
- Inputs: `FtsSourceLocationInvalidateCommand`.
- Logic: `Ok(fileTransferSvcService.InvalidateFileTransferSourceLocation(command))`.
- Rules: F01-058.

### AddFtpServer(AddFtpServerCommand command)
- Purpose: Add FTP server.
- Inputs: `AddFtpServerCommand`.
- Logic: `Ok(fileTransferSvcService.AddFtpServer(command))`.
- Rules: F02-018.

### UpdateFtpServer(UpdateFtpServerCommand command)
- Purpose: Update FTP server.
- Inputs: `UpdateFtpServerCommand`.
- Logic: `Ok(fileTransferSvcService.UpdateFtpServer(command))`.
- Rules: F02-020.

### InvalidateFtpServer(InvalidateFtpServerCommand command)
- Purpose: Invalidate FTP server.
- Inputs: `InvalidateFtpServerCommand`.
- Logic: `Ok(fileTransferSvcService.InvalidateFtpServer(command))`.
- Rules: F02-022.

## Rules implemented

| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-001 | Always calls `GetFileTransferConfiguration(...)` and renders `~/Views/FileTransfer/FileTransferConfigurationView.cshtml` with the resulting model. | FileTransferController.FileTransferConfigurationView | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-002 | If `FtsSourceLocationId` is missing, defaults `IsActive = true`; else reloads configuration by id and returns the default view. | FileTransferController.FileTransferConfigurationEdit | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-003 | Renders the default view using posted `DestinationConfigurationModel` without modifying it. | FileTransferController.DestinationConfigurationEditPanel | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-004 | Renders the default view using posted `DestinationConfigurationModel` without modifying it. | FileTransferController.DestinationConfigurationEdit | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-005 | Sends a query-provider request with `Requestor` from user context and `Data = FtsSourceLocationId`. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-006 | Resolves `IFileTransferQueryServiceDataProvider` by component name and calls `GetFileTransferConfigurationInformation(request)`. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-007 | Replaces the in-flight model only when response is non-null, status `OK`, and data non-null. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-008 | Provider starts as `NotFound` and changes to `OK` only when service returns at least one row. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-009 | Provider calls `GetSourceLocations` with `Where.SourceLocationId = requested id string`. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-010 | Provider maps only the first result row when multiple are returned. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-011 | Provider appends destinations in order and assigns 1-based `DestinationLineNumber` starting at 1. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-012 | Empty/missing `Where.SourceLocationId` is rejected by throwing `FileTransferValidationException`. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-013 | `Where.SourceLocationId` is parsed via `Guid.Parse(...)` when building `SoureceLocationGuid`. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-018 | Configuration status label is localized “Active” when active, otherwise “Inactive”. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-019 | Destinations set `IsAddedUpdated = false` and localize status “Active”/“Inactive” by `Active`. | FileTransferController.GetFileTransferConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-021 | When posted without source location id, marks configuration active and renders the add view. | FileTransferController.AddFileBaseConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-022 | When posted with a source location id, renders using a static `configModel` instance. | FileTransferController.AddFileBaseConfiguration | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-037 | API controller action returns HTTP 200 with result of `FileTransferAdd`. | FileTransferSvcServiceController.FileTransferAdd | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-056 | API controller action returns HTTP 200 with result of `UpdateFileTransferSourceLocation(command)`. | FileTransferSvcServiceController.UpdateFileTransferSourceLocation | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-058 | API controller action returns HTTP 200 with result of `InvalidateFileTransferSourceLocation(command)`. | FileTransferSvcServiceController.InvalidateFileTransferSourceLocation | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-080 | API `SearchSourceLocations` door returns HTTP 200 with result of `IFileTransferSvcQueryService.SearchSourceLocations`. | FileTransferSvcQueryServiceController.SearchSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-082 | API `GetSourceLocations` door returns HTTP 200 with result of `IFileTransferSvcQueryService.GetSourceLocations`. | FileTransferSvcQueryServiceController.GetSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F02-004 | API `GetFtpServers` always replies 200 OK with `IFileTransferSvcQueryService.GetFtpServers`. | FileTransferSvcQueryServiceController.GetFtpServers | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-018 | API AddFtpServer responds 200 OK with result of calling `AddFtpServer(command)`. | FileTransferSvcServiceController.AddFtpServer | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-020 | API UpdateFtpServer responds 200 OK with result of calling `UpdateFtpServer(command)`. | FileTransferSvcServiceController.UpdateFtpServer | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-022 | API InvalidateFtpServer responds 200 OK with result of calling `InvalidateFtpServer(command)`. | FileTransferSvcServiceController.InvalidateFtpServer | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F05-001 | Tracking search initializes editor as single-step create; cancel navigates to `Home/MyHome`. | FileTransferController.SearchFileTransferTracking | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-002 | Tracking search uses user profile locale code to localize Search action label. | FileTransferController.SearchFileTransferTracking | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-003 | Tracking search returns MVC view with an empty criteria model. | FileTransferController.SearchFileTransferTracking | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-020 | API `SearchFileTransferMovement` door returns HTTP 200 with result of `IFileTransferSvcQueryService.SearchFileTransferMovement`. | FileTransferSvcQueryServiceController.SearchFileTransferMovement | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-022 | API `SearchInboundTarget` door returns HTTP 200 with result of `IFileTransferSvcQueryService.SearchInboundTarget`. | FileTransferSvcQueryServiceController.SearchInboundTarget | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |

## Not carried over
- FileTransferController (MVC view rendering): dropped in favor of Next.js screens; UI behavior is carried over, but server-side Razor rendering and MVC action wiring are replaced by React components and BFF calls.
