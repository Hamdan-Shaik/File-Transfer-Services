---
type: Test Plan
title: 'Tests: backend-configure-source-locations-and-destinations-api'
description: The tests that prove the backend-configure-source-locations-and-destinations-api component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-configure-source-locations-and-destinations-api
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T06:28:48Z
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
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-configure-source-locations-and-destinations-api/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-configure-source-locations-and-destinations-api
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 49
  owned_rules: 22
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  - search-and-track-file-transfers
  depends_on:
  - backend-bas-file-transfer-api
  - backend-bas-file-transfer-svc
  - backend-bas-file-transfer-svc-2
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  deployable: fxfer-domain-api
  folder: src/Api
  body_sha256: 7db73d72cf4b0a5e371d39b47ec0944ded5ae73a9a9b1464d94d8bff8496958f
---

# Tests: `backend-configure-source-locations-and-destinations-api`

This component owns the API endpoints for configuration and tracking; its tests focus on the owned business rules (routing/parity semantics) plus the OpenAPI/contract artifacts.

## Rules

### F01-034

- `F01_034_create_transfer_configuration_routes_command_and_returns_result`
  - Given a valid transfer configuration create request
  - When `POST /api/fxfer-domain/v1/transfer-configurations` is called
  - Then the endpoint delegates to the command handler and returns the handler result

### F01-035

- `F01_035_api_hosted_variant_is_merged_into_single_domain_endpoint`
  - Given the target API surface
  - Then there is only one `POST /transfer-configurations` endpoint implementing the legacy API-hosted variant behavior

### F01-036

- `F01_036_core_variant_is_merged_into_single_domain_endpoint`
  - Given the target API surface
  - Then there is only one `POST /transfer-configurations` endpoint implementing the legacy core-service variant behavior

### F01-055

- `F01_055_update_transfer_configuration_routes_command_and_returns_result`
  - Given an existing transfer configuration id and an update request
  - When `PUT /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}` is called
  - Then the endpoint delegates to the command handler and returns the handler result

### F01-057

- `F01_057_invalidate_transfer_configuration_routes_command_and_returns_result`
  - Given an existing transfer configuration id and a patch request that invalidates/disables the configuration
  - When `PATCH /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}` is called
  - Then the endpoint delegates to the command handler and returns the handler result

### F01-079

- `F01_079_search_transfer_configurations_routes_query_and_returns_result`
  - Given query parameters for searching configurations
  - When `GET /api/fxfer-domain/v1/transfer-configurations` is called
  - Then the endpoint delegates to the query handler and returns the handler result

### F01-081

- `F01_081_get_transfer_configuration_routes_query_and_returns_result`
  - Given an existing transfer configuration id
  - When `GET /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}` is called
  - Then the endpoint delegates to the query handler and returns the handler result

### F01-107

- `F01_107_projection_update_throws_on_missing_or_malformed_source_location_id`
  - Given a `FtsSourceLocationUpdated` event with `FtsSourceLocationsId` missing or not a GUID string
  - When the projection writer applies the event
  - Then it throws during GUID parsing and the update is not applied

### F01-108

- `F01_108_projection_invalidate_throws_on_missing_or_malformed_source_location_id`
  - Given a `FtsSourceLocationInvalidated` event with `FtsSourceLocationsId` missing or not a GUID string
  - When the projection writer applies the event
  - Then it throws during GUID parsing and the invalidation is not applied

### F02-001

- `F02_001_search_ftp_servers_routes_query_and_returns_result`
  - Given query parameters for FTP server search
  - When `GET /api/fxfer-domain/v1/ftp-servers` is called
  - Then the endpoint delegates to the query handler and returns the handler result

### F02-002

- `F02_002_api_hosted_variant_is_merged_into_single_domain_query_endpoint`
  - Given the target API surface
  - Then there is only one `GET /ftp-servers` query endpoint implementing the legacy API-hosted variant behavior

### F02-003

- `F02_003_core_variant_is_merged_into_single_domain_query_endpoint`
  - Given the target API surface
  - Then there is only one `GET /ftp-servers` query endpoint implementing the legacy core-service variant behavior

### F02-017

- `F02_017_create_ftp_server_routes_command_and_returns_result`
  - Given a valid FTP server create request
  - When `POST /api/fxfer-domain/v1/ftp-servers` is called
  - Then the endpoint delegates to the command handler and returns the handler result

### F02-019

- `F02_019_update_ftp_server_routes_command_and_returns_result`
  - Given an existing FTP server id and an update request
  - When `PUT /api/fxfer-domain/v1/ftp-servers/{ftpServerId}` is called
  - Then the endpoint delegates to the command handler and returns the handler result

### F02-021

- `F02_021_invalidate_ftp_server_routes_command_and_returns_result`
  - Given an existing FTP server id and a patch request that sets `isValid = false`
  - When `PATCH /api/fxfer-domain/v1/ftp-servers/{ftpServerId}` is called
  - Then the endpoint delegates to the command handler and returns the handler result

### F03-001

- `F03_001_list_integration_files_routes_query_and_returns_result`
  - Given query parameters for integration file definition lookup
  - When `GET /api/fxfer-domain/v1/integration-files` is called
  - Then the endpoint delegates to the query handler and returns the handler result

### F03-002

- `F03_002_api_hosted_variant_is_merged_into_single_domain_query_endpoint`
  - Given the target API surface
  - Then there is only one `GET /integration-files` query endpoint implementing the legacy API-hosted variant behavior

### F03-003

- `F03_003_patch_integration_file_routes_command_and_returns_result`
  - Given an existing integration file id and a patch request
  - When `PATCH /api/fxfer-domain/v1/integration-files/{integrationFileId}` is called
  - Then the endpoint delegates to the command handler and returns the handler result

### F03-004

- `F03_004_core_variant_is_merged_into_single_domain_command_endpoint`
  - Given the target API surface
  - Then there is only one `PATCH /integration-files/{integrationFileId}` command endpoint implementing the legacy core-service variant behavior

### F05-019

- `F05_019_search_file_tracking_routes_query_and_returns_result`
  - Given query parameters for file tracking search
  - When `GET /api/fxfer-domain/v1/file-tracking` is called
  - Then the endpoint delegates to the query handler and returns the handler result

### F05-021

- `F05_021_list_inbound_targets_routes_query_and_returns_result`
  - Given query parameters for inbound target search
  - When `GET /api/fxfer-domain/v1/inbound-targets` is called
  - Then the endpoint delegates to the query handler and returns the handler result

### F05-034

- `F05_034_inbound_target_query_validate_does_not_throw`
  - Given an `InboundTargetQuery`
  - When `Validate()` is called
  - Then it does not throw because no validation messages are added

## Contract tests

- `openapi_yaml_is_valid_openapi_document`
  - Validate `technical/components/backend-configure-source-locations-and-destinations-api/openapi.yaml` is syntactically valid OpenAPI 3.0.

- `openapi_yaml_contains_problem_details_response`
  - Validate the default response uses `application/problem+json` and references `ProblemDetails`.

- `contract_md_declares_expected_component_dependencies`
  - Validate `technical/components/backend-configure-source-locations-and-destinations-api/CONTRACT.md` lists all dependencies named in the brief.

