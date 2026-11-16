namespace Fxfer.SchedulerWorker.Application.Scheduling;

public sealed record PlannedDispatch(string TenantId);

public sealed class DueRunPlanner
{
    public Task<IReadOnlyList<PlannedDispatch>> PlanDueRunsAsync(DateTimeOffset nowUtc, CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<PlannedDispatch>>(Array.Empty<PlannedDispatch>());
    }
}
