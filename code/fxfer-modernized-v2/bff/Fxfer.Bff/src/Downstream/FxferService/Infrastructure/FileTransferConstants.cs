namespace Fxfer.Bff.Downstream.FxferService;

internal static class FileTransferConstants
{
    public const string ModuleName = "FileTransfer";

    public static class ApplicationSettings
    {
        public const string ExternalFTECommonFolder = "ExternalFTECommonFolder";
    }

    public static class SearchFields
    {
        public const string FtpServerId = "FtpServerId";
    }

    public static class Miscellaneous
    {
        public const string FileToEvent = "FileToEvent";
    }

    public static class BusinessValidationExceptions
    {
        public const string DuplicatedFTPServerName = "DuplicatedFtpServerName";
        public const string InvalidFTPProxyPort = "InvalidFtpProxyPort";
        public const string InvalidIpAddressFormat = "InvalidIpAddressFormat";
    }

    public static class Localization
    {
        public static class Messages
        {
            public const string DuplicatedFTPServerName = "FileTransfer.Msg.DuplicatedFTPServerName";
            public const string InvalidFTPProxyPort = "FileTransfer.Msg.InvalidFTPProxyPort";
            public const string InvalidIpServerFormat = "FileTransfer.Msg.InvalidIpServerFormat";
        }
    }
}