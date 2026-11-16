---
type: Generation Brief
title: 'Brief: deployable fxfer-db-schema'
description: Instructions the tool gave the writer session for the fxfer-db-schema deployable.
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
  body_sha256: d8a28f44a44fb57a6b37e13cd615b298e55383fb364c955a72805b40943648c7
---

# Brief: deployable `fxfer-db-schema`

- Playbook: `inputs/playbook/deployable.md`
- Target state: `inputs/target-state.md`
- Architecture: `technical/target/architecture.yaml`, `technical/target/TARGET-ARCHITECTURE.md`
- Write exactly these files in `technical/target/deployables/fxfer-db-schema/`: `README.md`, `STRUCTURE.md`, `CONTRACT.md`
- Its path in the new repository: `schema/fxfer-db`

## The deployable (from architecture.yaml)

```yaml
name: fxfer-db-schema
kind: schema
path: schema/fxfer-db
stack: PostgreSQL migrations (Flyway)
role: Per-tenant FXFER database schema migrations.
build: flyway -configFiles=schema/fxfer-db/flyway.conf validate
depends_on: []
```

## Components placed in it

None.

## Deployables it depends on

None.
