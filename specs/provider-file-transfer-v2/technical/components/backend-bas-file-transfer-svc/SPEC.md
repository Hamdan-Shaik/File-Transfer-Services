---
type: Behavior Specification
title: 'backend-bas-file-transfer-svc: legacy WCF host wiring'
description: 'What the backend-bas-file-transfer-svc component does today: every class and method, and the business rules it implements.'
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
  body_sha256: c4545a6290bb4f3d3bc0e79b0965573a6520eb6eac15be0a55656126dac0e903
---

# backend-bas-file-transfer-svc: legacy WCF host wiring

This component is the legacy IIS/WCF host wiring for the File Transfer SOAP surfaces. It provides (a) container registrations for the File Transfer CQRS handlers and writers, (b) global exception-to-SOAP-fault mapping, and (c) AutoMapper profile discovery used by the legacy service implementations.

## FileTransferModuleConfigurator
Legacy type: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferModuleConfigurator

Configures runtime wiring for the legacy service host.

### ConfigureContractAssemblies(IDataContractRegistry contractRegistry)
- Purpose: register DataContract assemblies for the host.
- Inputs: contractRegistry (unused).
- Logic: no-op.
- Data: none.
- Errors: none.
- Rules: none.

### ConfigureDependencies(IUnityContainer container)
- Purpose: register File Transfer dependencies into the legacy Unity container.
- Inputs: container (Unity DI container).
- Logic:
  - Register IDbContextBase → FileTransferEntityDbContext.
  - Register the following as transient:
    - FtsSourceLocationAddCommandHandler
    - FtsSourceLocationProjectionWriter
    - FtpServerProjectionWriter
    - FtsInboundTargetProjectionWriter
    - FtsIntegrationFileProjectionWriter
- Data:
  - Reads (via the registered DbContext / writers/handlers): FTP_SERVER, FTS_INBOUND_TARGET, FTS_INTEGRATION_FILE, FTS_SUSPENSION.
- Errors:
  - Any DI container misconfiguration errors surface at service startup/resolve time.
- Rules: none.

### ConfigureEvents(IDistributableEventRegistry registry)
- Purpose: register distributable events for the host.
- Inputs: egistry (unused).
- Logic: no-op.
- Data: none.
- Errors: none.
- Rules: none.

## FileTransferSvcEventService
Legacy type: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcEventService

Legacy CQRS routing surface that accepts a polymorphic Command envelope and returns a polymorphic Event.

### TryRouteCommand(Command command)
- Purpose: route a command through the legacy CQRS routing base.
- Inputs: command (CQRS command envelope).
- Logic:
  - Calls the base routing method TryRoute(command).
  - Returns the routed Event instance (or whatever the base routing returns).
- Data: determined by the routed handler (not implemented in this component).
- Errors:
  - Exceptions are handled by the host's global error handler (Global.Application_Error) and returned as SOAP faults.
- Rules: none.

### GetConfigurator()
- Purpose: provide the module configurator used by the base host.
- Inputs: none.
- Logic: returns a new FileTransferModuleConfigurator().
- Data: none.
- Errors: none.
- Rules: none.

## Global
Legacy type: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Global

Legacy ASP.NET HttpApplication that configures AutoMapper and converts unhandled exceptions into WCF SOAP faults.

### GetLastException()
- Purpose: retrieve the last server error.
- Inputs: none.
- Logic: returns Server.GetLastError().
- Data: none.
- Errors: none.
- Rules: none.

### Application_Error(object sender, EventArgs e)
- Purpose: convert unhandled exceptions into SOAP faults in a consistent error envelope.
- Inputs: none (reads Server.GetLastError()).
- Logic:
  - Read ex = Server.GetLastError().
  - If ex is TargetInvocationException, set ctualEx = ex.InnerException; else ctualEx = ex.
  - Create svcException = new ServiceException(actualEx).
  - If ctualEx implements IFaultGenerator:
    - Call GenerateFault().
    - If non-null: log as fatal with FileTransferExceptionConstants.FaultExceptions.CustomFault + <fault message>, then 	hrow the custom FaultException.
  - Otherwise: log as fatal with FileTransferExceptionConstants.FaultExceptions.ApplicationError and throw FaultException<ServiceException>(svcException, new FaultReason(svcException.Reason)).
- Data: none.
- Errors:
  - Produces SOAP faults:
    - a custom FaultException when IFaultGenerator.GenerateFault() returns non-null.
    - otherwise a FaultException<ServiceException>.
- Rules: none.

### Application_Start(object sender, EventArgs e)
- Purpose: initialize host-wide configuration at application start.
- Inputs: none.
- Logic:
  - Calls SetProfiles().
  - Calls InitializeInstrumentationSettings().
- Data: none.
- Errors: any exception bubbles as an application startup failure.
- Rules: none.

### InitializeInstrumentationSettings()
- Purpose: set Application Insights instrumentation key from configuration.
- Inputs: reads WebConfigurationManager.AppSettings[InstrumentationKey].
- Logic:
  - If the key is non-empty, set TelemetryConfiguration.Active.InstrumentationKey to that value.
- Data: reads InstrumentationKey app setting.
- Errors: none.
- Rules: none.

### SetProfiles()
- Purpose: configure AutoMapper profiles for File Transfer service model mappings.
- Inputs: none.
- Logic:
  - Calls Mapper.Initialize(cfg => ...).
  - Uses DetectAndRegisterProfiles(...) for:
    - InboundTargetProfile assembly
    - IntegrationFileProfile assembly
- Data: none.
- Errors: mapping configuration exceptions surface during initialization.
- Rules: none.

## IFileTransferSvcQueryService
Legacy type: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.IFileTransferSvcQueryService

Legacy WCF service contract interface (SOAP). This file defines the legacy operation surface, but the operation implementations are not part of this component's inventory.

## IFileTransferSvcService
Legacy type: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.IFileTransferSvcService

Legacy WCF service contract interface (SOAP). This file defines the legacy operation surface, but the operation implementations are not part of this component's inventory.

## Rules implemented

None.

## Not carried over

- FileTransferModuleConfigurator.ConfigureContractAssemblies: no-op in legacy; contract registry wiring is not used in the target host.
- FileTransferModuleConfigurator.ConfigureDependencies: Unity container wiring is replaced by the target host DI composition root.
- FileTransferModuleConfigurator.ConfigureEvents: no-op in legacy; legacy distributable event registry is not used in the target host.
- FileTransferSvcEventService.TryRouteCommand: hosted as a CoreWCF SOAP service operation in the target (no IIS .svc host).
- FileTransferSvcEventService.GetConfigurator: module configurator discovery is replaced by explicit target startup registrations.
- Global.GetLastException: System.Web host is not used in the target.
- Global.Application_Error: exception-to-fault mapping is implemented via CoreWCF fault handling in the target host.
- Global.Application_Start: initialization occurs in the ASP.NET Core host startup.
- Global.InitializeInstrumentationSettings: Application Insights instrumentation key wiring is replaced by target OpenTelemetry configuration.
- Global.SetProfiles: AutoMapper profiles are registered through DI-based AutoMapper configuration in the target host.

