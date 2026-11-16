---
type: Interface Contract
title: 'Contract: backend-file-transfer-mgmt-business'
description: What the backend-file-transfer-mgmt-business component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-mgmt-business
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:00:07Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-mgmt-business/INVENTORY.yaml
  title: Every legacy class and method (23 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-mgmt-business
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 23
  methods: 121
  owned_rules: 0
  flows: []
  depends_on:
  - backend-business-helpers-2
  deployable: fxfer-transfer-worker
  folder: src/Application
  body_sha256: 70cf77494111658087677b1ed873c9aeec2c72962bfbcc768aabfb985ec1af55
---

# Contract: `backend-file-transfer-mgmt-business`

This component is placed in the `fxfer-transfer-worker` deployable under `workers/fxfer-transfer-worker/src/Application`.
It provides the legacy batch file-transfer discovery + execution logic used by the worker runtime.

## Offers

- **Batch entry** (FileTransferBatchExecutor): Execute(string[] inputArgs) -> int.
- **Work discovery** (FTSProducer : IFTSProducer):
  - ProcessAllFilesSources(string tenantId) -> BlockingCollection<FileToDeliverModel>
  - ProcessFailedTransfers(int? topXFailedTransfers, Guid? specificFileId, int? specificConfigId, string tenantId) -> BlockingCollection<FileToDeliverModel>
- **Work execution** (FTSConsumer : IFTSConsumer): ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId) -> bool.
- **Consumer creation** (ConsumerFactory : IConsumerFactory): CreateConsumer(int consumerIdentifier) -> IFTSConsumer.
- **Protocol selection and utilities** (FileTransferOperations : IFileTransferOperations): download/upload/delete/rename/list, destination file naming, .zip creation, CRC32, notifications/events.
- **Protocol implementations** (FileTransferProtocols : IFileTransferProtocols): UNC + FTP/FTPS/SFTP transfer operations, .ftsUpload temp-file handling, directory auto-create rules.
- **File-mask matching** (PatternMatcher : IPatternMatcher): StrictMatchPattern(string expression, string name) -> bool.

## Uses

- Component dependencies:
  - `backend-business-helpers-2`:
    - `EncryptionHelper.EncryptPgpFile(string inputFile, string outputFile, string publicKey, bool armor, bool withIntegrityCheck, out int result) -> void` (encrypt outgoing transfers before upload when destination has an encryption public key)
    - `EncryptionHelper.DecryptPgpData(string inputData, string outputFile, string privateKey, string password) -> string` (decrypt downloaded files when destination requires decryption; `inputData` is a file path)
    - `EventHelper.PublishFileTransferredEvent(FileToDeliverModel fileDeliveryData, string tenantId, TimeSpan elapsedTime) -> void` (publish per-destination transfer events after each successful/attempted transfer)
    - `NotificationServiceHelper.SendEmail(string subject, string emailBody, List<Address> addressList, Address fromAddress, MailPriority priority, List<Attachments> attachmentList) -> void` (send operational and error notification emails)
    - `NotificationServiceHelper.PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId) -> void` (post operational events to the legacy notification service)
    - `DuplicateFileNameException(string message)` (signal "file exists at destination and overwrite not specified" and drive duplicate-file handling paths)

- External package contracts (must preserve behavior and edge cases):
  - ICSharpCode.SharpZipLib
  - Renci.SshNet
  - edtFTPnetPRO

## Data owned

None.

This component reads/writes legacy transfer tracking and operational records via IDataAccessFactory (file management status, file delivery events, error logs, and suspension records).

## Enterprise-Architecture standards applied

- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant-scoped execution and notifications)
- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Solution Overview` (FTP credentials, SSH keys, and key passphrases treated as secrets)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-004-structured-logging-pattern.md` section `Solution Overview` (avoid logging payload bytes/secrets; prefer structured contextual fields)