using CoreWCF;
using Fxfer.SoapFacade.CoreWcf.Contracts.Commands;
using Fxfer.SoapFacade.CoreWcf.Contracts.Events;
using Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;
using Fxfer.SoapFacade.CoreWcf.Contracts.Utilities;

namespace Fxfer.SoapFacade.CoreWcf.Contracts.Services;

[ServiceContract(Namespace = "HP.HSP.FileTransferSvc")]
public interface IFileTransferSvcService : IServiceAvailability
{
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    FtsSourceLocationAdded FileTransferAdd(FtsSourceLocationAddCommand command);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    FtsSourceLocationUpdated UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand command);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    FtsSourceLocationInvalidated InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand command);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    FtpServerAdded AddFtpServer(AddFtpServerCommand command);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    FtpServerUpdated UpdateFtpServer(UpdateFtpServerCommand command);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    FtpServerInvalidated InvalidateFtpServer(InvalidateFtpServerCommand command);

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command);
}
