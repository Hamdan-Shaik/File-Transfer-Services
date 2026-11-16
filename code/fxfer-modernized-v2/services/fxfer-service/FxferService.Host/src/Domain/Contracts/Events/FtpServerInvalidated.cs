namespace FxferService.Domain.Contracts.Events;

/// <summary>
/// <para>Contains the information of an invalidated record to be saved on the FTP_SERVER table</para>
/// </summary>
public class FtpServerInvalidated : FtpServerUpdated
{
    #region Constructors
    /// <exclude/>
    public FtpServerInvalidated()
    {
    }
    #endregion
}
