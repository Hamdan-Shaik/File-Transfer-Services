---
type: Behavior Specification
title: 'backend-business-aggregates-2: aggregate state + legacy CQRS dispatchers'
description: 'What the backend-business-aggregates-2 component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 46b6ba501bd48771cca6d87dfa5b7ba18d41bbf64d954acb7eb606075e10b80f
---

# backend-business-aggregates-2: aggregate state + legacy CQRS dispatchers

This component contains a File Transfer aggregate-state type plus legacy CQRS dispatcher marker types used by the legacy BAS CQRS framework.

## FtsSourceLocationState
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.Aggregates.FtsSourceLocationState`

Aggregate state object representing the current values of a File Transfer Source Location configuration record.

- Constructors:
  - `FtsSourceLocationState()`
  - `FtsSourceLocationState(string id)` sets `Id = id`.
  - `FtsSourceLocationState(string id, int version)` sets `Id = id` and `Version = version`.
- State fields (all are auto-properties in legacy):
  - `Id` (string): internal identifier; documented as GUID format `XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX`.
  - `ConfigId` (int)
  - `ConfigName` (string)
  - `Description` (string)
  - `FtpServerId` (string)
  - `Address` (string)
  - `Mask` (string)
  - `Version` (int)
  - `Protocol` (string)
  - `FileDirection` (string)
  - `OwnerEmail` (string)
  - `OwnerName` (string)
  - `OwnerPhone` (string)
  - `OwnerModule` (string)
  - `TradingPartner` (string)
  - `RunOnMonday` (bool)
  - `RunOnTuesday` (bool)
  - `RunOnWed` (bool)
  - `RunOnThrus` (bool)
  - `RunOnFri` (bool)
  - `RunOnSat` (bool)
  - `RunOnSun` (bool)
  - `RunOnFrstDayOfMonthInd` (bool)
  - `RunOnLastDayOfMonthInd` (bool)
  - `RunOnThisDayOfMonth` (byte)
  - `StartTime` (string)
  - `EndTime` (string)
  - `OperatorId` (string)
  - `IsValid` (bool)
  - `EffectiveDate` (DateTime)
  - `SendNotifications` (bool)
  - `EndDate` (DateTime)
  - `LastModifiedDate` (DateTime)
  - `CreationDate` (DateTime)

## CommandDispatcher
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.CommandDispatcher`

Concrete type inheriting legacy `CommandDispatcherBase` with no additional behavior.

## QueryDispatcher
Legacy type: `HPP.FileTransfer.BAS.FileTransferSvc.Business.QueryDispatcher`

Concrete type inheriting legacy `QueryDispatcherBase` with no additional behavior.

## Rules implemented

None.

## Not carried over

- `CommandDispatcher`: target command routing does not use legacy `CommandDispatcherBase` subclasses.
- `QueryDispatcher`: target query routing does not use legacy `QueryDispatcherBase` subclasses.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-008-domain-driven-design-pattern.md` section `2. Aggregates: Consistency Boundaries` (aggregate-related types live in the Domain layer and represent a single consistency boundary concept).
- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Write Side (Command Model)` (command-side domain types support command handling; legacy dispatcher types are infrastructure/framework-specific and are not carried forward).

