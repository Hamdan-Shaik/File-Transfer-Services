using CoreWCF;
using Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;
using Fxfer.SoapFacade.CoreWcf.Contracts.Utilities;

namespace Fxfer.SoapFacade.CoreWcf.Contracts.Services;

[ServiceContract]
public interface IEventDistribution
{
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    Event TryRouteCommand(Command command);
}

