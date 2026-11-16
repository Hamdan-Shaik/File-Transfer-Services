---
type: Target Placement
title: 'Target: backend-file-transfer-constants-localization'
description: Where the backend-file-transfer-constants-localization component goes in the new system and what to build, method by method.
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
  body_sha256: 68def1de671a5d2455b96687a80341afc25ba8d9ba7e6907332b1b5f171accb1
---

# Target: `backend-file-transfer-constants-localization`

## Where

- Deployable: `fxfer-mfe` (`apps/fxfer-mfe`)
- Folder: `apps/fxfer-mfe/src/legacy-constants/localization`

## What to create

Create a TypeScript module that provides the legacy File Transfer UX localization constant groups owned by this component.

- `apps/fxfer-mfe/src/legacy-constants/localization/file-transfer-constants-localization.ts`
  - Export `FILE_TRANSFER_CONSTANTS_LOCALIZATION` (object, `as const`) with the following nested constant groups and values (parity with the legacy constants):
    - `DataLists`
    - `DataListsItems`
    - `Entities`
    - `HtmlBlocks`
    - `Images`
    - `Labels` (including `Editor`, `Buttons`, `Fields`, `Titles`)
    - `Messages`

## Mapping

Inventory contains constants-only legacy classes (no methods); mapping is class-to-object-path.

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.DataLists` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION.DataLists` | Preserve constant names and values. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.DataListsItems` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION.DataListsItems` | Preserve constant names and values (including legacy spelling). |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Entities` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION.Entities` | Preserve constant names and values. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.HtmlBlocks` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION.HtmlBlocks` | Preserve constant names and values. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Images` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION.Images` | Preserve as an empty object (no members). |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Labels` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION.Labels` | Preserve nested groups and their constant names/values. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Messages` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION.Messages` | Preserve constant names and values. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `standards/technology/nextjs/STD-TEC-NXT-001-coding.md` section `2.3 Naming Conventions` (exported constants use UPPER_SNAKE_CASE)
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public exports are named for usage/meaning)
- `strategy/guidance/application/GUID-APP-I18N-002-medicaid-portal-localization.md` section `Compliance Notes` (translation units are UI strings only; do not include PHI/PII in localization keys or message catalogs)

