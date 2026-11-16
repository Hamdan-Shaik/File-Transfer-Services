---
type: Target Placement
title: 'Target: backend-bas-file-transfer-svc'
description: Where the backend-bas-file-transfer-svc component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-bas-file-transfer-svc
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:09:36Z
sources:
- id: inventory
  resource: /technical/components/backend-bas-file-transfer-svc/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-bas-file-transfer-svc
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 5
  methods: 10
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-soap-facade
  folder: src/LegacyWcf
  body_sha256: ccab341f97b8f8aa7e44124eb65d717d8166096f091c15f5f9ecc42c75d93a3c
---

# Target: backend-bas-file-transfer-svc

## Where

- Deployable: xfer-soap-facade (services/fxfer-soap-facade)
- Folder: services/fxfer-soap-facade/src/LegacyWcf

## What to build

This component does not produce runtime hosting code in the target. The legacy implementation relies on IIS + System.Web (Global.asax) and Unity container wiring, which are not carried into the .NET 10 / ASP.NET Core / CoreWCF host.

Create only the legacy parity reference artifacts under services/fxfer-soap-facade/src/LegacyWcf, used to validate migration parity (for example, via contract comparison and test fixtures). No new contract development happens here; the CoreWCF contract source of truth is in services/fxfer-soap-facade/src/CoreWcf.

### Contracts (parity reference)

Create the legacy WCF service contract interfaces as parity reference types (SOAP):

- LegacyWcf/Contracts/IFileTransferSvcService.cs
  - [ServiceContract(Namespace = HP.HSP.FileTransferSvc)]
  - Inherits IServiceAvailability (legacy availability operations remain part of the SOAP surface).
  - Declares the legacy operations (commands):
    - FileTransferAdd(FtsSourceLocationAddCommand) : FtsSourceLocationAdded
    - UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand) : FtsSourceLocationUpdated
    - InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand) : FtsSourceLocationInvalidated
    - AddFtpServer(AddFtpServerCommand) : FtpServerAdded
    - UpdateFtpServer(UpdateFtpServerCommand) : FtpServerUpdated
    - InvalidateFtpServer(InvalidateFtpServerCommand) : FtpServerInvalidated
    - UpdateIntegrationFile(UpdateIntegrationFileCommand) : IntegrationFileUpdated
  - Faults: ServiceException, BusinessValidationException.

- LegacyWcf/Contracts/IFileTransferSvcQueryService.cs
  - [ServiceContract(Namespace = HP.HSP.FileTransferSvcQueryService)]
  - Inherits IServiceAvailability.
  - Declares the legacy operations (queries):
    - SearchSourceLocations(SourceLocationQuery) : SourceLocationQuery
    - GetFtpServers(FtpServerQuery) : FtpServerQuery
    - GetSourceLocations(GetSourceLocationQuery) : GetSourceLocationQuery
    - SearchFileTransferMovement(FileTransferTrackingQuery) : FileTransferTrackingQuery
    - SearchInboundTarget(InboundTargetQuery) : InboundTargetQuery
    - GetIntegrationFile(GetIntegrationFileQuery) : GetIntegrationFileQuery
  - Faults: ServiceException.

The canonical definitions for these contracts (and their DataContracts) are owned by the CoreWCF-layer components in the target; the LegacyWcf copies exist only to preserve a frozen view of the legacy namespaces/attributes.

## Mapping (legacy → target)

| Legacy | Target | Note |
| --- | --- | --- |
| FileTransferModuleConfigurator.ConfigureContractAssemblies(IDataContractRegistry) | drop: ASP.NET Core/CoreWCF does not use contract registry wiring | Legacy method is a no-op. |
| FileTransferModuleConfigurator.ConfigureDependencies(IUnityContainer) | merged into fxfer-soap-facade DI composition root | Unity is not used in the target host. |
| FileTransferModuleConfigurator.ConfigureEvents(IDistributableEventRegistry) | drop: ASP.NET Core/CoreWCF host does not use legacy distributable event registry | Legacy method is a no-op. |
| FileTransferSvcEventService.TryRouteCommand(Command) | merged into CoreWCF SOAP service implementation | Target hosts SOAP operations via CoreWCF (no IIS .svc). |
| FileTransferSvcEventService.GetConfigurator() | drop: DI composition replaces module configurator discovery | Target uses explicit registrations at startup. |
| Global.GetLastException() | drop: System.Web host is not used | Target host uses ASP.NET Core exception pipeline. |
| Global.Application_Error(object, EventArgs) | merged into CoreWCF fault handling + logging | Target must preserve custom-fault passthrough and default ServiceException SOAP fault behavior. |
| Global.Application_Start(object, EventArgs) | merged into target host startup | Target startup registers AutoMapper profiles and observability. |
| Global.InitializeInstrumentationSettings() | drop: Application Insights instrumentation key configuration is replaced by target OpenTelemetry configuration | Legacy behavior is host-level observability. |
| Global.SetProfiles() | merged into target host startup AutoMapper configuration | Target uses DI-based AutoMapper configuration. |

## Enterprise-Architecture standards applied

- patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md section When NOT to Use (SOAP/XML services use WSDL; OpenAPI is not the contract format).
- standards/application/integration/dotnet/STD-APP-INT-003-dotnet-protocol-adapters.md section REQ-INT-003-004: SOAP-to-Event Adapter (SOAP fault handling and protocol-boundary requirements applied to the target SOAP facade fault posture).

