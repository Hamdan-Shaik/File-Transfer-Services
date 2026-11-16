---
type: Test Plan
title: technical/components/backend-providers-file-transfer/TESTS
description: The tests that prove the backend-providers-file-transfer component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-providers-file-transfer
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T20:52:42Z
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
  resource: /technical/components/backend-providers-file-transfer/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-providers-file-transfer
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 4
  methods: 73
  owned_rules: 15
  flows: []
  depends_on: []
  deployable: fxfer-bff
  folder: src/Application/Providers
  body_sha256: 659097fffabaa41cd898ef302d60c9fd38a781f84d8483debac3d73a3fe897b9
---

﻿# Tests: `backend-providers-file-transfer`

## Owned rule tests

- `F01_027_removes_hyphens_from_destination_contact_phone`
  - Given a `TransferConfigurationUpsertRequest` with one destination where `contactPhoneNumber = "555-111-2222"`
  - When `FileTransferCommandProvider.AddFileTransferBaseConfigurationAsync` maps the request into the downstream DTO
  - Then the downstream destination payload uses `contactPhoneNumber = "5551112222"`

- `F01_028_file_to_event_overwrites_destination_address_with_external_fte_common_folder`
  - Given `ExternalFTECommonFolder = "\\\\server\\share\\fte"`
  - And a destination with `destinationType = FileToEvent` and an arbitrary `destinationAddress`
  - When the provider maps the destination for create/update
  - Then the mapped destination address equals the configured `ExternalFTECommonFolder`

- `F01_029_destination_ftp_server_id_empty_string_maps_to_guid_empty`
  - Given a destination with an FTP server id value represented as an empty string
  - When the provider maps the destination
  - Then the downstream payload uses `ftpServerId = 00000000-0000-0000-0000-000000000000`

- `F01_030_empty_run_on_this_day_maps_to_byte_zero`
  - Given a create/update request where the run-on-this-day value is missing/empty
  - When the provider maps the schedule fields
  - Then the downstream payload uses `runOnThisDayOfMonth = 0`

- `F01_031_sets_is_suspended_changed_date_only_when_suspended`
  - Given a request where `suspended = true`
  - When the provider maps the command
  - Then the downstream payload includes `isSuspendedChangedDateUtc` set to the current UTC time (assert within a tolerance window)
  - And given a request where `suspended = false`
  - When the provider maps the command
  - Then the downstream payload uses `isSuspendedChangedDateUtc = null`

- `F01_032_calls_domain_operation_for_add_transfer_configuration`
  - Given a valid upsert request with destinations
  - When `AddFileTransferBaseConfigurationAsync` is called
  - Then `FxferDomainApiClient` is invoked with the expected create configuration operation and payload

- `F01_033_propagates_downstream_faults_for_problem_details_mapping`
  - Given `FxferDomainApiClient` throws a downstream exception (e.g. non-success status)
  - When any command provider method is called
  - Then the exception propagates (or is converted to the deployable-standard provider exception) without returning a partial success result

- `F02_051_add_ftp_server_assigns_new_id_and_uses_requestor_operator_id`
  - Given an `FtpServerUpsertRequest` and an authenticated operator id in request context
  - When `AddFtpServerAsync` is called
  - Then the downstream create request includes a newly generated FTP server id
  - And the downstream request includes the operator id from the requestor context

- `F02_052_add_ftp_server_non_null_response_maps_id_and_preserves_editor`
  - Given the downstream create call returns a response containing `ftpServerId`
  - When `AddFtpServerAsync` returns
  - Then the returned `FtpServer` contains the same `ftpServerId`
  - And the provider-level result preserves the request editor metadata when the BFF contract includes it

- `F02_056_update_ftp_server_reuses_existing_password_when_omitted`
  - Given an update request with `ftpPassword` omitted/empty
  - And the provider queries existing credentials with `removePassword=false` and receives a non-empty list
  - When `UpdateFtpServerAsync` is called
  - Then the downstream update request uses the fetched existing password value

- `F02_058_update_ftp_server_non_null_response_maps_id_and_is_valid`
  - Given the downstream update call returns `ftpServerId` and `isValid`
  - When `UpdateFtpServerAsync` returns
  - Then the returned `FtpServer` contains those values

- `F02_059_invalidate_ftp_server_non_null_response_maps_id_and_is_valid`
  - Given the downstream invalidate call returns `ftpServerId` and `isValid`
  - When `InvalidateFtpServerAsync` returns
  - Then the returned `FtpServer` contains those values

- `F02_060_invalidate_builds_command_by_parsing_ftp_server_id_as_guid`
  - Given `ftpServerId = "{guid}"`
  - When `InvalidateFtpServerAsync` is called
  - Then the downstream request uses that GUID value (parse required)

- `F02_063_password_lookup_ok_with_empty_list_throws_unhandled_exception`
  - Given the provider performs the existing-password lookup
  - And the lookup returns an OK status with an empty list
  - When `UpdateFtpServerAsync` attempts to reuse the password
  - Then the method throws an unhandled exception (legacy parity)

- `F05_035_tracking_search_maps_only_supported_filters_and_parses_date_and_size`
  - Given a file-tracking search request containing:
    - supported fields (`fileId`, `fileName`, `fileStatus`, `direction`, `transportType`, `tradingPartner`, `sourceLocationId`)
    - `fromDate`/`toDate` strings that are and are not parseable
    - `fromSize`/`toSize` strings that are and are not parseable
    - unsupported field names
  - When `SearchFileTransferTrackingAsync` builds downstream query parameters
  - Then only supported filters are sent
  - And date/size filters are included only when parsing succeeds

## Target-state requirement tests

- `T_008_bff_provider_has_no_direct_database_access`
  - Validate (static/architecture test) that `services/fxfer-bff/src/Application/Providers` has no references to database client libraries/ORM contexts and only depends on the downstream API client(s).

## Contract tests

- Validate `technical/components/backend-providers-file-transfer/openapi.yaml` parses as OpenAPI 3.x and includes:
  - `paths: {}`
  - `x-no-http-surface: true`

- Validate `technical/components/backend-providers-file-transfer/CONTRACT.md`:
  - lists the offered provider interfaces and methods
  - lists downstream usage of `fxfer-domain-api`
  - lists no component-to-component dependencies
