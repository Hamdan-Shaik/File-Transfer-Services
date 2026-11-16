---
type: Generation Brief
title: 'Brief: split flow configure-source-locations-and-destinations into parts'
description: Instructions the tool gave the writer session for splitting this flow into parts.
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
  body_sha256: 9b0dcc3fbbd7ec394108d2c14c92be190b89885657303286055b024d968471f2
---

# Brief: split flow `configure-source-locations-and-destinations` into parts

- Business name: **Configure file transfer source locations and destinations**
- 27 doors; a part holds at most 12, so at least 3 parts
- Write exactly: `technical/flows/configure-source-locations-and-destinations/PARTS.yaml`
- Playbook: `inputs/playbook/parts.md`
- What each door reaches: `technical/doors/<door id>.yaml`; the flow's map: `technical/flows/configure-source-locations-and-destinations/TECH.md`

## Doors

| Door id | Kind | Name | Class | Where | Endpoints |
| --- | --- | --- | --- | --- | --- |
| `file-transfer-controller-file-transfer-configuration-view` | api | FileTransferConfigurationView | HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:88` | GET /FileTransfer |
| `file-transfer-controller-file-transfer-configuration-edit` | api | FileTransferConfigurationEdit | HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:113` | POST /FileTransfer |
| `file-transfer-controller-add-file-base-configuration` | api | AddFileBaseConfiguration | HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:62` | POST /FileTransfer |
| `file-transfer-controller-add-file-transfer-base-configuration` | api | AddFileTransferBaseConfiguration | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:21` | - |
| `file-transfer-controller-destination-configuration-edit` | api | DestinationConfigurationEdit | HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:132` | POST /FileTransfer |
| `file-transfer-controller-destination-configuration-edit-panel` | api | DestinationConfigurationEditPanel | HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:103` | POST /FileTransfer |
| `file-transfer-svc-service-file-transfer-add` | wcf | FileTransferAdd | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:25` | - |
| `file-transfer-svc-service-file-transfer-add-2` | wcf | FileTransferAdd | HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:26` | - |
| `file-transfer-svc-service-file-transfer-add-3` | wcf | FileTransferAdd | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:25` | - |
| `file-transfer-svc-service-controller-file-transfer-add` | api | FileTransferAdd | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:22` | POST /FileTransferSvcService |
| `file-transfer-svc-service-update-file-transfer-source-location` | wcf | UpdateFileTransferSourceLocation | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:31` | - |
| `file-transfer-svc-service-update-file-transfer-source-location-2` | wcf | UpdateFileTransferSourceLocation | HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:32` | - |
| `file-transfer-svc-service-update-file-transfer-source-location-3` | wcf | UpdateFileTransferSourceLocation | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:31` | - |
| `file-transfer-svc-service-controller-update-file-transfer-source-location` | api | UpdateFileTransferSourceLocation | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:28` | POST /FileTransferSvcService |
| `file-transfer-svc-service-invalidate-file-transfer-source-location` | wcf | InvalidateFileTransferSourceLocation | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:37` | - |
| `file-transfer-svc-service-invalidate-file-transfer-source-location-2` | wcf | InvalidateFileTransferSourceLocation | HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:38` | - |
| `file-transfer-svc-service-invalidate-file-transfer-source-location-3` | wcf | InvalidateFileTransferSourceLocation | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:37` | - |
| `file-transfer-svc-service-controller-invalidate-file-transfer-source-location` | api | InvalidateFileTransferSourceLocation | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:34` | POST /FileTransferSvcService |
| `file-transfer-svc-query-service-get-source-locations` | wcf | GetSourceLocations | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:40` | - |
| `file-transfer-svc-query-service-get-source-locations-2` | wcf | GetSourceLocations | HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:31` | - |
| `file-transfer-svc-query-service-get-source-locations-3` | wcf | GetSourceLocations | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:31` | - |
| `file-transfer-svc-query-service-controller-get-source-locations` | api | GetSourceLocations | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:39` | POST /FileTransferSvcQueryService |
| `file-transfer-controller-get-source-locations` | api | GetSourceLocations | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:24` | - |
| `file-transfer-svc-query-service-search-source-locations` | wcf | SearchSourceLocations | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:25` | - |
| `file-transfer-svc-query-service-search-source-locations-2` | wcf | SearchSourceLocations | HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:16` | - |
| `file-transfer-svc-query-service-search-source-locations-3` | wcf | SearchSourceLocations | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:16` | - |
| `file-transfer-svc-query-service-controller-search-source-locations` | api | SearchSourceLocations | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:21` | POST /FileTransferSvcQueryService |
