using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Contracts.Domain;

namespace FxferService.Application.Contracts.Queries;

/// <summary>
/// <para>Search Sourcelocations Query</para>
/// </summary>
[DataContract]
[KnownType(typeof(SourceLocationModel))]
[KnownType(typeof(SourceLocationQueryParms))]
public class SourceLocationQuery : Query<SourceLocationModel>
{
    /// <summary>
    /// <para>Gets and sets the query params</para>
    /// </summary>
    [DataMember]
    public SourceLocationQueryParms Where { get; set; } = null!;
}

