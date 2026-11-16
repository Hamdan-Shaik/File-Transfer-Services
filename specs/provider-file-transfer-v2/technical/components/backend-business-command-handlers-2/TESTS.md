---
type: Test Plan
title: 'Tests: backend-business-command-handlers-2'
description: The tests that prove the backend-business-command-handlers-2 component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-command-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T06:11:41Z
sources:
- id: inventory
  resource: /technical/components/backend-business-command-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-command-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 25
  owned_rules: 0
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  depends_on:
  - backend-business-command-handlers
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - shared-contracts-commands
  deployable: fxfer-domain-api
  folder: src/Application/Commands
  body_sha256: a9bf29a42999a2285fc4dd7893640737f57ba5dd40d1adfdf50165999589dc1b
---

# Tests: backend-business-command-handlers-2

Tests cover the legacy-parity command semantics described in `technical/components/backend-business-command-handlers-2/SPEC.md` and the component contract artifacts.

Because this component is an alternate legacy implementation that is merged into the canonical target command handler set (component `technical/components/backend-business-command-handlers/`), these tests are implemented once against the target command handlers and are treated as covering both legacy variants.

## Contract tests

- openapi_no_http_surface_is_declared
  - Given `technical/components/backend-business-command-handlers-2/openapi.yaml`
  - Then `x-no-http-surface` is `true`
  - And `paths` is empty (`{}`)

- contract_names_expected_dependencies
  - Given `technical/components/backend-business-command-handlers-2/CONTRACT.md`
  - Then it names the component dependencies from the brief under `Uses`
  - And it lists the legacy package dependencies (`automapper`, `hpp.core.bas.cqrs`)

## Method tests

### FTP server handlers

- add_ftp_server_duplicate_name_is_rejected
  - Given an existing valid FTP server with `FtpServerName = "X"` and id `<existingId>`
  - And an `AddFtpServerCommand` with `FtpServerName = "X"` and id `<differentId>`
  - When handled
  - Then it fails with the legacy duplicated-name validation message `Duplicated FTP Server Name`

- invalidate_ftp_server_skips_validation
  - Given an `InvalidateFtpServerCommand` that would fail `Validate()` if validation were executed
  - When handled
  - Then it does not invoke command validation
  - And it performs the invalidation mutation

### Inbound target handlers

- add_inbound_target_is_no_op_when_row_exists
  - Given an existing inbound target row for `FTEMetadataId = <id>`
  - When `AddFtsInboundTargetCommand` is handled
  - Then no inbound target mutation is performed
  - And no configuration-change event is emitted

- invalidate_inbound_target_is_no_op_when_row_is_missing
  - Given no inbound target row exists for `FTEMetadataId = <id>`
  - When `InvalidateFtsInboundTargetCommand` is handled
  - Then no inbound target mutation is performed
  - And no configuration-change event is emitted

### Integration file handlers

- add_integration_file_is_no_op_when_group_info_is_missing_or_zero
  - Given an `AddIntegrationFileCommand` with `GroupInfo = null` (or with no entry satisfying `NumberOfRecords > 0` and `GroupId != null`)
  - When handled
  - Then no integration file rows are created
  - And no configuration-change event is emitted

- update_integration_file_validates_and_emits_update
  - Given an `UpdateIntegrationFileCommand` with `FtsIntegrationFileId`, `FileName`, `ConfigId`, `IsValid`, and `Requestor.IdentifierId = <operator>`
  - When handled
  - Then it validates the command
  - And it performs the update mutation
  - And it emits a configuration-change event describing the update

### Source location handlers

- add_source_location_allocates_config_id_from_FTS_SOURCE_LOCATION_SEQ
  - Given an `FtsSourceLocationAddCommand` with representative fields
  - When handled
  - Then the persisted configuration uses the next value from sequence `FTS_SOURCE_LOCATION_SEQ`

- update_missing_config_id_throws_business_exception_shape
  - Given an `FtsSourceLocationUpdateCommand` with `ConfigId = 123` and no stored source location for that ConfigId
  - When handled
  - Then it throws a `BusinessException` with message `Business Error occurred processing the command.`
  - And it contains exactly one validation entry (`keyName = ConfigId`, `message = ConfigIdNotFound`, `processingSystem = FileTransfer`)

- invalidate_missing_config_id_throws_business_exception_shape
  - Given an `FtsSourceLocationInvalidateCommand` with `ConfigId = 123` and no stored source location for that ConfigId
  - When handled
  - Then it throws the same `BusinessException` shape as the update handler

## Rule tests

None (this component owns no business rules; see `technical/components/backend-business-command-handlers-2/COMPONENT-BRIEF.md`).

## Enterprise-Architecture standards applied

- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Unit Test: Command Handler` (command handler behavior is covered by targeted unit tests).
- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Command Flow (Write Path)` (tests validate write-path validation/no-op behaviors).