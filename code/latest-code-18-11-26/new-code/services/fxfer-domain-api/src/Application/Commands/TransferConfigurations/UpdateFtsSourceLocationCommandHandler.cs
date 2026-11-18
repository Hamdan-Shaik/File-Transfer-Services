using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Exceptions;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using HPP.Core.BAS.CQRS.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Application.Commands.TransferConfigurations;

public sealed class UpdateFtsSourceLocationCommandHandler : IRequestHandler<FtsSourceLocationUpdateCommand, FxferDomainEvent<FxferConfigurationChangedV1>>
{
    private readonly FxferDbContext _dbContext;
    private readonly IFxferDomainEventFactory _eventFactory;
    private readonly OutboxMessageWriter _outboxWriter;

    public UpdateFtsSourceLocationCommandHandler(
        FxferDbContext dbContext,
        IFxferDomainEventFactory eventFactory,
        OutboxMessageWriter outboxWriter)
    {
        _dbContext = dbContext;
        _eventFactory = eventFactory;
        _outboxWriter = outboxWriter;
    }

    public async Task<FxferDomainEvent<FxferConfigurationChangedV1>> Handle(FtsSourceLocationUpdateCommand command, CancellationToken cancellationToken)
    {
        command.Validate();

        var existing = await _dbContext.FtsSourceLocations
            .FirstOrDefaultAsync(x => x.ConfigId == command.ConfigId, cancellationToken);

        if (existing is null)
        {
            // F01-069: missing ConfigId throws legacy BusinessException.
            ThrowConfigIdNotFound();
        }

        var now = DateTimeOffset.UtcNow;
        var sourceLocationId = existing!.FtsSourceLocationId;

        // F01-071: update uses the stored source location id/config id, not the inbound id.
        existing.ConfigName = command.ConfigName!;
        existing.Description = command.Description;
        existing.FtpServerId = string.IsNullOrEmpty(command.FtpServerId) ? null : Guid.Parse(command.FtpServerId);
        existing.SourceProtocolCD = command.Protocol!;
        existing.SourceAddress = command.Address!;
        existing.SourceFileMask = command.Mask!;
        existing.FileDirection = command.FileDirection;
        existing.SendNotificationsInd = command.SendNotifications;
        existing.IsSuspended = command.IsSuspended;
        existing.IsSuspendedChangedTs = ToUtcOffset(command.IsSuspendedChangedDate);
        existing.IsValid = command.IsValid;
        existing.OwnerName = command.OwnerName!;
        existing.OwnerModule = Guid.Parse(command.OwnerModule!);
        existing.OwnerPhone = command.OwnerPhone;
        existing.OwnerEmail = command.OwnerEmail;
        existing.RunOnMondayInd = command.RunOnMonday;
        existing.RunOnTuesdayInd = command.RunOnTuesday;
        existing.RunOnWednesdayInd = command.RunOnWed;
        existing.RunOnThursdayInd = command.RunOnThrus;
        existing.RunOnFridayInd = command.RunOnFri;
        existing.RunOnSaturdayInd = command.RunOnSat;
        existing.RunOnSundayInd = command.RunOnSun;
        existing.RunOnFirstDayOfMonthInd = command.RunOnFrstDayOfMonthInd;
        existing.RunOnLastDayOfMonthInd = command.RunOnLastDayOfMonthInd;
        existing.RunOnThisDayOfMonth = (short)command.RunOnThisDayOfMonth;
        existing.StartTime24HHMM = command.StartTime!;
        existing.EndTime24HHMM = command.EndTime!;
        existing.TradingPartner = command.TradingPartner;
        existing.OperatorId = command.OperatorId!;
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
                    _dbContext.FtsDestinationLocations.Add(new FtsDestinationLocations
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

            if (!command.IsValid)
            {
                var destinations = await _dbContext.FtsDestinationLocations
                    .AsNoTracking()
                    .Where(x => x.FtsSourceLocationId == sourceLocationId)
                    .ToListAsync(cancellationToken);

                foreach (var dest in destinations)
                {
                    _dbContext.FtsDestinationLocations.Update(dest with { IsValid = false, OperatorId = command.OperatorId! });
                }
            }
        }

        var payload = new FxferConfigurationChangedV1
        {
            AggregateType = "transfer_configuration",
            AggregateId = sourceLocationId,
            ChangeKind = "updated",
            OperatorId = command.OperatorId,
            ChangedAt = now
        };

        var domainEvent = _eventFactory.Create(FxferEventMetadata.ConfigurationChangedEventName, FxferEventMetadata.V1, payload, now);
        _outboxWriter.Enqueue(domainEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return domainEvent;
    }

    private static DateTimeOffset? ToUtcOffset(DateTime? value)
    {
        return value.HasValue
            ? new DateTimeOffset(DateTime.SpecifyKind(value.Value, DateTimeKind.Utc))
            : null;
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

