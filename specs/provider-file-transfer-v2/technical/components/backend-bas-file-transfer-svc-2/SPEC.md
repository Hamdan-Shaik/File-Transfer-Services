---
type: Behavior Specification
title: 'backend-bas-file-transfer-svc-2: CoreWCF host wiring (legacy)'
description: 'What the backend-bas-file-transfer-svc-2 component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 95b6158c6bf5fd56165db158a05e25aa84e2dd2e583a40e6c9d6a0d90e94d95c
---

# backend-bas-file-transfer-svc-2: CoreWCF host wiring (legacy)

This component contains the legacy ASP.NET Core + CoreWCF host wiring for the File Transfer SOAP surfaces. It wires up the DI container and hosts the SOAP endpoints for command, query, and CQRS command routing services.

## FileTransferModuleConfigurator
Legacy type: HPP.FileTransfer.BAS.FileTransferSvc.FileTransferModuleConfigurator

Configures runtime wiring for the legacy CoreWCF host.

### ConfigureContractAssemblies(IDataContractRegistry contractRegistry)
- Purpose: register DataContract assemblies for the host.
- Inputs: contractRegistry (unused).
- Logic: no-op.
- Data: none.
- Errors: none.
- Rules: none.

### ConfigureDependencies(ContainerBuilder container)
- Purpose: register File Transfer dependencies into the Autofac container.
- Inputs: container (Autofac DI container builder).
- Logic:
  - Register IDbContextBase -> FileTransferEntityDbContext, using a constructor that takes an IDbSession.
  - Register the following as transient (InstancePerDependency):
    - FtsSourceLocationAddCommandHandler
    - FtsSourceLocationProjectionWriter
    - FtpServerProjectionWriter
    - FtsInboundTargetProjectionWriter
    - FtsIntegrationFileProjectionWriter
- Data:
  - Reads (via the registered DbContext / writers/handlers): FTP_SERVER, FTS_INBOUND_TARGET, FTS_INTEGRATION_FILE, FTS_SUSPENSION.
- Errors:
  - Any DI misconfiguration errors surface at service startup/resolve time.
- Rules: none.

### ConfigureEvents(IDistributableEventRegistry registry)
- Purpose: register distributable events for the host.
- Inputs: registry (unused).
- Logic: no-op.
- Data: none.
- Errors: none.
- Rules: none.

## FileTransferSvcEventService
Legacy type: HPP.FileTransfer.BAS.FileTransferSvc.FileTransferSvcEventService

CQRS routing surface that accepts a polymorphic Command envelope and returns a polymorphic Event.

### TryRouteCommand(Command command)
- Purpose: route a command through the CQRS routing base.
- Inputs: command (CQRS command envelope).
- Logic:
  - Calls the base routing method TryRoute(command).
  - Returns the routed Event instance (or whatever the base routing returns).
- Data: determined by the routed handler (not implemented in this component's inventory).
- Errors:
  - Exceptions bubble to the CoreWCF host and are returned as SOAP faults per the host's fault behavior.
- Rules: none.

### GetConfigurator()
- Purpose: provide the module configurator used by the routing base.
- Inputs: none.
- Logic: returns a new FileTransferModuleConfigurator().
- Data: none.
- Errors: none.
- Rules: none.

## IFileTransferSvcQueryService
Legacy type: HPP.FileTransfer.BAS.FileTransferSvc.IFileTransferSvcQueryService

Legacy SOAP service contract interface (CoreWCF) for query operations. This file defines the operation surface; the operation implementations are not part of this component's inventory.

## IFileTransferSvcService
Legacy type: HPP.FileTransfer.BAS.FileTransferSvc.IFileTransferSvcService

Legacy SOAP service contract interface (CoreWCF) for command operations. This file defines the operation surface; the operation implementations are not part of this component's inventory.

## Program
Legacy type: HPP.FileTransfer.BAS.FileTransferSvc.Program

Legacy process entry point for the File Transfer CoreWCF host.

### Main(string[] args)
- Purpose: initialize tenant/BAS runtime wiring then run the ASP.NET Core host.
- Inputs: args (unused by this method; forwarded to host builder).
- Logic:
  - Call TenantBasHelper.Initialize(new FileTransferModuleConfigurator()).
  - Call CreateHostBuilder(args).Build().Run().
- Data: none.
- Errors: unhandled exceptions terminate the process.
- Rules: none.

### CreateHostBuilder(string[] args)
- Purpose: create the default generic host configured with Startup.
- Inputs: args.
- Logic:
  - Host.CreateDefaultBuilder(args)
  - ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>()).
- Data: none.
- Errors: host builder exceptions surface during startup.
- Rules: none.

## Startup
Legacy type: HPP.FileTransfer.BAS.FileTransferSvc.Startup

ASP.NET Core Startup class that wires CoreWCF services, endpoints, and WSDL metadata settings.

### ConfigureServices(IServiceCollection services)
- Purpose: register CoreWCF runtime services and WSDL metadata behavior.
- Inputs: services (ASP.NET Core service collection).
- Logic:
  - services.AddServiceModelServices().AddServiceModelMetadata().
  - Register IServiceBehavior -> UseRequestHeadersForMetadataAddressBehavior as singleton.
- Data: none.
- Errors: DI registration exceptions surface during startup.
- Rules: none.

### Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
- Purpose: configure CoreWCF endpoints and enable service metadata publishing.
- Inputs: app, env, logger.
- Logic:
  - Log the current environment name.
  - Compute a binding by calling CoreWCFBinding.ReturnBinding(ParseEnum<CoreWCFBinding.BindingTypes>("BasicHttpsBinding")).
  - Configure CoreWCF endpoints via app.UseServiceModel(builder => ...):
    - Add FileTransferSvcEventService endpoint at path /FileTransferSvcEventService for IEventDistribution.
    - Add FileTransferSvcQueryService endpoint at path /FileTransferSvcQueryService for IFileTransferSvcQueryService.
    - Add FileTransferSvcService endpoint at path /FileTransferSvcService for IFileTransferSvcService.
  - Enable metadata:
    - ServiceMetadataBehavior.HttpsGetEnabled = true
    - ServiceMetadataBehavior.HttpGetEnabled = true
- Data: none.
- Errors: startup/endpoint configuration errors surface as host startup failures.
- Rules: none.

### ParseEnum<T>(string value)
- Purpose: parse a string into an enum value (case-insensitive).
- Inputs: value.
- Logic: return (T)Enum.Parse(typeof(T), value, true).
- Data: none.
- Errors: throws if value is not a valid enum name.
- Rules: none.

## Rules implemented

None.

## Not carried over

- FileTransferModuleConfigurator.ConfigureContractAssemblies: legacy no-op; contract registry wiring is not used in the target host.
- FileTransferModuleConfigurator.ConfigureEvents: legacy no-op; legacy distributable event registry wiring is not used in the target host.
- FileTransferSvcEventService.TryRouteCommand: legacy CQRS routing base is replaced by explicit SOAP-to-domain-API delegation in the target facade.
- FileTransferSvcEventService.GetConfigurator: module configurator discovery is replaced by explicit startup registrations in the target.
- Program.Main: target host entry point is owned by the ackend-file-transfer-svc-main component in services/fxfer-soap-facade/src/Host.
- Program.CreateHostBuilder: merged into the target host entry point.
- Startup.ConfigureServices: merged into the target host DI composition root.
- Startup.Configure: merged into the target host CoreWCF endpoint configuration.
- Startup.ParseEnum: binding selection is handled by the target host binding factory.
