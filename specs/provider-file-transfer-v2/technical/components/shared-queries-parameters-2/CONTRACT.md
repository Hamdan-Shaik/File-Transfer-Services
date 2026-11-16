---
type: Interface Contract
title: 'Contract: shared-queries-parameters-2'
description: What the shared-queries-parameters-2 component offers other components and what it uses from them.
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
  body_sha256: 6069995319f14769a1d4befb1132bdde433ede213a215d38883d4157f1ea9e30
---

# Contract: shared-queries-parameters-2

This component defines query contract and parameter DTOs for integration-file lookup.
It is consumed by query execution code and query endpoints within xfer-domain-api.

## Offers

- **Query contract**: GetIntegrationFileQuery with Where criteria for integration-file lookup.
- **Query parameters**: IntegrationFileQueryParms filter/lookup criteria.
- **Domain DTO**: GroupInfo summary data shape used by callers/handlers when returning group-related summary information.

## Uses

None (no cross-component contracts declared by this component).

## Data owned

None.

## Enterprise-Architecture standards applied

- patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md section Query Side Components (query-side contracts/DTOs are read-only and must not introduce side effects).
- standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md section Recommended Requirements (queries and their DTOs participate in CQRS query handling).

