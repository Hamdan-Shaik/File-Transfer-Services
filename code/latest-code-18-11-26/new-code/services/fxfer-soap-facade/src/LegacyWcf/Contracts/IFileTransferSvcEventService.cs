using CoreWCF;
using Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;
using Fxfer.SoapFacade.CoreWcf.Contracts.Utilities;

namespace Fxfer.SoapFacade.LegacyWcf.Contracts;

[ServiceContract]
public interface IFileTransferSvcEventService
{
    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    Event TryRouteCommand(Command command);
}
