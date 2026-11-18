using System;
using System.Runtime.Serialization;

namespace Fxfer.DomainApi.Domain.Aggregates;

/// <summary>
/// Fts Source Location Aggregate.
/// The current state of the Fts Source Location record.
/// </summary>
[DataContract]
public sealed class FtsSourceLocationState
{
    public FtsSourceLocationState()
    {
    }

    public FtsSourceLocationState(string id)
        : this()
    {
        Id = id;
    }

    public FtsSourceLocationState(string id, int version)
        : this(id)
    {
        Version = version;
    }

    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public int ConfigId { get; set; }

    [DataMember]
    public string ConfigName { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public string FtpServerId { get; set; }

    [DataMember]
    public string Address { get; set; }

    [DataMember]
    public string Mask { get; set; }

    [DataMember]
    public int Version { get; set; }

    [DataMember]
    public string Protocol { get; set; }

    [DataMember]
    public string FileDirection { get; set; }

    [DataMember]
    public string OwnerEmail { get; set; }

    [DataMember]
    public string OwnerName { get; set; }

    [DataMember]
    public string OwnerPhone { get; set; }

    [DataMember]
    public string OwnerModule { get; set; }

    [DataMember]
    public string TradingPartner { get; set; }

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
    public string StartTime { get; set; }

    [DataMember]
    public string EndTime { get; set; }

    [DataMember]
    public string OperatorId { get; set; }

    [DataMember]
    public bool IsValid { get; set; }

    [DataMember]
    public DateTime EffectiveDate { get; set; }

    [DataMember]
    public bool SendNotifications { get; set; }

    [DataMember]
    public DateTime EndDate { get; set; }

    [DataMember]
    public DateTime LastModifiedDate { get; set; }

    [DataMember]
    public DateTime CreationDate { get; set; }
}
