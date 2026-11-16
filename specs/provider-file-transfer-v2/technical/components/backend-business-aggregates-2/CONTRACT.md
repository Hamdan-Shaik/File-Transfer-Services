---
type: Interface Contract
title: 'Contract: backend-business-aggregates-2'
description: What the backend-business-aggregates-2 component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-aggregates-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:36:00Z
sources:
- id: inventory
  resource: /technical/components/backend-business-aggregates-2/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-aggregates-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 3
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain
  body_sha256: c02e38feee0269f3bbe781782bb06f872ba799d67a07bc931854f61ee6c7504b
---

# Contract: backend-business-aggregates-2

This component provides a domain-layer type representing File Transfer Source Location configuration state.

## Offers

### Domain aggregate state

- `FtsSourceLocationState`
  - Represents the current values of a File Transfer Source Location configuration record.
  - Used by command handlers and writers as the aggregate state object for this configuration concept.

## Uses

None (no component-to-component dependencies declared for this component).

## Data

This component does not directly read or write persistence stores. It defines domain state types; persistence is performed by application-layer handlers and infrastructure writers.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-008-domain-driven-design-pattern.md` section `2. Aggregates: Consistency Boundaries` (aggregate-related types live in the Domain layer and represent a single consistency boundary concept).

