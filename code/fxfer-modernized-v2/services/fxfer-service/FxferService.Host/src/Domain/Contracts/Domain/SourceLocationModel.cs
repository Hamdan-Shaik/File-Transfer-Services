using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Events;

#nullable disable

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// <para>SourceLocation Model</para>
/// <para>SourceLocation Details</para>
/// </summary>
[Serializable]
[DataContract]
public class SourceLocationModel
{
    public SourceLocationModel()
        : base()
    {
    }

    /// <summary>
    /// <para>The internal identifier for the SourceLocation record.</para>
    /// <para>Remarks: Is of type Global Unique Identifier(GUID). Format XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</para>
    /// </summary>
    [DataMember]
    public Guid FtsSourceLocationId { get; set; }

    /// <summary>
    /// <para>The Config ID of the Sourcelocation.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public int ConfigId { get; set; }

    /// <summary>
    /// <para>The Config Name of the SourceLocation.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string ConfigName { get; set; }

    /// <summary>
    /// <para>The Config Description of the Sourcelocation</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string Description { get; set; }

    /// <summary>
    /// <para>The Ftp Server Id Sourcelocation</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public Guid? FtpServerId { get; set; }

    /// <summary>
    /// <para>The Ftp Server Name Sourcelocation</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string FtpServerName { get; set; }

    /// <summary>
    /// <para>The Address of the Sourcelocation</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string Address { get; set; }

    /// <summary>
    /// <para>The Owner Name of the SourceLocation.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string OwnerName { get; set; }

    /// <summary>
    /// <para>The Owner Email of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string OwnerEmail { get; set; }

    /// <summary>
    /// <para>The Owner Subsystem of the SourceLocation.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public Guid OwnerSubsystem { get; set; }

    /// <summary>
    /// <para>The Owner Phone of the Sourcelocation</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string OwnerPhoneNumber { get; set; }

    /// <summary>
    /// <para>The Protocol of the SourceLocation.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string Protocol { get; set; }

    /// <summary>
    /// <para>The File Direction of the Sourcelocation</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string FileDirection { get; set; }

    /// <summary>
    /// <para>The Mask of the SourceLocation.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string Mask { get; set; }

    /// <summary>
    /// <para>The Trading Partner for File Transfer</para>
    /// </summary>
    [DataMember]
    public string TradingPartner { get; set; }

    /// <summary>
    /// <para>The OperatorId for File Transfer</para>
    /// </summary>
    [DataMember]
    public string OperatorId { get; set; }

    /// <summary>
    /// <para> RunOn Monday for File Transfer</para>
    /// </summary>
    [DataMember]
    public bool RunOnMonday { get; set; }

    /// <summary>
    /// <para> RunOn Tuesday for File Transfer</para>
    /// </summary>
    [DataMember]
    public bool RunOnTuesday { get; set; }

    /// <summary>
    /// <para> RunOn Wednesday for File Transfer</para>
    /// </summary>
    [DataMember]
    public bool RunOnWednesday { get; set; }

    /// <summary>
    /// <para> RunOn Thrusday for File Transfer</para>
    /// </summary>
    [DataMember]
    public bool RunOnThursday { get; set; }

    /// <summary>
    /// <para> RunOn Friday for File Transfer</para>
    /// </summary>
    [DataMember]
    public bool RunOnFriday { get; set; }

    /// <summary>
    /// <para> RunOn Saturday for File Transfer</para>
    /// </summary>
    [DataMember]
    public bool RunOnSaturday { get; set; }

    /// <summary>
    /// <para>RunOn Sunday for File Transfer</para>
    /// </summary>
    [DataMember]
    public bool RunOnSunday { get; set; }

    /// <summary>
    /// <para>RunOnFrstDayOfMonthInd for File Transfer</para>
    /// </summary>
    [DataMember]
    public bool RunOnFirstDay { get; set; }

    /// <summary>
    /// <para>RunOnLastDayOfMonthInd for File Transfer</para>
    /// </summary>
    [DataMember]
    public bool RunOnLastDay { get; set; }

    /// <summary>
    /// <para>RunOnThisDayOfMonth for File Transfer</para>
    /// </summary>
    [DataMember]
    public byte RunOnThisDay { get; set; }

    /// <summary>
    /// <para>The StartTime for File Transfer</para>
    /// </summary>
    [DataMember]
    public string StartWindow { get; set; }

    /// <summary>
    /// <para>The EndTime for File Transfer</para>
    /// </summary>
    [DataMember]
    public string EndWindow { get; set; }

    /// <summary>
    /// <para>SendNotifications for File Transfer</para>
    /// </summary>
    [DataMember]
    public bool SendNotification { get; set; }

    /// <summary>
    /// <para>The Active</para>
    /// </summary>
    [DataMember]
    public bool IsActive { get; set; }

    /// <summary>
    /// <para>The ValidChangeDate for SourceLocation</para>
    /// </summary>
    [DataMember]
    public DateTime ValidChangeDate { get; set; }

    /// <summary>
    /// <para>The Suspended of the SourceLocation.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool Suspended { get; set; }

    /// <summary>
    /// <para>The SuspenddedChangeDate for SourceLocation</para>
    /// </summary>
    [DataMember]
    public DateTime SuspendedChangeDate { get; set; }

    /// <summary>
    /// <para>The LastModifiedDate for SourceLocation</para>
    /// </summary>
    [DataMember]
    public DateTime LastModifiedDate { get; set; }

    /// <summary>
    /// <para>The CreationDate for SourceLocation</para>
    /// </summary>
    [DataMember]
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// <para>The List of related Destination address.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public List<FtsDestinationLocationAdded> DestinationAddress { get; set; }
}
