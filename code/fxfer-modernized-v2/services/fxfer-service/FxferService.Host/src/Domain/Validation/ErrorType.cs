namespace FxferService.Domain.Validation;

// F01-023, F01-026, F01-027, F01-028.
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

