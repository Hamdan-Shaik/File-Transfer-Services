using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

[DataContract]
public class FtsDestinationLocationAdded : FtsSourceLocationEvents
{
    [ExcludeFromCodeCoverage]
    public FtsDestinationLocationAdded()
    {
    }

    [DataMember]
    public Guid DestinationId { get; set; }

    [DataMember]
    public Guid SourceLocationId { get; set; }

    [DataMember]
    public string? DestinationAddress { get; set; }

    [DataMember]
    public string? DestinationProtocol { get; set; }

    [DataMember]
    public Guid FtpServerId { get; set; }

    [DataMember]
    public string? RenamePattern { get; set; }

    [DataMember]
    public bool OverWriteAtDestination { get; set; }

    [DataMember]
    public bool CompressAtDestination { get; set; }

    [DataMember]
    public bool RenameAtDestination { get; set; }

    [DataMember]
    public bool DestinationArchive { get; set; }

    [DataMember]
    public string? ContactName { get; set; }

    [DataMember]
    public string? ContactPhone { get; set; }

    [DataMember]
    public string? ContactEmail { get; set; }

    [DataMember]
    public bool IsValid { get; set; }

    [DataMember]
    public DateTime LastModifiedDate { get; set; }

    [DataMember]
    public DateTime CreationDate { get; set; }

    [DataMember]
    public string? OperatorId { get; set; }

    [DataMember]
    public Guid? FTEMetadataId { get; set; }

    [DataMember]
    public string? DestinationType { get; set; }

    [DataMember]
    public string? ContractName { get; set; }

    [DataMember]
    public string? ImportJobName { get; set; }
}
