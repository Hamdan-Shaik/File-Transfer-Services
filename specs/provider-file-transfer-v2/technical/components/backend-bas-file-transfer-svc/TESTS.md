---
type: Test Plan
title: 'Tests: backend-bas-file-transfer-svc'
description: The tests that prove the backend-bas-file-transfer-svc component, one or more per business rule.
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
  body_sha256: e3a76105b1c4a0cc4d52cea5d72889766a985370dd100f75199d09afa42750e1
---

# Tests: backend-bas-file-transfer-svc

This component owns no business rules. Tests focus on contract artifacts and parity-critical hosting semantics that must be preserved by the target SOAP facade host.

## Contract tests

- openapi_no_http_surface_is_declared
  - Given 	echnical/components/backend-bas-file-transfer-svc/openapi.yaml
  - Then x-no-http-surface is 	rue
  - And paths is empty ({})

## SOAP fault mapping tests (parity semantics)

- soap_fault_passthrough_when_exception_implements_ifaultgenerator
  - Given a service operation throws an exception implementing IFaultGenerator whose GenerateFault() returns a non-null FaultException
  - When the exception reaches the host fault pipeline
  - Then the exact returned FaultException is surfaced to the caller (no ServiceException wrapping)

- soap_fault_wraps_unhandled_exception_as_serviceexception
  - Given a service operation throws a non-IFaultGenerator exception
  - When the exception reaches the host fault pipeline
  - Then the caller receives a FaultException<ServiceException>
  - And FaultReason equals ServiceException.Reason

## Startup wiring tests (parity semantics)

- utomapper_profiles_are_registered
  - Given the SOAP facade host starts
  - Then AutoMapper registers profiles from the assemblies containing InboundTargetProfile and IntegrationFileProfile

## Enterprise-Architecture standards applied

- standards/application/integration/dotnet/STD-APP-INT-003-dotnet-protocol-adapters.md section REQ-INT-003-004: SOAP-to-Event Adapter (fault handling expectations at protocol boundaries).

