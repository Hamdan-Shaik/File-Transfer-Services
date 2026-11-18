using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Application.Commands.IntegrationFiles;

public sealed class UpdateIntegrationFileCommandHandler : IRequestHandler<UpdateIntegrationFileCommand, FxferDomainEvent<FxferConfigurationChangedV1>>
{
    private readonly FxferDbContext _dbContext;
    private readonly IFxferDomainEventFactory _eventFactory;
    private readonly OutboxMessageWriter _outboxWriter;

    public UpdateIntegrationFileCommandHandler(
        FxferDbContext dbContext,
        IFxferDomainEventFactory eventFactory,
        OutboxMessageWriter outboxWriter)
    {
        _dbContext = dbContext;
        _eventFactory = eventFactory;
        _outboxWriter = outboxWriter;
    }

    public async Task<FxferDomainEvent<FxferConfigurationChangedV1>> Handle(UpdateIntegrationFileCommand command, CancellationToken cancellationToken)
    {
        // F03-011: update validates before emitting the event.
        command.Validate();

        var now = DateTimeOffset.UtcNow;

        var existing = await _dbContext.FtsIntegrationFile
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FtsIntegrationFileId == command.FtsIntegrationFileId, cancellationToken);

        if (existing is not null)
        {
            var tracked = _dbContext.ChangeTracker
                .Entries<FtsIntegrationFile>()
                .FirstOrDefault(e => e.Entity.FtsIntegrationFileId == command.FtsIntegrationFileId);

            if (tracked is not null)
            {
                tracked.State = EntityState.Detached;
            }

            _dbContext.FtsIntegrationFile.Update(existing with
            {
                ConfigId = command.ConfigId,
                IsProcessed = true,
                IsValid = command.IsValid,
                LastModifiedDate = now,
                OperatorId = command.Requestor!.IdentifierId
            });
        }

        var payload = new FxferConfigurationChangedV1
        {
            AggregateType = "integration_file",
            AggregateId = command.FtsIntegrationFileId,
            ChangeKind = "updated",
            OperatorId = command.Requestor!.IdentifierId,
            ChangedAt = now
        };

        var domainEvent = _eventFactory.Create(FxferEventMetadata.ConfigurationChangedEventName, FxferEventMetadata.V1, payload, now);
        _outboxWriter.Enqueue(domainEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return domainEvent;
    }
}

