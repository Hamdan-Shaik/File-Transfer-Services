using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Queries.Parameters;

/// <summary>
/// <para>Query parameters for Source Locations</para>
/// </summary>
[DataContract]
public class GetSourceLocationQueryParms : SerializableBase
{
    /// <summary>
    /// <para>Gets or Sets the SourceLocation Id</para>
    /// </summary>
    [DataMember]
    public string SourceLocationId { get; set; } = null!;

    /// <summary>
    /// <para>Gets or Sets the IsActive</para>
    /// </summary>
    [DataMember]
    public bool IsActive { get; set; }

    public Guid SoureceLocationGuid
    {
        get
        {
            // F02-013: treats SourceLocationId as GUID and parses with Guid.Parse (throws on invalid input)
            return Guid.Parse(this.SourceLocationId);
        }
    }
}

