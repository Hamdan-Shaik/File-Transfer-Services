---
type: Interface Contract
title: technical/components/data-data-access-entities/CONTRACT
description: What the data-data-access-entities component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/data-data-access-entities
tags:
- component
- data
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:16:01Z
sources:
- id: inventory
  resource: /technical/components/data-data-access-entities/INVENTORY.yaml
  title: Every legacy class and method (14 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:data-data-access-entities
  generation:
    status: done
    attempts: 1
  tier: data
  classes: 14
  methods: 8
  owned_rules: 0
  flows:
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Infrastructure/DataAccess
  body_sha256: 44b8a8df4bcb928e61311810c1a61fdcc6ced0d053db5cc80c7e5442793eddd8
---

﻿# Contract: data-data-access-entities

This component is internal to the `fxfer-domain-api` deployable. It defines EF Core persistence entities, the tenant-scoped `FxferDbContext`, and a small read-only repository used by query handlers.

## Offers

### In-process persistence types

- `FxferDbContext`
- Entity types in `Infrastructure/DataAccess/Entities/`:
  - `FtpServer`, `FtsSourceLocation`, `FtsDestinationLocations`
  - `FtsFileManagement`, `FtsFileDelivery`, `FtsFileEvent`, `FtsFileException`
  - `FtsSuspension`, `FtsNotification`
  - `FtsInboundTarget`, `FtsIntegrationFile`

### In-process query adapter

- `IFileTransferReadOnlyRepository`
  - `GetFtpServerAsync(FtpServerQuery, CancellationToken)`
  - `GetInboundTargetAsync(InboundTargetQuery, CancellationToken)`

## Uses

### Tenant database schema

- Uses the schema owned by deployable `fxfer-db-schema`.
- The EF Core mappings in this component must match the tables/columns created by the Flyway migrations.

### Secrets

- Stores only secret references/handles in the tenant database (no raw passwords, private keys, or key passwords).
- Secret resolution (AWS Secrets Manager access) is performed by higher-level services (API/application layer) and not by the entity classes.
