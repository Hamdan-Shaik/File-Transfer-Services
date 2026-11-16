---
type: Target Placement
title: technical/components/backend-business-command-handlers/TARGET
description: Where the backend-business-command-handlers component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-command-handlers
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:59:42Z
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
  resource: /technical/components/backend-business-command-handlers/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-command-handlers
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 25
  owned_rules: 12
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  depends_on:
  - backend-business-command-handlers-2
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - shared-contracts-commands
  deployable: fxfer-domain-api
  folder: src/Application/Commands
  body_sha256: f4b651180023b434867b86172e8a1b6c00ad4dc847d673583bd016aa7dff178e
---

﻿# Target: backend-business-command-handlers

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Application/Commands`

## What to build

Create the target-state command-handling types that implement the legacy mutation semantics while fitting the target CQRS posture (MediatR command handlers + pipeline behaviors).

### Cross-cutting: tenant authorization (T-003)

Create:

- `services/fxfer-domain-api/src/Application/Commands/Behaviors/TenantAuthorizationBehavior.cs`
  - Type: `public sealed class TenantAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>`
  - Purpose: enforce tenant-scoped authorization and invariants at the `fxfer-domain-api` domain boundary for all command requests.
  - Inputs:
    - `ITenantContext` (resolved from validated identity context; provides `TenantId`).
    - `IAuthorizationService` + `ClaimsPrincipal` (route-level policy/scopes).
  - Logic:
    - When `TenantId` is missing/empty: fail authorization (mapped to Problem Details).
    - Enforce a command policy appropriate to the request type (create/update/invalidate operations must be authorized).
    - Do not accept tenant identity from request bodies/headers as the source of truth.

### FTP server commands

Create:

- `services/fxfer-domain-api/src/Application/Commands/FtpServers/AddFtpServerCommandHandler.cs`
  - Type: `public sealed class AddFtpServerCommandHandler : IRequestHandler<AddFtpServerCommand, FxferDomainEvent<FxferConfigurationChangedV1>>`
  - Dependencies:
    - `IFileTransferReadOnlyRepository` (duplicate-name check)
    - `FxferDbContext` (write model persistence)
    - `IFxferDomainEventFactory` (builds `FxferDomainEvent<TPayload>` using tenant + correlation context)
  - Logic:
    - Call `command.Validate()`.
    - Query existing valid FTP servers by name (equivalent to `BuildFtpServerQuery`): `IsValid = true`, `FtpServerName = command.FtpServerName`.
    - When any returned row has `FtpServerId != command.FtpServerId`, add the legacy duplicated-name validation and fail (legacy throws from `HandleValidations()`).
    - Persist the new FTP server profile to the tenant write model.
    - Return a `FxferDomainEvent<FxferConfigurationChangedV1>` with:
      - `aggregate_type = ftp_server`
      - `aggregate_id = command.FtpServerId`
      - `change_kind = created`
      - `operator_id = command.Requestor.IdentifierId`
      - `changed_at = UtcNow`

- `services/fxfer-domain-api/src/Application/Commands/FtpServers/UpdateFtpServerCommandHandler.cs`
  - Type: `public sealed class UpdateFtpServerCommandHandler : IRequestHandler<UpdateFtpServerCommand, FxferDomainEvent<FxferConfigurationChangedV1>>`
  - Logic:
    - Call `command.Validate()`.
    - Run the same duplicate-name check as Add.
    - Persist the update.
    - Return `FxferConfigurationChangedV1` with `change_kind = updated`.

- `services/fxfer-domain-api/src/Application/Commands/FtpServers/InvalidateFtpServerCommandHandler.cs`
  - Type: `public sealed class InvalidateFtpServerCommandHandler : IRequestHandler<InvalidateFtpServerCommand, FxferDomainEvent<FxferConfigurationChangedV1>>`
  - Logic:
    - Do not call `command.Validate()` (legacy parity).
    - Persist `is_valid = false` for the FTP server id.
    - Return `FxferConfigurationChangedV1` with `change_kind = invalidated`.

### Transfer configuration (source location) commands

Create:

- `services/fxfer-domain-api/src/Application/Commands/TransferConfigurations/AddFtsSourceLocationCommandHandler.cs`
  - Type: `public sealed class AddFtsSourceLocationCommandHandler : IRequestHandler<FtsSourceLocationAddCommand, FxferDomainEvent<FxferConfigurationChangedV1>>`
  - Dependencies:
    - `FxferDbContext`
    - `IFxferDomainEventFactory`
  - Logic:
    - Call `command.Validate()`.
    - Allocate `ConfigId` from the database sequence named `FTS_SOURCE_LOCATION_SEQ`.
    - Persist the new transfer configuration to the tenant write model (including destination rows).
    - Return `FxferConfigurationChangedV1` with `aggregate_type = transfer_configuration`, `change_kind = created`, and `operator_id = command.Requestor.IdentifierId`.

- `services/fxfer-domain-api/src/Application/Commands/TransferConfigurations/UpdateFtsSourceLocationCommandHandler.cs`
  - Type: `public sealed class UpdateFtsSourceLocationCommandHandler : IRequestHandler<FtsSourceLocationUpdateCommand, FxferDomainEvent<FxferConfigurationChangedV1>>`
  - Logic:
    - Call `command.Validate()`.
    - Load an existing source location by `ConfigId` (tenant-scoped). The lookup is filtered only by `ConfigId`.
    - When none exists: throw `BusinessException` with one `BusinessExceptionMessage` using `FileTransferExceptionConstants.Validation` (`ConfigId`, `ConfigIdNotFound`, `FileTransfer`) and exception message `Business Error occurred processing the command.`.
    - Persist the update.
    - Return `FxferConfigurationChangedV1` with `change_kind = updated` and `operator_id = command.OperatorId` (legacy parity).

- `services/fxfer-domain-api/src/Application/Commands/TransferConfigurations/InvalidateFtsSourceLocationCommandHandler.cs`
  - Type: `public sealed class InvalidateFtsSourceLocationCommandHandler : IRequestHandler<FtsSourceLocationInvalidateCommand, FxferDomainEvent<FxferConfigurationChangedV1>>`
  - Logic:
    - Call `command.Validate()`.
    - Load an existing source location by `ConfigId` (tenant-scoped, `ConfigId` only).
    - When none exists: throw the same `BusinessException` shape as Update.
    - Persist invalidation/deactivation.
    - Return `FxferConfigurationChangedV1` with `change_kind = invalidated` and `operator_id = command.OperatorId`.

### Inbound target commands

Create:

- `services/fxfer-domain-api/src/Application/Commands/InboundTargets/AddFtsInboundTargetCommandHandler.cs`
  - Type: `public sealed class AddFtsInboundTargetCommandHandler : IRequestHandler<AddFtsInboundTargetCommand, FxferDomainEvent<FxferConfigurationChangedV1>?>`
  - Dependencies:
    - `IFileTransferReadOnlyRepository` (lookup by `FTEMetadataId`)
    - `FxferDbContext`
    - `IFxferDomainEventFactory`
  - Logic:
    - Query inbound targets by `FTEMetadataId`.
    - When none exists: persist a new inbound target with `is_valid = true` and return `FxferConfigurationChangedV1` (`aggregate_type = inbound_target`, `change_kind = created`, `operator_id = command.Requestor.IdentifierId`).
    - Otherwise: return `null` (legacy parity no-op; do not publish a configuration change event).

- `services/fxfer-domain-api/src/Application/Commands/InboundTargets/InvalidateFtsInboundTargetCommandHandler.cs`
  - Type: `public sealed class InvalidateFtsInboundTargetCommandHandler : IRequestHandler<InvalidateFtsInboundTargetCommand, FxferDomainEvent<FxferConfigurationChangedV1>?>`
  - Logic:
    - Query inbound targets by `FTEMetadataId`.
    - When none exists: return `null` (no-op).
    - Otherwise: persist `is_valid = false` and return `FxferConfigurationChangedV1` (`change_kind = invalidated`).

### Integration file commands

Create:

- `services/fxfer-domain-api/src/Application/Commands/IntegrationFiles/AddIntegrationFileCommandHandler.cs`
  - Type: `public sealed class AddIntegrationFileCommandHandler : IRequestHandler<AddIntegrationFileCommand, FxferDomainEvent<FxferConfigurationChangedV1>?>`
  - Dependencies: `FxferDbContext`, `IFxferDomainEventFactory`.
  - Logic:
    - Do not call `command.Validate()` (legacy parity).
    - When `command.GroupInfo` is null or contains no entry with `NumberOfRecords > 0` and `GroupId != null`: return `null` (no-op).
    - Otherwise persist integration file rows (one per group entry according to the write-model rules).
    - Return `FxferConfigurationChangedV1` with `aggregate_type = integration_file`, `change_kind = created`, `operator_id = command.Requestor.IdentifierId`.

- `services/fxfer-domain-api/src/Application/Commands/IntegrationFiles/UpdateIntegrationFileCommandHandler.cs`
  - Type: `public sealed class UpdateIntegrationFileCommandHandler : IRequestHandler<UpdateIntegrationFileCommand, FxferDomainEvent<FxferConfigurationChangedV1>>`
  - Logic:
    - Call `command.Validate()`.
    - Persist the update.
    - Return `FxferConfigurationChangedV1` with `aggregate_type = integration_file`, `change_kind = updated`, `operator_id = command.Requestor.IdentifierId`.

## Mapping (legacy → target)

| Legacy | Target | Note |
| --- | --- | --- |
| `FtpServerCommandHandler.Handle(AddFtpServerCommand)` | `AddFtpServerCommandHandler.Handle(...)` | Split by command type (MediatR). |
| `FtpServerCommandHandler.Handle(UpdateFtpServerCommand)` | `UpdateFtpServerCommandHandler.Handle(...)` | Split by command type. |
| `FtpServerCommandHandler.Handle(InvalidateFtpServerCommand)` | `InvalidateFtpServerCommandHandler.Handle(...)` | Validation is skipped (legacy parity). |
| `FtpServerCommandHandler.ValidateFtpServerCommand(AddFtpServerCommand)` | merged into `AddFtpServerCommandHandler.Handle(...)` / `UpdateFtpServerCommandHandler.Handle(...)` | Validation + duplicate-name check are performed in the handler. |
| `FtpServerCommandHandler.IsFtpServerNameDuplicated(string, Guid)` | merged into handlers | Duplicate-name check inlined using repository query result set. |
| `FtpServerCommandHandler.BuildFtpServerQuery(string)` | merged into handlers | Query construction inlined. |
| `FtpServerCommandHandler.GetFtpServers(FtpServerQuery)` | merged into handlers | Handlers call `IFileTransferReadOnlyRepository.GetFtpServerAsync(...)`. |
| `FtpServerCommandHandler.BuildEvent(AddFtpServerCommand)` | merged into `AddFtpServerCommandHandler.Handle(...)` | Target publishes `Fxfer.ConfigurationChanged` (minimized payload). |
| `FtpServerCommandHandler.BuildEvent(UpdateFtpServerCommand)` | merged into `UpdateFtpServerCommandHandler.Handle(...)` | Target publishes `Fxfer.ConfigurationChanged`. |
| `FtpServerCommandHandler.BuildEvent(InvalidateFtpServerCommand)` | merged into `InvalidateFtpServerCommandHandler.Handle(...)` | Target publishes `Fxfer.ConfigurationChanged`. |
| `FtsInboundTargetCommandHandler.Handle(AddFtsInboundTargetCommand)` | `AddFtsInboundTargetCommandHandler.Handle(...)` | Returns null on duplicate (no-op). |
| `FtsInboundTargetCommandHandler.Handle(InvalidateFtsInboundTargetCommand)` | `InvalidateFtsInboundTargetCommandHandler.Handle(...)` | Returns null when missing (no-op). |
| `FtsInboundTargetCommandHandler.BuildAddFtsInboundTargetEvent(AddFtsInboundTargetCommand)` | merged into `AddFtsInboundTargetCommandHandler.Handle(...)` | Target persists + publishes configuration change event. |
| `FtsInboundTargetCommandHandler.BuildInvalidateFtsInboundTargetEvent(List<FtsInboundTargetModel>)` | merged into `InvalidateFtsInboundTargetCommandHandler.Handle(...)` | Target persists + publishes configuration change event. |
| `FtsInboundTargetCommandHandler.BuildInboundTargetQuery(Guid, RequestorModel)` | merged into inbound-target handlers | Query construction inlined. |
| `FtsIntegrationFileCommandHandler.Handle(AddIntegrationFileCommand)` | `AddIntegrationFileCommandHandler.Handle(...)` | Returns null on missing/invalid `GroupInfo` (no-op). |
| `FtsIntegrationFileCommandHandler.Handle(UpdateIntegrationFileCommand)` | `UpdateIntegrationFileCommandHandler.Handle(...)` | Validation is performed (legacy parity). |
| `FtsIntegrationFileCommandHandler.ValidateGroupInfo(AddIntegrationFileCommand)` | merged into `AddIntegrationFileCommandHandler.Handle(...)` | Group-info predicate is inlined. |
| `FtsIntegrationFileCommandHandler.BuildAddIntegrationFileEvent(AddIntegrationFileCommand)` | merged into `AddIntegrationFileCommandHandler.Handle(...)` | Target persists + publishes configuration change event. |
| `FtsIntegrationFileCommandHandler.BuildUpdateIntegrationFileEvent(UpdateIntegrationFileCommand)` | merged into `UpdateIntegrationFileCommandHandler.Handle(...)` | Target persists + publishes configuration change event. |
| `FtsSourceLocationAddCommandHandler.Handle(FtsSourceLocationAddCommand)` | `AddFtsSourceLocationCommandHandler.Handle(...)` | Allocates ConfigId from sequence; publishes `Fxfer.ConfigurationChanged`. |
| `FtsSourceLocationAddCommandHandler.Handle(FtsSourceLocationUpdateCommand)` | `UpdateFtsSourceLocationCommandHandler.Handle(...)` | Missing ConfigId throws legacy `BusinessException`. |
| `FtsSourceLocationAddCommandHandler.Handle(FtsSourceLocationInvalidateCommand)` | `InvalidateFtsSourceLocationCommandHandler.Handle(...)` | Missing ConfigId throws legacy `BusinessException`. |
| `FtsSourceLocationAddCommandHandler.IsSourceLocationExists(int)` | merged into update/invalidate handlers | Source location lookup by ConfigId only; throws the legacy exception shape when missing. |
| `FtsSourceLocationAddCommandHandler.CreateConfigNumber()` | merged into `AddFtsSourceLocationCommandHandler.Handle(...)` | Sequence read is performed directly against the tenant database. |

## Enterprise-Architecture standards applied

- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Recommended Requirements` (use MediatR handlers and pipeline behaviors for command validation/authorization).
- `standards/security/authorization/STD-SEC-AUTHZ-001-authorization-implementation.md` section `3.4 Multi-Tenant Authorization` (tenant-scoped authorization enforced within the service boundary).
- `standards/application/api/STD-APP-API-001-api-design.md` section `3.5 Error Handling` (authorization/validation failures surface as consistent error responses).
