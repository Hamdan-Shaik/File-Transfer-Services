---
type: Interface Contract
title: 'Contract: backend-data-file-transfer-constants'
description: What the backend-data-file-transfer-constants component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-file-transfer-constants
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:29:46Z
sources:
- id: inventory
  resource: /technical/components/backend-data-file-transfer-constants/INVENTORY.yaml
  title: Every legacy class and method (16 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-file-transfer-constants
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 16
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants
  body_sha256: fc4465d835dfcea9646619170fcc0e514bab85678fb9a6c0230fc81ee7a65a89
---

# Contract: `backend-data-file-transfer-constants`

This component provides constant values used by the File Transfer UX for localization lookups, MVC/WebApi route naming, search-field naming, and help identifiers.

## Offers

- `FILE_TRANSFER_CONSTANTS_BACKEND`
  - `BusinessValidationExceptions`
  - `Ioc.ComponentNames`
  - `Localization` (`DataLists`, `DataListsItems`, `Entities`, `HtmlBlocks`, `Images`, `Labels`, `Messages`)
  - `Miscellaneous`
  - `MVC.Actions`, `MVC.Controllers`
  - `SearchFields`
  - `Help.UserGuide`
  - `WebApi.Actions`, `WebApi.Controllers`
  - `Xml.URI`

## Uses

None (no component dependencies).

## Data owned

None (constants only; no storage).

## Enterprise-Architecture standards applied

- `standards/technology/nextjs/STD-TEC-NXT-001-coding.md` section `2.3 Naming Conventions` (exported constants use UPPER_SNAKE_CASE)
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public exports are named for usage/meaning)

