---
type: Interface Contract
title: 'Contract: backend-bas-file-transfer-svc-2'
description: What the backend-bas-file-transfer-svc-2 component offers other components and what it uses from them.
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
  body_sha256: f89f5f101ba510fafc34696e37448b6a53f92848dc1737f544fcc94308f96223
---

# Contract: backend-bas-file-transfer-svc-2

This component defines the legacy File Transfer SOAP contract surface (CoreWCF ServiceContract interfaces) and the minimal module wiring entry points used by the host.

## Offers

### SOAP service contracts

- IFileTransferSvcService (ServiceContract Namespace: HP.HSP.FileTransferSvc)
  - Operations (commands):
    - FileTransferAdd(FtsSourceLocationAddCommand) : FtsSourceLocationAdded
    - UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand) : FtsSourceLocationUpdated
    - InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand) : FtsSourceLocationInvalidated
    - AddFtpServer(AddFtpServerCommand) : FtpServerAdded
    - UpdateFtpServer(UpdateFtpServerCommand) : FtpServerUpdated
    - InvalidateFtpServer(InvalidateFtpServerCommand) : FtpServerInvalidated
    - UpdateIntegrationFile(UpdateIntegrationFileCommand) : IntegrationFileUpdated
  - Faults:
    - ServiceException
    - BusinessValidationException (where declared per-operation)

- IFileTransferSvcQueryService (ServiceContract Namespace: HP.HSP.FileTransferSvcQueryService)
  - Operations (queries):
    - SearchSourceLocations(SourceLocationQuery) : SourceLocationQuery
    - GetFtpServers(FtpServerQuery) : FtpServerQuery
    - GetSourceLocations(GetSourceLocationQuery) : GetSourceLocationQuery
    - SearchFileTransferMovement(FileTransferTrackingQuery) : FileTransferTrackingQuery
    - SearchInboundTarget(InboundTargetQuery) : InboundTargetQuery
    - GetIntegrationFile(GetIntegrationFileQuery) : GetIntegrationFileQuery
  - Faults:
    - ServiceException

### Module wiring

- FileTransferModuleConfigurator
  - Entry point for registering file-transfer data access and handler dependencies into the host DI container.

## Uses

None (no dependencies on other spec-pack components).

## Data owned

None.

## Notes

- In the target architecture, this component is part of the SOAP compatibility facade (anti-corruption layer). The facade translates legacy SOAP operations into modern domain API calls; this component describes only the contract surface and host wiring primitives, not the domain behavior.

## Enterprise-Architecture standards applied

- strategy/guidance/security/zero-trust/GUID-SEC-ZTA-MS-WP-001-zero-trust-microservices.md section 4.6.1 The Strangler Fig Boundary as a Security Boundary (treat the SOAP compatibility facade as an explicit boundary/adapter between legacy callers and modern services).

