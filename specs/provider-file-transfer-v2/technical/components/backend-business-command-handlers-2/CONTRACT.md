---
type: Interface Contract
title: 'Contract: backend-business-command-handlers-2'
description: What the backend-business-command-handlers-2 component offers other components and what it uses from them.
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
  body_sha256: c0f9de03790e4e6cc77d6fbdb592448b2e779657f8c773e07d9d81c55b388493
---

# Contract: backend-business-command-handlers-2

This component is an alternate-namespace variant of the legacy FXFER configuration mutation command handlers (FTP servers, transfer configurations/source locations, inbound targets, and integration files).

In the target state, these behaviors are implemented once by component `technical/components/backend-business-command-handlers/` in the `fxfer-domain-api` application layer. This component therefore does not add a second set of target command handler types; it documents the additional legacy code path that must remain behaviorally consistent.

## Offers

- Legacy behavior inventory for command handler methods in namespace `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandHandlers`.

## Uses

### Component dependencies (target-state naming)

- `backend-business-command-handlers`
  - Uses: canonical target implementation of these behaviors (single set of MediatR command handlers).

- `backend-data-access-event-handlers`
  - Used by: downstream projection worker implementations that apply configuration-change events to projection tables.

- `backend-data-access-event-handlers-2`
  - Used by: canonical target projection-writer implementations (`FtpServerProjectionWriter`, `FtsSourceLocationProjectionWriter`, etc.).

- `shared-contracts-commands`
  - Uses: command DTOs (`AddFtpServerCommand`, `UpdateFtpServerCommand`, `InvalidateFtpServerCommand`, `FtsSourceLocationAddCommand`, `FtsSourceLocationUpdateCommand`, `FtsSourceLocationInvalidateCommand`, `AddFtsInboundTargetCommand`, `InvalidateFtsInboundTargetCommand`, `AddIntegrationFileCommand`, `UpdateIntegrationFileCommand`).

### Legacy package dependencies (contracts to keep)

- `automapper`
  - Used by: FTP server add/update event construction (`MapperWrapper.Mapper.Map(...)`).

- `hpp.core.bas.cqrs`
  - Used by: legacy `CommandHandler` base type and CQRS `Event` / `BusinessException` contracts.

## Data

- Reads (legacy read model / repository queries): FTP server rows and inbound target rows (existence/duplicate checks).
- Reads (legacy query executor): source location rows filtered by `ConfigId`.
- Sequence: allocates configuration ids from `FTS_SOURCE_LOCATION_SEQ`.
- Writes: none directly (legacy handlers only return CQRS `Event` payloads; persistence occurs elsewhere in the legacy pipeline).

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Command Side Components` (command handlers validate and apply mutations before publishing events).
- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Technical Specifications` (separate command handlers per command type in the target).