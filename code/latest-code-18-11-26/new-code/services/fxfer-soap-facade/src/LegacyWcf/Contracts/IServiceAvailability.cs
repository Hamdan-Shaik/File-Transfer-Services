using CoreWCF;
using Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;

namespace Fxfer.SoapFacade.LegacyWcf.Contracts;

public interface IServiceAvailability
{
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    bool IsServiceAvailable();

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    bool IsServiceHealthy(string tenantId);
}
