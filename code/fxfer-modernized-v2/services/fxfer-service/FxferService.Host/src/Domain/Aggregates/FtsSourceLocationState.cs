using System.Runtime.Serialization;

namespace FxferService.Domain.Aggregates;

[DataContract]
public class FtsSourceLocationState : IAggregateState
{
    public FtsSourceLocationState()
    {
    }

    public FtsSourceLocationState(string id)
        : this()
    {
        this.Id = id;
    }

    public FtsSourceLocationState(string id, int version)
        : this(id)
    {
        this.Version = version;
    }

    /// <summary>
    /// <para>The internal identifier for the provider record.</para>
    /// <para>Remarks: Is of type Global Unique Identifier(GUID). Format XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</para>
    /// </summary>
    [DataMember]
    public string? Id { get; set; }

    /// <summary>
    /// <para>The Config ID of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public int ConfigId { get; set; }

    /// <summary>
    /// <para>The Config Name of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? ConfigName { get; set; }

    /// <summary>
    /// <para>The Config Description of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? Description { get; set; }

    /// <summary>
    /// <para>The Ftp Server Id File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? FtpServerId { get; set; }

    /// <summary>
    /// <para>The Address of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? Address { get; set; }

    /// <summary>
    /// <para>The Mask of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? Mask { get; set; }

    /// <summary>
    /// <para>The Version of the FtsSourceLocationState.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public int Version { get; set; }

    /// <summary>
    /// <para>The Protocol of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? Protocol { get; set; }

    /// <summary>
    /// <para>The FileDirection of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? FileDirection { get; set; }

    /// <summary>
    /// <para>The OwnerEmail of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? OwnerEmail { get; set; }

    /// <summary>
    /// <para>The OwnerName of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? OwnerName { get; set; }

    /// <summary>
    /// <para>The OwnerPhone of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? OwnerPhone { get; set; }

    /// <summary>
    /// <para>The OwnerModule of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? OwnerModule { get; set; }

    /// <summary>
    /// <para>The TradingPartner of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? TradingPartner { get; set; }

    /// <summary>
    /// <para>The RunOnMonday of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool RunOnMonday { get; set; }

    /// <summary>
    /// <para>The RunOnTuesday of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool RunOnTuesday { get; set; }

    /// <summary>
    /// <para>The RunOnWed of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool RunOnWed { get; set; }

    /// <summary>
    /// <para>The RunOnThrus of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool RunOnThrus { get; set; }

    /// <summary>
    /// <para>The RunOnFri of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool RunOnFri { get; set; }

    /// <summary>
    /// <para>The RunOnSat of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool RunOnSat { get; set; }

    /// <summary>
    /// <para>The RunOnSun of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool RunOnSun { get; set; }

    /// <summary>
    /// <para>The RunOnFrstDayOfMonthInd of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool RunOnFrstDayOfMonthInd { get; set; }

    /// <summary>
    /// <para>The RunOnLastDayOfMonthInd of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool RunOnLastDayOfMonthInd { get; set; }

    /// <summary>
    /// <para>The RunOnThisDayOfMonth of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public byte RunOnThisDayOfMonth { get; set; }

    /// <summary>
    /// <para>The StartTime of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? StartTime { get; set; }

    /// <summary>
    /// <para>The EndTime of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? EndTime { get; set; }

    /// <summary>
    /// <para>The OperatorId of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public string? OperatorId { get; set; }

    /// <summary>
    /// <para>The IsValid File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool IsValid { get; set; }

    /// <summary>
    /// <para>The EffectiveDate of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// <para>The SendNotifications of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public bool SendNotifications { get; set; }

    /// <summary>
    /// <para>The EndDate of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// <para>The LastModifiedDate of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public DateTime LastModifiedDate { get; set; }

    /// <summary>
    /// <para>The CreationDate of the File Transfer.</para>
    /// <para>Remarks:</para>
    /// </summary>
    [DataMember]
    public DateTime CreationDate { get; set; }
}

