namespace FxferService.Infrastructure.Persistence.Batch.Contracts;

public sealed class FtsSuspensionModel
{
    public Guid SakFileTrack { get; set; }

    public Guid SuspensionId { get; set; }
}
