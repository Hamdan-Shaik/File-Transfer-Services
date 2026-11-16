---
type: Test Plan
title: technical/components/backend-business-command-handlers/TESTS
description: The tests that prove the backend-business-command-handlers component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-command-handlers
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:59:42Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-manage-integration-files
  resource: /technical/flows/manage-integration-files/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-business-command-handlers/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-command-handlers
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 25
  owned_rules: 12
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  depends_on:
  - backend-business-command-handlers-2
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - shared-contracts-commands
  deployable: fxfer-domain-api
  folder: src/Application/Commands
  body_sha256: c1c813ea81178dd52052fb41657e9e647c0f305141bc7397623da7bd1d377f23
---

﻿# Tests: backend-business-command-handlers

Tests cover rule-owned command semantics, no-op behavior, and the component contract artifacts.

## Contract tests

- openapi_no_http_surface_is_declared
  - Given `technical/components/backend-business-command-handlers/openapi.yaml`
  - Then `x-no-http-surface` is `true`
  - And `paths` is empty (`{}`)

- contract_names_expected_offers_and_dependencies
  - Given `technical/components/backend-business-command-handlers/CONTRACT.md`
  - Then it lists the provided MediatR command handlers and `TenantAuthorizationBehavior`
  - And it names the component dependencies from the brief under `Uses`

## Rule tests

- F01_046_allocates_config_id_from_FTS_SOURCE_LOCATION_SEQ
  - Given an `FtsSourceLocationAddCommand` with representative fields
  - When `AddFtsSourceLocationCommandHandler` handles the command
  - Then the persisted configuration uses the next value from sequence `FTS_SOURCE_LOCATION_SEQ`
  - And the emitted configuration-change event references the created configuration aggregate

- F01_047_add_copies_fields_and_sets_operator_id_from_requestor
  - Given an `FtsSourceLocationAddCommand` and `Requestor.IdentifierId = <operator>`
  - When handled
  - Then the created configuration fields match the command values
  - And the configuration-change event `operator_id` equals `<operator>`

- F01_069_update_missing_config_id_throws_business_exception_shape
  - Given an `FtsSourceLocationUpdateCommand` with `ConfigId = 123` and no stored source location for that ConfigId
  - When `UpdateFtsSourceLocationCommandHandler` handles the command
  - Then it throws a `BusinessException` with message `Business Error occurred processing the command.`
  - And it contains exactly one validation entry (`keyName = ConfigId`, `message = ConfigIdNotFound`, `processingSystem = FileTransfer`)

- F01_070_invalidate_missing_config_id_throws_business_exception_shape
  - Given an `FtsSourceLocationInvalidateCommand` with `ConfigId = 123` and no stored source location for that ConfigId
  - When `InvalidateFtsSourceLocationCommandHandler` handles the command
  - Then it throws the same `BusinessException` shape as F01-069

- F01_071_update_uses_stored_source_location_id_and_stored_config_id
  - Given an existing stored source location with `ConfigId = 123` and `SourceLocationId = <id>`
  - And an `FtsSourceLocationUpdateCommand` with `ConfigId = 123` and updated fields
  - When handled
  - Then the updated configuration uses the stored source location id
  - And the updated configuration uses `ConfigId = 123`

- F01_072_invalidate_uses_stored_source_location_id_and_copies_operator_id
  - Given an existing stored source location with `ConfigId = 123` and `SourceLocationId = <id>`
  - And an `FtsSourceLocationInvalidateCommand` with `OperatorId = <operator>` and destinations
  - When handled
  - Then the invalidation targets the stored source location id
  - And the emitted configuration-change event `operator_id` equals `<operator>`

- F02_023_add_ftp_server_validates_and_fails_on_validations
  - Given an `AddFtpServerCommand` that violates required-field validation
  - When `AddFtpServerCommandHandler` handles the command
  - Then it fails the request with validation errors (legacy parity: command validation failure)

- F02_031_duplicate_ftp_server_name_is_rejected
  - Given an existing valid FTP server with `FtpServerName = "X"` and id `<existingId>`
  - And an `AddFtpServerCommand` with `FtpServerName = "X"` and id `<differentId>`
  - When handled
  - Then it fails with the legacy duplicated-name validation

- F02_032_add_ftp_server_emits_created_change_event_with_operator_id
  - Given an `AddFtpServerCommand` with `Requestor.IdentifierId = <operator>`
  - When handled successfully
  - Then the emitted event has `aggregate_type = ftp_server`
  - And `change_kind = created`
  - And `operator_id = <operator>`

- F02_033_update_ftp_server_emits_updated_change_event_with_operator_id
  - Given an `UpdateFtpServerCommand` with `Requestor.IdentifierId = <operator>`
  - When handled successfully
  - Then the emitted event has `aggregate_type = ftp_server`
  - And `change_kind = updated`
  - And `operator_id = <operator>`

- F02_034_invalidate_ftp_server_skips_validation_and_emits_invalidated
  - Given an `InvalidateFtpServerCommand` with missing/invalid optional fields
  - When handled
  - Then it does not run `Validate()`
  - And it emits a change event with `change_kind = invalidated` and `operator_id` from the requestor

- F03_011_update_integration_file_emits_updated_change_event
  - Given an `UpdateIntegrationFileCommand` with `FtsIntegrationFileId`, `FileName`, `ConfigId`, `IsValid`, and `Requestor.IdentifierId = <operator>`
  - When handled
  - Then it emits a change event with `aggregate_type = integration_file`, `change_kind = updated`, and `operator_id = <operator>`

## Target-state requirement tests

- T_003_enforces_tenant_scoped_authorization_for_commands
  - Given a command request with no resolved `tenant_id`
  - When any command handler in this component is invoked
  - Then the request is rejected as an authorization failure
  - And no write-model mutation is performed

## Enterprise-Architecture standards applied

- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Unit Test: Command Handler` (command handler behavior is covered by targeted unit tests).
- `standards/security/authorization/STD-SEC-AUTHZ-001-authorization-implementation.md` section `3.4 Multi-Tenant Authorization` (tests cover tenant isolation failures).
