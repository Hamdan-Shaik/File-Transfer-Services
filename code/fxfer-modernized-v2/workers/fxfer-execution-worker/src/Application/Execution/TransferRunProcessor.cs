using System.Security.Cryptography;
using System.Text;
using Fxfer.ExecutionWorker.Infrastructure.DomainApi;

namespace Fxfer.ExecutionWorker;

public sealed class TransferRunProcessor
{
    private readonly FxferServiceClient _fxferService;
    private readonly ILogger<TransferRunProcessor> _logger;

    public TransferRunProcessor(FxferServiceClient fxferService, ILogger<TransferRunProcessor> logger)
    {
        _fxferService = fxferService;
        _logger = logger;
    }

    public async Task ProcessAsync(string tenantId, Guid runId, CancellationToken cancellationToken)
    {
        // T-005: derive a stable idempotency key so retries/redrives do not create duplicate authoritative outcomes.
        var idempotencyKey = DeriveIdempotencyKey(tenantId, runId, attemptId: null, stepId: null);

        // Skeleton: execution context fetching, protocol I/O, and authoritative outcome recording are added later.
        _logger.LogInformation("Accepted TransferRunRequested work item");
        await _fxferService.RecordSkeletonOutcomeAsync(runId, idempotencyKey, cancellationToken);
    }

    public static string DeriveIdempotencyKey(string tenantId, Guid runId, string? attemptId, string? stepId)
    {
        var input = string.Join('|', new[]
        {
            tenantId ?? string.Empty,
            runId.ToString("D"),
            attemptId ?? string.Empty,
            stepId ?? string.Empty,
        });

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
