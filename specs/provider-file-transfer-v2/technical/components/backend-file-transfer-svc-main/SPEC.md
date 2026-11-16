---
type: Behavior Specification
title: 'backend-file-transfer-svc-main: Service Fabric host bootstrap'
description: 'What the backend-file-transfer-svc-main component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 3f2ba675bf7aa0390bbc45fcc5c9e353770e202a9968f9ee023acb818dd9cc48
---

# backend-file-transfer-svc-main: Service Fabric host bootstrap

Legacy Service Fabric host for FXFER FileTransferSvc WCF services. It performs host initialization (AutoMapper profiles, telemetry key, tenant startup checks) and registers the WCF listeners that expose the FileTransferSvc command/query/routing services.

## Main

### CreateServiceInstanceListeners()
- Purpose: Register WCF listeners for this service instance.
- Inputs: `this.Context` (Service Fabric `StatelessServiceContext`).
- Logic:
  - `FabricTelemetryInitializerExtension.SetServiceCallContext(this.Context)`.
  - Create one `WcfServiceListener`.
  - Return three `ServiceInstanceListener`s:
    - Name `FileTransferSvcEventServiceListener`: hosts `FileTransferSvcEventService` as `IEventDistribution` on `FileTransferSvcEventServiceEndpoint`.
    - Name `FileTransferSvcQueryServiceListener`: hosts `FileTransferSvcQueryService` as `IFileTransferSvcQueryService` on `FileTransferSvcQueryServiceEndpoint`.
    - Name `FileTransferSvcServiceListener`: hosts `FileTransferSvcService` as `IFileTransferSvcService` on `FileTransferSvcServiceEndpoint`.
- Data: None.
- Errors: Listener creation failures propagate.
- Rules: None.

## Program

### Main()
- Purpose: Entry point of the service host process.
- Inputs: App settings + tenant configuration.
- Logic:
  - `SetProfiles()`.
  - `InitializeInstrumentationSettings()`.
  - `ApplicationConfigurationManager.InitializeTenantsAppConfiguration()`.
  - If `ApplicationConfigurationManager.Tenants?.Keys.Any() == true`: `new Startup().InitializeTenants(tenants)`.
  - Register service type `HPP.FileTransfer.BAS.FileTransferSvc.MainType` mapped to `new Main(context)`.
  - `ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Main).Name)`.
  - Block forever (`Thread.Sleep(Timeout.Infinite)`).
- Data: Reads `ConfigurationManager.AppSettings[InstrumentationKey]` and `ApplicationConfigurationManager.Tenants`.
- Errors: Catch-all logs `ServiceHostInitializationFailed(e.ToString())` then rethrows.
- Rules: None.

### InitializeInstrumentationSettings()
- Purpose: Configure Application Insights instrumentation.
- Inputs: `ConfigurationManager.AppSettings[InstrumentationKey]`.
- Logic: If non-empty, set `TelemetryConfiguration.Active.InstrumentationKey`.
- Data: Reads app settings.
- Errors: None explicitly handled.
- Rules: None.

### SetProfiles()
- Purpose: Register AutoMapper profiles.
- Inputs: None.
- Logic: `Mapper.Initialize` and `DetectAndRegisterProfiles` for the assemblies of `InboundTargetProfile` and `IntegrationFileProfile`.
- Data: None.
- Errors: Exceptions propagate to `Main()`.
- Rules: None.

## ServiceEventSource

### Message(string message, params object[] args)
- Purpose: Emit a formatted informational message.
- Inputs: `message` format string, `args`.
- Logic: If enabled, format via `string.Format` then call `Message(string)`.
- Data: None.
- Errors: `string.Format` exceptions propagate (only if enabled).
- Rules: None.

### Message(string message)
- Purpose: Emit informational event id 1.
- Inputs: `message`.
- Logic: If enabled, `WriteEvent(1, message)`.
- Data: None.
- Errors: None explicitly handled.
- Rules: None.

### ServiceMessage(StatelessServiceContext serviceContext, string message, params object[] args)
- Purpose: Emit formatted message with Service Fabric context fields.
- Inputs: `serviceContext`, `message`, `args`.
- Logic: If enabled, format then call `ServiceMessage(string serviceName, ..., string message)` populated from `serviceContext`.
- Data: None.
- Errors: `string.Format` exceptions propagate (only if enabled).
- Rules: None.

### ServiceTypeRegistered(int hostProcessId, string serviceType)
- Purpose: Emit event id 3 (service type registered).
- Inputs: `hostProcessId`, `serviceType`.
- Logic: `WriteEvent(3, hostProcessId, serviceType)`.
- Data: None.
- Errors: None explicitly handled.
- Rules: None.

### ServiceHostInitializationFailed(string exception)
- Purpose: Emit event id 4 (host init failed).
- Inputs: `exception`.
- Logic: `WriteEvent(4, exception)`.
- Data: None.
- Errors: None explicitly handled.
- Rules: None.

### ServiceRequestStart(string requestTypeName)
- Purpose: Emit event id 5 (request start).
- Inputs: `requestTypeName`.
- Logic: `WriteEvent(5, requestTypeName)`.
- Data: None.
- Errors: None explicitly handled.
- Rules: None.

### ServiceRequestStop(string requestTypeName, string exception)
- Purpose: Emit event id 6 (request stop).
- Inputs: `requestTypeName`, optional `exception`.
- Logic: `WriteEvent(6, requestTypeName, exception)`.
- Data: None.
- Errors: None explicitly handled.
- Rules: None.

### ServiceMessage(string serviceName, string serviceTypeName, long replicaOrInstanceId, System.Guid partitionId, string applicationName, string applicationTypeName, string nodeName, string message)
- Purpose: Emit event id 2 (structured service message).
- Inputs: Context fields + `message`.
- Logic: `WriteEvent(2, serviceName, serviceTypeName, replicaOrInstanceId, partitionId, applicationName, applicationTypeName, nodeName, message)`.
- Data: None.
- Errors: None explicitly handled.
- Rules: None.

## ServiceEventSource.Keywords

- Purpose: Define ETW keyword flags used by `ServiceEventSource`.

## Startup

### InitializeTenants(System.Collections.Generic.List<string> tenants)
- Purpose: Best-effort per-tenant startup health check.
- Inputs: `tenants`.
- Logic: For each tenant call `this.CheckServiceHealth(tenant)`; on any exception, log warning `Service startup partially succeeded` and swallow.
- Data: None.
- Errors: Swallows exceptions from `CheckServiceHealth`.
- Rules: None.

### GetConfigurator()
- Purpose: Provide the CQRS module configurator used by `CommandServiceBase`.
- Inputs: None.
- Logic: Return `new FileTransferModuleConfigurator()`.
- Data: None.
- Errors: None explicitly handled.
- Rules: None.

## Rules implemented

| Rule | In short | Where | Flow |
|---|---|---|---|

None.

## Not carried over

None.
