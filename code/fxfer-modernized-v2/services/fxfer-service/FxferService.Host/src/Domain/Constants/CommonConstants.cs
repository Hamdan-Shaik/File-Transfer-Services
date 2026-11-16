namespace FxferService.Domain.Constants;

// F01-023, F02-029, F02-045, F02-067: business error message/code constants.
// F01-026, F01-027, F01-028, F02-030, F02-031, F02-032, F02-034, F02-046, F02-047, F02-051: validation constants.
// F02-033: FTP protocol code constant.
public class CommonConstants
{
    public const string FtpServerCode = "1";
    public const string ModuleName = "FileTransfer";

    // F01-027: shared IP address regex.
    public static class RegexPatterns
    {
        public const string IpAddressPattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
    }

    public static class Validation
    {
        // F01-023, F02-029, F02-045, F02-067.
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
            // F01-023, F02-029, F02-045, F02-067.
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

