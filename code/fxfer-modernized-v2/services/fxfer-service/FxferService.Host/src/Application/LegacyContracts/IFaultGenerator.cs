namespace FxferService.Application.LegacyContracts;

public interface IFaultGenerator
{
    FaultException? GenerateFault();
}

