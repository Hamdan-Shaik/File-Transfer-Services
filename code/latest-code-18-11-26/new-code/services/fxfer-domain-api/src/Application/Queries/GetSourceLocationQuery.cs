using Fxfer.DomainApi.Contracts.Exceptions;
using Fxfer.DomainApi.Contracts.Queries.Parameters;

namespace Fxfer.DomainApi.Contracts.Queries;

public sealed record GetSourceLocationQuery
{
    public GetSourceLocationQueryParms? Where { get; set; }

    public void Validate()
    {
        // F01-100: Getting source location details requires a non-empty SourceLocationId.
        if (Where is null || string.IsNullOrEmpty(Where.SourceLocationId))
        {
            throw new FileTransferValidationException("FtpSoureceLocationId");
        }
    }
}
