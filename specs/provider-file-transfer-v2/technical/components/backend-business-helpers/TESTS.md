---
type: Test Plan
title: technical/components/backend-business-helpers/TESTS
description: The tests that prove the backend-business-helpers component, one or more per business rule.
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
  body_sha256: a06699ecbc7e14cc0448ed80eb4079a9862224cf674a714dd7c47c1d415f28e4
---

﻿# Tests: `backend-business-helpers`

This component owns no business rules.
Tests focus on method-level parity for helper behavior and basic contract checks for `openapi.yaml` and `CONTRACT.md`.

## Unit tests (method parity)

### DuplicateFileNameException
- `DuplicateFileNameException_sets_Message_from_constructor_argument`
  - Given `message = "dup"`, when `new DuplicateFileNameException(message)`, then `ex.Message` contains `message`.

### IOHelper
- `IOHelper_GetStream_returns_a_stream_positioned_at_zero`
  - Given `"abc"`, when `GetStream("abc")`, then `stream.Position == 0` immediately after return.

- `IOHelper_GetString_reads_the_entire_stream`
  - Given a stream containing `"hello"`, when `GetString(stream)`, then result equals `"hello"`.

- `IOHelper_WriteStream_writes_all_bytes`
  - Given a writable stream and `dataBytes` with known content, when `WriteStream(stream, ref dataBytes)`, then the stream contains exactly the written bytes.

### EncryptionHelper.DecryptPgpData (string overload)
- `EncryptionHelper_DecryptPgpData_string_overload_reads_input_file_and_delegates_to_stream_overload`
  - Given an existing `inputData` file and key/passphrase inputs, when calling the string overload, then it opens the file and produces the same observable result/exception as the stream overload.

### EncryptionHelper.DecryptPgpData (stream overload)
- `EncryptionHelper_DecryptPgpData_throws_PgpException_when_encrypted_data_list_is_missing`
  - Given an input stream that does not contain a `PgpEncryptedDataList`, when decrypting, then it throws `PgpException` with message prefix `"Encryption failed. please contact Support. Additional info: "`.

- `EncryptionHelper_DecryptPgpData_throws_PgpException_when_secret_key_is_not_found`
  - Given encrypted data where no matching key id exists in the secret key ring bundle, when decrypting, then it throws `PgpException` (wrapped) and includes `"Secret key for message not found."` in the message.

- `EncryptionHelper_DecryptPgpData_writes_outputFile_when_payload_is_compressed_literal_data_without_signature_list`
  - Given a compressed literal-data payload with no one-pass signature list, when decrypting with `outputFile` as a file path, then it creates/writes `outputFile`.

- `EncryptionHelper_DecryptPgpData_writes_outputFile_plus_literal_filename_when_payload_is_signed_compressed_literal_data`
  - Given a compressed payload where the first decompressed object is `PgpOnePassSignatureList` and the next is `PgpLiteralData` with `FileName = "a.txt"`, when decrypting with `outputFile` as a directory path, then it creates/writes `outputFile + "\\" + "a.txt"`.

- `EncryptionHelper_DecryptPgpData_returns_text_when_payload_is_literal_data`
  - Given an uncompressed `PgpLiteralData` payload containing a known text body, when decrypting, then it returns that text (and does not require a filesystem output file).

- `EncryptionHelper_DecryptPgpData_throws_PgpException_when_payload_is_one_pass_signature_list_without_literal_data`
  - Given a decrypted message object that is `PgpOnePassSignatureList`, when decrypting, then it throws `PgpException("Encrypted message contains a signed message - not literal data.")`.

- `EncryptionHelper_DecryptPgpData_throws_PgpException_when_payload_type_is_unknown`
  - Given a decrypted message object that is neither compressed data nor literal data nor a one-pass signature list, when decrypting, then it throws `PgpException("Message is not a simple encrypted file - type unknown.")`.

### EncryptionHelper.EncryptPgpFile
- `EncryptionHelper_EncryptPgpFile_sets_result_0_and_throws_PgpException_when_output_write_fails`
  - Given an `outputFile` path that cannot be created/written, when encrypting, then it sets `result = 0` and throws `PgpException` with message starting `"Encryption failed "` and with an inner exception.

- `EncryptionHelper_EncryptPgpFile_writes_armored_output_when_armor_true`
  - Given a valid input file and a public key, when `armor == true`, then output begins with the ASCII armor header for PGP.

- `EncryptionHelper_EncryptPgpFile_writes_binary_output_when_armor_false`
  - Given a valid input file and a public key, when `armor == false`, then output is not ASCII armored.

## Contract tests

- `openapi_yaml_is_minimal_and_marked_no_http_surface`
  - Assert `x-no-http-surface: true`.
  - Assert `paths` is `{}`.

- `contract_declares_no_cross_component_dependencies`
  - Assert `technical/components/backend-business-helpers/CONTRACT.md` has `Uses` section indicating no cross-component contracts.

## Enterprise-Architecture standards applied

- `patterns/security/api-microservices/PTN-SEC-API-008-secret-management-pattern.md` section `Executive Summary > Solution Overview` (test data must not embed real secret material; use ephemeral test keys)
- `standards/infrastructure/iac/STD-INFRA-IAC-013-secrets-handling.md` section `Standard Requirements > Mandatory Requirements` (no secret material is committed into the repo as test fixtures)
