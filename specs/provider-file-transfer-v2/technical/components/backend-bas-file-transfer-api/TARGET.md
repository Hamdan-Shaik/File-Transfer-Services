---
type: Target Placement
title: 'Target: backend-bas-file-transfer-api'
description: Where the backend-bas-file-transfer-api component goes in the new system and what to build, method by method.
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
  body_sha256: 2e3979ff34d502a7d096c455aad064a6fae8c85be7b6c6a00b8c3871787c0453
---

# Target: `backend-bas-file-transfer-api`

## Where

- Deployable: `fxfer-domain-api`
- Folder: `services/fxfer-domain-api/src/Api`

## What to create

Create the API host composition and wiring for `fxfer-domain-api`.

- `services/fxfer-domain-api/src/Api/Program.cs`
  - Configure ASP.NET Core hosting.
  - Register API-layer services and middleware used by endpoint components placed in `src/Api`.
  - Configure OpenTelemetry (traces/metrics/logs) and correlation/tenant tagging per T-005.

- Service registration equivalent to `FileTransferModuleConfigurator.ConfigureDependencies(...)`
  - Use `Microsoft.Extensions.DependencyInjection` registrations (target-state stack) rather than Autofac.
  - Ensure API-layer registrations are present for:
    - request exception handling middleware (target-state middleware in `services/fxfer-domain-api/src/Api/Middleware`)
    - command/query endpoints (target-state endpoints in `services/fxfer-domain-api/src/Api/Endpoints`)
    - any required DbContext/handler/projection-writer registrations provided by other components placed in this deployable.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `FileTransferSvcEventService.TryRouteCommand(Command)` | `drop: legacy CQRS EventDistributionServiceBase routing` | Target uses explicit ASP.NET Core endpoints + application handlers; no 1:1 routing wrapper. |
| `FileTransferSvcEventService.GetConfigurator()` | `drop: legacy CQRS module configurator hook` | Target DI registration is performed in `Program.cs` / service-registration extensions. |
| `FileTransferModuleConfigurator.ConfigureContractAssemblies(IDataContractRegistry)` | `drop: legacy CQRS contract registry` | Target contracts are plain .NET DTOs in `src/Contracts/*` without runtime registry. |
| `FileTransferModuleConfigurator.ConfigureDependencies(ContainerBuilder)` | `Program.ConfigureServices` | Implement DI registrations using built-in DI; ensure equivalent type availability. |
| `FileTransferModuleConfigurator.ConfigureEvents(IDistributableEventRegistry)` | `drop: legacy CQRS event registry` | Target event publishing is handled via outbox/event adapter (deployable-level contract). |
| `Program.Main(string[])` | `Program.Main(string[])` | Merge legacy two-step init+host-run into ASP.NET Core minimal hosting entrypoint. |
| `Program.CreateHostBuilder(string[])` | `Program.Main(string[])` | Minimal hosting replaces `CreateHostBuilder` pattern. |
| `Startup.ConfigureServices(IServiceCollection)` | `Program.Main(string[])` | Register services/middleware on `builder.Services`. |
| `Startup.ConfigureDevelopment(IApplicationBuilder, IWebHostEnvironment, ILogger)` | `Program.Main(string[])` | Use `if (app.Environment.IsDevelopment()) { ... }` to toggle dev-only pipeline pieces. |
| `Startup.Configure(IApplicationBuilder, IWebHostEnvironment, ILogger)` | `Program.Main(string[])` | Configure middleware order on `app` (routing, auth, exception handling, endpoints). |

## Dependencies

This component does not depend on other components by contract, but it must wire API hosting to the other `fxfer-domain-api` components placed in the same deployable (handlers, DbContext, endpoints, contracts).

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `Solution Overview` (OpenAPI as the mandatory API contract format; contract-first posture)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (use OpenTelemetry SDK/API; export via OTLP; propagate W3C trace context)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (attribute naming conventions; custom attributes)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (generate/extract correlation id at boundaries; propagate through logs)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` header; UUID v4 format)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Component 3: JWT Validation Middleware (API Services)` (JWT validation middleware in API services)
- `patterns/security/api-microservices/PTN-SEC-API-002-jwt-token-pattern.md` section `Best Practice 2: Validate All JWT Claims` (issuer/audience/signature/expiry validation)

