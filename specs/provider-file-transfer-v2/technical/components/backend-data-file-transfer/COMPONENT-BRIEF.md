---
type: Generation Brief
title: 'Brief: component backend-data-file-transfer'
description: Instructions the tool gave the writer session for the backend-data-file-transfer component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-file-transfer
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:21:12Z
sources:
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-data-file-transfer/INVENTORY.yaml
  title: Every legacy class and method (11 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-file-transfer
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 11
  methods: 1
  owned_rules: 2
  flows:
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-bff
  folder: src/Contracts
  body_sha256: bc6ace85bf0f19595ef576990562d0e2dfbedcda46a3fbb33aa44f369afcca4e
---

# Brief: component `backend-data-file-transfer`

- HP.HSP.UA3.FileTransfer.UX.Data.FileTransfer (backend tier, 11 classes, 1 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-data-file-transfer/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-data-file-transfer/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-bff` (`services/fxfer-bff`), folder `src/Contracts`: write `services/fxfer-bff/src/Contracts` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-bff/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`

## Packages it calls (contracts to keep)

- `hp.hsp.ua3.core.ux.common`

## Business rules it owns (2)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-042** When building the FTP server search request, missing search fields (or missing matching field names) are treated as empty strings for `FtpServerName`, `FtpDescription`, and `FtpUser`.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Controller\FileTransferController.FTPServerCredentials.cs:96-102, ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs:33-36
- Legacy: `BuildRequest` calls `SearchHelper.GetFieldValue(...)`; `GetFieldValue` returns `string.Empty` when there is no criteria or the field is not found.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-044** A search field lookup returns the first matching field by name and converts its value to a string; if no matching field exists, it returns an empty string.
- Doors: file-transfer-controller-get-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Services\Helpers\SearchHelper.cs:33-36
- Legacy: `GetFieldValue` checks `SearchFields` for emptiness, then `FirstOrDefault(filter => filter.FieldName == fieldName)` and returns `FieldValue.ToString()`.
- Decision: keep

