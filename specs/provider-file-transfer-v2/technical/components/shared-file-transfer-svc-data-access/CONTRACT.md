---
type: Interface Contract
title: technical/components/shared-file-transfer-svc-data-access/CONTRACT
description: What the shared-file-transfer-svc-data-access component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-file-transfer-svc-data-access
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:14:49Z
sources:
- id: inventory
  resource: /technical/components/shared-file-transfer-svc-data-access/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-file-transfer-svc-data-access
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
  body_sha256: b208a6d003bb7eb39c949732dac4b0476fd6dd1ebe6d7f26e7024f56841afa9d
---

﻿# Contract: `shared-file-transfer-svc-data-access`

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
