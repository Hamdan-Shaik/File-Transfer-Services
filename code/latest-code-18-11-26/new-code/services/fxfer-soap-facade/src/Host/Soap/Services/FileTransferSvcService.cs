using CoreWCF;
using Fxfer.SoapFacade.CoreWcf.Contracts.Commands;
using Fxfer.SoapFacade.CoreWcf.Contracts.Events;
using Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;
using Fxfer.SoapFacade.CoreWcf.Contracts.Services;

namespace Fxfer.SoapFacade.Host.Soap.Services;

public sealed class FileTransferSvcService : IFileTransferSvcService
{
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public bool IsServiceAvailable() => true;

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public bool IsServiceHealthy(string tenantId) => true;

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public FtsSourceLocationAdded FileTransferAdd(FtsSourceLocationAddCommand command) => NotYetWired<FtsSourceLocationAdded>();

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    public FtsSourceLocationUpdated UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand command) => NotYetWired<FtsSourceLocationUpdated>();

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    public FtsSourceLocationInvalidated InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand command) => NotYetWired<FtsSourceLocationInvalidated>();

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    public FtpServerAdded AddFtpServer(AddFtpServerCommand command) => NotYetWired<FtpServerAdded>();

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    public FtpServerUpdated UpdateFtpServer(UpdateFtpServerCommand command) => NotYetWired<FtpServerUpdated>();

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    public FtpServerInvalidated InvalidateFtpServer(InvalidateFtpServerCommand command) => NotYetWired<FtpServerInvalidated>();

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    [FaultContract(typeof(BusinessValidationException))]
    public IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command) => NotYetWired<IntegrationFileUpdated>();

    private static T NotYetWired<T>()
    {
        throw new FaultException<ServiceException>(
            new ServiceException
            {
                ErrorCode = StatusCodeType.Error,
                ErrorMessages = ["SOAP facade skeleton: operation not yet wired to fxfer-domain-api"]
            },
            new FaultReason("Service not implemented"));
    }
}

