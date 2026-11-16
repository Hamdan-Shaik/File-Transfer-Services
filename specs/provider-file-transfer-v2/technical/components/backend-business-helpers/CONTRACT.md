---
type: Interface Contract
title: technical/components/backend-business-helpers/CONTRACT
description: What the backend-business-helpers component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-helpers
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:45:18Z
sources:
- id: inventory
  resource: /technical/components/backend-business-helpers/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-helpers
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 3
  methods: 9
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Application/Helpers
  body_sha256: 1eb4173e6dbfbbda0fa097f5b598d86bb27a837fb3d4f2c4fc0ccb3a87812e5d
---

﻿# Contract: `backend-business-helpers`

This component is a helper library placed in the `fxfer-transfer-worker` deployable under `src/Application/Helpers`.
It provides utility types used by other worker components for PGP encryption/decryption and stream IO.

## Offers

- **Duplicate-file signaling**: `DuplicateFileNameException(string message)` for batch/business code paths that detect duplicate source filenames.

- **PGP decryption** (`EncryptionHelper`):
  - `DecryptPgpData(string inputData, string outputFile, string privateKey, string password) -> string`
  - `DecryptPgpData(Stream inputStream, string outputFile, Stream privateKeyStream, string passPhrase) -> string`
  - Output behavior parity:
    - file output is written to `outputFile` (or `outputFile + "\\" + <literal filename>` when decrypting a signed compressed payload).
    - returned string is a single space by default and is populated only when decrypting `PgpLiteralData` by reading the literal stream fully as text.
  - Error behavior parity: exceptions are wrapped to `PgpException` with the legacy message prefix.

- **PGP encryption** (`EncryptionHelper`):
  - `EncryptPgpFile(string inputFile, string outputFile, string publicKey, bool armor, bool withIntegrityCheck, out int result) -> void`
  - Uses BouncyCastle PGP with ZIP compression and CAST5 symmetric encryption.

- **Stream utilities** (`IOHelper`):
  - `GetStream(string stringData) -> Stream`
  - `GetString(Stream inputStream) -> string`
  - `WriteStream(Stream inputStream, ref byte[] dataBytes) -> void`

## Uses

None (no cross-component contracts declared by this component).

## Data owned

None.

## Enterprise-Architecture standards applied

- `patterns/security/api-microservices/PTN-SEC-API-008-secret-management-pattern.md` section `Executive Summary > Solution Overview` (key material and passphrases are treated as secrets and retrieved at runtime from an approved secret manager)
- `strategy/guidance/security/Data/ref-arch/GUID-SEC-DATA-ENC-KMS-RAD-3.5.02.md` section `Logical Architecture > Secrets Injection Pattern` (secrets are injected into the workload at runtime; avoid persisting private keys/passphrases)
- `standards/infrastructure/iac/STD-INFRA-IAC-013-secrets-handling.md` section `Standard Requirements > Mandatory Requirements` (no secrets in source control; approved secret handling mechanisms)
