namespace FxferService.Domain.Contracts.Batch;

public class FtsSuspensionModel
{
    /// <summary>
    /// File Track Id
    /// </summary>
    public Guid SakFileTrack { get; set; }

    /// <summary>
    /// File Suspension Id
    /// </summary>
    public Guid SuspensionId { get; set; }
}

