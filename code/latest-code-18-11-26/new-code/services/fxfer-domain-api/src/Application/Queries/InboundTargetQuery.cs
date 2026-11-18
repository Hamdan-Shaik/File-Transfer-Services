using Fxfer.DomainApi.Contracts.Queries.Parameters;
using HPP.Core.BAS.CQRS.UserMeta;

namespace Fxfer.DomainApi.Contracts.Queries;

public sealed record InboundTargetQuery
{
    public InboundTargetQuery()
    {
        Where = new InboundTargetQueryParms();
    }

    public InboundTargetQueryParms Where { get; set; }

    public RequestorModel? Requestor { get; set; }

    public void Validate()
    {
        // legacy no-op: Validate() only initializes an internal message list and never throws.
    }
}
