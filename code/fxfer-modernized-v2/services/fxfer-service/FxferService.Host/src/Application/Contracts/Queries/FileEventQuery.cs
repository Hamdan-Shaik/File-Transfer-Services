using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Contracts.Domain;

namespace FxferService.Application.Contracts.Queries;

/// <summary>
/// <para>File Event Query</para>
/// <para>Performs search against the FTS_FILE_MANAGEMENT and FTS_FILE_EVENT</para>
/// </summary>
[DataContract]
[KnownType(typeof(FileEventModel))]
[KnownType(typeof(FileEventQueryParms))]
public class FileEventQuery : Query<FileEventModel>
{
    /// <summary>
    /// <para>Gets and sets the query params</para>
    /// </summary>
    [DataMember]
    public FileEventQueryParms Where { get; set; } = null!;
}

