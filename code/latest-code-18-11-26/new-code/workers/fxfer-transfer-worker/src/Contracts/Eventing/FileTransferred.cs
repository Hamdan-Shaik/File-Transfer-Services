using System.Runtime.Serialization;

namespace Fxfer.TransferWorker.Contracts.Eventing;

[DataContract]
public sealed class FileTransferred
{
    [DataMember]
    public string? ApplicationName { get; set; }

    [DataMember]
    public Guid GroupId { get; set; }

    [DataMember]
    public Guid FTEMetadataId { get; set; }

    [DataMember]
    public Guid FileId { get; set; }

    [DataMember]
    public Guid SourceLocationId { get; set; }

    [DataMember]
    public string? FileName { get; set; }

    [DataMember]
    public long ConfigurationId { get; set; }

    [DataMember]
    public string? ConfigurationName { get; set; }

    [DataMember]
    public string? SourceProtocolCode { get; set; }

    [DataMember]
    public string? SourceAddressPath { get; set; }

    [DataMember]
    public string? DestinationAddressPath { get; set; }

    [DataMember]
    public string? SourceMask { get; set; }

    [DataMember]
    public string? OwnerModule { get; set; }

    [DataMember]
    public string? ModuleContract { get; set; }

    [DataMember]
    public string? ConfigurationDescription { get; set; }

    [DataMember]
    public DateTime TransferDate { get; set; }

    [DataMember]
    public string? FileDirection { get; set; }

    [DataMember]
    public long FileSize { get; set; }

    [DataMember]
    public string? FileType { get; set; }

    [DataMember]
    public DateTime Timestamp { get; set; }

    [DataMember]
    public DateTime StartTime { get; set; }

    [DataMember]
    public Guid CommitId { get; set; }

    [DataMember]
    public Dictionary<string, string> EventFilterMetadata { get; set; } = new();
}