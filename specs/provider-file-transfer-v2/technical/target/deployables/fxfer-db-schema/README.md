---
type: Deployable Overview
title: 'Deployable: fxfer-db-schema'
description: 'Overview of the fxfer-db-schema deployable: where it lives, its stack and the flows it serves.'
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
  body_sha256: b0fe70ca81f7be48c60e0c56605bedd2ee581757e4772678b33cda147d159e14
---

# Deployable: `fxfer-db-schema`

`fxfer-db-schema` is the Flyway migration set for the per-tenant FXFER Aurora PostgreSQL database. It defines the authoritative relational schema that FXFER uses for configuration and tracking persistence.

- Path in the new repository: `schema/fxfer-db`
- Kind: `schema`
- Stack: PostgreSQL migrations (Flyway)
- Build: `flyway -configFiles=schema/fxfer-db/flyway.conf validate`

## Dependencies

- Depends on: none.
- Depended on by: `fxfer-domain-api`, `fxfer-projection-worker`.

## Components

No components are placed directly in this deployable; it is the database schema owned by the FXFER bounded context.

| Component | Folder | What it does |
| --- | --- | --- |
| _None_ | _N/A_ | Schema/migrations only |

## Enterprise-Architecture standards applied

- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (per-tenant isolation; encryption at rest/in transit; auditability expectations)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (separate database per tenant)
- `decisions/information/ADR-INF-004-rdbms-platform-strategy.md` section `PostgreSQL Version Standard` (PostgreSQL major-version baseline)

