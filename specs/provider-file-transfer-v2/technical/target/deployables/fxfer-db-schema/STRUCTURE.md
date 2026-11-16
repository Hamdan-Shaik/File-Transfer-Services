---
type: Deployable Structure
title: 'Structure: fxfer-db-schema'
description: 'Skeleton of the fxfer-db-schema deployable: projects, folders and cross-cutting setup.'
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-db-schema
tags:
- deployable
- provider-file-transfer-v2
- schema
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T15:50:16Z
sources:
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (0 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-db-schema
  generation:
    status: done
    attempts: 1
  path: schema/fxfer-db
  kind: schema
  stack: PostgreSQL migrations (Flyway)
  components: 0
  depends_on: []
  body_sha256: 9437d6deb17cf89843e74d026a525af62fa64627dfe80d23f4cee2f26a202541
---

# Structure: `fxfer-db-schema`

Deployable path: `schema/fxfer-db`

This deployable is a Flyway migration set that is executed against each tenant's Aurora PostgreSQL database for FXFER. Its repository structure is SQL-first (no application code), and it is deployed/run before any FXFER service version that depends on new schema is rolled out.

## Folder skeleton (new repository)

Create the following skeleton at `schema/fxfer-db`:

```text
schema/fxfer-db/
  flyway.conf
  sql/
    V1__baseline.sql
    V2__...sql
    R__...sql
  Dockerfile
  helm/
    fxfer-db-schema/
      Chart.yaml
      values.yaml
      templates/
        job-migrate.yaml
        job-validate.yaml
```

## Flyway configuration

- `schema/fxfer-db/flyway.conf` sets:
  - `flyway.locations=filesystem:schema/fxfer-db/sql`
  - `flyway.validateMigrationNaming=true`
  - `flyway.sqlMigrationPrefix=V` and `flyway.repeatableSqlMigrationPrefix=R`
- Connection details are injected at runtime per tenant (credentials stored and encrypted per tenant in AWS Secrets Manager + KMS). The config uses placeholders/env var substitution for at least `flyway.url`, `flyway.user`, and `flyway.password`.

## Execution model (per-tenant)

- CI/build uses `flyway validate` (as defined in `technical/target/architecture.yaml`).
- Deployment runs `flyway migrate` against each tenant database sequentially before deploying FXFER service images that require the new schema.
- Migrations are written to be safe to apply repeatedly (idempotent where applicable) across many isolated tenant databases.

## Migration discipline

- Expand/contract is mandatory for backward compatibility across rolling upgrades:
  - Expand: add new nullable columns/tables first; keep old readers working.
  - Migrate: backfill data where required.
  - Contract: remove old columns only after all readers are upgraded.
- Treat migrations as forward-only changes (rollback is handled operationally by restoring a prior DB snapshot/PITR when required, not by automatic down scripts).

## PostgreSQL compatibility

- Target databases are Aurora PostgreSQL per tenant.
- PostgreSQL major versions: minimum 16; recommended 17 (subject to Aurora region availability).

## Enterprise-Architecture standards applied

- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (separate database per tenant; migration fan-out runs per tenant database)
- `decisions/information/ADR-INF-004-rdbms-platform-strategy.md` section `PostgreSQL Version Standard` (PostgreSQL version baseline for Aurora/RDS)
- `strategy/guidance/application/GUID-APP-DB-001-forward-only-migration-strategy.md` section `Decision Summary` (forward-only, idempotent migration posture; apply as a pre-rollout Job)

