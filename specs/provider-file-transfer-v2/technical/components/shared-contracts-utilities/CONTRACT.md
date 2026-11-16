---
type: Interface Contract
title: 'Contract: shared-contracts-utilities'
description: What the shared-contracts-utilities component offers other components and what it uses from them.
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
  body_sha256: 203c418f521431ca84eb801376a53d5181778fe809d41ab1d7e1451ec625f391
---

# Contract: `shared-contracts-utilities`

This component is a small contract/utility library placed in the `fxfer-domain-api` deployable under `src/Contracts/Utilities`.
It provides helper types used by other components for localized message lookup and search-mode selection.

## Offers

- **Localized message lookup** (`BusinessErrorUtility`):
  - `GetMessage(RequestorModel requestor, string contentId) -> string`
  - Returns `string.Empty` when no localized entry exists for the requestor locale.
  - Uses message-code configuration lookup keyed by `(tenant_id, content_id)`.

- **Source annotation attribute** (`MaintainabilityDetails : Attribute`)
  - Properties: `ReviewedOnSprint`, `ReviewerName`, `IsApproved`, `ApprovedIndex`, `NeedsAttention`, `TechnicalDebtNumber`, `Notes`.

- **Search mode helpers** (`SearchModeHelper`, `SearchModeOption`):
  - `GetSearchMode(string fieldName, List<SearchModeOption> searchOptions) -> string` (enum name)
  - `GetSearchModeValue(string fieldName, List<SearchModeOption> searchOptions) -> SearchModeOption.SearchMode`
  - `SearchModeOption.SearchMode`: `Contains`, `EndsWith`, `EqualTo`, `StartsWith`

## Uses

- Package dependency: `hpp.core.bas.cqrs` (message-code provider and requestor/message models).
- Cross-component dependencies: none.

## Data owned

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Problem Details` (business-facing message strings are separated from internal error details; do not leak stack traces/internal exceptions)

