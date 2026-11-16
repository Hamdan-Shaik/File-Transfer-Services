using System.Reflection;

namespace FxferService.Application.LegacyContracts;

public interface IDataContractRegistry
{
    void RegisterContractAssembly(Assembly assembly);
}