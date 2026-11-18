using System.Runtime.Serialization;
using Fxfer.SoapFacade.CoreWcf.Contracts.Domain;
using Fxfer.SoapFacade.CoreWcf.Contracts.Utilities;

namespace Fxfer.SoapFacade.CoreWcf.Contracts.Queries;

[DataContract]
public sealed class SourceLocationQuery : Query
{
    [DataMember(Order = 1)] public SourceLocationQueryParms? Where { get; set; }
    [DataMember(Order = 2)] public List<SortCriteria> SortCriteria { get; set; } = [];
    [DataMember(Order = 3)] public List<SourceLocationModel> Results { get; set; } = [];
}

[DataContract]
public sealed class SourceLocationQueryParms
{
    [DataMember(Order = 1)] public int? ConfigId { get; set; }
    [DataMember(Order = 2)] public string? ConfigName { get; set; }
    [DataMember(Order = 3)] public bool IsActive { get; set; }
    [DataMember(Order = 4)] public string? OwnerName { get; set; }
    [DataMember(Order = 5)] public string? OwnerSubsystem { get; set; }
    [DataMember(Order = 6)] public string? Protocol { get; set; }
}

[DataContract]
public sealed class FtpServerQuery : Query
{
    [DataMember(Order = 1)] public FtpServerQueryParms? Where { get; set; }
    [DataMember(Order = 2)] public List<SortCriteria> SortCriteria { get; set; } = [];
    [DataMember(Order = 3)] public List<FtpServerModel> Results { get; set; } = [];
}

[DataContract]
public sealed class FtpServerQueryParms
{
    [DataMember(Order = 1)] public string? FtpDescription { get; set; }
    [DataMember(Order = 2)] public Guid FtpServerId { get; set; }
    [DataMember(Order = 3)] public string? FtpServerName { get; set; }
    [DataMember(Order = 4)] public string? FtpUser { get; set; }
    [DataMember(Order = 5)] public bool? IsValid { get; set; }
    [DataMember(Order = 6)] public List<SearchModeOption> SearchOptions { get; set; } = [];
}

[DataContract]
public sealed class GetSourceLocationQuery : Query
{
    [DataMember(Order = 1)] public GetSourceLocationQueryParms? Where { get; set; }
    [DataMember(Order = 2)] public List<SortCriteria> SortCriteria { get; set; } = [];
    [DataMember(Order = 3)] public List<SourceLocationModel> Results { get; set; } = [];
}

[DataContract]
public sealed class GetSourceLocationQueryParms
{
    [DataMember(Order = 1)] public bool IsActive { get; set; }
    [DataMember(Order = 2)] public string? SourceLocationId { get; set; }
}

[DataContract]
public sealed class FileTransferTrackingQuery : Query
{
    [DataMember(Order = 1)] public FileTransferTrackingQueryParms? Where { get; set; }
    [DataMember(Order = 2)] public List<SortCriteria> SortCriteria { get; set; } = [];
    [DataMember(Order = 3)] public List<FileTransferTrackingModel> Results { get; set; } = [];
}

[DataContract]
public sealed class FileTransferTrackingQueryParms
{
    [DataMember(Order = 1)] public DateTime? EndDate { get; set; }
    [DataMember(Order = 2)] public string? FileDirection { get; set; }
    [DataMember(Order = 3)] public string? FileId { get; set; }
    [DataMember(Order = 4)] public string? FileName { get; set; }
    [DataMember(Order = 5)] public decimal? FileSizeFrom { get; set; }
    [DataMember(Order = 6)] public decimal? FileSizeThrough { get; set; }
    [DataMember(Order = 7)] public string? FileSourceLocationId { get; set; }
    [DataMember(Order = 8)] public string? FileStatus { get; set; }
    [DataMember(Order = 9)] public string? FileTransportType { get; set; }
    [DataMember(Order = 10)] public DateTime? StartDate { get; set; }
    [DataMember(Order = 11)] public string? TradingPartner { get; set; }
}

[DataContract]
public sealed class InboundTargetQuery : Query
{
    [DataMember(Order = 1)] public InboundTargetQueryParms? Where { get; set; }
    [DataMember(Order = 2)] public List<SortCriteria> SortCriteria { get; set; } = [];
    [DataMember(Order = 3)] public List<FtsInboundTargetModel> Results { get; set; } = [];
}

[DataContract]
public sealed class InboundTargetQueryParms
{
    [DataMember(Order = 1)] public string? ContractName { get; set; }
    [DataMember(Order = 2)] public Guid FteMetadataId { get; set; }
}

[DataContract]
public sealed class GetIntegrationFileQuery : Query
{
    [DataMember(Order = 1)] public IntegrationFileQueryParms? Where { get; set; }
    [DataMember(Order = 2)] public List<SortCriteria> SortCriteria { get; set; } = [];
    [DataMember(Order = 3)] public List<FtsIntegrationFileModel> Results { get; set; } = [];
}

[DataContract]
public sealed class IntegrationFileQueryParms
{
    [DataMember(Order = 1)] public Guid FtsIntegrationFileId { get; set; }
    [DataMember(Order = 2)] public string? FileName { get; set; }
}