namespace FxferService.Application.LegacyContracts;

public abstract class EventDistributionServiceBase
{
    protected Event TryRoute(Command command)
    {
        return new Event(command);
    }

    protected abstract IModuleConfigurator GetConfigurator();
}

