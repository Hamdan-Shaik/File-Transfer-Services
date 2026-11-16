using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

[DataContract]
public class FtsSourceLocationAdded : FtsSourceLocationEvents
{
    [ExcludeFromCodeCoverage]
    public FtsSourceLocationAdded()
    {
    }

    [DataMember]
    public int ConfigId { get; set; }

    [DataMember]
    public string? ConfigName { get; set; }

    [DataMember]
    public string? Description { get; set; }

    [DataMember]
    public string? FtpServerId { get; set; }

    [DataMember]
    public string? Address { get; set; }

    [DataMember]
    public string? Mask { get; set; }

    [DataMember]
    public string? Protocol { get; set; }

    [DataMember]
    public string? FileDirection { get; set; }

    [DataMember]
    public string? OwnerEmail { get; set; }

    [DataMember]
    public string? OwnerName { get; set; }

    [DataMember]
    public string? OwnerPhone { get; set; }

    [DataMember]
    public string? OwnerModule { get; set; }

    [DataMember]
    public string? TradingPartner { get; set; }

    [DataMember]
    public string? OperatorId { get; set; }

    [DataMember]
    public bool RunOnMonday { get; set; }

    [DataMember]
    public bool RunOnTuesday { get; set; }

    [DataMember]
    public bool RunOnWed { get; set; }

    [DataMember]
    public bool RunOnThrus { get; set; }

    [DataMember]
    public bool RunOnFri { get; set; }

    [DataMember]
    public bool RunOnSat { get; set; }

    [DataMember]
    public bool RunOnSun { get; set; }

    [DataMember]
    public bool RunOnFrstDayOfMonthInd { get; set; }

    [DataMember]
    public bool RunOnLastDayOfMonthInd { get; set; }

    [DataMember]
    public byte RunOnThisDayOfMonth { get; set; }

    [DataMember]
    public string? StartTime { get; set; }

    [DataMember]
    public string? EndTime { get; set; }

    [DataMember]
    public bool IsValid { get; set; }

    [DataMember]
    public bool IsSuspended { get; set; }

    [DataMember]
    public DateTime? IsSuspendedChangedDate { get; set; }

    [DataMember]
    public bool SendNotifications { get; set; }

    [DataMember]
    public DateTime LastModifiedDate { get; set; }

    [DataMember]
    public DateTime CreationDate { get; set; }

    [DataMember]
    public List<FtsDestinationLocationAdded>? DestinationAddress { get; set; }
}
