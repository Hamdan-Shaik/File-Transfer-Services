using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Contracts.Domain;

namespace FxferService.Application.Contracts.Queries;

/// <summary>
/// <para>File Exception Query</para>
/// <para>Performs search against the FTS_FILE_MANAGEMENT and FTS_FILE_EXCEPTION</para>
/// </summary>
[DataContract]
[KnownType(typeof(FileExceptionModel))]
[KnownType(typeof(FileExceptionQueryParms))]
public class FileExceptionQuery : Query<FileExceptionModel>
{
    /// <summary>
    /// <para>Gets and sets the query params</para>
    /// </summary>
    [DataMember]
    public FileExceptionQueryParms Where { get; set; } = null!;
}

