using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// Describes FTS Delivery
/// </summary>
[DataContract]
public class FtsDeliveryModel
{
    /// <summary>
    /// ID used to identify an Delivery details
    /// </summary>
    [DataMember]
    public Guid DeliveryId { get; set; }
}

