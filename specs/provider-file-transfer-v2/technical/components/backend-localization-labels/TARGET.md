---
type: Target Placement
title: 'Target: backend-localization-labels'
description: Where the backend-localization-labels component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-localization-labels
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T20:30:25Z
sources:
- id: inventory
  resource: /technical/components/backend-localization-labels/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-localization-labels
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 4
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants/localization
  body_sha256: 17ae70a0e086209e3635a7fc97aa45d882483865ad8b1a95366e376f9e483084
---

# Target: backend-localization-labels

## Where

- Deployable: `fxfer-mfe` (`apps/fxfer-mfe`)
- Folder: `apps/fxfer-mfe/src/legacy-constants/localization`

## What to create

Create a TypeScript module that provides the legacy File Transfer UX localization label constant groups owned by this component.

- `apps/fxfer-mfe/src/legacy-constants/localization/file-transfer-constants-localization-labels.ts`
  - Export `FILE_TRANSFER_CONSTANTS_LOCALIZATION_LABELS` (object, `as const`) with the following nested constant groups and values (parity with the legacy constants):
    - `Editor`
    - `Buttons`
    - `Fields`
    - `Titles`

## Mapping

Inventory contains constants-only legacy classes (no methods); mapping is class-to-object-path.

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Labels.Editor` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION_LABELS.Editor` | Preserve as an empty object (no members). |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Labels.Buttons` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION_LABELS.Buttons` | Preserve constant names and values (including legacy casing). |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Labels.Fields` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION_LABELS.Fields` | Preserve constant names and values (including legacy spelling). |
| `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Labels.Titles` | `FILE_TRANSFER_CONSTANTS_LOCALIZATION_LABELS.Titles` | Preserve constant names and values. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `standards/technology/nextjs/STD-TEC-NXT-001-coding.md` section `2.3 Naming Conventions` (exported constants use UPPER_SNAKE_CASE)
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public exports reflect usage rather than implementation)
- `strategy/guidance/application/GUID-APP-I18N-002-medicaid-portal-localization.md` section `Compliance Notes` (translation units are UI strings only; do not include PHI/PII in localization keys or message catalogs)

