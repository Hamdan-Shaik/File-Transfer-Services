---
type: Target Placement
title: technical/components/shared-common-constants-validation/TARGET
description: Where the shared-common-constants-validation component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-common-constants-validation
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:46:01Z
sources:
- id: inventory
  resource: /technical/components/shared-common-constants-validation/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-common-constants-validation
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
  folder: src/Domain/Validation
  body_sha256: 7965689b32405d27c4b5e4defdc12f25ee94cc7ea6df49faeefc0a371dadde68
---

﻿# Target: `shared-common-constants-validation`

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Domain/Validation`

## What to create

Create a C# constants type that preserves the legacy validation constant names and values for parity.

- `services/fxfer-domain-api/src/Domain/Validation/ValidationConstants.cs`
  - Type: `public static class ValidationConstants`
  - Nested types:
    - `public static class ErrorType`
      - `DuplicatedFtpServerName = "DuplicatedFtpServerName"`
      - `InvalidCommand = "Invalid Command"`
      - `InvalidFtpProxyPort = "InvalidFtpProxyPort"`
      - `InvalidFtpServerId = "InvalidFtpServerId"`
      - `InvalidParameters = "InvalidSearchParameters"`
      - `InvalidIpAddressFormat = "InvalidIpAddressFormat"`
      - `NuGetPackage = "NuGet package"`
      - `QueryExecutionError = "QueryExecutionError"`
      - `RequiredFields = "RequiredFields"`
    - `public static class Messages`
      - `CommandError = "Command Execution Error"`
      - `DuplicatedFtpServerName = "Duplicated FTP Server Name"`
      - `ExcludedExtension = "Source file mask extension is not Allowed:"`
      - `FTRepositoryGetResultsError = "File Transfer Repository error when getting results for"`
      - `IpAddressInvalidFormat = "Invalid Format for IP Address"`
      - `InvalidValue = "Invalid value for"`
      - `MissingConfigId = "Missing ConfigId"`
      - `MissingField = "Missing field"`
    - `public static class Properties`
      - `FtpProxyPort = "FtpProxyPort"`
      - `FtpServerId = "FtpServerId"`
      - `FtpServerIp = "FtpServerIp"`
      - `RunSchedule = "Run Schedule"`

Notes:

- These constants are intended to be used when emitting validation errors (for example, as ProblemDetails extension fields describing validation failures).

## Mapping

Inventory contains constants-only legacy classes (no methods); mapping is type-to-type.

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.Validation.ErrorType` | `ValidationConstants.ErrorType` | Preserve constant names and values. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.Validation.Messages` | `ValidationConstants.Messages` | Preserve constant names and values. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.Validation.Properties` | `ValidationConstants.Properties` | Preserve constant names and values. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (validation errors include field-level details; constants provide a stable error vocabulary).
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public API items are named for usage/meaning).
