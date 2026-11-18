using CoreWCF;
using Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;
using Fxfer.SoapFacade.CoreWcf.Contracts.Queries;
using Fxfer.SoapFacade.CoreWcf.Contracts.Services;

namespace Fxfer.SoapFacade.Host.Soap.Services;

public sealed class FileTransferSvcQueryService : IFileTransferSvcQueryService
{
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public bool IsServiceAvailable() => true;

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public bool IsServiceHealthy(string tenantId) => true;

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public SourceLocationQuery SearchSourceLocations(SourceLocationQuery query) => query;

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public FtpServerQuery GetFtpServers(FtpServerQuery query) => query;

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public GetSourceLocationQuery GetSourceLocations(GetSourceLocationQuery query) => query;

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public FileTransferTrackingQuery SearchFileTransferMovement(FileTransferTrackingQuery query) => query;

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public InboundTargetQuery SearchInboundTarget(InboundTargetQuery query) => query;

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public GetIntegrationFileQuery GetIntegrationFile(GetIntegrationFileQuery query) => query;
}
