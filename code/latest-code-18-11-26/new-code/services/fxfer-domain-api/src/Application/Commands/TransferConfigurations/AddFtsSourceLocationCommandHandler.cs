using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Exceptions;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Application.Commands.TransferConfigurations;

public sealed class AddFtsSourceLocationCommandHandler : IRequestHandler<FtsSourceLocationAddCommand, FxferDomainEvent<FxferConfigurationChangedV1>>
{
    private readonly FxferDbContext _dbContext;
    private readonly IFxferDomainEventFactory _eventFactory;
    private readonly OutboxMessageWriter _outboxWriter;

    public AddFtsSourceLocationCommandHandler(
        FxferDbContext dbContext,
        IFxferDomainEventFactory eventFactory,
        OutboxMessageWriter outboxWriter)
    {
        _dbContext = dbContext;
        _eventFactory = eventFactory;
        _outboxWriter = outboxWriter;
    }

    public async Task<FxferDomainEvent<FxferConfigurationChangedV1>> Handle(FtsSourceLocationAddCommand command, CancellationToken cancellationToken)
    {
        command.Validate();

        // F01-046: allocate ConfigId from the FTS_SOURCE_LOCATION_SEQ sequence.
        var configId = await AllocateConfigIdAsync(_dbContext, cancellationToken);

        var now = DateTimeOffset.UtcNow;
        var sourceLocationId = Guid.NewGuid();

        // F01-047: operator id comes from requestor identifier.
        var sourceLocation = new FtsSourceLocation
        {
            FtsSourceLocationId = sourceLocationId,
            ConfigId = configId,
            ConfigName = command.ConfigName!,
            Description = command.Description,
            FtpServerId = string.IsNullOrEmpty(command.FtpServerId) ? null : Guid.Parse(command.FtpServerId),
            SourceProtocolCD = command.Protocol!,
            SourceAddress = command.Address!,
            SourceFileMask = command.Mask!,
            FileDirection = command.FileDirection,
            SendNotificationsInd = command.SendNotifications,
            IsValid = command.IsValid,
            IsSuspended = command.IsSuspended,
            IsSuspendedChangedTs = ToUtcOffset(command.IsSuspendedChangedDate),
            OwnerName = command.OwnerName!,
            OwnerModule = Guid.Parse(command.OwnerModule!),
            OwnerPhone = command.OwnerPhone,
            OwnerEmail = command.OwnerEmail,
            RunOnMondayInd = command.RunOnMonday,
            RunOnTuesdayInd = command.RunOnTuesday,
            RunOnWednesdayInd = command.RunOnWed,
            RunOnThursdayInd = command.RunOnThrus,
            RunOnFridayInd = command.RunOnFri,
            RunOnSaturdayInd = command.RunOnSat,
            RunOnSundayInd = command.RunOnSun,
            RunOnFirstDayOfMonthInd = command.RunOnFrstDayOfMonthInd,
            RunOnLastDayOfMonthInd = command.RunOnLastDayOfMonthInd,
            RunOnThisDayOfMonth = (short)command.RunOnThisDayOfMonth,
            StartTime24HHMM = command.StartTime!,
            EndTime24HHMM = command.EndTime!,
            TradingPartner = command.TradingPartner,
            OperatorId = command.Requestor!.IdentifierId,
            CreationDate = now,
            LastModifiedDate = now
        };

        _dbContext.FtsSourceLocations.Add(sourceLocation);

        foreach (var dest in command.DestinationAddress)
        {
            var destIsValid = command.IsValid && dest.IsValid;
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
                IsValid = destIsValid,
                FtpServerId = dest.FtpServerId,
                OperatorId = dest.OperatorId ?? command.Requestor!.IdentifierId,
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

        var payload = new FxferConfigurationChangedV1
        {
            AggregateType = "transfer_configuration",
            AggregateId = sourceLocationId,
            ChangeKind = "created",
            OperatorId = command.Requestor!.IdentifierId,
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

    private static async Task<int> AllocateConfigIdAsync(FxferDbContext dbContext, CancellationToken ct)
    {
        // shortcut: EF InMemory provider used by tests does not support sequence queries.
        if (dbContext.Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) == true)
        {
            var max = await dbContext.FtsSourceLocations.Select(x => (int?)x.ConfigId).MaxAsync(ct);
            return (max ?? 0) + 1;
        }

        var sequenceName = FileTransferExceptionConstants.Fields.FtsSourceLocationSeq;
        var result = await dbContext.Database.SqlQueryRaw<long>($"SELECT nextval('{sequenceName}')").SingleAsync(ct);
        return checked((int)result);
    }
}

