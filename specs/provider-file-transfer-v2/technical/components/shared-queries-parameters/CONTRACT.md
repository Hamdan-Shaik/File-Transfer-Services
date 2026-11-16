---
type: Interface Contract
title: technical/components/shared-queries-parameters/CONTRACT
description: What the shared-queries-parameters component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-queries-parameters
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:38:34Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-queries-parameters/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-queries-parameters
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 0
  owned_rules: 3
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Queries
  body_sha256: d0f5e84aa871f4b29f0e8782e579f074629acc550f5726d0848b42155307c5b4
---

﻿# Contract: `shared-queries-parameters`

This component defines the query parameter (filter) contract types used by `fxfer-domain-api` query requests and query execution.
These types are part of the CQRS query-side inputs and are used to carry filter criteria for searching and lookup.

## Offers

- Query parameter contract types:
  - `FileEventQueryParms`
  - `FileExceptionQueryParms`
  - `FileTransferTrackingQueryParms`
  - `FtpServerQueryParms`
  - `GetSourceLocationQueryParms`
  - `InboundTargetQueryParms`
  - `IntegrationFileQueryParms`
  - `SourceLocationQueryParms`

Business-rule behavior owned by this component:

- Rule F01-101: `GetSourceLocationQueryParms.SoureceLocationGuid` parses `SourceLocationId` using `Guid.Parse(SourceLocationId)`; invalid values throw.
- Rule F01-104: `GetSourceLocationQueryParms.IsActive` is present in the request parameters but must not be used as a filter by the source-location details query behavior.
- Rule F05-031: `InboundTargetQueryParms` defaults `FTEMetadataId` to `Guid.Empty`.

## Uses

None.

## Data owned

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (query-side contracts represent read-only filter criteria).
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (validation/parsing failures are surfaced consistently via a standardized error contract).