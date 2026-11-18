using System.Runtime.Serialization;

namespace Fxfer.SoapFacade.CoreWcf.Contracts.Domain;

[DataContract]
public sealed class FtsDestinationLocationAdded
{
    [DataMember(Order = 1)] public bool CompressAtDestination { get; set; }
    [DataMember(Order = 2)] public string? ContactEmail { get; set; }
    [DataMember(Order = 3)] public string? ContactName { get; set; }
    [DataMember(Order = 4)] public string? ContactPhone { get; set; }
    [DataMember(Order = 5)] public string? ContractName { get; set; }
    [DataMember(Order = 6)] public DateTime CreationDate { get; set; }
    [DataMember(Order = 7)] public string? DestinationAddress { get; set; }
    [DataMember(Order = 8)] public bool DestinationArchive { get; set; }
    [DataMember(Order = 9)] public Guid DestinationId { get; set; }
    [DataMember(Order = 10)] public string? DestinationProtocol { get; set; }
    [DataMember(Order = 11)] public string? DestinationType { get; set; }
    [DataMember(Order = 12)] public Guid? FteMetadataId { get; set; }
    [DataMember(Order = 13)] public Guid FtpServerId { get; set; }
    [DataMember(Order = 14)] public string? ImportJobName { get; set; }
    [DataMember(Order = 15)] public bool IsValid { get; set; }
    [DataMember(Order = 16)] public DateTime LastModifiedDate { get; set; }
    [DataMember(Order = 17)] public string? OperatorId { get; set; }
    [DataMember(Order = 18)] public bool OverWriteAtDestination { get; set; }
    [DataMember(Order = 19)] public bool RenameAtDestination { get; set; }
    [DataMember(Order = 20)] public string? RenamePattern { get; set; }
    [DataMember(Order = 21)] public Guid SourceLocationId { get; set; }
}

[DataContract]
public sealed class GroupInfo
{
    [DataMember(Order = 1)] public Guid? GroupId { get; set; }
    [DataMember(Order = 2)] public int NumberOfRecords { get; set; }
}

[DataContract]
public sealed class SourceLocationModel
{
    [DataMember(Order = 1)] public string? Address { get; set; }
    [DataMember(Order = 2)] public int ConfigId { get; set; }
    [DataMember(Order = 3)] public string? ConfigName { get; set; }
    [DataMember(Order = 4)] public DateTime CreationDate { get; set; }
    [DataMember(Order = 5)] public string? Description { get; set; }
    [DataMember(Order = 6)] public List<FtsDestinationLocationAdded> DestinationAddress { get; set; } = [];
    [DataMember(Order = 7)] public string? EndWindow { get; set; }
    [DataMember(Order = 8)] public string? FileDirection { get; set; }
    [DataMember(Order = 9)] public Guid? FtpServerId { get; set; }
    [DataMember(Order = 10)] public string? FtpServerName { get; set; }
    [DataMember(Order = 11)] public Guid FtsSourceLocationId { get; set; }
    [DataMember(Order = 12)] public bool IsActive { get; set; }
    [DataMember(Order = 13)] public DateTime LastModifiedDate { get; set; }
    [DataMember(Order = 14)] public string? Mask { get; set; }
    [DataMember(Order = 15)] public string? OperatorId { get; set; }
    [DataMember(Order = 16)] public string? OwnerEmail { get; set; }
    [DataMember(Order = 17)] public string? OwnerName { get; set; }
    [DataMember(Order = 18)] public string? OwnerPhoneNumber { get; set; }
    [DataMember(Order = 19)] public Guid OwnerSubsystem { get; set; }
    [DataMember(Order = 20)] public string? Protocol { get; set; }
    [DataMember(Order = 21)] public bool RunOnFirstDay { get; set; }
    [DataMember(Order = 22)] public bool RunOnFriday { get; set; }
    [DataMember(Order = 23)] public bool RunOnLastDay { get; set; }
    [DataMember(Order = 24)] public bool RunOnMonday { get; set; }
    [DataMember(Order = 25)] public bool RunOnSaturday { get; set; }
    [DataMember(Order = 26)] public bool RunOnSunday { get; set; }
    [DataMember(Order = 27)] public byte RunOnThisDay { get; set; }
    [DataMember(Order = 28)] public bool RunOnThursday { get; set; }
    [DataMember(Order = 29)] public bool RunOnTuesday { get; set; }
    [DataMember(Order = 30)] public bool RunOnWednesday { get; set; }
    [DataMember(Order = 31)] public bool SendNotification { get; set; }
    [DataMember(Order = 32)] public string? StartWindow { get; set; }
    [DataMember(Order = 33)] public bool Suspended { get; set; }
    [DataMember(Order = 34)] public DateTime SuspendedChangeDate { get; set; }
    [DataMember(Order = 35)] public string? TradingPartner { get; set; }
    [DataMember(Order = 36)] public DateTime ValidChangeDate { get; set; }
}

[DataContract]
public sealed class FtpServerModel
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
}

[DataContract]
public sealed class FileTransferTrackingModel
{
    [DataMember(Order = 1)] public string? CheckSumCode { get; set; }
    [DataMember(Order = 2)] public DateTime CreatedDate { get; set; }
    [DataMember(Order = 3)] public List<FileEventModel> Events { get; set; } = [];
    [DataMember(Order = 4)] public List<FileExceptionModel> Exceptions { get; set; } = [];
    [DataMember(Order = 5)] public string? FileDirection { get; set; }
    [DataMember(Order = 6)] public Guid FileId { get; set; }
    [DataMember(Order = 7)] public string? FileName { get; set; }
    [DataMember(Order = 8)] public decimal FileSize { get; set; }
    [DataMember(Order = 9)] public Guid FileSourceLocationId { get; set; }
    [DataMember(Order = 10)] public string? FileStatus { get; set; }
    [DataMember(Order = 11)] public string? FileTransportType { get; set; }
    [DataMember(Order = 12)] public string? OriginatingServerName { get; set; }
    [DataMember(Order = 13)] public string? SourceLocAddress { get; set; }
    [DataMember(Order = 14)] public string? SourceLocConfigName { get; set; }
    [DataMember(Order = 15)] public string? TradingPartner { get; set; }
}

[DataContract]
public sealed class FileEventModel
{
    [DataMember(Order = 1)] public DateTime CreatedDate { get; set; }
    [DataMember(Order = 2)] public Guid DestinationId { get; set; }
    [DataMember(Order = 3)] public string? EventCode { get; set; }
    [DataMember(Order = 4)] public Guid EventId { get; set; }
    [DataMember(Order = 5)] public string? EventTrackDescription { get; set; }
    [DataMember(Order = 6)] public Guid FileId { get; set; }
}

[DataContract]
public sealed class FileExceptionModel
{
    [DataMember(Order = 1)] public DateTime CreatedDate { get; set; }
    [DataMember(Order = 2)] public Guid DestinationId { get; set; }
    [DataMember(Order = 3)] public Guid EventId { get; set; }
    [DataMember(Order = 4)] public Guid ExceptionId { get; set; }
    [DataMember(Order = 5)] public string? ExceptionType { get; set; }
    [DataMember(Order = 6)] public Guid FileId { get; set; }
    [DataMember(Order = 7)] public string? Message { get; set; }
    [DataMember(Order = 8)] public string? MethodName { get; set; }
    [DataMember(Order = 9)] public string? StackTrace { get; set; }
}

[DataContract]
public sealed class FtsInboundTargetModel
{
    [DataMember(Order = 1)] public string? ContractName { get; set; }
    [DataMember(Order = 2)] public DateTime CreationDate { get; set; }
    [DataMember(Order = 3)] public Guid FteMetadataId { get; set; }
    [DataMember(Order = 4)] public Guid FtsInboundTargetId { get; set; }
    [DataMember(Order = 5)] public string? ImportJobName { get; set; }
    [DataMember(Order = 6)] public bool IsValid { get; set; }
    [DataMember(Order = 7)] public DateTime LastModifiedDate { get; set; }
}

[DataContract]
public sealed class FtsIntegrationFileModel
{
    [DataMember(Order = 1)] public int? ConfigId { get; set; }
    [DataMember(Order = 2)] public DateTime CreationDate { get; set; }
    [DataMember(Order = 3)] public Guid EtfOutboundMetadataDefnId { get; set; }
    [DataMember(Order = 4)] public string? FileName { get; set; }
    [DataMember(Order = 5)] public Guid FtsIntegrationFileId { get; set; }
    [DataMember(Order = 6)] public Guid? GroupId { get; set; }
    [DataMember(Order = 7)] public bool IsProcessed { get; set; }
    [DataMember(Order = 8)] public bool IsValid { get; set; }
    [DataMember(Order = 9)] public DateTime LastModifiedDate { get; set; }
}