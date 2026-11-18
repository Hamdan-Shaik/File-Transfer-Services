using System.Runtime.Serialization;
using Fxfer.SoapFacade.CoreWcf.Contracts.Domain;
using Fxfer.SoapFacade.CoreWcf.Contracts.Utilities;

namespace Fxfer.SoapFacade.CoreWcf.Contracts.Commands;

[DataContract]
public class FtsSourceLocationAddCommand : Command
{
    [DataMember(Order = 1)] public string? Address { get; set; }
    [DataMember(Order = 2)] public int ConfigId { get; set; }
    [DataMember(Order = 3)] public string? ConfigName { get; set; }
    [DataMember(Order = 4)] public DateTime CreationDate { get; set; }
    [DataMember(Order = 5)] public string? Description { get; set; }
    [DataMember(Order = 6)] public List<FtsDestinationLocationAdded> DestinationAddress { get; set; } = [];
    [DataMember(Order = 7)] public string? EndTime { get; set; }
    [DataMember(Order = 8)] public string? FileDirection { get; set; }
    [DataMember(Order = 9)] public string? FtpServerId { get; set; }
    [DataMember(Order = 10)] public string? FtsSourceLocationId { get; set; }
    [DataMember(Order = 11)] public bool IsSuspended { get; set; }
    [DataMember(Order = 12)] public DateTime? IsSuspendedChangedDate { get; set; }
    [DataMember(Order = 13)] public bool IsValid { get; set; }
    [DataMember(Order = 14)] public DateTime LastModifiedDate { get; set; }
    [DataMember(Order = 15)] public string? Mask { get; set; }
    [DataMember(Order = 16)] public string? OperatorId { get; set; }
    [DataMember(Order = 17)] public string? OwnerEmail { get; set; }
    [DataMember(Order = 18)] public string? OwnerModule { get; set; }
    [DataMember(Order = 19)] public string? OwnerName { get; set; }
    [DataMember(Order = 20)] public string? OwnerPhone { get; set; }
    [DataMember(Order = 21)] public string? Protocol { get; set; }
    [DataMember(Order = 22)] public bool RunOnFri { get; set; }
    [DataMember(Order = 23)] public bool RunOnFrstDayOfMonthInd { get; set; }
    [DataMember(Order = 24)] public bool RunOnLastDayOfMonthInd { get; set; }
    [DataMember(Order = 25)] public bool RunOnMonday { get; set; }
    [DataMember(Order = 26)] public bool RunOnSat { get; set; }
    [DataMember(Order = 27)] public bool RunOnSun { get; set; }
    [DataMember(Order = 28)] public byte RunOnThisDayOfMonth { get; set; }
    [DataMember(Order = 29)] public bool RunOnThrus { get; set; }
    [DataMember(Order = 30)] public bool RunOnTuesday { get; set; }
    [DataMember(Order = 31)] public bool RunOnWed { get; set; }
    [DataMember(Order = 32)] public bool SendNotifications { get; set; }
    [DataMember(Order = 33)] public string? StartTime { get; set; }
    [DataMember(Order = 34)] public string? TradingPartner { get; set; }
}

[DataContract]
public sealed class FtsSourceLocationUpdateCommand : FtsSourceLocationAddCommand
{
}

[DataContract]
public sealed class FtsSourceLocationInvalidateCommand : Command
{
    [DataMember(Order = 1)] public int ConfigId { get; set; }
    [DataMember(Order = 2)] public List<FtsDestinationLocationAdded> DestinationAddress { get; set; } = [];
    [DataMember(Order = 3)] public string? FtsSourceLocationId { get; set; }
    [DataMember(Order = 4)] public string? OperatorId { get; set; }
}

[DataContract]
public class AddFtpServerCommand : Command
{
    [DataMember(Order = 1)] public DateTime CreationDate { get; set; }
    [DataMember(Order = 2)] public string? FtpDescription { get; set; }
    [DataMember(Order = 3)] public string? FtpPassword { get; set; }
    [DataMember(Order = 4)] public short FtpPort { get; set; }
    [DataMember(Order = 5)] public string? FtpProxyAddress { get; set; }
    [DataMember(Order = 6)] public short? FtpProxyPort { get; set; }
    [DataMember(Order = 7)] public Guid FtpServerId { get; set; }
    [DataMember(Order = 8)] public string? FtpServerIp { get; set; }
    [DataMember(Order = 9)] public string? FtpServerName { get; set; }
    [DataMember(Order = 10)] public string? FtpTransferMode { get; set; }
    [DataMember(Order = 11)] public string? FtpUser { get; set; }
    [DataMember(Order = 12)] public bool IsValid { get; set; }
    [DataMember(Order = 13)] public DateTime LastModifiedDate { get; set; }
    [DataMember(Order = 14)] public string? OperatorId { get; set; }
    [DataMember(Order = 15)] public bool UseSSHAuth { get; set; }
}

[DataContract]
public sealed class UpdateFtpServerCommand : AddFtpServerCommand
{
}

[DataContract]
public sealed class InvalidateFtpServerCommand : AddFtpServerCommand
{
}

[DataContract]
public sealed class UpdateIntegrationFileCommand : Command
{
    [DataMember(Order = 1)] public int? ConfigId { get; set; }
    [DataMember(Order = 2)] public Guid FtsIntegrationFileId { get; set; }
}