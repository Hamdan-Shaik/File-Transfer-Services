using System.Runtime.Serialization;

namespace FxferService.Application.Contracts.Queries;

[DataContract]
public class SortCriteria
{
    [DataMember]
    public string? FieldName { get; set; }

    [DataMember]
    public SortOrder Order { get; set; }
}

