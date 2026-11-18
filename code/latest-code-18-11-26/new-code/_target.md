---
type: Target Placement
title: technical/components/shared-common-common-constants/TARGET
description: Where the shared-common-common-constants component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-common-common-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:40:13Z
sources:
- id: inventory
  resource: /technical/components/shared-common-common-constants/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-common-common-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 5
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain/Constants
  body_sha256: ee7a070ad817ce2ee8d29ccf24de376c128e0a679894c9bc2056e04ce2330988
---

ï»¿# Target: `shared-common-common-constants`

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Domain/Constants`

## What to create

Create a C# constants type that preserves the legacy constant names and values for parity.

- `services/fxfer-domain-api/src/Domain/Constants/CommonConstants.cs`
  - Type: `public static class CommonConstants`
  - Members:
    - `public const string FtpServerCode = "1";`
    - `public const string ModuleName = "FileTransfer";`
  - Nested types:
    - `public static class RegexPatterns`
      - `public const string IpAddressPattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";`
    - `public static class Validation`
      - `public const string BusinessErrorCode = "E";`
      - `public const string ProcessingSystem = "FileTransfer";`
      - Nested `public static class ErrorType`
        - `DuplicatedFtpServerName = "DuplicatedFtpServerName"`
        - `InvalidCommand = "Invalid Command"`
        - `InvalidFtpProxyPort = "InvalidFtpProxyPort"`
        - `InvalidFtpServerId = "InvalidFtpServerId"`
        - `InvalidParameters = "InvalidSearchParameters"`
        - `InvalidIpAddressFormat = "InvalidIpAddressFormat"`
        - `NuGetPackage = "NuGet package"`
        - `QueryExecutionError = "QueryExecutionError"`
        - `RequiredFields = "RequiredFields"`
      - Nested `public static class Messages`
        - `CommandError = "Command Execution Error"`
        - `DuplicatedFtpServerName = "Duplicated FTP Server Name"`
        - `ExcludedExtension = "Source file mask extension is not Allowed:"`
        - `FTRepositoryGetResultsError = "File Transfer Repository error when getting results for"`
        - `IpAddressInvalidFormat = "Invalid Format for IP Address"`
        - `InvalidValue = "Invalid value for"`
        - `MissingConfigId = "Missing ConfigId"`
        - `MissingField = "Missing field"`
      - Nested `public static class Properties`
        - `FtpProxyPort = "FtpProxyPort"`
        - `FtpServerId = "FtpServerId"`
        - `FtpServerIp = "FtpServerIp"`
        - `RunSchedule = "Run Schedule"`
    - `public static class SortFields`
      - Nested `public static class FtpServer`
        - `CreationDate = "CreationDate"`
        - `FtpDescription = "FtpDescription"`
        - `FtpServerId = "FtpServerId"`
        - `FtpServerIp = "FtpServerIp"`
        - `FtpServerName = "FtpServerName"`
        - `FtpUser = "FtpUser"`
        - `IsValid = "IsValid"`

## Mapping

Inventory contains constants-only legacy classes (no methods); mapping is type-to-type.

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants` | `CommonConstants` | Preserve constant names and values. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.RegexPatterns` | `CommonConstants.RegexPatterns` | Preserve regex pattern string. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.Validation` | `CommonConstants.Validation` | Preserve validation vocabulary constants. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.SortFields` | `CommonConstants.SortFields` | Preserve sort-field container. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common.CommonConstants.SortFields.FtpServer` | `CommonConstants.SortFields.FtpServer` | Preserve FTP server sort-field names. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-008-domain-driven-design-pattern.md` section `Pattern 3: Shared Kernel (Tight Coupling Accepted)` (shared constant vocabulary is kept in a shared Domain layer module for reuse).
- `decisions/governance/ADR-GOV-004-code-style-guide.md` section `Naming Conventions` (public API items are named for usage/meaning).

