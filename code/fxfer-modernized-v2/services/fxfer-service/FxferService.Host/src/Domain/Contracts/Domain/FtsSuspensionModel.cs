using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// File Transfer Suspension model
/// </summary>
[DataContract]
public class FtsSuspensionModel
{
    /// <summary>
    /// File Track Id
    /// </summary>
    [DataMember]
    public Guid SakFileTrack { get; set; }

    /// <summary>
    /// File Suspension Id
    /// </summary>
    [DataMember]
    public Guid SuspensionId { get; set; }
}

