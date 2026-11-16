using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Exceptions;

namespace FxferService.Application.Contracts.Queries;

/// <summary>
/// <para>File Transfer Tracking Query</para>
/// <para>Performs search against the FTS_FILE_MANAGEMENT and FTS_SOURCE_LOCATION</para>
/// </summary>
[DataContract]
[KnownType(typeof(FileTransferTrackingModel))]
[KnownType(typeof(FileTransferTrackingQueryParms))]
public class FileTransferTrackingQuery : Query<FileTransferTrackingModel>
{
#pragma warning disable CS0169 // Field is never used
    #region Private Properties
    private List<BusinessExceptionMessage>? messages;
    #endregion
#pragma warning restore CS0169

    #region Public Properties
    /// <summary>
    /// <para>Gets and sets the query params</para>
    /// </summary>
    [DataMember]
    public FileTransferTrackingQueryParms Where { get; set; } = null!;
    #endregion
}

