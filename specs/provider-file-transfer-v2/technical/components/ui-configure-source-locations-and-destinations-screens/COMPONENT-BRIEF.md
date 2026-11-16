---
type: Generation Brief
title: 'Brief: component ui-configure-source-locations-and-destinations-screens'
description: Instructions the tool gave the writer session for the ui-configure-source-locations-and-destinations-screens component.
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
  body_sha256: c521790f0b6c3237c54fe65c13af0a1f788d792f6dc2bf11da09060d641a5e18
---

# Brief: component `ui-configure-source-locations-and-destinations-screens`

- Screens of the flow configure-source-locations-and-destinations (ui tier, 3 classes, 23 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/ui-configure-source-locations-and-destinations-screens/INVENTORY.yaml`
- Write exactly these files in `technical/components/ui-configure-source-locations-and-destinations-screens/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-mfe` (`apps/fxfer-mfe`), folder `src/screens/configure-source-locations-and-destinations`: write `apps/fxfer-mfe/src/screens/configure-source-locations-and-destinations` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-mfe/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

- `configure-source-locations-and-destinations`: `technical/flows/configure-source-locations-and-destinations/` (FLOW.md, RULES.md) and its scenarios `business/configure-source-locations-and-destinations.feature`
- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`
- `search-and-track-file-transfers`: `technical/flows/search-and-track-file-transfers/` (FLOW.md, RULES.md) and its scenarios `business/search-and-track-file-transfers.feature`

## Legacy endpoints it serves (list each in openapi.yaml's x-legacy-endpoints)

- `GET /FileTransfer`
- `POST /FileTransfer`
- `POST /FileTransferSvcQueryService`
- `POST /FileTransferSvcService`

## Packages it calls (contracts to keep)

- `hp.hsp.ua3.core.ux.common`
- `hp.hsp.ua3.core.ux.interfaces`
- `hp.hsp.ua3.core.ux.presentation`
- `microsoft.aspnet.mvc`

## Business rules it owns (31)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-001** When the configuration view is requested, the flow builds a `FileTransferConfigurationModel` with the provided `ftsSourceLocationId`, attempts to load the configuration, and renders `~/Views/FileTransfer/FileTransferConfigurationView.cshtml` with the resulting model.
- Doors: file-transfer-controller-file-transfer-configuration-view
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:88-100
- Legacy: The controller always calls `GetFileTransferConfiguration(...)` and always returns the named view with the model (loaded or not).
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-002** When editing a configuration, if `FtsSourceLocationId` is missing the flow treats the configuration as active by default (`IsActive = true`); otherwise it reloads the configuration by source location id and renders the default MVC view with the resulting model.
- Doors: file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:113-129
- Legacy: The controller sets `IsActive` only when the id is null; it does not validate the id format.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-003** When the destination configuration edit panel is requested, the flow renders the default MVC view using the posted `DestinationConfigurationModel` without modifying it.
- Doors: file-transfer-controller-destination-configuration-edit-panel
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:103-110
- Legacy: The controller just returns `View(addDestinationConfiguration)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-004** When destination configuration edit is requested, the flow renders the default MVC view using the posted `DestinationConfigurationModel` without modifying it.
- Doors: file-transfer-controller-destination-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:132-139
- Legacy: The controller just returns `View(destinationConfiguration)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-005** When loading configuration details by source location id, the flow sends a request to the query-service data provider with `Requestor` generated from the current user context and `Data` set to the `FtsSourceLocationId` value.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:142-155
- Legacy: The controller always calls `GenerateRequestor()` and passes the source location id string as the request data.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-006** The flow resolves `IFileTransferQueryServiceDataProvider` from the tenant configuration container using the component name `FileTransferQueryServiceDataProvider` and calls `GetFileTransferConfigurationInformation(request)`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:150-155
- Legacy: The controller does not fall back to other providers; resolution and call happen unconditionally.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-007** The flow replaces the in-flight configuration model with the provider’s response only when the response is non-null, has status `OK`, and contains non-null data.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:156-162
- Legacy: Any non-`OK` status (including `NotFound` or error results) leaves the model unchanged.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-008** When querying configuration information, the provider initializes the response status as `NotFound` and only changes it to `OK` when the service returns at least one result row.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:141-149, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:175-191
- Legacy: A “no rows” service response is treated as `NotFound` (not an error).
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-009** The provider queries the file transfer query service by calling `GetSourceLocations` with a `Where.SourceLocationId` equal to the requested source location id string.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:151-158, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:171-174
- Legacy: The provider forwards the id as-is; it does not pre-parse or pre-validate it.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-010** When the service returns multiple source-location results, the provider maps only the first result row into the configuration model.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:175-179
- Legacy: Only `Results[0]` is used for configuration mapping.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-011** When destination addresses are present, the provider appends each destination to `DestinationDetails` in the service-provided order and assigns a 1-based sequential `DestinationLineNumber` starting at 1.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:180-187, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:638-640
- Legacy: Destination line numbers are regenerated on every load and are not persisted.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-012** The source-location lookup requires a non-empty `Where.SourceLocationId`; an empty or missing id is rejected by throwing `FileTransferValidationException`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\GetSourceLocationQuery.cs:28-34
- Legacy: The service validates only “present vs empty”, not GUID format.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-013** The service parses the `Where.SourceLocationId` value as a GUID using `Guid.Parse(...)` when building `SoureceLocationGuid`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\GetSourceLocationQueryParms.cs:30-35
- Legacy: Invalid GUID text triggers a `Guid.Parse` failure when `SoureceLocationGuid` is evaluated.
- Decision: keep
- ⚑ Flag: `Validate()` checks only for empty strings; a non-empty but non-GUID `SourceLocationId` can still fail later during GUID parsing.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-018** The configuration status label is set to the localized “Active” label when `IsActive` is true, otherwise it is set to the localized “Inactive” label.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:506-515
- Legacy: The label text is resolved through the localization provider using the current user locale.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-019** Each destination configuration is marked as not newly added/updated (`IsAddedUpdated = false`) and its status label is localized as “Active” when `Active` is true, otherwise localized as “Inactive”.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:638-651
- Legacy: Destination status is driven by the destination `IsValid` flag (mapped into `Active`).
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-021** When `AddFileBaseConfiguration` is posted without a source location id, the UI marks the configuration as active and renders the add view.
- Doors: file-transfer-controller-add-file-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:62-85
- Legacy: Sets `ViewBag.FileTransferViewMode = "add"`; when `addFileTransferConfiguration.FtsSourceLocationId == null`, sets `addFileTransferConfiguration.IsActive = true` and returns `View(addFileTransferConfiguration)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-022** When `AddFileBaseConfiguration` is posted with a source location id, the UI renders the view using a static `configModel` instance.
- Doors: file-transfer-controller-add-file-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:22-23, ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:66-83
- Legacy: Creates `configModel` once (static) and sets `configModel.OwnerSubsystemId = configModel.OwnerSubsystem.ID`; when `addFileTransferConfiguration.FtsSourceLocationId != null`, returns `View(configModel)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-037** The BAS API controller `FileTransferAdd` action returns HTTP 200 with the result of `IFileTransferSvcService.FileTransferAdd`.
- Doors: file-transfer-svc-service-controller-file-transfer-add
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:22-26
- Legacy: Returns `Ok(fileTransferSvcService.FileTransferAdd(command))`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-056** When `UpdateFileTransferSourceLocation` is called via the HTTP API controller, the controller returns HTTP 200 with the result of `IFileTransferSvcService.UpdateFileTransferSourceLocation(command)` as the response body.
- Doors: file-transfer-svc-service-controller-update-file-transfer-source-location
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:29-32
- Legacy: The controller always responds with `Ok(...)` and delegates all work to the injected service.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-058** When `InvalidateFileTransferSourceLocation` is called via the HTTP API controller, the controller returns HTTP 200 with the result of `IFileTransferSvcService.InvalidateFileTransferSourceLocation(command)` as the response body.
- Doors: file-transfer-svc-service-controller-invalidate-file-transfer-source-location
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:35-37
- Legacy: The controller always responds with `Ok(...)` and delegates all work to the injected service.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-080** The API `SearchSourceLocations` door returns HTTP 200 with the result of `IFileTransferSvcQueryService.SearchSourceLocations`.
- Doors: file-transfer-svc-query-service-controller-search-source-locations
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:21-25
- Legacy: The action returns `Ok(fileTransferSvcQueryService.SearchSourceLocations(query))`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-082** The API `GetSourceLocations` door returns HTTP 200 with the result of `IFileTransferSvcQueryService.GetSourceLocations`.
- Doors: file-transfer-svc-query-service-controller-get-source-locations
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:39-43
- Legacy: The action returns `Ok(fileTransferSvcQueryService.GetSourceLocations(query))`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-004** The HTTP API door for `GetFtpServers` always replies `200 OK` with the result of `IFileTransferSvcQueryService.GetFtpServers`.
- Doors: file-transfer-svc-query-service-controller-get-ftp-servers
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:33-37
- Legacy: Calls `Ok(this.fileTransferSvcQueryService.GetFtpServers(query))` without additional validation or error mapping.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-018** The HTTP controller endpoint for AddFtpServer responds `200 OK` containing the result of calling the service’s `AddFtpServer(command)`.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:40-44
- Legacy: The controller wraps `fileTransferSvcService.AddFtpServer(command)` in `Ok(...)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-020** The HTTP controller endpoint for UpdateFtpServer responds `200 OK` containing the result of calling the service’s `UpdateFtpServer(command)`.
- Doors: file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:46-50
- Legacy: The controller wraps `fileTransferSvcService.UpdateFtpServer(command)` in `Ok(...)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-022** The HTTP controller endpoint for InvalidateFtpServer responds `200 OK` containing the result of calling the service’s `InvalidateFtpServer(command)`.
- Doors: file-transfer-svc-service-controller-invalidate-ftp-server
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:52-56
- Legacy: The controller wraps `fileTransferSvcService.InvalidateFtpServer(command)` in `Ok(...)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-001** When the File Transfer tracking search page is requested, the editor is initialized as a single-step create flow for the FileTransfer entity and the cancel action navigates to `Home/MyHome`.
- Doors: file-transfer-controller-search-file-transfer-tracking
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:30-40
- Legacy: Calls `InitializeEditor(FileTransfer, 1, 1, Create, Next, BuildMvcUrl(Home, MyHome), false, true, true, true)` (dirty-data disabled; reset enabled; cancel enabled; summary errors shown).
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-002** The tracking search page uses the user profile locale code to localize the editor Search action label.
- Doors: file-transfer-controller-search-file-transfer-tracking
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:42-43
- Legacy: Sets `criteria.Editor.SaveActionText` from `LocalizationProvider.GetLabel(UserProfile.LocalCode, CoreConstants.Localization.Labels.Buttons.Search).Text`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-003** The tracking search page returns an MVC view with an empty tracking search criteria model.
- Doors: file-transfer-controller-search-file-transfer-tracking
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:28, ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:45-47
- Legacy: Creates `new TrackingSearchCriteriaModel()` and returns `View(criteria)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-020** The API `SearchFileTransferMovement` door returns HTTP 200 with the result of `IFileTransferSvcQueryService.SearchFileTransferMovement`.
- Doors: file-transfer-svc-query-service-controller-search-file-transfer-movement
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:27-31
- Legacy: The controller calls the query service and wraps the returned query object in `Ok(...)`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-022** The API `SearchInboundTarget` door returns HTTP 200 with the result of `IFileTransferSvcQueryService.SearchInboundTarget`.
- Doors: file-transfer-svc-query-service-controller-search-inbound-target
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:45-49
- Legacy: The controller calls the query service and wraps the returned query object in `Ok(...)`.
- Decision: keep

