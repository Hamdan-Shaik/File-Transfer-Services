namespace Fxfer.DomainApi.Domain.Validation;

public static class ValidationConstants
{
    // F01-039
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

    // F01-039, F01-041, F01-042, F01-043, F01-045, F01-059, F01-060, F01-065, F02-026, F02-027, F03-010
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

    // F01-043, F01-060
    public static class Properties
    {
        public const string FtpProxyPort = "FtpProxyPort";
        public const string FtpServerId = "FtpServerId";
        public const string FtpServerIp = "FtpServerIp";
        public const string RunSchedule = "Run Schedule";
    }
}

