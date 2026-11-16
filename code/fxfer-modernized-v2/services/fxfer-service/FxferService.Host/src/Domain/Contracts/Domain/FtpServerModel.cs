using System;
using System.Runtime.Serialization;

#nullable disable


namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// <para>FTP Server model details</para>
/// </summary>
[Serializable]
[DataContract]
public class FtpServerModel
{
    #region Constructors
    /// <exclude/>
    public FtpServerModel()
    {
    }
    #endregion

    #region Properties
    /// <summary>
    /// <para>The internal identifier for the FtpServer record.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>  
    /// <item><term>MaxLength</term><description>36</description></item>
    /// <item><term>Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public Guid FtpServerId { get; set; }

    /// <summary>
    /// <para>The unique Name assigned to FTP Server.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>  
    /// <item><term>MaxLength</term><description>50</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FtpServerName { get; set; }

    /// <summary>
    /// <para>The FTP Server IP Address.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>  
    /// <item><term>MaxLength</term><description>15</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FtpServerIp { get; set; }

    /// <summary>
    /// <para>The File Transfer Direction, Inbound or Outbound.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Char</description></item>  
    /// <item><term>MaxLength</term><description>1</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FtpTransferMode { get; set; }

    /// <summary>
    /// <para>The FTP Server Username.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Alphanumeric</description></item>  
    /// <item><term>MaxLength</term><description>50</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FtpUser { get; set; }

    /// <summary>
    /// <para>The FTP Server password.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Alphanumeric</description></item>  
    /// <item><term>MaxLength</term><description>50</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FtpPassword { get; set; }

    /// <summary>
    /// <para>Brief description of the FTP Server.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Text</description></item>  
    /// <item><term>MaxLength</term><description>100</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FtpDescription { get; set; }

    /// <summary>
    /// <para>The FTP Server Port.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Numeric</description></item>  
    /// <item><term>MaxLength</term><description>5</description></item>
    /// <item><term>Valid Values</term><description>-32,768 to 32,7672</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public short FtpPort { get; set; }

    /// <summary>
    /// <para>Proxy Address to access the FTP Server.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>No</description></item>
    /// <item><term>Format</term><description>Alphanumeric</description></item>  
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FtpProxyAddress { get; set; }

    /// <summary>
    /// <para>The Proxy Port to access the FTP Server Port.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>No</description></item>
    /// <item><term>Format</term><description>Numeric</description></item>  
    /// <item><term>MaxLength</term><description>5</description></item>
    /// <item><term>Valid Values</term><description>-32,768 to 32,7672</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public short? FtpProxyPort { get; set; }

    /// <summary>
    /// <para>Flag to identify if the Ftp Server uses SSH Authentication.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Boolean</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public bool UseSSHAuth { get; set; }

    /// <summary>
    /// <para>Flag to identify if it is a valid FTP Server.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Boolean</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public bool IsValid { get; set; }

    /// <summary>
    /// <para>Date when the record was last modified.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>No</description></item>
    /// <item><term>Format</term><description>DateTime</description></item>
    /// <item><term>Valid Values</term><description>yyyy-MM-ddTHH.mm.ss.fffffffZ</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public DateTime LastModifiedDate { get; set; }

    /// <summary>
    /// <para>Date when the record was created.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>No</description></item>
    /// <item><term>Format</term><description>DateTime</description></item>
    /// <item><term>Valid Values</term><description>yyyy-MM-ddTHH.mm.ss.fffffffZ</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// The Id of the operator of user who inserted or updated records
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Alphanumeric</description></item>  
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string OperatorId { get; set; }
    #endregion
}
