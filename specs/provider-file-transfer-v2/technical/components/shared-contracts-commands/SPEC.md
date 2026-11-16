---
type: Behavior Specification
title: technical/components/shared-contracts-commands/SPEC
description: 'What the shared-contracts-commands component does today: every class and method, and the business rules it implements.'
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
  body_sha256: 89cc71e069ee7d2a5c436e767e7710e51b7fa3593e7224c1af9f5b366ea2928e
---

﻿# shared-contracts-commands: Shared command contract types (validation + base command helpers)

This component contains the legacy command contract classes used by the File Transfer service for write operations (add/update/invalidate) over FTP servers, source locations, inbound targets, and integration file definitions.
These command types are consumed by command endpoints/handlers and include legacy validation behaviors (accumulated validation messages, `BusinessException` failures, and a few `ArgumentNullException(...)` guard clauses).

It owns the business rules listed under **Rules implemented** and exposes no HTTP surface.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.AddFtpServerCommand
Command DTO for adding an FTP server definition.

- Inherits: `FileTransferCommand`.
- Constructor behavior:
  - Sets `FtpServerId = Guid.NewGuid()`.
  - Sets `IsValid = true`.
- Properties (DataMember):
  - `FtpServerId` (Guid)
  - `FtpServerName` (string)
  - `FtpDescription` (string)
  - `FtpServerIp` (string)
  - `FtpTransferMode` (string)
  - `FtpUser` (string)
  - `FtpPassword` (string)
  - `FtpPort` (short)
  - `FtpProxyAddress` (string)
  - `FtpProxyPort` (short?)
  - `UseSSHAuth` (bool)
  - `IsValid` (bool)
  - `LastModifiedDate` (DateTime)
  - `CreationDate` (DateTime)
  - `OperatorId` (string)

### SetCommandValidations()
- Purpose: Run add/update-specific validations after required-field validation has executed.
- Inputs: none.
- Logic:
  - Calls `ValidateFtpServerId()`.
  - Calls `ValidateFtpServerIp()`.
  - Calls `ValidateProxyPort()`.
- Data: none.
- Errors: none (this method adds validations; it does not throw).
- Rules: F02-028, F02-029, F02-030, F02-062.

### ValidateFtpServerId()
- Purpose: Reject `Guid.Empty` as an FTP server id.
- Inputs: `FtpServerId`.
- Logic:
  - When `FtpServerId.Equals(Guid.Empty)`:
    - Calls `AddValidation(FtpServerId, InvalidFtpServerId, Invalid value for: FtpServerId)` (exact key names come from `Common.CommonConstants`).
- Data: none.
- Errors: none.
- Rules: F02-028.

### ValidateFtpServerIp()
- Purpose: When a non-empty server IP is provided, validate it against the legacy IP-address regex.
- Inputs: `FtpServerIp`.
- Logic:
  - When `FtpServerIp` is not null/empty and `ValidateFtpServerIpPattern()` is false:
    - Adds a validation entry for `FtpServerIp` with key `InvalidIpAddressFormat` and message `IpAddressInvalidFormat`.
- Data: none.
- Errors: none.
- Rules: F02-029.

### ValidateFtpServerIpPattern()
- Purpose: Apply the legacy regex pattern check for `FtpServerIp`.
- Inputs: `FtpServerIp`.
- Logic: `Regex.IsMatch(FtpServerIp, Common.CommonConstants.RegexPatterns.IpAddressPattern)`.
- Data: none.
- Errors: Propagates `ArgumentNullException` when `FtpServerIp` is null.
- Rules: F02-029.

### ValidateProxyPort()
- Purpose: When a proxy address is provided, require a proxy port greater than 0.
- Inputs: `FtpProxyAddress`, `FtpProxyPort`.
- Logic:
  - When `FtpProxyAddress` is not null/empty AND (`FtpProxyPort` is null OR `FtpProxyPort <= 0`):
    - Adds a validation entry for `FtpProxyPort` with key `InvalidFtpProxyPort` and message `Invalid value for: FtpProxyPort`.
- Data: none.
- Errors: none.
- Rules: F02-030.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.AddFtsInboundTargetCommand
Command DTO for adding an inbound target record.

- Inherits: `Command`.
- Properties:
  - `FTEMetadataId` (Guid)
  - `ImportJobName` (string)
  - `ContractName` (string)
  - `IsValid` (bool; default value attribute: true)

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.AddIntegrationFileCommand
Command DTO for adding an integration file definition record.

- Inherits: `Command`.
- Properties:
  - `GroupInfo` (List<GroupInfo>)
  - `FileName` (string)
  - `ETFOutboundMetadataDefnId` (Guid)
  - `IsValid` (bool; default value attribute: true)

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FileTransferCommand
Legacy base command class used by FTP server add/update/invalidate commands.

- Inherits: `Command`.
- Constructor behavior:
  - Sets `ErrorUtility = new BusinessErrorUtility()`.
  - Sets `Validations = new List<BusinessExceptionMessage>()`.

Public properties:

- `Validations` (List<BusinessExceptionMessage>; private setter)
- `ErrorUtility` (BusinessErrorUtility; virtual get/set)

### Validate()
- Purpose: Execute command validation in a fixed order: required fields, command-specific validations, then error handling.
- Inputs: The current command instance, including `Requestor` (used only when a validation message is a content id).
- Logic:
  - Ensure `ErrorUtility` is non-null (`ErrorUtility ?? new BusinessErrorUtility()`).
  - Reset `Validations` to a new empty list.
  - Call `ValidateRequiredFields()`.
  - Call `SetCommandValidations()`.
  - Call `HandleValidations()`.
- Data: none.
- Errors:
  - Throws `BusinessException(...)` from `HandleValidations()` when validations exist.
- Rules: F02-025, F02-026, F02-027.

### SetCommandValidations()
- Purpose: Hook for derived commands to add validations beyond required-field validation.
- Inputs: none.
- Logic: abstract; implemented by derived command types.
- Data: none.
- Errors: none.
- Rules: F02-062.

### HandleValidations()
- Purpose: Fail the command when any validations exist.
- Inputs: `Validations`.
- Logic:
  - When `Validations.Any()` is true, throws `new BusinessException(Validations, Command Execution Error, E)`.
- Data: none.
- Errors: throws `BusinessException`.
- Rules: F02-027.

### AddValidation(string, string, bool)
Inventory includes this signature; legacy source implements a single `AddValidation(name, keyName, message, isMessageAContentId=false)` overload.

- Purpose: Add a validation entry, optionally localizing the message from message-code configuration.
- Inputs: `name`, `keyName`, `message`, `isMessageAContentId`.
- Logic:
  - When `isMessageAContentId` is true, compute `errorMessage = ErrorUtility.GetMessage(Requestor, message)`; otherwise use `message` directly.
  - Append `new BusinessExceptionMessage(name, keyName, errorMessage)` to `Validations`.
- Data: none.
- Errors:
  - Propagates any exception from `ErrorUtility.GetMessage(...)`.
- Rules: F02-026.

### AddValidation(string, string, string, bool)
Same underlying legacy behavior as the inventory's alternate signature spelling.

- Purpose/Inputs/Logic/Data/Errors/Rules: same as `AddValidation(string, string, bool)` above.

### ValidateRequiredFields()
- Purpose: Run DataAnnotations required-field validation and add one validation entry per failing required field.
- Inputs: The current command instance and its DataAnnotations attributes.
- Logic:
  - Run `Validator.TryValidateObject(this, new ValidationContext(this), validationResults)`.
  - For each validation result `v`:
    - Call `AddValidation(...)` with:
      - `name = v.MemberNames.First()`
      - `keyName = Common.CommonConstants.Validation.ErrorType.RequiredFields`
      - `message = Missing field: <PropertyName>` (built from `CommonConstants.Validation.Messages.MissingField` and the member name)
- Data: none.
- Errors:
  - Propagates any exceptions from `Validator.TryValidateObject`.
- Rules: F02-025, F02-026.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsCommand
Legacy base command class used by source-location add/update commands.

- Inherits: `Command`.
- Public properties:
  - `Validations` (List<BusinessExceptionMessage>; private setter)

### FtsCommandValidate()
- Purpose: Reset the validations list to an empty list at the start of validation.
- Inputs: none.
- Logic: `Validations = new List<BusinessExceptionMessage>()`.
- Data: none.
- Errors: none.
- Rules: F01-038.

### HandleValidations()
- Purpose: Fail the command when any validations exist.
- Inputs: `Validations`.
- Logic:
  - When `Validations.Any()` is true, throws `new BusinessException(Validations, Command Execution Error, E)`.
- Data: none.
- Errors: throws `BusinessException`.
- Rules: F01-039.

### AddValidation(string keyName, string message)
- Purpose: Append a validation entry where field and key are the same string.
- Inputs: `keyName`, `message`.
- Logic: `Validations.Add(new BusinessExceptionMessage(keyName, keyName, message))`.
- Data: none.
- Errors: throws `NullReferenceException` when `Validations` is null.
- Rules: F01-040.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationAddCommand
Command DTO for adding a source-location configuration.

- Inherits: `FtsCommand`.
- Constructor behavior: initializes `DestinationAddress = new List<FtsDestinationLocationAdded>()`.
- Properties (DataMember):
  - Identifiers: `FtsSourceLocationId` (string), `ConfigId` (int), `FtpServerId` (string)
  - Text fields: `ConfigName`, `Description`, `Address`, `Mask`, `Protocol`, `FileDirection`, `OwnerEmail`, `OwnerName`, `OwnerPhone`, `OwnerModule`, `TradingPartner`, `OperatorId`, `StartTime`, `EndTime` (all string)
  - Schedule flags: `RunOnMonday`, `RunOnTuesday`, `RunOnWed`, `RunOnThrus`, `RunOnFri`, `RunOnSat`, `RunOnSun`, `RunOnFrstDayOfMonthInd`, `RunOnLastDayOfMonthInd` (bool) and `RunOnThisDayOfMonth` (byte)
  - Status flags: `IsValid` (bool), `IsSuspended` (bool), `IsSuspendedChangedDate` (DateTime?), `SendNotifications` (bool)
  - Timestamps: `LastModifiedDate` (DateTime), `CreationDate` (DateTime)
  - Destinations: `DestinationAddress` (List<FtsDestinationLocationAdded>)

### Validate()
- Purpose: Validate a new source-location configuration and throw a `BusinessException` when any validation entries exist.
- Inputs: The command properties.
- Logic:
  - Call `FtsCommandValidate()`.
  - For each required string field, call `ValidateRequiredField(fieldName, value)`:
    - `ConfigName`, `Protocol`, `Address`, `OwnerName`, `OwnerModule`, `OwnerPhone`, `OwnerEmail`, `StartTime`, `EndTime`.
  - Call `ValidateDestinationAddress()`.
  - Call `ValidateRunSchedule()`.
  - Call `ValidateFtpServerId()`.
  - Call `ValidateMaskExtension()`.
  - Call `HandleValidations()`.
- Data: none.
- Errors:
  - Throws `ArgumentNullException(FtpServerId)` when protocol code is 1 (FTP) and `FtpServerId` is empty.
  - Throws `BusinessException(...)` when validation entries exist.
- Rules: F01-038, F01-039, F01-041, F01-042, F01-043, F01-044, F01-045.

### ValidateDestinationAddress()
- Purpose: Require at least one destination entry marked valid.
- Inputs: `DestinationAddress`.
- Logic:
  - When `DestinationAddress` is null OR empty OR the count of entries where `IsValid == true` is 0:
    - Add a validation with key `DestinationAddress` and message `Missing field`.
- Data: none.
- Errors: none.
- Rules: F01-042.

### ValidateFtpServerId()
- Purpose: For FTP protocol, require a non-empty server id.
- Inputs: `Protocol`, `FtpServerId`.
- Logic:
  - When `Protocol == CommonConstants.FtpServerCode` (FTP server code is 1):
    - If `FtpServerId` is null/empty, throw `ArgumentNullException(FileTransferExceptionConstants.Validation.FtpServerId)`.
- Data: none.
- Errors: throws `ArgumentNullException`.
- Rules: F01-044.

### ValidateMaskExtension()
- Purpose: Reject unsafe file-mask extensions.
- Inputs: `Mask`.
- Logic:
  - Compute `extension` as the substring after the last `.` in `Mask`, lowercased; when no dot or mask is empty, use `string.Empty`.
  - When `Enum.IsDefined(typeof(ExcludedFileExtensions.ExcludedFileExtensionList), extension)` is true:
    - Add a validation with key `Mask` and message from `BuildMaskValidationMessage(extension)`.
- Data: none.
- Errors: none.
- Rules: F01-045.

### BuildMaskValidationMessage(string extension)
- Purpose: Create the validation message text for an excluded extension.
- Inputs: `extension`.
- Logic: concatenates `ExcludedExtension` + a space + `extension`.
- Data: none.
- Errors: none.
- Rules: F01-045.

### ValidateRequiredField(string fieldName, string value)
- Purpose: Add a missing-field validation when a required string field is empty.
- Inputs: `fieldName`, `value`.
- Logic:
  - When `string.IsNullOrEmpty(value)` is true:
    - Add a validation with key `fieldName` and message `Missing field`.
- Data: none.
- Errors: none.
- Rules: F01-041.

### ValidateRunSchedule()
- Purpose: Require at least one run-schedule indicator to be selected.
- Inputs: `RunOnThisDayOfMonth` and the day/month flags.
- Logic:
  - When `RunOnThisDayOfMonth == 0` AND all day/month flags are false:
    - Add a validation with key `Run Schedule` and message `Invalid value for`.
- Data: none.
- Errors: none.
- Rules: F01-043.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationInvalidateCommand
Command DTO for invalidating (deactivating) a source-location configuration.

- Inherits: `Command`.
- Properties:
  - `FtsSourceLocationId` (string)
  - `ConfigId` (int)
  - `OperatorId` (string)
  - `DestinationAddress` (List<FtsDestinationLocationAdded>)

### Validate()
- Purpose: Validate required identifiers for invalidation.
- Inputs: `OperatorId`, `FtsSourceLocationId`, `ConfigId`.
- Logic:
  - Call `ValidateOperatorId()`.
  - Call `ValidateFtpSourceLocationId()`.
  - Call `ValidateConfigId()`.
- Data: none.
- Errors: throws `ArgumentNullException(...)` for missing required values.
- Rules: F01-066, F01-067, F01-068.

### ValidateConfigId()
- Purpose: Require a non-zero configuration id.
- Inputs: `ConfigId`.
- Logic: when `ConfigId == 0`, throw `ArgumentNullException(ConfigIdNotFound)`.
- Data: none.
- Errors: throws `ArgumentNullException`.
- Rules: F01-068.

### ValidateFtpSourceLocationId()
- Purpose: Require a non-empty source-location id.
- Inputs: `FtsSourceLocationId`.
- Logic: when null/empty, throw `ArgumentNullException(FtpSoureceLocationId)`.
- Data: none.
- Errors: throws `ArgumentNullException`.
- Rules: F01-067.

### ValidateOperatorId()
- Purpose: Require a non-empty operator id.
- Inputs: `OperatorId`.
- Logic: when null/empty, throw `ArgumentNullException(OperatorId)`.
- Data: none.
- Errors: throws `ArgumentNullException`.
- Rules: F01-066.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationUpdateCommand
Command DTO for updating a source-location configuration.

- Inherits: `FtsCommand`.
- Constant: `FtpServerCode = 1` (value is the string 1).
- Properties: same shape as `FtsSourceLocationAddCommand`.

### Validate()
- Purpose: Validate an updated source-location configuration and throw a `BusinessException` when any validation entries exist.
- Inputs: The command properties.
- Logic:
  - Call `FtsCommandValidate()`.
  - For each required string field, call `ValidateRequiredField(fieldName, value)`:
    - `ConfigName`, `Protocol`, `Address`, `OwnerName`, `OwnerModule`, `OwnerPhone`, `OwnerEmail`, `StartTime`, `EndTime`.
  - Call `ValidateRunSchedule()`.
  - Call `ValidateFtpServerId()`.
  - Call `ValidateFtpSourceLocationId()`.
  - Call `ValidateFtpSuspendedDate()`.
  - Call `ValidateConfigId()`.
  - Call `ValidateMaskExtension()`.
  - Call `HandleValidations()`.
- Data: none.
- Errors:
  - Throws `ArgumentNullException(...)` for missing required ids/dates.
  - Throws `BusinessException(...)` when validation entries exist.
- Rules: F01-038, F01-039, F01-059, F01-060, F01-061, F01-062, F01-063, F01-064, F01-065.

### ValidateFtpServerId()
- Purpose: For FTP protocol, require a non-empty server id.
- Inputs: `Protocol`, `FtpServerId`.
- Logic:
  - When `Protocol == FtpServerCode` and `FtpServerId` is empty, throw `ArgumentNullException(FtpServerId)`.
- Data: none.
- Errors: throws `ArgumentNullException`.
- Rules: F01-061.

### ValidateFtpSourceLocationId()
- Purpose: Require a non-empty source-location id.
- Inputs: `FtsSourceLocationId`.
- Logic: when null/empty, throw `ArgumentNullException(FtpSoureceLocationId)`.
- Data: none.
- Errors: throws `ArgumentNullException`.
- Rules: F01-062.

### ValidateFtpSuspendedDate()
- Purpose: When a configuration is suspended, require a suspension change date.
- Inputs: `IsSuspended`, `IsSuspendedChangedDate`.
- Logic:
  - When `IsSuspended == true` and `IsSuspendedChangedDate` is null, throw `ArgumentNullException(IsSuspendedChangedDate)`.
- Data: none.
- Errors: throws `ArgumentNullException`.
- Rules: F01-063.

### ValidateConfigId()
- Purpose: Require a non-zero configuration id.
- Inputs: `ConfigId`.
- Logic: when `ConfigId == 0`, throw `ArgumentNullException(ConfigIdNotFound)`.
- Data: none.
- Errors: throws `ArgumentNullException`.
- Rules: F01-064.

### ValidateMaskExtension()
- Purpose/Inputs/Logic/Data/Errors/Rules: same as `FtsSourceLocationAddCommand.ValidateMaskExtension`.

### BuildMaskValidationMessage(string extension)
- Purpose/Inputs/Logic/Data/Errors/Rules: same as `FtsSourceLocationAddCommand.BuildMaskValidationMessage`.

### ValidateRequiredField(string fieldName, string value)
- Purpose/Inputs/Logic/Data/Errors/Rules: same as `FtsSourceLocationAddCommand.ValidateRequiredField`.

### ValidateRunSchedule()
- Purpose/Inputs/Logic/Data/Errors/Rules: same as `FtsSourceLocationAddCommand.ValidateRunSchedule`.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.InvalidateFtpServerCommand
Marker command type for invalidating an FTP server.

- Inherits: `UpdateFtpServerCommand`.
- Behavior: inherits all validations and properties from `AddFtpServerCommand` via `UpdateFtpServerCommand`.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.InvalidateFtsInboundTargetCommand
Marker command type for invalidating an inbound target.

- Inherits: `AddFtsInboundTargetCommand`.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.UpdateFtpServerCommand
Marker command type for updating an FTP server.

- Inherits: `AddFtpServerCommand`.

## HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.UpdateIntegrationFileCommand
Command DTO for updating an integration file definition record.

- Inherits: `AddIntegrationFileCommand`.
- Properties:
  - `FtsIntegrationFileId` (Guid)
  - `ConfigId` (int?)

### Validate()
- Purpose: Enforce the integration-file update requirement that a configuration id must be present.
- Inputs: `ConfigId`.
- Logic: Calls `ValidateConfigId()`.
- Data: none.
- Errors: Throws `BusinessException(...)` when `ConfigId` is null.
- Rules: F03-010.

## Rules implemented

| Rule | In short | Where | Flow |
| --- | --- | --- | --- |
| F01-038 | Each add command validation run starts with an empty validations list. | FtsCommand.FtsCommandValidate | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-039 | When any validations exist, the command throws a `BusinessException` (message `Command Execution Error`, code `E`). | FtsCommand.HandleValidations | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-040 | Each validation added uses the same value for key and field. | FtsCommand.AddValidation | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-041 | Add source-location records `Missing field` for each required string field that is empty. | FtsSourceLocationAddCommand.ValidateRequiredField | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-042 | Add source-location requires at least one destination marked valid; else records `Missing field` for `DestinationAddress`. | FtsSourceLocationAddCommand.ValidateDestinationAddress | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-043 | Add source-location requires a run schedule selection; else adds validation key `Run Schedule` with message `Invalid value for`. | FtsSourceLocationAddCommand.ValidateRunSchedule | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-044 | When protocol code is 1 (FTP), add source-location requires `FtpServerId`; else throws `ArgumentNullException(FtpServerId)`. | FtsSourceLocationAddCommand.ValidateFtpServerId | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-045 | Add source-location rejects excluded file-mask extensions and fails validation. | FtsSourceLocationAddCommand.ValidateMaskExtension | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-059 | Update source-location treats specific string fields as required and fails with a `BusinessException` when any are missing. | FtsSourceLocationUpdateCommand.Validate | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-060 | Update source-location requires a run schedule selection; else adds validation key `Run Schedule` with message `Invalid value for`. | FtsSourceLocationUpdateCommand.ValidateRunSchedule | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-061 | When protocol code is 1 (FTP), update source-location requires `FtpServerId`; else throws `ArgumentNullException(FtpServerId)`. | FtsSourceLocationUpdateCommand.ValidateFtpServerId | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-062 | Update source-location requires `FtsSourceLocationId`; else throws `ArgumentNullException(FtpSoureceLocationId)`. | FtsSourceLocationUpdateCommand.ValidateFtpSourceLocationId | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-063 | When updating and `IsSuspended` is true, update source-location requires `IsSuspendedChangedDate`; else throws `ArgumentNullException(IsSuspendedChangedDate)`. | FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-064 | Update source-location requires non-zero `ConfigId`; else throws `ArgumentNullException(ConfigIdNotFound)`. | FtsSourceLocationUpdateCommand.ValidateConfigId | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-065 | Update source-location rejects excluded file-mask extensions and fails validation. | FtsSourceLocationUpdateCommand.ValidateMaskExtension | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-066 | Invalidate source-location requires `OperatorId`; else throws `ArgumentNullException(OperatorId)`. | FtsSourceLocationInvalidateCommand.ValidateOperatorId | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-067 | Invalidate source-location requires `FtsSourceLocationId`; else throws `ArgumentNullException(FtpSoureceLocationId)`. | FtsSourceLocationInvalidateCommand.ValidateFtpSourceLocationId | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F01-068 | Invalidate source-location requires non-zero `ConfigId`; else throws `ArgumentNullException(ConfigIdNotFound)`. | FtsSourceLocationInvalidateCommand.ValidateConfigId | [configure-source-locations-and-destinations](/technical/flows/configure-source-locations-and-destinations/RULES.md) |
| F02-024 | Update FTP server shares add-command validation rules because it inherits the add command. | UpdateFtpServerCommand (inherits AddFtpServerCommand) | [manage-ftp-servers-and-credentials](/technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-025 | Add/update FTP server treat specific string properties as required. | FileTransferCommand.ValidateRequiredFields | [manage-ftp-servers-and-credentials](/technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-026 | Required-field validation adds one entry per failing property with message text `Missing field: <PropertyName>`. | FileTransferCommand.ValidateRequiredFields | [manage-ftp-servers-and-credentials](/technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-027 | When any validations exist after validation runs, the command fails by throwing `BusinessException` (message `Command Execution Error`, code `E`). | FileTransferCommand.HandleValidations | [manage-ftp-servers-and-credentials](/technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-028 | Add/update FTP server reject an empty GUID FTP server id. | AddFtpServerCommand.ValidateFtpServerId | [manage-ftp-servers-and-credentials](/technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-029 | Add/update FTP server validate `FtpServerIp` against the legacy regex pattern. | AddFtpServerCommand.ValidateFtpServerIp | [manage-ftp-servers-and-credentials](/technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-030 | If `FtpProxyAddress` is provided, add/update FTP server require `FtpProxyPort` to be present and greater than 0. | AddFtpServerCommand.ValidateProxyPort | [manage-ftp-servers-and-credentials](/technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F02-062 | Add/update FTP server do not validate that `FtpPort` is greater than 0. | AddFtpServerCommand.SetCommandValidations | [manage-ftp-servers-and-credentials](/technical/flows/manage-ftp-servers-and-credentials/RULES.md) |
| F03-010 | Update integration file requires `ConfigId`; missing value throws business validation error (message `Missing ConfigId`, error type `Command Execution Error`, error code `E`). | UpdateIntegrationFileCommand.ValidateConfigId | [manage-integration-files](/technical/flows/manage-integration-files/RULES.md) |

## Target-state requirements

### T-004
- Purpose: Support idempotent command submission for operator retries so resubmission does not create duplicates.
- Target-state source: `inputs/target-state.md` section `6.4 API Design Standards`.
- Applies to: HTTP command endpoints that accept these command contracts (add/update/invalidate operations).
- Inputs: `Idempotency-Key` request header (caller-supplied opaque key).
- Logic:
  - When an `Idempotency-Key` is present, the domain API treats repeated submissions with the same `(tenant_id, command kind, idempotency key)` as retries.
  - A retry returns the same logical result as the first submission (no duplicate state changes).
- Data:
  - Stores idempotency keys and the corresponding command result identity (at minimum: accepted/created identifiers and status) for a bounded retention window.
- Errors:
  - If an `Idempotency-Key` is re-used with a different command payload, the API rejects the request as a client error.

## Not carried over

None.

## Enterprise-Architecture standards applied

- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Command and Query Base Types` (command types are explicit requests; validation is treated as a first-class concern of command execution).
- `standards/application/api/STD-APP-API-001-api-design.md` section `3.5 Error Handling` (command validation failures are surfaced consistently and avoid leaking internal exception details).

### ValidateConfigId()
- Purpose: Fail update when `ConfigId` is missing.
- Inputs: `ConfigId`.
- Logic:
  - Allocate a local `validations` list.
  - When `ConfigId == null`:
    - Add `new BusinessExceptionMessage(ConfigId, ConfigId, Missing ConfigId)`.
    - Throw `new BusinessException(validations, Command Execution Error, E)`.
- Data: none.
- Errors: throws `BusinessException`.
- Rules: F03-010.


