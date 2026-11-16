using System.Runtime.Serialization;

namespace FxferService.Application.Contracts.Queries;

[DataContract]
public enum SortOrder
{
    [EnumMember]
    Asc = 0,

    [EnumMember]
    Desc = 1,
}

