namespace Fxfer.DomainApi.Contracts.Domain;

public sealed class FtsSuspensionModel
{
    public Guid SakFileTrack { get; set; }

    public Guid SuspensionId { get; set; }
}