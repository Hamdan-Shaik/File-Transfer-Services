---
type: Target Placement
title: 'Target: data-data-access-entities-2'
description: Where the data-data-access-entities-2 component goes in the new system and what to build, method by method.
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
  body_sha256: cd66fd28f83ea7d495d4f869dfa88d9bd906f82f8c89ade2ae6e13bfe9c5b012
---

# Target: data-data-access-entities-2

## Where

Deployable: fxfer-domain-api

Target folder: services/fxfer-domain-api/src/Infrastructure/DataAccess

## What to create

Create these types under `services/fxfer-domain-api/src/Infrastructure/DataAccess`.

### DbContext

- Class `FxferDbContext : DbContext`
  - Location: `services/fxfer-domain-api/src/Infrastructure/DataAccess/FxferDbContext.cs`.
  - Purpose: EF Core context for the tenant-scoped Aurora PostgreSQL database.
  - Construction: created via DI with tenant-scoped connection string resolved outside this component (tenant routing is handled by the API layer).
  - DbSets:
    - `DbSet<FtpServer> FtpServers`
    - `DbSet<FtsSourceLocation> FtsSourceLocations`
    - `DbSet<FtsDestinationLocations> FtsDestinationLocations`
    - `DbSet<FtsFileManagement> FtsFileManagement`
    - `DbSet<FtsFileDelivery> FtsFileDelivery`
    - `DbSet<FtsFileEvent> FtsFileEvents`
    - `DbSet<FtsFileException> FtsFileExceptions`
    - `DbSet<FtsSuspension> FtsSuspension`
    - `DbSet<FtsNotification> FtsNotification`
  - Model configuration:
    - Fluent mapping equivalent to the legacy `OnModelCreating` (table names, keys, relationships).
    - Delete behavior must be `Restrict` / `NoAction` for relationships that were configured with `WillCascadeOnDelete(false)` in legacy.
    - Relationships to configure:
      - `FtpServer` -> `FtsSourceLocation` (FK `FtsSourceLocation.FtpServerId`).
      - `FtsSourceLocation` -> `FtsFileManagement` (FK `FtsFileManagement.FileSourceLocationId`).
      - `FtsDestinationLocations` -> `FtsFileDelivery` (FK `FtsFileDelivery.DestinationId`).
      - `FtsDestinationLocations` -> `FtsFileEvent` (FK `FtsFileEvent.DestinationId`).
      - `FtsDestinationLocations` -> `FtsFileException` (FK `FtsFileException.DestinationId`).
      - `FtsFileManagement` -> `FtsFileDelivery` / `FtsFileEvent` / `FtsFileException` (FK `FileId`).
      - `FtsFileEvent` -> `FtsFileException` (FK `EventId`).
    - `FtsFileManagement.FileStatus` must be mapped as fixed-length (`char(1)` in PostgreSQL).
    - Table/column names must match the schema created by `fxfer-db-schema`.

### Entities

- Create EF Core entity types in `services/fxfer-domain-api/src/Infrastructure/DataAccess/Entities/` corresponding to the legacy entity set:
  - `FtpServer`
  - `FtsSourceLocation`
  - `FtsDestinationLocations`
  - `FtsFileManagement`
  - `FtsFileDelivery`
  - `FtsFileEvent`
  - `FtsFileException`
  - `FtsSuspension`
  - `FtsNotification`

Secret-handling posture (target state):

- Do not store raw credentials, private keys, or key passwords in the tenant database.
- Replace legacy secret columns with secret references/handles:
  - `FtpServer.FtpUser` + `FtpServer.FtpPassword` -> `FtpServer.CredentialSecretRef` (string reference to Secrets Manager).
  - `FtsDestinationLocations.EncryptionPublicKey` / `EncryptionPrivateKey` / `PrivateKeyPassword` -> secret references (for example `EncryptionKeySecretRef`, `PrivateKeySecretRef`, `PrivateKeyPasswordSecretRef`).

## Mapping

| Legacy | Target | Note |
|---|---|---|
| FileTransferEntityDbContext.Create() | drop: DbContext is created via DI with tenant-scoped connection | legacy constructs DbSession from config; target resolves tenant db connection per request |
| FileTransferEntityDbContext.OnModelCreating(DbModelBuilder) | FxferDbContext.OnModelCreating(ModelBuilder) | EF Core fluent configuration + restrict deletes |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-007-database-per-service-pattern.md` section `Solution Overview` (domain API exclusively accesses its tenant database; other deployables use service APIs)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant isolation; audit posture; encryption expectations)
- `decisions/information/ADR-INF-004-rdbms-platform-strategy.md` section `Decision Summary` (PostgreSQL on managed cloud services as standard)
- `strategy/white-papers/operations/enterprise-kubernetes-strategy.md` section `2.3.4 Secrets Management` (secrets stored in Secrets Manager; avoid embedding secrets in app config/data stores)
- `standards/information/data-modeling/STD-INF-DM-001-canonical-model-standard.md` section `Mandatory Requirements` (persistence entities are not treated as shared seam contracts; shared API shapes must be defined once and referenced)

