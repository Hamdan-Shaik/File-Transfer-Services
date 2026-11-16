namespace FxferService.Domain.Constants;

public static class EventDistribution
{
    public const string AttemptingToDistributeEvent = "Attempting to distibute {0} event, CommitId: {1}";
    public const string DistributionOfEventCompleted = "Distribution of {0} event completed, CommitId: {1}";
}
