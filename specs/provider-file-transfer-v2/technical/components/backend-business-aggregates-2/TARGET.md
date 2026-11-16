---
type: Target Placement
title: 'Target: backend-business-aggregates-2'
description: Where the backend-business-aggregates-2 component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-aggregates-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T17:36:00Z
sources:
- id: inventory
  resource: /technical/components/backend-business-aggregates-2/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-aggregates-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 3
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain
  body_sha256: 5eeb57fcd3130f0bfac282fcff91133beabaa66a1bce5516883deb49d0e0805a
---

# Target: backend-business-aggregates-2

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Domain`

## What to build

### Domain aggregate state

Use the existing domain aggregate state type created by component `backend-business-aggregates`:

- `services/fxfer-domain-api/src/Domain/Aggregates/FtsSourceLocationState.cs`
  - Type: `public sealed class FtsSourceLocationState`
  - Purpose: represent the current values of a File Transfer Source Location configuration record.
  - Constructors:
    - `FtsSourceLocationState()`
    - `FtsSourceLocationState(string id)` sets `Id = id`.
    - `FtsSourceLocationState(string id, int version)` sets `Id = id` and `Version = version`.
  - Properties (types match legacy):
    - `string Id`
    - `int ConfigId`
    - `string ConfigName`
    - `string Description`
    - `string FtpServerId`
    - `string Address`
    - `string Mask`
    - `int Version`
    - `string Protocol`
    - `string FileDirection`
    - `string OwnerEmail`
    - `string OwnerName`
    - `string OwnerPhone`
    - `string OwnerModule`
    - `string TradingPartner`
    - `bool RunOnMonday`
    - `bool RunOnTuesday`
    - `bool RunOnWed`
    - `bool RunOnThrus`
    - `bool RunOnFri`
    - `bool RunOnSat`
    - `bool RunOnSun`
    - `bool RunOnFrstDayOfMonthInd`
    - `bool RunOnLastDayOfMonthInd`
    - `byte RunOnThisDayOfMonth`
    - `string StartTime`
    - `string EndTime`
    - `string OperatorId`
    - `bool IsValid`
    - `DateTime EffectiveDate`
    - `bool SendNotifications`
    - `DateTime EndDate`
    - `DateTime LastModifiedDate`
    - `DateTime CreationDate`

This legacy type is structurally identical to `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business.Aggregates.FtsSourceLocationState` and is represented by the same target type.

### Dispatcher types

Do not create `CommandDispatcher` or `QueryDispatcher` in the target. The legacy types are empty subclasses used for legacy CQRS base discovery.

## Mapping (legacy → target)

| Legacy | Target | Note |
| --- | --- | --- |

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-008-domain-driven-design-pattern.md` section `2. Aggregates: Consistency Boundaries` (aggregate state is represented as a domain type in the Domain layer).
- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Recommended Requirements` (target CQRS mediation uses MediatR patterns rather than legacy dispatcher subclasses).

