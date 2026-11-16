---
type: Interface Contract
title: 'Contract: shared-data-file-transfer-constants'
description: What the shared-data-file-transfer-constants component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-data-file-transfer-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:02:16Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-data-file-transfer-constants/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-data-file-transfer-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 3
  methods: 0
  owned_rules: 1
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants
  body_sha256: cfab0c58bf52a64b181a5cb8349ce4502e22c22b510f5881cabaa5932b0c8aab
---

# Contract: `shared-data-file-transfer-constants`

This component provides shared constant values used by the FXFER micro-frontend when mapping file-transfer configuration models and identifying protocol/schedule codes.

## Offers

- `FILE_TRANSFER_CONSTANTS_SHARED`
  - `ModuleName`
  - `ApplicationSettings.ExternalFTECommonFolder`
  - `Constants` (`FTPCode`, `SFTPCode`, `FTPSCode`, `GuidFormat`)
  - `Localization.DataListsItems` (`RunOnThisDay`, `RunScheduleMonhly`, `RunScheduleWeekly`)

## Uses

None (no component dependencies).

## Data owned

None (constants only; no storage).

## Enterprise-Architecture standards applied

- `standards/technology/nextjs/STD-TEC-NXT-001-coding.md` section `2.3 Naming Conventions` (exported constants use UPPER_SNAKE_CASE)
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public exports are named for usage/meaning)