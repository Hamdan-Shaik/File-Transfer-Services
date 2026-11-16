---
type: Target Placement
title: 'Target: backend-bas-file-transfer-svc-2'
description: Where the backend-bas-file-transfer-svc-2 component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-bas-file-transfer-svc-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:18:17Z
sources:
- id: inventory
  resource: /technical/components/backend-bas-file-transfer-svc-2/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-bas-file-transfer-svc-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 10
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-soap-facade
  folder: src/CoreWcf
  body_sha256: 546b81ec115ceae3eb8974eba66b6f4054a419a374d806a1c07295fffc276020
---

# Target: backend-bas-file-transfer-svc-2

## Where

- Deployable: fxfer-soap-facade (services/fxfer-soap-facade)
- Folder: services/fxfer-soap-facade/src/CoreWcf

## What to build

This component does not own the target service runtime entry point. The legacy Program/Startup host wiring is merged into the target host composition root in services/fxfer-soap-facade/src/Host.

Create only the CoreWCF-facing contract and wiring primitives that the host uses:

- CoreWcf/FileTransferModuleConfigurator.cs
  - Preserve the legacy Autofac registration intent (DbContext + command handler + projection writers) as target DI registrations.
  - ConfigureContractAssemblies(...) remains a no-op (not used by the target host).
  - ConfigureEvents(...) remains a no-op (not used by the target host).

- CoreWcf/Contracts/Services/IFileTransferSvcService.cs
  - Canonical CoreWCF service contract interface for command operations.
  - Preserve the legacy ServiceContract namespace values.
  - Preserve fault contracts (ServiceException, BusinessValidationException where present).

- CoreWcf/Contracts/Services/IFileTransferSvcQueryService.cs
  - Canonical CoreWCF service contract interface for query operations.
  - Preserve the legacy ServiceContract namespace values.
  - Preserve fault contracts (ServiceException).

Runtime hosting and endpoint wiring is owned by ackend-file-transfer-svc-main in services/fxfer-soap-facade/src/Host:

- services/fxfer-soap-facade/src/Host/Program.cs composes the app and calls CoreWCF.
- SOAP endpoint routing uses the deployable structure standard versioned paths (see 	echnical/target/deployables/fxfer-soap-facade/STRUCTURE.md).

## Mapping (legacy -> target)

| Legacy | Target | Note |
| --- | --- | --- |
| FileTransferModuleConfigurator.ConfigureContractAssemblies(IDataContractRegistry) | drop: contract registry wiring not used in target host | Legacy method is a no-op. |
| FileTransferModuleConfigurator.ConfigureDependencies(ContainerBuilder) | merged into fxfer-soap-facade host DI composition root | Target uses explicit DI registrations; Autofac use is optional. |
| FileTransferModuleConfigurator.ConfigureEvents(IDistributableEventRegistry) | drop: event registry wiring not used in target host | Legacy method is a no-op. |
| FileTransferSvcEventService.TryRouteCommand(Command) | merged into Host.Soap.Services.FileTransferSvcEventService.TryRouteCommand | Target facade routes to fxfer-domain-api instead of legacy CQRS routing. |
| FileTransferSvcEventService.GetConfigurator() | drop: explicit startup registrations replace module configurator discovery | Target uses explicit registrations at startup. |
| Program.Main(string[]) | merged into services/fxfer-soap-facade/src/Host/Program.cs | Target host owns runtime entry point. |
| Program.CreateHostBuilder(string[]) | merged into services/fxfer-soap-facade/src/Host/Program.cs | Target uses minimal hosting model. |
| Startup.ConfigureServices(IServiceCollection) | merged into services/fxfer-soap-facade/src/Host/Program.cs | Target host registers CoreWCF runtime services and metadata. |
| Startup.Configure(IApplicationBuilder, IWebHostEnvironment, ILogger<Startup>) | merged into services/fxfer-soap-facade/src/Host/Program.cs | Target host configures CoreWCF endpoints and metadata publishing. |
| Startup.ParseEnum<T>(string) | drop: binding selection handled by SoapBindingFactory | Target uses an explicit binding factory. |

## Enterprise-Architecture standards applied

- patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md section TL;DR / When NOT to Use (SOAP/XML services use WSDL; OpenAPI is not the contract format).
- standards/application/integration/dotnet/STD-APP-INT-003-dotnet-protocol-adapters.md section REQ-INT-003-004: SOAP-to-Event Adapter (SOAP fault handling + protocol-boundary behavior requirements applied to the SOAP facade boundary).

