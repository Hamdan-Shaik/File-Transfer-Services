---
type: Behavior Specification
title: technical/components/backend-business-helpers/SPEC
description: 'What the backend-business-helpers component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 2f6c01e937e11a6a3643d39ce63b102fe03134c240f9df0e08a60256d5d60f9b
---

﻿# backend-business-helpers: Batch business helper utilities (PGP encryption/decryption + IO)

This component is a small helper library used by batch/business code to:

- encrypt files to PGP using a provided public key string.
- decrypt PGP content using a provided private key string + passphrase.
- perform simple Stream/string/byte[] IO helper operations.

It owns no business rules and exposes no HTTP surface.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.Helpers.DuplicateFileNameException
Indicates that a duplicate file was found at the source.

Constructor behavior:

- `DuplicateFileNameException(string message)` passes `message` to the base `System.Exception` constructor.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.Helpers.EncryptionHelper
Static helper for PGP decryption/encryption using BouncyCastle.

### DecryptPgpData(string inputData, string outputFile, string privateKey, string password)
- Purpose: Decrypt a PGP-encrypted input file path and write the decrypted content to `outputFile` (or to a file inside `outputFile` when the PGP payload is signed).
- Inputs:
  - `inputData`: path to the encrypted file to read.
  - `outputFile`: output file path, or (when the payload is signed) a directory path.
  - `privateKey`: private key content as a string (not a file path).
  - `password`: passphrase for the private key.
- Logic:
  - Opens `inputData` using `File.OpenRead(inputData)`.
  - Converts `privateKey` to a `Stream` via `IOHelper.GetStream(privateKey)`.
  - Calls `DecryptPgpData(Stream inputStream, string outputFile, Stream privateKeyStream, string passPhrase)` and returns its return value.
- Data:
  - Reads: filesystem file at `inputData`.
  - Writes: filesystem file(s) under `outputFile` (delegated to the stream overload).
- Errors: Propagates any `PgpException` thrown by the stream overload.
- Rules: None.

### DecryptPgpData(Stream inputStream, string outputFile, Stream privateKeyStream, string passPhrase)
- Purpose: Decrypt a PGP-encrypted stream using a private key stream + passphrase, writing file output in the compressed/literal-data cases, and returning a string in the literal-data-as-text case.
- Inputs:
  - `inputStream`: PGP-encrypted input stream.
  - `outputFile`: output file path, or directory path when the payload is signed.
  - `privateKeyStream`: stream containing secret key ring bundle data.
  - `passPhrase`: passphrase for the secret key.
- Logic:
  - Initializes `output` to a single space: `" "`.
  - Creates `PgpObjectFactory` from `PgpUtilities.GetDecoderStream(inputStream)`.
  - Reads the first PGP object and then obtains `PgpEncryptedDataList`:
    - If the first object is a `PgpEncryptedDataList`, use it.
    - Otherwise, read the next PGP object and cast it to `PgpEncryptedDataList`.
  - Builds a `PgpSecretKeyRingBundle` from `PgpUtilities.GetDecoderStream(privateKeyStream)`.
  - Locates a matching private key:
    - Iterates `encryptedData.GetEncryptedDataObjects()`.
    - For each `PgpPublicKeyEncryptedData`, calls `FindSecretKey(pgpKeyRing, pubKeyDataItem.KeyId, passPhrase.ToCharArray())`.
    - Uses the first non-null private key found.
    - If `encryptedData` is null, throws `ArgumentException("Encrypted data for message not found.")`.
    - If no private key is found, throws `ArgumentException("Secret key for message not found.")`.
  - Decrypts the message:
    - Creates `PgpObjectFactory` from `pubKeyData.GetDataStream(privateKey)`.
    - Reads the next PGP object as `message`.
  - Handles `message` types:
    - If `message` is `PgpCompressedData`:
      - Creates a `PgpObjectFactory` over the decompressed stream and reads the next object as `message`.
      - If `message` is `PgpOnePassSignatureList`:
        - Reads the next object as `PgpLiteralData`.
        - Writes the literal data stream to a new file at `outputFile + "\\" + pgpLiteralData.FileName`.
      - Else if `message` is not null:
        - Casts `message` to `PgpLiteralData`.
        - Writes the literal data stream to a new file at `outputFile`.
      - Else throws `ArgumentException("Message is null.")`.
    - Else if `message` is `PgpLiteralData`:
      - Reads the literal data stream fully as text using `IOHelper.GetString(streamData)` and assigns it to `output`.
    - Else if `message` is `PgpOnePassSignatureList`:
      - Throws `PgpException("Encrypted message contains a signed message - not literal data.")`.
    - Else:
      - Throws `PgpException("Message is not a simple encrypted file - type unknown.")`.
  - Catches any exception `e` and throws `PgpException("Encryption failed. please contact Support. Additional info: " + e.Message)`.
  - Returns `output`.
- Data:
  - Reads: `inputStream`, `privateKeyStream`.
  - Writes: filesystem file at `outputFile` or at `outputFile + "\\" + <literal filename>`.
- Errors:
  - `ArgumentException` is raised internally for missing encrypted data, missing secret key, or null message, then wrapped into a `PgpException` by the catch block.
  - All exceptions are wrapped into `PgpException` with the fixed prefix `"Encryption failed. please contact Support. Additional info: "`.
- Rules: None.

### EncryptPgpFile(string inputFile, string outputFile, string publicKey, bool armor, bool withIntegrityCheck, out int result)
- Purpose: Encrypt a file to PGP using the provided public key string.
- Inputs:
  - `inputFile`: input file path.
  - `outputFile`: output file path.
  - `publicKey`: public key content as a string (not a file path).
  - `armor`: when true, outputs ASCII armored PGP.
  - `withIntegrityCheck`: when true, enables integrity protection in the PGP encrypted data generator.
  - `result`: set to `1` by default; set to `0` on encryption failure.
- Logic:
  - Sets `result = 1`.
  - Converts `publicKey` to a `Stream` via `IOHelper.GetStream(publicKey)` and reads the encryption key via `ReadPublicKey(publicKeyStream)`.
  - Creates a `MemoryStream` (`outputBytes`), then:
    - Creates a `PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip)`.
    - Writes `inputFile` content to literal data in the compressed stream via `PgpUtilities.WriteFileToLiteralData(..., PgpLiteralData.Binary, new FileInfo(inputFile))`.
    - Closes the compressor.
  - Creates `PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, withIntegrityCheck, new SecureRandom())`.
  - Adds the public key method via `dataGenerator.AddMethod(pubKey)`.
  - Gets `dataBytes = outputBytes.ToArray()`.
  - Writes encrypted bytes to `outputFile`:
    - If `armor` is true, wraps the output file stream in `ArmoredOutputStream`.
    - Opens an encryption output stream using `dataGenerator.Open(...)` and writes `dataBytes` via `IOHelper.WriteStream(...)`.
  - If any exception occurs during output file creation/encryption:
    - Sets `result = 0`.
    - Throws `new PgpException("Encryption failed " + e.Message, e)`.
- Data:
  - Reads: filesystem file at `inputFile`.
  - Writes: filesystem file at `outputFile`.
- Errors:
  - Throws `PgpException("Encryption failed " + <message>, <inner>)` when encryption fails during output.
- Rules: None.

### FindSecretKey(PgpSecretKeyRingBundle pgpSec, long keyId, char[] pass)
- Purpose: Locate a secret key by `keyId` and extract the corresponding private key using the provided passphrase.
- Inputs: `pgpSec`, `keyId`, `pass`.
- Logic:
  - `pgpSecKey = pgpSec.GetSecretKey(keyId)`.
  - If `pgpSecKey` is null, returns null.
  - Otherwise returns `pgpSecKey.ExtractPrivateKey(pass)`.
- Data: None.
- Errors: Any exceptions from `ExtractPrivateKey` propagate to the caller.
- Rules: None.

### GetPrivateKey(string privateKeyPath)
- Purpose: Read a private key from a secret key ring file and return the first ElGamal private key found.
- Inputs: `privateKeyPath` (filesystem path).
- Logic:
  - Opens `privateKeyPath` and wraps it in `PgpUtilities.GetDecoderStream(...)`.
  - Builds a `PgpSecretKeyRingBundle`.
  - Iterates all secret key rings and secret keys.
  - For each secret key, extracts a private key using the hardcoded passphrase `"guttula"`.
  - Returns the first extracted private key where `privKey.Key.GetType()` equals `ElGamalPrivateKeyParameters`.
  - Returns null if none found.
- Data: Reads filesystem file at `privateKeyPath`.
- Errors: Exceptions from file IO and key parsing/extraction propagate to the caller.
- Rules: None.

### ReadPublicKey(Stream inputStream)
- Purpose: Read a PGP public key ring stream and return the first key where `IsEncryptionKey` is true.
- Inputs: `inputStream`.
- Logic:
  - Wraps `inputStream` in `PgpUtilities.GetDecoderStream(...)`.
  - Builds `PgpPublicKeyRingBundle`.
  - Iterates key rings and public keys; returns the first key with `key.IsEncryptionKey`.
  - If no encryption key is found, throws `ArgumentException("Can't find encryption key in key ring.")`.
- Data: Reads `inputStream`.
- Errors: Throws `ArgumentException` when no encryption key exists; other parsing errors propagate to the caller.
- Rules: None.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Business.Helpers.IOHelper
Static helper for common Stream operations.

### GetStream(string stringData)
- Purpose: Convert an input string into a `Stream`.
- Inputs: `stringData`.
- Logic:
  - Creates a `MemoryStream` and writes `stringData` into it using a `StreamWriter`.
  - Flushes the writer and sets `stream.Position = 0`.
  - Returns the `MemoryStream` instance.
  - The `MemoryStream` and `StreamWriter` are created in `using` scopes.
- Data: None.
- Errors: None.
- Rules: None.

### GetString(Stream inputStream)
- Purpose: Read a stream fully as a string.
- Inputs: `inputStream`.
- Logic: Uses `StreamReader(inputStream).ReadToEnd()`.
- Data: Reads `inputStream`.
- Errors: Exceptions from the stream or reader propagate to the caller.
- Rules: None.

### WriteStream(Stream inputStream, ref byte[] dataBytes)
- Purpose: Write `dataBytes` to the provided stream.
- Inputs: `inputStream`, `dataBytes`.
- Logic:
  - Assigns `inputStream` to a local `outputStream` inside a `using` scope.
  - Calls `outputStream.Write(dataBytes, 0, dataBytes.Length)`.
- Data: Writes to `inputStream`.
- Errors: Exceptions from the stream write propagate to the caller.
- Rules: None.

## Rules implemented

None (this component owns no business rules).

## Not carried over

None.
