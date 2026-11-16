namespace FxferService.Application.LegacyContracts;

public interface IModuleConfigurator
{
    void ConfigureContractAssemblies(IDataContractRegistry contractRegistry);
    void ConfigureDependencies(IUnityContainer container);
    void ConfigureEvents(IDistributableEventRegistry registry);
}

