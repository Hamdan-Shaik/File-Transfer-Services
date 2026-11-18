//--------------------------------------------------------------------------------------------------
// This code is the property of Gainwell Technologies, Copyright (c) 2025. All rights reserved.
// Any unauthorized use in whole or in part without written consent is strictly prohibited.
// Violators may be punished to the full extent of the law.
//--------------------------------------------------------------------------------------------------

namespace HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Common
{
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
}

