---
type: Interface Contract
title: 'Contract: shared-contracts-domain'
description: What the shared-contracts-domain component offers other components and what it uses from them.
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
  body_sha256: d7d00395508c42bf2ba56515e04f2dd70cf4c3cb110f098a8e66f828cf85ebe9
---

# Contract: `shared-contracts-domain`

This component defines the target-state domain contract types (DTOs) used by `fxfer-domain-api` for FXFER configuration and tracking.
The types in this component are consumed by API endpoints, command/query handlers, and other FXFER deployables that call the domain API.

## Offers

- **Domain DTOs (responses)**: `FtpServer`, `TransferConfigurationSummary`, `TransferConfiguration`, `Destination`, `SourceLocationOption`, `IntegrationFileDefinition`, `InboundTarget`, `FileTransferTracking`, `FileEvent`, `FileException`, and `GroupInfo` (when referenced).
- **Domain DTOs (requests)**: `FtpServerUpsertRequest`, `FtpServerPatchRequest`, `DestinationUpsert`, `IntegrationFileProcessingRequest`.

Security posture applied to DTOs:

- Endpoint credential values are never represented as raw password fields in request/response DTOs; the contract carries secret references/handles only (e.g., `credentialSecretRef`).
- Diagnostic content is represented in a way that allows redaction/sanitization before exposure to non-privileged callers (e.g., `FileException.message`/`stackTrace`).

## Uses

None.

## Data owned

None.

## Enterprise-Architecture standards applied

- `patterns/security/kubernetes/PTN-SEC-K8S-003-security-secrets-management-pattern.md` section `Benefits`
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Problem Details`

