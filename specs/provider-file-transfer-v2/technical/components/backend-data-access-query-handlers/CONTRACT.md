---
type: Interface Contract
title: 'Contract: backend-data-access-query-handlers'
description: What the backend-data-access-query-handlers component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-query-handlers
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T01:03:24Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-manage-integration-files
  resource: /technical/flows/manage-integration-files/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-data-access-query-handlers/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-query-handlers
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 8
  methods: 43
  owned_rules: 37
  flows: []
  depends_on:
  - shared-contracts-queries
  deployable: fxfer-domain-api
  folder: src/Application/Queries
  body_sha256: d5d18c0058e2ba9577b00b493936cf68e54229062ec94c8d72b795e2f043ec58
---

# Contract: backend-data-access-query-handlers

This component is internal to the `fxfer-domain-api` deployable and does not directly expose HTTP endpoints.
Its contract is the set of query execution behaviors it provides to the domain API query endpoints and other internal callers.

## Provided behavior

- Executes read-only configuration queries:
  - Search source locations (transfer configuration summaries).
  - Get one source location (transfer configuration detail) including destinations.
  - Search FTP server profiles.
  - Look up integration file definitions.
  - Search inbound target records.
- Executes read-only tracking queries:
  - Search file transfer movement/tracking records.
  - For each returned tracking record, loads and attaches file events and file exceptions.

## Consumed components

- Depends on component `shared-contracts-queries` for query request contract types (`*Query`, `Where` parameter types).

## Data read

All access is tenant-scoped via `FxferDbContext` database-per-tenant routing.

- `FTP_SERVER`
- `FTS_SOURCE_LOCATION`
- `FTS_DESTINATION`
- `FTS_INBOUND_TARGET`
- `FTS_INTEGRATION_FILE`
- `FTS_FILE_MANAGEMENT`
- `FTS_FILE_EVENT`
- `FTS_FILE_EXCEPTION`

## Errors

- Validation and parsing errors from query contract validation (`query.Validate()`) propagate to the caller.
- GUID parsing behaviors are preserved:
  - `Guid.Parse(...)` failures throw (for computed GUID properties such as `SoureceLocationGuid` / `OwnerModuleGuid`).
  - `Guid.TryParse(...)` failures fall back to `Guid.Empty` and may still be used as filter values.

## Security posture

- Query behavior must not leak plaintext secrets. In legacy, FTP server query results included `FtpPassword` from `FTP_SERVER`; in the target implementation, credential fields are represented as secret references (per `technical/target/deployables/fxfer-domain-api/CONTRACT.md`).

## Dependencies

- Uses `FxferDbContext` (EF Core + Npgsql) for data access.
- Uses the entity types placed by `data-data-access-entities*` components.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components`
- `patterns/application/api-microservices/PTN-APP-API-016-pagination-pattern.md` section `Solution Overview`
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies`
- `strategy/white-papers/operations/enterprise-kubernetes-strategy.md` section `2.3.4 Secrets Management`

