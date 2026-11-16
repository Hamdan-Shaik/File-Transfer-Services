---
type: Target Placement
title: technical/components/shared-file-transfer-svc-data-access/TARGET
description: Where the shared-file-transfer-svc-data-access component goes in the new system and what to build, method by method.
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
  body_sha256: 8ccaac57db3f8153bb0054b664d2216be3d13dc2c16c763f6b27c6f8d49f889a
---

﻿# Target: `shared-file-transfer-svc-data-access`

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Infrastructure/DataAccess/Constants`

## What to create

Create C# constants types that preserve the legacy constant names and values for parity.

- `services/fxfer-domain-api/src/Infrastructure/DataAccess/Constants/DataAccessConstants.cs`
  - Type: `public static class DataAccessConstants`
  - Members:
    - `public const string DefaultConnection = "DefaultConnection";`
    - `public const string DefaultConnectionProvider = "DefaultConnectionProvider";`
    - `public const string DefaultConnectionString = "DefaultConnectionString";`
  - Nested types:
    - `public static class FileStatusConstant`
      - `public const string Completed = "1";`
      - `public const string Error = "2";`
      - `public const string Transfering = "3";`
      - `public const string UNCProtocolCD = "3";`

- `services/fxfer-domain-api/src/Infrastructure/DataAccess/Constants/ErrorLogConstants.cs`
  - Type: `public static class ErrorLogConstants`
  - Members:
    - `public const string ExceptionType = "ExceptionType";`
    - `public const string Message = "Message";`
    - `public const string MethodName = "MethodName";`
    - `public const string StackTrace = "StackTrace";`

- `services/fxfer-domain-api/src/Infrastructure/DataAccess/Constants/FileDeliveryEventConstants.cs`
  - Type: `public static class FileDeliveryEventConstants`
  - Members:
    - `public const string Renamed = "R";`
    - `public const string Transferred = "T";`
    - `public const string Archieved = "A";`

## Mapping

Inventory contains constants-only legacy classes (no methods); mapping is type-to-type.

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.DataAccessConstants` | `DataAccessConstants` | Preserve constant names and values, including nested `FileStatusConstant` values. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.ErrorLogConstants` | `ErrorLogConstants` | Preserve canonical error log field names. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.FileDeliveryEventConstants` | `FileDeliveryEventConstants` | Preserve single-character delivery event codes. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions > Constants` (constant naming is preserved as part of the stable vocabulary consumed by other code).
