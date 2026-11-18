using System.Runtime.Serialization;

namespace Fxfer.SoapFacade.CoreWcf.Contracts.Utilities;

[DataContract]
public class Command
{
    [DataMember(Order = 1)]
    public DateTime? CycleDate { get; set; }

    [DataMember(Order = 2)]
    public RequestorModel? Requestor { get; set; }

    [DataMember(Order = 3)]
    public DateTime ViewTimeStamp { get; set; }
}

[DataContract]
public sealed class RequestorModel
{
    [DataMember(Order = 1)]
    public string? ApplicationName { get; set; }

    [DataMember(Order = 2)]
    public string? BrowserInfo { get; set; }

    [DataMember(Order = 3)]
    public string? CorrelationId { get; set; }

    [DataMember(Order = 4)]
    public string? IdentifierId { get; set; }

    [DataMember(Order = 5)]
    public IdentifierIdType IdentifierIdType { get; set; }

    [DataMember(Order = 6)]
    public string? IpAddress { get; set; }

    [DataMember(Order = 7)]
    public string? LocaleCode { get; set; }

    [DataMember(Order = 8)]
    public DateTime RequestDate { get; set; }

    [DataMember(Order = 9)]
    public string? TenantId { get; set; }
}

public enum IdentifierIdType
{
    User = 0,
    System = 1
}

[DataContract]
public class Event
{
    [DataMember(Order = 1)]
    public string? ApplicationName { get; set; }

    [DataMember(Order = 2)]
    public Guid? CommitId { get; set; }

    [DataMember(Order = 3)]
    public string? CorrelationId { get; set; }

    [DataMember(Order = 4)]
    public DateTime? CycleDate { get; set; }

    [DataMember(Order = 5)]
    public string? EventID { get; set; }

    [DataMember(Order = 6)]
    public string? EventSequenceId { get; set; }

    [DataMember(Order = 7)]
    public Guid? InGroupId { get; set; }

    [DataMember(Order = 8)]
    public Guid? InProxyId { get; set; }

    [DataMember(Order = 9)]
    public bool IsLastFromGroup { get; set; }

    [DataMember(Order = 10)]
    public string? ModelID { get; set; }

    [DataMember(Order = 11)]
    public string? ModelType { get; set; }

    [DataMember(Order = 12)]
    public DateTime? Timestamp { get; set; }

    [DataMember(Order = 13)]
    public string? UserID { get; set; }

    [DataMember(Order = 14)]
    public string? UserIp { get; set; }
}

[DataContract]
public abstract class Query
{
    [DataMember(Order = 1)]
    public bool ByPassCountQuery { get; set; }

    [DataMember(Order = 2)]
    public int CurrentPage { get; set; }

    [DataMember(Order = 3)]
    public DateTime? CycleDate { get; set; }

    [DataMember(Order = 4)]
    public int PageSize { get; set; }

    [DataMember(Order = 5)]
    public RequestorModel? Requestor { get; set; }

    [DataMember(Order = 6)]
    public int RowCount { get; set; }

    [DataMember(Order = 7)]
    public DateTime ViewTimeStamp { get; set; }
}

[DataContract]
public sealed class SortCriteria
{
    [DataMember(Order = 1)]
    public string? FieldName { get; set; }

    [DataMember(Order = 2)]
    public SortOrder Order { get; set; }
}

public enum SortOrder
{
    Asc = 0,
    Desc = 1
}

[DataContract]
public sealed class SearchModeOption
{
    [DataMember(Order = 1)]
    public string? FieldName { get; set; }

    [DataMember(Order = 2)]
    public SearchMode FilterType { get; set; }
}

public enum SearchMode
{
    Contains = 0,
    EndsWith = 1,
    EqualTo = 2,
    StartsWith = 3
}