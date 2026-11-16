---
type: Interface Contract
title: 'Contract: shared-contracts-commands'
description: What the shared-contracts-commands component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-commands
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:20:12Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-manage-integration-files
  resource: /technical/flows/manage-integration-files/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-contracts-commands/INVENTORY.yaml
  title: Every legacy class and method (12 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-commands
  generation:
    status: done
    attempts: 2
  tier: shared
  classes: 12
  methods: 36
  owned_rules: 27
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  depends_on:
  - shared-contracts-utilities
  deployable: fxfer-domain-api
  folder: src/Contracts/Commands
  body_sha256: 6c43588a0103ea30642d4a7b49b9c7feff56312c7caaad8442fd2f61b353c63f
---

# Contract: `shared-contracts-commands`

This component is a contract library placed in the `fxfer-domain-api` deployable under `src/Contracts/Commands`.
It defines the command DTOs used by FXFER write operations (FTP server maintenance, source-location maintenance, inbound-target maintenance, and integration-file updates), including legacy validation behavior for those command types.

## Offers

- **FTP server maintenance commands** (inherit `FileTransferCommand`):
  - `AddFtpServerCommand`
  - `UpdateFtpServerCommand`
  - `InvalidateFtpServerCommand`
  - Validation behavior:
    - Required-field validation via DataAnnotations (`Missing field: <PropertyName>` entries).
    - Additional rules for `FtpServerId`, `FtpServerIp` regex validation, and proxy-port validation.
    - No validation that `FtpPort` is greater than 0.

- **Source location commands** (inherit `FtsCommand` or `Command`):
  - `FtsSourceLocationAddCommand`
  - `FtsSourceLocationUpdateCommand`
  - `FtsSourceLocationInvalidateCommand`
  - Validation behavior:
    - Accumulate `BusinessExceptionMessage` entries and throw `BusinessException` when any exist.
    - For some fields (FTP server id, config id, operator id, source location id, suspension date), validation fails by throwing `ArgumentNullException(<field>)` immediately.
    - Reject disallowed file-mask extensions using `ExcludedFileExtensions`.

- **Inbound target commands**:
  - `AddFtsInboundTargetCommand`
  - `InvalidateFtsInboundTargetCommand`

- **Integration file commands**:
  - `AddIntegrationFileCommand`
  - `UpdateIntegrationFileCommand`
  - Validation behavior:
    - Update requires a non-null `ConfigId`; missing value throws `BusinessException` with one validation entry for `ConfigId` and message `Missing ConfigId`.

- **Command idempotency contract (target-state requirement T-004)**:
  - Command endpoints that accept these command DTOs support operator retries via an `Idempotency-Key` request header.

## Uses

- Package dependency: `hpp.core.bas.cqrs` (legacy `Command` base type and `BusinessException` / `BusinessExceptionMessage` contracts).
- Cross-component dependencies:
  - `shared-contracts-utilities` (`BusinessErrorUtility` used by `FileTransferCommand`)
  - `shared-common-common-constants` (`CommonConstants` validation messages, error codes, and regex patterns)
  - `shared-contracts-domain` (`GroupInfo` referenced by `AddIntegrationFileCommand`)
  - `shared-contracts-events` (`FtsDestinationLocationAdded` referenced by source-location commands)
  - `shared-contracts-exceptions` (`ExcludedFileExtensions`)
  - `shared-exceptions-file-transfer-exception-constants` (`FileTransferExceptionConstants` validation field keys)

## Data owned

None.

## Enterprise-Architecture standards applied

- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Phase 1: Define Commands and Command Handlers` (commands are explicit, validated requests)
- `standards/application/api/STD-APP-API-001-api-design.md` section `3.5 Error Handling` (validation failures are structured and do not leak internal exceptions)

