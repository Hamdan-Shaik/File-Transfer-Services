---
type: Target Placement
title: technical/components/ui-configure-source-locations-and-destinations-screens/TARGET
description: Where the ui-configure-source-locations-and-destinations-screens component goes in the new system and what to build, method by method.
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
  body_sha256: 32e25d2f11012074c60e1e11647800e4498036ace8b5a4915eb3b82391364f08
---

﻿# Target: ui-configure-source-locations-and-destinations-screens

## Where

- Deployable: `fxfer-mfe`
- Folder: `apps/fxfer-mfe/src/screens/configure-source-locations-and-destinations`

## What to create

Implement the legacy behaviors as React/Next.js screens and modal panels. All backend integration is via the FXFER BFF (`/api/file-transfer/v1`) and must use bearer JWT from the shell.

### Screen modules

Create the following modules under `apps/fxfer-mfe/src/screens/configure-source-locations-and-destinations`:

- `SearchSourceLocationsScreen`
  - Renders the search form and results table for source-location configurations.
  - Search fields (all optional): `ConfigId`, `ConfigName`, `OwnerName`, `Protocol`, `OwnerSubsystem`.
  - Include-inactives checkbox behavior matches legacy: it drives the `IsActive` filter value as `!includeInactivesChecked`.
  - On search, calls `FxferBffClient.searchSourceLocations` and shows results.
  - Add button opens `SourceLocationConfigurationEditModal` with no `FtsSourceLocationId`.
  - Row selection stores `ftsSourceLocationId` and view action opens `SourceLocationConfigurationViewModal`.

- `SourceLocationConfigurationViewModal`
  - Loads configuration using `FxferBffClient.getSourceLocationConfiguration(ftsSourceLocationId)`.
  - Displays source fields and a destinations table.
  - Destination row click switches the modal content from “Configuration” view to “Destination” view; modal close returns to the configuration view when currently showing destination view, otherwise closes the modal.
  - Edit button closes this modal and opens `SourceLocationConfigurationEditModal` with `{ FtsSourceLocationId }`.
  - Run schedule radio state is derived exactly as legacy:
    - If `FtsSourceLocationId` is empty, default weekly.
    - Else monthly when `RunOnFirstDay == true` OR `RunOnLastDay == true` OR `RunOnThisDay.ID != "0"`; otherwise weekly.

- `SourceLocationConfigurationEditModal`
  - Supports add/edit; edit mode is selected when `FtsSourceLocationId` is present.
  - Save behavior:
    - Validates `SourceAddress` format for FTP/SFTP/FTPS protocols using the same regex behavior as legacy: test `ftpRegexp` against `SourceAddress + " "`.
    - Requires at least one destination row with `Active == true`; otherwise fails validation with the legacy “destination required” message.
    - Requires FTP server selection when protocol id is `1`, `2`, or `4`; otherwise FTP server is not required and the selector is hidden.
    - Requires `RunSchedule.ID` radio selection; when missing, shows required-field validation message.
  - Close behavior:
    - When configuration panel is dirty or a record was saved, prompts with dirty-data confirmation.
    - When destination edit panel is active and dirty, prompts with dirty-data confirmation.

- `DestinationConfigurationEditPanel`
  - Add mode defaults `Active = true` and shows `Add Another` and `Save and Close` actions.
  - Edit mode is entered when a destination table row has `DestinationLineNumber > 0`; it hides add-mode actions and shows update action.
  - When destination type equals `fileToEvent`:
    - Destination protocol/type/contract/metadata and contact fields are read-only.
    - Required markers for those fields are removed.
    - Reset and create-from-existing actions are hidden.
  - Otherwise:
    - Destination protocol/type are required.
    - When destination type equals `fileToEvent`, contract and metadata are required and metadata required indicator is displayed.

- `SearchFtpServerCredentialsScreen`
  - Renders FTP server credential search form and results grid.
  - On search, calls `FxferBffClient.getFtpServers` with filters:
    - `IsValid == true` when show-inactives is not checked.
    - `IsValid == ""` (empty) when show-inactives is checked.
    - Optional name filter uses `contains` semantics.
  - Row selection opens `ViewEditFtpServerCredentialsModal`.

- `ViewEditFtpServerCredentialsModal`
  - For an existing server (`FtpServerId` non-empty and not the default GUID): shows audit info.
  - When `IsValid == false`: all fields are read-only and Save/Invalidate/Reset controls are hidden.
  - When `IsValid == true`: `FtpServerName` is read-only; password fields are optional unless either password field has a non-empty value.
  - Proxy port becomes required when proxy address is non-empty.
  - Validates IP address format using legacy pattern `^(?!0)(?!.*\.$)((1?\d?\d|25[0-5]|2[0-4]\d)(\.|$)){4}$` for server IP and proxy address (when present).
  - Invalidate prompts with the legacy invalidation warning and, on confirm, calls `FxferBffClient.invalidateFtpServer`.

- `SearchFileTransferTrackingScreen`
  - Initializes `ToDate` and `FromDate` pickers to “today” and then clears the input value.
  - Search uses the same filter fields as legacy:
    - `FileId`, `FileName`, `FileStatus`, `Direction`, `TransportType`, `FromDate`, `ToDate`, `FromSize`, `ToSize`, `TradingPartnerId`, `SourceLocationId`.
  - Row selection opens `FileTransferTrackingViewModal`.

- `FileTransferTrackingViewModal`
  - Close is performed by `closeRootOverlay` equivalent.
  - Tracking events/exceptions grids auto-fit columns 0 and 1 on data bound.

### Shared support (used, not owned by this component)

- Uses deployable-wide BFF client wrapper in `apps/fxfer-mfe/src/api/` for authenticated fetch + correlation/trace headers.
- Uses deployable-wide observability helpers in `apps/fxfer-mfe/src/observability/`.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| FileTransferController.SearchSourceLocations | SearchSourceLocationsScreen | MVC view replaced by screen component. |
| FileTransferController.AddFileBaseConfiguration | SourceLocationConfigurationEditModal | Add mode; defaults `IsActive = true` when id missing. |
| FileTransferController.FileTransferConfigurationView | SourceLocationConfigurationViewModal | Loads configuration via BFF query. |
| FileTransferController.DestinationConfigurationEditPanel | DestinationConfigurationEditPanel | Panel is a client-side subview. |
| FileTransferController.FileTransferConfigurationEdit | SourceLocationConfigurationEditModal | Edit mode when id present; refreshes via BFF query. |
| FileTransferController.DestinationConfigurationEdit | DestinationConfigurationEditPanel | Edit view/panel merged into one React panel. |
| FileTransferController.GetFileTransferConfiguration | FxferBffClient.getSourceLocationConfiguration | Target fetch returns configuration model; only replaces model on success. |
| FileTransferController.SearchFTPServerCredentials | SearchFtpServerCredentialsScreen | MVC view replaced by screen component. |
| FileTransferController.AddFTPServerCredentials | ViewEditFtpServerCredentialsModal | Razor partial replaced by modal component. |
| FileTransferController.BuildFTPServerModel | SearchFtpServerCredentialsScreen | Editor setup replaced by screen initialization. |
| FileTransferController.SearchFileTransferTracking | SearchFileTransferTrackingScreen | MVC view replaced by screen component. |
| FileTransferController.FileTransferTrackingView | FileTransferTrackingViewModal | Overlay editor replaced by modal component. |
| FileTransferSvcQueryServiceController.SearchSourceLocations | FxferBffClient.searchSourceLocations | Legacy controller replaced by BFF endpoint consumed by UI. |
| FileTransferSvcQueryServiceController.SearchFileTransferMovement | FxferBffClient.searchFileTransferMovement | Legacy controller replaced by BFF endpoint consumed by UI. |
| FileTransferSvcQueryServiceController.GetFtpServers | FxferBffClient.getFtpServers | Legacy controller replaced by BFF endpoint consumed by UI. |
| FileTransferSvcQueryServiceController.GetSourceLocations | FxferBffClient.getSourceLocationConfiguration | Legacy controller replaced by BFF endpoint consumed by UI. |
| FileTransferSvcQueryServiceController.SearchInboundTarget | FxferBffClient.searchInboundTargets | Legacy controller replaced by BFF endpoint consumed by UI. |
| FileTransferSvcServiceController.FileTransferAdd | FxferBffClient.addSourceLocation | Legacy controller replaced by BFF endpoint consumed by UI. |
| FileTransferSvcServiceController.UpdateFileTransferSourceLocation | FxferBffClient.updateSourceLocation | Legacy controller replaced by BFF endpoint consumed by UI. |
| FileTransferSvcServiceController.InvalidateFileTransferSourceLocation | FxferBffClient.invalidateSourceLocation | Legacy controller replaced by BFF endpoint consumed by UI. |
| FileTransferSvcServiceController.AddFtpServer | FxferBffClient.addFtpServer | Legacy controller replaced by BFF endpoint consumed by UI. |
| FileTransferSvcServiceController.UpdateFtpServer | FxferBffClient.updateFtpServer | Legacy controller replaced by BFF endpoint consumed by UI. |
| FileTransferSvcServiceController.InvalidateFtpServer | FxferBffClient.invalidateFtpServer | Legacy controller replaced by BFF endpoint consumed by UI. |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-001-backend-for-frontend-pattern.md` section `Executive Summary / Solution Overview` (UI integrates through a dedicated BFF)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution / Pattern Description` (BFF endpoints represented in OpenAPI)
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `TL;DR` (RFC 7807 Problem Details error contract, sanitized errors)
- `patterns/application/frontend/PTN-APP-FE-001-accessible-component-pattern.md` sections `Solution / Capability Area Definitions / 3. Keyboard Navigation` and `Acceptance Criteria` (keyboard operability and accessible semantics)
