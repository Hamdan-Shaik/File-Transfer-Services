namespace FxferService.Application.LegacyContracts;

public abstract record Command
{
    public RequestorModel? Requestor { get; init; }
}

