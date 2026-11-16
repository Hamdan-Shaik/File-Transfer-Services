using CoreWCF;

using FxferService.Application.Contracts.Queries;

namespace FxferService.Application.LegacyContracts;

/// <summary>
/// <para>Web Service</para>
/// <para>Business Name: File Transfer query service</para>
/// <para></para>
/// <para>Overview:</para>
/// <para>Describe overall purpose of the service</para>
/// <para>Data Models</para>
/// <list type="bullet">
/// <item><term><a target="_blank" href="../erd/Integration/FileTransferService_AllDataModels.png">Class Diagram</a></term><description>Data model classes in the File Transfer service</description></item>
/// </list>
/// </summary>
[ServiceContract(Namespace = "HP.HSP.FileTransferSvcQueryService")]
public interface IFileTransferSvcQueryService : IServiceAvailability
{
    /// <summary>
    /// Search Sourcelocations for matches to passed in search criteria
    /// </summary>
    /// <param name="query">Query object containing search criteria to be used to search for matching Sourcelocations records.</param>
    /// <returns>A Query object containing the results the of the Search.</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    SourceLocationQuery SearchSourceLocations(SourceLocationQuery query);

    /// <summary>
    /// Display for FTPServer Records
    /// </summary>
    /// <param name="query">Query object containing search for  FTPServer records.</param>
    /// <returns>A Query object containing the results the of the Values.</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    FtpServerQuery GetFtpServers(FtpServerQuery query);

    /// <summary>
    /// Get Sourcelocations for matches to passed in search criteria
    /// </summary>
    /// <param name="query">Query object containing search criteria to be used to search for matching Sourcelocations records.</param>
    /// <returns>A Query object containing the results the of the Search.</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    GetSourceLocationQuery GetSourceLocations(GetSourceLocationQuery query);

    /// <summary>
    /// Search File movement based on passed in search criteria.
    /// </summary>
    /// <param name="query">Query object containing search criteria to be used to search for matching File attributes records.</param>
    /// <returns>A Query object containing the results the of the Search.</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    FileTransferTrackingQuery SearchFileTransferMovement(FileTransferTrackingQuery query);

    /// <summary>
    /// Search Inbound Target in search criteria.
    /// </summary>
    /// <param name="query">Query object containing search criteria to be used to search for inbound target records.</param>
    /// <returns>A Query object containing the results of the Search.</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    InboundTargetQuery SearchInboundTarget(InboundTargetQuery query);

    /// <summary>
    /// Get Integration File in search criteria.
    /// </summary>
    /// <param name="query">Query object containing search criteria to be used to search for the integration file records.</param>
    /// <returns>A Query object containing the results of the Get.</returns>
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    GetIntegrationFileQuery GetIntegrationFile(GetIntegrationFileQuery query);
}

