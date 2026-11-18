using CoreWCF;
using Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;
using Fxfer.SoapFacade.CoreWcf.Contracts.Queries;
using Fxfer.SoapFacade.CoreWcf.Contracts.Utilities;

namespace Fxfer.SoapFacade.LegacyWcf.Contracts;

[ServiceContract(Namespace = "HP.HSP.FileTransferSvcQueryService")]
public interface IFileTransferSvcQueryService : IServiceAvailability
{
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    SourceLocationQuery SearchSourceLocations(SourceLocationQuery query);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    FtpServerQuery GetFtpServers(FtpServerQuery query);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    GetSourceLocationQuery GetSourceLocations(GetSourceLocationQuery query);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    FileTransferTrackingQuery SearchFileTransferMovement(FileTransferTrackingQuery query);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    InboundTargetQuery SearchInboundTarget(InboundTargetQuery query);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    GetIntegrationFileQuery GetIntegrationFile(GetIntegrationFileQuery query);
}
