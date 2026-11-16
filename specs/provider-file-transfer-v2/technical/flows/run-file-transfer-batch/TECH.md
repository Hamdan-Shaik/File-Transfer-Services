---
type: Technical Flow Map
title: 'Technical map: Run file transfer batch processing'
description: 'How the legacy code implements the flow: call chains, classes, data, outside calls.'
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main.BatchProcess
tags:
- batch
- flow
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T07:15:42Z
sources:
- id: door-main-batch-process
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main/BatchProcess.cs#L17
  title: BatchProcess (batch door)
- id: door-main-batch-process-2
  resource: batch/FileTransferManagement/HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main/BatchProcessProgram.cs#L21
  title: BatchProcess (batch door)
- id: footprint
  resource: /technical/flows/run-file-transfer-batch/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:run-file-transfer-batch
  generation:
    status: done
    attempts: 2
  rule_prefix: F04
  rules: 14
  doors:
  - main-batch-process
  - main-batch-process-2
  tables: 0
  procedures: 0
  components:
  - backend-run-file-transfer-batch-api
  body_sha256: df0898e89b9d47faee04812891f3de2ec09ab78cba5564ef4abaa810e4dcd683
---

# Technical map: Run file transfer batch processing

How the legacy code implements the flow `run-file-transfer-batch`, as the code graph sees it. Written by the tool from `FOOTPRINT.yaml`, not by AI; the business view is in `FLOW.md` and `RULES.md`.

## Doors

| Door | Kind | Where |
| --- | --- | --- |
| `main-batch-process` (BatchProcess) | batch | `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main\BatchProcess.cs:17` |
| `main-batch-process-2` (BatchProcess) | batch | `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main\BatchProcessProgram.cs:21` |

## Call chains

### `main-batch-process`

- `BatchProcess.Main` `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main\BatchProcess.cs:19`

### `main-batch-process-2`

- `BatchProcess.Main` `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main\BatchProcessProgram.cs:26`
  - `BatchProcess.Validate` `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main\BatchProcessProgram.cs:76`
    - `BatchProcess.ValidateInputArguments` `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main\BatchProcessProgram.cs:86`
      - `BatchProcess.PrintUsage` `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main\BatchProcessProgram.cs:156`
  - `BatchProcess.ValidateAppConfig` `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main\BatchProcessProgram.cs:132`
    - `BatchProcess.CheckAndThrowConfigurationException` `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main\BatchProcessProgram.cs:147`

## Classes involved (by namespace; methods walked)

- `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main`: `BatchProcess` (1)
- `HPE.HSP.UA3.FileTransfer.Batch.FileTransferMgmt.Main`: `BatchProcess` (6)

## Data

None reached in the graph.

## Calls into code the system does not own

- **** : `?.LogError` (1 call sites)
- **** : `?.LogFatal` (1 call sites)
- **** : `?.LogInformational` (9 call sites)
- **** : `?.Resolve` (2 call sites)
- **** : `ApplicationConfigurationManager.LoadApplicationConfiguration` (1 call sites)
- **** : `BASUnityContainer.Initialize` (2 call sites)

## Where the walk stops

The graph sees nothing these methods call: a router, a call into an unresolved package, reflection, configuration, or simply a leaf. Read the code from here.

Nowhere: every path ends in data, a package call or a leaf the graph understands.

## Shared pieces used (not walked)

None.
