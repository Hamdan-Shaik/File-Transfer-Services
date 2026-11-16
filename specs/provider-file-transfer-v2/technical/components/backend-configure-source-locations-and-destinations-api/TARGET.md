---
type: Target Placement
title: 'Target: backend-configure-source-locations-and-destinations-api'
description: Where the backend-configure-source-locations-and-destinations-api component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-configure-source-locations-and-destinations-api
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T06:28:48Z
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
  resource: /technical/components/backend-configure-source-locations-and-destinations-api/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-configure-source-locations-and-destinations-api
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 49
  owned_rules: 22
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  - search-and-track-file-transfers
  depends_on:
  - backend-bas-file-transfer-api
  - backend-bas-file-transfer-svc
  - backend-bas-file-transfer-svc-2
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  deployable: fxfer-domain-api
  folder: src/Api
  body_sha256: 10f75ec45960739b98cc621e0e75f5357962aef2d1f14ac6c760057ed55a333a
---

# Target: `backend-configure-source-locations-and-destinations-api`

## Where

- Deployable: `fxfer-domain-api`
- Folder: `services/fxfer-domain-api/src/Api`

## What to create

Create the domain API endpoints that replace the legacy WCF/CoreWCF/CQRS service doors for configuration and tracking.

Create endpoint implementations under `services/fxfer-domain-api/src/Api/Endpoints`:

- `services/fxfer-domain-api/src/Api/Endpoints/Commands/*`
  - Command endpoints for:
    - transfer configuration create/update/state-change (source locations + destinations)
    - FTP server create/update/invalidate
    - integration file update/processing recording

- `services/fxfer-domain-api/src/Api/Endpoints/Queries/*`
  - Query endpoints for:
    - transfer configuration search and get-by-id
    - source-location pick-list lookup
    - FTP server search/list
    - integration file definition lookup
    - inbound target lookup
    - file tracking search

Endpoint behavior requirements (parity with the legacy doors):

- Each endpoint delegates to the application layer (command/query handlers) and returns the handler result without additional mapping/validation beyond route/parameter binding.
- Unhandled exceptions bubble into the API-wide exception handling middleware (target-state Problem Details response).

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.SearchSourceLocations(SourceLocationQuery)` | `TransferConfigurationsQueries.SearchTransferConfigurations` | Implement as `GET /api/fxfer-domain/v1/transfer-configurations`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.SearchFileTransferMovement(FileTransferTrackingQuery)` | `FileTrackingQueries.SearchFileTracking` | Implement as `GET /api/fxfer-domain/v1/file-tracking`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.GetFtpServers(FtpServerQuery)` | `FtpServersQueries.SearchFtpServers` | Implement as `GET /api/fxfer-domain/v1/ftp-servers`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.GetSourceLocations(GetSourceLocationQuery)` | `TransferConfigurationsQueries.GetTransferConfiguration` | Implement as `GET /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.SearchInboundTarget(InboundTargetQuery)` | `InboundTargetsQueries.ListInboundTargets` | Implement as `GET /api/fxfer-domain/v1/inbound-targets`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.GetIntegrationFile(GetIntegrationFileQuery)` | `IntegrationFilesQueries.ListIntegrationFiles` | Implement as `GET /api/fxfer-domain/v1/integration-files`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.TryRouteQuery(Query)` | `drop: legacy CQRS routing wrapper` | Target calls MediatR/query handlers directly from endpoints.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.GetConfigurator()` | `drop: legacy CQRS module configurator hook` | Target DI registration is performed in `Program.cs`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.FileTransferAdd(FtsSourceLocationAddCommand)` | `TransferConfigurationsCommands.CreateTransferConfiguration` | Implement as `POST /api/fxfer-domain/v1/transfer-configurations`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand)` | `TransferConfigurationsCommands.UpdateTransferConfiguration` | Implement as `PUT /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand)` | `TransferConfigurationsCommands.PatchTransferConfiguration` | Implement as `PATCH /api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.AddFtpServer(AddFtpServerCommand)` | `FtpServersCommands.CreateFtpServer` | Implement as `POST /api/fxfer-domain/v1/ftp-servers`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.UpdateFtpServer(UpdateFtpServerCommand)` | `FtpServersCommands.UpdateFtpServer` | Implement as `PUT /api/fxfer-domain/v1/ftp-servers/{ftpServerId}`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.UpdateIntegrationFile(UpdateIntegrationFileCommand)` | `IntegrationFilesCommands.PatchIntegrationFile` | Implement as `PATCH /api/fxfer-domain/v1/integration-files/{integrationFileId}`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.InvalidateFtpServer(InvalidateFtpServerCommand)` | `FtpServersCommands.PatchFtpServer` | Implement as `PATCH /api/fxfer-domain/v1/ftp-servers/{ftpServerId}`.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.TryRouteCommand(Command)` | `drop: legacy CQRS routing wrapper` | Target calls MediatR/command handlers directly from endpoints.
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.GetConfigurator()` | `drop: legacy CQRS module configurator hook` | Target DI registration is performed in `Program.cs`.
| `HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService.FileTransferAdd(FtsSourceLocationAddCommand)` | `merged into TransferConfigurationsCommands.CreateTransferConfiguration` | Legacy API-hosted service variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService.UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand)` | `merged into TransferConfigurationsCommands.UpdateTransferConfiguration` | Legacy API-hosted service variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService.InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand)` | `merged into TransferConfigurationsCommands.PatchTransferConfiguration` | Legacy API-hosted service variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService.AddFtpServer(AddFtpServerCommand)` | `merged into FtpServersCommands.CreateFtpServer` | Legacy API-hosted service variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService.UpdateFtpServer(UpdateFtpServerCommand)` | `merged into FtpServersCommands.UpdateFtpServer` | Legacy API-hosted service variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService.InvalidateFtpServer(InvalidateFtpServerCommand)` | `merged into FtpServersCommands.PatchFtpServer` | Legacy API-hosted service variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService.TryRouteCommand(HPP.Core.BAS.CQRS.Base.Command)` | `drop: legacy CQRS routing wrapper` | Target calls MediatR/command handlers directly from endpoints.
| `HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService.GetConfigurator()` | `drop: legacy CQRS module configurator hook` | Target DI registration is performed in `Program.cs`.
| `HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService.SearchSourceLocations(SourceLocationQuery)` | `merged into TransferConfigurationsQueries.SearchTransferConfigurations` | Legacy API-hosted query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService.SearchFileTransferMovement(FileTransferTrackingQuery)` | `merged into FileTrackingQueries.SearchFileTracking` | Legacy API-hosted query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService.GetFtpServers(FtpServerQuery)` | `merged into FtpServersQueries.SearchFtpServers` | Legacy API-hosted query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService.GetSourceLocations(GetSourceLocationQuery)` | `merged into TransferConfigurationsQueries.GetTransferConfiguration` | Legacy API-hosted query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService.SearchInboundTarget(InboundTargetQuery)` | `merged into InboundTargetsQueries.ListInboundTargets` | Legacy API-hosted query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService.TryRouteQuery(HPP.Core.BAS.CQRS.Base.Query)` | `drop: legacy CQRS routing wrapper` | Target calls MediatR/query handlers directly from endpoints.
| `HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService.GetConfigurator()` | `drop: legacy CQRS module configurator hook` | Target DI registration is performed in `Program.cs`.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.SearchSourceLocations(SourceLocationQuery)` | `merged into TransferConfigurationsQueries.SearchTransferConfigurations` | Legacy core query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.SearchFileTransferMovement(FileTransferTrackingQuery)` | `merged into FileTrackingQueries.SearchFileTracking` | Legacy core query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.GetFtpServers(FtpServerQuery)` | `merged into FtpServersQueries.SearchFtpServers` | Legacy core query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.GetSourceLocations(GetSourceLocationQuery)` | `merged into TransferConfigurationsQueries.GetTransferConfiguration` | Legacy core query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.SearchInboundTarget(InboundTargetQuery)` | `merged into InboundTargetsQueries.ListInboundTargets` | Legacy core query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.GetIntegrationFile(GetIntegrationFileQuery)` | `merged into IntegrationFilesQueries.ListIntegrationFiles` | Legacy core query variant is unified into the single domain API query endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.TryRouteQuery(HPP.Core.BAS.CQRS.Base.Query)` | `drop: legacy CQRS routing wrapper` | Target calls MediatR/query handlers directly from endpoints.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService.GetConfigurator()` | `drop: legacy CQRS module configurator hook` | Target DI registration is performed in `Program.cs`.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.FileTransferAdd(FtsSourceLocationAddCommand)` | `merged into TransferConfigurationsCommands.CreateTransferConfiguration` | Legacy core command variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand)` | `merged into TransferConfigurationsCommands.UpdateTransferConfiguration` | Legacy core command variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand)` | `merged into TransferConfigurationsCommands.PatchTransferConfiguration` | Legacy core command variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.AddFtpServer(AddFtpServerCommand)` | `merged into FtpServersCommands.CreateFtpServer` | Legacy core command variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.UpdateFtpServer(UpdateFtpServerCommand)` | `merged into FtpServersCommands.UpdateFtpServer` | Legacy core command variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.UpdateIntegrationFile(UpdateIntegrationFileCommand)` | `merged into IntegrationFilesCommands.PatchIntegrationFile` | Legacy core command variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.InvalidateFtpServer(InvalidateFtpServerCommand)` | `merged into FtpServersCommands.PatchFtpServer` | Legacy core command variant is unified into the single domain API command endpoint.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.TryRouteCommand(HPP.Core.BAS.CQRS.Base.Command)` | `drop: legacy CQRS routing wrapper` | Target calls MediatR/command handlers directly from endpoints.
| `HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService.GetConfigurator()` | `drop: legacy CQRS module configurator hook` | Target DI registration is performed in `Program.cs`.

## Dependencies

- `backend-bas-file-transfer-api`
  - Uses: API host wiring (middleware, auth, Problem Details error formatting) that exposes these endpoints.

- `backend-bas-file-transfer-svc`
  - Uses: legacy SOAP parity contract surface (used by `fxfer-soap-facade`), not by the HTTP endpoints.

- `backend-bas-file-transfer-svc-2`
  - Uses: legacy SOAP/CoreWCF parity contract surface (used by `fxfer-soap-facade`), not by the HTTP endpoints.

- `backend-business-command-handlers`
  - Uses: application-layer command handlers for transfer configuration, FTP server, and integration file mutations.

- `backend-business-command-handlers-2`
  - Uses: legacy duplicate/alternate namespace implementation; target behavior remains unified.

Legacy note: `hpp.core.bas.cqrs` is not carried over as a runtime dependency; its CQRS router wrappers are replaced by explicit HTTP endpoints calling MediatR handlers.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Variation 1: Contract-First (Design-First) - **Recommended for Gainwell**` (author OpenAPI before implementation; use it for governance and generation)
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Step 1: Define Error Schema in OpenAPI Specification` (standardized Problem Details schema documented in OpenAPI)
- `patterns/application/api-microservices/PTN-APP-API-016-pagination-pattern.md` section `Solution Overview` (standard pagination envelope for collection endpoints)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Component 3: JWT Validation Middleware (API Services)` (JWT validation middleware)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Best Practice 2: Validate All JWT Claims` (issuer/audience/signature/expiry validation)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (extract/generate and propagate correlation id)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` header format and propagation)
