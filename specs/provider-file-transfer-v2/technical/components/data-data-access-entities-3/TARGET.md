---
type: Target Placement
title: 'Target: data-data-access-entities-3'
description: Where the data-data-access-entities-3 component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/data-data-access-entities-3
tags:
- component
- data
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:32:53Z
sources:
- id: inventory
  resource: /technical/components/data-data-access-entities-3/INVENTORY.yaml
  title: Every legacy class and method (14 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:data-data-access-entities-3
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
  body_sha256: 49cb61999f9eb08ad9b9dc3f37ba02656fbbac8c30a9120cdc0b371b3233ab56
---

# Target: data-data-access-entities-3

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
    - `DbSet<FtsInboundTarget> FtsInboundTarget`
    - `DbSet<FtsIntegrationFile> FtsIntegrationFile`
  - Model configuration:
    - Fluent mapping equivalent to the legacy `OnModelCreating` relationships (FKs and `DeleteBehavior.Restrict`).
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
  - `FtsInboundTarget`
  - `FtsIntegrationFile`

Secret-handling posture (target state):

- Do not store raw credentials or private keys in the tenant database.
- Replace legacy secret columns with secret references/handles:
  - `FtpServer.FtpUser` + `FtpServer.FtpPassword` -> `FtpServer.CredentialSecretRef` (string reference to Secrets Manager).
  - `FtsDestinationLocations.EncryptionPublicKey` / `EncryptionPrivateKey` / `PrivateKeyPassword` -> secret references (for example `EncryptionKeySecretRef`, `PrivateKeySecretRef`, `PrivateKeyPasswordSecretRef`).

### Read-only repository

- Interface `IFileTransferReadOnlyRepository`
  - `Task<IReadOnlyList<FtpServerModel>> GetFtpServerAsync(FtpServerQuery query, CancellationToken cancellationToken)`
  - `Task<IReadOnlyList<FtsInboundTargetModel>> GetInboundTargetAsync(InboundTargetQuery query, CancellationToken cancellationToken)`
  - Purpose: query-side adapter used by query handlers; implemented using `FxferDbContext`.

- Class `FileTransferReadOnlyRepository : IFileTransferReadOnlyRepository`
  - Uses `FxferDbContext` (tenant scoped) and executes LINQ queries equivalent to the legacy query executors.
  - Error behavior parity: on DB/query exceptions, log error including query type + message and return an empty list.

## Mapping

| Legacy | Target | Note |
|---|---|---|
| FileTransferEntityDbContext.Create() | drop: DbContext is created via DI with tenant-scoped connection | legacy constructs DbSession from config; target resolves tenant db connection per request |
| FileTransferEntityDbContext.OnModelCreating(ModelBuilder) | FxferDbContext.OnModelCreating(ModelBuilder) | EF Core fluent configuration + restrict deletes |
| FileTransferReadOnlyRepository.GetFtpServer(FtpServerQuery) | FileTransferReadOnlyRepository.GetFtpServerAsync | Async, EF Core LINQ against tenant db |
| FileTransferReadOnlyRepository.GetInboundTarget(InboundTargetQuery) | FileTransferReadOnlyRepository.GetInboundTargetAsync | Async, EF Core LINQ against tenant db |
| FileTransferReadOnlyRepository.GetResults<Query, QueryExecutor, Model>(Query) | merged into FileTransferReadOnlyRepository methods | EF Core query execution is direct; no generic query-executor wrapper |
| FileTransferReadOnlyRepository.OpenConnection(IDbSession) | drop: EF Core manages connections | no explicit open required |
| FileTransferReadOnlyRepository.ExecuteQuery<Query, QueryExecutor, Model>(Query, IDbSession) | merged into FileTransferReadOnlyRepository methods | direct LINQ replaces legacy executor dispatch |
| FileTransferReadOnlyRepository.FormatErrorMsg(Exception, Query, string) | FileTransferReadOnlyRepository.FormatErrorMsg | same message format, used for logging |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-007-database-per-service-pattern.md` section `Solution Overview` (domain API exclusively accesses its tenant database; other deployables use service APIs)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant isolation; audit posture; encryption expectations)
- `decisions/information/ADR-INF-004-rdbms-platform-strategy.md` section `Decision Summary` (PostgreSQL on managed cloud services as standard)
- `strategy/white-papers/operations/enterprise-kubernetes-strategy.md` section `2.3.4 Secrets Management` (secrets stored in Secrets Manager; avoid embedding secrets in app config/data stores)
- `standards/information/data-modeling/STD-INF-DM-001-canonical-model-standard.md` section `Mandatory Requirements` (persistence entities are not treated as shared seam contracts; shared API shapes must be defined once and referenced)