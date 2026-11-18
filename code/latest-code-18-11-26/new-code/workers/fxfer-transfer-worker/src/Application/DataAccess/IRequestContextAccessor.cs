namespace Fxfer.TransferWorker.Application.DataAccess;

public interface IRequestContextAccessor
{
    string? CorrelationId { get; }

    string? TenantId { get; }
}

