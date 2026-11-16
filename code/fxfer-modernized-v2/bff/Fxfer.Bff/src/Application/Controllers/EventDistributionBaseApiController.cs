using Fxfer.Bff.Application.LegacyContracts;
using Microsoft.AspNetCore.Mvc;

namespace Fxfer.Bff.Application.Controllers;

// shortcut: legacy EventDistributionBaseApiController behavior lives in an external CQRS package not present in the legacy repo; this base preserves only dependency wiring.
public abstract class EventDistributionBaseApiController : ControllerBase
{
    protected EventDistributionBaseApiController(IEventDistribution eventDistribution)
    {
        this.EventDistribution = eventDistribution;
    }

    protected IEventDistribution EventDistribution { get; }
}
