using System.Runtime.Serialization;

namespace FxferService.Application.Contracts.Queries;

[DataContract]
public class Query<TResult>
{
    [DataMember]
    public bool ByPassCountQuery { get; set; }

    [DataMember]
    public int CurrentPage { get; set; }

    [DataMember]
    public DateTime? CycleDate { get; set; }

    [DataMember]
    public int PageSize { get; set; }

    [DataMember]
    public int RowCount { get; set; }

    [DataMember]
    public DateTime ViewTimeStamp { get; set; }

    [DataMember]
    public List<TResult>? Results { get; set; }

    [DataMember]
    public List<SortCriteria>? SortCriteria { get; set; }
}

