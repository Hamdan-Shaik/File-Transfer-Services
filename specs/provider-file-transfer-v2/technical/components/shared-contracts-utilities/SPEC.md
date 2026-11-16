---
type: Behavior Specification
title: 'shared-contracts-utilities: Shared contract helper utilities (message lookup + search mode helpers)'
description: 'What the shared-contracts-utilities component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-utilities
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:55:00Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-utilities/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-utilities
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 4
  methods: 6
  owned_rules: 0
  flows:
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Utilities
  body_sha256: e7dc71aadeda9504844e354c2464613bbaf4838abb7a366587868a54d39165a1
---

# shared-contracts-utilities: Shared contract helper utilities (message lookup + search mode helpers)

This component is a small utility/contract library used by other `fxfer-domain-api` contract and application components.
It provides:

- localized message text lookup (tenant + locale + content id) via the legacy CQRS message-code provider.
- simple search-mode helper types used to choose string filter behavior.

It owns no business rules and exposes no HTTP surface.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.BusinessErrorUtility
Utility for looking up localized message descriptions from configuration message codes.

Constructor behavior:

- `BusinessErrorUtility()` initializes an `IMessageCodesReadOnly` instance (`new MessageCodesReadOnly()`).

### GetMessage(HPP.Core.BAS.CQRS.UserMeta.RequestorModel requestor, string contentId)
- Purpose: Return a localized message description string for `contentId`, using the requestor's tenant id and locale.
- Inputs:
  - `requestor`: provides `TenantId` and `LocaleCode`.
  - `contentId`: message/content identifier used to retrieve a configured message.
- Logic:
  - Calls `LoadConfigurationMessage(requestor, contentId)` to load a `CodeListModel`.
  - If `message != null` and `message.LanguageList != null` and `message.LanguageList.Count > 0`:
    - Finds the first `Languages` entry where `LocaleID.ToLower() == requestor.LocaleCode.ToLower()`.
    - Returns the matching language's `Description`.
    - If no matching language is found, returns `string.Empty`.
  - Otherwise returns `string.Empty`.
- Data:
  - Reads: message code configuration via `IMessageCodesReadOnly.GetMessage(<tenantGuid>, contentId)`.
  - Writes: none.
- Errors:
  - Propagates exceptions from `LoadConfigurationMessage`, including `FormatException`/`ArgumentNullException` from `Guid.Parse(requestor.TenantId)`.
  - Throws `NullReferenceException` when `requestor` is null.
  - Throws `NullReferenceException` when `requestor.LocaleCode` is null (due to `ToLower()`).
  - Throws `NullReferenceException` when a `Languages` entry has `LocaleID == null` (due to `ToLower()`).
- Rules: None.

### GetMessage(RequestorModel requestor, string contentId)
Legacy behavior is identical to `GetMessage(HPP.Core.BAS.CQRS.UserMeta.RequestorModel, string)`.

- Purpose: Return a localized message description string for `contentId`, using the requestor's tenant id and locale.
- Inputs: `requestor`, `contentId`.
- Logic/Data/Errors: same as the method signature above.
- Rules: None.

### LoadConfigurationMessage(HPP.Core.BAS.CQRS.UserMeta.RequestorModel requestor, string contentId)
- Purpose: Load the configured message-code model for a given tenant and content id.
- Inputs:
  - `requestor.TenantId`: parsed using `Guid.Parse(...)`.
  - `contentId`.
- Logic: Returns `messageCodes.GetMessage(Guid.Parse(requestor.TenantId), contentId)`.
- Data:
  - Reads: message code configuration via `IMessageCodesReadOnly`.
  - Writes: none.
- Errors:
  - Propagates exceptions from `Guid.Parse` and from `messageCodes.GetMessage(...)`.
- Rules: None.

### LoadConfigurationMessage(RequestorModel requestor, string contentId)
Legacy behavior is identical to `LoadConfigurationMessage(HPP.Core.BAS.CQRS.UserMeta.RequestorModel, string)`.

- Purpose/Inputs/Logic/Data/Errors: same as the method signature above.
- Rules: None.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.MaintainabilityDetails
Source annotation attribute with no runtime behavior.

Properties (get/set auto-properties):

- `ReviewedOnSprint` (int)
- `ReviewerName` (string)
- `IsApproved` (bool)
- `ApprovedIndex` (int)
- `NeedsAttention` (bool)
- `TechnicalDebtNumber` (int)
- `Notes` (string)

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.SearchModeHelper
Helper for selecting a search mode for a field name based on a list of configured options.

### GetSearchMode(string fieldName, List<SearchModeOption> searchOptions)
- Purpose: Return the selected search mode enum name as a string.
- Inputs:
  - `fieldName`: field identifier used to match a `SearchModeOption.FieldName`.
  - `searchOptions`: list of field search options.
- Logic:
  - Calls `GetSearchModeValue(fieldName, searchOptions)`.
  - Returns the enum value converted to a string via `.ToString()`.
- Data: none.
- Errors: Propagates any exceptions from `GetSearchModeValue`.
- Rules: None.

### GetSearchModeValue(string fieldName, List<SearchModeOption> searchOptions)
- Purpose: Return the selected search mode enum value.
- Inputs: `fieldName`, `searchOptions`.
- Logic:
  - If `searchOptions != null && searchOptions.Count > 0`, finds the first entry where `option.FieldName == fieldName`, else treats it as not found.
  - If a matching option is found, returns `option.FilterType`.
  - If no option is found (including when the list is null/empty), returns `SearchModeOption.SearchMode.EqualTo`.
- Data: none.
- Errors:
  - Throws `NullReferenceException` if `searchOptions` contains a null element (predicate dereferences `.FieldName`).
- Rules: None.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.SearchModeOption
DTO for configuring how a given field should be filtered during search.

Constructor behavior:

- `SearchModeOption()` sets `FilterType = SearchMode.EqualTo`.

Enumeration:

- `SearchMode` values: `Contains`, `EndsWith`, `EqualTo`, `StartsWith`.

Properties:

- `FieldName` (string)
- `FilterType` (SearchMode)

## Rules implemented

None (this component owns no business rules).

## Not carried over

None.

