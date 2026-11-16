---
type: Target Placement
title: 'Target: backend-file-transfer-mgmt-ctrl'
description: Where the backend-file-transfer-mgmt-ctrl component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-mgmt-ctrl
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T06:39:37Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-mgmt-ctrl/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-mgmt-ctrl
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 4
  methods: 2
  owned_rules: 0
  flows: []
  depends_on:
  - backend-configure-source-locations-and-destinations-api
  deployable: fxfer-transfer-worker
  folder: src/Infrastructure/Adapters
  body_sha256: 80655c9107d5f448115d9a1c50571f56541fef0ec4917b793c4e1ac19d28e44f
---

# Target: backend-file-transfer-mgmt-ctrl

## Where

Deployable: fxfer-transfer-worker

Target folder: workers/fxfer-transfer-worker/src/Infrastructure/Adapters

## What to create

Create these types in `workers/fxfer-transfer-worker/src/Infrastructure/Adapters`. These are worker-internal adapters; they do not access tenant databases directly.

- Interface `IFileTransferSvcQueryServiceProvider`
  - `FtsIntegrationFileModel? GetIntegrationFile(GetIntegrationFileQuery query)`
  - Purpose: Preserve the legacy batch call shape (`GetIntegrationFileQuery` -> one `FtsIntegrationFileModel`), while sourcing data from `fxfer-domain-api`.

- Class `FileTransferSvcQueryServiceProvider` : `IFileTransferSvcQueryServiceProvider`
  - Constructor inputs:
    - `IFxferDomainApiClient domainApiClient`
    - `IRequestContextAccessor requestContextAccessor` (tenant_id, correlation_id, W3C trace context)
  - `GetIntegrationFile(GetIntegrationFileQuery query)`:
    - Builds domain API query parameters from `query.Where`:
      - `integrationFileId` from `query.Where.FTSIntegrationFileId` when it is not `Guid.Empty`.
      - `fileName` from `query.Where.FileName` when it is not null/empty.
      - `isValid = true` (legacy batch behavior expects only currently-valid integration files).
    - Calls `GET /api/fxfer-domain/v1/integration-files` via `domainApiClient`.
    - If the returned list is null/empty, returns null.
    - Otherwise returns the first item mapped to `FtsIntegrationFileModel`.
    - Parity: returns only the first match (legacy `FirstOrDefault()` behavior).

- Interface `IFileTransferSvcServiceProvider`
  - `IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command)`
  - Purpose: Preserve the legacy batch call shape (`UpdateIntegrationFileCommand` -> `IntegrationFileUpdated`) while updating through `fxfer-domain-api`.

- Class `FileTransferSvcServiceProvider` : `IFileTransferSvcServiceProvider`
  - Constructor inputs:
    - `IFxferDomainApiClient domainApiClient`
    - `IRequestContextAccessor requestContextAccessor` (tenant_id, correlation_id, W3C trace context)
  - `UpdateIntegrationFile(UpdateIntegrationFileCommand command)`:
    - Calls `command.Validate()` to preserve the legacy behavior when `ConfigId` is missing (throws a legacy `BusinessException`).
    - Calls `PATCH /api/fxfer-domain/v1/integration-files/{integrationFileId}` with:
      - `integrationFileId = command.FtsIntegrationFileId`
      - request body: `{ configId = command.ConfigId.Value, isValid = command.IsValid }`
    - Maps the returned `IntegrationFileDefinition` into a legacy `IntegrationFileUpdated` event:
      - `FtsIntegrationFileId`: from the returned definition.
      - `ConfigId`: from the returned definition when present, else `command.ConfigId`.
      - `FileName`: from the returned definition when present, else `command.FileName`.
      - `GroupInfo`: from `command.GroupInfo` (legacy event includes group list while the domain API exposes one `groupId`).
      - `ETFOutboundMetadataId`: from the returned definition when present, else `command.ETFOutboundMetadataDefnId`.
      - `IsProcessed`: from the returned definition.
      - `IsValid`: from the returned definition.
      - `LastModifiedDate`: from the returned definition.
      - `OperatorId`: set from the legacy base `Command` identifier (the value batch code sets as `IdentifierId`).

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| FileTransferSvcQueryServiceProvider.GetIntegrationFile(GetIntegrationFileQuery) | FileTransferSvcQueryServiceProvider.GetIntegrationFile | Replaced BAS/WCF query door with `fxfer-domain-api` `GET /integration-files`; preserves `FirstOrDefault()` behavior. |
| FileTransferSvcServiceProvider.UpdateIntegrationFile(UpdateIntegrationFileCommand) | FileTransferSvcServiceProvider.UpdateIntegrationFile | Replaced BAS/WCF command door with `fxfer-domain-api` `PATCH /integration-files/{integrationFileId}`; maps response to `IntegrationFileUpdated`. |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-007-database-per-service-pattern.md` section `Solution Overview` (worker adapters must not read/write a shared database; use service APIs)
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (domain API errors are Problem Details; client must treat them as the error contract)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (domain API is OpenAPI-first; clients generated/validated from contract)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant-scoped execution context; no cross-tenant access)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (propagate W3C TraceContext on outbound HTTP calls)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` propagation)

<!-- END -->

