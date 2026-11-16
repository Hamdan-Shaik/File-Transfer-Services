using System.Runtime.Serialization;

namespace FxferService.Domain.Exceptions;

// F02-012, F02-033, F02-048, F02-049, F02-050, F02-052.
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

