using System.Reflection;

namespace FxferService.Application.LegacyContracts;

internal static class Mapper
{
    public static void Initialize(Action<MapperConfigurationExpression> configure)
    {
        configure(new MapperConfigurationExpression());
    }
}

internal sealed class MapperConfigurationExpression
{
    public void DetectAndRegisterProfiles(Assembly assembly)
    {
        ProfilesRegistry.RegisterProfilesFrom(assembly);
    }
}

internal static class ProfilesRegistry
{
    private static readonly HashSet<Assembly> _assemblies = new();

    public static void RegisterProfilesFrom(Assembly assembly)
    {
        _assemblies.Add(assembly);
    }
}

