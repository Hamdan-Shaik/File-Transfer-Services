---
type: Interface Contract
title: technical/components/backend-data-file-transfer/CONTRACT
description: What the backend-data-file-transfer component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-file-transfer
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:21:12Z
sources:
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-data-file-transfer/INVENTORY.yaml
  title: Every legacy class and method (11 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-file-transfer
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 11
  methods: 1
  owned_rules: 2
  flows:
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-bff
  folder: src/Contracts
  body_sha256: 63fc3fb2d6702341dd0469448641327f89ad21558bd043df259c93831a283b06
---

﻿# Contract: `backend-data-file-transfer`

This component defines the BFF contract DTOs used by `fxfer-bff` for transfer configurations and FTP server profiles, plus a small search-field helper used when translating UI search/filter input into a downstream query payload.

## Offers

- Transfer configuration DTOs
  - `TransferConfigurationUpsertRequest`, `TransferConfigurationPatchRequest`
  - `TransferConfigurationSummary`, `TransferConfiguration`
  - `DestinationUpsert`, `Destination`

- FTP server profile DTOs
  - `FtpServerUpsertRequest`, `FtpServerPatchRequest`
  - `FtpServer`, `FtpServerOption`

- Shared/common DTOs
  - `PagedResult<T>`
  - `SourceLocationOption`
  - `InboundTargetMetadata`

- Search helper
  - `SearchField` (name/value pair)
  - `SearchHelper.GetFieldValue(searchFields, fieldName)`

## Uses

None (no component dependencies).

## Data owned

None (contract-only types; no storage).

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `1. OpenAPI Specification Document` (DTOs are treated as contract-first artifacts aligned to OpenAPI schemas)