---
type: Interface Contract
title: technical/components/shared-common-common-constants/CONTRACT
description: What the shared-common-common-constants component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-common-common-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:40:13Z
sources:
- id: inventory
  resource: /technical/components/shared-common-common-constants/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-common-common-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 5
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain/Constants
  body_sha256: 249634b5c9d4d13fdeed6c4cdc9be48a6ff07c20245c6f5fa00454820948ade3
---

﻿# Contract: `shared-common-common-constants`

This component provides shared constant values used by FXFER domain logic for module identifiers, validation error vocabulary, regex patterns, and search sort-field naming.

## Offers

- `CommonConstants`
  - `FtpServerCode`, `ModuleName`
  - `RegexPatterns.IpAddressPattern`
  - `Validation.BusinessErrorCode`, `Validation.ProcessingSystem`
  - `Validation.ErrorType.*`
  - `Validation.Messages.*`
  - `Validation.Properties.*`
  - `SortFields.FtpServer.*`

## Uses

None (no component dependencies).

## Data owned

None (constants only; no storage).

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-008-domain-driven-design-pattern.md` section `Pattern 3: Shared Kernel (Tight Coupling Accepted)` (shared constant vocabulary is kept in a shared Domain layer module for reuse).
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public API items are named for usage/meaning).
