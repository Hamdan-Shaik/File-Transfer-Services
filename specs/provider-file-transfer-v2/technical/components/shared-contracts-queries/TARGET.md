---
type: Target Placement
title: 'Target: shared-contracts-queries'
description: Where the shared-contracts-queries component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-queries
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:46:37Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-contracts-queries/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-queries
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 5
  owned_rules: 1
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Queries
  body_sha256: ff09c3e0d1f27ae1306cffded6b7ee8c1b2e182d55fd96f32bdc363c05265445
---

# Target: `shared-contracts-queries`

## Where

- Deployable: `fxfer-domain-api`
- Folder: `services/fxfer-domain-api/src/Contracts/Queries`

## What to create

Create the query contract types used by `fxfer-domain-api` query endpoints and query handlers.
These types represent read-only query requests (no side effects) and carry filter criteria for searching or looking up FXFER configuration and tracking data.

Create the following C# types in `services/fxfer-domain-api/src/Contracts/Queries`:

- `FileEventQuery`
  - Expose `Where` criteria for searching file events.

- `FileExceptionQuery`
  - Expose `Where` criteria for searching file exceptions.

- `FileTransferTrackingQuery`
  - Expose `Where` criteria for searching tracking records.

- `FtpServerQuery`
  - Expose `Where` criteria for searching FTP server profiles.

- `SourceLocationQuery`
  - Expose `Where` criteria for searching transfer configurations.

- `GetSourceLocationQuery`
  - Expose `Where` criteria for details lookup.
  - Implement `Validate()` such that it enforces rule F01-100:
    - when `Where` is null, or `Where.SourceLocationId` is null/empty, throw `FileTransferValidationException` with validation key/message `FtpSoureceLocationId`.

- `GetIntegrationFileQuery`
  - Expose `Where` criteria for integration-file lookup.
  - Provide a parameterless constructor that initializes `Where`.
  - Implement `Validate()` as a no-op (legacy validation does not raise errors for this query type).

- `InboundTargetQuery`
  - Expose `Where` criteria for inbound-target lookup.
  - Provide a parameterless constructor that initializes `Where`.
  - Implement `Validate()` as a no-op (legacy validation does not raise errors for this query type).

`Where` property naming and filter semantics must align with the deployable contract for the corresponding endpoints in `technical/target/deployables/fxfer-domain-api/CONTRACT.md`.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `GetIntegrationFileQuery.Validate()` | `GetIntegrationFileQuery.Validate()` | Preserve as no-op (legacy only allocates a list and never throws in practice). |
| `GetIntegrationFileQuery.HandleErrors()` | `drop: legacy no-op scaffolding` | Legacy throws only when internal `messages` is non-empty, which is not possible from callers. |
| `GetSourceLocationQuery.Validate()` | `GetSourceLocationQuery.Validate()` | Preserve rule F01-100: missing/empty source location id throws `FileTransferValidationException(FtpSoureceLocationId)`. |
| `InboundTargetQuery.Validate()` | `InboundTargetQuery.Validate()` | Preserve as no-op (legacy only allocates a list and never throws in practice). |
| `InboundTargetQuery.HandleErrors()` | `drop: legacy no-op scaffolding` | Legacy throws only when internal `messages` is non-empty, which is not possible from callers. |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Query Side Components` (query requests are separated from commands and must be read-only / side-effect free).
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (validation failures are represented consistently so the HTTP layer can surface standardized Problem Details responses).
- `standards/application/api/STD-APP-API-001-api-design.md` section `3.5 Error Handling` (validation failures use a consistent, machine-readable error code/message key).

