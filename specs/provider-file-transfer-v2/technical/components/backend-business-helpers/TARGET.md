---
type: Target Placement
title: technical/components/backend-business-helpers/TARGET
description: Where the backend-business-helpers component goes in the new system and what to build, method by method.
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
  body_sha256: eda28215010b74bf4a705492f932dc2b760db824c12ccece2673ce924c0387fa
---

﻿# Target: `backend-business-helpers`

## Where

- Deployable: `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`)
- Folder: `workers/fxfer-transfer-worker/src/Application/Helpers`

## What to create

Create the helper types used by transfer execution/business code for PGP encryption/decryption and stream utilities.

- `workers/fxfer-transfer-worker/src/Application/Helpers/DuplicateFileNameException.cs`
  - Type: `public sealed class DuplicateFileNameException : Exception`
  - Constructor: `DuplicateFileNameException(string message) : base(message)`.

- `workers/fxfer-transfer-worker/src/Application/Helpers/IOHelper.cs`
  - Type: `public static class IOHelper`
  - Methods:
    - `Stream GetStream(string stringData)`
      - Keep legacy behavior: write `stringData` to a `MemoryStream` using a `StreamWriter`, flush, set `Position = 0`, and return the `MemoryStream` instance (the legacy implementation creates the stream/writer inside `using` scopes).
    - `string GetString(Stream inputStream)`
      - Read the stream fully using `StreamReader.ReadToEnd()`.
    - `void WriteStream(Stream inputStream, ref byte[] dataBytes)`
      - Write `dataBytes` to the provided stream (`Write(dataBytes, 0, dataBytes.Length)`), with the same lifetime behavior as legacy (the legacy implementation disposes the provided stream via a `using` scope).

- `workers/fxfer-transfer-worker/src/Application/Helpers/EncryptionHelper.cs`
  - Type: `public static class EncryptionHelper`
  - Dependencies:
    - `BouncyCastle.Crypto` (BouncyCastle PGP types under `Org.BouncyCastle.*`).
  - Methods:
    - `string DecryptPgpData(string inputData, string outputFile, string privateKey, string password)`
      - Open `inputData` with `File.OpenRead`.
      - Convert `privateKey` string to a stream using `IOHelper.GetStream`.
      - Delegate to the stream overload.
    - `string DecryptPgpData(Stream inputStream, string outputFile, Stream privateKeyStream, string passPhrase)`
      - Preserve legacy PGP decoding and outputs:
        - output string starts as `" "` and is only set when the PGP payload is `PgpLiteralData` (non-compressed text read via `IOHelper.GetString`).
        - when the payload is compressed literal data, write the decrypted bytes to `outputFile` (or to `outputFile + "\\" + <literal filename>` when the payload is a signed message with a one-pass signature list).
        - wrap all exceptions as `PgpException("Encryption failed. please contact Support. Additional info: " + e.Message)`.
    - `void EncryptPgpFile(string inputFile, string outputFile, string publicKey, bool armor, bool withIntegrityCheck, out int result)`
      - Preserve legacy encryption behavior:
        - `result = 1` initially; set to `0` only in the output/encryption catch block.
        - compress using ZIP (`PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip)`).
        - encrypt using CAST5 (`PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, withIntegrityCheck, new SecureRandom())`).
        - use ASCII armor output when `armor == true`.
        - on encryption failure in output, throw `new PgpException("Encryption failed " + e.Message, e)`.
    - Private helpers (same signatures/behavior):
      - `PgpPrivateKey? FindSecretKey(PgpSecretKeyRingBundle pgpSec, long keyId, char[] pass)`.
      - `PgpPrivateKey? GetPrivateKey(string privateKeyPath)` (uses the hardcoded passphrase `"guttula"` and returns the first ElGamal private key).
      - `PgpPublicKey ReadPublicKey(Stream inputStream)` (returns first `IsEncryptionKey`, else throws `ArgumentException`).

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `EncryptionHelper.DecryptPgpData(string, string, string, string)` | `EncryptionHelper.DecryptPgpData(string, string, string, string)` | Keep overload that accepts file paths + key material as strings. |
| `EncryptionHelper.DecryptPgpData(Stream, string, Stream, string)` | `EncryptionHelper.DecryptPgpData(Stream, string, Stream, string)` | Keep exception wrapping + output behaviors. |
| `EncryptionHelper.EncryptPgpFile(string, string, string, bool, bool, out int)` | `EncryptionHelper.EncryptPgpFile(string, string, string, bool, bool, out int)` | Keep algorithm choices + `result` semantics. |
| `EncryptionHelper.FindSecretKey(PgpSecretKeyRingBundle, long, char[])` | `EncryptionHelper.FindSecretKey(PgpSecretKeyRingBundle, long, char[])` | Keep as a private helper. |
| `EncryptionHelper.GetPrivateKey(string)` | `EncryptionHelper.GetPrivateKey(string)` | Keep as a private helper (even though unused by other methods in legacy). |
| `EncryptionHelper.ReadPublicKey(Stream)` | `EncryptionHelper.ReadPublicKey(Stream)` | Keep as a private helper. |
| `IOHelper.GetStream(string)` | `IOHelper.GetStream(string)` | Keep stream construction + lifetime behavior. |
| `IOHelper.GetString(Stream)` | `IOHelper.GetString(Stream)` | Direct parity. |
| `IOHelper.WriteStream(Stream, ref byte[])` | `IOHelper.WriteStream(Stream, ref byte[])` | Direct parity. |

## Dependencies

- Component dependencies: none.
- Package dependencies: `BouncyCastle.Crypto`.

## Enterprise-Architecture standards applied

- `patterns/security/api-microservices/PTN-SEC-API-008-secret-management-pattern.md` section `Executive Summary > Solution Overview` (secret material is retrieved at runtime from an enterprise secret manager; no secrets are stored in code/config)
- `strategy/guidance/security/Data/ref-arch/GUID-SEC-DATA-ENC-KMS-RAD-3.5.02.md` section `Logical Architecture > Secrets Injection Pattern` (secrets/keys are injected at runtime into the executing workload; avoid persisting key material)
- `strategy/guidance/security/Data/ref-arch/GUID-SEC-DATA-ENC-KMS-RAD-3.5.02.md` section `Logical Architecture > Key Management Hierarchy` (keys are managed using an enterprise hierarchy rooted in a key-management service)
- `standards/infrastructure/iac/STD-INFRA-IAC-013-secrets-handling.md` section `Standard Requirements > Mandatory Requirements` (secrets are not committed to source control and are handled via approved secret mechanisms)
