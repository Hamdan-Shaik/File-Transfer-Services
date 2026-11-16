using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Exceptions;

public class FileTransferExceptionConstants
{
    [Serializable]
    [DataContract]
    public class Validation
    {
        // F02-035: used by command handler validation failures.
        public const string Address = "Address";

        // F02-049: used when ConfigId is missing.
        public const string ConfigId = "ConfigId";

        // F02-049, F02-052: used when ConfigId is missing or not found.
        public const string ConfigIdNotFound = "ConfigIdNotFound";

        public const string ConfigName = "ConfigName";

        // F02-034, F02-051: used when adding a mask validation.
        public const string Mask = "Mask";

        public const string Message = "Business Error occurred processing the command.";

        public const string MissingRequiredSearchCriteria = "MissingSearchCriteria";

        public const string OwnerName = "OwnerName";

        public const string OwnerSubsystem = "OwnerSubsystem";

        public const string ProcessingSystem = "FileTransfer";

        // F02-012, F02-049, F02-052: used as validation/fault key for source location id.
        public const string FtpSoureceLocationId = "FtpSoureceLocationId";

        // F02-033, F02-048: used as parameter name when FTP protocol requires FtpServerId.
        public const string FtpServerId = "FtpServerId";

        // F02-050: used as parameter name when IsSuspendedChangedDate is required.
        public const string IsSuspendedChangedDate = "IsSuspendedChangedDate";

        // F02-052: used as parameter name when OperatorId is required.
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

        // F02-035: database sequence used to allocate a new ConfigId.
        public const string FtsSourceLocationSeq = "FTS_SOURCE_LOCATION_SEQ";
    }

    public class FaultExceptions
    {
        public const string CustomFault = "BAS File Transfer Service Application Error Custom Fault ";

        public const string ApplicationError = "BAS File Transfer Service Application Error";
    }
}

