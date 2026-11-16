---
type: Technical Flow Map
title: 'Technical map: Configure file transfer source locations and destinations'
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
  body_sha256: 91fc34c78a45aadf5bb7a71d13a085a63c345886cde6e62bbf84007fec590808
---

# Technical map: Configure file transfer source locations and destinations

How the legacy code implements the flow `configure-source-locations-and-destinations`, as the code graph sees it. Written by the tool from `FOOTPRINT.yaml`, not by AI; the business view is in `FLOW.md` and `RULES.md`.

## Doors

| Door | Kind | Where |
| --- | --- | --- |
| `file-transfer-controller-file-transfer-configuration-view` (FileTransferConfigurationView) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:88` |
| `file-transfer-controller-file-transfer-configuration-edit` (FileTransferConfigurationEdit) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:113` |
| `file-transfer-controller-add-file-base-configuration` (AddFileBaseConfiguration) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:62` |
| `file-transfer-controller-add-file-transfer-base-configuration` (AddFileTransferBaseConfiguration) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:21` |
| `file-transfer-controller-destination-configuration-edit` (DestinationConfigurationEdit) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:132` |
| `file-transfer-controller-destination-configuration-edit-panel` (DestinationConfigurationEditPanel) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:103` |
| `file-transfer-svc-service-file-transfer-add` (FileTransferAdd) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:25` |
| `file-transfer-svc-service-file-transfer-add-2` (FileTransferAdd) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:26` |
| `file-transfer-svc-service-file-transfer-add-3` (FileTransferAdd) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:25` |
| `file-transfer-svc-service-controller-file-transfer-add` (FileTransferAdd) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:22` |
| `file-transfer-svc-service-update-file-transfer-source-location` (UpdateFileTransferSourceLocation) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:31` |
| `file-transfer-svc-service-update-file-transfer-source-location-2` (UpdateFileTransferSourceLocation) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:32` |
| `file-transfer-svc-service-update-file-transfer-source-location-3` (UpdateFileTransferSourceLocation) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:31` |
| `file-transfer-svc-service-controller-update-file-transfer-source-location` (UpdateFileTransferSourceLocation) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:28` |
| `file-transfer-svc-service-invalidate-file-transfer-source-location` (InvalidateFileTransferSourceLocation) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:37` |
| `file-transfer-svc-service-invalidate-file-transfer-source-location-2` (InvalidateFileTransferSourceLocation) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:38` |
| `file-transfer-svc-service-invalidate-file-transfer-source-location-3` (InvalidateFileTransferSourceLocation) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:37` |
| `file-transfer-svc-service-controller-invalidate-file-transfer-source-location` (InvalidateFileTransferSourceLocation) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:34` |
| `file-transfer-svc-query-service-get-source-locations` (GetSourceLocations) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:40` |
| `file-transfer-svc-query-service-get-source-locations-2` (GetSourceLocations) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:31` |
| `file-transfer-svc-query-service-get-source-locations-3` (GetSourceLocations) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:31` |
| `file-transfer-svc-query-service-controller-get-source-locations` (GetSourceLocations) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:39` |
| `file-transfer-controller-get-source-locations` (GetSourceLocations) | api | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:24` |
| `file-transfer-svc-query-service-search-source-locations` (SearchSourceLocations) | wcf | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:25` |
| `file-transfer-svc-query-service-search-source-locations-2` (SearchSourceLocations) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:16` |
| `file-transfer-svc-query-service-search-source-locations-3` (SearchSourceLocations) | wcf | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:16` |
| `file-transfer-svc-query-service-controller-search-source-locations` (SearchSourceLocations) | api | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:21` |

## Call chains

### `file-transfer-controller-file-transfer-configuration-view`

- `FileTransferController.FileTransferConfigurationView` `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:88`
  - `FileTransferController.GetFileTransferConfiguration` `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:142`
    - reads table `orm:FTS_SUSPENSION`
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-controller-file-transfer-configuration-edit`

- `FileTransferController.FileTransferConfigurationEdit` `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:113`
  - `FileTransferController.GetFileTransferConfiguration` `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:142`
    - reads table `orm:FTS_SUSPENSION`
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-controller-add-file-base-configuration`

- `FileTransferController.AddFileBaseConfiguration` `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:62`
  - reads table `orm:FTS_SUSPENSION`

### `file-transfer-controller-add-file-transfer-base-configuration`

- `FileTransferController.AddFileTransferBaseConfiguration` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:21`
  - `FileTransferController.GenerateAddFileTransferBaseConfigurationResult` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:40`
    - reads table `orm:FTS_SUSPENSION`
  - `FileTransferController.GenerateInvalidModelStateRepsonse` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:65`

### `file-transfer-controller-destination-configuration-edit`

- `FileTransferController.DestinationConfigurationEdit` `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:132`

### `file-transfer-controller-destination-configuration-edit-panel`

- `FileTransferController.DestinationConfigurationEditPanel` `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:103`

### `file-transfer-svc-service-file-transfer-add`

- `FileTransferSvcService.FileTransferAdd` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:25`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69`
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:48`
    - `FtsSourceLocationAddCommandHandler.CreateConfigNumber` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:165`
      - reads table `orm:FTS_SUSPENSION`
    - `FtsSourceLocationAddCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:272`
      - `FtsCommand.FtsCommandValidate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28` **- walk stops here**
      - `FtsCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33` **- walk stops here**
      - `FtsSourceLocationAddCommand.ValidateDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:327`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` **- walk stops here**
      - `FtsSourceLocationAddCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:293`
        - reads table `orm:FTP_SERVER`
      - `FtsSourceLocationAddCommand.ValidateMaskExtension` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:304`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
        - `FtsSourceLocationAddCommand.BuildMaskValidationMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:314` **- walk stops here**
      - `FtsSourceLocationAddCommand.ValidateRequiredField` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:319`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - `FtsSourceLocationAddCommand.ValidateRunSchedule` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:335`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36`
      - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
        - writes table `orm:FTP_SERVER` (see above)
        - writes table `orm:FTS_DESTINATION`
        - writes table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36`
      - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
        - writes table `orm:FTP_SERVER` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:44`
    - `FtsSourceLocationAddCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:272` (see above)
    - `FtsSourceLocationAddCommandHandler.CreateConfigNumber` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:161`
      - reads table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-file-transfer-add-2`

- `FileTransferSvcService.FileTransferAdd` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:26`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:48`
    - `FtsSourceLocationAddCommandHandler.CreateConfigNumber` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:165`
      - reads table `orm:FTS_SUSPENSION`
    - `FtsSourceLocationAddCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:272`
      - `FtsCommand.FtsCommandValidate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28` **- walk stops here**
      - `FtsCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33` **- walk stops here**
      - `FtsSourceLocationAddCommand.ValidateDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:327`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` **- walk stops here**
      - `FtsSourceLocationAddCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:293`
        - reads table `orm:FTP_SERVER`
      - `FtsSourceLocationAddCommand.ValidateMaskExtension` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:304`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
        - `FtsSourceLocationAddCommand.BuildMaskValidationMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:314` **- walk stops here**
      - `FtsSourceLocationAddCommand.ValidateRequiredField` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:319`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - `FtsSourceLocationAddCommand.ValidateRunSchedule` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:335`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36`
      - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
        - writes table `orm:FTP_SERVER` (see above)
        - writes table `orm:FTS_DESTINATION`
        - writes table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36`
      - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
        - writes table `orm:FTP_SERVER` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:44`
    - `FtsSourceLocationAddCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:272` (see above)
    - `FtsSourceLocationAddCommandHandler.CreateConfigNumber` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:161`
      - reads table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-file-transfer-add-3`

- `FileTransferSvcService.FileTransferAdd` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:25`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69`
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:48`
    - `FtsSourceLocationAddCommandHandler.CreateConfigNumber` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:165`
      - reads table `orm:FTS_SUSPENSION`
    - `FtsSourceLocationAddCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:272`
      - `FtsCommand.FtsCommandValidate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28` **- walk stops here**
      - `FtsCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33` **- walk stops here**
      - `FtsSourceLocationAddCommand.ValidateDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:327`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` **- walk stops here**
      - `FtsSourceLocationAddCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:293`
        - reads table `orm:FTP_SERVER`
      - `FtsSourceLocationAddCommand.ValidateMaskExtension` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:304`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
        - `FtsSourceLocationAddCommand.BuildMaskValidationMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:314` **- walk stops here**
      - `FtsSourceLocationAddCommand.ValidateRequiredField` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:319`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - `FtsSourceLocationAddCommand.ValidateRunSchedule` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:335`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36`
      - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
        - writes table `orm:FTP_SERVER` (see above)
        - writes table `orm:FTS_DESTINATION`
        - writes table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36`
      - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
        - writes table `orm:FTP_SERVER` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:44`
    - `FtsSourceLocationAddCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:272` (see above)
    - `FtsSourceLocationAddCommandHandler.CreateConfigNumber` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:161`
      - reads table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-controller-file-transfer-add`

- `FileTransferSvcServiceController.FileTransferAdd` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:22`
  - `IFileTransferSvcService.FileTransferAdd` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcService.cs:59`
    - implementation: `FileTransferSvcService.FileTransferAdd` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:26`
      - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
      - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:48`
        - `FtsSourceLocationAddCommandHandler.CreateConfigNumber` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:165`
          - reads table `orm:FTS_SUSPENSION`
        - `FtsSourceLocationAddCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:272`
          - `FtsCommand.FtsCommandValidate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28` **- walk stops here**
          - `FtsCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33` **- walk stops here**
          - `FtsSourceLocationAddCommand.ValidateDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:327`
            - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` **- walk stops here**
          - `FtsSourceLocationAddCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:293`
            - reads table `orm:FTP_SERVER`
          - `FtsSourceLocationAddCommand.ValidateMaskExtension` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:304`
            - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
            - `FtsSourceLocationAddCommand.BuildMaskValidationMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:314` **- walk stops here**
          - `FtsSourceLocationAddCommand.ValidateRequiredField` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:319`
            - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
          - `FtsSourceLocationAddCommand.ValidateRunSchedule` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:335`
            - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36`
          - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
            - writes table `orm:FTP_SERVER` (see above)
            - writes table `orm:FTS_DESTINATION`
            - writes table `orm:FTS_SUSPENSION` (see above)
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36`
          - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
            - writes table `orm:FTP_SERVER` (see above)
            - writes table `orm:FTS_DESTINATION` (see above)
            - writes table `orm:FTS_SUSPENSION` (see above)
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTS_SUSPENSION` (see above)
      - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:44`
        - `FtsSourceLocationAddCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:272` (see above)
        - `FtsSourceLocationAddCommandHandler.CreateConfigNumber` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:161`
          - reads table `orm:FTS_SUSPENSION` (see above)
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36` (see above)
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:36` (see above)
        - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTS_SUSPENSION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-update-file-transfer-source-location`

- `FileTransferSvcService.UpdateFileTransferSourceLocation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:31`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69`
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:89`
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143`
    - `FtsSourceLocationUpdateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268`
      - `FtsCommand.FtsCommandValidate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28` **- walk stops here**
      - `FtsCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateConfigId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:319` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:289`
        - reads table `orm:FTP_SERVER`
      - `FtsSourceLocationUpdateCommand.ValidateFtpSourceLocationId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:300`
        - reads table `orm:FTS_SUSPENSION`
      - `FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:308` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateMaskExtension` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:327`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` **- walk stops here**
        - `FtsSourceLocationUpdateCommand.BuildMaskValidationMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:337` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateRequiredField` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:342`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - `FtsSourceLocationUpdateCommand.ValidateRunSchedule` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:350`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_DESTINATION`
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_DESTINATION` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:85`
    - `FtsSourceLocationUpdateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268` (see above)
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139`
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-update-file-transfer-source-location-2`

- `FileTransferSvcService.UpdateFileTransferSourceLocation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:32`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:89`
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143`
    - `FtsSourceLocationUpdateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268`
      - `FtsCommand.FtsCommandValidate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28` **- walk stops here**
      - `FtsCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateConfigId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:319` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:289`
        - reads table `orm:FTP_SERVER`
      - `FtsSourceLocationUpdateCommand.ValidateFtpSourceLocationId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:300`
        - reads table `orm:FTS_SUSPENSION`
      - `FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:308` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateMaskExtension` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:327`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` **- walk stops here**
        - `FtsSourceLocationUpdateCommand.BuildMaskValidationMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:337` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateRequiredField` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:342`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - `FtsSourceLocationUpdateCommand.ValidateRunSchedule` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:350`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_DESTINATION`
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_DESTINATION` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:85`
    - `FtsSourceLocationUpdateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268` (see above)
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139`
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-update-file-transfer-source-location-3`

- `FileTransferSvcService.UpdateFileTransferSourceLocation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:31`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69`
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:89`
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143`
    - `FtsSourceLocationUpdateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268`
      - `FtsCommand.FtsCommandValidate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28` **- walk stops here**
      - `FtsCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateConfigId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:319` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:289`
        - reads table `orm:FTP_SERVER`
      - `FtsSourceLocationUpdateCommand.ValidateFtpSourceLocationId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:300`
        - reads table `orm:FTS_SUSPENSION`
      - `FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:308` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateMaskExtension` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:327`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` **- walk stops here**
        - `FtsSourceLocationUpdateCommand.BuildMaskValidationMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:337` **- walk stops here**
      - `FtsSourceLocationUpdateCommand.ValidateRequiredField` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:342`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - `FtsSourceLocationUpdateCommand.ValidateRunSchedule` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:350`
        - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
      - reads table `orm:FTP_SERVER` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_DESTINATION`
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_DESTINATION` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTP_SERVER` (see above)
      - writes table `orm:FTP_SERVER` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:85`
    - `FtsSourceLocationUpdateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268` (see above)
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139`
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89` (see above)
    - reads table `orm:FTP_SERVER` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-controller-update-file-transfer-source-location`

- `FileTransferSvcServiceController.UpdateFileTransferSourceLocation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:28`
  - `IFileTransferSvcService.UpdateFileTransferSourceLocation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcService.cs:82`
    - implementation: `FileTransferSvcService.UpdateFileTransferSourceLocation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:32`
      - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
      - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:89`
        - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143`
        - `FtsSourceLocationUpdateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268`
          - `FtsCommand.FtsCommandValidate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28` **- walk stops here**
          - `FtsCommand.HandleValidations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33` **- walk stops here**
          - `FtsSourceLocationUpdateCommand.ValidateConfigId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:319` **- walk stops here**
          - `FtsSourceLocationUpdateCommand.ValidateFtpServerId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:289`
            - reads table `orm:FTP_SERVER`
          - `FtsSourceLocationUpdateCommand.ValidateFtpSourceLocationId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:300`
            - reads table `orm:FTS_SUSPENSION`
          - `FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:308` **- walk stops here**
          - `FtsSourceLocationUpdateCommand.ValidateMaskExtension` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:327`
            - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` **- walk stops here**
            - `FtsSourceLocationUpdateCommand.BuildMaskValidationMessage` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:337` **- walk stops here**
          - `FtsSourceLocationUpdateCommand.ValidateRequiredField` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:342`
            - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
          - `FtsSourceLocationUpdateCommand.ValidateRunSchedule` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:350`
            - `FtsCommand.AddValidation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44` (see above)
          - reads table `orm:FTP_SERVER` (see above)
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89`
          - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
            - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
              - writes table `orm:FTP_SERVER` (see above)
              - writes table `orm:FTS_DESTINATION`
              - writes table `orm:FTS_SUSPENSION` (see above)
            - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
              - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTS_DESTINATION` (see above)
            - reads table `orm:FTS_DESTINATION` (see above)
            - writes table `orm:FTS_DESTINATION` (see above)
          - reads table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
          - writes table `orm:FTS_DESTINATION` (see above)
          - reads table `orm:FTS_SUSPENSION` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89`
          - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
            - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
              - writes table `orm:FTP_SERVER` (see above)
              - writes table `orm:FTS_DESTINATION` (see above)
              - writes table `orm:FTS_SUSPENSION` (see above)
            - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
              - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTS_DESTINATION` (see above)
            - reads table `orm:FTS_DESTINATION` (see above)
            - writes table `orm:FTS_DESTINATION` (see above)
          - reads table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
          - writes table `orm:FTS_DESTINATION` (see above)
          - reads table `orm:FTS_SUSPENSION` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTS_SUSPENSION` (see above)
      - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:85`
        - `FtsSourceLocationUpdateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268` (see above)
        - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139`
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89` (see above)
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89` (see above)
        - reads table `orm:FTP_SERVER` (see above)
        - reads table `orm:FTS_SUSPENSION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-invalidate-file-transfer-source-location`

- `FileTransferSvcService.InvalidateFileTransferSourceLocation` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:37`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69`
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:130`
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143`
    - `FtsSourceLocationInvalidateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58`
      - `FtsSourceLocationInvalidateCommand.ValidateConfigId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:81` **- walk stops here**
      - `FtsSourceLocationInvalidateCommand.ValidateFtpSourceLocationId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:65`
        - reads table `orm:FTS_SUSPENSION`
      - `FtsSourceLocationInvalidateCommand.ValidateOperatorId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:73` **- walk stops here**
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER`
          - writes table `orm:FTS_DESTINATION`
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_DESTINATION` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:126`
    - `FtsSourceLocationInvalidateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58` (see above)
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139`
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-invalidate-file-transfer-source-location-2`

- `FileTransferSvcService.InvalidateFileTransferSourceLocation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:38`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:130`
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143`
    - `FtsSourceLocationInvalidateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58`
      - `FtsSourceLocationInvalidateCommand.ValidateConfigId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:81` **- walk stops here**
      - `FtsSourceLocationInvalidateCommand.ValidateFtpSourceLocationId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:65`
        - reads table `orm:FTS_SUSPENSION`
      - `FtsSourceLocationInvalidateCommand.ValidateOperatorId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:73` **- walk stops here**
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER`
          - writes table `orm:FTS_DESTINATION`
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_DESTINATION` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:126`
    - `FtsSourceLocationInvalidateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58` (see above)
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139`
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-invalidate-file-transfer-source-location-3`

- `FileTransferSvcService.InvalidateFileTransferSourceLocation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:37`
  - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69`
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:130`
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143`
    - `FtsSourceLocationInvalidateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58`
      - `FtsSourceLocationInvalidateCommand.ValidateConfigId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:81` **- walk stops here**
      - `FtsSourceLocationInvalidateCommand.ValidateFtpSourceLocationId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:65`
        - reads table `orm:FTS_SUSPENSION`
      - `FtsSourceLocationInvalidateCommand.ValidateOperatorId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:73` **- walk stops here**
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER`
          - writes table `orm:FTS_DESTINATION`
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142`
      - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
        - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
          - writes table `orm:FTP_SERVER` (see above)
          - writes table `orm:FTS_DESTINATION` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
          - reads table `orm:FTP_SERVER` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
        - reads table `orm:FTS_DESTINATION` (see above)
        - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_DESTINATION` (see above)
      - writes table `orm:FTS_DESTINATION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
      - writes table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:126`
    - `FtsSourceLocationInvalidateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58` (see above)
    - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139`
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142` (see above)
    - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-service-controller-invalidate-file-transfer-source-location`

- `FileTransferSvcServiceController.InvalidateFileTransferSourceLocation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:34`
  - `IFileTransferSvcService.InvalidateFileTransferSourceLocation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcService.cs:105`
    - implementation: `FileTransferSvcService.InvalidateFileTransferSourceLocation` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:38`
      - `FileTransferSvcService.TryRouteCommand` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`
      - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:130`
        - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143`
        - `FtsSourceLocationInvalidateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58`
          - `FtsSourceLocationInvalidateCommand.ValidateConfigId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:81` **- walk stops here**
          - `FtsSourceLocationInvalidateCommand.ValidateFtpSourceLocationId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:65`
            - reads table `orm:FTS_SUSPENSION`
          - `FtsSourceLocationInvalidateCommand.ValidateOperatorId` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:73` **- walk stops here**
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142`
          - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
            - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
              - writes table `orm:FTP_SERVER`
              - writes table `orm:FTS_DESTINATION`
              - writes table `orm:FTS_SUSPENSION` (see above)
            - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
              - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTS_DESTINATION` (see above)
            - reads table `orm:FTS_DESTINATION` (see above)
            - writes table `orm:FTS_DESTINATION` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
          - writes table `orm:FTS_DESTINATION` (see above)
          - reads table `orm:FTS_SUSPENSION` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142`
          - `FtsSourceLocationProjectionWriter.AddDestinationAddress` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165`
            - `FtsSourceLocationProjectionWriter.AddDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:191`
              - writes table `orm:FTP_SERVER` (see above)
              - writes table `orm:FTS_DESTINATION` (see above)
              - writes table `orm:FTS_SUSPENSION` (see above)
            - `FtsSourceLocationProjectionWriter.UpdateDestination` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:218`
              - reads table `orm:FTP_SERVER` (see above)
              - reads table `orm:FTS_DESTINATION` (see above)
            - reads table `orm:FTS_DESTINATION` (see above)
            - writes table `orm:FTS_DESTINATION` (see above)
          - reads table `orm:FTS_DESTINATION` (see above)
          - writes table `orm:FTS_DESTINATION` (see above)
          - reads table `orm:FTS_SUSPENSION` (see above)
          - writes table `orm:FTS_SUSPENSION` (see above)
        - reads table `orm:FTS_SUSPENSION` (see above)
      - routed to: `FtsSourceLocationAddCommandHandler.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:126`
        - `FtsSourceLocationInvalidateCommand.Validate` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58` (see above)
        - `FtsSourceLocationAddCommandHandler.IsSourceLocationExists` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139`
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142` (see above)
        - routed to: `FtsSourceLocationProjectionWriter.Handle` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:142` (see above)
        - reads table `orm:FTS_SUSPENSION` (see above)
      - reads table `orm:FTS_SUSPENSION` (see above)
    - reads table `orm:FTS_SUSPENSION` (see above)
  - reads table `orm:FTS_SUSPENSION` (see above)

### `file-transfer-svc-query-service-get-source-locations`

- `FileTransferSvcQueryService.GetSourceLocations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:40`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55`

### `file-transfer-svc-query-service-get-source-locations-2`

- `FileTransferSvcQueryService.GetSourceLocations` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:31`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`

### `file-transfer-svc-query-service-get-source-locations-3`

- `FileTransferSvcQueryService.GetSourceLocations` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:31`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46`

### `file-transfer-svc-query-service-controller-get-source-locations`

- `FileTransferSvcQueryServiceController.GetSourceLocations` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:39`
  - `IFileTransferSvcQueryService.GetSourceLocations` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcQueryService.cs:50`
    - implementation: `FileTransferSvcQueryService.GetSourceLocations` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:31`
      - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`

### `file-transfer-controller-get-source-locations`

- `FileTransferController.GetSourceLocations` `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:24`

### `file-transfer-svc-query-service-search-source-locations`

- `FileTransferSvcQueryService.SearchSourceLocations` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:25`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55`

### `file-transfer-svc-query-service-search-source-locations-2`

- `FileTransferSvcQueryService.SearchSourceLocations` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:16`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`

### `file-transfer-svc-query-service-search-source-locations-3`

- `FileTransferSvcQueryService.SearchSourceLocations` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:16`
  - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46`

### `file-transfer-svc-query-service-controller-search-source-locations`

- `FileTransferSvcQueryServiceController.SearchSourceLocations` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:21`
  - `IFileTransferSvcQueryService.SearchSourceLocations` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Interfaces\IFileTransferSvcQueryService.cs:32`
    - implementation: `FileTransferSvcQueryService.SearchSourceLocations` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:16`
      - `FileTransferSvcQueryService.TryRouteQuery` `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`

## Classes involved (by namespace; methods walked)

- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc`: `FileTransferSvcQueryService` (3), `FileTransferSvcService` (4)
- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers`: `FtsSourceLocationAddCommandHandler` (5)
- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands`: `FtsCommand` (3), `FtsSourceLocationAddCommand` (7), `FtsSourceLocationInvalidateCommand` (4), `FtsSourceLocationUpdateCommand` (9)
- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers`: `FtsSourceLocationProjectionWriter` (6)
- `HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers`: `FileTransferController` (6)
- `HP.HSP.UA3.FileTransfer.UX.Services.Controller`: `FileTransferController` (4)
- `HPP.FileTransfer.BAS.FileTransferApi.Commands`: `FileTransferSvcService` (4)
- `HPP.FileTransfer.BAS.FileTransferApi.Controllers`: `FileTransferSvcQueryServiceController` (2), `FileTransferSvcServiceController` (3)
- `HPP.FileTransfer.BAS.FileTransferApi.Interfaces`: `IFileTransferSvcQueryService` (2), `IFileTransferSvcService` (3)
- `HPP.FileTransfer.BAS.FileTransferApi.Queries`: `FileTransferSvcQueryService` (3)
- `HPP.FileTransfer.BAS.FileTransferSvc`: `FileTransferSvcQueryService` (3), `FileTransferSvcService` (4)
- `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers`: `FtsSourceLocationAddCommandHandler` (5)
- `HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers`: `FtsSourceLocationProjectionWriter` (6)

## Data

- table `orm:FTP_SERVER` (reads, writes)
- table `orm:FTS_DESTINATION` (reads, writes)
- table `orm:FTS_SUSPENSION` (reads, writes)

## Calls into code the system does not own

- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.ITenantConfigurationManager.Resolve<T>(string)` (3 call sites)
- **hp.hsp.ua3.core.ux.interfaces** 30.0.0: `HP.HSP.UA3.Core.UX.Interfaces.IUserConfigurationManager.GenerateRequestor()` (3 call sites)
- **hp.hsp.ua3.core.ux.presentation** 30.0.0: `HP.HSP.UA3.Core.UX.Presentation.Controllers.Base.BaseViewController.View(object)` (5 call sites)
- **hp.hsp.ua3.core.ux.services** 30.0.0: `HP.HSP.UA3.Core.UX.Services.Controllers.Base.BaseApiController.FormatResultModel(System.Web.Http.ModelBinding.ModelStateDictionary)` (1 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.CommandServiceBase.TryRoute(HPP.Core.BAS.CQRS.Base.Command)` (2 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.ProjectionWriterBase<T>.Add(T)` (1 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.ProjectionWriterBase<T>.Update(dynamic, System.Action<T>)` (2 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.QueryExecutorBase<TInput, TQuery>.Execute(HPP.Core.BAS.CQRS.Interfaces.IDbContextBase, TQuery)` (1 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Base.QueryServiceBase.TryRoute<TQuery>(TQuery)` (2 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Interfaces.IDbContextBase.GetNextSequenceValue(string)` (1 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Interfaces.IDbContextBase.SaveChanges()` (4 call sites)
- **hpp.core.bas.cqrs** 30.0.0: `HPP.Core.BAS.CQRS.Interfaces.IDbContextBase.Set<T>()` (5 call sites)
- **microsoft.aspnet.mvc** 5.3.0: `System.Web.Mvc.Controller.View()` (5 call sites)
- **microsoft.aspnet.mvc** 5.3.0: `System.Web.Mvc.Controller.View(string, object)` (1 call sites)
- **microsoft.aspnet.webapi.core** 5.3.0: `System.Net.Http.HttpRequestMessageExtensions.CreateResponse<T>(System.Net.Http.HttpRequestMessage, System.Net.HttpStatusCode, T)` (3 call sites)
- **microsoft.entityframeworkcore** 9.0.12: `Microsoft.EntityFrameworkCore.DbSet<TEntity>.Add(TEntity)` (1 call sites)
- **** : `?.Add` (1 call sites)
- **** : `?.FirstOrDefault` (2 call sites)
- **** : `?.ForEach` (2 call sites)
- **** : `?.Set` (2 call sites)
- **** : `?.ToList` (2 call sites)
- **** : `?.Where` (3 call sites)
- **** : `CommandServiceBase.TryRoute` (1 call sites)
- **** : `Guid.ToString` (2 call sites)
- **** : `IDbContextBase.GetNextSequenceValue` (1 call sites)
- **** : `IDbContextBase.SaveChanges` (4 call sites)
- **** : `IDbContextBase.Set` (3 call sites)
- **** : `ProjectionWriterBase<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsSourceLocation>.Add` (1 call sites)
- **** : `ProjectionWriterBase<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsSourceLocation>.Update` (2 call sites)
- **** : `QueryExecutorBase<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.SourceLocationModel, HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.SourceLocationQuery>.Execute` (1 call sites)
- **** : `QueryServiceBase.TryRoute` (1 call sites)
- **** : `existingDestination.Where` (1 call sites)
- **** : `string.Replace` (1 call sites)
- **** : `string.ToString` (2 call sites)

## Where the walk stops

The graph sees nothing these methods call: a router, a call into an unresolved package, reflection, configuration, or simply a leaf. Read the code from here.

- `FtsCommand.AddValidation` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44`
- `FtsCommand.FtsCommandValidate` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28`
- `FtsCommand.HandleValidations` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33`
- `FtsSourceLocationAddCommand.BuildMaskValidationMessage` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:314`
- `FtsSourceLocationInvalidateCommand.ValidateConfigId` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:81`
- `FtsSourceLocationInvalidateCommand.ValidateOperatorId` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:73`
- `FtsSourceLocationUpdateCommand.BuildMaskValidationMessage` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:337`
- `FtsSourceLocationUpdateCommand.ValidateConfigId` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:319`
- `FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate` at `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:308`

## Shared pieces used (not walked)

None.
