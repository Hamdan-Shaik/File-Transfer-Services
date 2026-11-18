namespace Fxfer.DomainApi.Contracts.Queries.Parameters;

public sealed class GetSourceLocationQueryParms
{
    public string? SourceLocationId { get; set; }

    // F01-104: Request parameter for details lookup; query execution does not filter on this flag.
    public bool IsActive { get; set; }

    // F01-101: Parse SourceLocationId using Guid.Parse; invalid values throw.
    public Guid SoureceLocationGuid
    {
        get
        {
            return Guid.Parse(SourceLocationId);
        }
    }
}