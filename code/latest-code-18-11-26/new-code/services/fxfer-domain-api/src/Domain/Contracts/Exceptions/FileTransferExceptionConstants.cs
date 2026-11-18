using System.Runtime.Serialization;

namespace Fxfer.DomainApi.Contracts.Exceptions;

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
        public const string MissingRequiredSearchCriteria = "MissingRequiredSearchCriteria";
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

