using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// Describes File Management Response
/// </summary>
[DataContract]
public class FileManagementResponseModel
{
    [DataMember]
    public Guid FileId { get; set; }

    [DataMember]
    public Guid FileTrackId { get; set; }
}

