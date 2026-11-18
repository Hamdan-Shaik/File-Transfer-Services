-- T-009: Record per-tenant migration failures.
-- shortcut: failure rows do not capture the exception text; upgrade to a Java callback if required.

CREATE TABLE IF NOT EXISTS fxfer_schema_migration_audit (
  occurred_at timestamp with time zone NOT NULL,
  tenant_id varchar(200) NOT NULL,
  event varchar(50) NOT NULL,
  success boolean NOT NULL
);

INSERT INTO fxfer_schema_migration_audit (occurred_at, tenant_id, event, success)
VALUES (current_timestamp, '${tenant_id}', 'afterMigrateError', false);

