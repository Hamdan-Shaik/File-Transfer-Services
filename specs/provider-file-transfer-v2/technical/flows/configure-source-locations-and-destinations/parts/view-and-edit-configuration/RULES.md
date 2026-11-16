---
type: Business Rules
title: 'Rules: View and edit file transfer configuration'
description: 20 business rules (P1-NNN) of the flow, each with its legacy source lines.
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController
tags:
- api
- flow
- provider-file-transfer-v2
- wcf
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T08:09:56Z
sources:
- id: door-file-transfer-controller-file-transfer-configuration-view
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L88
  title: FileTransferConfigurationView (api door)
- id: door-file-transfer-controller-file-transfer-configuration-edit
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L113
  title: FileTransferConfigurationEdit (api door)
- id: door-file-transfer-controller-add-file-base-configuration
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L62
  title: AddFileBaseConfiguration (api door)
- id: door-file-transfer-controller-add-file-transfer-base-configuration
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.AddFileTransferBaseConfiguration.cs#L21
  title: AddFileTransferBaseConfiguration (api door)
- id: door-file-transfer-controller-destination-configuration-edit
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L132
  title: DestinationConfigurationEdit (api door)
- id: door-file-transfer-controller-destination-configuration-edit-panel
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L103
  title: DestinationConfigurationEditPanel (api door)
- id: door-file-transfer-svc-service-file-transfer-add
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L25
  title: FileTransferAdd (wcf door)
- id: door-file-transfer-svc-service-file-transfer-add-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L26
  title: FileTransferAdd (wcf door)
- id: door-file-transfer-svc-service-file-transfer-add-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L25
  title: FileTransferAdd (wcf door)
- id: door-file-transfer-svc-service-controller-file-transfer-add
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L22
  title: FileTransferAdd (api door)
- id: door-file-transfer-svc-service-update-file-transfer-source-location
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L31
  title: UpdateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-update-file-transfer-source-location-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L32
  title: UpdateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-update-file-transfer-source-location-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L31
  title: UpdateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-controller-update-file-transfer-source-location
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L28
  title: UpdateFileTransferSourceLocation (api door)
- id: door-file-transfer-svc-service-invalidate-file-transfer-source-location
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L37
  title: InvalidateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-invalidate-file-transfer-source-location-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L38
  title: InvalidateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-invalidate-file-transfer-source-location-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L37
  title: InvalidateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-controller-invalidate-file-transfer-source-location
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L34
  title: InvalidateFileTransferSourceLocation (api door)
- id: door-file-transfer-svc-query-service-get-source-locations
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L40
  title: GetSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-get-source-locations-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L31
  title: GetSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-get-source-locations-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L31
  title: GetSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-controller-get-source-locations
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L39
  title: GetSourceLocations (api door)
- id: door-file-transfer-controller-get-source-locations
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SourceLocation.cs#L24
  title: GetSourceLocations (api door)
- id: door-file-transfer-svc-query-service-search-source-locations
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L25
  title: SearchSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-search-source-locations-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L16
  title: SearchSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-search-source-locations-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L16
  title: SearchSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-controller-search-source-locations
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L21
  title: SearchSourceLocations (api door)
- id: footprint
  resource: /technical/flows/configure-source-locations-and-destinations/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:configure-source-locations-and-destinations
  generation:
    status: done
    attempts: 1
  rule_prefix: F01
  rules: 108
  doors:
  - file-transfer-controller-file-transfer-configuration-view
  - file-transfer-controller-file-transfer-configuration-edit
  - file-transfer-controller-add-file-base-configuration
  - file-transfer-controller-add-file-transfer-base-configuration
  - file-transfer-controller-destination-configuration-edit
  - file-transfer-controller-destination-configuration-edit-panel
  - file-transfer-svc-service-file-transfer-add
  - file-transfer-svc-service-file-transfer-add-2
  - file-transfer-svc-service-file-transfer-add-3
  - file-transfer-svc-service-controller-file-transfer-add
  - file-transfer-svc-service-update-file-transfer-source-location
  - file-transfer-svc-service-update-file-transfer-source-location-2
  - file-transfer-svc-service-update-file-transfer-source-location-3
  - file-transfer-svc-service-controller-update-file-transfer-source-location
  - file-transfer-svc-service-invalidate-file-transfer-source-location
  - file-transfer-svc-service-invalidate-file-transfer-source-location-2
  - file-transfer-svc-service-invalidate-file-transfer-source-location-3
  - file-transfer-svc-service-controller-invalidate-file-transfer-source-location
  - file-transfer-svc-query-service-get-source-locations
  - file-transfer-svc-query-service-get-source-locations-2
  - file-transfer-svc-query-service-get-source-locations-3
  - file-transfer-svc-query-service-controller-get-source-locations
  - file-transfer-controller-get-source-locations
  - file-transfer-svc-query-service-search-source-locations
  - file-transfer-svc-query-service-search-source-locations-2
  - file-transfer-svc-query-service-search-source-locations-3
  - file-transfer-svc-query-service-controller-search-source-locations
  tables: 3
  procedures: 0
  components:
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  - backend-configure-source-locations-and-destinations-api
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - shared-contracts-commands
  - ui-configure-source-locations-and-destinations-screens
  - ui-manage-ftp-servers-and-credentials-screens
  body_sha256: fd1ab3db2b1b3cc249b80027026ff976c9742cb28d0d6e66466ee41f006c401e
---

# Rules: View and edit file transfer configuration

## View configuration

**P1-001** When the configuration view is requested, the flow builds a `FileTransferConfigurationModel` with the provided `ftsSourceLocationId`, attempts to load the configuration, and renders `~/Views/FileTransfer/FileTransferConfigurationView.cshtml` with the resulting model.
- Doors: file-transfer-controller-file-transfer-configuration-view
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:88-100
- Legacy: The controller always calls `GetFileTransferConfiguration(...)` and always returns the named view with the model (loaded or not).
- Decision: keep

## Edit configuration

**P1-002** When editing a configuration, if `FtsSourceLocationId` is missing the flow treats the configuration as active by default (`IsActive = true`); otherwise it reloads the configuration by source location id and renders the default MVC view with the resulting model.
- Doors: file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:113-129
- Legacy: The controller sets `IsActive` only when the id is null; it does not validate the id format.
- Decision: keep

## Destination edit screens

**P1-003** When the destination configuration edit panel is requested, the flow renders the default MVC view using the posted `DestinationConfigurationModel` without modifying it.
- Doors: file-transfer-controller-destination-configuration-edit-panel
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:103-110
- Legacy: The controller just returns `View(addDestinationConfiguration)`.
- Decision: keep

**P1-004** When destination configuration edit is requested, the flow renders the default MVC view using the posted `DestinationConfigurationModel` without modifying it.
- Doors: file-transfer-controller-destination-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:132-139
- Legacy: The controller just returns `View(destinationConfiguration)`.
- Decision: keep

## Load configuration details

**P1-005** When loading configuration details by source location id, the flow sends a request to the query-service data provider with `Requestor` generated from the current user context and `Data` set to the `FtsSourceLocationId` value.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:142-155
- Legacy: The controller always calls `GenerateRequestor()` and passes the source location id string as the request data.
- Decision: keep

**P1-006** The flow resolves `IFileTransferQueryServiceDataProvider` from the tenant configuration container using the component name `FileTransferQueryServiceDataProvider` and calls `GetFileTransferConfigurationInformation(request)`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:150-155
- Legacy: The controller does not fall back to other providers; resolution and call happen unconditionally.
- Decision: keep

**P1-007** The flow replaces the in-flight configuration model with the provider’s response only when the response is non-null, has status `OK`, and contains non-null data.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:156-162
- Legacy: Any non-`OK` status (including `NotFound` or error results) leaves the model unchanged.
- Decision: keep

## Provider query and mapping

**P1-008** When querying configuration information, the provider initializes the response status as `NotFound` and only changes it to `OK` when the service returns at least one result row.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:141-149, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:175-191
- Legacy: A “no rows” service response is treated as `NotFound` (not an error).
- Decision: keep

**P1-009** The provider queries the file transfer query service by calling `GetSourceLocations` with a `Where.SourceLocationId` equal to the requested source location id string.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:151-158, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:171-174
- Legacy: The provider forwards the id as-is; it does not pre-parse or pre-validate it.
- Decision: keep

**P1-010** When the service returns multiple source-location results, the provider maps only the first result row into the configuration model.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:175-179
- Legacy: Only `Results[0]` is used for configuration mapping.
- Decision: keep

**P1-011** When destination addresses are present, the provider appends each destination to `DestinationDetails` in the service-provided order and assigns a 1-based sequential `DestinationLineNumber` starting at 1.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:180-187, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:638-640
- Legacy: Destination line numbers are regenerated on every load and are not persisted.
- Decision: keep

## Service-side query rules

**P1-012** The source-location lookup requires a non-empty `Where.SourceLocationId`; an empty or missing id is rejected by throwing `FileTransferValidationException`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\GetSourceLocationQuery.cs:28-34
- Legacy: The service validates only “present vs empty”, not GUID format.
- Decision: keep

**P1-013** The service parses the `Where.SourceLocationId` value as a GUID using `Guid.Parse(...)` when building `SoureceLocationGuid`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\GetSourceLocationQueryParms.cs:30-35
- Legacy: Invalid GUID text triggers a `Guid.Parse` failure when `SoureceLocationGuid` is evaluated.
- Decision: keep
- ⚑ Flag: `Validate()` checks only for empty strings; a non-empty but non-GUID `SourceLocationId` can still fail later during GUID parsing.

**P1-014** A source location is returned only when there is exactly one `FtsSourceLocation` row matching the parsed source location GUID; otherwise the lookup returns no `FtsSourceLocation`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:130-135
- Legacy: The executor returns `null` when there are zero or more than one matching rows.
- Decision: keep

**P1-015** When a source location is found, the returned model includes `IsActive` from `FTS_SOURCE_LOCATION.IS_VALID` and `Suspended` from `FTS_SOURCE_LOCATION.IS_SUSPENDED`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:83-88
- Legacy: The “suspended” value comes from the source location row.
- Decision: keep
- ⚑ Flag: The brief mentions a read of `FTS_SUSPENSION`, but this query path populates “suspended” from `FTS_SOURCE_LOCATION.IS_SUSPENDED` instead.

**P1-016** Destination addresses returned with the source location are ordered by destination `CreationDate` descending (newest first).
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:98-104
- Legacy: Destination ordering is determined by the destination creation timestamp, not by destination id.
- Decision: keep

## Schedule and status presentation

**P1-017** The configuration run schedule is set to monthly (`RunSchedule.ID = 2`) when any of these are true: `RunOnLastDay`, `RunOnFirstDay`, or `RunOnThisDay.ID` is not empty and not equal to 0; otherwise it is weekly (`RunSchedule.ID = 1`).
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:500-505, ux\HP.HSP.UA3.FileTransfer.UX.Data\FileTransferConstants.cs:95-100
- Legacy: The schedule is inferred from the day-of-month fields and stored as a string id (1 or 2).
- Decision: keep

**P1-018** The configuration status label is set to the localized “Active” label when `IsActive` is true, otherwise it is set to the localized “Inactive” label.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:506-515
- Legacy: The label text is resolved through the localization provider using the current user locale.
- Decision: keep

**P1-019** Each destination configuration is marked as not newly added/updated (`IsAddedUpdated = false`) and its status label is localized as “Active” when `Active` is true, otherwise localized as “Inactive”.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.cs:638-651
- Legacy: Destination status is driven by the destination `IsValid` flag (mapped into `Active`).
- Decision: keep

**P1-020** When resolving `ImportJobName` for a destination with a non-null `FTEMetadataId`, the service looks up the first inbound target record with a matching `FTEMetadataId` and reads its `ImportJobName`.
- Doors: file-transfer-controller-file-transfer-configuration-view, file-transfer-controller-file-transfer-configuration-edit
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:144-152
- Legacy: The lookup is performed in-memory from an eagerly-loaded list of inbound targets.
- Decision: keep
- ⚑ Flag: If an inbound target is not found for a given non-null `FTEMetadataId`, the code dereferences `.ImportJobName` on a null object.
