---
type: Interface Contract
title: 'Contract: backend-file-transfer-constants-localization'
description: What the backend-file-transfer-constants-localization component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-constants-localization
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:44:39Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-constants-localization/INVENTORY.yaml
  title: Every legacy class and method (7 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-constants-localization
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 7
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants/localization
  body_sha256: 6be6ab351e8bea90aa19571d136b9a7a9ed54ee36c7f2d4f33d5b75be02a7deb
---

# Contract: `backend-file-transfer-constants-localization`

This component provides legacy File Transfer UX localization keys for datalists, entities, HTML blocks, labels, and messages.

## Offers

- `FILE_TRANSFER_CONSTANTS_LOCALIZATION`
  - `DataLists`
  - `DataListsItems`
  - `Entities`
  - `HtmlBlocks`
  - `Images`
  - `Labels` (`Editor`, `Buttons`, `Fields`, `Titles`)
  - `Messages`

## Uses

None (no component dependencies).

## Data owned

None (constants only; no storage).

## Enterprise-Architecture standards applied

- `standards/technology/nextjs/STD-TEC-NXT-001-coding.md` section `2.3 Naming Conventions` (exported constants use UPPER_SNAKE_CASE)
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public exports are named for usage/meaning)
- `strategy/guidance/application/GUID-APP-I18N-002-medicaid-portal-localization.md` section `Compliance Notes` (translation units are UI strings only; do not include PHI/PII in localization keys or message catalogs)

