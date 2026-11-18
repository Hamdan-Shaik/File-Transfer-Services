using System.Runtime.Serialization;
using Fxfer.SoapFacade.CoreWcf.Contracts.Commands;
using Fxfer.SoapFacade.CoreWcf.Contracts.Domain;
using Fxfer.SoapFacade.CoreWcf.Contracts.Utilities;

namespace Fxfer.SoapFacade.CoreWcf.Contracts.Events;

[DataContract]
public sealed class FtsSourceLocationAdded : Event
{
    [DataMember(Order = 1)] public FtsSourceLocationAddCommand? Payload { get; set; }
}

[DataContract]
public sealed class FtsSourceLocationUpdated : Event
{
    [DataMember(Order = 1)] public FtsSourceLocationUpdateCommand? Payload { get; set; }
}

[DataContract]
public sealed class FtsSourceLocationInvalidated : Event
{
    [DataMember(Order = 1)] public long ConfigId { get; set; }
    [DataMember(Order = 2)] public List<FtsDestinationLocationAdded> DestinationAddress { get; set; } = [];
    [DataMember(Order = 3)] public string? OperatorId { get; set; }
}

[DataContract]
public sealed class FtpServerAdded : Event
{
    [DataMember(Order = 1)] public string? FtpDescription { get; set; }
    [DataMember(Order = 2)] public string? FtpPassword { get; set; }
    [DataMember(Order = 3)] public short FtpPort { get; set; }
    [DataMember(Order = 4)] public string? FtpProxyAddress { get; set; }
    [DataMember(Order = 5)] public short? FtpProxyPort { get; set; }
    [DataMember(Order = 6)] public Guid FtpServerId { get; set; }
    [DataMember(Order = 7)] public string? FtpServerIp { get; set; }
    [DataMember(Order = 8)] public string? FtpServerName { get; set; }
    [DataMember(Order = 9)] public string? FtpTransferMode { get; set; }
    [DataMember(Order = 10)] public string? FtpUser { get; set; }
    [DataMember(Order = 11)] public bool IsValid { get; set; }
    [DataMember(Order = 12)] public string? OperatorId { get; set; }
    [DataMember(Order = 13)] public bool UseSSHAuth { get; set; }
}

[DataContract]
public sealed class FtpServerUpdated : Event
{
    [DataMember(Order = 1)] public string? FtpDescription { get; set; }
    [DataMember(Order = 2)] public string? FtpPassword { get; set; }
    [DataMember(Order = 3)] public short FtpPort { get; set; }
    [DataMember(Order = 4)] public string? FtpProxyAddress { get; set; }
    [DataMember(Order = 5)] public short? FtpProxyPort { get; set; }
    [DataMember(Order = 6)] public Guid FtpServerId { get; set; }
    [DataMember(Order = 7)] public string? FtpServerIp { get; set; }
    [DataMember(Order = 8)] public string? FtpServerName { get; set; }
    [DataMember(Order = 9)] public string? FtpTransferMode { get; set; }
    [DataMember(Order = 10)] public string? FtpUser { get; set; }
    [DataMember(Order = 11)] public bool IsValid { get; set; }
    [DataMember(Order = 12)] public string? OperatorId { get; set; }
    [DataMember(Order = 13)] public bool UseSSHAuth { get; set; }
}

[DataContract]
public sealed class FtpServerInvalidated : Event
{
    [DataMember(Order = 1)] public string? FtpDescription { get; set; }
    [DataMember(Order = 2)] public string? FtpPassword { get; set; }
    [DataMember(Order = 3)] public short FtpPort { get; set; }
    [DataMember(Order = 4)] public string? FtpProxyAddress { get; set; }
    [DataMember(Order = 5)] public short? FtpProxyPort { get; set; }
    [DataMember(Order = 6)] public Guid FtpServerId { get; set; }
    [DataMember(Order = 7)] public string? FtpServerIp { get; set; }
    [DataMember(Order = 8)] public string? FtpServerName { get; set; }
    [DataMember(Order = 9)] public string? FtpTransferMode { get; set; }
    [DataMember(Order = 10)] public string? FtpUser { get; set; }
    [DataMember(Order = 11)] public bool IsValid { get; set; }
    [DataMember(Order = 12)] public string? OperatorId { get; set; }
    [DataMember(Order = 13)] public bool UseSSHAuth { get; set; }
}

[DataContract]
public sealed class IntegrationFileAdded : Event
{
    [DataMember(Order = 1)] public int? ConfigId { get; set; }
    [DataMember(Order = 2)] public Guid EtfOutboundMetadataId { get; set; }
    [DataMember(Order = 3)] public string? FileName { get; set; }
    [DataMember(Order = 4)] public Guid FtsIntegrationFileId { get; set; }
    [DataMember(Order = 5)] public List<GroupInfo> GroupInfo { get; set; } = [];
    [DataMember(Order = 6)] public bool IsProcessed { get; set; }
    [DataMember(Order = 7)] public bool IsValid { get; set; }
    [DataMember(Order = 8)] public string? OperatorId { get; set; }
}

[DataContract]
public sealed class IntegrationFileUpdated : Event
{
    [DataMember(Order = 1)] public int? ConfigId { get; set; }
    [DataMember(Order = 2)] public Guid EtfOutboundMetadataId { get; set; }
    [DataMember(Order = 3)] public string? FileName { get; set; }
    [DataMember(Order = 4)] public Guid FtsIntegrationFileId { get; set; }
    [DataMember(Order = 5)] public List<GroupInfo> GroupInfo { get; set; } = [];
    [DataMember(Order = 6)] public bool IsProcessed { get; set; }
    [DataMember(Order = 7)] public bool IsValid { get; set; }
    [DataMember(Order = 8)] public string? OperatorId { get; set; }
    [DataMember(Order = 9)] public DateTime LastModifiedDate { get; set; }
}