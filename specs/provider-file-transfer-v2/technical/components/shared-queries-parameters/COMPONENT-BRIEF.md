---
type: Generation Brief
title: 'Brief: component shared-queries-parameters'
description: Instructions the tool gave the writer session for the shared-queries-parameters component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-queries-parameters
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:38:34Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/shared-queries-parameters/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-queries-parameters
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 0
  owned_rules: 3
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Queries
  body_sha256: 342f23158eb22593af424768d379464beaf99990ac8f32112efc68f48322c99c
---

# Brief: component `shared-queries-parameters`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Queries.Parameters (shared tier, 8 classes, 0 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-queries-parameters/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-queries-parameters/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Contracts/Queries`: write `services/fxfer-domain-api/src/Contracts/Queries` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Business rules it owns (3)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-101** The requested `SourceLocationId` is parsed as a GUID and used to look up `FtsSourceLocation` by id; the lookup returns a source location only when exactly one row matches.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\GetSourceLocationQueryParms.cs:30-35, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:130-135
- Legacy: Parses the id via `Guid.Parse(SourceLocationId)` and queries `FtsSourceLocationId == query.Where.SoureceLocationGuid`, then returns `locations.First()` only when `locations.Count == 1`.
- Decision: keep
- ⚑ Flag: If `SourceLocationId` is not a GUID string, `Guid.Parse` throws.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-104** The details query does not filter by requested `IsActive` and always returns the source location when the id matches, including the stored active flag.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\GetSourceLocationQueryParms.cs:24-28, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:31-34, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:52-88
- Legacy: `ApplyWhereClause` returns the base query unchanged; the `IsActive` request parameter is not referenced, and the response model sets `IsActive = sourceLocation.IsValid`.
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-031** `InboundTargetQuery` initializes its `Where` object and defaults `FTEMetadataId` to `Guid.Empty`.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\InboundTargetQuery.cs:27-30, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\InboundTargetQueryParms.cs:18-21
- Legacy: A new `InboundTargetQuery` always has a non-null `Where`, and `Where.FTEMetadataId` starts as `Guid.Empty`.
- Decision: keep

