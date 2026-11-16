---
type: Behavior Specification
title: technical/components/backend-data-file-transfer-tracking/SPEC
description: 'What the backend-data-file-transfer-tracking component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-file-transfer-tracking
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:37:57Z
sources:
- id: inventory
  resource: /technical/components/backend-data-file-transfer-tracking/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-file-transfer-tracking
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-bff
  folder: src/Contracts/Tracking
  body_sha256: 399e10e7ab9abda188cfa9b968fbd053b3d157f7095d8d7ed93e3dd943aebd86
---

﻿# backend-data-file-transfer-tracking: FXFER BFF tracking contract models

This component contains the legacy UX/BFF-facing data models used to represent file-transfer tracking search inputs, tracking search results, and tracking detail content (header/information + events + exceptions).
It is used by UX/controller code that binds datalist-backed selection fields and returns tracking DTOs to the UI.

## TrackingDetailModel
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferTracking.TrackingDetailModel`

Represents the tracking-detail overlay payload: a header/information object plus event and exception lists.

- Constructor initialization (always non-null):
  - `Events` = empty list
  - `Information` = new `TrackingInformationModel`
  - `Exceptions` = empty list
- Fields:
  - `Events`: `List<TrackingEventModel>`
  - `Exceptions`: `List<TrackingExceptionModel>`
  - `Information`: `TrackingInformationModel`

## TrackingEventModel
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferTracking.TrackingEventModel`

Represents one tracking event entry.

- Fields:
  - `CreateDate`: nullable datetime
  - `EventCode`: string
  - `Description`: string

## TrackingExceptionModel
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferTracking.TrackingExceptionModel`

Represents one diagnostic/exception entry associated with a tracked file.

- Fields:
  - `Date`: nullable datetime
  - `ExceptionType`: string
  - `Message`: string
  - `MethodName`: string
  - `StackTrace`: string

## TrackingInformationModel
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferTracking.TrackingInformationModel`

Represents the tracking header/information section for a file: identifiers, direction/status/transport selections, and origin/size metadata.

- Constructor initialization (always non-null):
  - `Direction`, `FileStatus`, `TransportType` = new list-item objects
- Fields:
  - `CheckSumCode`: string
  - `Direction`: list-item object (legacy `[ListItem]` bound to `FileTransfer.DataList.FileDirection`)
  - `FileId`: string (GUID in practice)
  - `FileName`: string
  - `FileStatus`: list-item object (legacy `[ListItem]` bound to `FileTransfer.DataList.FileStatus`)
  - `CreateDate`: nullable datetime
  - `OriginatingServerName`: string
  - `Size`: nullable decimal
  - `SourceLocation`: string
  - `TradingPartnerId`: string
  - `TransportType`: list-item object (legacy `[ListItem]` bound to `FileTransfer.DataList.ProtocolType`)

## TrackingSearchCriteriaModel
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferTracking.TrackingSearchCriteriaModel`

Represents the UI-facing search criteria for finding tracking records.

- Constructor initialization (always non-null):
  - `Direction`, `FileStatus`, `SourceLocationId`, `TransportType` = new list-item objects
- Fields:
  - `Direction`: list-item object (legacy `[ListItem]` bound to `FileTransfer.DataList.FileDirection`)
  - `FileId`: string (GUID in practice)
  - `FileName`: string
  - `FromDate`, `ToDate`: nullable datetime
  - `FromSize`, `ToSize`: nullable decimal
  - `FileStatus`: list-item object (legacy `[ListItem]` bound to `FileTransfer.DataList.FileStatus`)
  - `SourceLocationId`: list-item object
  - `TradingPartnerId`: string
  - `TransportType`: list-item object (legacy `[ListItem]` bound to `FileTransfer.DataList.ProtocolType`)

## TrackingSearchResultModel
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferTracking.TrackingSearchResultModel`

Represents one row returned by a tracking search.

- Constructor initialization (always non-null):
  - `Direction`, `FileStatus`, `TransportType` = new list-item objects
- Fields:
  - `Direction`: list-item object (legacy `[ListItem]` bound to `FileTransfer.DataList.FileDirection`)
  - `FileId`: string (GUID in practice)
  - `FileName`: string
  - `FileStatus`: list-item object (legacy `[ListItem]` bound to `FileTransfer.DataList.FileStatus`)
  - `CreateDate`: nullable datetime
  - `Size`: nullable decimal
  - `TradingPartnerId`: string
  - `TransportType`: list-item object (legacy `[ListItem]` bound to `FileTransfer.DataList.ProtocolType`)

## Rules implemented

None.

## Not carried over

- Legacy UX base types (`BaseModel`, `BaseEditorModel`, `BaseOverlayEditorModel`) and `[Serializable]` attributes: UX framework plumbing; replaced by plain .NET DTOs in the target.
- Legacy `[ListItem]` attributes and list-item wrapper fields: target contracts use explicit primitive fields (string/uuid) and datalist lookups via BFF endpoints.
