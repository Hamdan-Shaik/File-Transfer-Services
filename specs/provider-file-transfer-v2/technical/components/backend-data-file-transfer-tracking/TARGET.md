---
type: Target Placement
title: technical/components/backend-data-file-transfer-tracking/TARGET
description: Where the backend-data-file-transfer-tracking component goes in the new system and what to build, method by method.
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
  body_sha256: ddb61702e9d88e15ee264b10c0a2fed5ec6600b7c17e7676c97886303040d5b7
---

﻿# Target: `backend-data-file-transfer-tracking`

## Where

- Deployable: `fxfer-bff` (`services/fxfer-bff`)
- Folder: `services/fxfer-bff/src/Contracts/Tracking`

## What to create

Create BFF contract DTOs (response types) for the file tracking endpoints (search + detail), matching `technical/target/deployables/fxfer-bff/CONTRACT.md`.

- Tracking contracts:
  - `services/fxfer-bff/src/Contracts/Tracking/FileTransferTracking.cs`
    - Fields: `FileId`, `FileSourceLocationId`, `FileName`, `FileStatus`, `FileDirection`, `FileTransportType`, `CreatedDate`, `TradingPartner`, `CheckSumCode`, `FileSize`, `SourceLocAddress`, `SourceLocConfigName`, `OriginatingServerName`, `Events`, `Exceptions`.
    - `Events` and `Exceptions` are always non-null lists (default empty).
  - `services/fxfer-bff/src/Contracts/Tracking/FileEvent.cs`
    - Fields: `EventId`, `FileId`, `DestinationId`, `EventCode`, `EventTrackDescription`, `CreatedDate`.
  - `services/fxfer-bff/src/Contracts/Tracking/FileException.cs`
    - Fields: `ExceptionId`, `FileId`, `DestinationId`, `EventId`, `ExceptionType`, `MethodName`, `Message`, `StackTrace`, `CreatedDate`.
    - `StackTrace` and other diagnostic fields follow the deployable contract note: diagnostic content is redacted/sanitized for non-privileged callers.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `TrackingSearchCriteriaModel` | `drop: represented as query parameters on GET /api/file-transfer/v1/file-tracking` | Target uses explicit query params (`startDate`/`endDate`, `fileSizeFrom`/`fileSizeThrough`, `fileSourceLocationId`, etc.) rather than a criteria DTO. |
| `TrackingSearchResultModel` | `FileTransferTracking` | Legacy datalist-backed fields are replaced by primitive strings per BFF schema (`fileStatus`, `fileDirection`, `fileTransportType`). |
| `TrackingDetailModel` | `FileTransferTracking` | Legacy wrapper object (`Information` + lists) is flattened into one schema carrying header fields plus `events`/`exceptions`. |
| `TrackingInformationModel` | `merged into FileTransferTracking` | Header/information fields become top-level tracking fields in the BFF schema (including source-location address/config name). |
| `TrackingEventModel` | `FileEvent` | `Description` maps to `EventTrackDescription`; target adds identifiers (`eventId`, `fileId`, `destinationId`). |
| `TrackingExceptionModel` | `FileException` | `Date` maps to `CreatedDate`; target adds identifiers (`exceptionId`, `fileId`, `destinationId`, `eventId`). |

## Dependencies

None.

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-001-backend-for-frontend-pattern.md` section `Solution Overview` (BFF exposes UI-optimized contract DTOs and shapes downstream data)
- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `1. OpenAPI Specification Document` (DTOs are contract artifacts aligned to the OpenAPI-described schemas)
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `TL;DR` (sanitize/redact stack traces and other internal details from browser-facing payloads)
