---
type: Behavior Specification
title: 'backend-bas-file-transfer-api: BAS File Transfer API host wiring'
description: 'What the backend-bas-file-transfer-api component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-bas-file-transfer-api
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:01:04Z
sources:
- id: inventory
  resource: /technical/components/backend-bas-file-transfer-api/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-bas-file-transfer-api
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 10
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Api
  body_sha256: d02a452d3923ca025f366da40931c1efa33f7cc8a5f342dab047d0894bd8e859
---

# backend-bas-file-transfer-api: BAS File Transfer API host wiring

This component is the legacy ASP.NET Core host and composition root for the BAS File Transfer service API.
It wires CQRS command/query routing and registers the service’s handlers/projection writers into the DI container.

## HPP.FileTransfer.BAS.FileTransferApi.Commands.FileTransferSvcEventService
Legacy event distribution service wrapper used by the CQRS framework.

### TryRouteCommand(HPP.Core.BAS.CQRS.Base.Command)
- Purpose: Route an incoming CQRS `Command` to its handler and return the resulting `Event`.
- Inputs: `command` (CQRS command instance).
- Logic: Calls `TryRoute(command)` on the base `EventDistributionServiceBase`.
- Data: None directly (delegated to CQRS infrastructure).
- Errors: Any exceptions from routing/handlers bubble up through the CQRS middleware/controller pipeline.
- Rules: None.

### GetConfigurator()
- Purpose: Provide the module configurator used by CQRS routing/bootstrapping.
- Inputs: None.
- Logic: Returns a new `FileTransferModuleConfigurator()`.
- Data: None.
- Errors: None.
- Rules: None.

## HPP.FileTransfer.BAS.FileTransferApi.FileTransferModuleConfigurator
Legacy CQRS module configurator that registers the File Transfer service’s data context, command handler(s), and projection writer(s) into Autofac.

### ConfigureContractAssemblies(HPP.Core.BAS.CQRS.Interfaces.IDataContractRegistry)
- Purpose: Register contract assemblies (DTOs/events) with the CQRS contract registry.
- Inputs: `contractRegistry`.
- Logic: No-op in legacy implementation.
- Data: None.
- Errors: None.
- Rules: None.

### ConfigureDependencies(Autofac.ContainerBuilder)
- Purpose: Register service dependencies into the Autofac container.
- Inputs: `container`.
- Logic:
  - Registers `FileTransferEntityDbContext` as `IDbContextBase`.
  - Registers command handler `FtsSourceLocationAddCommandHandler` as self.
  - Registers projection writers as self: `FtsSourceLocationProjectionWriter`, `FtpServerProjectionWriter`, `FtsInboundTargetProjectionWriter`, `FtsIntegrationFileProjectionWriter`.
  - All registrations are `InstancePerDependency()`.
- Data:
  - Reads (via downstream handlers/writers): `orm:FTP_SERVER`, `orm:FTS_INBOUND_TARGET`, `orm:FTS_INTEGRATION_FILE`, `orm:FTS_SUSPENSION`.
- Errors: Container build errors if referenced types are missing or misconfigured.
- Rules: None.

### ConfigureEvents(HPP.Core.BAS.CQRS.Interfaces.IDistributableEventRegistry)
- Purpose: Register distributable events with the CQRS event registry.
- Inputs: `registry`.
- Logic: No-op in legacy implementation.
- Data: None.
- Errors: None.
- Rules: None.

## HPP.FileTransfer.BAS.FileTransferApi.Interfaces.IFileTransferSvcQueryService
Legacy service-contract interface defining query operations for File Transfer configuration and tracking searches.

Operations declared (each is a WCF/ServiceContract operation and is exposed via the legacy API controller layer):

- `SearchSourceLocations(SourceLocationQuery query) -> SourceLocationQuery`
- `GetFtpServers(FtpServerQuery query) -> FtpServerQuery`
- `GetSourceLocations(GetSourceLocationQuery query) -> GetSourceLocationQuery`
- `SearchFileTransferMovement(FileTransferTrackingQuery query) -> FileTransferTrackingQuery`
- `SearchInboundTarget(InboundTargetQuery query) -> InboundTargetQuery`

## HPP.FileTransfer.BAS.FileTransferApi.Interfaces.IFileTransferSvcService
Legacy service-contract interface defining command operations for File Transfer configuration writes (create/update/invalidate).

Operations declared (each is a WCF/ServiceContract operation and is exposed via the legacy API controller layer):

- `FileTransferAdd(FtsSourceLocationAddCommand command) -> FtsSourceLocationAdded`
- `UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand command) -> FtsSourceLocationUpdated`
- `InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand command) -> FtsSourceLocationInvalidated`
- `AddFtpServer(AddFtpServerCommand command) -> FtpServerAdded`
- `UpdateFtpServer(UpdateFtpServerCommand command) -> FtpServerUpdated`
- `InvalidateFtpServer(InvalidateFtpServerCommand command) -> FtpServerInvalidated`

## HPP.FileTransfer.BAS.FileTransferApi.Program
Legacy ASP.NET Core process entrypoint.

### Main(string[])
- Purpose: Initialize BAS tenant/CQRS wiring and start the ASP.NET Core host.
- Inputs: `args` (command-line args).
- Logic:
  - Calls `TenantBasHelper.Initialize(new FileTransferModuleConfigurator())`.
  - Builds and runs the host created by `CreateHostBuilder(args)`.
- Data: None directly.
- Errors: Host startup errors and runtime exceptions propagate to process termination/logging.
- Rules: None.

### CreateHostBuilder(string[])
- Purpose: Configure and return the ASP.NET Core generic host builder.
- Inputs: `args`.
- Logic: `Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())`.
- Data: None.
- Errors: Host configuration errors propagate during build/run.
- Rules: None.

## HPP.FileTransfer.BAS.FileTransferApi.Startup
Legacy ASP.NET Core Startup responsible for DI registration, Swagger wiring, and HTTP middleware pipeline configuration.

### ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)
- Purpose: Register MVC controllers, middleware, service implementations, and Swagger generation.
- Inputs: `services`.
- Logic:
  - Registers transient services:
    - `ApiExceptionHandlingMiddleware`
    - `IEventDistribution` -> `FileTransferSvcEventService`
    - `IFileTransferSvcService` -> `FileTransferSvcService`
    - `IFileTransferSvcQueryService` -> `FileTransferSvcQueryService`
  - Adds MVC controllers.
  - Adds SwaggerGen, with:
    - conflict resolution: first description wins
    - schema IDs based on `FullName` with nested `+` replaced by `.`
- Data: None.
- Errors: Service registration errors propagate to host startup failure.
- Rules: None.

### ConfigureDevelopment(Microsoft.AspNetCore.Builder.IApplicationBuilder, Microsoft.AspNetCore.Hosting.IWebHostEnvironment, Microsoft.Extensions.Logging.ILogger<Startup>)
- Purpose: Configure development-only middleware and then apply the standard pipeline.
- Inputs: `app`, `env`, `logger`.
- Logic:
  - Enables developer exception page.
  - Enables Swagger JSON and Swagger UI.
  - Calls `Configure(app, env, logger)`.
- Data: None.
- Errors: Middleware setup errors propagate to host startup.
- Rules: None.

### Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder, Microsoft.AspNetCore.Hosting.IWebHostEnvironment, Microsoft.Extensions.Logging.ILogger<Startup>)
- Purpose: Configure the HTTP request pipeline.
- Inputs: `app`, `env`, `logger`.
- Logic:
  - Logs environment name.
  - Enables HTTPS redirection.
  - Enables routing.
  - Adds `ApiExceptionHandlingMiddleware`.
  - Enables authorization.
  - Maps controllers via endpoint routing.
- Data: None.
- Errors: Runtime middleware/controller exceptions are handled by `ApiExceptionHandlingMiddleware` where applicable; otherwise propagate.
- Rules: None.

## Rules implemented

None (this component owns no business rules).

## Target-state requirements

### T-005 Emit OpenTelemetry traces/metrics/logs from services and workers tagged with tenant_id, correlation_id, and trace_id, and propagate these values end-to-end.
- Purpose: Ensure FXFER service boundaries consistently emit telemetry and preserve tracing/correlation context across calls.
- Inputs:
  - Inbound request trace context headers: `traceparent`, `tracestate`, `baggage`.
  - Inbound correlation header: `X-Correlation-ID`.
  - Tenant identity context (e.g., validated JWT claims) providing `tenant_id`.
- Logic:
  - Configure OpenTelemetry SDK for ASP.NET Core service hosting.
  - Extract/propagate W3C Trace Context and ensure `trace_id` is present for each request.
  - Extract or generate `X-Correlation-ID` at the HTTP boundary and propagate it through logs/outbound calls.
  - Attach `tenant_id`, `correlation_id`, `trace_id` to all spans/logs/metrics (taggable attributes).
- Data: No domain tables owned; telemetry export targets are configured by platform/runtime.
- Errors:
  - Telemetry export failures must not fail requests; they are reported via logs/metrics per platform configuration.
- Target-state section: `inputs/target-state.md` section `12.5 Observability`.

## Not carried over

None.

