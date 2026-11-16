---
type: Interface Contract
title: 'Contract: backend-business-aggregates'
description: What the backend-business-aggregates component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-aggregates
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:26:50Z
sources:
- id: inventory
  resource: /technical/components/backend-business-aggregates/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-aggregates
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 8
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain
  body_sha256: 9336cc287302e9c6ed2b8c601e4e5d0eed6de25be488f9507d337cd68a3aada1
---

# Contract: backend-business-aggregates

This component provides domain-layer types representing File Transfer configuration state and mapping utilities used to translate inbound integration events into FXFER command DTOs.

## Offers

### Domain aggregate state

- `FtsSourceLocationState`
  - Represents the current values of a File Transfer Source Location configuration record.
  - Used by command handlers and writers as the aggregate state object for this configuration concept.

### Mapping profiles

- `InboundTargetProfile`
  - AutoMapper profile mapping `FTEMetadataUpdated`/`FTEMetadataInvalidated` events to `AddFtsInboundTargetCommand`/`InvalidateFtsInboundTargetCommand`.

- `IntegrationFileProfile`
  - AutoMapper profile mapping `ExtractFileGenerated`/`SummaryReportGenerated` events to `AddIntegrationFileCommand`.
  - Includes group-info translation helpers.

## Uses

None (no component-to-component dependencies declared for this component).

## Data

This component does not directly read or write persistence stores. It defines domain state and mapping configuration; persistence is performed by application-layer handlers and infrastructure writers.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-008-domain-driven-design-pattern.md` section `2. Aggregates: Consistency Boundaries` (aggregate-related types live in the Domain layer and represent a single consistency boundary concept).

