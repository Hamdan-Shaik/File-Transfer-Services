---
type: Flow Specification
title: Search and track file transfers
description: Staff search for file transfers and review what happened to a transfer over time.
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
  body_sha256: a5eddbb5ac6e67439566d8b371584c2648fe9c18cb272879d93aa8c5b94c3489
---

# Search and track file transfers

Staff search for file transfers and review what happened to a transfer over time.
Other systems can request file transfer movement details and inbound target details to support reporting and troubleshooting.

## Who starts it

- Staff using the file transfer screens.
- Another system requesting file transfer movement and inbound targets.

## Doors

| Door | Request | Answers |
| --- | --- | --- |
| `file-transfer-controller-search-file-transfer-tracking` (GET /FileTransfer) | Open the file transfer tracking search screen. | A tracking search screen with empty criteria. |
| `file-transfer-controller-search-file-transfer-tracking-2` | Search for tracking results with criteria, paging, and ordering. | A result list and total count, or a validation/error result. |
| `file-transfer-controller-search-file-transfer` | Search for saved file transfer configurations with criteria, paging, and ordering. | A result list and total count, or a validation/error result. |
| `file-transfer-svc-query-service-search-file-transfer-movement` | Search for file transfer movement details. | The routed movement search query with results. |
| `file-transfer-svc-query-service-search-file-transfer-movement-2` | Search for file transfer movement details. | The routed movement search query with results. |
| `file-transfer-svc-query-service-search-file-transfer-movement-3` | Search for file transfer movement details. | The routed movement search query with results. |
| `file-transfer-svc-query-service-controller-search-file-transfer-movement` (POST /FileTransferSvcQueryService) | Search for file transfer movement details. | The movement search query with results. |
| `file-transfer-svc-query-service-search-inbound-target` | Search for inbound target details. | The routed inbound target query with results. |
| `file-transfer-svc-query-service-search-inbound-target-2` | Search for inbound target details. | The routed inbound target query with results. |
| `file-transfer-svc-query-service-search-inbound-target-3` | Search for inbound target details. | The routed inbound target query with results. |
| `file-transfer-svc-query-service-controller-search-inbound-target` (POST /FileTransferSvcQueryService) | Search for inbound target details. | The inbound target query with results. |
| `file-transfer-controller-get-ftemetadatas` | Look up inbound metadata IDs for a contract name. | A list of matching inbound metadata IDs, or an empty list when not found. |
| `file-transfer-controller-get-data-list-data` | Look up pick-list options for a screen field. | A list of matching pick-list options, or a validation/error result. |
| `file-transfer-controller-get-data-list-data-2` | Select a specific pick-list type to populate (modules only). | A list of active modules, or an empty list for unsupported types. |

## Steps

1. Staff open the tracking search screen and receive an editor set up for a single-step search, with a cancel action back to My Home and a localized “Search” label. (F05-001, F05-002, F05-003)
2. Staff submit a tracking search from their signed-in session; the search criteria is built from the requested filters/paging and sent to the query provider, applying only supported filter fields and ignoring date/size values that cannot be parsed. (F05-004, F05-005, F05-035)
3. When the provider returns an OK result, staff receive a result list and total count; missing paging information defaults the total to 0, and missing data returns an empty list with total 0. (F05-006, F05-007)
4. When the provider does not return an OK result, staff receive the provider result as an error response. (F05-008)
5. Staff submit a file transfer configuration search from their signed-in session; the provider builds a query only when paging details are present, and when the provider returns OK with a non-null data list staff receive the list and total count, otherwise they receive an error response. (F05-009, F05-010, F05-036, F05-011)
6. Staff request pick-list options; when the internal processor result is valid they receive the options list, otherwise they receive an error response. (F05-012, F05-013, F05-014)
7. When the pick-list type is “MODULE”, the system returns active modules ordered by module name (A to Z). (F05-015, F05-016)
8. Staff request inbound metadata IDs for a contract name; when the provider response includes a data list (even empty) they receive it as the response body with OK, and the response does not check the provider status code; otherwise they receive an empty list with a not-found response. (F05-017, F05-018)
9. Another system requests file transfer movement details; the request is routed and the routed query (with results) is returned. (F05-019, F05-020)
10. Movement results apply optional filtering and sorting, are built only from valid source locations, and each movement result includes its activity entries and recorded problems. (F05-023, F05-024, F05-025, F05-026, F05-027, F05-028, F05-029, F05-030)
11. Another system requests inbound targets; the request is routed and returned with results filtered to valid records, optionally filtered by contract and metadata ID, de-duplicated to one result per metadata ID. (F05-021, F05-022, F05-031, F05-032, F05-033, F05-034)

## Outcome

- Staff see matching file transfer records along with a total count (or an empty list and total 0).
- Staff see module options using the active-only query `Module?$filter=IsActive%20eq%20true&$orderby=ModuleName%20asc`.
- Another system receives a movement result list where shared-folder source locations use an empty `OriginatingServerName`, and FTP-based locations use the FTP server name. (F05-025)

## Errors

- Tracking search and file transfer configuration searches return an error response when the provider indicates a non-OK result. (F05-008, F05-011)
- File transfer configuration search treats an OK provider result with no returned list as an error response. (F05-011)
- Pick-list lookup returns an error response when the internal processor result is not valid. (F05-014)
- Inbound metadata lookup returns a not-found response with an empty list only when the provider response or its data list is missing. (F05-018)
