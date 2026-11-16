using Fxfer.Bff.Application.LegacyContracts;

namespace Fxfer.Bff.Application.Controllers;

public class FileTransferSvcEventServiceController : EventDistributionBaseApiController
{
    private readonly IEventDistribution eventDistribution;

    public FileTransferSvcEventServiceController(IEventDistribution eventDistribution)
        : base(eventDistribution)
    {
        this.eventDistribution = eventDistribution;
    }

    private IEventDistribution EventDistributionDependency => this.eventDistribution;
}
