---
type: Generation Brief
title: 'Brief: split flow search-and-track-file-transfers into parts'
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
  at: 2026-11-15T09:12:32Z
sources:
- id: door-file-transfer-controller-search-file-transfer-tracking
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.Tracking.cs#L24
  title: SearchFileTransferTracking (api door)
- id: door-file-transfer-controller-search-file-transfer-tracking-2
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.Tracking.cs#L24
  title: SearchFileTransferTracking (api door)
- id: door-file-transfer-controller-search-file-transfer
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L28
  title: SearchFileTransfer (api door)
- id: door-file-transfer-svc-query-service-search-file-transfer-movement
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L30
  title: SearchFileTransferMovement (wcf door)
- id: door-file-transfer-svc-query-service-search-file-transfer-movement-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L21
  title: SearchFileTransferMovement (wcf door)
- id: door-file-transfer-svc-query-service-search-file-transfer-movement-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L21
  title: SearchFileTransferMovement (wcf door)
- id: door-file-transfer-svc-query-service-controller-search-file-transfer-movement
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L27
  title: SearchFileTransferMovement (api door)
- id: door-file-transfer-svc-query-service-search-inbound-target
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L45
  title: SearchInboundTarget (wcf door)
- id: door-file-transfer-svc-query-service-search-inbound-target-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L36
  title: SearchInboundTarget (wcf door)
- id: door-file-transfer-svc-query-service-search-inbound-target-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L36
  title: SearchInboundTarget (wcf door)
- id: door-file-transfer-svc-query-service-controller-search-inbound-target
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L45
  title: SearchInboundTarget (api door)
- id: door-file-transfer-controller-get-ftemetadatas
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L160
  title: GetFTEMetadatas (api door)
- id: door-file-transfer-controller-get-data-list-data
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L63
  title: GetDataListData (api door)
- id: door-file-transfer-controller-get-data-list-data-2
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L90
  title: GetDataListData (api door)
- id: footprint
  resource: /technical/flows/search-and-track-file-transfers/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:search-and-track-file-transfers
  generation:
    status: done
    attempts: 1
  rule_prefix: F05
  rules: 36
  doors:
  - file-transfer-controller-search-file-transfer-tracking
  - file-transfer-controller-search-file-transfer-tracking-2
  - file-transfer-controller-search-file-transfer
  - file-transfer-svc-query-service-search-file-transfer-movement
  - file-transfer-svc-query-service-search-file-transfer-movement-2
  - file-transfer-svc-query-service-search-file-transfer-movement-3
  - file-transfer-svc-query-service-controller-search-file-transfer-movement
  - file-transfer-svc-query-service-search-inbound-target
  - file-transfer-svc-query-service-search-inbound-target-2
  - file-transfer-svc-query-service-search-inbound-target-3
  - file-transfer-svc-query-service-controller-search-inbound-target
  - file-transfer-controller-get-ftemetadatas
  - file-transfer-controller-get-data-list-data
  - file-transfer-controller-get-data-list-data-2
  tables: 0
  procedures: 0
  components:
  - backend-configure-source-locations-and-destinations-api
  - ui-configure-source-locations-and-destinations-screens
  - ui-manage-ftp-servers-and-credentials-screens
  body_sha256: d12affac69fb9e8dc812b8a150d686e010db9dcbad78c5310470e0e1a1637bd3
---

# Brief: split flow `search-and-track-file-transfers` into parts

- Business name: **Search and track file transfers**
- 14 doors; a part holds at most 12, so at least 2 parts
- Write exactly: `technical/flows/search-and-track-file-transfers/PARTS.yaml`
- Playbook: `inputs/playbook/parts.md`
- What each door reaches: `technical/doors/<door id>.yaml`; the flow's map: `technical/flows/search-and-track-file-transfers/TECH.md`

## Doors

| Door id | Kind | Name | Class | Where | Endpoints |
| --- | --- | --- | --- | --- | --- |
| `file-transfer-controller-search-file-transfer-tracking` | api | SearchFileTransferTracking | HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Presentation\Controllers\FileTransferController.Tracking.cs:24` | GET /FileTransfer |
| `file-transfer-controller-search-file-transfer-tracking-2` | api | SearchFileTransferTracking | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.Tracking.cs:24` | - |
| `file-transfer-controller-search-file-transfer` | api | SearchFileTransfer | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:28` | - |
| `file-transfer-svc-query-service-search-file-transfer-movement` | wcf | SearchFileTransferMovement | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:30` | - |
| `file-transfer-svc-query-service-search-file-transfer-movement-2` | wcf | SearchFileTransferMovement | HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:21` | - |
| `file-transfer-svc-query-service-search-file-transfer-movement-3` | wcf | SearchFileTransferMovement | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:21` | - |
| `file-transfer-svc-query-service-controller-search-file-transfer-movement` | api | SearchFileTransferMovement | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:27` | POST /FileTransferSvcQueryService |
| `file-transfer-svc-query-service-search-inbound-target` | wcf | SearchInboundTarget | HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc\FileTransferSvcQueryService.svc.cs:45` | - |
| `file-transfer-svc-query-service-search-inbound-target-2` | wcf | SearchInboundTarget | HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Queries\FileTransferSvcQueryService.cs:36` | - |
| `file-transfer-svc-query-service-search-inbound-target-3` | wcf | SearchInboundTarget | HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvcCore\FileTransferSvcQueryService.cs:36` | - |
| `file-transfer-svc-query-service-controller-search-inbound-target` | api | SearchInboundTarget | HPP.FileTransfer.BAS.FileTransferApi.Controllers.FileTransferSvcQueryServiceController | `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcQueryServiceController.cs:45` | POST /FileTransferSvcQueryService |
| `file-transfer-controller-get-ftemetadatas` | api | GetFTEMetadatas | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:160` | - |
| `file-transfer-controller-get-data-list-data` | api | GetDataListData | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:63` | - |
| `file-transfer-controller-get-data-list-data-2` | api | GetDataListData | HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController | `ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.SearchFileTransfer.cs:90` | - |
