---
type: Flow Review List
title: 'provider-file-transfer-v2: proposed flows'
description: 77 doors grouped into 40 proposed flows.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T06:21:10Z
cef:
  pack: provider-file-transfer-v2
  body_sha256: 333f74c194c53d095180fb9d533f086c5052fb26065d262d6c5680297ab2bff5
---

# provider-file-transfer-v2: proposed flows

77 doors grouped into 40 proposed flows. Doors are grouped when their own procedures and tables overlap by at least 30%. The tool cannot see which API a screen's JavaScript calls, so screens usually need merging by hand.

For each flow, tick one decision and give it a business name.

| Flow | Doors | Own procedures | Own tables | Shared pieces used |
| --- | --- | ---: | ---: | ---: |
| `file-transfer-api-program` | Program (batch) | 0 | 0 | 0 |
| `file-transfer-controller-1` | DestinationConfigurationEdit (api) | 0 | 0 | 0 |
| `file-transfer-controller-2` | DestinationConfigurationEditPanel (api) | 0 | 0 | 0 |
| `file-transfer-controller-3` | GetDataListData (api) | 0 | 0 | 0 |
| `file-transfer-controller-4` | GetDataListData (api) | 0 | 0 | 0 |
| `file-transfer-controller-5` | GetFTEMetadatas (api) | 0 | 0 | 0 |
| `file-transfer-controller-6` | GetSourceLocations (api) | 0 | 0 | 0 |
| `file-transfer-controller-7` | SearchFileTransfer (api) | 0 | 0 | 0 |
| `file-transfer-controller-8` | SearchFileTransferTracking (api) | 0 | 0 | 0 |
| `file-transfer-controller-9` | SearchFileTransferTracking (api) | 0 | 0 | 0 |
| `file-transfer-svc-program` | Program (batch) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-controller-1` | GetSourceLocations (api) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-controller-2` | SearchFileTransferMovement (api) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-controller-3` | SearchInboundTarget (api) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-controller-4` | SearchSourceLocations (api) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-1` | GetFtpServers (wcf), AddFtpServer (wcf), FileTransferAdd (wcf), InvalidateFileTransferSourceLocation (wcf), InvalidateFtpServer (wcf), UpdateFileTransferSourceLocation (wcf), UpdateFtpServer (wcf), AddFileBaseConfiguration (api), FileTransferConfigurationEdit (api), FileTransferConfigurationView (api), AddFTPServerCredentials (api), AddFileTransferBaseConfiguration (api), CreateFTPServerCredentials (api), GetFTPServerCredentials (api), GetFtpServerDetails (api), UpdateFTPServerCredentials (api), AddFtpServer (wcf), FileTransferAdd (wcf), InvalidateFileTransferSourceLocation (wcf), InvalidateFtpServer (wcf), UpdateFileTransferSourceLocation (wcf), UpdateFtpServer (wcf), GetFtpServers (api), AddFtpServer (api), FileTransferAdd (api), InvalidateFileTransferSourceLocation (api), InvalidateFtpServer (api), UpdateFileTransferSourceLocation (api), UpdateFtpServer (api), GetFtpServers (wcf), GetFtpServers (wcf), AddFtpServer (wcf), FileTransferAdd (wcf), InvalidateFileTransferSourceLocation (wcf), InvalidateFtpServer (wcf), UpdateFileTransferSourceLocation (wcf), UpdateFtpServer (wcf) | 0 | 3 | 0 |
| `file-transfer-svc-query-service-2` | GetIntegrationFile (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-3` | GetIntegrationFile (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-4` | GetSourceLocations (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-5` | GetSourceLocations (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-6` | GetSourceLocations (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-7` | SearchFileTransferMovement (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-8` | SearchFileTransferMovement (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-9` | SearchFileTransferMovement (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-10` | SearchInboundTarget (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-11` | SearchInboundTarget (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-12` | SearchInboundTarget (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-13` | SearchSourceLocations (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-14` | SearchSourceLocations (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-15` | SearchSourceLocations (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-16` | TryRouteQuery (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-17` | TryRouteQuery (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-query-service-18` | TryRouteQuery (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-service-1` | TryRouteCommand (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-service-2` | TryRouteCommand (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-service-3` | TryRouteCommand (wcf) | 0 | 0 | 0 |
| `file-transfer-svc-service-4` | UpdateIntegrationFile (wcf), UpdateIntegrationFile (wcf) | 0 | 1 | 0 |
| `main-batch-process` | BatchProcess (batch) | 0 | 0 | 0 |
| `main-batch-process-2` | BatchProcess (batch) | 0 | 0 | 0 |
| `main-program` | Program (batch) | 0 | 0 | 0 |

38 of 40 flows reach no table or procedure in the graph: their walk stops early (see each flow's `stops`), so they cannot be grouped by data and stay one door each.

## `file-transfer-api-program`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- batch `Program`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Program.cs:14`

Walk stops at: `CreateHostBuilder`

## `file-transfer-controller-1`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `DestinationConfigurationEdit`: `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:132`

## `file-transfer-controller-2`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `DestinationConfigurationEditPanel`: `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:103`

## `file-transfer-controller-3`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `GetDataListData`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:63`

## `file-transfer-controller-4`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `GetDataListData`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:90`

## `file-transfer-controller-5`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `GetFTEMetadatas`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:160`

## `file-transfer-controller-6`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `GetSourceLocations`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SourceLocation.cs:24`

## `file-transfer-controller-7`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `SearchFileTransfer`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:28`

## `file-transfer-controller-8`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `SearchFileTransferTracking`: `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:24`

## `file-transfer-controller-9`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `SearchFileTransferTracking`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:24`

## `file-transfer-svc-program`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- batch `Program`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\Program.cs:13`

Walk stops at: `CreateHostBuilder`

## `file-transfer-svc-query-service-controller-1`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `GetSourceLocations`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:39`

## `file-transfer-svc-query-service-controller-2`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `SearchFileTransferMovement`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:27`

## `file-transfer-svc-query-service-controller-3`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `SearchInboundTarget`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:45`

## `file-transfer-svc-query-service-controller-4`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- api `SearchSourceLocations`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:21`

## `file-transfer-svc-query-service-1`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `GetFtpServers`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:35`
- wcf `AddFtpServer`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:43`
- wcf `FileTransferAdd`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:25`
- wcf `InvalidateFileTransferSourceLocation`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:37`
- wcf `InvalidateFtpServer`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:61`
- wcf `UpdateFileTransferSourceLocation`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:31`
- wcf `UpdateFtpServer`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:49`
- api `AddFileBaseConfiguration`: `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:62`
- api `FileTransferConfigurationEdit`: `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:113`
- api `FileTransferConfigurationView`: `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.cs:88`
- api `AddFTPServerCredentials`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:43`
- api `AddFileTransferBaseConfiguration`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.AddFileTransferBaseConfiguration.cs:21`
- api `CreateFTPServerCredentials`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:52`
- api `GetFTPServerCredentials`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:26`
- api `GetFtpServerDetails`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:111`
- api `UpdateFTPServerCredentials`: `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:68`
- wcf `AddFtpServer`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:44`
- wcf `FileTransferAdd`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:26`
- wcf `InvalidateFileTransferSourceLocation`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:38`
- wcf `InvalidateFtpServer`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:56`
- wcf `UpdateFileTransferSourceLocation`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:32`
- wcf `UpdateFtpServer`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:50`
- api `GetFtpServers`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:33`
- api `AddFtpServer`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:40`
- api `FileTransferAdd`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:22`
- api `InvalidateFileTransferSourceLocation`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:34`
- api `InvalidateFtpServer`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:52`
- api `UpdateFileTransferSourceLocation`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:28`
- api `UpdateFtpServer`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcServiceController.cs:46`
- wcf `GetFtpServers`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:26`
- wcf `GetFtpServers`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:26`
- wcf `AddFtpServer`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:43`
- wcf `FileTransferAdd`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:25`
- wcf `InvalidateFileTransferSourceLocation`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:37`
- wcf `InvalidateFtpServer`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:61`
- wcf `UpdateFileTransferSourceLocation`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:31`
- wcf `UpdateFtpServer`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:49`

Tables: `orm:FTP_SERVER` (reads/writes), `orm:FTS_DESTINATION` (reads/writes), `orm:FTS_SUSPENSION` (reads/writes)

Walk stops at: `HandleValidations`, `AddValidation`, `FtsCommandValidate`, `HandleValidations`, `BuildMaskValidationMessage`, `ValidateConfigId`, `ValidateOperatorId`, `BuildMaskValidationMessage`, `ValidateConfigId`, `ValidateFtpSuspendedDate`, `FormatInvalidResponse`

## `file-transfer-svc-query-service-2`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `GetIntegrationFile`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:50`

## `file-transfer-svc-query-service-3`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `GetIntegrationFile`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:41`

## `file-transfer-svc-query-service-4`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `GetSourceLocations`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:40`

## `file-transfer-svc-query-service-5`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `GetSourceLocations`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:31`

## `file-transfer-svc-query-service-6`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `GetSourceLocations`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:31`

## `file-transfer-svc-query-service-7`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `SearchFileTransferMovement`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:30`

## `file-transfer-svc-query-service-8`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `SearchFileTransferMovement`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:21`

## `file-transfer-svc-query-service-9`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `SearchFileTransferMovement`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:21`

## `file-transfer-svc-query-service-10`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `SearchInboundTarget`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:45`

## `file-transfer-svc-query-service-11`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `SearchInboundTarget`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:36`

## `file-transfer-svc-query-service-12`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `SearchInboundTarget`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:36`

## `file-transfer-svc-query-service-13`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `SearchSourceLocations`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:25`

## `file-transfer-svc-query-service-14`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `SearchSourceLocations`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:16`

## `file-transfer-svc-query-service-15`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `SearchSourceLocations`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:16`

## `file-transfer-svc-query-service-16`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `TryRouteQuery`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:55`

## `file-transfer-svc-query-service-17`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `TryRouteQuery`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:41`

## `file-transfer-svc-query-service-18`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `TryRouteQuery`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:46`

## `file-transfer-svc-service-1`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `TryRouteCommand`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:69`

## `file-transfer-svc-service-2`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `TryRouteCommand`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Commands\FileTransferSvcService.cs:64`

## `file-transfer-svc-service-3`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `TryRouteCommand`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:69`

## `file-transfer-svc-service-4`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- wcf `UpdateIntegrationFile`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcService.svc.cs:55`
- wcf `UpdateIntegrationFile`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcService.cs:55`

Tables: `orm:FTS_INTEGRATION_FILE` (reads)

Walk stops at: `ValidateConfigId`

## `main-batch-process`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- batch `BatchProcess`: `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main\BatchProcess.cs:17`

## `main-batch-process-2`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- batch `BatchProcess`: `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main\BatchProcessProgram.cs:21`

## `main-program`

Business name: ____________________

- [ ] Approve  - [ ] Merge with: ________  - [ ] Split  - [ ] Not a flow

Doors:

- batch `Program`: `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Main\Program.cs:18`

Walk stops at: `InitializeInstrumentationSettings`, `ServiceHostInitializationFailed`, `ServiceTypeRegistered`
