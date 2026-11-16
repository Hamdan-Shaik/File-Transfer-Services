---
type: Target Placement
title: 'Target: shared-contracts-domain'
description: Where the shared-contracts-domain component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-domain
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:10:36Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-domain/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-domain
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Domain
  body_sha256: 2a13469d6dc7884fe169d8687f570ac900ed6474e631671a15f20e51df85e0a7
---

# Target: `shared-contracts-domain`

## Where

- Deployable: `fxfer-domain-api`
- Folder: `services/fxfer-domain-api/src/Contracts/Domain`

## What to create

Create the domain-level DTOs and request/response contract types used by `fxfer-domain-api` endpoints.
These types are JSON-serialized and form part of the private domain API contract (`technical/target/deployables/fxfer-domain-api/CONTRACT.md`).

Create the following C# types in `services/fxfer-domain-api/src/Contracts/Domain` (property naming and required/optional semantics follow the deployable contract schemas):

- FTP server profiles:
  - `FtpServerUpsertRequest` (must use `credentialSecretRef`; no raw password fields).
  - `FtpServerPatchRequest` (supports partial updates; credential secret reference is omitted to keep existing).
  - `FtpServer` (must not expose raw secret material; response does not include a password field).

- Transfer configurations (source locations + destinations):
  - `TransferConfigurationSummary`.
  - `TransferConfiguration`.
  - `DestinationUpsert` and `Destination`.
  - `SourceLocationOption`.

- Integration file definitions:
  - `IntegrationFileProcessingRequest`.
  - `IntegrationFileDefinition`.
  - `InboundTarget`.

- Tracking:
  - `FileTransferTracking`.
  - `FileEvent`.
  - `FileException`.
    - Diagnostic fields (e.g., message/stack trace) must support redaction/sanitization for non-privileged callers.

## Mapping

The legacy inventory contains no methods; mapping is at the model/type level.

| Legacy | Target | Note |
| --- | --- | --- |
| `FileEventModel` | `FileEvent` | Property parity.
| `FileExceptionModel` | `FileException` | Property parity; target-state redaction/sanitization applies.
| `FileTransferTrackingModel` | `FileTransferTracking` | Property parity; `events` and `exceptions` arrays map from the legacy lists.
| `FtpServerModel` | `FtpServer`, `FtpServerUpsertRequest`, `FtpServerPatchRequest` | Target contract does not carry `FtpPassword`; credentials are represented by `credentialSecretRef`.
| `FtsInboundTargetModel` | `InboundTarget` | Property parity.
| `FtsIntegrationFileModel` | `IntegrationFileDefinition` | Property parity.
| `GroupInfo` | `GroupInfo` (if referenced by target endpoints) | Retain as a shared domain DTO when used by queries.
| `SourceLocationModel` | `TransferConfiguration`, `TransferConfigurationSummary`, `SourceLocationOption` | Target splits list/summary/option shapes; destination list maps to `destinations`.

## Dependencies

None.

## Enterprise-Architecture standards applied

- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Benefits` (secrets never in Git/container images; centralized secrets management and rotation)
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Problem Details` (avoid information disclosure such as stack traces and internal details; sanitize/redact diagnostic content)

