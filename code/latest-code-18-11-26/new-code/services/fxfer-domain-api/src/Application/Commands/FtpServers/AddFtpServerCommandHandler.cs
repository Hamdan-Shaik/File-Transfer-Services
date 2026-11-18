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

public sealed class AddFtpServerCommandHandler : IRequestHandler<AddFtpServerCommand, FxferDomainEvent<FxferConfigurationChangedV1>>
{
    private readonly IFileTransferReadOnlyRepository _readOnlyRepository;
    private readonly FxferDbContext _dbContext;
    private readonly IFxferDomainEventFactory _eventFactory;
    private readonly OutboxMessageWriter _outboxWriter;

    public AddFtpServerCommandHandler(
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

    public async Task<FxferDomainEvent<FxferConfigurationChangedV1>> Handle(AddFtpServerCommand command, CancellationToken cancellationToken)
    {
        // F02-023: command validation is executed before processing.
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

        // F02-032: add forces IsValid=true and OperatorId=requestor.
        var entity = new FtpServer
        {
            FtpServerId = command.FtpServerId,
            FtpServerName = command.FtpServerName!,
            FtpDescription = command.FtpDescription!,
            FtpServerHost = command.FtpServerIp!,
            FtpTransferMode = command.FtpTransferMode!,
            FtpUser = command.FtpUser!,
            FtpPasswordSecretRef = command.FtpPassword!,
            FtpPort = command.FtpPort,
            FtpProxyAddress = command.FtpProxyAddress,
            FtpProxyPort = command.FtpProxyPort,
            UseSshAuth = command.UseSSHAuth,
            IsValid = true,
            OperatorId = command.Requestor!.IdentifierId,
            CreationDate = now,
            LastModifiedDate = now
        };

        _dbContext.FtpServers.Add(entity);

        var payload = new FxferConfigurationChangedV1
        {
            AggregateType = "ftp_server",
            AggregateId = command.FtpServerId,
            ChangeKind = "created",
            OperatorId = command.Requestor!.IdentifierId,
            ChangedAt = now
        };

        var domainEvent = _eventFactory.Create(FxferEventMetadata.ConfigurationChangedEventName, FxferEventMetadata.V1, payload, now);
        _outboxWriter.Enqueue(domainEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return domainEvent;
    }
}

