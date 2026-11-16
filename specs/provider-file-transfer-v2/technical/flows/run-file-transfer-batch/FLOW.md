---
type: Flow Specification
title: Run file transfer batch processing
description: Operations staff run a batch that starts the file transfer work for one tenant or as a selective run.
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
  body_sha256: ef834cfbb323b49aa1a0f6cebf3f883fcee01674dfec5bd20309438792f77c5d
---

# Run file transfer batch processing

Operations staff run a batch that starts the file transfer work for one tenant or as a selective run.
The run checks the start-up values and required settings before it begins.
The caller gets back one number: the batch runner's result, or -1 when the run fails.

## Who starts it

Operations staff (usually on a schedule).

## Doors

| Door | Request | Answers |
| --- | --- | --- |
| main-batch-process | Start-up values (any number of values) | One number from the batch runner, or -1 on failure |
| main-batch-process-2 | Start-up values (either 1 value or 3 values) | One number from the batch runner, or -1 on failure |

## Steps

1. (main-batch-process-2) Record a start record and always record an end record. (F04-003)
2. (main-batch-process-2) Check the start-up values follow an accepted pattern; otherwise show guidance and stop. (F04-004, F04-005, F04-006, F04-007, F04-008, F04-009, F04-010)
3. (main-batch-process-2) Ensure required run settings exist, loading tenant settings first when needed. (F04-011, F04-012)
4. (main-batch-process, main-batch-process-2) Prepare the runtime container and hand the work to the FileTransferExecutor batch runner. (F04-001, F04-013)
5. (main-batch-process, main-batch-process-2) If an unexpected failure occurs, record it for support and report -1. (F04-002, F04-014)

## Outcome

The caller receives one number.

Example: a failed run reports -1.

## Errors

- When the start-up values are not in an accepted pattern, the run stops early, shows guidance, and reports -1.
- When a required setting is missing, the run stops and reports -1.
- When an unexpected failure occurs, the run reports -1 and records the failure for support.

