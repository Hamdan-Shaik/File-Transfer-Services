---
type: Target Placement
title: 'Target: ui-manage-ftp-servers-and-credentials-screens'
description: Where the ui-manage-ftp-servers-and-credentials-screens component goes in the new system and what to build, method by method.
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
  body_sha256: e7f5e055bdb69e93eb2623ad5a879b11c5572e6f2da7c2e6470749a6b8c076b6
---

# Target: ui-manage-ftp-servers-and-credentials-screens

## Where

- Deployable: `fxfer-mfe`
- Folder: `apps/fxfer-mfe/src/screens/manage-ftp-servers-and-credentials`

## What to create

Implement the legacy UX Web API controller behaviors as screen-local BFF integrations in the micro-frontend.

- All backend integration is `UI -> fxfer-bff` only (base URL `/api/file-transfer/v1`) and uses bearer JWT supplied by the shell.
- Errors are handled as RFC 7807 Problem Details and always surface `correlation_id` to the operator.
- Legacy anti-forgery requirements are carried via the target-state posture (stateless bearer JWT + strict CORS and Origin or Referer validation for state-changing requests), not by porting the legacy ASP.NET anti-forgery token implementation.

### Screen-local API modules

Create the following modules under `apps/fxfer-mfe/src/screens/manage-ftp-servers-and-credentials`:

- `TransferConfigurationBaseApi`
  - `upsertBaseConfiguration(model)`
    - Rejects invalid payloads with 400.
    - When `OwnerPhoneNumber` is present, removes hyphens before sending.
    - When `FtsSourceLocationId` is empty, triggers create behavior; otherwise triggers update behavior.
    - On success, returns the submitted model (echo) as legacy does.

- `SourceLocationOptionsApi`
  - `getSourceLocationOptions()`
    - Returns list-items whose display name is ConfigName, space-hyphen-space, Address and whose id is the source location id string.

- `FtpServerOptionsApi`
  - `getFtpServerOptions()`
    - Returns list-items whose `Name` is `FtpServerName` and whose `ID` is `FtpServerId` formatted as a string.

- `FtpServerCredentialsApi`
  - `searchCredentials(gridState)`
    - Converts UI grid paging, sorting, and filters into the BFF request model.
    - Filter mapping matches legacy `BuildRequest` semantics for the IsValid filter: any non-empty value forces IsValid true, otherwise IsValid is omitted.
    - When multiple sorts are provided, only the last sort criterion is used.
    - Ensures returned rows do not include saved passwords by default.
    - Returns `{ data, total }` where total comes from service-reported total rows.
  - `saveCredentials(model)`
    - Create vs update matches legacy: empty `FtpServerId` means create.
    - Update vs invalidate matches legacy: `IsValid == true` updates, `IsValid == false` invalidates.
    - On success, sets localized `Editor.SavedMessage` and `Editor.SaveActionText = Success` and returns the saved model.
    - When the update or invalidate response has no Data, treat it as an error and surface only the first service message.

- `FileTransferSearchApi`
  - `searchFileTransfers(gridState)`
    - Returns 200 only when provider status is OK and data list is non-null; otherwise surfaces error.

- `FileTransferTrackingSearchApi`
  - `searchTracking(gridState)`
    - On OK, returns `{ data, total }`.
    - When paging results are missing, total defaults to 0.
    - When provider data list is null, data is empty list and total is 0.

- `DataListApi`
  - `getDataListData({ contentId, dataListType, localeId })`
    - Treat missing dataListType as empty string.
    - Only MODULE type populates results.
    - Module list is active modules ordered by module name ascending.

- `FteMetadataApi`
  - `getFteMetadatas(contractName)`
    - Returns 200 whenever response data list is non-null.
    - Returns 404 with empty list only when response or response data list is null.

### Shared support (used, not owned by this component)

- Uses deployable-wide BFF client wrapper in `apps/fxfer-mfe/src/api/` for authenticated fetch + correlation and trace headers.
- Uses shared DTOs from component `backend-data-file-transfer` where applicable.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `FileTransferController.AddFTPServerCredentials` | `FtpServerCredentialsApi.saveCredentials` | Create vs update dispatch moved into UI-side request shaping + BFF call. |
| `FileTransferController.AddFileTransferBaseConfiguration` | `TransferConfigurationBaseApi.upsertBaseConfiguration` | Upsert behavior preserved; legacy anti-forgery token replaced by BFF trust-boundary posture. |
| `FileTransferController.BuildDataSourceResult` | `FtpServerCredentialsApi.searchCredentials` | Target returns `{ data, total }` derived from service total rows. |
| `FileTransferController.BuildRequest` | `FtpServerCredentialsApi.searchCredentials` | Search field mapping preserved. |
| `FileTransferController.BuildResponse` | `FtpServerCredentialsApi.searchCredentials` | Error vs OK branching preserved. |
| `FileTransferController.CreateFTPServerCredentials` | `FtpServerCredentialsApi.saveCredentials` | Save operation preserved; errors surfaced through Problem Details. |
| `FileTransferController.CreateRequest` | `FtpServerCredentialsApi.saveCredentials` | Request envelope becomes BFF request DTO creation. |
| `FileTransferController.FormatInvalidResponse` | `FtpServerCredentialsApi.saveCredentials` | Preserve single-message failure behavior. |
| `FileTransferController.FtpServerCredentialsSave` | `FtpServerCredentialsApi.saveCredentials` | Localized success message and SaveActionText preserved. |
| `FileTransferController.GenerateAddFileTransferBaseConfigurationResult` | `TransferConfigurationBaseApi.upsertBaseConfiguration` | Add vs update choice preserved. |
| `FileTransferController.GenerateInvalidModelStateRepsonse` | `TransferConfigurationBaseApi.upsertBaseConfiguration` | Invalid model maps to 400 with field errors. |
| `FileTransferController.GenerateResponse<T>` | `TransferConfigurationBaseApi.upsertBaseConfiguration` | OK vs error mapping preserved. |
| `FileTransferController.GetDataListData(string, string, string)` | `DataListApi.getDataListData` | Public data list lookup preserved (MODULE only). |
| `FileTransferController.GetDataListData(string, string, string, out List<ListItemModel>)` | `DataListApi.getDataListData` | Internal selector preserved (MODULE only). |
| `FileTransferController.GetFTEMetadatas` | `FteMetadataApi.getFteMetadatas` | 200 vs 404 behavior preserved. |
| `FileTransferController.GetFTPServerCredentials` | `FtpServerCredentialsApi.searchCredentials` | Kendo request replaced by typed gridState; behavior preserved. |
| `FileTransferController.GetFtpServerDetails` | `FtpServerOptionsApi.getFtpServerOptions` | List-item output preserved for dropdown selection. |
| `FileTransferController.GetModules` | `DataListApi.getDataListData` | Active-only and A to Z ordering preserved. |
| `FileTransferController.GetSourceLocations` | `SourceLocationOptionsApi.getSourceLocationOptions` | List-item output preserved for dropdown selection. |
| `FileTransferController.GetSuccessMessage` | `FtpServerCredentialsApi.saveCredentials` | Localization remains server-owned; UI displays returned text. |
| `FileTransferController.SearchFileTransfer` | `FileTransferSearchApi.searchFileTransfers` | Search result shaping preserved. |
| `FileTransferController.SearchFileTransferTracking` | `FileTransferTrackingSearchApi.searchTracking` | Null data becomes empty list preserved. |
| `FileTransferController.SetResponse` | `FtpServerCredentialsApi.saveCredentials` | OK returns saved model; error returns Problem Details. |
| `FileTransferController.UpdateFTPServerCredentials` | `FtpServerCredentialsApi.saveCredentials` | Update or invalidate branching preserved. |
| `FileTransferController.UpdateFtpServer` | `FtpServerCredentialsApi.saveCredentials` | IsValid controls update vs invalidate. |

## Dependencies

- Component: `backend-data-file-transfer`.

## Enterprise-Architecture standards applied

- `patterns/application/micro-frontend/PTN-APP-MFE-012-backend-api-integration-pattern.md` section `Pattern Description` (frontend integrates through shared authenticated client and query or mutation hooks)
- `patterns/security/api-microservices/PTN-SEC-API-008-secret-management-pattern.md` section `Pattern Description / Core Principles` (no hardcoded credentials; secrets are backend-managed and not persisted client-side)

