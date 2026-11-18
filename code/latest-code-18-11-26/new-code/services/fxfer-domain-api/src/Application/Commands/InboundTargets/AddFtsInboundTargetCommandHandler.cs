using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using HPP.Core.BAS.CQRS.UserMeta;
using MediatR;

namespace Fxfer.DomainApi.Application.Commands.InboundTargets;

public sealed class AddFtsInboundTargetCommandHandler : IRequestHandler<AddFtsInboundTargetCommand, FxferDomainEvent<FxferConfigurationChangedV1>?>
{
    private readonly IFileTransferReadOnlyRepository _readOnlyRepository;
    private readonly FxferDbContext _dbContext;
    private readonly IFxferDomainEventFactory _eventFactory;
    private readonly OutboxMessageWriter _outboxWriter;

    public AddFtsInboundTargetCommandHandler(
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

    public async Task<FxferDomainEvent<FxferConfigurationChangedV1>?> Handle(AddFtsInboundTargetCommand command, CancellationToken cancellationToken)
    {
        var existing = await _readOnlyRepository.GetInboundTargetAsync(
            BuildInboundTargetQuery(command.FTEMetadataId, command.Requestor!),
            cancellationToken);
        if (existing is not null && existing.Count > 0)
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;
        var inboundTargetId = Guid.NewGuid();

        var inboundTargetAdded = BuildAddFtsInboundTargetEvent(command, inboundTargetId);

        _dbContext.FtsInboundTarget.Add(new FtsInboundTarget
        {
            FtsInboundTargetId = inboundTargetAdded.FtsInboundTargetId,
            FTEMetadataId = inboundTargetAdded.FTEMetadataId,
            ImportJobName = inboundTargetAdded.ImportJobName!,
            ContractName = inboundTargetAdded.ContractName!,
            IsValid = inboundTargetAdded.IsValid,
            OperatorId = inboundTargetAdded.OperatorId!,
            CreationDate = now,
            LastModifiedDate = now
        });

        var payload = new FxferConfigurationChangedV1
        {
            AggregateType = "inbound_target",
            AggregateId = inboundTargetId,
            ChangeKind = "created",
            OperatorId = inboundTargetAdded.OperatorId,
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

    private static FtsInboundTargetAdded BuildAddFtsInboundTargetEvent(AddFtsInboundTargetCommand command, Guid inboundTargetId)
    {
        return new FtsInboundTargetAdded
        {
            FtsInboundTargetId = inboundTargetId,
            FTEMetadataId = command.FTEMetadataId,
            ImportJobName = command.ImportJobName,
            ContractName = command.ContractName,
            IsValid = true,
            OperatorId = command.Requestor!.IdentifierId
        };
    }
}

