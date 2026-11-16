---
type: Test Plan
title: technical/components/backend-data-access-query-handlers-2/TESTS
description: The tests that prove the backend-data-access-query-handlers-2 component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-query-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T01:10:43Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-query-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-query-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 8
  methods: 38
  owned_rules: 0
  flows: []
  depends_on:
  - shared-contracts-queries
  - shared-queries-parameters-2
  deployable: fxfer-domain-api
  folder: src/Application/Queries
  body_sha256: f9c3bf9a583465da8300fe091d125d6d3f9c2f0da73c316b7a04556d6e05236b
---

﻿# Tests: backend-data-access-query-handlers-2

This component owns no business rules. Tests cover query executor behavior and contract/doc consistency.

## Rule tests

None (no owned rules).

## Unit tests

### FileEventQueryExecutor

- `FileEventQueryExecutor_execute_applies_default_sort_EventId_then_EventCode`
- `FileEventQueryExecutor_execute_supports_sort_key_EVENT_ID`
- `FileEventQueryExecutor_execute_filters_by_FileId_when_Where_FileId_is_not_null`
- `FileEventQueryExecutor_execute_uses_Guid_Empty_when_Where_FileId_is_invalid_guid_string`
- `FileEventQueryExecutor_execute_throws_when_query_Where_is_null`

### FileExceptionQueryExecutor

- `FileExceptionQueryExecutor_execute_applies_default_sort_ExceptionId_then_ExceptionType`
- `FileExceptionQueryExecutor_execute_supports_sort_key_EXCEPTION_ID`
- `FileExceptionQueryExecutor_execute_filters_by_FileId_when_Where_FileId_is_not_null`
- `FileExceptionQueryExecutor_execute_uses_Guid_Empty_when_Where_FileId_is_invalid_guid_string`
- `FileExceptionQueryExecutor_execute_throws_when_query_Where_is_null`

### FileTransferTrackingQueryExecutor

- `FileTransferTrackingQueryExecutor_execute_unions_unc_and_ftp_lists_before_filtering`
- `FileTransferTrackingQueryExecutor_execute_applies_default_sort_FileName_ascending`
- `FileTransferTrackingQueryExecutor_execute_filters_by_FileId_with_Guid_TryParse_semantics`
- `FileTransferTrackingQueryExecutor_execute_filters_by_FileSourceLocationId_with_Guid_TryParse_semantics`
- `FileTransferTrackingQueryExecutor_execute_filters_by_dates_inclusive_bounds`
- `FileTransferTrackingQueryExecutor_execute_filters_by_file_size_inclusive_bounds_when_bounds_greater_than_zero`
- `FileTransferTrackingQueryExecutor_execute_enriches_each_returned_row_with_events_and_exceptions`
- `FileTransferTrackingQueryExecutor_execute_throws_when_query_Where_is_null`

### GetFtpServerQueryExecutor

- `GetFtpServerQueryExecutor_execute_filters_by_IsValid_when_IsValid_is_not_null`
- `GetFtpServerQueryExecutor_execute_filters_by_FtpServerName_case_insensitive_via_ToUpper`
- `GetFtpServerQueryExecutor_execute_does_not_apply_explicit_sorting`
- `GetFtpServerQueryExecutor_execute_throws_when_query_Where_is_null`

### GetSourceLocationQueryExecutor

- `GetSourceLocationQueryExecutor_execute_calls_query_Validate`
- `GetSourceLocationQueryExecutor_execute_loads_all_destinations_and_inbound_targets_before_mapping`
- `GetSourceLocationQueryExecutor_execute_returns_single_SourceLocationModel_when_exactly_one_match_exists`
- `GetSourceLocationQueryExecutor_execute_returns_empty_list_when_zero_or_multiple_matches_exist_then_throws_on_First_in_assignment`
- `GetSourceLocationQueryExecutor_execute_assigns_DestinationAddress_only_on_first_result`
- `GetSourceLocationQueryExecutor_execute_orders_destinations_by_CreationDate_descending`
- `GetSourceLocationQueryExecutor_execute_throws_NullReference_when_inbound_target_missing_for_FTEMetadataId`

### InboundTargetQueryExecutor

- `InboundTargetQueryExecutor_execute_filters_by_FTEMetadataId_when_not_empty`
- `InboundTargetQueryExecutor_execute_filters_by_ContractName_when_not_empty`
- `InboundTargetQueryExecutor_execute_filters_to_IsValid_true_only`
- `InboundTargetQueryExecutor_execute_deduplicates_by_FTEMetadataId_choosing_lowest_FtsInboundTargetId`

### IntegrationFileQueryExecutor

- `IntegrationFileQueryExecutor_execute_filters_by_FTSIntegrationFileId_when_not_empty`
- `IntegrationFileQueryExecutor_execute_filters_by_FileName_substring_when_not_empty`
- `IntegrationFileQueryExecutor_execute_filters_to_IsValid_true_only`

### SearchSourceLocationQueryExecutor

- `SearchSourceLocationQueryExecutor_execute_applies_default_sort_ConfigId_asc_then_IsActive_desc`
- `SearchSourceLocationQueryExecutor_execute_filters_by_ConfigId_when_greater_than_zero`
- `SearchSourceLocationQueryExecutor_execute_filters_by_OwnerName_and_ConfigName_case_insensitive_via_ToUpper`
- `SearchSourceLocationQueryExecutor_execute_filters_by_OwnerSubsystem_using_OwnerModuleGuid_condition`
- `SearchSourceLocationQueryExecutor_execute_filters_by_Protocol_when_not_empty`
- `SearchSourceLocationQueryExecutor_execute_filters_by_IsActive_only_when_IsActive_is_true`
- `SearchSourceLocationQueryExecutor_execute_throws_when_query_Where_is_null`

## Contract and spec consistency tests

- `openapi_has_no_http_surface_flag`
  - Assert `technical/components/backend-data-access-query-handlers-2/openapi.yaml` contains `x-no-http-surface: true` and `paths: {}`.
- `contract_lists_declared_component_dependencies`
  - Assert `technical/components/backend-data-access-query-handlers-2/CONTRACT.md` lists `shared-contracts-queries` and `shared-queries-parameters-2` under Uses.
- `inventory_coverage`
  - Assert every inventory class/method in `technical/components/backend-data-access-query-handlers-2/INVENTORY.yaml` appears in `technical/components/backend-data-access-query-handlers-2/SPEC.md` and has a row in the mapping table in `technical/components/backend-data-access-query-handlers-2/TARGET.md`.
