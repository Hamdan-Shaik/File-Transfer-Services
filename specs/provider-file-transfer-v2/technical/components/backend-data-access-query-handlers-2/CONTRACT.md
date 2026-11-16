---
type: Interface Contract
title: technical/components/backend-data-access-query-handlers-2/CONTRACT
description: What the backend-data-access-query-handlers-2 component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-query-handlers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T01:10:43Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-query-handlers-2/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-query-handlers-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 8
  methods: 38
  owned_rules: 0
  flows: []
  depends_on:
  - shared-contracts-queries
  - shared-queries-parameters-2
  deployable: fxfer-domain-api
  folder: src/Application/Queries
  body_sha256: d4f987ff27de0caf9d6543309baecdce05b356bc0f2bb996ca0464f94762180b
---

﻿# Contract: `backend-data-access-query-handlers-2`

This component defines internal query-execution classes used by `fxfer-domain-api` query endpoints/handlers to read configuration and tracking data from the tenant-scoped FXFER database.

## Offers

- Query executor classes (application-layer services; no HTTP surface):
  - `FileEventQueryExecutor`: executes `FileEventQuery` and returns `IReadOnlyList<FileEventModel>`.
  - `FileExceptionQueryExecutor`: executes `FileExceptionQuery` and returns `IReadOnlyList<FileExceptionModel>`.
  - `FileTransferTrackingQueryExecutor`: executes `FileTransferTrackingQuery` and returns `IReadOnlyList<FileTransferTrackingModel>` (including per-row `Events` and `Exceptions` enrichment).
  - `GetFtpServerQueryExecutor`: executes `FtpServerQuery` and returns `IReadOnlyList<FtpServerModel>`.
  - `GetSourceLocationQueryExecutor`: executes `GetSourceLocationQuery` and returns `IReadOnlyList<SourceLocationModel>` (with destination list attached to the first row).
  - `InboundTargetQueryExecutor`: executes `InboundTargetQuery` and returns `IReadOnlyList<FtsInboundTargetModel>`.
  - `IntegrationFileQueryExecutor`: executes `GetIntegrationFileQuery` and returns `IReadOnlyList<FtsIntegrationFileModel>`.
  - `SearchSourceLocationQueryExecutor`: executes `SourceLocationQuery` and returns `IReadOnlyList<SourceLocationModel>`.

## Uses

- Component `shared-contracts-queries`:
  - Query request contract types (`FileEventQuery`, `FileExceptionQuery`, `FileTransferTrackingQuery`, `FtpServerQuery`, `GetSourceLocationQuery`, `InboundTargetQuery`, `SourceLocationQuery`).
  - Request validation behavior invoked by query executors (`Validate()`).
- Component `shared-queries-parameters-2`:
  - Integration-file query contract and parameter DTOs (`GetIntegrationFileQuery`, `IntegrationFileQueryParms`).
- `fxfer-domain-api` infrastructure:
  - Tenant-scoped EF Core `FxferDbContext` and entity sets (tables: `orm:FTS_FILE_MANAGEMENT`, `orm:FTS_FILE_EVENT`, `orm:FTS_FILE_EXCEPTION`, `orm:FTS_SOURCE_LOCATION`, `orm:FTS_DESTINATION`, `orm:FTS_INBOUND_TARGET`, `orm:FTS_INTEGRATION_FILE`, `orm:FTP_SERVER`).

## Data owned

None (read-only query executors).

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (query-side handlers are read-only and execute optimized reads).
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (query execution is tenant-scoped; no cross-tenant access).
- `strategy/white-papers/operations/enterprise-kubernetes-strategy.md` section `2.3.4 Secrets Management` (FTP credential material must not be exposed; represent credentials via secret references in target state).
