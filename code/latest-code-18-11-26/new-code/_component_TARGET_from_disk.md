---
type: Target Placement
title: 'Target: shared-queries-parameters-2'
description: Where the shared-queries-parameters-2 component goes in the new system and what to build, method by method.
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
  body_sha256: e16868e653e7252d38e4544b9a071262b9dceffa5558cb6b594296cd47f62a7e
---

# Target: shared-queries-parameters-2

## Where

- Deployable: xfer-domain-api (services/fxfer-domain-api)
- Folder: services/fxfer-domain-api/src/Contracts/Queries

## What to create

Create the query contract and parameter types used by xfer-domain-api query handlers when looking up integration-file records.
Implement them as plain C# contract types under services/fxfer-domain-api/src/Contracts/Queries.

Create the following C# types:

- Domain/GroupInfo
  - Fields/properties: GroupId (Guid?), NumberOfRecords (int).

- Parameters/IntegrationFileQueryParms
  - Provide a parameterless constructor that sets FTSIntegrationFileId = Guid.Empty.
  - Fields/properties: FTSIntegrationFileId (Guid), FileName (string).

- GetIntegrationFileQuery
  - Expose Where criteria for integration-file lookup.
  - Provide a parameterless constructor that initializes Where = new IntegrationFileQueryParms().
  - Implement Validate() as a no-op (legacy validation initializes an empty message list and never throws in practice).

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| GroupInfo | GroupInfo | Preserve nullable GroupId and NumberOfRecords value type. |
| IntegrationFileQueryParms | IntegrationFileQueryParms | Preserve constructor default FTSIntegrationFileId = Guid.Empty. |
| GetIntegrationFileQuery.Validate() | GetIntegrationFileQuery.Validate() | Preserve as no-op (legacy only allocates a list and never throws in practice). |
| GetIntegrationFileQuery.HandleErrors() | drop: legacy no-op scaffolding | Legacy throws only when internal messages is non-empty, which is not possible from callers. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md section Query Side Components (query requests are separated from commands and must be read-only / side-effect free).
- standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md section Recommended Requirements (queries are named and treated as part of the query/read side).
- patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md section Solution Overview (validation failures are represented consistently so the HTTP layer can surface standardized Problem Details responses).
- standards/application/api/STD-APP-API-001-api-design.md section 3.5 Error Handling (validation failures use consistent, machine-readable error responses at the API boundary).

