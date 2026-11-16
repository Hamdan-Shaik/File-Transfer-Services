namespace FxferService.Application.LegacyContracts;

public interface IUnityContainer
{
    void RegisterType<TFrom, TTo>() where TTo : TFrom;
    void RegisterType<T>(TransientLifetimeManager lifetimeManager);
}

