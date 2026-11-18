---
type: Behavior Specification
title: 'shared-queries-parameters-2: legacy integration-file query contract and parameter types'
description: 'What the shared-queries-parameters-2 component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-queries-parameters-2
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:47:34Z
sources:
- id: inventory
  resource: /technical/components/shared-queries-parameters-2/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-queries-parameters-2
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 3
  methods: 2
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Queries
  body_sha256: 2c13177752cfe6723e54041a82176447c3b028ca57cc1d71e97221c76536e5b4
---

# shared-queries-parameters-2: legacy integration-file query contract and parameter types

This component defines the legacy query contract and parameter objects used for integration-file lookup.
These types are consumed by query execution code as read-only request DTOs that carry Where filter criteria.

## HPP.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.GroupInfo
Group record summary information.

- Properties:
  - GroupId (Guid?): system-generated unique identifier for the group record.
  - NumberOfRecords (int): number of outbound records in an extract file.
- Data: none.
- Errors: none.
- Rules: none.

## HPP.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.GetIntegrationFileQuery
Integration file lookup query.

- Constructors:
  - GetIntegrationFileQuery(): initializes Where = new IntegrationFileQueryParms().
- Properties:
  - Where (IntegrationFileQueryParms): filter criteria for integration-file lookup.

### Validate()
- Purpose: perform request validation for the integration-file lookup query.
- Inputs: none (uses Where).
- Logic:
  - Assign messages = new List<BusinessExceptionMessage>().
  - Call HandleErrors().
  - Since messages is always empty in this type, validation completes without throwing.
- Data: none.
- Errors:
  - If messages.Count > 0, throws BusinessException(messages, CommonConstants.Validation.ErrorType.InvalidParameters, CommonConstants.Validation.ProcessingSystem).
- Rules: none.

### HandleErrors()
- Purpose: raise a BusinessException when validation has accumulated error messages.
- Inputs: none (uses messages).
- Logic:
  - If messages.Count > 0, throw BusinessException(...).
- Data: none.
- Errors: throws BusinessException when messages.Count > 0.
- Rules: none.

## HPP.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.Parameters.IntegrationFileQueryParms
Integration file lookup query filter criteria.

- Constructors:
  - IntegrationFileQueryParms(): sets FTSIntegrationFileId = Guid.Empty.
- Properties:
  - FTSIntegrationFileId (Guid): key identifying an integration file record.
  - FileName (string): file name filter.
- Data: none.
- Errors: none.
- Rules: none.

## Rules implemented

None (this component owns no business rules).

## Not carried over

None.

## Enterprise-Architecture standards applied

- patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md section Query Side Components (queries and their DTOs are part of the read-only query path and must not introduce side effects).
- standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md section Recommended Requirements (query contracts follow query naming conventions and are handled on the read/query side).
- patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md section Solution Overview (validation failures are represented consistently so the HTTP layer can surface standardized Problem Details responses).
- standards/application/api/STD-APP-API-001-api-design.md section 3.5 Error Handling (validation failures are surfaced via standard, machine-readable error responses at the API boundary).

