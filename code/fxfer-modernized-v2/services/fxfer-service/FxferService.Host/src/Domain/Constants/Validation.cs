namespace FxferService.Domain.Constants;

// F01-023, F01-026, F01-027, F01-028, F02-029, F02-030, F02-031, F02-032, F02-034, F02-045, F02-046, F02-047, F02-051, F02-067.
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

