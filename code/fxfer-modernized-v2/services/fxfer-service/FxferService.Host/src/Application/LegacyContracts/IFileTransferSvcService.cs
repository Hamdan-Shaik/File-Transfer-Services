using CoreWCF;
using FxferService.Domain.Contracts.Events;

namespace FxferService.Application.LegacyContracts;

/// <summary>    
/// <para>Service used to Transfer Files from a Source Location to a Destination Location</para>
/// <para>Service Characteristics</para>
/// <list type="bullet">
/// <item><term>Version</term><description>1.0</description></item>
/// <item><term>Dependent Service(s)</term><description>None</description></item>
/// <item><term>Category Type</term><description>Entity Service</description></item>
/// <item><term>Implementation Type</term><description>Custom</description></item>
/// <item><term>Service Pattern</term><description>SOAP</description></item>
/// <item><term>Design Pattern</term><description>CQRS Command</description></item>
/// <item><term>Processing Modes</term><description>Single Call, Batch</description></item>
/// <item><term>Published Events</term><description>No</description></item>
/// <item><term>Audit Type</term><description>None</description></item>
/// <item><term>Bindings Supported</term><description>ws2007FederationHttpBinding, basicHttpsBinding</description></item>
/// <item><term>Expected Volume</term><description>Less than 100 txns per hour</description></item>
/// </list>    
/// <para>Faults</para>
/// <list type="bullet">
/// <item><term>ServiceException</term><description>Unhandled exception</description></item>
/// <item><term>BusinessValidationException</term><description>Missing Parameters</description></item>
/// </list>
///  <para>Domains</para>
/// <list type="bullet">
/// <item><term><a target="_blank" href="../erd/Integration/FileTransferService_AllDomainObjects.png">Class Diagram</a></term><description>Domain classes in the File Transfer service</description></item>
/// </list>
/// </summary>
[ServiceContract(Namespace = "HP.HSP.FileTransferSvc")]
public interface IFileTransferSvcService : IServiceAvailability
{
    /// <summary>
    /// <para>This operation adds a new Location for File Transfer details</para>
    /// <para>Operation Characteristics</para>
    /// <list type="bullet">
    /// <item><term>Data Classification</term><description>Standard</description></item>
    /// <item><term>Operation Mode</term><description>Single</description></item>
    /// <item><term>Callback Service</term><description>No</description></item>
    /// <item><term>Event Sourced</term><description>No</description></item>
    /// </list>
    /// <para>BusinessValidationException</para>
    /// <list type="bullet">
    /// <item><term>MissingFtpServerId</term><description>Missing Parameters: FtpServerId</description></item>
    /// </list>
    /// </summary>
    /// <param name="command">This operation expect a FtsSourceLocationAddCommand object</param>
    /// <returns>This operation returns a FtsSourceLocationAdded(event) object that contains the new Source Location configuration</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    FtsSourceLocationAdded FileTransferAdd(FtsSourceLocationAddCommand command);

    /// <summary>
    /// <para>This operation to update a File Transfer details.</para>
    /// <para>Operation Characteristics</para>
    /// <list type="bullet">
    /// <item><term>Data Classification</term><description>Standard</description></item>
    /// <item><term>Operation Mode</term><description>Single</description></item>
    /// <item><term>Callback Service</term><description>No</description></item>
    /// <item><term>Event Sourced</term><description>No</description></item>
    /// </list>
    /// <para>BusinessValidationException</para>
    /// <list type="bullet">
    /// <item><term>MissingFtpServerId</term><description>Missing Parameters: FtpServerId</description></item>
    /// <item><term>MissingFtpSoureceLocationId</term><description>Missing Parameters: FtpSoureceLocationId</description></item>
    /// <item><term>MissingIsSuspendedChangedDate</term><description>Missing Parameters: IsSuspendedChangedDate</description></item>
    /// <item><term>MissingConfigId</term><description>Missing Parameters: ConfigId</description></item>
    /// </list>
    /// </summary>
    /// <param name="command">This operation expect a FtsSourceLocationUpdateCommand object</param>
    /// <returns>This operation returns a FtsSourceLocationUpdated(event) object that contains the updated Source Location configuration</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    FtsSourceLocationUpdated UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand command);

    /// <summary>
    /// <para>This operation to invalidate a File Transfer details.</para>
    /// <para>Operation Characteristics</para>
    /// <list type="bullet">
    /// <item><term>Data Classification</term><description>Standard</description></item>
    /// <item><term>Operation Mode</term><description>Single</description></item>
    /// <item><term>Callback Service</term><description>No</description></item>
    /// <item><term>Event Sourced</term><description>No</description></item>
    /// </list>
    /// <para>BusinessValidationException</para>
    /// <list type="bullet">
    /// <item><term>MissingOperatorId</term><description>Missing Parameters: OperatorId</description></item>
    /// <item><term>MissingFtpSoureceLocationId</term><description>Missing Parameters: FtpSoureceLocationId</description></item>        
    /// <item><term>MissingConfigId</term><description>Missing Parameters: ConfigId</description></item>
    /// </list>
    /// </summary>
    /// <param name="command">This operation expect a FtsSourceLocationInvalidateCommand object</param>
    /// <returns>This operation returns a FtsSourceLocationInvalidated(event) object that contains the invalidated Source Location configuration</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    FtsSourceLocationInvalidated InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand command);

    /// <summary>
    /// <para>This operation Adds FTP Server details.</para>
    /// <para>Operation Characteristics</para>
    /// <list type="bullet">
    /// <item><term>Data Classification</term><description>Standard</description></item>
    /// <item><term>Operation Mode</term><description>Single</description></item>
    /// <item><term>Callback Service</term><description>No</description></item>
    /// <item><term>Event Sourced</term><description>No</description></item>
    /// </list>
    /// <para>BusinessValidationException</para>
    /// <list type="bullet">
    /// <item><term>MissingFtpServerId</term><description>Missing Parameters: FtpServerId</description></item>
    /// <item><term>MissingFtpServerName</term><description>Missing Parameters: FtpServerName</description></item>        
    /// <item><term>MissingFtpDescription</term><description>Missing Parameters: FtpDescription</description></item>
    /// <item><term>MissingFtpServerIp</term><description>Missing Parameters: FtpServerIp</description></item>
    /// <item><term>MissingFtpTransferMode</term><description>Missing Parameters: FtpTransferMode</description></item>
    /// <item><term>MissingFtpUser</term><description>Missing Parameters: FtpUser</description></item>
    /// <item><term>MissingFtpFtpPassword</term><description>Missing Parameters: FtpPassword</description></item>
    /// <item><term>MissingFtpPort</term><description>Missing Parameters: FtpPort</description></item>
    /// <item><term>InvalidFormatFtpProxyAddress</term><description>Invalid Format: FtpProxyAddress</description></item>
    /// <item><term>InvalidValueFtpProxyPort</term><description>Invalid Value: FtpProxyPort</description></item>
    /// </list>
    /// </summary>
    /// <param name="command">This operation expect a AddFtpServerCommand object</param>
    /// <returns>This operation returns a FtpServerAdded(event) object that contains the new FTP Server</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    FtpServerAdded AddFtpServer(AddFtpServerCommand command);

    /// <summary>
    /// <para>This operation Updates FTP Server details.</para>
    /// <para>Operation Characteristics</para>
    /// <list type="bullet">
    /// <item><term>Data Classification</term><description>Standard</description></item>
    /// <item><term>Operation Mode</term><description>Single</description></item>
    /// <item><term>Callback Service</term><description>No</description></item>
    /// <item><term>Event Sourced</term><description>No</description></item>
    /// </list>
    /// <para>BusinessValidationException</para>
    /// <list type="bullet">
    /// <item><term>MissingFtpServerId</term><description>Missing Parameters: FtpServerId</description></item>
    /// <item><term>MissingFtpServerName</term><description>Missing Parameters: FtpServerName</description></item>        
    /// <item><term>MissingFtpDescription</term><description>Missing Parameters: FtpDescription</description></item>
    /// <item><term>MissingFtpServerIp</term><description>Missing Parameters: FtpServerIp</description></item>
    /// <item><term>MissingFtpTransferMode</term><description>Missing Parameters: FtpTransferMode</description></item>
    /// <item><term>MissingFtpUser</term><description>Missing Parameters: FtpUser</description></item>
    /// <item><term>MissingFtpFtpPassword</term><description>Missing Parameters: FtpPassword</description></item>
    /// <item><term>MissingFtpPort</term><description>Missing Parameters: FtpPort</description></item>
    /// <item><term>InvalidFormatFtpProxyAddress</term><description>Invalid Format: FtpProxyAddress</description></item>
    /// <item><term>InvalidValueFtpProxyPort</term><description>Invalid Value: FtpProxyPort</description></item>
    /// </list>
    /// </summary>
    /// <param name="command">This operation expect a UpdateFtpServerCommand object</param>
    /// <returns>This operation returns a FtpServerUpdated(event) object that contains the updated FTP Server</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    FtpServerUpdated UpdateFtpServer(UpdateFtpServerCommand command);

    /// <summary>
    /// <para>This operation Invalidates FTP Server details.</para>
    /// <para>Operation Characteristics</para>
    /// <list type="bullet">
    /// <item><term>Data Classification</term><description>Standard</description></item>
    /// <item><term>Operation Mode</term><description>Single</description></item>
    /// <item><term>Callback Service</term><description>No</description></item>
    /// <item><term>Event Sourced</term><description>No</description></item>
    /// </list>
    /// <para>BusinessValidationException</para>
    /// <list type="bullet">
    /// <item><term>MissingFtpServerId</term><description>Missing Parameters: FtpServerId</description></item>
    /// </list>
    /// </summary>
    /// <param name="command">This operation expect a InvalidateFtpServerCommand object</param>
    /// <returns>This operation returns a FtpServerInvalidated(event) object that contains the invalidated FTP Server</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    FtpServerInvalidated InvalidateFtpServer(InvalidateFtpServerCommand command);

    /// <summary>
    /// <para>This operation Updates FTS Integration File details.</para>
    /// <para>Operation Characteristics</para>
    /// <list type="bullet">
    /// <item><term>Data Classification</term><description>Standard</description></item>
    /// <item><term>Operation Mode</term><description>Single</description></item>
    /// <item><term>Callback Service</term><description>No</description></item>
    /// <item><term>Event Sourced</term><description>No</description></item>
    /// </list>
    /// <para>BusinessValidationException</para>
    /// <list type="bullet">
    /// <item><term>MissingFileName</term><description>Missing Parameters: FileName</description></item> 
    /// <item><term>MissingETFOutboundMetadataDefnId</term><description>Missing Parameters: ETF_Outbound_Metadata_Defn_Id</description></item> 
    /// </list>
    /// </summary>
    /// <param name="command">This operation expect a UpdateIntegrationFileCommand object</param>
    /// <returns>This operation returns a IntegrationFileUpdated(event) object that contains the updated Integration File</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command);
}

