---
type: Interface Contract
title: technical/components/backend-providers-file-transfer/CONTRACT
description: What the backend-providers-file-transfer component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-providers-file-transfer
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T20:52:42Z
sources:
- id: rules-configure-source-locations-and-destinations
  resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
  title: Business rules it implements
- id: rules-manage-ftp-servers-and-credentials
  resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
  title: Business rules it implements
- id: rules-search-and-track-file-transfers
  resource: /technical/flows/search-and-track-file-transfers/RULES.md
  title: Business rules it implements
- id: inventory
  resource: /technical/components/backend-providers-file-transfer/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-providers-file-transfer
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 4
  methods: 73
  owned_rules: 15
  flows: []
  depends_on: []
  deployable: fxfer-bff
  folder: src/Application/Providers
  body_sha256: 8d16bbcb43219467dd259f300f5b4bdd6e1b912fb110b06dab29b9012079b479
---

﻿# Contract: `backend-providers-file-transfer`

This component defines the BFF *application providers* that implement file-transfer configuration commands/queries, FTP server credential/profile commands/queries, inbound-target metadata lookup, source-location options lookup, and file-tracking search/detail.

It is an internal application-layer contract within deployable `fxfer-bff` and is consumed by the API endpoint/controller component(s).

## Offers

- Command provider (configuration + FTP server writes)
  - `IFileTransferCommandProvider.AddFileTransferBaseConfigurationAsync(request, ct) -> TransferConfiguration`
  - `IFileTransferCommandProvider.UpdateFileTransferBaseConfigurationAsync(sourceLocationId, request, ct) -> TransferConfiguration`
  - `IFileTransferCommandProvider.InvalidateFileTransferBaseConfigurationAsync(sourceLocationId, ct) -> void`
  - `IFileTransferCommandProvider.AddFtpServerAsync(request, ct) -> FtpServer`
  - `IFileTransferCommandProvider.UpdateFtpServerAsync(ftpServerId, request, ct) -> FtpServer`
  - `IFileTransferCommandProvider.InvalidateFtpServerAsync(ftpServerId, request, ct) -> FtpServer`

- Query provider (configuration + FTP server reads + tracking)
  - `IFileTransferQueryProvider.SearchFileTransferAsync(search, ct) -> PagedResult<TransferConfigurationSummary>`
  - `IFileTransferQueryProvider.GetFileTransferConfigurationInformationAsync(sourceLocationId, ct) -> TransferConfiguration?`
  - `IFileTransferQueryProvider.GetFtpServersOptionsAsync(isValid, ct) -> IReadOnlyList<FtpServerOption>`
  - `IFileTransferQueryProvider.GetFtpServersAsync(search, removePassword, ct) -> PagedResult<FtpServer>`
  - `IFileTransferQueryProvider.GetMetadataIdsAsync(contractName, ct) -> IReadOnlyList<InboundTargetMetadata>`
  - `IFileTransferQueryProvider.GetSourceLocationsAsync(isActive, ct) -> IReadOnlyList<SourceLocationOption>`
  - `IFileTransferQueryProvider.SearchFileTransferTrackingAsync(search, ct) -> PagedResult<FileTransferTracking>`
  - `IFileTransferQueryProvider.GetFileTransferTrackingDetailAsync(fileId, ct) -> FileTransferTracking?`

## Uses

- Downstream deployable: `fxfer-domain-api` (all authoritative reads/writes)
  - Uses `FxferDomainApiClient` to call the downstream HTTP APIs.

- Reference data
  - Uses `DataListsClient` for reference-data lookup required for UI shaping (e.g., owner subsystem display name).

## Data owned

None (providers are stateless; they do not own storage).

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-001-backend-for-frontend-pattern.md` section `Executive Summary > Solution Overview` (providers shape UI-facing DTOs while delegating authoritative persistence to domain services)
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `Solution Overview` (errors are normalized at the HTTP boundary as Problem Details; provider surfaces downstream failures without leaking secrets/PII)
