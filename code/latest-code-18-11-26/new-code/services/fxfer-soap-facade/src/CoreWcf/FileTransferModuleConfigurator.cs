using Microsoft.Extensions.DependencyInjection;

namespace Fxfer.SoapFacade.CoreWcf;

public sealed class FileTransferModuleConfigurator
{
    public void ConfigureContractAssemblies(object contractRegistry)
    {
    }

    public void ConfigureDependencies(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // drop: legacy Autofac registrations for CQRS handlers/projection writers.
        // Target-state routes SOAP calls to fxfer-domain-api; persistence/projection processing is out-of-proc.
    }

    public void ConfigureEvents(object registry)
    {
    }
}
