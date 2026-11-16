using System.Reflection;

namespace FxferService.Application.LegacyContracts;

public sealed class DataContractRegistry : IDataContractRegistry
{
    private readonly HashSet<Assembly> _assemblies = new();

    public IReadOnlyCollection<Assembly> Assemblies => _assemblies;

    public void RegisterContractAssembly(Assembly assembly)
    {
        _assemblies.Add(assembly);
    }
}

public sealed class DistributableEventRegistry : IDistributableEventRegistry
{
    private readonly HashSet<Type> _eventTypes = new();

    public IReadOnlyCollection<Type> EventTypes => _eventTypes;

    public void Register<TEvent>() where TEvent : Event
    {
        _eventTypes.Add(typeof(TEvent));
    }
}

public sealed class DefaultModuleConfigurator : IModuleConfigurator
{
    public void ConfigureContractAssemblies(IDataContractRegistry contractRegistry)
    {
    }

    public void ConfigureDependencies(IUnityContainer container)
    {
    }

    public void ConfigureEvents(IDistributableEventRegistry registry)
    {
    }
}

public sealed class ServiceAvailability : IServiceAvailability
{
}

public sealed class UnityContainerShim : IUnityContainer
{
    private readonly Dictionary<Type, Type> _registrations = new();

    public IReadOnlyDictionary<Type, Type> Registrations => _registrations;

    public void RegisterType<TFrom, TTo>() where TTo : TFrom
    {
        _registrations[typeof(TFrom)] = typeof(TTo);
    }

    public void RegisterType<T>(TransientLifetimeManager lifetimeManager)
    {
        _ = lifetimeManager;
        _registrations[typeof(T)] = typeof(T);
    }
}