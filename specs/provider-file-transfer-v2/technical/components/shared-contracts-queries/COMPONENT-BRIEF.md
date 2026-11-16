---
type: Generation Brief
title: 'Brief: component shared-contracts-queries'
description: Instructions the tool gave the writer session for the shared-contracts-queries component.
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
  body_sha256: 2c0b04321138cd9df6514b6f5586f1a2d3443034f0ce7b759e9bae932bcf812e
---

# Brief: component `shared-contracts-queries`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries (shared tier, 8 classes, 5 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-contracts-queries/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-contracts-queries/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Contracts/Queries`: write `services/fxfer-domain-api/src/Contracts/Queries` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (1)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-100** Getting source location details requires a non-empty `SourceLocationId`; when it is missing, the query throws a validation exception with message `FtpSoureceLocationId`.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\GetSourceLocationQuery.cs:30-36, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:39
- Legacy: `Validate()` throws `new FileTransferValidationException("FtpSoureceLocationId")` when `Where.SourceLocationId` is empty.
- Decision: keep

