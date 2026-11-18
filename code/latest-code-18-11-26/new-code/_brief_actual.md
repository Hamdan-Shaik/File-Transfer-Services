# Brief: port part 1 of 1 of component `shared-contracts-exceptions`

- Deployable: `fxfer-domain-api` at `services/fxfer-domain-api`; build: `dotnet build services/fxfer-domain-api`; tests: `dotnet test services/fxfer-domain-api`
- The component's folder (architecture.yaml): `services/fxfer-domain-api/src/Contracts/Exceptions`
- The component's spec: `technical/components/shared-contracts-exceptions/` (TARGET.md for names, signatures and places; SPEC.md for what each method does; CONTRACT.md)
- 4 legacy methods or classes, 116 legacy lines
- Playbook: `inputs/playbook/port.md`

## What to port

### `ExcludedFileExtensions`: role **domain**, target file(s) `ExcludedFileExtensions.cs` (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\ExcludedFileExtensions.cs`)

#### `ExcludedFileExtensions` (lines 11-38) Â· rules F01-045, F01-065

```csharp
    public class ExcludedFileExtensions
    {
        public enum ExcludedFileExtensionList
        {
            exe,
            dll,
            bat,
            cmd,
            vbs,
            ps1,
            jar,
            app,
            com,
            scr,
            msi,
            js,
            wsf,
            reg,
            hta,
            pif,
            vb,
            py,
            rb,
            sh,
            cpl,
            msc
        }
    }
```

### `FileTransferExceptionConstants`: role **domain**, target file(s) `FileTransferExceptionConstants.cs` (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs`)

#### `FileTransferExceptionConstants` (lines 13-71) Â· rules F01-044, F01-046, F01-061, F01-064, F01-068, F01-069, F01-070, F01-100, F03-010

```csharp
    public class FileTransferExceptionConstants
    {
        [Serializable]
        [DataContract]
        public class Validation
        {
            public const string Address = "Address";

            public const string ConfigId = "ConfigId";

            public const string ConfigIdNotFound = "ConfigIdNotFound";

            public const string ConfigName = "ConfigName";

            public const string Mask = "Mask";

            public const string Message = "Business Error occurred processing the command.";

            public const string MissingRequiredSearchCriteria = "MissingSearchCriteria";

            public const string OwnerName = "OwnerName";

            public const string OwnerSubsystem = "OwnerSubsystem";

            public const string ProcessingSystem = "FileTransfer";

            public const string FtpSoureceLocationId = "FtpSoureceLocationId";

            public const string FtpServerId = "FtpServerId";

            public const string IsSuspendedChangedDate = "IsSuspendedChangedDate";

            public const string OperatorId = "OperatorId";
        }

        public class Fields
        {
            public const string ConfigId = "ConfigId";

            public const string ConfigName = "ConfigName";

            public const string MissingRequiredSearchCriteria = "MissingSearchCriteria";

            public const string OwnerName = "OwnerName";

            public const string OwnerSubsystem = "OwnerSubsystem";

            public const string SoureceLocationId = "SoureceLocationId";

            public const string FtsSourceLocationSeq = "FTS_SOURCE_LOCATION_SEQ";
        }

        public class FaultExceptions
        {
            public const string CustomFault = "BAS File Transfer Service Application Error Custom Fault ";

            public const string ApplicationError = "BAS File Transfer Service Application Error";
        }
    }
```

### `FileTransferValidationException`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferValidationException.cs`)

#### `FileTransferValidationException.GenerateFault()` (lines 41-44) â†’ `drop: WCF fault generation is not used in target`

```csharp
        public FaultException GenerateFault()
        {
            return new FaultException<FileTransfertBASValidationException>(new FileTransfertBASValidationException(this.FieldName, this.ValidationErrorMessage), new FaultReason(this.ValidationErrorMessage));
        }
```

### `FileTransfertBASValidationException`: role **domain**, target file(s) `FileTransfertBASValidationException.cs` (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransfertBASValidationException.cs`)

#### `FileTransfertBASValidationException` (lines 13-37)

```csharp
    [DataContract]
    public class FileTransfertBASValidationException
    {
        public FileTransfertBASValidationException(string fieldName, string message)
        {
            this.FieldName = fieldName;
            this.ValidationErrorMessage = message;
        }

        public FileTransfertBASValidationException(string message)
            : this(string.Empty, message)
        {
        }

        public FileTransfertBASValidationException()
            : this(string.Empty)
        {
        }

        [DataMember]
        public string FieldName { get; set; }

        [DataMember]
        public string ValidationErrorMessage { get; set; }
    }
```

## The rules these methods implement

**F01-044** When the protocol code is `1` (FTP), the add command requires a non-empty `FtpServerId` and throws `ArgumentNullException("FtpServerId")` when it is missing.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:293-301, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:11-12, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:40-42
- Legacy: Checks `Protocol == "1"` and throws `ArgumentNullException("FtpServerId")` when `FtpServerId` is empty.
- Decision: keep

**F01-045** When the source file mask has an extension in the excluded list (e.g. `exe`, `dll`, `ps1`), the add command records a `Mask` validation with message `Source file mask extension is not Allowed: <extension>`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:304-317, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\ExcludedFileExtensions.cs:13-37, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:40-42
- Legacy: Extracts the substring after the last `.` in `Mask` (lowercased); when it matches an `ExcludedFileExtensionList` value, adds a validation for `Mask` using the excluded-extension message prefix.
- Decision: keep

**F01-046** When handling an add command, the system allocates a new `ConfigId` from the `FTS_SOURCE_LOCATION_SEQ` database sequence and includes it in the `FtsSourceLocationAdded` event.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:48-56, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:165-168, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:61-63
- Legacy: Calls `contextBase.GetNextSequenceValue("FTS_SOURCE_LOCATION_SEQ")`, casts it to `int`, and assigns it to `FtsSourceLocationAdded.ConfigId`.
- Decision: keep

**F01-061** When updating a source location and `Protocol` equals `1`, the flow requires `FtpServerId` to be present; otherwise it throws `ArgumentNullException(FtpServerId)`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:24, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:289-296, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:295, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:41
- Legacy: The check is conditional on the literal protocol code value `1`.
- Decision: keep

**F01-064** When updating a source location, the flow requires `ConfigId` to be non-zero; otherwise it throws `ArgumentNullException(ConfigIdNotFound)`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:319-325, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:323, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23
- Legacy: The exception parameter name comes from the constant value `ConfigIdNotFound`.
- Decision: keep

**F01-065** When updating a source location, if `Mask` ends with one of the excluded extensions (`exe`, `dll`, `bat`, `cmd`, `vbs`, `ps1`, `jar`, `app`, `com`, `scr`, `msi`, `js`, `wsf`, `reg`, `hta`, `pif`, `vb`, `py`, `rb`, `sh`, `cpl`, `msc`), the flow adds a validation entry for `Mask` with message `Source file mask extension is not Allowed: <extension>` and then fails validation as part of `HandleValidations()`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:327-335, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:337-339, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\ExcludedFileExtensions.cs:13-36, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:41
- Legacy: The extension is computed from the substring after the last `.` and compared case-insensitively by lowercasing it.
- Decision: keep

**F01-068** When invalidating a source location, the flow requires `ConfigId` to be non-zero; otherwise it throws `ArgumentNullException(ConfigIdNotFound)`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:81-87, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationInvalidateCommand.cs:85, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23
- Legacy: The exception parameter name comes from the constant value `ConfigIdNotFound`.
- Decision: keep

**F01-069** When handling an update command, if no source location exists for the provided `ConfigId`, the flow throws a `BusinessException` with one entry (`keyName = ConfigId`, `message = ConfigIdNotFound`, `processingSystem = FileTransfer`) and exception message `Business Error occurred processing the command.`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143-163, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139-159, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:29, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:37
- Legacy: Existence is checked by executing a `SourceLocationQuery` filtered only by `ConfigId`.
- Decision: keep

**F01-070** When handling an invalidate command, if no source location exists for the provided `ConfigId`, the flow throws a `BusinessException` with one entry (`keyName = ConfigId`, `message = ConfigIdNotFound`, `processingSystem = FileTransfer`) and exception message `Business Error occurred processing the command.`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:143-163, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Business\CommandHandlers\FtsSourceLocationAddCommandHandler.cs:139-159, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:23, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:29, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:37
- Legacy: Existence is checked by executing a `SourceLocationQuery` filtered only by `ConfigId`.
- Decision: keep

**F01-100** Getting source location details requires a non-empty `SourceLocationId`; when it is missing, the query throws a validation exception with message `FtpSoureceLocationId`.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\GetSourceLocationQuery.cs:30-36, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\FileTransferExceptionConstants.cs:39
- Legacy: `Validate()` throws `new FileTransferValidationException("FtpSoureceLocationId")` when `Where.SourceLocationId` is empty.
- Decision: keep

**F03-010** An update request must include a configuration id; when it is missing, the update fails with a business validation error (message `Missing ConfigId`, error type `Command Execution Error`, error code `E`).
- Doors: file-transfer-svc-service-update-integration-file, file-transfer-svc-service-update-integration-file-2
- Source: UPDCMD:61-73, COMMON:21-22, COMMON:39-46, EXCONST:17-46
- Legacy: When `ConfigId` is null, the command throws a `BusinessException` with a validation entry for `ConfigId` and the message `Missing ConfigId`.
- Decision: keep

