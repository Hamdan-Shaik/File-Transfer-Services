---
type: Interface Contract
title: technical/components/backend-business-command-handlers/CONTRACT
description: What the backend-business-command-handlers component offers other components and what it uses from them.
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
  body_sha256: f8ffcab0986cb90e55974f5a260c22810543482fda3681279adb0a7d2288f319
---

﻿# Contract: backend-business-command-handlers

This component provides the `fxfer-domain-api` application-layer command handling for FXFER configuration mutations (FTP servers, transfer configurations/source locations, inbound targets, and integration files).

It enforces legacy validation/no-op semantics on the write path and publishes configuration-change events for downstream projections.

## Offers

- MediatR command handlers for:
  - FTP server create/update/invalidate
  - Transfer configuration create/update/invalidate (source location + destinations)
  - Inbound target create/invalidate
  - Integration file create/update
- Command pipeline behavior enforcing tenant-scoped authorization (T-003).
- Emission of `Fxfer.ConfigurationChanged` (v1) domain events (`FxferDomainEvent<FxferConfigurationChangedV1>`) describing the change kind and aggregate identity.

## Uses

### Component dependencies

- `shared-contracts-commands`
  - Uses: command DTOs (`AddFtpServerCommand`, `UpdateFtpServerCommand`, `InvalidateFtpServerCommand`, `FtsSourceLocationAddCommand`, `FtsSourceLocationUpdateCommand`, `FtsSourceLocationInvalidateCommand`, `AddFtsInboundTargetCommand`, `InvalidateFtsInboundTargetCommand`, `AddIntegrationFileCommand`, `UpdateIntegrationFileCommand`).

- `backend-data-access-event-handlers`
  - Used by: downstream projection worker implementations that apply `Fxfer.ConfigurationChanged` to projection tables.

- `backend-data-access-event-handlers-2`
  - Used by: the canonical target projection-writer implementations (`FtpServerProjectionWriter`, `FtsSourceLocationProjectionWriter`, etc.).

- `backend-business-command-handlers-2`
  - Legacy note: duplicate/alternate-namespace implementation; target behavior must remain consistent across both variants.

### Deployable-internal dependencies

- Uses tenant-scoped persistence (`FxferDbContext`) and read-only repository adapters (`IFileTransferReadOnlyRepository`) owned by the `fxfer-domain-api` deployable.

## Data

- Reads (tenant-scoped): `orm:FTP_SERVER`, `orm:FTS_SOURCE_LOCATION`, `orm:FTS_INBOUND_TARGET`.
- Writes (tenant-scoped): write-model tables for FTP servers, transfer configurations, inbound targets, and integration files.
- Sequence: allocates configuration ids from `FTS_SOURCE_LOCATION_SEQ`.

## Enterprise-Architecture standards applied

- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Command Handler Responsibilities` (command handlers validate rules, apply mutations, and publish domain events).
- `standards/security/authorization/STD-SEC-AUTHZ-001-authorization-implementation.md` section `3.4 Multi-Tenant Authorization` (tenant isolation enforced at the service boundary).
- `standards/application/api/STD-APP-API-001-api-design.md` section `3.5 Error Handling` (validation/authorization failures surface as standard errors).
