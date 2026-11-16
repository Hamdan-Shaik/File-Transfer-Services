namespace FxferService.Domain.Contracts.Batch;

public class FtsSuspensionSearchCriteria
{
    /// <summary>
    /// Config Id
    /// </summary>
    public Guid SourceLocationId { get; set; }

    /// <summary>
    /// Error Text for Suspension Search
    /// </summary>
    public string? ErrorText { get; set; }

    /// <summary>
    /// StackTrace for Suspension Search
    /// </summary>
    public string? StackTrace { get; set; }
}

