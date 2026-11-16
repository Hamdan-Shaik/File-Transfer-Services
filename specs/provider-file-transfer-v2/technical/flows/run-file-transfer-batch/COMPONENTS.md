---
type: Component Map
title: Components of the flow run-file-transfer-batch
description: The technical components that implement this flow, and the rules each one owns.
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
  body_sha256: d755de29baf241395faa2f476f6bcf2159b6bf658db17a520083bed4363cc297
---

# Components of the flow run-file-transfer-batch

The technical components that implement this flow, and the rules each one owns. Each component's spec is in `technical/components/<component>/`.

| Component | Tier | Doors here | Rules it owns |
| --- | --- | ---: | --- |
| [backend-run-file-transfer-batch-api](../../../technical/components/backend-run-file-transfer-batch-api/SPEC.md) | backend | 2 | F04-001..014 |
