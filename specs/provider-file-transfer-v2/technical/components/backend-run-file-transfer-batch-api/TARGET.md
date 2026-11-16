---
type: Target Placement
title: 'Target: backend-run-file-transfer-batch-api'
description: Where the backend-run-file-transfer-batch-api component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-run-file-transfer-batch-api
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:02:28Z
sources:
- id: rules-run-file-transfer-batch
  resource: /technical/flows/run-file-transfer-batch/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-run-file-transfer-batch-api/INVENTORY.yaml
  title: Every legacy class and method (2 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-run-file-transfer-batch-api
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 2
  methods: 7
  owned_rules: 14
  flows:
  - run-file-transfer-batch
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Worker
  body_sha256: 6c9d5ef844144f19ec5e883615468102e28d0465fa22724190e5210ae0aa6e7e
---

# Target: backend-run-file-transfer-batch-api

Where:

- Deployable: `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`)
- Folder: `workers/fxfer-transfer-worker/src/Worker`

## What to create

Create the following command/runner types in `workers/fxfer-transfer-worker/src/Worker`.

### FileTransferBatchBasicRunner

Implements the legacy `main-batch-process` door behavior.

- `int Run(string[] args)`
  - Build/initialize the worker runtime container for CLI execution.
  - Resolve `IBatchExecutor<int>` keyed/named `FileTransferExecutor`.
  - Call `Execute(args)` and return its result.
  - On any exception: record the failure message under source `File Transfer Batch Process` and return `-1`.

### FileTransferBatchManagedRunner

Implements the legacy `main-batch-process-2` door behavior.

- `int Run(string[] args)`
  - Log `Starting FileTransfer Batch Process`.
  - Log `Validating batch arguments.`.
  - If `Validate(args)` is `false`, return `-1`.
  - Log `Retrieving app settings for tenant`.
  - If tenant settings are not already loaded, load them for the extracted `tenantId`.
  - Log `Validating app settings.`.
  - Call `ValidateAppConfig()`.
  - Log `Initialize Unit Container.` then initialize the runtime container.
  - Log `Resolve the batch executor.` then resolve `IBatchExecutor<int>` keyed/named `FileTransferExecutor`.
  - Log `Begin the process.` then call `Execute(args)` and return its result.
  - On any exception:
    - record a fatal log entry `Exception occurred: ` including the exception.
    - record the failure message under source `File Transfer Batch Process`.
    - return `-1`.
  - Always log `Ending FileTransfer Batch Process`.

- `bool Validate(string[] args)`
  - Return `ValidateInputArguments(args)`.

- `bool ValidateInputArguments(string[] args)`
  - Accept only the 1-value and 3-value forms.
  - 1-value form:
    - treat `args[0]` as `tenantId`.
    - require `args[0]` parse as GUID.
  - 3-value form:
    - do not validate `args[0]`.
    - require `args[1]` parse as `int` or GUID.
    - require `args[2]` parse as GUID and treat it as `tenantId`.
  - On any validation failure, call `PrintUsage()` and return `false`.

- `void ValidateAppConfig()`
  - For each mandatory key (see SPEC.md), call `CheckAndThrowConfigurationException(key)`.

- `void CheckAndThrowConfigurationException(string key)`
  - If the key is missing from the loaded tenant settings:
    - emit error `"{key} is not specified in app.config"`.
    - throw a configuration error exception with the same message.

- `void PrintUsage()`
  - Emit the usage text exactly as specified in SPEC.md.

### FileTransferBatchRerunCommand (T-007)

Provide a privileged CLI rerun mode that targets a specific tenant and tracked file identifier.

- `int Run(Guid tenantId, Guid trackedFileId)`
  - Validate that both identifiers are present and well-formed.
  - Invoke `IBatchExecutor<int>` keyed/named `FileTransferExecutor` using run details that target only the specified tracked file.
  - Return the batch runner result code.
  - On any exception: record the failure for support and return `-1`.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main.BatchProcess.Main(string[])` | `FileTransferBatchBasicRunner.Run` | Preserves pass-through argument behavior (no validation). |
| `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.Main(string[])` | `FileTransferBatchManagedRunner.Run` | Preserves managed argument validation + app-settings validation + start/end logging. |
| `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.Validate(string[])` | `FileTransferBatchManagedRunner.Validate` | Direct mapping. |
| `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.ValidateInputArguments(string[])` | `FileTransferBatchManagedRunner.ValidateInputArguments` | Direct mapping. |
| `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.ValidateAppConfig()` | `FileTransferBatchManagedRunner.ValidateAppConfig` | Direct mapping. |
| `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.CheckAndThrowConfigurationException(string)` | `FileTransferBatchManagedRunner.CheckAndThrowConfigurationException` | Direct mapping. |
| `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.PrintUsage()` | `FileTransferBatchManagedRunner.PrintUsage` | Direct mapping. |

## Dependencies

- None on other components.
- Uses the worker runtime DI container to resolve `IBatchExecutor<int>` keyed/named `FileTransferExecutor`.

## Enterprise-Architecture standards applied

- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (use OpenTelemetry SDK/API; export via OTLP; propagate W3C TraceContext; no PII/PHI in telemetry)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `3.6 Semantic Conventions` (OTel semantic conventions; custom attributes prefixed `gainwell.*` such as `gainwell.tenant.id`)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (generate/extract correlation id at entry point including scheduled job/CLI; propagate to downstream calls)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Correlation ID Format Standards` (UUID v4 correlation ids; header name `X-Correlation-ID` on outbound HTTP calls)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant isolation posture; no cross-tenant access)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `3.1 Tenant Isolation Strategies` (tenant-scoped execution context)