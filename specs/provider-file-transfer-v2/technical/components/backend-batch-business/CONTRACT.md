---
type: Interface Contract
title: technical/components/backend-batch-business/CONTRACT
description: What the backend-batch-business component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-batch-business
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T00:36:41Z
sources:
- id: inventory
  resource: /technical/components/backend-batch-business/INVENTORY.yaml
  title: Every legacy class and method (19 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-batch-business
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 19
  methods: 78
  owned_rules: 0
  flows: []
  depends_on:
  - backend-business-helpers
  deployable: fxfer-transfer-worker
  folder: src/Application
  body_sha256: f01898e6a9f3814b1f7a0b8584ec76a4882f423d1f5d0a2210ab6108092fbc33
---

﻿# Contract: `backend-batch-business`

This component is placed in the `fxfer-transfer-worker` deployable under `src/Application`.
It implements the legacy batch file-transfer execution loop: discover work (producer), execute work (consumer), and perform protocol I/O plus tracking and notifications.

## Offers

- **Batch executor**: `FileTransferBatchExecutor : IBatchExecutor<int>`
  - `int Execute(string[] inputArgs)`.

- **Work discovery (producer)**: `IFTSProducer` / `FTSProducer`
  - `BlockingCollection<FileToDeliverModel> ProcessAllFilesSources()`
  - `BlockingCollection<FileToDeliverModel> ProcessFailedTransfers(int? topXFailedTransfers, Guid? specificSakFileTracking, int? specificConfigId)`

- **Work execution (consumer)**: `IFTSConsumer` / `FTSConsumer`
  - `bool ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue)`

- **Protocol-agnostic operations**: `IFileTransferOperations` / `FileTransferOperations`
  - `bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination)`
  - `string UploadFile(string fullyQualifiedSourceFileName, string fullyQualifiedDestinationFileName, DestinationModel destinationLocation)`
  - `bool DeleteFile(SourceLocationModel source, string fullyQualifedSourceFile)`
  - `bool RenameSourceFile(SourceLocationModel source, string fullyQualifedSourceFile, string renameExtension)`
  - `List<string> GetListOfFileNamesFromSource(SourceLocationModel location, int maxFilesPerCycle)`
  - `string GetDestinationFileName(string fullyQualifiedSourceFileName, DestinationModel destinationLocation, Guid fileId, string crc32, string uploadedSFTPUserName)`
  - `string CreateZipFile(string inputFileName)`
  - `void SendNotification(long configId, string subject, string body, bool isErrorNotification)`
  - `string GetCrc32(string filePath)`

- **Protocol implementations**: `IFileTransferProtocols` / `FileTransferProtocols`
  - UNC copy/delete/rename and FTP/SFTP/FTPS download/upload/delete/rename.
  - CRC32 calculation and FTP directory listing with mask filtering.

- **Pattern matching**: `IPatternMatcher` / `PatternMatcher`
  - `bool StrictMatchPattern(string expression, string name)`.

## Uses

- `backend-business-helpers`:
  - `EncryptionHelper` (PGP encrypt/decrypt used by `FTSConsumer.DeliverFile`).
  - `DuplicateFileNameException` (raised by destination upload when overwrite is not allowed).

## Data owned

None.

This component reads and writes batch tracking and notification data via batch data-access surfaces (tables referenced in `technical/components/backend-batch-business/INVENTORY.yaml`).

## Enterprise-Architecture standards applied

- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Must-Know Standards` (correlation identifiers are opaque; never encode PHI/PII into identifiers or log fields)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-004-structured-logging-pattern.md` section `Solution Overview` (structured logging; include contextual fields rather than unstructured text)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Solution Overview` (endpoint credentials and PGP key material are treated as secrets retrieved at runtime; secrets are not stored in code/config or emitted in logs)

