---
type: Behavior Specification
title: 'backend-run-file-transfer-batch-api: start file transfer batch execution'
description: 'What the backend-run-file-transfer-batch-api component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 4061abe1c49ccc0ccb22601cbebdd4919d87a369ccb0ffb54761f64be4dc1242
---

# backend-run-file-transfer-batch-api: start file transfer batch execution

This component is the batch/CLI entry surface for the flow `run-file-transfer-batch`.
Operations (or a scheduler) invoke it with run details; it validates the run details (managed door), validates required configuration keys, then resolves the batch runner named `FileTransferExecutor` and returns its integer result.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main.BatchProcess

CLI entrypoint that hands execution to the `FileTransferExecutor` batch runner with no argument validation.

### Main(string[] args)
- Purpose: Initialize the runtime container, resolve the named batch runner `FileTransferExecutor`, and run it with the provided `args`.
- Inputs:
  - `args`: any number of string values; passed through without validation.
- Logic:
  1. Call `BASUnityContainer.Initialize()`.
  2. Resolve `IBatchExecutor<int>` from the container using the name `IocConstants.FileTransferExecutor` (value `FileTransferExecutor`).
  3. Call `Execute(args)` and return its integer result.
  4. On any exception:
     - write an event entry with source `File Transfer Batch Process` and message `ex.Message` as an error.
     - return `-1`.
- Data: none.
- Errors:
  - Any exception during initialization, resolution, or execution causes `-1` to be returned and the exception message to be recorded under `File Transfer Batch Process`.
- Rules: F04-001, F04-002.

## HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess

Managed CLI entrypoint that validates run details and required tenant-scoped settings before handing execution to the `FileTransferExecutor` batch runner.

### Main(string[] args)
- Purpose: Validate the run details and required settings, then initialize the runtime container, resolve the named batch runner `FileTransferExecutor`, and run it.
- Inputs:
  - `args`: either 1 value or 3 values (enforced by validation).
- Logic:
  1. Create a new `BatchProcess` instance.
  2. Log informational messages in this order:
     - `Starting FileTransfer Batch Process`
     - `Validating batch arguments.`
  3. Call `Validate(args)`.
     - If it returns `false`, return `-1` immediately.
  4. Log `Retrieving app settings for tenant`.
  5. If `ApplicationConfigurationManager.AppSettings` is `null`, call `ApplicationConfigurationManager.LoadApplicationConfiguration(tenantId, new CacheManager())`.
  6. Log `Validating app settings.`.
  7. Call `ValidateAppConfig()`.
  8. Log `Initialize Unit Container.` then call `BASUnityContainer.Initialize()`.
  9. Log `Resolve the batch executor.` then resolve `IBatchExecutor<int>` by name `IocConstants.FileTransferExecutor` (value `FileTransferExecutor`).
  10. Log `Begin the process.` then return `Execute(args)`.
  11. On any exception:
      - log fatal `Exception occurred: ` with the exception.
      - write an event entry with source `File Transfer Batch Process` and message `ex.Message` as an error.
      - return `-1`.
  12. In a `finally` block, log `Ending FileTransfer Batch Process`.
- Data: none.
- Errors:
  - When validation fails, the method returns `-1` without calling the batch executor.
  - Any exception causes `-1` to be returned, a fatal log entry to be recorded, and the exception message to be recorded under `File Transfer Batch Process`.
- Rules: F04-003, F04-004, F04-011, F04-012, F04-013, F04-014.

### Validate(string[] args)
- Purpose: Validate that the input arguments are in an accepted pattern.
- Inputs:
  - `args`: the run details.
- Logic:
  1. Call `ValidateInputArguments(args)`.
  2. If it returns `false`, return `false`.
  3. Otherwise return `true`.
- Data: none.
- Errors: none (returns `false` for invalid input).
- Rules: F04-004.

### ValidateInputArguments(string[] args)
- Purpose: Accept only the 1-value or 3-value argument patterns and extract the `tenantId` used for tenant configuration loading.
- Inputs:
  - `args`:
    - 1-value form: `args[0]` is the tenant identifier.
    - 3-value form: `args[0]` is a label (not validated), `args[1]` is a chosen value, and `args[2]` is the tenant identifier.
- Logic:
  1. If `args.Length` is not `1` and not `3`, call `PrintUsage()` and return `false`.
  2. If `args.Length == 1`:
     - set `processingAllFiles = true`.
     - set `tenantId = args[0]`.
     - if `Guid.TryParse(args[0])` is `false`, mark the arguments as bad.
  3. If `args.Length == 3`:
     - set `tenantId = args[2]`.
     - if both `int.TryParse(args[1])` and `Guid.TryParse(args[1])` are `false`, mark the arguments as bad.
     - if `Guid.TryParse(args[2])` is `false`, mark the arguments as bad.
     - do not validate `args[0]`.
  4. If any bad argument was found:
     - call `PrintUsage()` and return `false`.
  5. Return `true`.
- Data: none.
- Errors: none (returns `false` for invalid input).
- Rules: F04-004, F04-005, F04-006, F04-007, F04-008, F04-009, F04-010.

### ValidateAppConfig()
- Purpose: Require each mandatory application setting key to be present.
- Inputs: none.
- Logic:
  - Call `CheckAndThrowConfigurationException(key)` for each key below (in this order):
    1. `ExternalFTECommonFolder`
    2. `WorkingDirectory`
    3. `MaxConcurrentDeliveryProcesses`
    4. `MaxFilesPerCycle`
    5. `MinsToWaitBeforeRetryFailedFile`
    6. `MinsToWaitBeforeRetrySuspendedConfig`
    7. `DisableNotifications`
    8. `EnvironmentName`
    9. `DeleteArchiveIfExists`
    10. `DuplicateFileExtension`
    11. `ActivateDuplicateRenameLogic`
- Data: reads `ApplicationConfigurationManager.AppSettings`.
- Errors:
  - If any key is missing, `CheckAndThrowConfigurationException` throws, which is caught by `Main` and causes it to return `-1`.
- Rules: F04-012.

### CheckAndThrowConfigurationException(string key)
- Purpose: Enforce presence of a configuration key and emit a support-visible message when missing.
- Inputs:
  - `key`: configuration key name.
- Logic:
  1. If `ApplicationConfigurationManager.AppSettings[key]` is `null`:
     - format message `"{0} is not specified in app.config"` with `key`.
     - log the formatted message as an error.
     - throw a configuration error exception with the same formatted message.
- Data: reads `ApplicationConfigurationManager.AppSettings`.
- Errors: throws when the key is missing.
- Rules: F04-012.

### PrintUsage()
- Purpose: Output usage guidance when the input arguments do not match an accepted pattern.
- Inputs: none.
- Logic:
  - Build and log the following usage text (including line breaks):
    - `Usage: FileTransferService <command> <command_value>`
    - `  command = sakTrack X (X is the sak_file_track to be processed)`
    - `            configId X (X is the config_id of the location to be processed`
    - `                     top X (X is the number of failed entries to be processed`
    - `  To process all files do not specify the <command> and <command_value> arguments`
- Data: none.
- Errors: none.
- Rules: F04-004.

## Rules implemented

| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F04-001 | Initialize container; resolve `FileTransferExecutor`; run it with provided details | `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main.BatchProcess.Main` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-002 | On unexpected failure, record message under `File Transfer Batch Process`; return `-1` | `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main.BatchProcess.Main` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-003 | Record start and always record end record | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.Main` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-004 | Accept only supported run detail patterns; otherwise show usage and return `-1` | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.ValidateInputArguments`, `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.PrintUsage`, `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.Main` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-005 | One-detail run treats the detail as tenant id and processes all files | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.ValidateInputArguments` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-006 | One-detail run requires tenant id to be a valid GUID | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.ValidateInputArguments` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-007 | Three-detail run treats the third detail as tenant id | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.ValidateInputArguments` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-008 | Three-detail run requires the second detail be an int or GUID | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.ValidateInputArguments` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-009 | Three-detail run requires the third detail be a valid GUID | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.ValidateInputArguments` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-010 | Three-detail run does not validate the first detail | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.ValidateInputArguments`, `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.PrintUsage` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-011 | Load tenant settings only when not already loaded | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.Main` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-012 | Require each mandatory setting key; missing keys stop the run with `-1` | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.ValidateAppConfig`, `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.CheckAndThrowConfigurationException`, `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.Main` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-013 | After validation and settings checks, initialize container; resolve `FileTransferExecutor`; run it | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.Main` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |
| F04-014 | On unexpected failure, record fatal; record message under `File Transfer Batch Process`; return `-1` | `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main.BatchProcess.Main` | [run-file-transfer-batch](../../../technical/flows/run-file-transfer-batch/RULES.md) |

## Target-state requirements

### T-007
- Purpose: Provide privileged rerun execution via a CLI mode targeting a specific tenant and tracked file identifier (without introducing a required UI rerun button).
- Inputs:
  - `tenantId`: tenant identifier (GUID in standard 36-character format).
  - `trackedFileId`: tracked file identifier (GUID in standard 36-character format).
- Logic:
  1. Accept a dedicated CLI rerun mode that takes both `tenantId` and `trackedFileId`.
  2. Use the same validation rules for `tenantId` as the managed batch run.
  3. Invoke the same `FileTransferExecutor` batch runner as the managed batch run, passing run details that target only the specified tracked file.
  4. Return the batch runner result code; on unexpected failure, return `-1` and record the failure for support.
- Data: none in this component; it delegates to the batch runner.
- Errors:
  - Invalid `tenantId` or invalid `trackedFileId` causes usage guidance to be output and the run to end with `-1`.
- Target-state source: `inputs/target-state.md` section `0.5 Decision Status (Trigger / scheduling + rerun)`.

## Not carried over

None.