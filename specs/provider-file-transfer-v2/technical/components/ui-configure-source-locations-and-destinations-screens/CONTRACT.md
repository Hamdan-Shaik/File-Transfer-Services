---
type: Interface Contract
title: technical/components/ui-configure-source-locations-and-destinations-screens/CONTRACT
description: What the ui-configure-source-locations-and-destinations-screens component offers other components and what it uses from them.
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
  body_sha256: 9111a2017bb77ea775959660990338a7575045e2a3e4ee0b98bd40d15021b7cf
---

﻿# Contract: ui-configure-source-locations-and-destinations-screens

This component is a UI-only component in the target state. Its external contract is the set of BFF endpoints it requires (documented in `openapi.yaml`) and the UI routes/screens it provides within the `fxfer-mfe` micro-frontend.

## Offers

### UI screens (internal to `fxfer-mfe`)

- Search source locations and configurations.
- View configuration details and destination details.
- Add/edit a source-location configuration and its destinations.
- Search FTP server credentials and view/edit/invalidate a credential record.
- Search file transfer tracking results and view tracking details.

## Uses

### FXFER BFF HTTP APIs

Base path: `/api/file-transfer/v1`.

The UI uses the following logical queries/commands (names are UI-side; the HTTP contract is in `openapi.yaml`):

- Query: `searchSourceLocations(filters)`
  - Filters: `ConfigId`, `OwnerName`, `ConfigName`, `Protocol`, `OwnerSubsystem`, `IsActive`.
  - Returns: list of `SourceLocationSummary` including `FtsSourceLocationId`.

- Query: `getSourceLocationConfiguration(ftsSourceLocationId)`
  - Returns: `SourceLocationConfiguration` containing `DestinationDetails` (each with `DestinationLineNumber`).

- Command: `addSourceLocation(payload)`
  - Payload is the editable configuration state from the edit modal, including destination rows.

- Command: `updateSourceLocation(ftsSourceLocationId, payload)`
  - Payload is the editable configuration state from the edit modal, including destination rows.

- Command: `invalidateSourceLocation(ftsSourceLocationId)`

- Query: `getFtpServers(filters)`
  - Filters: `IsValid`, optional server-name contains search.
  - Returns: list of `FtpServerSummary`.

- Command: `addFtpServer(payload)`
- Command: `updateFtpServer(ftpServerId, payload)`
- Command: `invalidateFtpServer(ftpServerId)`

- Query: `searchFileTransferMovement(filters)`
- Query: `searchInboundTargets(filters)`

### Error contract

All BFF errors consumed by this component are RFC 7807 Problem Details (`application/problem+json`). The UI displays a safe summary plus `correlation_id` for support.

## Data owned

None. This component does not own persistent data; it owns only UI state (form fields, grid selection, dirty-state) in the browser.
