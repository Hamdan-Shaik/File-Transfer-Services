using Fxfer.TransferWorker.Application.DataAccess;

namespace Fxfer.TransferWorker.Worker.Services;

public sealed class RequestContextAccessor : IRequestContextAccessor
{
    private readonly CorrelationIdContext _correlationIdContext;
    private readonly TenantIdContext _tenantIdContext;

    public RequestContextAccessor(CorrelationIdContext correlationIdContext, TenantIdContext tenantIdContext)
    {
        _correlationIdContext = correlationIdContext;
        _tenantIdContext = tenantIdContext;
    }

    public string? CorrelationId => _correlationIdContext.Get();

    public string? TenantId => _tenantIdContext.Get();
}

