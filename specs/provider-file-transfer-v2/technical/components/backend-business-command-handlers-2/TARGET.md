---
type: Target Placement
title: 'Target: backend-business-command-handlers-2'
description: Where the backend-business-command-handlers-2 component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-command-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T06:11:41Z
sources:
- id: inventory
  resource: /technical/components/backend-business-command-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-command-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 25
  owned_rules: 0
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  depends_on:
  - backend-business-command-handlers
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - shared-contracts-commands
  deployable: fxfer-domain-api
  folder: src/Application/Commands
  body_sha256: 0a6c63c63561e77999d9a5bd58c5df6f601b3c03a84ba47263cd04cae42adcaf
---

# Target: backend-business-command-handlers-2

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Application/Commands`

## What to build

This component is a legacy alternate-namespace implementation of the same configuration-write command handling behaviors covered by component `technical/components/backend-business-command-handlers/`.

Do not create a second set of command handler types in the target for these legacy classes. Implement the target behavior once (in `services/fxfer-domain-api/src/Application/Commands`) and treat this component’s legacy methods as merged into those same target handlers.

Legacy-only plumbing:

- `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers.FtsSourceLocationRepositoryHandler` is not carried over (legacy CQRS base type); target uses DI + MediatR handlers.

## Legacy → Target mapping

| Legacy method | Target method | Notes |
| --- | --- | --- |
| `FtpServerCommandHandler.Handle(AddFtpServerCommand)` | `AddFtpServerCommandHandler.Handle(...)` | Same behavior as canonical component; duplicated-name check uses `IsValid=true` and exact-name match. |
| `FtpServerCommandHandler.Handle(UpdateFtpServerCommand)` | `UpdateFtpServerCommandHandler.Handle(...)` | Same behavior as canonical component. |
| `FtpServerCommandHandler.Handle(InvalidateFtpServerCommand)` | `InvalidateFtpServerCommandHandler.Handle(...)` | Validation is skipped (legacy parity). |
| `FtpServerCommandHandler.BuildEvent(AddFtpServerCommand)` | merged into `AddFtpServerCommandHandler.Handle(...)` | Legacy uses AutoMapper; target builds the persisted record + emitted change event directly. |
| `FtpServerCommandHandler.BuildEvent(UpdateFtpServerCommand)` | merged into `UpdateFtpServerCommandHandler.Handle(...)` | Legacy uses AutoMapper; target builds the persisted record + emitted change event directly. |
| `FtpServerCommandHandler.BuildEvent(InvalidateFtpServerCommand)` | merged into `InvalidateFtpServerCommandHandler.Handle(...)` | Legacy returns `FtpServerInvalidated { IsValid=false }`; target persists `is_valid=false` and emits an invalidation change event. |
| `FtpServerCommandHandler.ValidateFtpServerCommand(AddFtpServerCommand)` | merged into `AddFtpServerCommandHandler.Handle(...)` / `UpdateFtpServerCommandHandler.Handle(...)` | Validation + duplicate-name check are performed in the handler. |
| `FtpServerCommandHandler.IsFtpServerNameDuplicated(string, Guid)` | merged into FTP-server handlers | Duplicate-name check inlined using repository query result set. |
| `FtpServerCommandHandler.GetFtpServers(FtpServerQuery)` | merged into FTP-server handlers | Handler calls the read-only repository query. |
| `FtpServerCommandHandler.BuildFtpServerQuery(string)` | merged into FTP-server handlers | Query construction inlined. |
| `FtsInboundTargetCommandHandler.Handle(AddFtsInboundTargetCommand)` | `AddFtsInboundTargetCommandHandler.Handle(...)` | Legacy returns `new Event()` for no-op; target uses a no-op return value (no persistence, no emitted change event). |
| `FtsInboundTargetCommandHandler.Handle(InvalidateFtsInboundTargetCommand)` | `InvalidateFtsInboundTargetCommandHandler.Handle(...)` | Legacy returns `new Event()` when missing; target uses a no-op return value (no persistence, no emitted change event). |
| `FtsInboundTargetCommandHandler.BuildAddFtsInboundTargetEvent(AddFtsInboundTargetCommand)` | merged into `AddFtsInboundTargetCommandHandler.Handle(...)` | Target persists + emits a configuration change event. |
| `FtsInboundTargetCommandHandler.BuildInvalidateFtsInboundTargetEvent(List<FtsInboundTargetModel>)` | merged into `InvalidateFtsInboundTargetCommandHandler.Handle(...)` | Target persists + emits a configuration change event. |
| `FtsInboundTargetCommandHandler.BuildInboundTargetQuery(Guid, RequestorModel)` | merged into inbound-target handlers | Query construction inlined. |
| `FtsIntegrationFileCommandHandler.Handle(AddIntegrationFileCommand)` | `AddIntegrationFileCommandHandler.Handle(...)` | Legacy returns `new Event()` when `GroupInfo` is missing/invalid; target uses a no-op return value (no persistence, no emitted change event). |
| `FtsIntegrationFileCommandHandler.Handle(UpdateIntegrationFileCommand)` | `UpdateIntegrationFileCommandHandler.Handle(...)` | Validation is performed (legacy parity). |
| `FtsIntegrationFileCommandHandler.ValidateGroupInfo(AddIntegrationFileCommand)` | merged into `AddIntegrationFileCommandHandler.Handle(...)` | Group-info predicate is inlined. |
| `FtsIntegrationFileCommandHandler.BuildAddIntegrationFileEvent(AddIntegrationFileCommand)` | merged into `AddIntegrationFileCommandHandler.Handle(...)` | Target persists + emits a configuration change event. |
| `FtsIntegrationFileCommandHandler.BuildUpdateIntegrationFileEvent(UpdateIntegrationFileCommand)` | merged into `UpdateIntegrationFileCommandHandler.Handle(...)` | Target persists + emits a configuration change event. |
| `FtsSourceLocationAddCommandHandler.Handle(FtsSourceLocationAddCommand)` | `AddFtsSourceLocationCommandHandler.Handle(...)` | Allocates `ConfigId` from sequence `FTS_SOURCE_LOCATION_SEQ` and emits a configuration change event. |
| `FtsSourceLocationAddCommandHandler.Handle(FtsSourceLocationUpdateCommand)` | `UpdateFtsSourceLocationCommandHandler.Handle(...)` | Missing `ConfigId` throws the legacy `BusinessException` shape. |
| `FtsSourceLocationAddCommandHandler.Handle(FtsSourceLocationInvalidateCommand)` | `InvalidateFtsSourceLocationCommandHandler.Handle(...)` | Missing `ConfigId` throws the legacy `BusinessException` shape. |
| `FtsSourceLocationAddCommandHandler.IsSourceLocationExists(int)` | merged into update/invalidate handlers | Lookup is filtered only by `ConfigId`; missing `ConfigId` throws the same legacy exception shape. |
| `FtsSourceLocationAddCommandHandler.CreateConfigNumber()` | merged into `AddFtsSourceLocationCommandHandler.Handle(...)` | Sequence read is performed directly against the tenant database. |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Command Side Components` (command handlers execute business logic/validation on the write path).
- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Prohibited Practices` (target uses one handler per command type; this legacy multi-command handler is decomposed).