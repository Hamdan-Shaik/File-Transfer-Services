using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Exceptions;

namespace FxferService.Application.Contracts.Queries;

/// <summary>
/// <para>Search Sourcelocations Query</para>
/// </summary>
[DataContract]
[KnownType(typeof(SourceLocationModel))]
[KnownType(typeof(SourceLocationQueryParms))]
public class GetSourceLocationQuery : Query<SourceLocationModel>
{
    /// <summary>
    /// <para>Gets and sets the query params</para>
    /// </summary>
    [DataMember]
    public GetSourceLocationQueryParms Where { get; set; } = null!;

    public void Validate()
    {
        // F02-012: GetSourceLocations requires Where.SourceLocationId to be present.
        if (string.IsNullOrEmpty(Where.SourceLocationId))
        {
            throw new FileTransferValidationException(FileTransferExceptionConstants.Validation.FtpSoureceLocationId);
        }
    }
}

