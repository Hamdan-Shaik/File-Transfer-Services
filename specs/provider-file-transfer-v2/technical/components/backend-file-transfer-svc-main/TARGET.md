---
type: Target Placement
title: 'Target: backend-file-transfer-svc-main'
description: Where the backend-file-transfer-svc-main component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-svc-main
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T20:16:25Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-svc-main/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-svc-main
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 14
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-soap-facade
  folder: src/Host
  body_sha256: 296a404a07062b07e4563d09bbda96b613975e5e94e98a0551fab8374f6939c8
---

# Target: `backend-file-transfer-svc-main`

Where: `services/fxfer-soap-facade/src/Host`

Build the ASP.NET Core (.NET 10) host startup for `fxfer-soap-facade`. This replaces the legacy Service Fabric host process with a Kubernetes-friendly entry point that (1) configures OpenTelemetry + correlation/trace propagation, (2) registers AutoMapper profiles needed by the SOAP service implementations, (3) maps the CoreWCF SOAP endpoints, and (4) optionally runs best-effort tenant startup probes.

## What to create

### `Program`

`src/Host/Program.cs`

- `public static Task Main(string[] args)`
  - Build the web host (`WebApplicationBuilder` / `WebApplication`).
  - Call `SetProfiles(services)`.
  - Call `InitializeInstrumentationSettings(configuration, services)`.
  - Load tenant ids from configuration (target-state config source) and, when present, call `new Startup(...).InitializeTenants(tenantIds)`.
  - Configure CoreWCF and map SOAP endpoints (see `SoapEndpointHost.MapSoapEndpoints`).
  - Expose health endpoints required by the deployable (`GET /api/health/live`, `GET /api/health/ready`).
  - Block by running the web host (`app.RunAsync()`).

- `internal static void InitializeInstrumentationSettings(IConfiguration configuration, IServiceCollection services)`
  - Configure OpenTelemetry traces/metrics/logs and propagation for W3C Trace Context + `X-Correlation-ID`.
  - Read `InstrumentationKey` from configuration and, when non-empty, configure the chosen exporter/backend for parity with existing telemetry wiring.

- `internal static void SetProfiles(IServiceCollection services)`
  - Register AutoMapper profiles equivalent to legacy `InboundTargetProfile` and `IntegrationFileProfile` assembly scanning.

### `SoapEndpointHost`

`src/Host/Soap/SoapEndpointHost.cs`

- `internal static void MapSoapEndpoints(WebApplication app)`
  - Register CoreWCF services and endpoints for:
    - `FileTransferSvcService` at `/soap/file-transfer/v1/FileTransferSvcService.svc`
    - `FileTransferSvcQueryService` at `/soap/file-transfer/v1/FileTransferSvcQueryService.svc`
    - `FileTransferSvcEventService` at `/soap/file-transfer/v1/FileTransferSvcEventService.svc`
  - Apply the binding/transport requirements defined by the deployable contract (`technical/target/deployables/fxfer-soap-facade/CONTRACT.md`).

### `ServiceEventSource`

`src/Host/Logging/ServiceEventSource.cs`

Implement a lightweight host logging facade that preserves the legacy event intents (message, service host init, request start/stop). Use `ILogger` + OpenTelemetry spans/events rather than ETW-only semantics.

- `public void Message(string message)`
- `public void Message(string message, params object[] args)`
- `public void ServiceMessage(string serviceName, string serviceTypeName, long replicaOrInstanceId, Guid partitionId, string applicationName, string applicationTypeName, string nodeName, string message)`
- `public void ServiceMessage(/* host request context */, string message, params object[] args)`
- `public void ServiceTypeRegistered(int hostProcessId, string serviceType)`
- `public void ServiceHostInitializationFailed(string exception)`
- `public void ServiceRequestStart(string requestTypeName)`
- `public void ServiceRequestStop(string requestTypeName, string exception)`

Include a nested `public static class Keywords` with the same keyword names for parity (even if only used as labels).

### `Startup`

`src/Host/Startup.cs`

- `public Task InitializeTenants(IReadOnlyCollection<string> tenantIds, CancellationToken ct = default)`
  - For each `tenantId`, perform a best-effort health probe for the tenant (target equivalent of legacy `CheckServiceHealth`).
  - If any probe throws, log a warning and continue/return without failing process startup.

## Mapping

| Legacy | Target | Note |
|---|---|---|
| `Main.CreateServiceInstanceListeners` | `SoapEndpointHost.MapSoapEndpoints` | Service Fabric listener registration becomes CoreWCF endpoint mapping. |
| `Program.Main` | `Program.Main` | Service host entry point becomes ASP.NET Core host. |
| `Program.InitializeInstrumentationSettings` | `Program.InitializeInstrumentationSettings` | Uses OpenTelemetry (not Service Fabric + TelemetryConfiguration.Active). |
| `Program.SetProfiles` | `Program.SetProfiles` | Uses DI + AutoMapper scanning. |
| `ServiceEventSource.Message(string, params object[])` | `ServiceEventSource.Message(string, params object[])` | ILogger + OTel event. |
| `ServiceEventSource.Message(string)` | `ServiceEventSource.Message(string)` | ILogger + OTel event. |
| `ServiceEventSource.ServiceMessage(StatelessServiceContext, string, params object[])` | `ServiceEventSource.ServiceMessage(/* host request context */, string, params object[])` | Service Fabric context fields replaced by host request/runtime context. |
| `ServiceEventSource.ServiceMessage(string, string, long, Guid, string, string, string, string)` | `ServiceEventSource.ServiceMessage(string, string, long, Guid, string, string, string, string)` | Keep same shape for structured host messages. |
| `ServiceEventSource.ServiceTypeRegistered` | `ServiceEventSource.ServiceTypeRegistered` | Host startup log event. |
| `ServiceEventSource.ServiceHostInitializationFailed` | `ServiceEventSource.ServiceHostInitializationFailed` | Host startup failure log event. |
| `ServiceEventSource.ServiceRequestStart` | `ServiceEventSource.ServiceRequestStart` | Request activity start marker. |
| `ServiceEventSource.ServiceRequestStop` | `ServiceEventSource.ServiceRequestStop` | Request activity stop marker. |
| `Startup.InitializeTenants` | `Startup.InitializeTenants` | Best-effort tenant startup probe retained. |
| `Startup.GetConfigurator` | `drop: legacy CQRS module configurator` | Target uses ASP.NET Core DI and does not use `CommandServiceBase` plumbing. |

## Dependencies

- No component dependencies (per brief).
- Uses target-state host libraries: ASP.NET Core, CoreWCF, OpenTelemetry, AutoMapper.

## Enterprise-Architecture standards applied

- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (OTel signals + propagation)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (`X-Correlation-ID` expectations)
- `strategy/guidance/security/Application/GUID-SEC-APP-API-SECURITY-2.7.01.md` section `Pattern 1: External API Access with Token-Based Authentication` (SOAP caller auth posture)
- `strategy/guidance/security/Application/GUID-SEC-APP-API-SECURITY-2.7.01.md` section `Pattern 2: Internal Service-to-Service API Access (Zero Trust)` (facade-to-domain-api posture)
