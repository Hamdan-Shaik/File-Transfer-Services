---
type: Behavior Specification
title: 'shared-data-file-transfer-constants: shared legacy file-transfer constants'
description: 'What the shared-data-file-transfer-constants component does today: every class and method, and the business rules it implements.'
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
  body_sha256: a553389e13663b19c25aaddfcc14712c830cb65b9877aaabb78a1b0876d9c2a1
---

# shared-data-file-transfer-constants: shared legacy file-transfer constants

This component provides a shared subset of the legacy `FileTransferConstants` used by the FXFER operator UI.
It defines fixed string values (module name, protocol codes, schedule ids) that other UI code uses when building requests and mapping configuration models.

## FileTransferConstants
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants`

Shared constants used by multiple UX areas.

- `ModuleName` = `FileTransfer`

- Nested group: `ApplicationSettings`
  - `ExternalFTECommonFolder` = `ExternalFTECommonFolder`

- Nested group: `Constants`
  - `FTPCode` = `1`
  - `SFTPCode` = `2`
  - `FTPSCode` = `4`
  - `GuidFormat` = `00000000-0000-0000-0000-000000000000`

- Nested group: `Localization.DataListsItems`
  - `RunOnThisDay` = `0`
  - `RunScheduleMonhly` = `2`
  - `RunScheduleWeekly` = `1`

  These schedule id constants are used when applying rule `F01-017` (monthly vs weekly schedule inference) in legacy mapping logic.

## FileTransferConstants.ApplicationSettings
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.ApplicationSettings`

Application configuration key names.

- `ExternalFTECommonFolder` = `ExternalFTECommonFolder`

## FileTransferConstants.Constants
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Constants`

General module constants.

- `FTPCode` = `1`
- `SFTPCode` = `2`
- `FTPSCode` = `4`
- `GuidFormat` = `00000000-0000-0000-0000-000000000000`

## Rules implemented

| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-017 | Run schedule id is monthly (`2`) when a day-of-month field is set, otherwise weekly (`1`). | `FileTransferConstants.Localization.DataListsItems` | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |

## Not carried over

- `FileTransferConstants` nested constant groups other than `ApplicationSettings`, `Constants`, and `Localization.DataListsItems`: provided by other constants components (e.g., `backend-data-file-transfer-constants`) because they are UX localization / route / label constants rather than shared protocol/schedule identifiers.