---
type: Target Placement
title: 'Target: backend-data-file-transfer-constants'
description: Where the backend-data-file-transfer-constants component goes in the new system and what to build, method by method.
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
  body_sha256: 9a44c25550be4ed7912684d0f53a3d94a1da32b66eed0627fdc2a117ececf96b
---

# Target: `backend-data-file-transfer-constants`

## Where

- Deployable: `fxfer-mfe` (`apps/fxfer-mfe`)
- Folder: `apps/fxfer-mfe/src/legacy-constants`

## What to create

Create a TypeScript module that provides the legacy UX constant groups owned by this component.

- `apps/fxfer-mfe/src/legacy-constants/file-transfer-constants-backend.ts`
  - Export `FILE_TRANSFER_CONSTANTS_BACKEND` (object, `as const`) with the following nested constant groups and values (parity with the legacy constants):
    - `BusinessValidationExceptions`
    - `Ioc.ComponentNames`
    - `Localization` (including `DataLists`, `DataListsItems`, `Entities`, `HtmlBlocks`, `Images`, `Labels`, `Messages`)
    - `Miscellaneous`
    - `MVC.Actions`, `MVC.Controllers`
    - `SearchFields`
    - `Help.UserGuide`
    - `WebApi.Actions`, `WebApi.Controllers`
    - `Xml.URI`

## Mapping

Inventory contains constants-only legacy classes (no methods); mapping is class-to-object-path.

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.BusinessValidationExceptions` | `FILE_TRANSFER_CONSTANTS_BACKEND.BusinessValidationExceptions` | Preserve constant names and values. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Ioc` | `FILE_TRANSFER_CONSTANTS_BACKEND.Ioc` | Container for IOC-related constant groups. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Ioc.ComponentNames` | `FILE_TRANSFER_CONSTANTS_BACKEND.Ioc.ComponentNames` | IOC component name strings. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization` | `FILE_TRANSFER_CONSTANTS_BACKEND.Localization` | Full nested localization constant groups. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Miscellaneous` | `FILE_TRANSFER_CONSTANTS_BACKEND.Miscellaneous` | Preserve `FileToEvent` and `UNC` values. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.MVC` | `FILE_TRANSFER_CONSTANTS_BACKEND.MVC` | Container for MVC action/controller names. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.MVC.Actions` | `FILE_TRANSFER_CONSTANTS_BACKEND.MVC.Actions` | MVC action names. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.MVC.Controllers` | `FILE_TRANSFER_CONSTANTS_BACKEND.MVC.Controllers` | MVC controller names. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.SearchFields` | `FILE_TRANSFER_CONSTANTS_BACKEND.SearchFields` | Search field name constants. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Help` | `FILE_TRANSFER_CONSTANTS_BACKEND.Help` | Container for help IDs. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Help.UserGuide` | `FILE_TRANSFER_CONSTANTS_BACKEND.Help.UserGuide` | User-guide help IDs. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.WebApi` | `FILE_TRANSFER_CONSTANTS_BACKEND.WebApi` | Container for Web API action/controller names. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.WebApi.Actions` | `FILE_TRANSFER_CONSTANTS_BACKEND.WebApi.Actions` | Web API action names. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.WebApi.Controllers` | `FILE_TRANSFER_CONSTANTS_BACKEND.WebApi.Controllers` | Web API controller names. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Xml` | `FILE_TRANSFER_CONSTANTS_BACKEND.Xml` | Container for XML constants. |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Xml.URI` | `FILE_TRANSFER_CONSTANTS_BACKEND.Xml.URI` | XML namespace constants. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `standards/technology/nextjs/STD-TEC-NXT-001-coding.md` section `2.3 Naming Conventions` (exported constants use UPPER_SNAKE_CASE)
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public exports are named for usage/meaning)

