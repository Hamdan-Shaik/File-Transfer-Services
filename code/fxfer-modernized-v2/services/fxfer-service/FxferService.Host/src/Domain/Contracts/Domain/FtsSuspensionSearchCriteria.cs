using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// File Transfer Suspension Search Criteria
/// </summary>
[DataContract]
public class FtsSuspensionSearchCriteria
{
    /// <summary>
    /// Config Id
    /// </summary>
    [DataMember]
    public Guid SourceLocationId { get; set; }

    /// <summary>
    /// Error Text for Suspension Search
    /// </summary>
    [DataMember]
    public string? ErrorText { get; set; }

    /// <summary>
    /// StackTrace for Suspension Search
    /// </summary>
    [DataMember]
    public string? StackTrace { get; set; }

    /// <summary>
    /// Operator Id
    /// </summary>
    [DataMember]
    public string? OperatorId { get; set; }
}

