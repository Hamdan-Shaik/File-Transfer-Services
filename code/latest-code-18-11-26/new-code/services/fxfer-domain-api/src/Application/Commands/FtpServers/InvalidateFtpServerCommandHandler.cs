using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Domain.Entities;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Application.Commands.FtpServers;

public sealed class InvalidateFtpServerCommandHandler : IRequestHandler<InvalidateFtpServerCommand, FxferDomainEvent<FxferConfigurationChangedV1>>
{
    private readonly FxferDbContext _dbContext;
    private readonly IFxferDomainEventFactory _eventFactory;
    private readonly OutboxMessageWriter _outboxWriter;

    public InvalidateFtpServerCommandHandler(
        FxferDbContext dbContext,
        IFxferDomainEventFactory eventFactory,
        OutboxMessageWriter outboxWriter)
    {
        _dbContext = dbContext;
        _eventFactory = eventFactory;
        _outboxWriter = outboxWriter;
    }

    public async Task<FxferDomainEvent<FxferConfigurationChangedV1>> Handle(InvalidateFtpServerCommand command, CancellationToken cancellationToken)
    {
        // F02-034: invalidate does not call command.Validate().

        var now = DateTimeOffset.UtcNow;

        var existing = await _dbContext.FtpServers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FtpServerId == command.FtpServerId, cancellationToken);

        if (existing is not null)
        {
            _dbContext.FtpServers.Update(existing with
            {
                IsValid = false,
                OperatorId = command.Requestor!.IdentifierId,
                LastModifiedDate = now
            });
        }

        var payload = new FxferConfigurationChangedV1
        {
            AggregateType = "ftp_server",
            AggregateId = command.FtpServerId,
            ChangeKind = "invalidated",
            OperatorId = command.Requestor!.IdentifierId,
            ChangedAt = now
        };

        var domainEvent = _eventFactory.Create(FxferEventMetadata.ConfigurationChangedEventName, FxferEventMetadata.V1, payload, now);
        _outboxWriter.Enqueue(domainEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return domainEvent;
    }
}

