---
type: Interface Contract
title: technical/components/shared-common-constants-validation/CONTRACT
description: What the shared-common-constants-validation component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-common-constants-validation
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:46:01Z
sources:
- id: inventory
  resource: /technical/components/shared-common-constants-validation/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-common-constants-validation
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 3
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain/Validation
  body_sha256: 733249bd799463f7eb21bf086cd34edf3e0234820866029aaf9bade47a85ad30
---

﻿# Contract: `shared-common-constants-validation`

This component provides shared validation constant values (error type identifiers, message strings, and property-name strings) used by FXFER domain validation logic.

## Offers

- `ValidationConstants`
  - `ErrorType.*`
  - `Messages.*`
  - `Properties.*`

## Uses

None (no component dependencies).

## Data owned

None (constants only; no storage).

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (standardize validation error representation; constants support consistent error categorization).
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public API items are named for usage/meaning).
