using System;
using System.Runtime.Serialization;
using FxferService.Domain.Constants;

namespace FxferService.Domain.Contracts.Events;

/// <summary>
/// <para>Base event class for File Transfer Services.</para>
/// </summary>
[DataContract]
public class FileTransferEvents : Event
{
    public override string ModelType
    {
        get
        {
            return CommonConstants.ModuleName;
        }
    }

    [DataMember]
    public DateTime StartTime { get; set; }
}