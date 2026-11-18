using Fxfer.DomainApi.Contracts.Queries.Parameters;

namespace Fxfer.DomainApi.Contracts.Queries;

public sealed record GetIntegrationFileQuery
{
    public GetIntegrationFileQuery()
    {
        Where = new IntegrationFileQueryParms();
    }

    public IntegrationFileQueryParms Where { get; set; }

    public void Validate()
    {
        // legacy no-op: Validate() only initializes an internal message list and never throws.
    }
}
