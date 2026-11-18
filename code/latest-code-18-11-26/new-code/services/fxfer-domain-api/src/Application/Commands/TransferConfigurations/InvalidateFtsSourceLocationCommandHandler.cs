using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Exceptions;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using HPP.Core.BAS.CQRS.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Application.Commands.TransferConfigurations;

public sealed class InvalidateFtsSourceLocationCommandHandler : IRequestHandler<FtsSourceLocationInvalidateCommand, FxferDomainEvent<FxferConfigurationChangedV1>>
{
    private readonly FxferDbContext _dbContext;
    private readonly IFxferDomainEventFactory _eventFactory;
    private readonly OutboxMessageWriter _outboxWriter;

    public InvalidateFtsSourceLocationCommandHandler(
        FxferDbContext dbContext,
        IFxferDomainEventFactory eventFactory,
        OutboxMessageWriter outboxWriter)
    {
        _dbContext = dbContext;
        _eventFactory = eventFactory;
        _outboxWriter = outboxWriter;
    }

    public async Task<FxferDomainEvent<FxferConfigurationChangedV1>> Handle(FtsSourceLocationInvalidateCommand command, CancellationToken cancellationToken)
    {
        command.Validate();

        var existing = await _dbContext.FtsSourceLocations
            .FirstOrDefaultAsync(x => x.ConfigId == command.ConfigId, cancellationToken);

        if (existing is null)
        {
            // F01-070: missing ConfigId throws legacy BusinessException.
            ThrowConfigIdNotFound();
        }

        var now = DateTimeOffset.UtcNow;

        // F01-072: invalidate uses the stored source location id/config id, not the inbound FtsSourceLocationId.
        var sourceLocationId = existing!.FtsSourceLocationId;

        existing.OperatorId = command.OperatorId!;
        existing.IsValid = false;
        existing.LastModifiedDate = now;

        if (command.DestinationAddress is not null)
        {
            foreach (var dest in command.DestinationAddress)
            {
                var destination = await _dbContext.FtsDestinationLocations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.DestinationId == dest.DestinationId, cancellationToken);

                if (destination is not null)
                {
                    _dbContext.FtsDestinationLocations.Update(destination with
                    {
                        DestinationAddress = dest.DestinationAddress!,
                        DestProtocolCD = dest.DestinationProtocol!,
                        OverwriteAtDest = dest.OverWriteAtDestination,
                        CompressAtDest = dest.CompressAtDestination,
                        RenameAtDest = dest.RenameAtDestination,
                        DestIsArchive = dest.DestinationArchive,
                        LastModifiedDate = now,
                        IsValid = dest.IsValid,
                        FtpServerId = dest.FtpServerId,
                        OperatorId = dest.OperatorId ?? command.OperatorId!,
                        RenamePattern = dest.RenamePattern,
                        ContactName = dest.ContactName,
                        ContactPhone = dest.ContactPhone,
                        ContactEmail = dest.ContactEmail,
                        FTEMetadataId = dest.FTEMetadataId,
                        DestinationType = dest.DestinationType,
                        ContractName = dest.ContractName
                    });
                }
                else
                {
                    _dbContext.FtsDestinationLocations.Add(new Fxfer.DomainApi.Infrastructure.DataAccess.Entities.FtsDestinationLocations
                    {
                        DestinationId = Guid.NewGuid(),
                        FtsSourceLocationId = sourceLocationId,
                        DestinationAddress = dest.DestinationAddress!,
                        DestProtocolCD = dest.DestinationProtocol!,
                        OverwriteAtDest = dest.OverWriteAtDestination,
                        CompressAtDest = dest.CompressAtDestination,
                        RenameAtDest = dest.RenameAtDestination,
                        DestIsArchive = dest.DestinationArchive,
                        CreationDate = now,
                        LastModifiedDate = now,
                        IsValid = dest.IsValid,
                        FtpServerId = dest.FtpServerId,
                        OperatorId = dest.OperatorId ?? command.OperatorId!,
                        RenamePattern = dest.RenamePattern,
                        ContactName = dest.ContactName,
                        ContactPhone = dest.ContactPhone,
                        ContactEmail = dest.ContactEmail,
                        FTEMetadataId = dest.FTEMetadataId,
                        DestinationType = dest.DestinationType,
                        ContractName = dest.ContractName,
                        CdeEncryption = null,
                        EncryptionPublicKey = null,
                        EncryptionPrivateKeySecretRef = null,
                        PrivateKeyPasswordSecretRef = null
                    });
                }
            }
        }

        var destinations = await _dbContext.FtsDestinationLocations
            .AsNoTracking()
            .Where(x => x.FtsSourceLocationId == sourceLocationId)
            .ToListAsync(cancellationToken);

        foreach (var dest in destinations)
        {
            _dbContext.FtsDestinationLocations.Update(dest with { IsValid = false, OperatorId = command.OperatorId! });
        }

        var payload = new FxferConfigurationChangedV1
        {
            AggregateType = "transfer_configuration",
            AggregateId = sourceLocationId,
            ChangeKind = "invalidated",
            OperatorId = command.OperatorId,
            ChangedAt = now
        };

        var domainEvent = _eventFactory.Create(FxferEventMetadata.ConfigurationChangedEventName, FxferEventMetadata.V1, payload, now);
        _outboxWriter.Enqueue(domainEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return domainEvent;
    }

    private static void ThrowConfigIdNotFound()
    {
        var exceptions = new List<BusinessExceptionMessage>
        {
            new(
                FileTransferExceptionConstants.Validation.ConfigId,
                FileTransferExceptionConstants.Validation.ConfigIdNotFound,
                FileTransferExceptionConstants.Validation.ProcessingSystem)
        };

        throw new BusinessException(
            exceptions,
            FileTransferExceptionConstants.Validation.Message,
            FileTransferExceptionConstants.Validation.ProcessingSystem);
    }
}

