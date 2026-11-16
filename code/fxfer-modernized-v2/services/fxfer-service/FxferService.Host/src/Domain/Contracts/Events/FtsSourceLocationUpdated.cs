using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

/// <summary>
/// <para>Fts Source Location Aggregate</para>
/// <para>The current state of the Fts Source Location record.</para>
/// </summary>
[DataContract]
public class FtsSourceLocationUpdated : FtsSourceLocationEvents
{
    /// <summary>
    /// <para>Fts Source Location ConfigId</para>
    /// <para>The ConfigId of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public int ConfigId { get; set; }

    /// <summary>
    /// <para>Fts Source Location ConfigName</para>
    /// <para>The ConfigName of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? ConfigName { get; set; }

    /// <summary>
    /// <para>Fts Source Location Description</para>
    /// <para>The Description of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? Description { get; set; }

    /// <summary>
    /// <para>Fts Source Location FtpServerId</para>
    /// <para>The FtpServerId of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? FtpServerId { get; set; }

    /// <summary>
    /// <para>Fts Source Location Address</para>
    /// <para>The Address of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? Address { get; set; }

    /// <summary>
    /// <para>Fts Source Location Mask</para>
    /// <para>The Mask of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? Mask { get; set; }

    /// <summary>
    /// <para>Fts Source Location Protocol</para>
    /// <para>The Protocol of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? Protocol { get; set; }

    /// <summary>
    /// <para>Fts Source Location FileDirection</para>
    /// <para>The FileDirection of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? FileDirection { get; set; }

    /// <summary>
    /// <para>Fts Source Location OwnerEmail</para>
    /// <para>The OwnerEmail of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? OwnerEmail { get; set; }

    /// <summary>
    /// <para>Fts Source Location OwnerName</para>
    /// <para>The OwnerName of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? OwnerName { get; set; }

    /// <summary>
    /// <para>Fts Source Location OwnerPhone</para>
    /// <para>The OwnerPhone of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? OwnerPhone { get; set; }

    /// <summary>
    /// <para>Fts Source Location OwnerModule</para>
    /// <para>The OwnerModule of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? OwnerModule { get; set; }

    /// <summary>
    /// <para>Fts Source Location TradingPartner</para>
    /// <para>The TradingPartner of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? TradingPartner { get; set; }

    /// <summary>
    /// <para>Fts Source Location OperatorId</para>
    /// <para>The OperatorId of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? OperatorId { get; set; }

    /// <summary>
    /// <para>Fts Source Location RunOnMonday</para>
    /// <para>The RunOnMonday of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public bool RunOnMonday { get; set; }

    /// <summary>
    /// <para>Fts Source Location RunOnTuesday</para>
    /// <para>The RunOnTuesday of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public bool RunOnTuesday { get; set; }

    /// <summary>
    /// <para>Fts Source Location RunOnWed</para>
    /// <para>The RunOnWed of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public bool RunOnWed { get; set; }

    /// <summary>
    /// <para>Fts Source Location RunOnThrus</para>
    /// <para>The RunOnThrus of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public bool RunOnThrus { get; set; }

    /// <summary>
    /// <para>Fts Source Location RunOnFri</para>
    /// <para>The RunOnFri of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public bool RunOnFri { get; set; }

    /// <summary>
    /// <para>Fts Source Location RunOnSat</para>
    /// <para>The RunOnSat of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public bool RunOnSat { get; set; }

    /// <summary>
    /// <para>Fts Source Location RunOnSun</para>
    /// <para>The Run On sunday  of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public bool RunOnSun { get; set; }

    /// <summary>
    /// <para>Fts Source Location RunOnFrstDayOfMonthInd</para>
    /// <para>The Run On particluar day  of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public bool RunOnFrstDayOfMonthInd { get; set; }

    /// <summary>
    /// <para>Fts Source Location RunOnLastDayOfMonthInd</para>
    /// <para>The Run On particluar day  of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public bool RunOnLastDayOfMonthInd { get; set; }

    /// <summary>
    /// <para>Fts Source Location RunOnThisDayOfMonth</para>
    /// <para>The Run On particluar day  of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public byte RunOnThisDayOfMonth { get; set; }

    /// <summary>
    /// <para>Fts Source Location run start time</para>
    /// <para>The start time of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? StartTime { get; set; }

    /// <summary>
    /// <para>Fts Source Location run end time</para>
    /// <para>The end time of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public string? EndTime { get; set; }

    /// <summary>
    /// <para>The Is valid</para>
    /// <para>The current state of the Fts Source Location record.</para>
    /// </summary>
    [DataMember]
    public bool IsValid { get; set; }

    /// <summary>
    /// <para>The IsSuspended</para>
    /// <para>Is Suspended</para>
    /// </summary>
    [DataMember]
    public bool IsSuspended { get; set; }

    /// <summary>
    /// <para>the IsSuspended Changed Date</para>
    /// <para>The date when issuspended checked</para>
    /// </summary>
    [DataMember]
    public DateTime? IsSuspendedChangedDate { get; set; }

    /// <summary>
    /// <para>Send Notifications</para>
    /// <para>Send Notifications.</para>
    /// </summary>
    [DataMember]
    public bool SendNotifications { get; set; }

    /// <summary>
    /// <para>Fts Source Location LastModified Date</para>
    /// <para>The Fts Source Location LastModified Date.</para>
    /// </summary>
    [DataMember]
    public DateTime LastModifiedDate { get; set; }

    /// <summary>
    /// <para>Fts Source Location Creation Date</para>
    /// <para>The Creation Date</para>
    /// </summary>
    [DataMember]
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// <para>Destination Address</para>
    /// <para>The list of Destination Address</para>
    /// </summary>
    [DataMember]
    public List<FtsDestinationLocationAdded>? DestinationAddress { get; set; }
}
