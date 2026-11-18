//-----------------------------------------------------------------------------------------
// This code is the property of Gainwell Technologies, Copyright (c) 2021. All rights reserved.
//
// Any unauthorized use in whole or in part without written consent is strictly prohibited.
// Violators may be punished to the full extent of the law.
//-----------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions
{
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
}

