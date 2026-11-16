---
type: Test Plan
title: 'Tests: shared-contracts-utilities'
description: The tests that prove the shared-contracts-utilities component, one or more per business rule.
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
  body_sha256: 8fab02bd78a77eb97d02e005371f7eaccdbe76e773fb288a30b468ce19972352
---

# Tests: `shared-contracts-utilities`

This component owns no business rules.
Tests focus on method-level parity for helper behavior and basic contract checks for `openapi.yaml` and `CONTRACT.md`.

## Unit tests (method parity)

### BusinessErrorUtility

- `BusinessErrorUtility_GetMessage_returns_empty_string_when_message_is_null`
  - Given a `BusinessErrorUtility` with a message-code provider returning null for `(tenantId, contentId)`, when calling `GetMessage`, then it returns `string.Empty`.

- `BusinessErrorUtility_GetMessage_returns_empty_string_when_language_list_is_empty`
  - Given a message with `LanguageList` empty, when calling `GetMessage`, then it returns `string.Empty`.

- `BusinessErrorUtility_GetMessage_returns_empty_string_when_locale_is_not_found`
  - Given a message with `LanguageList` entries for other locales, when calling `GetMessage`, then it returns `string.Empty`.

- `BusinessErrorUtility_GetMessage_returns_description_when_locale_matches_case_insensitive`
  - Given `requestor.LocaleCode = en-US` and a message language entry with `LocaleID = EN-us` and `Description = Saved`, when calling `GetMessage`, then it returns `Saved`.

### SearchModeOption

- `SearchModeOption_constructor_sets_FilterType_EqualTo`
  - Given `new SearchModeOption()`, then `FilterType == SearchModeOption.SearchMode.EqualTo`.

### SearchModeHelper

- `SearchModeHelper_GetSearchModeValue_returns_EqualTo_when_options_null`
  - Given `searchOptions = null`, when calling `GetSearchModeValue`, then it returns `EqualTo`.

- `SearchModeHelper_GetSearchModeValue_returns_EqualTo_when_no_match`
  - Given a non-empty list with no option where `FieldName == fieldName`, when calling `GetSearchModeValue`, then it returns `EqualTo`.

- `SearchModeHelper_GetSearchModeValue_returns_configured_filter_type_when_match_exists`
  - Given an option where `FieldName == fieldName` and `FilterType = StartsWith`, when calling `GetSearchModeValue`, then it returns `StartsWith`.

- `SearchModeHelper_GetSearchMode_returns_enum_name_as_string`
  - Given a match with `FilterType = EndsWith`, when calling `GetSearchMode`, then it returns `EndsWith`.

## Contract tests

- `openapi_yaml_is_minimal_and_marked_no_http_surface`
  - Assert `x-no-http-surface: true`.
  - Assert `paths` is `{}`.

- `contract_declares_no_cross_component_dependencies`
  - Assert `technical/components/shared-contracts-utilities/CONTRACT.md` indicates no cross-component `Uses`.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution > Components > 1. OpenAPI Specification Document` (OpenAPI artifacts exist even when a component has no HTTP surface; schema remains valid and versioned)

