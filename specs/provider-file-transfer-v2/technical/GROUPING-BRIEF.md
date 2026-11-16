---
type: Generation Brief
title: 'Brief: group the doors of module provider-file-transfer-v2 into business flows'
description: Instructions the tool gave the writer session for grouping the doors into flows.
tags:
- grouping
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T06:38:05Z
cef:
  pack: provider-file-transfer-v2
  item: group
  generation:
    status: done
    attempts: 1
  body_sha256: cc7238d46fc8aee4649aa5682d4da95875305a9e639001d6adaf2f0450c67942
---

# Brief: group the doors of module `provider-file-transfer-v2` into business flows

- Playbook: `inputs/playbook/group.md`
- Source root (read-only): `C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\legacy-kg\components`. Paths below are relative to it.
- Write exactly: `technical/flows.yaml`
- 77 doors, in `technical/doors.yaml`; what each one reaches: `technical/doors/<door id>.yaml`
- Expect about 3 to 10 business flows; a grouping where most flows have one door is rejected.

In the tables, *Walk stops at* names the methods where the graph saw nothing further (often a router or a call into code it could not resolve): a door whose walk stops before any table still has data behind it, which the code shows.

## Doors, by class

### `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main.BatchProcess` (batch, `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main\BatchProcess.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `main-batch-process` | BatchProcess | 17 | - | - | - | - |

### `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService` (wcf, `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-svc-query-service-search-source-locations` | SearchSourceLocations | 25 | - | - | - | - |
| `file-transfer-svc-query-service-search-file-transfer-movement` | SearchFileTransferMovement | 30 | - | - | - | - |
| `file-transfer-svc-query-service-get-ftp-servers` | GetFtpServers | 35 | - | orm:FTP_SERVER | - | - |
| `file-transfer-svc-query-service-get-source-locations` | GetSourceLocations | 40 | - | - | - | - |
| `file-transfer-svc-query-service-search-inbound-target` | SearchInboundTarget | 45 | - | - | - | - |
| `file-transfer-svc-query-service-get-integration-file` | GetIntegrationFile | 50 | - | - | - | - |
| `file-transfer-svc-query-service-try-route-query` | TryRouteQuery | 55 | - | - | - | - |

### `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService` (wcf, `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-svc-service-file-transfer-add` | FileTransferAdd | 25 | - | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsCommand.AddValidation, FtsCommand.FtsCommandValidate, FtsCommand.HandleValidations, FtsSourceLocationAddCommand.BuildMaskValidationMessage |
| `file-transfer-svc-service-update-file-transfer-source-location` | UpdateFileTransferSourceLocation | 31 | - | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsCommand.AddValidation, FtsCommand.FtsCommandValidate, FtsCommand.HandleValidations, FtsSourceLocationUpdateCommand.BuildMaskValidationMessage, FtsSourceLocationUpdateCommand.ValidateConfigId, FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate |
| `file-transfer-svc-service-invalidate-file-transfer-source-location` | InvalidateFileTransferSourceLocation | 37 | - | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsSourceLocationInvalidateCommand.ValidateConfigId, FtsSourceLocationInvalidateCommand.ValidateOperatorId |
| `file-transfer-svc-service-add-ftp-server` | AddFtpServer | 43 | - | orm:FTP_SERVER | - | FileTransferCommand.HandleValidations |
| `file-transfer-svc-service-update-ftp-server` | UpdateFtpServer | 49 | - | orm:FTP_SERVER | - | FileTransferCommand.HandleValidations |
| `file-transfer-svc-service-update-integration-file` | UpdateIntegrationFile | 55 | - | orm:FTS_INTEGRATION_FILE | - | UpdateIntegrationFileCommand.ValidateConfigId |
| `file-transfer-svc-service-invalidate-ftp-server` | InvalidateFtpServer | 61 | - | orm:FTP_SERVER | - | - |
| `file-transfer-svc-service-try-route-command` | TryRouteCommand | 69 | - | - | - | - |

### `HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController` (api, `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-controller-search-file-transfer-tracking` | SearchFileTransferTracking | 24 | GET /FileTransfer | - | - | - |
| `file-transfer-controller-add-file-base-configuration` | AddFileBaseConfiguration | 62 | POST /FileTransfer | orm:FTS_SUSPENSION | - | - |
| `file-transfer-controller-file-transfer-configuration-view` | FileTransferConfigurationView | 88 | GET /FileTransfer | orm:FTS_SUSPENSION | - | - |
| `file-transfer-controller-destination-configuration-edit-panel` | DestinationConfigurationEditPanel | 103 | POST /FileTransfer | - | - | - |
| `file-transfer-controller-file-transfer-configuration-edit` | FileTransferConfigurationEdit | 113 | POST /FileTransfer | orm:FTS_SUSPENSION | - | - |
| `file-transfer-controller-destination-configuration-edit` | DestinationConfigurationEdit | 132 | POST /FileTransfer | - | - | - |

### `HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController` (api, `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-controller-add-file-transfer-base-configuration` | AddFileTransferBaseConfiguration | 21 | - | orm:FTS_SUSPENSION | - | - |
| `file-transfer-controller-get-source-locations` | GetSourceLocations | 24 | - | - | - | - |
| `file-transfer-controller-search-file-transfer-tracking-2` | SearchFileTransferTracking | 24 | - | - | - | - |
| `file-transfer-controller-get-ftpserver-credentials` | GetFTPServerCredentials | 26 | - | orm:FTP_SERVER | - | - |
| `file-transfer-controller-search-file-transfer` | SearchFileTransfer | 28 | - | - | - | - |
| `file-transfer-controller-add-ftpserver-credentials` | AddFTPServerCredentials | 43 | - | orm:FTP_SERVER | - | FileTransferController.FormatInvalidResponse |
| `file-transfer-controller-create-ftpserver-credentials` | CreateFTPServerCredentials | 52 | - | orm:FTP_SERVER | - | - |
| `file-transfer-controller-get-data-list-data` | GetDataListData | 63 | - | - | - | - |
| `file-transfer-controller-update-ftpserver-credentials` | UpdateFTPServerCredentials | 68 | - | orm:FTP_SERVER | - | FileTransferController.FormatInvalidResponse |
| `file-transfer-controller-get-data-list-data-2` | GetDataListData | 90 | - | - | - | - |
| `file-transfer-controller-get-ftp-server-details` | GetFtpServerDetails | 111 | - | orm:FTP_SERVER | - | - |
| `file-transfer-controller-get-ftemetadatas` | GetFTEMetadatas | 160 | - | - | - | - |

### `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess` (batch, `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main\BatchProcessProgram.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `main-batch-process-2` | BatchProcess | 21 | - | - | - | - |

### `HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService` (wcf, `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-svc-service-file-transfer-add-2` | FileTransferAdd | 26 | - | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsCommand.AddValidation, FtsCommand.FtsCommandValidate, FtsCommand.HandleValidations, FtsSourceLocationAddCommand.BuildMaskValidationMessage |
| `file-transfer-svc-service-update-file-transfer-source-location-2` | UpdateFileTransferSourceLocation | 32 | - | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsCommand.AddValidation, FtsCommand.FtsCommandValidate, FtsCommand.HandleValidations, FtsSourceLocationUpdateCommand.BuildMaskValidationMessage, FtsSourceLocationUpdateCommand.ValidateConfigId, FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate |
| `file-transfer-svc-service-invalidate-file-transfer-source-location-2` | InvalidateFileTransferSourceLocation | 38 | - | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsSourceLocationInvalidateCommand.ValidateConfigId, FtsSourceLocationInvalidateCommand.ValidateOperatorId |
| `file-transfer-svc-service-add-ftp-server-2` | AddFtpServer | 44 | - | orm:FTP_SERVER | - | FileTransferCommand.HandleValidations |
| `file-transfer-svc-service-update-ftp-server-2` | UpdateFtpServer | 50 | - | orm:FTP_SERVER | - | FileTransferCommand.HandleValidations |
| `file-transfer-svc-service-invalidate-ftp-server-2` | InvalidateFtpServer | 56 | - | orm:FTP_SERVER | - | - |
| `file-transfer-svc-service-try-route-command-2` | TryRouteCommand | 64 | - | - | - | - |

### `HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController` (api, `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-svc-query-service-controller-search-source-locations` | SearchSourceLocations | 21 | POST /FileTransferSvcQueryService | - | - | - |
| `file-transfer-svc-query-service-controller-search-file-transfer-movement` | SearchFileTransferMovement | 27 | POST /FileTransferSvcQueryService | - | - | - |
| `file-transfer-svc-query-service-controller-get-ftp-servers` | GetFtpServers | 33 | POST /FileTransferSvcQueryService | orm:FTP_SERVER | - | - |
| `file-transfer-svc-query-service-controller-get-source-locations` | GetSourceLocations | 39 | POST /FileTransferSvcQueryService | - | - | - |
| `file-transfer-svc-query-service-controller-search-inbound-target` | SearchInboundTarget | 45 | POST /FileTransferSvcQueryService | - | - | - |

### `HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController` (api, `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-svc-service-controller-file-transfer-add` | FileTransferAdd | 22 | POST /FileTransferSvcService | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsCommand.AddValidation, FtsCommand.FtsCommandValidate, FtsCommand.HandleValidations, FtsSourceLocationAddCommand.BuildMaskValidationMessage |
| `file-transfer-svc-service-controller-update-file-transfer-source-location` | UpdateFileTransferSourceLocation | 28 | POST /FileTransferSvcService | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsCommand.AddValidation, FtsCommand.FtsCommandValidate, FtsCommand.HandleValidations, FtsSourceLocationUpdateCommand.BuildMaskValidationMessage, FtsSourceLocationUpdateCommand.ValidateConfigId, FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate |
| `file-transfer-svc-service-controller-invalidate-file-transfer-source-location` | InvalidateFileTransferSourceLocation | 34 | POST /FileTransferSvcService | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsSourceLocationInvalidateCommand.ValidateConfigId, FtsSourceLocationInvalidateCommand.ValidateOperatorId |
| `file-transfer-svc-service-controller-add-ftp-server` | AddFtpServer | 40 | POST /FileTransferSvcService | orm:FTP_SERVER | - | FileTransferCommand.HandleValidations |
| `file-transfer-svc-service-controller-update-ftp-server` | UpdateFtpServer | 46 | POST /FileTransferSvcService | orm:FTP_SERVER | - | FileTransferCommand.HandleValidations |
| `file-transfer-svc-service-controller-invalidate-ftp-server` | InvalidateFtpServer | 52 | POST /FileTransferSvcService | orm:FTP_SERVER | - | - |

### `HPP.FileTransfer.BAS.FileTransferApi.Program` (batch, `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Program.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-api-program` | Program | 14 | - | - | - | Program.CreateHostBuilder |

### `HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService` (wcf, `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-svc-query-service-search-source-locations-2` | SearchSourceLocations | 16 | - | - | - | - |
| `file-transfer-svc-query-service-search-file-transfer-movement-2` | SearchFileTransferMovement | 21 | - | - | - | - |
| `file-transfer-svc-query-service-get-ftp-servers-2` | GetFtpServers | 26 | - | orm:FTP_SERVER | - | - |
| `file-transfer-svc-query-service-get-source-locations-2` | GetSourceLocations | 31 | - | - | - | - |
| `file-transfer-svc-query-service-search-inbound-target-2` | SearchInboundTarget | 36 | - | - | - | - |
| `file-transfer-svc-query-service-try-route-query-2` | TryRouteQuery | 41 | - | - | - | - |

### `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService` (wcf, `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-svc-query-service-search-source-locations-3` | SearchSourceLocations | 16 | - | - | - | - |
| `file-transfer-svc-query-service-search-file-transfer-movement-3` | SearchFileTransferMovement | 21 | - | - | - | - |
| `file-transfer-svc-query-service-get-ftp-servers-3` | GetFtpServers | 26 | - | orm:FTP_SERVER | - | - |
| `file-transfer-svc-query-service-get-source-locations-3` | GetSourceLocations | 31 | - | - | - | - |
| `file-transfer-svc-query-service-search-inbound-target-3` | SearchInboundTarget | 36 | - | - | - | - |
| `file-transfer-svc-query-service-get-integration-file-2` | GetIntegrationFile | 41 | - | - | - | - |
| `file-transfer-svc-query-service-try-route-query-3` | TryRouteQuery | 46 | - | - | - | - |

### `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService` (wcf, `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-svc-service-file-transfer-add-3` | FileTransferAdd | 25 | - | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsCommand.AddValidation, FtsCommand.FtsCommandValidate, FtsCommand.HandleValidations, FtsSourceLocationAddCommand.BuildMaskValidationMessage |
| `file-transfer-svc-service-update-file-transfer-source-location-3` | UpdateFileTransferSourceLocation | 31 | - | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsCommand.AddValidation, FtsCommand.FtsCommandValidate, FtsCommand.HandleValidations, FtsSourceLocationUpdateCommand.BuildMaskValidationMessage, FtsSourceLocationUpdateCommand.ValidateConfigId, FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate |
| `file-transfer-svc-service-invalidate-file-transfer-source-location-3` | InvalidateFileTransferSourceLocation | 37 | - | orm:FTP_SERVER, orm:FTS_DESTINATION, orm:FTS_SUSPENSION | - | FtsSourceLocationInvalidateCommand.ValidateConfigId, FtsSourceLocationInvalidateCommand.ValidateOperatorId |
| `file-transfer-svc-service-add-ftp-server-3` | AddFtpServer | 43 | - | orm:FTP_SERVER | - | FileTransferCommand.HandleValidations |
| `file-transfer-svc-service-update-ftp-server-3` | UpdateFtpServer | 49 | - | orm:FTP_SERVER | - | FileTransferCommand.HandleValidations |
| `file-transfer-svc-service-update-integration-file-2` | UpdateIntegrationFile | 55 | - | orm:FTS_INTEGRATION_FILE | - | UpdateIntegrationFileCommand.ValidateConfigId |
| `file-transfer-svc-service-invalidate-ftp-server-3` | InvalidateFtpServer | 61 | - | orm:FTP_SERVER | - | - |
| `file-transfer-svc-service-try-route-command-3` | TryRouteCommand | 69 | - | - | - | - |

### `HPP.FileTransfer.BAS.FileTransferSvc.Main.Program` (batch, `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Main\Program.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `main-program` | Program | 18 | - | - | - | Program.InitializeInstrumentationSettings, ServiceEventSource.ServiceHostInitializationFailed, ServiceEventSource.ServiceTypeRegistered |

### `HPP.FileTransfer.BAS.FileTransferSvc.Program` (batch, `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\Program.cs`)

| Door id | Name | Line | Endpoints | Tables | Procedures | Walk stops at |
| --- | --- | ---: | --- | --- | --- | --- |
| `file-transfer-svc-program` | Program | 13 | - | - | - | Program.CreateHostBuilder |

## The same operation name in several classes

Often one operation in two generations of the code, or a service and the controller in front of it: usually one flow.

- AddFtpServer: `file-transfer-svc-service-add-ftp-server` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService), `file-transfer-svc-service-add-ftp-server-2` (HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService), `file-transfer-svc-service-controller-add-ftp-server` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController), `file-transfer-svc-service-add-ftp-server-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService)
- BatchProcess: `main-batch-process` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main.BatchProcess), `main-batch-process-2` (HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess)
- FileTransferAdd: `file-transfer-svc-service-file-transfer-add` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService), `file-transfer-svc-service-file-transfer-add-2` (HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService), `file-transfer-svc-service-controller-file-transfer-add` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController), `file-transfer-svc-service-file-transfer-add-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService)
- GetFtpServers: `file-transfer-svc-query-service-get-ftp-servers` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService), `file-transfer-svc-query-service-controller-get-ftp-servers` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController), `file-transfer-svc-query-service-get-ftp-servers-2` (HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService), `file-transfer-svc-query-service-get-ftp-servers-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService)
- GetIntegrationFile: `file-transfer-svc-query-service-get-integration-file` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService), `file-transfer-svc-query-service-get-integration-file-2` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService)
- GetSourceLocations: `file-transfer-svc-query-service-get-source-locations` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService), `file-transfer-controller-get-source-locations` (HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController), `file-transfer-svc-query-service-controller-get-source-locations` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController), `file-transfer-svc-query-service-get-source-locations-2` (HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService), `file-transfer-svc-query-service-get-source-locations-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService)
- InvalidateFileTransferSourceLocation: `file-transfer-svc-service-invalidate-file-transfer-source-location` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService), `file-transfer-svc-service-invalidate-file-transfer-source-location-2` (HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService), `file-transfer-svc-service-controller-invalidate-file-transfer-source-location` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController), `file-transfer-svc-service-invalidate-file-transfer-source-location-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService)
- InvalidateFtpServer: `file-transfer-svc-service-invalidate-ftp-server` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService), `file-transfer-svc-service-invalidate-ftp-server-2` (HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService), `file-transfer-svc-service-controller-invalidate-ftp-server` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController), `file-transfer-svc-service-invalidate-ftp-server-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService)
- Program: `file-transfer-api-program` (HPP.FileTransfer.BAS.FileTransferApi.Program), `main-program` (HPP.FileTransfer.BAS.FileTransferSvc.Main.Program), `file-transfer-svc-program` (HPP.FileTransfer.BAS.FileTransferSvc.Program)
- SearchFileTransferMovement: `file-transfer-svc-query-service-search-file-transfer-movement` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService), `file-transfer-svc-query-service-controller-search-file-transfer-movement` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController), `file-transfer-svc-query-service-search-file-transfer-movement-2` (HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService), `file-transfer-svc-query-service-search-file-transfer-movement-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService)
- SearchFileTransferTracking: `file-transfer-controller-search-file-transfer-tracking` (HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController), `file-transfer-controller-search-file-transfer-tracking-2` (HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController)
- SearchInboundTarget: `file-transfer-svc-query-service-search-inbound-target` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService), `file-transfer-svc-query-service-controller-search-inbound-target` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController), `file-transfer-svc-query-service-search-inbound-target-2` (HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService), `file-transfer-svc-query-service-search-inbound-target-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService)
- SearchSourceLocations: `file-transfer-svc-query-service-search-source-locations` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService), `file-transfer-svc-query-service-controller-search-source-locations` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController), `file-transfer-svc-query-service-search-source-locations-2` (HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService), `file-transfer-svc-query-service-search-source-locations-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService)
- TryRouteCommand: `file-transfer-svc-service-try-route-command` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService), `file-transfer-svc-service-try-route-command-2` (HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService), `file-transfer-svc-service-try-route-command-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService)
- TryRouteQuery: `file-transfer-svc-query-service-try-route-query` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService), `file-transfer-svc-query-service-try-route-query-2` (HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService), `file-transfer-svc-query-service-try-route-query-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService)
- UpdateFileTransferSourceLocation: `file-transfer-svc-service-update-file-transfer-source-location` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService), `file-transfer-svc-service-update-file-transfer-source-location-2` (HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService), `file-transfer-svc-service-controller-update-file-transfer-source-location` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController), `file-transfer-svc-service-update-file-transfer-source-location-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService)
- UpdateFtpServer: `file-transfer-svc-service-update-ftp-server` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService), `file-transfer-svc-service-update-ftp-server-2` (HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService), `file-transfer-svc-service-controller-update-ftp-server` (HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcServiceController), `file-transfer-svc-service-update-ftp-server-3` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService)
- UpdateIntegrationFile: `file-transfer-svc-service-update-integration-file` (HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService), `file-transfer-svc-service-update-integration-file-2` (HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService)

## The tool's proposal (doors grouped only by shared tables)

- `file-transfer-api-program`: `file-transfer-api-program`
- `file-transfer-controller-1`: `file-transfer-controller-destination-configuration-edit`
- `file-transfer-controller-2`: `file-transfer-controller-destination-configuration-edit-panel`
- `file-transfer-controller-3`: `file-transfer-controller-get-data-list-data`
- `file-transfer-controller-4`: `file-transfer-controller-get-data-list-data-2`
- `file-transfer-controller-5`: `file-transfer-controller-get-ftemetadatas`
- `file-transfer-controller-6`: `file-transfer-controller-get-source-locations`
- `file-transfer-controller-7`: `file-transfer-controller-search-file-transfer`
- `file-transfer-controller-8`: `file-transfer-controller-search-file-transfer-tracking`
- `file-transfer-controller-9`: `file-transfer-controller-search-file-transfer-tracking-2`
- `file-transfer-svc-program`: `file-transfer-svc-program`
- `file-transfer-svc-query-service-1`: `file-transfer-svc-query-service-get-ftp-servers`, `file-transfer-svc-service-add-ftp-server`, `file-transfer-svc-service-file-transfer-add`, `file-transfer-svc-service-invalidate-file-transfer-source-location`, `file-transfer-svc-service-invalidate-ftp-server`, `file-transfer-svc-service-update-file-transfer-source-location`, `file-transfer-svc-service-update-ftp-server`, `file-transfer-controller-add-file-base-configuration`, `file-transfer-controller-file-transfer-configuration-edit`, `file-transfer-controller-file-transfer-configuration-view`, `file-transfer-controller-add-ftpserver-credentials`, `file-transfer-controller-add-file-transfer-base-configuration`, `file-transfer-controller-create-ftpserver-credentials`, `file-transfer-controller-get-ftpserver-credentials`, `file-transfer-controller-get-ftp-server-details`, `file-transfer-controller-update-ftpserver-credentials`, `file-transfer-svc-service-add-ftp-server-2`, `file-transfer-svc-service-file-transfer-add-2`, `file-transfer-svc-service-invalidate-file-transfer-source-location-2`, `file-transfer-svc-service-invalidate-ftp-server-2`, `file-transfer-svc-service-update-file-transfer-source-location-2`, `file-transfer-svc-service-update-ftp-server-2`, `file-transfer-svc-query-service-controller-get-ftp-servers`, `file-transfer-svc-service-controller-add-ftp-server`, `file-transfer-svc-service-controller-file-transfer-add`, `file-transfer-svc-service-controller-invalidate-file-transfer-source-location`, `file-transfer-svc-service-controller-invalidate-ftp-server`, `file-transfer-svc-service-controller-update-file-transfer-source-location`, `file-transfer-svc-service-controller-update-ftp-server`, `file-transfer-svc-query-service-get-ftp-servers-2`, `file-transfer-svc-query-service-get-ftp-servers-3`, `file-transfer-svc-service-add-ftp-server-3`, `file-transfer-svc-service-file-transfer-add-3`, `file-transfer-svc-service-invalidate-file-transfer-source-location-3`, `file-transfer-svc-service-invalidate-ftp-server-3`, `file-transfer-svc-service-update-file-transfer-source-location-3`, `file-transfer-svc-service-update-ftp-server-3`
- `file-transfer-svc-query-service-10`: `file-transfer-svc-query-service-search-inbound-target`
- `file-transfer-svc-query-service-11`: `file-transfer-svc-query-service-search-inbound-target-2`
- `file-transfer-svc-query-service-12`: `file-transfer-svc-query-service-search-inbound-target-3`
- `file-transfer-svc-query-service-13`: `file-transfer-svc-query-service-search-source-locations`
- `file-transfer-svc-query-service-14`: `file-transfer-svc-query-service-search-source-locations-2`
- `file-transfer-svc-query-service-15`: `file-transfer-svc-query-service-search-source-locations-3`
- `file-transfer-svc-query-service-16`: `file-transfer-svc-query-service-try-route-query`
- `file-transfer-svc-query-service-17`: `file-transfer-svc-query-service-try-route-query-2`
- `file-transfer-svc-query-service-18`: `file-transfer-svc-query-service-try-route-query-3`
- `file-transfer-svc-query-service-2`: `file-transfer-svc-query-service-get-integration-file`
- `file-transfer-svc-query-service-3`: `file-transfer-svc-query-service-get-integration-file-2`
- `file-transfer-svc-query-service-4`: `file-transfer-svc-query-service-get-source-locations`
- `file-transfer-svc-query-service-5`: `file-transfer-svc-query-service-get-source-locations-2`
- `file-transfer-svc-query-service-6`: `file-transfer-svc-query-service-get-source-locations-3`
- `file-transfer-svc-query-service-7`: `file-transfer-svc-query-service-search-file-transfer-movement`
- `file-transfer-svc-query-service-8`: `file-transfer-svc-query-service-search-file-transfer-movement-2`
- `file-transfer-svc-query-service-9`: `file-transfer-svc-query-service-search-file-transfer-movement-3`
- `file-transfer-svc-query-service-controller-1`: `file-transfer-svc-query-service-controller-get-source-locations`
- `file-transfer-svc-query-service-controller-2`: `file-transfer-svc-query-service-controller-search-file-transfer-movement`
- `file-transfer-svc-query-service-controller-3`: `file-transfer-svc-query-service-controller-search-inbound-target`
- `file-transfer-svc-query-service-controller-4`: `file-transfer-svc-query-service-controller-search-source-locations`
- `file-transfer-svc-service-1`: `file-transfer-svc-service-try-route-command`
- `file-transfer-svc-service-2`: `file-transfer-svc-service-try-route-command-2`
- `file-transfer-svc-service-3`: `file-transfer-svc-service-try-route-command-3`
- `file-transfer-svc-service-4`: `file-transfer-svc-service-update-integration-file`, `file-transfer-svc-service-update-integration-file-2`
- `main-batch-process`: `main-batch-process`
- `main-batch-process-2`: `main-batch-process-2`
- `main-program`: `main-program`
