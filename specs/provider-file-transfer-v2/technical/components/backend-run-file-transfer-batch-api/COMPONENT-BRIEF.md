---
type: Generation Brief
title: 'Brief: component backend-run-file-transfer-batch-api'
description: Instructions the tool gave the writer session for the backend-run-file-transfer-batch-api component.
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
  body_sha256: 2dd04439778f8e932d35af87d549ab560c5e79eaaf0955b0650d8a078f713a53
---

# Brief: component `backend-run-file-transfer-batch-api`

- API of the flow run-file-transfer-batch (backend tier, 2 classes, 7 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-run-file-transfer-batch-api/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-run-file-transfer-batch-api/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`), folder `src/Worker`: write `workers/fxfer-transfer-worker/src/Worker` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-transfer-worker/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

- `run-file-transfer-batch`: `technical/flows/run-file-transfer-batch/` (FLOW.md, RULES.md) and its scenarios `business/run-file-transfer-batch.feature`

## Business rules it owns (14)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-001** The batch run initializes its runtime container, resolves the named batch runner "FileTransferExecutor", and asks it to run using the provided run details.
- Doors: main-batch-process
- Source: BP1:23-26, IOC1:13
- Legacy: Calls BASUnityContainer.Initialize(), resolves IBatchExecutor<int> by name, then returns Execute(args).
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-002** When the batch run hits an unexpected failure, it records the failure message under "File Transfer Batch Process" and reports a failure result of -1.
- Doors: main-batch-process
- Source: BP1:28-31, EX1:12
- Legacy: Writes an event entry using ExceptionConstants.BatchSource and ex.Message, then returns -1.
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-003** The batch run records a start record and always records an end record, even when the run fails.
- Doors: main-batch-process-2
- Source: BP2:31-34, BP2:70-73
- Legacy: Writes the start messages during startup and always writes the ending message in finally.
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-004** The batch run continues only when the provided run details match one of the accepted patterns; otherwise it outputs usage guidance and reports a failure result of -1.
- Doors: main-batch-process-2
- Source: BP2:35-39, BP2:78-80, BP2:91-95, BP2:123-127, BP2:156-165
- Legacy: Main returns -1 when Validate(args) returns false, and ValidateInputArguments prints the usage text before returning false.
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-005** When the run is started with exactly one detail, the run treats it as the tenant identifier and treats the run as "process all files".
- Doors: main-batch-process-2
- Source: BP2:97-100
- Legacy: Sets processingAllFiles = true and sets tenantId = args[0].
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-006** When the run is started with exactly one detail, that tenant identifier must be a valid identifier in the standard 36-character format (for example 11111111-1111-1111-1111-111111111111).
- Doors: main-batch-process-2
- Source: BP2:101-105, BP2:123-127
- Legacy: When Guid.TryParse(args[0]) is false, the run prints usage guidance and returns false from validation.
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-007** When the run is started with exactly three details, the run treats the third detail as the tenant identifier.
- Doors: main-batch-process-2
- Source: BP2:91-95, BP2:109-112
- Legacy: When processingAllFiles is false, the run assigns tenantId = args[2].
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-008** When the run is started with exactly three details, the second detail must be either a whole number or a valid identifier in the standard 36-character format.
- Doors: main-batch-process-2
- Source: BP2:107-115, BP2:123-127
- Legacy: When both int.TryParse(args[1]) and Guid.TryParse(args[1]) are false, the run prints usage guidance and returns false from validation.
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-009** When the run is started with exactly three details, the tenant identifier (the third detail) must be a valid identifier in the standard 36-character format.
- Doors: main-batch-process-2
- Source: BP2:117-120, BP2:123-127
- Legacy: When Guid.TryParse(args[2]) is false, the run prints usage guidance and returns false from validation.
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-010** When the run is started with exactly three details, the first detail is not validated, even though the usage guidance suggests specific option names.
- Doors: main-batch-process-2
- Source: BP2:107-121, BP2:159-163
- Legacy: Validation checks only the second and third details (args[1] and args[2]); the usage guidance text lists sakTrack, configId, and top but the code does not enforce them.
- Decision: keep
- ⚑ Flag: The run accepts any first detail value in the three-detail form.

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-011** The batch run loads tenant settings only when they have not already been loaded.
- Doors: main-batch-process-2
- Source: BP2:43-46
- Legacy: Calls ApplicationConfigurationManager.LoadApplicationConfiguration(tenantId, new CacheManager()) only when ApplicationConfigurationManager.AppSettings is null.
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-012** Before starting the work, the batch run requires each mandatory setting key to be present; if any key is missing, it reports "{key} is not specified in app.config" and the run ends as a failure.
- Doors: main-batch-process-2
- Source: BP2:132-145, BP2:149-153, BP2:64-69, AS:12-18, AS:22-28
- Legacy: Calls CheckAndThrowConfigurationException(...) for ExternalFTECommonFolder, WorkingDirectory, MaxConcurrentDeliveryProcesses, MaxFilesPerCycle, MinsToWaitBeforeRetryFailedFile, MinsToWaitBeforeRetrySuspendedConfig, DisableNotifications, EnvironmentName, DeleteArchiveIfExists, DuplicateFileExtension, and ActivateDuplicateRenameLogic; missing keys cause a thrown ConfigurationErrorsException which is caught by Main and causes the run to return -1.
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-013** After validation and mandatory setting checks, the batch run initializes its runtime container, resolves the named batch runner "FileTransferExecutor", and asks it to run using the provided run details.
- Doors: main-batch-process-2
- Source: BP2:50-62, IOC2:13
- Legacy: Calls BASUnityContainer.Initialize(), resolves IBatchExecutor<int> by name, then returns Execute(args).
- Decision: keep

From `technical/flows/run-file-transfer-batch/RULES.md`:

**F04-014** When the batch run hits an unexpected failure, it records the failure for support, records the failure message under "File Transfer Batch Process", and reports a failure result of -1.
- Doors: main-batch-process-2
- Source: BP2:64-69, EX2:12
- Legacy: Calls the fatal logger, writes an event entry using ExceptionConstants.BatchSource and ex.Message, then returns -1.
- Decision: keep


## Target-state requirements it owns (1)

No legacy code does these; the target state asks for them. Specify each in SPEC.md under its id, with the section it comes from, and give each a test in TESTS.md.

- **T-007** Provide privileged rerun execution via a CLI mode targeting a specific tenant and tracked file identifier (without introducing a required UI rerun button). (target state: 0.5 Decision Status (Trigger / scheduling + rerun))
