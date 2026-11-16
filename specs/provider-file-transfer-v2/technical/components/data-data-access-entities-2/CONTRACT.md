---
type: Interface Contract
title: 'Contract: data-data-access-entities-2'
description: What the data-data-access-entities-2 component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/data-data-access-entities-2
tags:
- component
- data
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:22:58Z
sources:
- id: inventory
  resource: /technical/components/data-data-access-entities-2/INVENTORY.yaml
  title: Every legacy class and method (10 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:data-data-access-entities-2
  generation:
    status: done
    attempts: 1
  tier: data
  classes: 10
  methods: 2
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Infrastructure/DataAccess
  body_sha256: 0841ace14b59f124d26adde2b340fef93e9b187c82306fa91965f4dbf207848e
---

# Contract: data-data-access-entities-2

This component is internal to the `fxfer-domain-api` deployable. It defines EF Core persistence entities and the tenant-scoped `FxferDbContext` used by the domain API to access the FXFER tenant database.

## Offers

### In-process persistence types

- `FxferDbContext`
- Entity types in `Infrastructure/DataAccess/Entities/`:
  - `FtpServer`, `FtsSourceLocation`, `FtsDestinationLocations`
  - `FtsFileManagement`, `FtsFileDelivery`, `FtsFileEvent`, `FtsFileException`
  - `FtsSuspension`, `FtsNotification`

## Uses

### Tenant database schema

- Uses the schema owned by deployable `fxfer-db-schema`.
- The EF Core mappings in this component must match the tables/columns created by the Flyway migrations.

### Secrets

- Stores only secret references/handles in the tenant database (no raw passwords, private keys, or key passwords).
- Secret resolution (AWS Secrets Manager access) is performed by higher-level services (API/application layer) and not by the entity classes.

