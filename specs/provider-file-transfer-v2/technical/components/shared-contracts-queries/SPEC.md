---
type: Behavior Specification
title: 'shared-contracts-queries: legacy FXFER query contract types'
description: 'What the shared-contracts-queries component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-queries
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:46:37Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-contracts-queries/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-queries
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 5
  owned_rules: 1
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Queries
  body_sha256: b17b4dae5f7a5d368bf68c3ab052c15ab383926075e728e726e1b109ef348f1e
---

# shared-contracts-queries: legacy FXFER query contract types

This component is the legacy set of WCF `DataContract` query types used by the File Transfer query service.
Each query carries a `Where` parameter object (filter criteria) and is executed by the legacy query layer against the FXFER projection/read tables.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.FileEventQuery
File-event search query.

- Properties:
  - `Where` (`FileEventQueryParms`): filter criteria for searching file events.
- Data:
  - Reads (via query execution): `FTS_FILE_MANAGEMENT`, `FTS_FILE_EVENT`.
- Errors: none in this contract type.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.FileExceptionQuery
File-exception search query.

- Properties:
  - `Where` (`FileExceptionQueryParms`): filter criteria for searching file exceptions.
- Data:
  - Reads (via query execution): `FTS_FILE_MANAGEMENT`, `FTS_FILE_EXCEPTION`.
- Errors: none in this contract type.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.FileTransferTrackingQuery
File transfer tracking search query.

- Properties:
  - `Where` (`FileTransferTrackingQueryParms`): filter criteria for searching tracking header records.
- Data:
  - Reads (via query execution): `FTS_FILE_MANAGEMENT`, `FTS_SOURCE_LOCATION`.
- Errors: none in this contract type.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.FtpServerQuery
FTP server profile search query.

- Properties:
  - `Where` (`FtpServerQueryParms`): filter criteria for searching FTP server profiles.
- Data:
  - Reads (via query execution): FTP server projection/store.
- Errors: none in this contract type.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.GetIntegrationFileQuery
Integration file lookup query.

- Properties:
  - `Where` (`IntegrationFileQueryParms`): filter criteria for integration file lookup.

### Validate()
- Purpose: perform request validation for the integration-file lookup query.
- Inputs: none (uses `Where`).
- Logic:
  - Assign `messages = new List<BusinessExceptionMessage>()`.
  - Call `HandleErrors()`.
  - Since `messages` is always empty in this type, validation completes without throwing.
- Data: none.
- Errors:
  - If `messages.Count > 0`, throws `BusinessException(messages, CommonConstants.Validation.ErrorType.InvalidParameters, CommonConstants.Validation.ProcessingSystem)`.
- Rules: none.

### HandleErrors()
- Purpose: raise a `BusinessException` when validation has accumulated error messages.
- Inputs: none (uses `messages`).
- Logic:
  - If `messages.Count > 0`, throw `BusinessException(...)`.
- Data: none.
- Errors: throws `BusinessException` when `messages.Count > 0`.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.GetSourceLocationQuery
Source location details lookup query.

- Properties:
  - `Where` (`GetSourceLocationQueryParms`): filter criteria for source location details; includes `SourceLocationId`.

### Validate()
- Purpose: ensure details lookup specifies a non-empty source location id.
- Inputs: none (uses `Where.SourceLocationId`).
- Logic:
  - If `string.IsNullOrEmpty(Where.SourceLocationId)` then throw `new FileTransferValidationException(FtpSoureceLocationId)`.
- Data: none.
- Errors:
  - Throws `FileTransferValidationException` with validation key/message `FtpSoureceLocationId`.
  - If `Where` is null, the call attempts to read `Where.SourceLocationId` and throws a `NullReferenceException`.
- Rules: F01-100.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.InboundTargetQuery
Inbound target search query.

- Properties:
  - `Where` (`InboundTargetQueryParms`): filter criteria for inbound target lookup.

### Validate()
- Purpose: perform request validation for the inbound-target lookup query.
- Inputs: none (uses `Where`).
- Logic:
  - Assign `messages = new List<BusinessExceptionMessage>()`.
  - Call `HandleErrors()`.
  - Since `messages` is always empty in this type, validation completes without throwing.
- Data: none.
- Errors:
  - If `messages.Count > 0`, throws `BusinessException(messages, CommonConstants.Validation.ErrorType.InvalidParameters, CommonConstants.Validation.ProcessingSystem)`.
- Rules: none.

### HandleErrors()
- Purpose: raise a `BusinessException` when validation has accumulated error messages.
- Inputs: none (uses `messages`).
- Logic:
  - If `messages.Count > 0`, throw `BusinessException(...)`.
- Data: none.
- Errors: throws `BusinessException` when `messages.Count > 0`.
- Rules: none.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.SourceLocationQuery
Source location search query.

- Properties:
  - `Where` (`SourceLocationQueryParms`): filter criteria for searching source locations.
- Data:
  - Reads (via query execution): source location projection/store.
- Errors: none in this contract type.
- Rules: none.

## Rules implemented
| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-100 | Getting source location details requires a non-empty `SourceLocationId`; when it is missing, the query throws a validation exception with message `FtpSoureceLocationId`. | `GetSourceLocationQuery.Validate` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |

## Not carried over

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (queries are read-only requests against read models; query contracts must not cause side effects).
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (validation failures are represented consistently and are surfaced in a standardized error contract at the HTTP boundary).

