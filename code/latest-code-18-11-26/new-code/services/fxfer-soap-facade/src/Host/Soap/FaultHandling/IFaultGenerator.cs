using CoreWCF;

namespace Fxfer.SoapFacade.Host.Soap.FaultHandling;

public interface IFaultGenerator
{
    FaultException? GenerateFault();
}
