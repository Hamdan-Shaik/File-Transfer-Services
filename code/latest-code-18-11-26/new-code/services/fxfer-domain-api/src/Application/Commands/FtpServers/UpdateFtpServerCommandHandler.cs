using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Domain.Constants;
using Fxfer.DomainApi.Domain.Entities;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Application.Commands.FtpServers;

public sealed class UpdateFtpServerCommandHandler : IRequestHandler<UpdateFtpServerCommand, FxferDomainEvent<FxferConfigurationChangedV1>>
{
    private readonly IFileTransferReadOnlyRepository _readOnlyRepository;
    private readonly FxferDbContext _dbContext;
    private readonly IFxferDomainEventFactory _eventFactory;
    private readonly OutboxMessageWriter _outboxWriter;

    public UpdateFtpServerCommandHandler(
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

    public async Task<FxferDomainEvent<FxferConfigurationChangedV1>> Handle(UpdateFtpServerCommand command, CancellationToken cancellationToken)
    {
        command.Validate();

        // F02-031: reject duplicate name among valid FTP servers.
        var query = new FtpServerQuery
        {
            Where = new FtpServerQueryParms
            {
                IsValid = true,
                FtpServerName = command.FtpServerName
            }
        };

        IReadOnlyList<Fxfer.DomainApi.Contracts.Domain.FtpServerModel> ftpServers = await _readOnlyRepository.GetFtpServerAsync(query, cancellationToken);
        if (ftpServers.Any(f => f.FtpServerId != command.FtpServerId))
        {
            command.AddValidation(
                command.FtpServerName!,
                CommonConstants.Validation.ErrorType.DuplicatedFtpServerName,
                CommonConstants.Validation.Messages.DuplicatedFtpServerName);
            command.HandleValidations();
        }

        var now = DateTimeOffset.UtcNow;

        var existing = await _dbContext.FtpServers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FtpServerId == command.FtpServerId, cancellationToken);

        var passwordSecretRef = !string.IsNullOrEmpty(command.FtpPassword) ? command.FtpPassword : existing?.FtpPasswordSecretRef;

        if (string.IsNullOrEmpty(passwordSecretRef))
        {
            command.AddValidation(
                nameof(UpdateFtpServerCommand.FtpPassword),
                CommonConstants.Validation.ErrorType.RequiredFields,
                string.Format(
                    "{0}: {1}",
                    CommonConstants.Validation.Messages.MissingField,
                    nameof(UpdateFtpServerCommand.FtpPassword)));
            command.HandleValidations();
        }

        var entity = new FtpServer
        {
            FtpServerId = command.FtpServerId,
            FtpServerName = command.FtpServerName!,
            FtpDescription = command.FtpDescription!,
            FtpServerHost = command.FtpServerIp!,
            FtpTransferMode = command.FtpTransferMode!,
            FtpUser = command.FtpUser!,
            FtpPasswordSecretRef = passwordSecretRef,
            FtpPort = command.FtpPort,
            FtpProxyAddress = command.FtpProxyAddress,
            FtpProxyPort = command.FtpProxyPort,
            UseSshAuth = command.UseSSHAuth,
            IsValid = command.IsValid,
            // F02-033: UpdateFtpServer sets OperatorId from the requestor identifier.
            OperatorId = command.Requestor!.IdentifierId,
            CreationDate = existing?.CreationDate ?? now,
            LastModifiedDate = now
        };

        _dbContext.FtpServers.Update(entity);

        var payload = new FxferConfigurationChangedV1
        {
            AggregateType = "ftp_server",
            AggregateId = command.FtpServerId,
            ChangeKind = "updated",
            // F02-033: UpdateFtpServer sets OperatorId from the requestor identifier.
            OperatorId = command.Requestor!.IdentifierId,
            ChangedAt = now
        };

        var domainEvent = _eventFactory.Create(FxferEventMetadata.ConfigurationChangedEventName, FxferEventMetadata.V1, payload, now);
        _outboxWriter.Enqueue(domainEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return domainEvent;
    }
}

