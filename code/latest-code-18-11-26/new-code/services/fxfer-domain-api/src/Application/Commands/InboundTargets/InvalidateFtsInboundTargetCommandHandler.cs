using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using HPP.Core.BAS.CQRS.UserMeta;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Application.Commands.InboundTargets;

public sealed class InvalidateFtsInboundTargetCommandHandler : IRequestHandler<InvalidateFtsInboundTargetCommand, FxferDomainEvent<FxferConfigurationChangedV1>?>
{
    private readonly IFileTransferReadOnlyRepository _readOnlyRepository;
    private readonly FxferDbContext _dbContext;
    private readonly IFxferDomainEventFactory _eventFactory;
    private readonly OutboxMessageWriter _outboxWriter;

    public InvalidateFtsInboundTargetCommandHandler(
        IFileTransferReadOnlyRepository readOnlyRepository,
        FxferDbContext dbContext,
        IFxferDomainEventFactory eventFactory,
        OutboxMessageWriter outboxWriter)
    {
        _readOnlyRepository = readOnlyRepository;
        _dbContext = dbContext;
        _eventFactory = eventFactory;
        _outboxWriter = outboxWriter;
    }

    public async Task<FxferDomainEvent<FxferConfigurationChangedV1>?> Handle(InvalidateFtsInboundTargetCommand command, CancellationToken cancellationToken)
    {
        var inboundTargets = await _readOnlyRepository.GetInboundTargetAsync(
            BuildInboundTargetQuery(command.FTEMetadataId, command.Requestor!),
            cancellationToken);
        if (inboundTargets is null || inboundTargets.Count == 0)
        {
            return null;
        }

        var inboundTargetInvalidated = BuildInvalidateFtsInboundTargetEvent(inboundTargets);
        var existingId = inboundTargetInvalidated.FtsInboundTargetId;
        var now = DateTimeOffset.UtcNow;

        var existing = await _dbContext.FtsInboundTarget
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FtsInboundTargetId == existingId, cancellationToken);

        if (existing is not null)
        {
            _dbContext.FtsInboundTarget.Update(existing with
            {
                IsValid = inboundTargetInvalidated.IsValid,
                OperatorId = command.Requestor!.IdentifierId,
                LastModifiedDate = now
            });
        }

        var payload = new FxferConfigurationChangedV1
        {
            AggregateType = "inbound_target",
            AggregateId = existingId,
            ChangeKind = "invalidated",
            OperatorId = command.Requestor!.IdentifierId,
            ChangedAt = now
        };

        var domainEvent = _eventFactory.Create(FxferEventMetadata.ConfigurationChangedEventName, FxferEventMetadata.V1, payload, now);
        _outboxWriter.Enqueue(domainEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return domainEvent;
    }

    private static InboundTargetQuery BuildInboundTargetQuery(Guid fteMetadataId, RequestorModel requestor)
    {
        return new InboundTargetQuery
        {
            Where = new InboundTargetQueryParms { FTEMetadataId = fteMetadataId },
            Requestor = requestor
        };
    }

    private static FtsInboundTargetInvalidated BuildInvalidateFtsInboundTargetEvent(IReadOnlyList<FtsInboundTargetModel> inboundTargets)
    {
        return new FtsInboundTargetInvalidated
        {
            FtsInboundTargetId = inboundTargets.First().FtsInboundTargetId,
            IsValid = false
        };
    }
}

