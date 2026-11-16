---
type: Generation Brief
title: 'Brief: component shared-contracts-commands'
description: Instructions the tool gave the writer session for the shared-contracts-commands component.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-commands
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-16T05:20:12Z
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
- id: inventory
  resource: /technical/components/shared-contracts-commands/INVENTORY.yaml
  title: Every legacy class and method (12 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-commands
  generation:
    status: done
    attempts: 2
  tier: shared
  classes: 12
  methods: 36
  owned_rules: 27
  flows:
  - configure-source-locations-and-destinations
  - manage-ftp-servers-and-credentials
  - manage-integration-files
  depends_on:
  - shared-contracts-utilities
  deployable: fxfer-domain-api
  folder: src/Contracts/Commands
  body_sha256: 8e82b63638e16c4d0e4d5e5b40c7a5758f71325a69bb4edaae4cb697e20b5ffb
---

# Brief: component `shared-contracts-commands`

- HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands (shared tier, 12 classes, 36 methods)
- Playbook: `inputs/playbook/component.md`
- Every class and method to cover: `technical/components/shared-contracts-commands/INVENTORY.yaml`
- Write exactly these files in `technical/components/shared-contracts-commands/`: `SPEC.md`, `TARGET.md`, `CONTRACT.md`, `openapi.yaml`, `TESTS.md`
- Target state: `inputs/target-state.md`
- Goes into: deployable `fxfer-domain-api` (`services/fxfer-domain-api`), folder `src/Contracts/Commands`: write `services/fxfer-domain-api/src/Contracts/Commands` in TARGET.md
- Deployable spec: `technical/target/deployables/fxfer-domain-api/`

## Components it uses (read their CONTRACT.md where written, else their INVENTORY.yaml)

- `shared-contracts-utilities`: `technical/components/shared-contracts-utilities/`

## Flows it takes part in

- `configure-source-locations-and-destinations`: `technical/flows/configure-source-locations-and-destinations/` (FLOW.md, RULES.md) and its scenarios `business/configure-source-locations-and-destinations.feature`
- `manage-ftp-servers-and-credentials`: `technical/flows/manage-ftp-servers-and-credentials/` (FLOW.md, RULES.md) and its scenarios `business/manage-ftp-servers-and-credentials.feature`
- `manage-integration-files`: `technical/flows/manage-integration-files/` (FLOW.md, RULES.md) and its scenarios `business/manage-integration-files.feature`

## Business rules it owns (27)

Implement each one; list them all under 'Rules implemented' in SPEC.md with a link to the flow's RULES.md.

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-038** Each add command validation run starts with an empty validations list.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28-31
- Legacy: `FtsCommandValidate()` sets `Validations = new List<BusinessExceptionMessage>()`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-039** When any validations have been added, the command throws a `BusinessException` with error code `E` and message `Command Execution Error`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33-41, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:21-47
- Legacy: `HandleValidations()` throws `BusinessException(validations, "Command Execution Error", "E")` when `Validations.Any()`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-040** Each validation added to the command uses the same value for the validation key and field, plus a message.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44-47
- Legacy: `AddValidation(keyName, message)` appends `new BusinessExceptionMessage(keyName, keyName, message)`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-041** The add command records a `Missing field` validation for each required string field that is null or empty.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:272-288, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:319-324, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:45-47
- Legacy: Validates `ConfigName`, `Protocol`, `Address`, `OwnerName`, `OwnerModule`, `OwnerPhone`, `OwnerEmail`, `StartTime`, and `EndTime`; when a value is empty, adds a validation with message `Missing field`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-042** The add command requires at least one destination address marked valid; otherwise it records `Missing field` for `DestinationAddress`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:327-333, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:45-47
- Legacy: Adds a `DestinationAddress` validation when the list is null, empty, or when the count of entries where `IsValid == true` is zero.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-043** The add command requires at least one run-schedule option to be selected; otherwise it records a `Run Schedule` validation with message `Invalid value for`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:335-349, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:44-55
- Legacy: When `RunOnThisDayOfMonth == 0` and all day-of-week flags and month-edge flags are false, adds a validation with key `Run Schedule` and message `Invalid value for`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-044** When the protocol code is `1` (FTP), the add command requires a non-empty `FtpServerId` and throws `ArgumentNullException("FtpServerId")` when it is missing.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:293-301, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:11-12, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:40-42
- Legacy: Checks `Protocol == "1"` and throws `ArgumentNullException("FtpServerId")` when `FtpServerId` is empty.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-045** When the source file mask has an extension in the excluded list (e.g. `exe`, `dll`, `ps1`), the add command records a `Mask` validation with message `Source file mask extension is not Allowed: <extension>`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:304-317, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\ExcludedFileExtensions.cs:13-37, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:40-42
- Legacy: Extracts the substring after the last `.` in `Mask` (lowercased); when it matches an `ExcludedFileExtensionList` value, adds a validation for `Mask` using the excluded-extension message prefix.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-059** When updating a source location, the flow treats `ConfigName`, `Protocol`, `Address`, `OwnerName`, `OwnerModule`, `OwnerPhone`, `OwnerEmail`, `StartTime`, and `EndTime` as required string fields; each missing/empty value adds a validation entry with message `Missing field`, and if any validation entries exist the flow throws a `BusinessException` with message `Command Execution Error` and error code `E`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268-286, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:271-279, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:342-348, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28-41, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44-46, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:21, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:46
- Legacy: These fields can all be non-empty but still invalid for later processing (for example, a non-GUID `OwnerModule` can still fail later during GUID parsing).
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-060** When updating a source location, if no run-schedule indicators are selected (`RunOnThisDayOfMonth == 0` and all daily/monthly flags are false), the flow adds a validation entry with key `Run Schedule` and message `Invalid value for` and then fails validation as part of the command's `HandleValidations()`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:350-364, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:44, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:54
- Legacy: The message stored for this error is the generic prefix `Invalid value for` (it does not include the property name).
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-061** When updating a source location and `Protocol` equals `1`, the flow requires `FtpServerId` to be present; otherwise it throws `ArgumentNullException(FtpServerId)`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:24, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:289-296, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:295, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:41
- Legacy: The check is conditional on the literal protocol code value `1`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-062** When updating a source location, the flow requires `FtsSourceLocationId` to be present; otherwise it throws `ArgumentNullException(FtpSoureceLocationId)`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:300-306, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:304, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:92-99, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:88-95
- Legacy: The update handler does not use the inbound `FtsSourceLocationId`; it looks up the current source location by `ConfigId` and uses the found record's id.
- ⚑ Flag: Callers must supply `FtsSourceLocationId` even though the handler ignores it.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-063** When updating a source location and `IsSuspended` is true, the flow requires `IsSuspendedChangedDate` to be present; otherwise it throws `ArgumentNullException(IsSuspendedChangedDate)`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:308-316, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:314
- Legacy: The date requirement is enforced only when `IsSuspended` is true.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-064** When updating a source location, the flow requires `ConfigId` to be non-zero; otherwise it throws `ArgumentNullException(ConfigIdNotFound)`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:319-325, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:323, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23
- Legacy: The exception parameter name comes from the constant value `ConfigIdNotFound`.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-065** When updating a source location, if `Mask` ends with one of the excluded extensions (`exe`, `dll`, `bat`, `cmd`, `vbs`, `ps1`, `jar`, `app`, `com`, `scr`, `msi`, `js`, `wsf`, `reg`, `hta`, `pif`, `vb`, `py`, `rb`, `sh`, `cpl`, `msc`), the flow adds a validation entry for `Mask` with message `Source file mask extension is not Allowed: <extension>` and then fails validation as part of `HandleValidations()`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:327-335, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:337-339, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\ExcludedFileExtensions.cs:13-36, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:41
- Legacy: The extension is computed from the substring after the last `.` and compared case-insensitively by lowercasing it.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-066** When invalidating a source location, the flow requires `OperatorId` to be present; otherwise it throws `ArgumentNullException(OperatorId)`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58-63, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:73-79, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:77
- Legacy: The command performs only null/empty checks and throws immediately.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-067** When invalidating a source location, the flow requires `FtsSourceLocationId` to be present; otherwise it throws `ArgumentNullException(FtpSoureceLocationId)`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:58-63, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:65-71, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:69, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:133-140, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:129-136
- Legacy: The invalidate handler does not use the inbound `FtsSourceLocationId`; it looks up the current source location by `ConfigId` and uses the found record's id.
- ⚑ Flag: Callers must supply `FtsSourceLocationId` even though the handler ignores it.
- Decision: keep

From `technical/flows/configure-source-locations-and-destinations/RULES.md`:

**F01-068** When invalidating a source location, the flow requires `ConfigId` to be non-zero; otherwise it throws `ArgumentNullException(ConfigIdNotFound)`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:81-87, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:85, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23
- Legacy: The exception parameter name comes from the constant value `ConfigIdNotFound`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-024** For AddFtpServer and UpdateFtpServer, the command type for update inherits the add command type, so both operations share the same add-command validation rules.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateFtpServerCommand.cs:14-15, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\UpdateFtpServerCommand.cs:14-15
- Legacy: `UpdateFtpServerCommand : AddFtpServerCommand`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-025** AddFtpServer/UpdateFtpServer treat these string properties as required (null or empty values are considered missing): `FtpServerName`, `FtpDescription`, `FtpServerIp`, `FtpTransferMode`, `FtpUser`, `FtpPassword`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:51-53, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:63-65, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:75-77, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:87-89, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:99-101, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:111-113
- Legacy: These properties are annotated with `[Required(AllowEmptyStrings = false)]` and are checked by `ValidateRequiredFields()`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-026** When required-field validation fails, one validation entry is added per failing property with message text `Missing field: <PropertyName>`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92-101, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92-100, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:46-47, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39-40
- Legacy: The code uses `Validator.TryValidateObject(...)` and formats each message as `"Missing field: {memberName}"`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-027** If any validations are present after validation runs, the command fails by throwing `BusinessException` with message `Command Execution Error` and business error code `E`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72-80, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72-80, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:21-22, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39-40, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:20-21, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:33-34
- Legacy: `HandleValidations()` throws a `BusinessException(validations, "Command Execution Error", "E")` when `Validations.Any()` is true.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-028** AddFtpServer/UpdateFtpServer reject an empty GUID FTP server id (`FtpServerId == Guid.Empty`).
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:220-228, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:219-226
- Legacy: `ValidateFtpServerId()` adds a validation when `FtpServerId.Equals(Guid.Empty)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-029** AddFtpServer/UpdateFtpServer validate `FtpServerIp` against the regex pattern `\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231-245, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:229-242, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:14-17, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:13-16
- Legacy: When `FtpServerIp` is non-empty, `ValidateFtpServerIp()` calls `Regex.IsMatch(FtpServerIp, IpAddressPattern)` and adds a validation on mismatch.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-030** If `FtpProxyAddress` is provided (non-empty), AddFtpServer/UpdateFtpServer require `FtpProxyPort` to be present and greater than 0.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:247-255, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:244-251
- Legacy: `ValidateProxyPort()` adds a validation when `!string.IsNullOrEmpty(FtpProxyAddress)` and `(!FtpProxyPort.HasValue || FtpProxyPort <= 0)`.
- Decision: keep

From `technical/flows/manage-ftp-servers-and-credentials/RULES.md`:

**F02-062** AddFtpServer/UpdateFtpServer do not validate that `FtpPort` is greater than 0.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:115-126, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:125-137, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:211-216, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:210-215
- Legacy: `FtpPort` is provided on the command as a `short`, but `SetCommandValidations()` only calls `ValidateFtpServerId()`, `ValidateFtpServerIp()`, and `ValidateProxyPort()`.
- Review: GAP-007
- Decision: keep

From `technical/flows/manage-integration-files/RULES.md`:

**F03-010** An update request must include a configuration id; when it is missing, the update fails with a business validation error (message `Missing ConfigId`, error type `Command Execution Error`, error code `E`).
- Doors: file-transfer-svc-service-update-integration-file, file-transfer-svc-service-update-integration-file-2
- Source: UPDCMD:61-73, COMMON:21-22, COMMON:39-46, EXCONST:17-46
- Legacy: When `ConfigId` is null, the command throws a `BusinessException` with a validation entry for `ConfigId` and the message `Missing ConfigId`.
- Decision: keep


## Target-state requirements it owns (1)

No legacy code does these; the target state asks for them. Specify each in SPEC.md under its id, with the section it comes from, and give each a test in TESTS.md.

- **T-004** Support idempotent command submission for operator retries (for example via an Idempotency-Key) so resubmission does not create duplicates. (target state: 6.4 API Design Standards)
