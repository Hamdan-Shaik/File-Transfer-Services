# Schema migrations

## T-009: Expand/contract discipline

Schema migrations follow an **expand → migrate → contract** discipline to keep tenant databases safe during rolling deploys:

- **Expand**: add new nullable columns/tables first; keep old readers working.
- **Migrate**: backfill data where required.
- **Contract**: remove old columns only after all readers are upgraded.

## T-009: Repeatable, idempotent, observable

- Run `flyway migrate` **per tenant** by passing tenant-specific connection details (`FLYWAY_URL`, `FLYWAY_USER`, `FLYWAY_PASSWORD`).
- Write SQL migrations to be **idempotent** (prefer `IF NOT EXISTS`, additive DDL first).
- Migrations are **observable per tenant** via `fxfer_schema_migration_audit` rows written by Flyway SQL callbacks.

