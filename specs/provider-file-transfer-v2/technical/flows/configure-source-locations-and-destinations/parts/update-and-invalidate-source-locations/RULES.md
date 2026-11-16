---
type: Business Rules
title: 'Rules: Update and invalidate existing source locations'
description: 24 business rules (P3-NNN) of the flow, each with its legacy source lines.
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
  body_sha256: 2a302dff8bc0ed14ceea7843e979b8985d5397f45460cf7c7c1268b118b90477
---

# Rules: Update and invalidate existing source locations

## Doors

**P3-001** When `UpdateFileTransferSourceLocation` is called through the WCF service surfaces, the flow routes the `FtsSourceLocationUpdateCommand` through `TryRouteCommand(...)` and returns the routed result cast as `FtsSourceLocationUpdated`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:31, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:32, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:31
- Legacy: The WCF methods are thin wrappers; they do no validation or mapping beyond `as FtsSourceLocationUpdated`.
- Decision: keep

**P3-002** When `UpdateFileTransferSourceLocation` is called via the HTTP API controller, the controller returns HTTP 200 with the result of `IFileTransferSvcService.UpdateFileTransferSourceLocation(command)` as the response body.
- Doors: file-transfer-svc-service-controller-update-file-transfer-source-location
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:29-32
- Legacy: The controller always responds with `Ok(...)` and delegates all work to the injected service.
- Decision: keep

**P3-003** When `InvalidateFileTransferSourceLocation` is called through the WCF service surfaces, the flow routes the `FtsSourceLocationInvalidateCommand` through `TryRouteCommand(...)` and returns the routed result cast as `FtsSourceLocationInvalidated`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:37, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:38, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:37
- Legacy: The WCF methods are thin wrappers; they do no validation or mapping beyond `as FtsSourceLocationInvalidated`.
- Decision: keep

**P3-004** When `InvalidateFileTransferSourceLocation` is called via the HTTP API controller, the controller returns HTTP 200 with the result of `IFileTransferSvcService.InvalidateFileTransferSourceLocation(command)` as the response body.
- Doors: file-transfer-svc-service-controller-invalidate-file-transfer-source-location
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:35-37
- Legacy: The controller always responds with `Ok(...)` and delegates all work to the injected service.
- Decision: keep

## Update command validation

**P3-005** When updating a source location, the flow treats `ConfigName`, `Protocol`, `Address`, `OwnerName`, `OwnerModule`, `OwnerPhone`, `OwnerEmail`, `StartTime`, and `EndTime` as required string fields; each missing/empty value adds a validation entry with message `Missing field`, and if any validation entries exist the flow throws a `BusinessException` with message `Command Execution Error` and error code `E`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268-286, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:271-279, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:342-348, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28-41, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44-46, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:21, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:46
- Legacy: These fields can all be non-empty but still invalid for later processing (for example, a non-GUID `OwnerModule` can still fail later during GUID parsing).
- Decision: keep

**P3-006** When updating a source location, if no run-schedule indicators are selected (`RunOnThisDayOfMonth == 0` and all daily/monthly flags are false), the flow adds a validation entry with key `Run Schedule` and message `Invalid value for` and then fails validation as part of the command's `HandleValidations()`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:350-364, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:44, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:54
- Legacy: The message stored for this error is the generic prefix `Invalid value for` (it does not include the property name).
- Decision: keep

**P3-007** When updating a source location and `Protocol` equals `1`, the flow requires `FtpServerId` to be present; otherwise it throws `ArgumentNullException(FtpServerId)`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:24, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:289-296, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:295, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:41
- Legacy: The check is conditional on the literal protocol code value `1`.
- Decision: keep

**P3-008** When updating a source location, the flow requires `FtsSourceLocationId` to be present; otherwise it throws `ArgumentNullException(FtpSoureceLocationId)`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:300-306, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:304, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:92-99, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:88-95
- Legacy: The update handler does not use the inbound `FtsSourceLocationId`; it looks up the current source location by `ConfigId` and uses the found record's id.
- ⚑ Flag: Callers must supply `FtsSourceLocationId` even though the handler ignores it.
- Decision: keep

**P3-009** When updating a source location and `IsSuspended` is true, the flow requires `IsSuspendedChangedDate` to be present; otherwise it throws `ArgumentNullException(IsSuspendedChangedDate)`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:308-316, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:314
- Legacy: The date requirement is enforced only when `IsSuspended` is true.
- Decision: keep

**P3-010** When updating a source location, the flow requires `ConfigId` to be non-zero; otherwise it throws `ArgumentNullException(ConfigIdNotFound)`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:319-325, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:323, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23
- Legacy: The exception parameter name comes from the constant value `ConfigIdNotFound`.
- Decision: keep

**P3-011** When updating a source location, if `Mask` ends with one of the excluded extensions (`exe`, `dll`, `bat`, `cmd`, `vbs`, `ps1`, `jar`, `app`, `com`, `scr`, `msi`, `js`, `wsf`, `reg`, `hta`, `pif`, `vb`, `py`, `rb`, `sh`, `cpl`, `msc`), the flow adds a validation entry for `Mask` with message `Source file mask extension is not Allowed: <extension>` and then fails validation as part of `HandleValidations()`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:327-335, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:337-339, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\ExcludedFileExtensions.cs:13-36, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:41
- Legacy: The extension is computed from the substring after the last `.` and compared case-insensitively by lowercasing it.
- Decision: keep

## Invalidate command validation

**P3-012** When invalidating a source location, the flow requires `OperatorId` to be present; otherwise it throws `ArgumentNullException(OperatorId)`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58-63, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:73-79, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:77
- Legacy: The command performs only null/empty checks and throws immediately.
- Decision: keep

**P3-013** When invalidating a source location, the flow requires `FtsSourceLocationId` to be present; otherwise it throws `ArgumentNullException(FtpSoureceLocationId)`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58-63, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:65-71, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:69, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:133-140, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:129-136
- Legacy: The invalidate handler does not use the inbound `FtsSourceLocationId`; it looks up the current source location by `ConfigId` and uses the found record's id.
- ⚑ Flag: Callers must supply `FtsSourceLocationId` even though the handler ignores it.
- Decision: keep

**P3-014** When invalidating a source location, the flow requires `ConfigId` to be non-zero; otherwise it throws `ArgumentNullException(ConfigIdNotFound)`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:81-87, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:85, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23
- Legacy: The exception parameter name comes from the constant value `ConfigIdNotFound`.
- Decision: keep

## Command handling (event creation)

**P3-015** When handling an update command, if no source location exists for the provided `ConfigId`, the flow throws a `BusinessException` with one entry (`keyName = ConfigId`, `message = ConfigIdNotFound`, `processingSystem = FileTransfer`) and exception message `Business Error occurred processing the command.`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143-163, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139-159, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:29, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:37
- Legacy: Existence is checked by executing a `SourceLocationQuery` filtered only by `ConfigId`.
- Decision: keep

**P3-016** When handling an invalidate command, if no source location exists for the provided `ConfigId`, the flow throws a `BusinessException` with one entry (`keyName = ConfigId`, `message = ConfigIdNotFound`, `processingSystem = FileTransfer`) and exception message `Business Error occurred processing the command.`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143-163, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139-159, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:29, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:37
- Legacy: Existence is checked by executing a `SourceLocationQuery` filtered only by `ConfigId`.
- Decision: keep

**P3-017** When handling an update command for an existing `ConfigId`, the flow creates `FtsSourceLocationUpdated` using the stored source location id (as `FtsSourceLocationsId`) and stored `ConfigId`, while copying the remaining configuration fields and `DestinationAddress` from the command.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:89-127, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:85-123
- Legacy: `OperatorId` is taken from `command.OperatorId` (not the command's `Requestor`).
- Decision: keep

**P3-018** When handling an invalidate command for an existing `ConfigId`, the flow creates `FtsSourceLocationInvalidated` using the stored source location id (as `FtsSourceLocationsId`) and stored `ConfigId`, while copying `OperatorId` and `DestinationAddress` from the command.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:130-141, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:126-137
- Legacy: The invalidation event is built from the looked-up source location for `ConfigId`, not from the inbound `FtsSourceLocationId`.
- Decision: keep

## Projection updates (source location and destinations)

**P3-019** When applying a `FtsSourceLocationUpdated` event, the flow updates the persisted source location identified by `FtsSourceLocationsId` and sets its fields directly from the event, including `IsSuspended`, `IsSuspendedChangedDate`, `IsValid`, and `OperatorId`, and updates `LastModifiedDate` to the current UTC timestamp.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89-132, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89-132, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:100, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:110, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:126
- Legacy: `FtpServerId` is stored as null when the event's `FtpServerId` is empty; otherwise the code parses it as a GUID.
- ⚑ Flag: `OwnerModule` and non-empty `FtpServerId` are parsed using `Guid.Parse(...)` and will throw if the event contains a non-GUID string.
- Decision: keep

**P3-020** When applying a `FtsSourceLocationUpdated` event, the flow attempts to upsert destination locations by calling `AddDestinationAddress(@event.DestinationAddress, @event.UserID)` whenever `FtsSourceLocationsId` is non-null.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:129-138, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:129-138, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:133, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165-188
- Legacy: The update path does not check whether `DestinationAddress` is null before calling `AddDestinationAddress(...)`.
- ⚑ Flag: A null `DestinationAddress` list will raise an exception during `foreach` in `AddDestinationAddress(...)`.
- Decision: keep

**P3-021** When applying a `FtsSourceLocationUpdated` event with `IsValid == false`, the flow marks every destination location row linked to the source location as invalid (`IsValid = false`) and sets each row's `OperatorId` to the event's `OperatorId`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:134-137, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:134-137, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:136
- Legacy: The flow saves changes after updating destination validity.
- Decision: keep

**P3-022** When applying a `FtsSourceLocationInvalidated` event, the flow marks the source location identified by `FtsSourceLocationsId` as invalid (`IsValid = false`) and stores `OperatorId` from the event.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:143-156, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:143-156
- Legacy: The invalidation projection does not update `LastModifiedDate`.
- Decision: keep

**P3-023** When applying a `FtsSourceLocationInvalidated` event, if `DestinationAddress` is provided the flow upserts destination locations from that list and then marks every destination location row linked to the source location as invalid (`IsValid = false`) and sets each row's `OperatorId` to the event's `OperatorId`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:154-163, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:154-163, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:158-162, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:161
- Legacy: Any destination rows upserted in this step are immediately marked invalid in the same handler.
- Decision: keep

**P3-024** When upserting destination locations (`AddDestinationAddress(...)`), the flow checks each inbound destination's `DestinationId`: if it matches an existing row the flow updates that row's fields; otherwise it inserts a new row with a freshly generated `DestinationId` and links it to the current source location id.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-controller-update-file-transfer-source-location
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165-188, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191-217, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:195, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:219-241, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:229
- Legacy: The insert path ignores any inbound `DestinationId` value and always generates a new GUID.
- ⚑ Flag: Callers cannot request a specific `DestinationId` for a new destination; the system always assigns one.
- Decision: keep

