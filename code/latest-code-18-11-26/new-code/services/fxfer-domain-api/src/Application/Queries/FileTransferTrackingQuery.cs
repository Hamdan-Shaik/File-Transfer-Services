using Fxfer.DomainApi.Contracts.Queries.Parameters;

namespace Fxfer.DomainApi.Contracts.Queries;

public sealed record FileTransferTrackingQuery
{
    public FileTransferTrackingQueryParms? Where { get; set; }

    public IReadOnlyList<SortCriteria>? SortCriteria { get; set; }

    public int? Page { get; set; }

    public int? PageSize { get; set; }
}
