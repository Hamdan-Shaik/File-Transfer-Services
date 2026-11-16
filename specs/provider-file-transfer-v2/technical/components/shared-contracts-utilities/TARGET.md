---
type: Target Placement
title: 'Target: shared-contracts-utilities'
description: Where the shared-contracts-utilities component goes in the new system and what to build, method by method.
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
  body_sha256: cc271aa0f37d08cd7b16d0dae05ea8f569e593f25bf15d19a02291accf0c1339
---

# Target: `shared-contracts-utilities`

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Contracts/Utilities`

## What to create

Create the following small contract/utility types in `services/fxfer-domain-api/src/Contracts/Utilities`.

- `services/fxfer-domain-api/src/Contracts/Utilities/BusinessErrorUtility.cs`
  - Type: `public sealed class BusinessErrorUtility`
  - Dependencies:
    - `hpp.core.bas.cqrs` message-code configuration types: `IMessageCodesReadOnly`, `MessageCodesReadOnly`, `CodeListModel`, `Languages`, `RequestorModel`.
  - Constructors:
    - `public BusinessErrorUtility()`
      - Initialize the message-code provider with `new MessageCodesReadOnly()` (legacy parity).
    - `internal BusinessErrorUtility(IMessageCodesReadOnly messageCodes)`
      - Assign the provided instance (test seam).
  - Methods:
    - `public string GetMessage(RequestorModel requestor, string contentId)`
      - Call `LoadConfigurationMessage(requestor, contentId)`.
      - When a message is returned and it has a non-empty `LanguageList`, find the first language where `LocaleID.ToLower() == requestor.LocaleCode.ToLower()`.
      - Return the matched language `Description`; when no match or no languages, return `string.Empty`.
      - Preserve legacy null/exception behavior (no additional null guards).
    - `private CodeListModel LoadConfigurationMessage(RequestorModel requestor, string contentId)`
      - Return `messageCodes.GetMessage(Guid.Parse(requestor.TenantId), contentId)`.

- `services/fxfer-domain-api/src/Contracts/Utilities/MaintainabilityDetails.cs`
  - Type: `public sealed class MaintainabilityDetails : Attribute`
  - Properties (get/set): `ReviewedOnSprint`, `ReviewerName`, `IsApproved`, `ApprovedIndex`, `NeedsAttention`, `TechnicalDebtNumber`, `Notes`.

- `services/fxfer-domain-api/src/Contracts/Utilities/SearchModeHelper.cs`
  - Type: `public static class SearchModeHelper`
  - Methods:
    - `public static string GetSearchMode(string fieldName, List<SearchModeOption> searchOptions)`
      - Return `GetSearchModeValue(fieldName, searchOptions).ToString()`.
    - `public static SearchModeOption.SearchMode GetSearchModeValue(string fieldName, List<SearchModeOption> searchOptions)`
      - If `searchOptions` is non-null and non-empty, locate the first option where `option.FieldName == fieldName`.
      - When found, return `option.FilterType`; otherwise return `SearchModeOption.SearchMode.EqualTo`.

- `services/fxfer-domain-api/src/Contracts/Utilities/SearchModeOption.cs`
  - Type: `public sealed class SearchModeOption`
  - Constructor:
    - `public SearchModeOption()` sets `FilterType = SearchMode.EqualTo`.
  - Enum: `public enum SearchMode { Contains, EndsWith, EqualTo, StartsWith }`.
  - Properties (get/set): `FieldName` (string), `FilterType` (SearchMode).

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.BusinessErrorUtility.GetMessage(HPP.Core.BAS.CQRS.UserMeta.RequestorModel, string)` | `BusinessErrorUtility.GetMessage(RequestorModel, string)` | Legacy inventory includes two signature spellings; target keeps one method using the CQRS `RequestorModel`. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.BusinessErrorUtility.GetMessage(RequestorModel, string)` | `BusinessErrorUtility.GetMessage(RequestorModel, string)` | Same underlying behavior/signature in legacy source. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.BusinessErrorUtility.LoadConfigurationMessage(HPP.Core.BAS.CQRS.UserMeta.RequestorModel, string)` | `BusinessErrorUtility.LoadConfigurationMessage(RequestorModel, string)` | Keep as a private helper invoked by `GetMessage`. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.BusinessErrorUtility.LoadConfigurationMessage(RequestorModel, string)` | `BusinessErrorUtility.LoadConfigurationMessage(RequestorModel, string)` | Same underlying behavior/signature in legacy source. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.SearchModeHelper.GetSearchMode(string, List<SearchModeOption>)` | `SearchModeHelper.GetSearchMode(string, List<SearchModeOption>)` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities.SearchModeHelper.GetSearchModeValue(string, List<SearchModeOption>)` | `SearchModeHelper.GetSearchModeValue(string, List<SearchModeOption>)` | Direct parity. |

## Dependencies

- Component dependencies: none.
- Package dependencies: `hpp.core.bas.cqrs`.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (localized/business-facing message text is surfaced via stable, consumer-friendly contracts; avoid leaking internal exception details)

