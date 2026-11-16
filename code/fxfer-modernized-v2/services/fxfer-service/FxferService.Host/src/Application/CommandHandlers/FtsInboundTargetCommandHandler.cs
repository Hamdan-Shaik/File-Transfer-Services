using FxferService.Application.LegacyContracts;
using FxferService.Domain.Contracts.Events;
using FxferService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Application.CommandHandlers;

public sealed class FtsInboundTargetCommandHandler
{
    private readonly FxferDbContext _dbContext;

    public FtsInboundTargetCommandHandler(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FtsInboundTargetAdded?> HandleAsync(AddFtsInboundTargetCommand command, CancellationToken ct)
    {
        var existingId = await GetInboundTarget(command.FTEMetadataId).FirstOrDefaultAsync(ct);
        return existingId == null ? BuildAddFtsInboundTargetEvent(command) : null;
    }

    public async Task<FtsInboundTargetInvalidated?> HandleAsync(InvalidateFtsInboundTargetCommand command, CancellationToken ct)
    {
        var existingId = await GetInboundTarget(command.FTEMetadataId).FirstOrDefaultAsync(ct);
        return existingId == null ? null : BuildInvalidateFtsInboundTargetEvent(existingId.Value);
    }

    private IQueryable<Guid?> GetInboundTarget(Guid fteMetadataId)
    {
        return BuildInboundTargetQuery(fteMetadataId);
    }

    private IQueryable<Guid?> BuildInboundTargetQuery(Guid fteMetadataId)
    {
        // Mirrors UA3 legacy InboundTargetQueryExecutor:
        // - filters to IS_VALID rows
        // - applies FTEMetadataId filter only when it is non-empty
        // - groups by FTEMetadataId and selects the smallest inbound-target id per group
        return _dbContext.InboundTargets
            .AsNoTracking()
            .Where(x => x.IsValid)
            .Where(x => fteMetadataId == Guid.Empty || x.ExternalFileDefnId == fteMetadataId)
            .GroupBy(x => x.ExternalFileDefnId)
            .Select(g => (Guid?)g.OrderBy(x => x.FtsInboundTargetId).Select(x => x.FtsInboundTargetId).FirstOrDefault())
            ;
    }

    private static FtsInboundTargetAdded BuildAddEvent(AddFtsInboundTargetCommand command)
    {
        return new FtsInboundTargetAdded()
        {
            FTEMetadataId = command.FTEMetadataId,
            ImportJobName = command.ImportJobName,
            ContractName = command.ContractName,
            IsValid = true,
            OperatorId = command.Requestor!.IdentifierId,
        };
    }

    private static FtsInboundTargetAdded BuildAddFtsInboundTargetEvent(AddFtsInboundTargetCommand command)
    {
        return BuildAddEvent(command);
    }

    private static FtsInboundTargetInvalidated BuildInvalidateEvent(Guid ftsInboundTargetId)
    {
        return new FtsInboundTargetInvalidated()
        {
            FtsInboundTargetId = ftsInboundTargetId,
            IsValid = false,
        };
    }

    private static FtsInboundTargetInvalidated BuildInvalidateFtsInboundTargetEvent(Guid ftsInboundTargetId)
    {
        return BuildInvalidateEvent(ftsInboundTargetId);
    }
}
