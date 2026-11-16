using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fxfer.ExecutionWorker.Application.Helpers;

[DataContract]
internal sealed class FileTransferred
{
    [DataMember]
    public string ApplicationName { get; set; } = null!;

    [DataMember]
    public string DestinationAddressPath { get; set; } = null!;

    [DataMember]
    public Guid FileId { get; set; }

    [DataMember]
    public string FileName { get; set; } = null!;

    [DataMember]
    public Guid FTEMetadataId { get; set; }

    [DataMember]
    public Guid GroupId { get; set; }

    [DataMember]
    public string ModuleContract { get; set; } = null!;

    [DataMember]
    public string OwnerModule { get; set; } = null!;

    [DataMember]
    public string SourceAddressPath { get; set; } = null!;

    [DataMember]
    public Guid SourceLocationId { get; set; }

    [DataMember]
    public string SourceMask { get; set; } = null!;

    [DataMember]
    public string SourceProtocolCode { get; set; } = null!;

    [DataMember]
    public DateTime Timestamp { get; set; }

    [DataMember]
    public DateTime TransferDate { get; set; }

    [DataMember]
    public long ConfigurationId { get; set; }

    [DataMember]
    public string ConfigurationName { get; set; } = null!;

    [DataMember]
    public string ConfigurationDescription { get; set; } = null!;

    [DataMember]
    public string FileDirection { get; set; } = null!;

    [DataMember]
    public string FileType { get; set; } = null!;

    [DataMember]
    public long FileSize { get; set; }

    [DataMember]
    public DateTime StartTime { get; set; }

    [DataMember]
    public Guid CommitId { get; set; }

    [DataMember]
    public Dictionary<string, string> EventFilterMetadata { get; set; } = new();
}

