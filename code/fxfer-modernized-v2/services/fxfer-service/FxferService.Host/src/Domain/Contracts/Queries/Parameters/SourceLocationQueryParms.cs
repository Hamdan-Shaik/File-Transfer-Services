using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Queries.Parameters;

/// <summary>
/// <para>Query parameters for Source Locations</para>
/// </summary>
[DataContract]
public class SourceLocationQueryParms : SerializableBase
{
    /// <summary>
    /// <para>Gets or Sets the SoureceLocation Id</para>
    /// </summary>
    public string SoureceLocationId { get; set; } = null!;

    /// <summary>
    /// <para>Gets or Sets the Config Id</para>
    /// </summary>
    [DataMember]
    public int? ConfigId { get; set; }

    /// <summary>
    /// <para>Gets or Sets the Config Name/para>
    /// </summary>
    [DataMember]
    public string ConfigName { get; set; } = null!;

    /// <summary>
    /// <para>Gets or Sets the Owner Name</para>
    /// </summary>
    [DataMember]
    public string OwnerName { get; set; } = null!;

    /// <summary>
    /// <para>Gets or Sets the Owner Subsystem</para>
    /// </summary>
    [DataMember]
    public string OwnerSubsystem { get; set; } = null!;

    /// <summary>
    /// <para>Gets the type of Protocol</para>
    /// </summary>
    [DataMember]
    public string Protocol { get; set; } = null!;

    /// <summary>
    /// <para>Gets or Sets the IsActive</para>
    /// </summary>
    [DataMember]
    public bool IsActive { get; set; }

    public Guid OwnerModuleGuid
    {
        get
        {
            // F02-010: OwnerSubsystem is parsed with Guid.Parse (throws on invalid input)
            return Guid.Parse(this.OwnerSubsystem);
        }
    }
}

