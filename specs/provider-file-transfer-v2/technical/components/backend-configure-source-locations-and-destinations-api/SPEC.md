---
type: Behavior Specification
title: 'backend-configure-source-locations-and-destinations-api: configuration + tracking CQRS service doors'
description: 'What the backend-configure-source-locations-and-destinations-api component does today: every class and method, and the business rules it implements.'
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
  body_sha256: be423e4752f7cbf5d874df8a49f54654b27ab2fd521cd35a2734c40551120758
---

# backend-configure-source-locations-and-destinations-api: configuration + tracking CQRS service doors

This component defines the legacy service-layer doors used for FXFER configuration (source locations + destinations, FTP servers, integration files) and for tracking/lookup queries.
Each door method is a thin wrapper around CQRS routing (`TryRoute(...)`) and returns the routed command/query result cast back to the expected event/query type.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
Legacy UA3 WCF query service that routes File Transfer configuration and tracking queries through CQRS.

### SearchSourceLocations(SourceLocationQuery)
- Purpose: Route a source-location search query through CQRS and return the routed query result.
- Inputs: `query` (`SourceLocationQuery`).
- Logic: Returns `TryRouteQuery(query) as SourceLocationQuery`.
- Data: None directly (downstream query handler reads configuration tables; includes `orm:FTP_SERVER`).
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-079.

### SearchFileTransferMovement(FileTransferTrackingQuery)
- Purpose: Route a file transfer tracking search query through CQRS and return the routed query result.
- Inputs: `query` (`FileTransferTrackingQuery`).
- Logic: Returns `TryRouteQuery(query) as FileTransferTrackingQuery`.
- Data: None directly (downstream query handler reads tracking projection/read model).
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F05-019.

### GetFtpServers(FtpServerQuery)
- Purpose: Route an FTP server lookup/search query through CQRS and return the routed query result.
- Inputs: `query` (`FtpServerQuery`).
- Logic: Returns `TryRouteQuery(query) as FtpServerQuery`.
- Data: None directly (downstream query handler reads `orm:FTP_SERVER`).
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-001.

### GetSourceLocations(GetSourceLocationQuery)
- Purpose: Route a source-location details query through CQRS and return the routed query result.
- Inputs: `query` (`GetSourceLocationQuery`).
- Logic: Returns `TryRouteQuery(query) as GetSourceLocationQuery`.
- Data: None directly (downstream query handler reads transfer configuration projection/read model).
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-081.

### SearchInboundTarget(InboundTargetQuery)
- Purpose: Route an inbound-target lookup/search query through CQRS and return the routed query result.
- Inputs: `query` (`InboundTargetQuery`).
- Logic: Returns `TryRouteQuery(query) as InboundTargetQuery`.
- Data: None directly (downstream query handler reads inbound-target projection/read model).
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F05-021, F05-034.

### GetIntegrationFile(GetIntegrationFileQuery)
- Purpose: Route an integration-file definition lookup query through CQRS and return the routed query result.
- Inputs: `query` (`GetIntegrationFileQuery`).
- Logic: Returns `TryRouteQuery(query) as GetIntegrationFileQuery`.
- Data: None directly (downstream query handler reads integration-file projection/read model).
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F03-001.

### TryRouteQuery(Query)
- Purpose: Delegate CQRS query routing to the base implementation.
- Inputs: `query` (CQRS base `Query`).
- Logic: Returns `TryRoute(query)`.
- Data: None directly.
- Errors: Any routing/handler exception bubbles up.
- Rules: F03-001, F05-019, F05-021.

### GetConfigurator()
- Purpose: Provide the CQRS module configurator instance used by `TryRoute(...)` routing.
- Inputs: None.
- Logic: Returns `new FileTransferModuleConfigurator()`.
- Data: None.
- Errors: None.
- Rules: None.

## Rules implemented

| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-034 | Routes `FileTransferAdd` command and returns routed result cast as `FtsSourceLocationAdded`. | FileTransferSvcService.FileTransferAdd | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-035 | Routes API-hosted `FileTransferAdd` command and returns routed result cast as `FtsSourceLocationAdded`. | FileTransferSvcService.FileTransferAdd | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-036 | Routes core `FileTransferAdd` command and returns routed result cast as `FtsSourceLocationAdded`. | FileTransferSvcService.FileTransferAdd | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-055 | Routes `UpdateFileTransferSourceLocation` command and returns routed result cast as `FtsSourceLocationUpdated`. | FileTransferSvcService.UpdateFileTransferSourceLocation | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-057 | Routes `InvalidateFileTransferSourceLocation` command and returns routed result cast as `FtsSourceLocationInvalidated`. | FileTransferSvcService.InvalidateFileTransferSourceLocation | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-079 | Routes `SearchSourceLocations` query and returns the routed query object. | FileTransferSvcQueryService.SearchSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-081 | Routes `GetSourceLocations` query and returns the routed query object. | FileTransferSvcQueryService.GetSourceLocations | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-107 | Projection update parses `FtsSourceLocationsId` as GUID using `new Guid(...)` and `Guid.Parse(...)` without guarding; malformed/missing values throw and the update does not apply. | FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationUpdated) | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-108 | Projection invalidation parses `FtsSourceLocationsId` as GUID using `new Guid(...)` and `Guid.Parse(...)` without guarding; malformed/missing values throw and the invalidation does not apply. | FtsSourceLocationProjectionWriter.Handle(FtsSourceLocationInvalidated) | [configure-source-locations-and-destinations](../../../technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F02-001 | Routes WCF `GetFtpServers` query via `TryRouteQuery` and returns the routed query object. | FileTransferSvcQueryService.GetFtpServers | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-002 | Routes API-hosted `GetFtpServers` query via `TryRouteQuery` and returns the routed query object. | FileTransferSvcQueryService.GetFtpServers | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-003 | Routes core `GetFtpServers` query via `TryRouteQuery` and returns the routed query object. | FileTransferSvcQueryService.GetFtpServers | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-017 | Routes `AddFtpServer` command to the command router and returns routed result cast to `FtpServerAdded`. | FileTransferSvcService.AddFtpServer | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-019 | Routes `UpdateFtpServer` command to the command router and returns routed result cast to `FtpServerUpdated`. | FileTransferSvcService.UpdateFtpServer | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-021 | Routes `InvalidateFtpServer` command to the command router and returns routed result cast to `FtpServerInvalidated`. | FileTransferSvcService.InvalidateFtpServer | [manage-ftp-servers-and-credentials](../../../technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F03-001 | Routes `GetIntegrationFile` query through the File Transfer query router. | FileTransferSvcQueryService.GetIntegrationFile | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |
| F03-002 | Routes API-hosted `GetIntegrationFile` query through the File Transfer query router. | FileTransferSvcQueryService.GetIntegrationFile | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |
| F03-003 | Routes `UpdateIntegrationFile` command through the File Transfer command router. | FileTransferSvcService.UpdateIntegrationFile | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |
| F03-004 | Routes API-hosted `UpdateIntegrationFile` command through the File Transfer command router. | FileTransferSvcService.UpdateIntegrationFile | [manage-integration-files](../../../technical/flows/manage-integration-files/RULES.md) |
| F05-019 | Routes `SearchFileTransferMovement` query through CQRS routing and returns the routed query object. | FileTransferSvcQueryService.SearchFileTransferMovement | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-021 | Routes `SearchInboundTarget` query through CQRS routing and returns the routed query object. | FileTransferSvcQueryService.SearchInboundTarget | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |
| F05-034 | `InboundTargetQuery.Validate()` throws only if messages exist, but `Validate()` adds no messages (so it does not throw). | InboundTargetQuery.Validate | [search-and-track-file-transfers](../../../technical/flows/search-and-track-file-transfers/RULES.md) |

## Not carried over

None.

## HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
Legacy core service query surface that routes configuration and tracking queries through CQRS.

### SearchSourceLocations(SourceLocationQuery)
- Purpose: Route a source-location search query through CQRS and return the routed query result.
- Inputs: `query` (`SourceLocationQuery`).
- Logic: Returns `TryRouteQuery(query) as SourceLocationQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-079.

### SearchFileTransferMovement(FileTransferTrackingQuery)
- Purpose: Route a tracking search query through CQRS and return the routed query result.
- Inputs: `query` (`FileTransferTrackingQuery`).
- Logic: Returns `TryRouteQuery(query) as FileTransferTrackingQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F05-019.

### GetFtpServers(FtpServerQuery)
- Purpose: Route an FTP server lookup/search query through CQRS and return the routed query result.
- Inputs: `query` (`FtpServerQuery`).
- Logic: Returns `TryRouteQuery(query) as FtpServerQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-003.

### GetSourceLocations(GetSourceLocationQuery)
- Purpose: Route a source-location details query through CQRS and return the routed query result.
- Inputs: `query` (`GetSourceLocationQuery`).
- Logic: Returns `TryRouteQuery(query) as GetSourceLocationQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-081.

### SearchInboundTarget(InboundTargetQuery)
- Purpose: Route an inbound-target lookup/search query through CQRS and return the routed query result.
- Inputs: `query` (`InboundTargetQuery`).
- Logic: Returns `TryRouteQuery(query) as InboundTargetQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F05-021.

### GetIntegrationFile(GetIntegrationFileQuery)
- Purpose: Route an integration-file definition lookup query through CQRS and return the routed query result.
- Inputs: `query` (`GetIntegrationFileQuery`).
- Logic: Returns `TryRouteQuery(query) as GetIntegrationFileQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F03-002.

### TryRouteQuery(HPP.Core.BAS.CQRS.Base.Query)
- Purpose: Delegate CQRS query routing to the base implementation.
- Inputs: `query` (CQRS base `Query`).
- Logic: Returns `TryRoute(query)`.
- Data: None directly.
- Errors: Any routing/handler exception bubbles up.
- Rules: F02-003, F03-002, F05-019, F05-021.

### GetConfigurator()
- Purpose: Provide the CQRS module configurator instance used by `TryRoute(...)` routing.
- Inputs: None.
- Logic: Returns `new FileTransferModuleConfigurator()`.
- Data: None.
- Errors: None.
- Rules: None.

## HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService
Legacy core service command surface that routes File Transfer configuration commands through CQRS.

### FileTransferAdd(FtsSourceLocationAddCommand)
- Purpose: Route a source-location add command and return the routed result cast as `FtsSourceLocationAdded`.
- Inputs: `command` (`FtsSourceLocationAddCommand`).
- Logic: Returns `TryRouteCommand(command) as FtsSourceLocationAdded`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-036.

### UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand)
- Purpose: Route a source-location update command and return the routed result cast as `FtsSourceLocationUpdated`.
- Inputs: `command` (`FtsSourceLocationUpdateCommand`).
- Logic: Returns `TryRouteCommand(command) as FtsSourceLocationUpdated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-055.

### InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand)
- Purpose: Route a source-location invalidation command and return the routed result cast as `FtsSourceLocationInvalidated`.
- Inputs: `command` (`FtsSourceLocationInvalidateCommand`).
- Logic: Returns `TryRouteCommand(command) as FtsSourceLocationInvalidated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-057.

### AddFtpServer(AddFtpServerCommand)
- Purpose: Route an FTP server add command and return the routed result cast as `FtpServerAdded`.
- Inputs: `command` (`AddFtpServerCommand`).
- Logic: Returns `TryRouteCommand(command) as FtpServerAdded`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-017.

### UpdateFtpServer(UpdateFtpServerCommand)
- Purpose: Route an FTP server update command and return the routed result cast as `FtpServerUpdated`.
- Inputs: `command` (`UpdateFtpServerCommand`).
- Logic: Returns `TryRouteCommand(command) as FtpServerUpdated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-019.

### UpdateIntegrationFile(UpdateIntegrationFileCommand)
- Purpose: Route an integration file definition update command and return the routed result cast as `IntegrationFileUpdated`.
- Inputs: `command` (`UpdateIntegrationFileCommand`).
- Logic: Returns `TryRouteCommand(command) as IntegrationFileUpdated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F03-004.

### InvalidateFtpServer(InvalidateFtpServerCommand)
- Purpose: Route an FTP server invalidation command and return the routed result cast as `FtpServerInvalidated`.
- Inputs: `command` (`InvalidateFtpServerCommand`).
- Logic: Returns `TryRouteCommand(command) as FtpServerInvalidated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-021.

### TryRouteCommand(HPP.Core.BAS.CQRS.Base.Command)
- Purpose: Delegate CQRS command routing to the base implementation.
- Inputs: `command` (CQRS base `Command`).
- Logic: Returns `TryRoute(command)`.
- Data: None directly.
- Errors: Any routing/handler exception bubbles up.
- Rules: F01-036, F01-055, F01-057, F02-017, F02-019, F02-021, F03-004.

### GetConfigurator()
- Purpose: Provide the CQRS module configurator instance used by `TryRoute(...)` routing.
- Inputs: None.
- Logic: Returns `new FileTransferModuleConfigurator()`.
- Data: None.
- Errors: None.
- Rules: None.

## HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcService
Legacy API-hosted command service surface that routes configuration commands through CQRS (HPP namespace variant).

### FileTransferAdd(FtsSourceLocationAddCommand)
- Purpose: Route a source-location add command and return the routed result cast as `FtsSourceLocationAdded`.
- Inputs: `command` (`FtsSourceLocationAddCommand`).
- Logic: Returns `TryRouteCommand(command) as FtsSourceLocationAdded`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-035.

### UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand)
- Purpose: Route a source-location update command and return the routed result cast as `FtsSourceLocationUpdated`.
- Inputs: `command` (`FtsSourceLocationUpdateCommand`).
- Logic: Returns `TryRouteCommand(command) as FtsSourceLocationUpdated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-055.

### InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand)
- Purpose: Route a source-location invalidation command and return the routed result cast as `FtsSourceLocationInvalidated`.
- Inputs: `command` (`FtsSourceLocationInvalidateCommand`).
- Logic: Returns `TryRouteCommand(command) as FtsSourceLocationInvalidated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-057.

### AddFtpServer(AddFtpServerCommand)
- Purpose: Route an FTP server add command and return the routed result cast as `FtpServerAdded`.
- Inputs: `command` (`AddFtpServerCommand`).
- Logic: Returns `TryRouteCommand(command) as FtpServerAdded`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-017.

### UpdateFtpServer(UpdateFtpServerCommand)
- Purpose: Route an FTP server update command and return the routed result cast as `FtpServerUpdated`.
- Inputs: `command` (`UpdateFtpServerCommand`).
- Logic: Returns `TryRouteCommand(command) as FtpServerUpdated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-019.

### InvalidateFtpServer(InvalidateFtpServerCommand)
- Purpose: Route an FTP server invalidation command and return the routed result cast as `FtpServerInvalidated`.
- Inputs: `command` (`InvalidateFtpServerCommand`).
- Logic: Returns `TryRouteCommand(command) as FtpServerInvalidated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-021.

### TryRouteCommand(HPP.Core.BAS.CQRS.Base.Command)
- Purpose: Delegate CQRS command routing to the base implementation.
- Inputs: `command` (CQRS base `Command`).
- Logic: Returns `TryRoute(command)`.
- Data: None directly.
- Errors: Any routing/handler exception bubbles up.
- Rules: F01-035, F01-055, F01-057, F02-017, F02-019, F02-021.

### GetConfigurator()
- Purpose: Provide the CQRS module configurator instance used by `TryRoute(...)` routing.
- Inputs: None.
- Logic: Returns `new FileTransferModuleConfigurator()`.
- Data: None.
- Errors: None.
- Rules: None.

## HPP.FileTransfer.BAS.FileTransferApi.Queries.FileTransferSvcQueryService
Legacy API-hosted query service surface that routes configuration/tracking queries through CQRS (HPP namespace variant).

### SearchSourceLocations(SourceLocationQuery)
- Purpose: Route a source-location search query through CQRS and return the routed query result.
- Inputs: `query` (`SourceLocationQuery`).
- Logic: Returns `TryRouteQuery(query) as SourceLocationQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-079.

### SearchFileTransferMovement(FileTransferTrackingQuery)
- Purpose: Route a tracking search query through CQRS and return the routed query result.
- Inputs: `query` (`FileTransferTrackingQuery`).
- Logic: Returns `TryRouteQuery(query) as FileTransferTrackingQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F05-019.

### GetFtpServers(FtpServerQuery)
- Purpose: Route an FTP server lookup/search query through CQRS and return the routed query result.
- Inputs: `query` (`FtpServerQuery`).
- Logic: Returns `TryRouteQuery(query) as FtpServerQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-002.

### GetSourceLocations(GetSourceLocationQuery)
- Purpose: Route a source-location details query through CQRS and return the routed query result.
- Inputs: `query` (`GetSourceLocationQuery`).
- Logic: Returns `TryRouteQuery(query) as GetSourceLocationQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-081.

### SearchInboundTarget(InboundTargetQuery)
- Purpose: Route an inbound-target lookup/search query through CQRS and return the routed query result.
- Inputs: `query` (`InboundTargetQuery`).
- Logic: Returns `TryRouteQuery(query) as InboundTargetQuery`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F05-021.

### TryRouteQuery(HPP.Core.BAS.CQRS.Base.Query)
- Purpose: Delegate CQRS query routing to the base implementation.
- Inputs: `query` (CQRS base `Query`).
- Logic: Returns `TryRoute(query)`.
- Data: None directly.
- Errors: Any routing/handler exception bubbles up.
- Rules: F02-002, F05-019, F05-021.

### GetConfigurator()
- Purpose: Provide the CQRS module configurator instance used by `TryRoute(...)` routing.
- Inputs: None.
- Logic: Returns `new FileTransferModuleConfigurator()`.
- Data: None.
- Errors: None.
- Rules: None.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcService
Legacy UA3 WCF command service that routes File Transfer configuration commands through CQRS.

### FileTransferAdd(FtsSourceLocationAddCommand)
- Purpose: Route a source-location add command and return the routed result cast as `FtsSourceLocationAdded`.
- Inputs: `command` (`FtsSourceLocationAddCommand`).
- Logic: Returns `TryRouteCommand(command) as FtsSourceLocationAdded`.
- Data: None directly (downstream command handler persists configuration and publishes/projection-writes).
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-034.

### UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand)
- Purpose: Route a source-location update command and return the routed result cast as `FtsSourceLocationUpdated`.
- Inputs: `command` (`FtsSourceLocationUpdateCommand`).
- Logic: Returns `TryRouteCommand(command) as FtsSourceLocationUpdated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-055, F01-107.

### InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand)
- Purpose: Route a source-location invalidation command and return the routed result cast as `FtsSourceLocationInvalidated`.
- Inputs: `command` (`FtsSourceLocationInvalidateCommand`).
- Logic: Returns `TryRouteCommand(command) as FtsSourceLocationInvalidated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F01-057, F01-108.

### AddFtpServer(AddFtpServerCommand)
- Purpose: Route an FTP server add command and return the routed result cast as `FtpServerAdded`.
- Inputs: `command` (`AddFtpServerCommand`).
- Logic: Returns `TryRouteCommand(command) as FtpServerAdded`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-017.

### UpdateFtpServer(UpdateFtpServerCommand)
- Purpose: Route an FTP server update command and return the routed result cast as `FtpServerUpdated`.
- Inputs: `command` (`UpdateFtpServerCommand`).
- Logic: Returns `TryRouteCommand(command) as FtpServerUpdated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-019.

### UpdateIntegrationFile(UpdateIntegrationFileCommand)
- Purpose: Route an integration file definition update command and return the routed result cast as `IntegrationFileUpdated`.
- Inputs: `command` (`UpdateIntegrationFileCommand`).
- Logic: Returns `TryRouteCommand(command) as IntegrationFileUpdated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F03-003.

### InvalidateFtpServer(InvalidateFtpServerCommand)
- Purpose: Route an FTP server invalidation command and return the routed result cast as `FtpServerInvalidated`.
- Inputs: `command` (`InvalidateFtpServerCommand`).
- Logic: Returns `TryRouteCommand(command) as FtpServerInvalidated`.
- Data: None directly.
- Errors: Exceptions thrown by routing/handlers bubble to the caller.
- Rules: F02-021.

### TryRouteCommand(Command)
- Purpose: Delegate CQRS command routing to the base implementation.
- Inputs: `command` (CQRS base `Command`).
- Logic: Returns `TryRoute(command)`.
- Data: None directly.
- Errors: Any routing/handler exception bubbles up.
- Rules: F01-034, F01-055, F01-057, F02-017, F02-019, F02-021, F03-003.

### GetConfigurator()
- Purpose: Provide the CQRS module configurator instance used by `TryRoute(...)` routing.
- Inputs: None.
- Logic: Returns `new FileTransferModuleConfigurator()`.
- Data: None.
- Errors: None.
- Rules: None.
