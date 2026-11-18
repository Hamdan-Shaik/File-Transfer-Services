-- T-009: Per-tenant migration observability (auditable from inside the tenant DB).

CREATE TABLE IF NOT EXISTS fxfer_schema_migration_audit (
  occurred_at timestamp with time zone NOT NULL,
  tenant_id varchar(200) NOT NULL,
  event varchar(50) NOT NULL,
  success boolean NOT NULL
);

CREATE INDEX IF NOT EXISTS ix_fxfer_schema_migration_audit_occurred_at
  ON fxfer_schema_migration_audit (occurred_at);

CREATE INDEX IF NOT EXISTS ix_fxfer_schema_migration_audit_tenant_id
  ON fxfer_schema_migration_audit (tenant_id);

