---
type: Generation Brief
title: 'Brief: component backend-providers-file-transfer'
description: Instructions the tool gave the writer session for the backend-providers-file-transfer component.
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
  body_sha256: 3f2b53e04aa9dcf5492abe106a7a595b2cfb33bf2b861a62088ec581ce325616
---

# Brief: component `backend-providers-file-transfer`

- HP.HSP.UA3.FileTransfer.UX.Providers.FileTransfer (backend tier, 4 classes, 73 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/backend-providers-file-transfer/INVENTORY.yaml`
- Write exactly these files in `technical/components/backend-providers-file-transfer/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-bff` (`services/fxfer-bff`), folder `src/Application/Providers`: write `services/fxfer-bff/src/Application/Providers` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-bff/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

None.

## Flows it takes part in

None.

## Packages it calls (contracts to keep)

- `automapper`
- `hp.hsp.ua3.core.ux.common`
- `hp.hsp.ua3.core.ux.data`
- `hp.hsp.ua3.core.ux.interfaces`
- `hp.hsp.ua3.core.ux.providers`

## Business rules it owns (15)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-027** When building a destination entry, the provider removes hyphens (`-`) from the destination contact phone number when it is present.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:90-100
- Legacy: If `destination.ContactPhone != null`, sets `destinationTempData.ContactPhone = destination.ContactPhone.Replace("-", string.Empty)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-028** When a destination type is `FileToEvent`, the provider overwrites the destination address with the configured External FTE common folder application setting.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:82-85, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:102-107
- Legacy: Reads `ExternalFTECommonFolder` via `TenantConfigurationManager.GetApplicationSetting<string>(..., ExternalFTECommonFolder)` and, when `destination.DestinationType.ID == ...FileToEvent`, sets `destination.DestinationAddress = externalFTECommonFolder` before copying it into the service destination DTO.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-029** When mapping a destination FTP server id, the provider sends `Guid.Empty` when the UI value is an empty string; otherwise it parses the string as a GUID.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:113
- Legacy: `destinationTempData.FtpServerId = destination.FTPServer.ID == string.Empty ? Guid.Empty : Guid.Parse(destination.FTPServer.ID)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-030** When the UI ?run on this day? value is empty, the provider sets `RunOnThisDayOfMonth` to byte `0`; otherwise it converts the string to a byte.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:149
- Legacy: `RunOnThisDayOfMonth = request.Data.RunOnThisDay.ID != string.Empty ? Convert.ToByte(request.Data.RunOnThisDay.ID) : Convert.ToByte(0)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-031** When the UI marks the configuration as suspended, the provider sets `IsSuspendedChangedDate` to the current UTC time; otherwise it sends null.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:155-156
- Legacy: `IsSuspendedChangedDate = request.Data.Suspended ? (DateTime?)DateTime.UtcNow : null`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-032** The provider calls the FileTransfer service `FileTransferAdd` operation with an `FtsSourceLocationAddCommand` built from the UI model and destinations.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:124-157, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:159-176
- Legacy: Builds `FtsSourceLocationAddCommand` (including `DestinationAddress = destArray`) and calls `serviceProxy.FileTransferAdd(cmd)`; when a non-null response is returned, maps it and returns an `OK` status response.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-033** The provider turns service faults and unexpected exceptions into an error `ResponseModel` using standard handlers.
- Doors: file-transfer-controller-add-file-transfer-base-configuration
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:178-189
- Legacy: Catches `FaultException<ServiceException>`, `FaultException<BusinessValidationException>`, and `Exception`, then calls `HandleServiceException`, `HandleBusinessException`, or `HandleException`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-051** When adding a new FTP server, the service command assigns a new server id (`Guid.NewGuid()`) and uses the requestor identifier as the operator id.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:724-738
- Legacy: `BuildAddFtpServerCommand` sets `FtpServerId = Guid.NewGuid()` and `OperatorId = request.Requestor.IdentifierId`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-052** When the add service returns a non-null response, the server responds with a model containing the new `FtpServerId`, preserves the request's `Editor` object, and marks the result status as `OK`.
- Doors: file-transfer-controller-create-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:650-661
- Legacy: `AddFtpServer` maps `serviceResponse.FtpServerId` into a new `FtpServerModel`, copies `request.Data.Editor`, and sets `response.Result.StatusCode = OK`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-056** When updating an FTP server and no new password is provided, the server reuses the existing password fetched from the credentials query with `RemovePassword=false`.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:743-759, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:770-777, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:788-795
- Legacy: `BuildUpdateFtpServerCommand` uses `request.Data.FtpPassword` when non-empty, else calls `GetFTPServerPass`, which queries for the server with `RemovePassword=false` and uses the first returned record's `FtpPassword`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-058** When the update service returns a non-null response, the server responds with a model containing `FtpServerId` and `IsValid`, preserves the request's `Editor` object, and marks the result status as `OK`.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:672-684
- Legacy: `UpdateFtpServer` maps `serviceResponse.FtpServerId` and `serviceResponse.IsValid`, copies `request.Data.Editor`, and sets `response.Result.StatusCode = OK`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-059** When the invalidate service returns a non-null response, the server responds with a model containing `FtpServerId` and `IsValid`, preserves the request's `Editor` object, and marks the result status as `OK`.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:695-707
- Legacy: `InvalidateFtpServer` maps `serviceResponse.FtpServerId` and `serviceResponse.IsValid`, copies `request.Data.Editor`, and sets `response.Result.StatusCode = OK`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-060** Invalidate requests build the invalidate command by parsing `FtpServerId` as a GUID and mapping the requestor.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:814-820
- Legacy: `BuildInvalidateFtpServerCommand` sets `FtpServerId = Guid.Parse(request.Data.FtpServerId)` and maps `Requestor`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-063** When updating FTP server credentials with no new password, if the password lookup returns `OK` with an empty result list, the update request fails with an unhandled exception.
- Doors: file-transfer-controller-update-ftpserver-credentials
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:444-469, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferServiceDataProvider.cs:743-779
- Legacy: `UpdateFtpServer` calls `BuildUpdateFtpServerCommand(request)` before its try/catch; `GetFTPServerPass` calls `resp.Data.First()` when the credentials query returns `OK` and `resp.Data != null`, without checking whether the list is empty.
- Review: GAP-008
- Decision: keep

From `technical/flows/search-and-track-file-transfers/RULES.md`:

**F05-035** Tracking search criteria mapping sets only supported filter fields, and date/size filters are applied only when their values can be parsed.
- Doors: file-transfer-controller-search-file-transfer-tracking-2
- Source: ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.Tracking.cs:36, ux\HP.HSP.UA3.FileTransfer.UX.Providers\FileTransfer\FileTransferQueryServiceDataProvider.Tracking.cs:126-203
- Legacy: The provider maps `SearchFieldModel.FieldName` values `FileId`, `FileName`, `FileStatus`, `Direction`, `TransportType`, `TradingPartnerId`, and `SourceLocationId` to query parameters as strings; it applies `FromDate`/`ToDate` only when `DateTime.TryParse(...)` succeeds and `FromSize`/`ToSize` only when `decimal.TryParse(...)` succeeds (unsupported field names are ignored).
- Review: GAP-002
- Decision: keep


## Target-state requirements it owns (1)

No legacy code does these; the target state asks for them. Specify each in SPEC.md under its id, with the section it comes from, and give each a test in TESTS.md.

- **T-008** Prohibit direct database access from the BFF; all persistence must occur via domain services. (target state: 5 BFF (DB access forbidden))
