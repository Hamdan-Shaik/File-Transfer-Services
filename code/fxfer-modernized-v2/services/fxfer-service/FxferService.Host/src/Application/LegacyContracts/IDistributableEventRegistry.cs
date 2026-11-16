namespace FxferService.Application.LegacyContracts;

public interface IDistributableEventRegistry
{
    void Register<TEvent>() where TEvent : Event;
}