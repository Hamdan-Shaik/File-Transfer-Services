using Fxfer.SchedulerWorker.Infrastructure.DomainApi;
using Fxfer.SchedulerWorker.Infrastructure.Messaging.Publisher;
using Fxfer.SchedulerWorker.Infrastructure.Tenancy;

namespace Fxfer.SchedulerWorker.Application.Scheduling;

public sealed class DispatchLoop
{
    private readonly DueRunPlanner _planner;
    private readonly FxferServiceClient _fxferService;
    private readonly TransferRunRequestedPublisher _publisher;
    private readonly TenantContext _tenant;
    private readonly ILogger<DispatchLoop> _logger;

    public DispatchLoop(
        DueRunPlanner planner,
        FxferServiceClient fxferService,
        TransferRunRequestedPublisher publisher,
        TenantContext tenant,
        ILogger<DispatchLoop> logger)
    {
        _planner = planner;
        _fxferService = fxferService;
        _publisher = publisher;
        _tenant = tenant;
        _logger = logger;
    }

    public async Task DispatchAsync(DateTimeOffset nowUtc, CancellationToken cancellationToken)
    {
        var planned = await _planner.PlanDueRunsAsync(nowUtc, cancellationToken);
        if (planned.Count == 0)
        {
            return;
        }

        foreach (var item in planned)
        {
            _tenant.TenantId = item.TenantId;

            using var _ = _logger.BeginScope(new Dictionary<string, object?>
            {
                ["tenant_id"] = item.TenantId,
            });

            var intent = await _fxferService.TryCreateRunIntentAsync(item.TenantId, cancellationToken);
            if (intent is null)
            {
                continue;
            }

            _tenant.CorrelationId = intent.CorrelationId;
            _tenant.TraceId = intent.TraceId;

            await _publisher.PublishAsync(item.TenantId, intent.RunId, intent.CorrelationId, intent.TraceId, cancellationToken);
        }
    }
}
