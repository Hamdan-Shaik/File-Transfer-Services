using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using MediatR;

namespace Fxfer.DomainApi.Application.Commands.IntegrationFiles;

public sealed class AddIntegrationFileCommandHandler : IRequestHandler<AddIntegrationFileCommand, FxferDomainEvent<FxferConfigurationChangedV1>?>
{
    private readonly FxferDbContext _dbContext;
    private readonly IFxferDomainEventFactory _eventFactory;
    private readonly OutboxMessageWriter _outboxWriter;

    public AddIntegrationFileCommandHandler(
        FxferDbContext dbContext,
        IFxferDomainEventFactory eventFactory,
        OutboxMessageWriter outboxWriter)
    {
        _dbContext = dbContext;
        _eventFactory = eventFactory;
        _outboxWriter = outboxWriter;
    }

    public async Task<FxferDomainEvent<FxferConfigurationChangedV1>?> Handle(AddIntegrationFileCommand command, CancellationToken cancellationToken)
    {
        // Legacy parity: AddIntegrationFileCommand is not validated.
        if (command.GroupInfo is null || !command.GroupInfo.Any(e => e.NumberOfRecords > 0 && e.GroupId is not null))
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;

        FxferDomainEvent<FxferConfigurationChangedV1>? firstEvent = null;

        foreach (var groupInfo in command.GroupInfo.Where(e => e.NumberOfRecords > 0))
        {
            var integrationFileId = Guid.NewGuid();

            _dbContext.FtsIntegrationFile.Add(new FtsIntegrationFile
            {
                FtsIntegrationFileId = integrationFileId,
                GroupId = groupInfo.GroupId,
                ConfigId = null,
                FileName = command.FileName!,
                ETFOutboundMetadataDefnId = command.ETFOutboundMetadataDefnId,
                IsProcessed = false,
                IsValid = true,
                OperatorId = command.Requestor!.IdentifierId,
                CreationDate = now,
                LastModifiedDate = now
            });

            var payload = new FxferConfigurationChangedV1
            {
                AggregateType = "integration_file",
                AggregateId = integrationFileId,
                ChangeKind = "created",
                OperatorId = command.Requestor!.IdentifierId,
                ChangedAt = now
            };

            var domainEvent = _eventFactory.Create(FxferEventMetadata.ConfigurationChangedEventName, FxferEventMetadata.V1, payload, now);
            _outboxWriter.Enqueue(domainEvent);

            firstEvent ??= domainEvent;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return firstEvent;
    }
}

