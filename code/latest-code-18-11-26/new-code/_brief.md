# Brief: port part 1 of 1 of component `shared-common-common-constants`

- Deployable: `fxfer-domain-api` at `services/fxfer-domain-api`; build: `dotnet build services/fxfer-domain-api`; tests: `dotnet test services/fxfer-domain-api`
- The component's folder (architecture.yaml): `services/fxfer-domain-api/src/Domain/Constants`
- The component's spec: `technical/components/shared-common-common-constants/` (TARGET.md for names, signatures and places; SPEC.md for what each method does; CONTRACT.md)
- 5 legacy methods or classes, 128 legacy lines
- Playbook: `inputs/playbook/port.md`

## What to port

### `CommonConstants`: role **domain**, target file(s) `CommonConstants.cs` (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs`)

#### `CommonConstants` (lines 9-71) Â· rules F01-039, F01-041, F01-042, F01-043, F01-044, F01-045, F01-059, F01-060, F01-065, F02-026, F02-027, F02-029, F03-010

```csharp
    public class CommonConstants
    {
        public const string FtpServerCode = "1";
        public const string ModuleName = "FileTransfer";

        public static class RegexPatterns
        {
            public const string IpAddressPattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
        }

        public static class Validation
        {
            public const string BusinessErrorCode = "E";
            public const string ProcessingSystem = "FileTransfer";

            public static class ErrorType
            {
                public const string DuplicatedFtpServerName = "DuplicatedFtpServerName";
                public const string InvalidCommand = "Invalid Command";
                public const string InvalidFtpProxyPort = "InvalidFtpProxyPort";
                public const string InvalidFtpServerId = "InvalidFtpServerId";
                public const string InvalidParameters = "InvalidSearchParameters";
                public const string InvalidIpAddressFormat = "InvalidIpAddressFormat";
                public const string NuGetPackage = "NuGet package";
                public const string QueryExecutionError = "QueryExecutionError";
                public const string RequiredFields = "RequiredFields";
            }

            public static class Messages
            {
                public const string CommandError = "Command Execution Error";
                public const string DuplicatedFtpServerName = "Duplicated FTP Server Name";
                public const string ExcludedExtension = "Source file mask extension is not Allowed:";
                public const string FTRepositoryGetResultsError = "File Transfer Repository error when getting results for";
                public const string IpAddressInvalidFormat = "Invalid Format for IP Address";
                public const string InvalidValue = "Invalid value for";
                public const string MissingConfigId = "Missing ConfigId";
                public const string MissingField = "Missing field";
            }

            public static class Properties
            {
                public const string FtpProxyPort = "FtpProxyPort";
                public const string FtpServerId = "FtpServerId";
                public const string FtpServerIp = "FtpServerIp";
                public const string RunSchedule = "Run Schedule";
            }
        }

        public static class SortFields
        {
            public static class FtpServer
            {
                public const string CreationDate = "CreationDate";
                public const string FtpDescription = "FtpDescription";
                public const string FtpServerId = "FtpServerId";
                public const string FtpServerIp = "FtpServerIp";
                public const string FtpServerName = "FtpServerName";
                public const string FtpUser = "FtpUser";
                public const string IsValid = "IsValid";    
            }
        }
    }
```

### `RegexPatterns`: role **domain**, target file(s) `RegexPatterns.cs` (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs`)

#### `RegexPatterns` (lines 14-17) Â· rules F02-029

```csharp
        public static class RegexPatterns
        {
            public const string IpAddressPattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
        }
```

### `SortFields`: role **domain**, target file(s) `SortFields.cs` (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs`)

#### `SortFields` (lines 58-70)

```csharp
        public static class SortFields
        {
            public static class FtpServer
            {
                public const string CreationDate = "CreationDate";
                public const string FtpDescription = "FtpDescription";
                public const string FtpServerId = "FtpServerId";
                public const string FtpServerIp = "FtpServerIp";
                public const string FtpServerName = "FtpServerName";
                public const string FtpUser = "FtpUser";
                public const string IsValid = "IsValid";    
            }
        }
```

### `FtpServer`: role **domain**, target file(s) `FtpServer.cs` (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs`)

#### `FtpServer` (lines 60-69)

```csharp
            public static class FtpServer
            {
                public const string CreationDate = "CreationDate";
                public const string FtpDescription = "FtpDescription";
                public const string FtpServerId = "FtpServerId";
                public const string FtpServerIp = "FtpServerIp";
                public const string FtpServerName = "FtpServerName";
                public const string FtpUser = "FtpUser";
                public const string IsValid = "IsValid";    
            }
```

### `Validation`: role **domain**, target file(s) `Validation.cs` (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs`)

#### `Validation` (lines 19-56) Â· rules F01-039, F01-041, F01-042, F01-043, F01-045, F01-059, F01-060, F01-065, F02-026, F02-027, F03-010

```csharp
        public static class Validation
        {
            public const string BusinessErrorCode = "E";
            public const string ProcessingSystem = "FileTransfer";

            public static class ErrorType
            {
                public const string DuplicatedFtpServerName = "DuplicatedFtpServerName";
                public const string InvalidCommand = "Invalid Command";
                public const string InvalidFtpProxyPort = "InvalidFtpProxyPort";
                public const string InvalidFtpServerId = "InvalidFtpServerId";
                public const string InvalidParameters = "InvalidSearchParameters";
                public const string InvalidIpAddressFormat = "InvalidIpAddressFormat";
                public const string NuGetPackage = "NuGet package";
                public const string QueryExecutionError = "QueryExecutionError";
                public const string RequiredFields = "RequiredFields";
            }

            public static class Messages
            {
                public const string CommandError = "Command Execution Error";
                public const string DuplicatedFtpServerName = "Duplicated FTP Server Name";
                public const string ExcludedExtension = "Source file mask extension is not Allowed:";
                public const string FTRepositoryGetResultsError = "File Transfer Repository error when getting results for";
                public const string IpAddressInvalidFormat = "Invalid Format for IP Address";
                public const string InvalidValue = "Invalid value for";
                public const string MissingConfigId = "Missing ConfigId";
                public const string MissingField = "Missing field";
            }

            public static class Properties
            {
                public const string FtpProxyPort = "FtpProxyPort";
                public const string FtpServerId = "FtpServerId";
                public const string FtpServerIp = "FtpServerIp";
                public const string RunSchedule = "Run Schedule";
            }
        }
```

## The rules these methods implement

**F01-039** When any validations have been added, the command throws a `BusinessException` with error code `E` and message `Command Execution Error`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:33-41, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:21-47
- Legacy: `HandleValidations()` throws `BusinessException(validations, "Command Execution Error", "E")` when `Validations.Any()`.
- Decision: keep

**F01-041** The add command records a `Missing field` validation for each required string field that is null or empty.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:272-288, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:319-324, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:45-47
- Legacy: Validates `ConfigName`, `Protocol`, `Address`, `OwnerName`, `OwnerModule`, `OwnerPhone`, `OwnerEmail`, `StartTime`, and `EndTime`; when a value is empty, adds a validation with message `Missing field`.
- Decision: keep

**F01-042** The add command requires at least one destination address marked valid; otherwise it records `Missing field` for `DestinationAddress`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:327-333, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:45-47
- Legacy: Adds a `DestinationAddress` validation when the list is null, empty, or when the count of entries where `IsValid == true` is zero.
- Decision: keep

**F01-043** The add command requires at least one run-schedule option to be selected; otherwise it records a `Run Schedule` validation with message `Invalid value for`.
- Doors: file-transfer-svc-service-file-transfer-add
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationAddCommand.cs:335-349, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:44-55
- Legacy: When `RunOnThisDayOfMonth == 0` and all day-of-week flags and month-edge flags are false, adds a validation with key `Run Schedule` and message `Invalid value for`.
- Decision: keep

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

**F01-059** When updating a source location, the flow treats `ConfigName`, `Protocol`, `Address`, `OwnerName`, `OwnerModule`, `OwnerPhone`, `OwnerEmail`, `StartTime`, and `EndTime` as required string fields; each missing/empty value adds a validation entry with message `Missing field`, and if any validation entries exist the flow throws a `BusinessException` with message `Command Execution Error` and error code `E`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:268-286, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:271-279, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:342-348, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:28-41, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsCommand.cs:44-46, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:21, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:46
- Legacy: These fields can all be non-empty but still invalid for later processing (for example, a non-GUID `OwnerModule` can still fail later during GUID parsing).
- Decision: keep

**F01-060** When updating a source location, if no run-schedule indicators are selected (`RunOnThisDayOfMonth == 0` and all daily/monthly flags are false), the flow adds a validation entry with key `Run Schedule` and message `Invalid value for` and then fails validation as part of the command's `HandleValidations()`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:350-364, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:44, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:54
- Legacy: The message stored for this error is the generic prefix `Invalid value for` (it does not include the property name).
- Decision: keep

**F01-065** When updating a source location, if `Mask` ends with one of the excluded extensions (`exe`, `dll`, `bat`, `cmd`, `vbs`, `ps1`, `jar`, `app`, `com`, `scr`, `msi`, `js`, `wsf`, `reg`, `hta`, `pif`, `vb`, `py`, `rb`, `sh`, `cpl`, `msc`), the flow adds a validation entry for `Mask` with message `Source file mask extension is not Allowed: <extension>` and then fails validation as part of `HandleValidations()`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:327-335, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FtsSourceLocationUpdateCommand.cs:337-339, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Exceptions\ExcludedFileExtensions.cs:13-36, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:41
- Legacy: The extension is computed from the substring after the last `.` and compared case-insensitively by lowercasing it.
- Decision: keep

**F02-026** When required-field validation fails, one validation entry is added per failing property with message text `Missing field: <PropertyName>`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92-101, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:92-100, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:46-47, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39-40
- Legacy: The code uses `Validator.TryValidateObject(...)` and formats each message as `"Missing field: {memberName}"`.
- Decision: keep

**F02-027** If any validations are present after validation runs, the command fails by throwing `BusinessException` with message `Command Execution Error` and business error code `E`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72-80, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\FileTransferCommand.cs:72-80, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:21-22, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:39-40, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:20-21, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:33-34
- Legacy: `HandleValidations()` throws a `BusinessException(validations, "Command Execution Error", "E")` when `Validations.Any()` is true.
- Decision: keep

**F02-029** AddFtpServer/UpdateFtpServer validate `FtpServerIp` against the regex pattern `\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b`.
- Doors: file-transfer-svc-service-controller-add-ftp-server, file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:231-245, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Commands\AddFtpServerCommand.cs:229-242, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:14-17, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Common\CommonConstants.cs:13-16
- Legacy: When `FtpServerIp` is non-empty, `ValidateFtpServerIp()` calls `Regex.IsMatch(FtpServerIp, IpAddressPattern)` and adds a validation on mismatch.
- Decision: keep

**F03-010** An update request must include a configuration id; when it is missing, the update fails with a business validation error (message `Missing ConfigId`, error type `Command Execution Error`, error code `E`).
- Doors: file-transfer-svc-service-update-integration-file, file-transfer-svc-service-update-integration-file-2
- Source: UPDCMD:61-73, COMMON:21-22, COMMON:39-46, EXCONST:17-46
- Legacy: When `ConfigId` is null, the command throws a `BusinessException` with a validation entry for `ConfigId` and the message `Missing ConfigId`.
- Decision: keep

