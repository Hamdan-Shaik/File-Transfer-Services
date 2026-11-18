namespace Fxfer.DomainApi.Contracts.Queries.Parameters;

public sealed class SourceLocationQueryParms
{
    public string? SoureceLocationId { get; set; }

    public int? ConfigId { get; set; }

    public string? ConfigName { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerSubsystem { get; set; }

    public string? Protocol { get; set; }

    public bool IsActive { get; set; }

    // F01-095: Parse OwnerSubsystem using Guid.Parse for owner-module filtering; invalid values throw.
    public Guid OwnerModuleGuid
    {
        get
        {
            return Guid.Parse(OwnerSubsystem);
        }
    }
}