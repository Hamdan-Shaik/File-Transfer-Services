using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Contracts.Domain;

namespace FxferService.Application.Contracts.Queries;

/// <summary>
/// <para>Search FTPServer Query</para>
/// </summary>
[DataContract]
[KnownType(typeof(FtpServerModel))]
[KnownType(typeof(FtpServerQueryParms))]
public class FtpServerQuery : Query<FtpServerModel>
{
    /// <summary>
    /// <para>Gets and sets the query params</para>
    /// </summary>
    [DataMember]
    public FtpServerQueryParms Where { get; set; } = null!;
}

