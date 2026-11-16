---
type: Interface Contract
title: 'Contract: backend-configure-source-locations-and-destinations-api'
description: What the backend-configure-source-locations-and-destinations-api component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-configure-source-locations-and-destinations-api
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T06:28:48Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-manage-integration-files
  resource: /technical/flows/manage-integration-files/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-configure-source-locations-and-destinations-api/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-configure-source-locations-and-destinations-api
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 49
  owned_rules: 22
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  - search-and-track-file-transfers
  depends_on:
  - backend-bas-file-transfer-api
  - backend-bas-file-transfer-svc
  - backend-bas-file-transfer-svc-2
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  deployable: fxfer-domain-api
  folder: src/Api
  body_sha256: 0e33e9f77f041ec10fbc7b83a62054d26aef4a130b3a72007662847ce563b056
---

# Contract: backend-configure-source-locations-and-destinations-api

This component specifies the `fxfer-domain-api` HTTP endpoints that replace the legacy WCF/CoreWCF File Transfer service doors for configuring source locations/destinations, managing FTP servers, updating integration file definitions, and running tracking/lookup queries.

## Offers

### Private HTTP API (domain API)

Base path: `/api/fxfer-domain/v1`

This component offers the endpoint surface described in `technical/components/backend-configure-source-locations-and-destinations-api/openapi.yaml`.

Command endpoints (all require `Authorization: Bearer <JWT>`):

- `POST /transfer-configurations` -> `TransferConfiguration`
- `PUT /transfer-configurations/{sourceLocationId}` -> `TransferConfiguration`
- `PATCH /transfer-configurations/{sourceLocationId}` -> `TransferConfiguration`
- `POST /ftp-servers` -> `FtpServer`
- `PUT /ftp-servers/{ftpServerId}` -> `FtpServer`
- `PATCH /ftp-servers/{ftpServerId}` -> `FtpServer`
- `PATCH /integration-files/{integrationFileId}` -> `IntegrationFileDefinition`

Query endpoints:

- `GET /transfer-configurations` -> `PagedResultOfTransferConfigurationSummary`
- `GET /transfer-configurations/{sourceLocationId}` -> `TransferConfiguration`
- `GET /source-locations` -> `SourceLocationOption[]`
- `GET /ftp-servers` -> `PagedResultOfFtpServer`
- `GET /integration-files` -> `IntegrationFileDefinition[]`
- `GET /inbound-targets` -> `InboundTarget[]`
- `GET /file-tracking` -> `PagedResultOfFileTransferTracking`

Errors: all non-2xx responses use RFC 7807-compatible Problem Details (`application/problem+json`) per the deployable contract `technical/target/deployables/fxfer-domain-api/CONTRACT.md`.

## Uses

### Component dependencies

- `backend-bas-file-transfer-api`
  - Uses: API hosting composition and middleware pipeline for `fxfer-domain-api`.

- `backend-bas-file-transfer-svc`
  - Uses: legacy WCF SOAP contract types/signatures as a parity reference for `fxfer-soap-facade`.

- `backend-bas-file-transfer-svc-2`
  - Uses: legacy CoreWCF/API contract types/signatures as a parity reference for `fxfer-soap-facade`.

- `backend-business-command-handlers`
  - Uses: application-layer command handling for configuration mutations.

- `backend-business-command-handlers-2`
  - Uses: legacy duplicate/alternate namespace implementation; target behavior remains unified.

### Legacy package contracts

- `hpp.core.bas.cqrs`
  - Legacy behavior: the service doors call CQRS routing (`TryRoute(...)`) and return the routed object cast to the expected type.
  - Target behavior: preserved by calling MediatR handlers from HTTP endpoints and returning the handler result.

## Data

This component does not own persistence. Through downstream handlers/query projections it participates in tenant-scoped reads/writes of:

- `orm:FTP_SERVER`
- `orm:FTS_SUSPENSION`
- transfer configuration/source location projections (including destinations)
- integration file projections
- inbound target projections
- tracking projections

