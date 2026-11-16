---
type: Interface Contract
title: technical/components/shared-file-transfer-svc-data-access-2/CONTRACT
description: What the shared-file-transfer-svc-data-access-2 component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-file-transfer-svc-data-access-2
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:28:54Z
sources:
- id: inventory
  resource: /technical/components/shared-file-transfer-svc-data-access-2/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-file-transfer-svc-data-access-2
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 3
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Infrastructure/DataAccess/Constants
  body_sha256: a4a4b9e76e47ef0d7177198f64f43b367fbe8ee6e4d35b8e8b8762c1557696d5
---

﻿# Contract: `shared-file-transfer-svc-data-access-2`

This component provides shared constants used by FXFER infrastructure data-access code for configuration key names, file status codes, error-log field names, and file-delivery event codes.

## Offers

- `DataAccessConstants`
  - `DefaultConnection`, `DefaultConnectionProvider`, `DefaultConnectionString`
  - `FileStatusConstant.Completed`, `FileStatusConstant.Error`, `FileStatusConstant.Transfering`, `FileStatusConstant.UNCProtocolCD`

- `ErrorLogConstants`
  - `ExceptionType`, `Message`, `MethodName`, `StackTrace`

- `FileDeliveryEventConstants`
  - `Renamed`, `Transferred`, `Archieved`

## Uses

None (no component dependencies).

## Data owned

None (constants only; no storage).

## Enterprise-Architecture standards applied

- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions > Constants` (constants are treated as stable vocabulary and named consistently for reuse).
