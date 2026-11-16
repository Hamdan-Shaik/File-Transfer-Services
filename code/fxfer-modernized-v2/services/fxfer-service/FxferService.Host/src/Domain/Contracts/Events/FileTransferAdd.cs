using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

[DataContract]
public class FileTransferAdd : FileTransferEvents
{
    public FileTransferAdd()
    {
    }

    [DataMember]
    public string? FtsSourceLocationsID { get; set; }
}
