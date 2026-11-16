---
type: Data Access
title: 'Data: Run file transfer batch processing'
description: Tables, stored procedures and data the flow reads and writes.
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
  body_sha256: f123501bfc943e9ec96350f116b80f54d7f809473685b68d27989a8d9476c4ee
---

# Data: Run file transfer batch processing

## Tables

None.

## Stored procedures

None.

## Answers

This flow reports a single numeric result from each door.

| Door | Answer | Meaning |
| --- | --- | --- |
| main-batch-process | one number | The number returned by the file-transfer batch runner, or -1 when the run fails. |
| main-batch-process-2 | one number | The number returned by the file-transfer batch runner, or -1 when the run fails. |

