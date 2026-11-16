---
type: Flow Specification
title: Configure file transfer source locations and destinations
description: 'This flow lets staff view and maintain file transfer setups: where files are picked up from (source locations) and where they are delivered (destinations).'
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
  body_sha256: 33314d7c75aa76a868e43d2ecc693aa70106eba5a00ccfaea0db204fd45d1146
---

# Configure file transfer source locations and destinations

This flow lets staff view and maintain file transfer setups: where files are picked up from (source locations) and where they are delivered (destinations). It covers viewing current settings, saving new or changed settings, deactivating a setup, and finding existing setups to reuse.

## Who starts it

- Staff using the file transfer configuration screens
- Other internal components calling the add, update, invalidate, and lookup doors

## Doors

| Door | Request | Answers |
| --- | --- | --- |
| `file-transfer-controller-file-transfer-configuration-view` | Open the file transfer configuration view (GET /FileTransfer) | Renders the configuration view with a configuration model (F01-001, F01-005, F01-007) |
| `file-transfer-controller-file-transfer-configuration-edit` | Submit edits from the configuration screen (POST /FileTransfer) | Renders the edit view with defaults or reloaded data (F01-002, F01-005, F01-007) |
| `file-transfer-controller-add-file-base-configuration` | Start adding a new setup (POST /FileTransfer) | Renders the add view and defaults the setup as active when no source location is provided (F01-021) |
| `file-transfer-controller-add-file-transfer-base-configuration` | Save a new or updated setup (HTTP) | Accepts the submitted details and either returns the submitted details or returns validation failures (F01-023, F01-026) |
| `file-transfer-controller-destination-configuration-edit` | Open the destination edit screen (POST /FileTransfer) | Renders the destination edit view as provided (F01-004) |
| `file-transfer-controller-destination-configuration-edit-panel` | Open the destination edit panel (POST /FileTransfer) | Renders the destination edit panel as provided (F01-003) |
| `file-transfer-svc-service-file-transfer-add` | Add a source location (WCF) | Returns an “added” event (F01-034) |
| `file-transfer-svc-service-file-transfer-add-2` | Add a source location (WCF, API-hosted) | Returns an “added” event (F01-035) |
| `file-transfer-svc-service-file-transfer-add-3` | Add a source location (WCF, core) | Returns an “added” event (F01-036) |
| `file-transfer-svc-service-controller-file-transfer-add` | Add a source location (HTTP) | Returns the “added” event as the response body (F01-037) |
| `file-transfer-svc-service-update-file-transfer-source-location` | Update a source location (WCF) | Returns an “updated” event (F01-055) |
| `file-transfer-svc-service-update-file-transfer-source-location-2` | Update a source location (WCF, API-hosted) | Returns an “updated” event (F01-055) |
| `file-transfer-svc-service-update-file-transfer-source-location-3` | Update a source location (WCF, core) | Returns an “updated” event (F01-055) |
| `file-transfer-svc-service-controller-update-file-transfer-source-location` | Update a source location (HTTP) | Returns the “updated” event as the response body (F01-056) |
| `file-transfer-svc-service-invalidate-file-transfer-source-location` | Deactivate a source location (WCF) | Returns an “invalidated” event (F01-057) |
| `file-transfer-svc-service-invalidate-file-transfer-source-location-2` | Deactivate a source location (WCF, API-hosted) | Returns an “invalidated” event (F01-057) |
| `file-transfer-svc-service-invalidate-file-transfer-source-location-3` | Deactivate a source location (WCF, core) | Returns an “invalidated” event (F01-057) |
| `file-transfer-svc-service-controller-invalidate-file-transfer-source-location` | Deactivate a source location (HTTP) | Returns the “invalidated” event as the response body (F01-058) |
| `file-transfer-svc-query-service-search-source-locations` | Search source locations (WCF) | Returns a result set of source locations (F01-079, F01-091) |
| `file-transfer-svc-query-service-search-source-locations-2` | Search source locations (WCF, API-hosted) | Returns a result set of source locations (F01-079, F01-091) |
| `file-transfer-svc-query-service-search-source-locations-3` | Search source locations (WCF, core) | Returns a result set of source locations (F01-079, F01-091) |
| `file-transfer-svc-query-service-controller-search-source-locations` | Search source locations (HTTP) | Returns the query result as the response body (F01-080) |
| `file-transfer-svc-query-service-get-source-locations` | Get one source location’s details (WCF) | Returns a single source location with its destinations (F01-081, F01-103, F01-105) |
| `file-transfer-svc-query-service-get-source-locations-2` | Get one source location’s details (WCF, API-hosted) | Returns a single source location with its destinations (F01-081, F01-103, F01-105) |
| `file-transfer-svc-query-service-get-source-locations-3` | Get one source location’s details (WCF, core) | Returns a single source location with its destinations (F01-081, F01-103, F01-105) |
| `file-transfer-svc-query-service-controller-get-source-locations` | Get one source location’s details (HTTP) | Returns the query result as the response body (F01-082) |
| `file-transfer-controller-get-source-locations` | Get a list of active source locations for selection (HTTP) | Returns a list of display items for active source locations (F01-083, F01-087, F01-090) |

## Steps

1. A staff member opens the configuration view for a source location (F01-001, F01-005, F01-006, F01-007).
2. The system loads configuration details; if it cannot find a single matching source location it leaves the in-flight model unchanged (F01-007, F01-014).
3. The configuration screen shows schedule and status labels based on the stored day selections and active flag (F01-017, F01-018, F01-019).
4. A staff member submits edits; when the source location id is missing the system defaults the setup to active, otherwise it reloads existing details (F01-002).
5. A staff member starts adding a new setup; the add screen defaults the new setup to active (F01-021).
6. A staff member saves a new or updated setup; invalid input is rejected before it is sent onward, and phone numbers are normalized by removing hyphens (F01-023, F01-024, F01-027).
7. Adding a setup validates required fields, requires at least one valid destination, and requires at least one schedule selection (F01-041, F01-042, F01-043).
8. Adding or updating may fail if the chosen transfer method requires server details and those details are missing (F01-044).
9. When an add succeeds, the system assigns a new configuration number, stores a new source location row, and stores one row per destination (F01-046, F01-048, F01-052, F01-054).
10. Updating or deactivating a setup produces an event that updates the stored source location and destinations, including marking them inactive when required; applying that update fails hard when the event's source location reference is missing or not a GUID string (F01-055, F01-073, F01-075, F01-076, F01-077, F01-107, F01-108).
11. A staff member requests a list of active source locations to select from; the list is sorted by name and each entry is shown as “name – address” (F01-088, F01-090, F01-087).
12. A staff member searches for source locations; filtering is exact-match (case-insensitive for names) and sorting defaults to configuration number then active status (F01-092, F01-093, F01-094, F01-098).
13. A staff member opens one source location’s details; the response includes the source location and its destinations ordered newest-first (F01-101, F01-103, F01-105, F01-106).

## Outcome

A caller gets back configuration details for a setup (when one exists), including its active/inactive status, its schedule, and its destinations. For example, when a setup named “Daily Claims” has address “/in/claims”, the selection list shows it as “Daily Claims - /in/claims” (F01-087), and phone numbers such as “555-010-1234” are processed without hyphens (F01-024).

## Errors

- A save can be rejected with missing-field validations, missing destinations, or missing schedule selections (F01-041, F01-042, F01-043).
- A save can fail when a required server selection is missing for a server-based transfer method (F01-044).
- A save can be rejected when the source file mask extension is in the excluded list (for example “exe”) (F01-045).
- Some lookups and saves can fail hard when values expected to be GUID strings are not parseable (flagged in F01-059, F01-095, F01-101, F01-107, F01-108).

