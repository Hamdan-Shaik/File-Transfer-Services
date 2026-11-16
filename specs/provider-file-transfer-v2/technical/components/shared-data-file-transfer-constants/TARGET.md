---
type: Target Placement
title: 'Target: shared-data-file-transfer-constants'
description: Where the shared-data-file-transfer-constants component goes in the new system and what to build, method by method.
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
  body_sha256: 7f294320a892e4d3a5c1a8205906f334de0c5cbd8f9f1ee1325b8edda83a2356
---

# Target: `shared-data-file-transfer-constants`

## Where

- Deployable: `fxfer-mfe` (`apps/fxfer-mfe`)
- Folder: `apps/fxfer-mfe/src/legacy-constants`

## What to create

Create a TypeScript module that provides the shared subset of the legacy UX constants owned by this component.

- `apps/fxfer-mfe/src/legacy-constants/file-transfer-constants-shared.ts`
  - Export `FILE_TRANSFER_CONSTANTS_SHARED` (object, `as const`) with the following nested constant groups and values (parity with the legacy constants):
    - `ModuleName`
    - `ApplicationSettings.ExternalFTECommonFolder`
    - `Constants` (`FTPCode`, `SFTPCode`, `FTPSCode`, `GuidFormat`)
    - `Localization.DataListsItems` (`RunOnThisDay`, `RunScheduleMonhly`, `RunScheduleWeekly`)

## Mapping

Inventory contains constants-only legacy classes (no methods); mapping is class-to-object-path.

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants` | `FILE_TRANSFER_CONSTANTS_SHARED` | Shared subset only; other legacy constant groups are provided by other constants components (e.g., `backend-data-file-transfer-constants`). |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.ApplicationSettings` | `FILE_TRANSFER_CONSTANTS_SHARED.ApplicationSettings` | Application setting key names. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Constants` | `FILE_TRANSFER_CONSTANTS_SHARED.Constants` | Shared protocol and formatting constants. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.DataListsItems` | `FILE_TRANSFER_CONSTANTS_SHARED.Localization.DataListsItems` | Schedule/list item ids used by rule `F01-017`. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `standards/technology/nextjs/STD-TEC-NXT-001-coding.md` section `2.3 Naming Conventions` (exported constants use UPPER_SNAKE_CASE)
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public exports are named for usage/meaning)