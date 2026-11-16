---
type: Target Placement
title: 'Target: shared-contracts-commands'
description: Where the shared-contracts-commands component goes in the new system and what to build, method by method.
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
  body_sha256: 17b198568b7696716ee31e5d32bc9b3d273c3e7fcd932dafd76a0c17e1606bc2
---

# Target: `shared-contracts-commands`

## Where

- Deployable: `fxfer-domain-api` (`services/fxfer-domain-api`)
- Folder: `services/fxfer-domain-api/src/Contracts/Commands`

## What to create

Create the command contract types in `services/fxfer-domain-api/src/Contracts/Commands`.
These types preserve legacy validation behavior so that command handlers and HTTP endpoints can translate failures into the target HTTP error format (Problem Details) without changing rule semantics.

Create the following C# types:

- `services/fxfer-domain-api/src/Contracts/Commands/FileTransferCommand.cs`
  - Type: `public abstract class FileTransferCommand : Command`
  - Properties:
    - `public List<BusinessExceptionMessage> Validations { get; private set; }`
    - `public virtual BusinessErrorUtility ErrorUtility { get; set; }`
  - Methods:
    - `public void Validate()`
      - Set `ErrorUtility = ErrorUtility ?? new BusinessErrorUtility()`.
      - Reset `Validations` to a new empty list.
      - Call `ValidateRequiredFields()`.
      - Call `SetCommandValidations()`.
      - Call `HandleValidations()`.
    - `public abstract void SetCommandValidations()`
    - `public virtual void HandleValidations()`
      - When `Validations.Any()` is true, throw `BusinessException(Validations, CommonConstants.Validation.Messages.CommandError, CommonConstants.Validation.BusinessErrorCode)`.
    - `public void AddValidation(string name, string keyName, string message, bool isMessageAContentId = false)`
      - If `isMessageAContentId` is true, use `ErrorUtility.GetMessage(Requestor, message)`; otherwise use `message`.
      - Add `new BusinessExceptionMessage(name, keyName, errorMessage)` to `Validations`.
    - `private void ValidateRequiredFields()`
      - Run DataAnnotations validation and add one entry per failing required field:
        - `name = <propertyName>`
        - `keyName = Common.CommonConstants.Validation.ErrorType.RequiredFields`
        - `message = Missing field: <propertyName>`.

- `services/fxfer-domain-api/src/Contracts/Commands/AddFtpServerCommand.cs`
  - Type: `public sealed class AddFtpServerCommand : FileTransferCommand`
  - Constructor:
    - Set `FtpServerId = Guid.NewGuid()`.
    - Set `IsValid = true`.
  - Properties: preserve the legacy properties and DataAnnotations attributes (`[Required]` and `[Required(AllowEmptyStrings = false)]`).
  - Override `SetCommandValidations()`:
    - Call `ValidateFtpServerId()`.
    - Call `ValidateFtpServerIp()`.
    - Call `ValidateProxyPort()`.
  - Private methods: `ValidateFtpServerId`, `ValidateFtpServerIp`, `ValidateFtpServerIpPattern`, `ValidateProxyPort` (logic identical to legacy).

- `services/fxfer-domain-api/src/Contracts/Commands/UpdateFtpServerCommand.cs`
  - Type: `public sealed class UpdateFtpServerCommand : AddFtpServerCommand`.

- `services/fxfer-domain-api/src/Contracts/Commands/InvalidateFtpServerCommand.cs`
  - Type: `public sealed class InvalidateFtpServerCommand : UpdateFtpServerCommand`.

- `services/fxfer-domain-api/src/Contracts/Commands/FtsCommand.cs`
  - Type: `public class FtsCommand : Command`
  - Properties:
    - `public List<BusinessExceptionMessage> Validations { get; private set; }`
  - Methods:
    - `public void FtsCommandValidate()` sets `Validations = new List<BusinessExceptionMessage>()`.
    - `public virtual void HandleValidations()` throws `BusinessException(Validations, CommonConstants.Validation.Messages.CommandError, CommonConstants.Validation.BusinessErrorCode)` when any validations exist.
    - `public void AddValidation(string keyName, string message)` adds `new BusinessExceptionMessage(keyName, keyName, message)`.

- `services/fxfer-domain-api/src/Contracts/Commands/FtsSourceLocationAddCommand.cs`
  - Type: `public sealed class FtsSourceLocationAddCommand : FtsCommand`
  - Constructor: initialize `DestinationAddress = new List<FtsDestinationLocationAdded>()`.
  - Properties: preserve legacy DataMember properties.
  - Methods:
    - `public void Validate()` implements the legacy required-field, destination, run-schedule, FTP-server-id, and mask-extension checks and then calls `HandleValidations()`.
    - Keep private helpers: `ValidateFtpServerId`, `ValidateMaskExtension`, `BuildMaskValidationMessage`, `ValidateRequiredField`, `ValidateDestinationAddress`, `ValidateRunSchedule`.

- `services/fxfer-domain-api/src/Contracts/Commands/FtsSourceLocationUpdateCommand.cs`
  - Type: `public sealed class FtsSourceLocationUpdateCommand : FtsCommand`
  - Constant: `public const string FtpServerCode = 1` (value is the string 1).
  - Properties: preserve legacy DataMember properties.
  - Methods:
    - `public void Validate()` implements the legacy required-field, run-schedule, FTP-server-id, source-location-id, suspended-date, config-id, and mask-extension checks and then calls `HandleValidations()`.
    - Keep private helpers: `ValidateFtpServerId`, `ValidateFtpSourceLocationId`, `ValidateFtpSuspendedDate`, `ValidateConfigId`, `ValidateMaskExtension`, `BuildMaskValidationMessage`, `ValidateRequiredField`, `ValidateRunSchedule`.

- `services/fxfer-domain-api/src/Contracts/Commands/FtsSourceLocationInvalidateCommand.cs`
  - Type: `public sealed class FtsSourceLocationInvalidateCommand : Command`
  - Methods:
    - `public void Validate()` calls `ValidateOperatorId`, `ValidateFtpSourceLocationId`, `ValidateConfigId`.
    - Keep private helpers: `ValidateOperatorId`, `ValidateFtpSourceLocationId`, `ValidateConfigId`.

- `services/fxfer-domain-api/src/Contracts/Commands/AddFtsInboundTargetCommand.cs`
  - Type: `public sealed class AddFtsInboundTargetCommand : Command`
  - Properties: preserve legacy DataMember properties.

- `services/fxfer-domain-api/src/Contracts/Commands/InvalidateFtsInboundTargetCommand.cs`
  - Type: `public sealed class InvalidateFtsInboundTargetCommand : AddFtsInboundTargetCommand`.

- `services/fxfer-domain-api/src/Contracts/Commands/AddIntegrationFileCommand.cs`
  - Type: `public sealed class AddIntegrationFileCommand : Command`
  - Properties: preserve legacy DataMember properties.

- `services/fxfer-domain-api/src/Contracts/Commands/UpdateIntegrationFileCommand.cs`
  - Type: `public sealed class UpdateIntegrationFileCommand : AddIntegrationFileCommand`
  - Methods:
    - `public void Validate()` calls `ValidateConfigId()`.
    - `private void ValidateConfigId()` throws a `BusinessException` with one validation entry (key `ConfigId`, message `Missing ConfigId`) when `ConfigId == null`.

Idempotency requirement T-004 is implemented at the command endpoint boundary (HTTP header `Idempotency-Key`), not by adding properties to these DTOs.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.AddFtpServerCommand.SetCommandValidations()` | `AddFtpServerCommand.SetCommandValidations()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.AddFtpServerCommand.ValidateFtpServerId()` | `AddFtpServerCommand.ValidateFtpServerId()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.AddFtpServerCommand.ValidateFtpServerIp()` | `AddFtpServerCommand.ValidateFtpServerIp()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.AddFtpServerCommand.ValidateFtpServerIpPattern()` | `AddFtpServerCommand.ValidateFtpServerIpPattern()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.AddFtpServerCommand.ValidateProxyPort()` | `AddFtpServerCommand.ValidateProxyPort()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FileTransferCommand.AddValidation(string, string, bool)` | `FileTransferCommand.AddValidation(string, string, string, bool)` | Legacy inventory contains alternate signature spellings; keep one method with `message` and optional `isMessageAContentId`. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FileTransferCommand.AddValidation(string, string, string, bool)` | `FileTransferCommand.AddValidation(string, string, string, bool)` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FileTransferCommand.HandleValidations()` | `FileTransferCommand.HandleValidations()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FileTransferCommand.SetCommandValidations()` | `FileTransferCommand.SetCommandValidations()` | Direct parity (abstract). |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FileTransferCommand.Validate()` | `FileTransferCommand.Validate()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FileTransferCommand.ValidateRequiredFields()` | `FileTransferCommand.ValidateRequiredFields()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsCommand.AddValidation(string, string)` | `FtsCommand.AddValidation(string, string)` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsCommand.FtsCommandValidate()` | `FtsCommand.FtsCommandValidate()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsCommand.HandleValidations()` | `FtsCommand.HandleValidations()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationAddCommand.BuildMaskValidationMessage(string)` | `FtsSourceLocationAddCommand.BuildMaskValidationMessage(string)` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationAddCommand.Validate()` | `FtsSourceLocationAddCommand.Validate()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationAddCommand.ValidateDestinationAddress()` | `FtsSourceLocationAddCommand.ValidateDestinationAddress()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationAddCommand.ValidateFtpServerId()` | `FtsSourceLocationAddCommand.ValidateFtpServerId()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationAddCommand.ValidateMaskExtension()` | `FtsSourceLocationAddCommand.ValidateMaskExtension()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationAddCommand.ValidateRequiredField(string, string)` | `FtsSourceLocationAddCommand.ValidateRequiredField(string, string)` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationAddCommand.ValidateRunSchedule()` | `FtsSourceLocationAddCommand.ValidateRunSchedule()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationInvalidateCommand.Validate()` | `FtsSourceLocationInvalidateCommand.Validate()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationInvalidateCommand.ValidateConfigId()` | `FtsSourceLocationInvalidateCommand.ValidateConfigId()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationInvalidateCommand.ValidateFtpSourceLocationId()` | `FtsSourceLocationInvalidateCommand.ValidateFtpSourceLocationId()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationInvalidateCommand.ValidateOperatorId()` | `FtsSourceLocationInvalidateCommand.ValidateOperatorId()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationUpdateCommand.BuildMaskValidationMessage(string)` | `FtsSourceLocationUpdateCommand.BuildMaskValidationMessage(string)` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationUpdateCommand.Validate()` | `FtsSourceLocationUpdateCommand.Validate()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationUpdateCommand.ValidateConfigId()` | `FtsSourceLocationUpdateCommand.ValidateConfigId()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationUpdateCommand.ValidateFtpServerId()` | `FtsSourceLocationUpdateCommand.ValidateFtpServerId()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationUpdateCommand.ValidateFtpSourceLocationId()` | `FtsSourceLocationUpdateCommand.ValidateFtpSourceLocationId()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate()` | `FtsSourceLocationUpdateCommand.ValidateFtpSuspendedDate()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationUpdateCommand.ValidateMaskExtension()` | `FtsSourceLocationUpdateCommand.ValidateMaskExtension()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationUpdateCommand.ValidateRequiredField(string, string)` | `FtsSourceLocationUpdateCommand.ValidateRequiredField(string, string)` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.FtsSourceLocationUpdateCommand.ValidateRunSchedule()` | `FtsSourceLocationUpdateCommand.ValidateRunSchedule()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.UpdateIntegrationFileCommand.Validate()` | `UpdateIntegrationFileCommand.Validate()` | Direct parity. |
| `HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Commands.UpdateIntegrationFileCommand.ValidateConfigId()` | `UpdateIntegrationFileCommand.ValidateConfigId()` | Direct parity. |

## Dependencies

- Component dependencies:
  - `shared-contracts-utilities`
  - `shared-common-common-constants`
  - `shared-contracts-domain`
  - `shared-contracts-events`
  - `shared-contracts-exceptions`
  - `shared-exceptions-file-transfer-exception-constants`
- Package dependency: `hpp.core.bas.cqrs`.

## Enterprise-Architecture standards applied

- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `Command Naming Convention` (command types are imperative and named with a `Command` suffix)
- `standards/application/event-driven/STD-APP-EVT-004-dotnet-cqrs.md` section `1. Validation Behavior` (command validation is deterministic and produces caller-consumable failure details)

