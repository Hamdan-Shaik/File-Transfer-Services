---
type: Test Plan
title: 'Tests: shared-contracts-commands'
description: The tests that prove the shared-contracts-commands component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-commands
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:20:12Z
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
  resource: /technical/components/shared-contracts-commands/INVENTORY.yaml
  title: Every legacy class and method (12 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-commands
  generation:
    status: done
    attempts: 2
  tier: shared
  classes: 12
  methods: 36
  owned_rules: 27
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  depends_on:
  - shared-contracts-utilities
  deployable: fxfer-domain-api
  folder: src/Contracts/Commands
  body_sha256: c2da04cfdaf3a1a5b20af5ab56235bbba07b4c3af1d0aa9a7417451e62b2246c
---

# Tests: `shared-contracts-commands`

This component owns validation-centric business rules that are enforced by command DTO validation methods.
Tests focus on rule-level validation parity (messages, exception types, and validation entry shapes) and basic contract checks for `openapi.yaml` and `CONTRACT.md`.

## Unit tests (owned rules)

### Configure source locations and destinations (F01)

- `F01_038_FtsCommandValidate_resets_validations_list`
  - Given an `FtsCommand` with a non-empty `Validations` list, when calling `FtsCommandValidate`, then `Validations` becomes an empty list.

- `F01_039_HandleValidations_throws_BusinessException_when_any_validation_exists`
  - Given an `FtsCommand` with one validation entry, when calling `HandleValidations`, then it throws `BusinessException` with message `Command Execution Error` and code `E`.

- `F01_040_AddValidation_uses_same_key_for_field_and_key`
  - Given `AddValidation(keyName, message)`, then the appended `BusinessExceptionMessage` has both field and key equal to `keyName`.

- `F01_041_AddSourceLocation_records_missing_field_for_each_required_string`
  - Given a `FtsSourceLocationAddCommand` where each required string field is empty, when calling `Validate`, then `Validations` contains an entry for each required field with message `Missing field`.

- `F01_042_AddSourceLocation_requires_at_least_one_valid_destination`
  - Given `DestinationAddress` null, empty, or only invalid entries, when calling `Validate`, then `Validations` contains a `DestinationAddress` entry with message `Missing field`.

- `F01_043_AddSourceLocation_requires_run_schedule_selection`
  - Given all schedule flags false and `RunOnThisDayOfMonth == 0`, when calling `Validate`, then `Validations` contains a `Run Schedule` entry with message `Invalid value for`.

- `F01_044_AddSourceLocation_requires_FtpServerId_when_protocol_is_ftp`
  - Given `Protocol` is FTP server code (1) and `FtpServerId` is empty, when calling `Validate`, then it throws `ArgumentNullException` for `FtpServerId`.

- `F01_045_AddSourceLocation_rejects_excluded_mask_extensions_case_insensitive`
  - Given `Mask` ends with an excluded extension (for example `FILE.EXE`), when calling `Validate`, then `Validations` contains a `Mask` entry with message `Source file mask extension is not Allowed: exe`.

- `F01_059_UpdateSourceLocation_missing_required_strings_fail_validation_with_BusinessException`
  - Given a `FtsSourceLocationUpdateCommand` with missing required strings, when calling `Validate`, then it throws `BusinessException` with message `Command Execution Error` and code `E`.

- `F01_060_UpdateSourceLocation_requires_run_schedule_selection`
  - Given all schedule flags false and `RunOnThisDayOfMonth == 0`, when calling `Validate`, then `Validations` includes `Run Schedule` with message `Invalid value for`.

- `F01_061_UpdateSourceLocation_requires_FtpServerId_when_protocol_is_ftp`
  - Given `Protocol` equals FTP server code (1) and `FtpServerId` is empty, when calling `Validate`, then it throws `ArgumentNullException` for `FtpServerId`.

- `F01_062_UpdateSourceLocation_requires_FtsSourceLocationId`
  - Given `FtsSourceLocationId` empty, when calling `Validate`, then it throws `ArgumentNullException` for `FtpSoureceLocationId`.

- `F01_063_UpdateSourceLocation_requires_suspended_changed_date_when_suspended`
  - Given `IsSuspended == true` and `IsSuspendedChangedDate == null`, when calling `Validate`, then it throws `ArgumentNullException` for `IsSuspendedChangedDate`.

- `F01_064_UpdateSourceLocation_requires_nonzero_ConfigId`
  - Given `ConfigId == 0`, when calling `Validate`, then it throws `ArgumentNullException` for `ConfigIdNotFound`.

- `F01_065_UpdateSourceLocation_rejects_excluded_mask_extensions`
  - Given `Mask` ends with an excluded extension, when calling `Validate`, then `Validations` contains a `Mask` entry with message `Source file mask extension is not Allowed: <ext>`.

- `F01_066_InvalidateSourceLocation_requires_OperatorId`
  - Given `OperatorId` empty, when calling `Validate`, then it throws `ArgumentNullException` for `OperatorId`.

- `F01_067_InvalidateSourceLocation_requires_FtsSourceLocationId`
  - Given `FtsSourceLocationId` empty, when calling `Validate`, then it throws `ArgumentNullException` for `FtpSoureceLocationId`.

- `F01_068_InvalidateSourceLocation_requires_nonzero_ConfigId`
  - Given `ConfigId == 0`, when calling `Validate`, then it throws `ArgumentNullException` for `ConfigIdNotFound`.

### Manage FTP servers and credentials (F02)

- `F02_024_UpdateFtpServerCommand_inherits_add_validation_rules`
  - Given `UpdateFtpServerCommand` inherits `AddFtpServerCommand`, when calling `Validate` on an update instance with missing required strings, then required-field validations match add behavior.

- `F02_025_AddFtpServer_required_strings_are_validated_as_missing_fields`
  - Given required string properties are empty, when calling `Validate`, then one validation entry is added per missing property.

- `F02_026_AddFtpServer_missing_field_message_includes_property_name`
  - Given a missing required string property (for example `FtpServerName`), when calling `Validate`, then the message is `Missing field: FtpServerName`.

- `F02_027_FileTransferCommand_throws_BusinessException_when_any_validation_exists`
  - Given any validation exists after validation runs, when calling `Validate`, then it throws `BusinessException` with message `Command Execution Error` and code `E`.

- `F02_028_AddFtpServer_rejects_empty_guid_ftp_server_id`
  - Given `FtpServerId == Guid.Empty`, when calling `Validate`, then it adds a validation entry for `FtpServerId`.

- `F02_029_AddFtpServer_validates_ip_against_legacy_regex`
  - Given `FtpServerIp` non-empty and not matching the legacy regex, when calling `Validate`, then it adds a validation entry with message `IpAddressInvalidFormat`.

- `F02_030_AddFtpServer_requires_proxy_port_when_proxy_address_is_provided`
  - Given `FtpProxyAddress` non-empty and `FtpProxyPort` null or `<= 0`, when calling `Validate`, then it adds a validation entry for `FtpProxyPort`.

- `F02_062_AddFtpServer_does_not_validate_ftp_port_greater_than_zero`
  - Given `FtpPort == 0` with otherwise valid input, when calling `Validate`, then it does not add a validation entry for `FtpPort`.

### Manage integration files (F03)

- `F03_010_UpdateIntegrationFile_requires_ConfigId`
  - Given `ConfigId == null`, when calling `Validate`, then it throws `BusinessException` containing a single validation entry for `ConfigId` with message `Missing ConfigId`.

## Unit tests (target-state requirement)

- `T_004_IdempotencyKey_rejects_reuse_with_different_payload`
  - Given two submissions with the same `Idempotency-Key` and different payloads, then the second submission fails as a client error.

## Contract tests

- `openapi_yaml_is_minimal_and_marked_no_http_surface`
  - Assert `x-no-http-surface: true`.
  - Assert `paths` is `{}`.

- `contract_lists_cross_component_dependencies`
  - Assert `technical/components/shared-contracts-commands/CONTRACT.md` lists the dependent components used by the command DTOs.

## Enterprise-Architecture standards applied

- `standards/application/api/STD-APP-API-001-api-design.md` section `5.1 Automated Testing` (contract-first artifacts and automated tests prevent contract drift)

