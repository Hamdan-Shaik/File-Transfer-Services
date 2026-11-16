---
type: Interface Contract
title: 'Contract: shared-contracts-queries'
description: What the shared-contracts-queries component offers other components and what it uses from them.
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
  body_sha256: 7789e1c8a2dbc61c26dec7aac7aa8b1e95f1a913051a607546843b352654c11d
---

# Contract: `shared-contracts-queries`

This component defines the query contract types used to request FXFER configuration and tracking read data.
These contracts are used by `fxfer-domain-api` query endpoints/handlers and support CQRS query-side behavior.

## Offers

- Query request contracts:
  - `FtpServerQuery`
  - `SourceLocationQuery`
  - `GetSourceLocationQuery`
  - `GetIntegrationFileQuery`
  - `InboundTargetQuery`
  - `FileTransferTrackingQuery`
  - `FileEventQuery`
  - `FileExceptionQuery`

Validation behavior owned by this component:

- Rule F01-100: `GetSourceLocationQuery.Validate()` requires a non-empty `SourceLocationId` and throws a validation exception message key `FtpSoureceLocationId` when missing.

## Uses

None.

## Data owned

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (query contracts represent read-side requests and must not introduce side effects).
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (validation failures are surfaced consistently through a standardized error contract).

