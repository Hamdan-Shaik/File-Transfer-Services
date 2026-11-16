---
type: Test Plan
title: 'Tests: backend-file-transfer-svc-main'
description: The tests that prove the backend-file-transfer-svc-main component, one or more per business rule.
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
  body_sha256: e0fb12fc2db06af0aa465513b64c7ad07631a4c75cd19bce8360ca8f8327c5da
---

# Tests: `backend-file-transfer-svc-main`

Owned business rules: none.

## Unit tests

- `Program_initialize_instrumentation_settings_does_nothing_when_key_missing`
  - Given config without `InstrumentationKey`, when `Program.InitializeInstrumentationSettings`, then no exporter/backend is configured.

- `Program_initialize_instrumentation_settings_configures_backend_when_key_present`
  - Given config with `InstrumentationKey = ...`, when `Program.InitializeInstrumentationSettings`, then the telemetry backend/exporter is configured.

- `Program_set_profiles_registers_expected_assemblies`
  - Given an empty DI container, when `Program.SetProfiles`, then AutoMapper profile scanning includes the assemblies containing `InboundTargetProfile` and `IntegrationFileProfile`.

- `SoapEndpointHost_maps_all_three_corewcf_endpoints`
  - Given a host app configured for CoreWCF, when `SoapEndpointHost.MapSoapEndpoints`, then the `.svc` endpoints exist under `/soap/file-transfer/v1/*`.

- `Startup_initialize_tenants_continues_after_probe_failure`
  - Given multiple tenant ids and a probe that throws for one tenant, when `Startup.InitializeTenants`, then a warning is logged and startup does not fail.

- `ServiceEventSource_message_formats_args_when_enabled`
  - Given a format string and args, when `ServiceEventSource.Message(string, object[])`, then the emitted message matches `string.Format` output.

## Contract tests

- `openapi_has_no_http_surface`
  - Validate `technical/components/backend-file-transfer-svc-main/openapi.yaml` has `x-no-http-surface: true` and `paths: {}`.

- `contract_doc_covers_provides_and_uses`
  - Validate `technical/components/backend-file-transfer-svc-main/CONTRACT.md` contains sections `Provides`, `Uses`, `Dependencies`.
