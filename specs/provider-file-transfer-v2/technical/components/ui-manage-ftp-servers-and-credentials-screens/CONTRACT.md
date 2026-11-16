---
type: Interface Contract
title: 'Contract: ui-manage-ftp-servers-and-credentials-screens'
description: What the ui-manage-ftp-servers-and-credentials-screens component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/ui-manage-ftp-servers-and-credentials-screens
tags:
- component
- provider-file-transfer-v2
- ui
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:47:47Z
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
  resource: /technical/components/ui-manage-ftp-servers-and-credentials-screens/INVENTORY.yaml
  title: Every legacy class and method (1 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:ui-manage-ftp-servers-and-credentials-screens
  generation:
    status: done
    attempts: 2
  tier: ui
  classes: 1
  methods: 25
  owned_rules: 51
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - search-and-track-file-transfers
  depends_on:
  - backend-data-file-transfer
  deployable: fxfer-mfe
  folder: src/screens/manage-ftp-servers-and-credentials
  body_sha256: 3f22f69db5152c988b40b9b5ad55acb6606e1fc90240c2e8a19a5e28fe5499a5
---

# Contract: ui-manage-ftp-servers-and-credentials-screens

This component is UI-only in the target state. Its external contract is the set of FXFER BFF endpoints it requires (documented in `openapi.yaml`) and the internal UI modules it provides within the `fxfer-mfe` micro-frontend.

## Offers

### UI modules (internal to `fxfer-mfe`)

- Screen-local API adapters and mappers for:
  - Upserting file-transfer base configuration.
  - Loading source-location and FTP-server option lists.
  - Browsing and saving FTP server credentials with paging, filtering, and localized success messages.
  - Searching file transfers and file transfer tracking results.
  - Data-list lookups (modules) and FTE metadata id lookups.

## Uses

### Component: `backend-data-file-transfer`

Uses shared contract types when shaping requests to, and reading responses from, `fxfer-bff`.

- Transfer configuration DTOs: `TransferConfigurationUpsertRequest`, `TransferConfiguration`, `TransferConfigurationSummary`.
- FTP server/profile DTOs: `FtpServer`, `FtpServerOption`.
- Common DTOs: `PagedResult<T>`, `SourceLocationOption`, `InboundTargetMetadata`.
- Search helper semantics: `SearchHelper.GetFieldValue(searchFields, fieldName)` (used when mapping UI filter fields, matching the legacy controller behavior).

### FXFER BFF HTTP APIs

Base path: `/api/file-transfer/v1`.

Logical operations consumed by this component (names are UI-side; the HTTP contract is in `openapi.yaml`):

- Command: `upsertFileTransferBaseConfiguration(model)`
  - Returns: echoes the submitted model on success.

- Query: `getSourceLocationOptions()`
  - Returns: list items with id string and display name ConfigName, space-hyphen-space, Address.

- Query: `getFtpServerOptions()`
  - Returns: list items with `Name = FtpServerName` and `ID = FtpServerId` (string).

- Query: `searchFtpServerCredentials(filters, paging, sorting)`
  - Returns: `{ data, total }` where total is based on service total rows.
  - Passwords are removed from results by default.

- Command: `saveFtpServerCredentials(model)`
  - Create vs update is driven by `FtpServerId`.
  - Update vs invalidate is driven by `IsValid`.
  - Returns: saved model with localized `Editor.SavedMessage` and `Editor.SaveActionText = Success` on success.

- Query: `searchFileTransfers(criteria)`
- Query: `searchFileTransferTracking(criteria)`

- Query: `getDataListData({ contentId, dataListType, localeId })`
  - Only MODULE yields populated results; other types return empty list.

- Query: `getFteMetadatas(contractName)`
  - Returns 200 whenever response data list is non-null.
  - Returns 404 with empty list only when response or response data list is null.

### Error contract

All BFF errors consumed by this component are RFC 7807 Problem Details (`application/problem+json`). The UI displays a safe summary plus `correlation_id` for support.

## Data owned

None. This component does not own persistent data; it owns only UI state (filters, paging state, dirty-state) in the browser.

