---
type: Test Plan
title: 'Tests: backend-bas-file-transfer-svc-2'
description: The tests that prove the backend-bas-file-transfer-svc-2 component, one or more per business rule.
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
  body_sha256: 4ba033b09d71ad9262b8a44eabd22a5295ac5b2b6d35da3cf6891b56672d29c6
---

# Tests: backend-bas-file-transfer-svc-2

Business rules owned: none.

## Unit tests

- FileTransferModuleConfigurator_configure_dependencies_registers_expected_types
  - Given an Autofac ContainerBuilder
  - When ConfigureDependencies(container) is called
  - Then the container can resolve:
    - IDbContextBase implemented by FileTransferEntityDbContext using an IDbSession constructor
    - FtsSourceLocationAddCommandHandler
    - FtsSourceLocationProjectionWriter
    - FtpServerProjectionWriter
    - FtsInboundTargetProjectionWriter
    - FtsIntegrationFileProjectionWriter

- FileTransferSvcEventService_try_route_command_delegates_to_base_router
  - Given a test double derived from FileTransferSvcEventService that exposes the routing result
  - When TryRouteCommand(command) is called
  - Then it returns the same Event instance produced by the base routing path.

## Integration tests (host-level)

These behaviors are owned by the target host composition root in services/fxfer-soap-facade/src/Host, but they verify the contract/wiring behavior described by this component:

- CoreWcf_endpoints_are_exposed_under_versioned_paths
  - When the host starts
  - Then the CoreWCF endpoints exist for:
    - FileTransferSvcService
    - FileTransferSvcQueryService
    - FileTransferSvcEventService
  - And WSDL/metadata publishing is enabled.

## Contract tests

- openapi_yaml_has_no_http_surface
  - Assert 	echnical/components/backend-bas-file-transfer-svc-2/openapi.yaml has:
    - x-no-http-surface: true
    - paths: {}

- contract_md_declares_no_component_dependencies
  - Assert 	echnical/components/backend-bas-file-transfer-svc-2/CONTRACT.md has Uses: None.

- soap_service_contract_namespaces_are_preserved
  - Assert IFileTransferSvcService uses ServiceContract Namespace HP.HSP.FileTransferSvc.
  - Assert IFileTransferSvcQueryService uses ServiceContract Namespace HP.HSP.FileTransferSvcQueryService.

