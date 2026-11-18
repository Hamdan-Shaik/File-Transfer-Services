using Fxfer.DomainApi.Application.Commands.TransferConfigurations;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Exceptions;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using HPP.Core.BAS.CQRS.Base;
using HPP.Core.BAS.CQRS.UserMeta;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Tests.Application.Commands.TransferConfigurations;

public sealed class SourceLocationCommandHandlerRuleTests
{
    [Fact]
    public async Task F01_046_add_allocates_config_id_and_persists_source_location()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();
        var handler = new AddFtsSourceLocationCommandHandler(dbContext, eventFactory, outboxWriter);

        var command = BuildAddCommand();

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.NotNull(result);

        var sourceLocation = await dbContext.FtsSourceLocations.SingleAsync();
        Assert.Equal(1, sourceLocation.ConfigId);
        Assert.Equal(command.IsValid, sourceLocation.IsValid);

        Assert.Single(dbContext.OutboxMessages);
    }

    [Fact]
    public async Task F01_069_update_throws_business_exception_when_config_id_not_found()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();
        var handler = new UpdateFtsSourceLocationCommandHandler(dbContext, eventFactory, outboxWriter);

        var command = BuildUpdateCommand(configId: 123);

        var ex = await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Contains(FileTransferExceptionConstants.Validation.Message, ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task F01_071_update_uses_stored_source_location_id_and_operator_id_from_command()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();

        var sourceLocationId = Guid.NewGuid();
        dbContext.FtsSourceLocations.Add(new FtsSourceLocation
        {
            FtsSourceLocationId = sourceLocationId,
            ConfigId = 50,
            ConfigName = "seed",
            SourceProtocolCD = "1",
            SourceAddress = "seed",
            SourceFileMask = "*.txt",
            SendNotificationsInd = true,
            OwnerName = "seed",
            OwnerModule = Guid.NewGuid(),
            OwnerPhone = "1",
            OwnerEmail = "a@b.com",
            StartTime24HHMM = "0000",
            EndTime24HHMM = "2359",
            OperatorId = "seed",
            CreationDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        });
        await dbContext.SaveChangesAsync();

        var handler = new UpdateFtsSourceLocationCommandHandler(dbContext, eventFactory, outboxWriter);

        var command = BuildUpdateCommand(configId: 50);
        command.FtsSourceLocationId = Guid.NewGuid().ToString();
        command.OperatorId = "operator-from-command";

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.NotNull(result);

        var outboxPayload = await dbContext.OutboxMessages.SingleAsync();
        Assert.Contains(FxferEventMetadata.ConfigurationChangedEventName, outboxPayload.PayloadJson, StringComparison.Ordinal);
    }

    [Fact]
    public async Task F01_072_invalidate_uses_stored_source_location_id_and_operator_id_from_command()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();

        var sourceLocationId = Guid.NewGuid();
        dbContext.FtsSourceLocations.Add(new FtsSourceLocation
        {
            FtsSourceLocationId = sourceLocationId,
            ConfigId = 50,
            ConfigName = "seed",
            SourceProtocolCD = "1",
            SourceAddress = "seed",
            SourceFileMask = "*.txt",
            SendNotificationsInd = true,
            OwnerName = "seed",
            OwnerModule = Guid.NewGuid(),
            OwnerPhone = "1",
            OwnerEmail = "a@b.com",
            StartTime24HHMM = "0000",
            EndTime24HHMM = "2359",
            IsValid = true,
            OperatorId = "seed",
            CreationDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        });
        await dbContext.SaveChangesAsync();

        var handler = new InvalidateFtsSourceLocationCommandHandler(dbContext, eventFactory, outboxWriter);

        var command = new FtsSourceLocationInvalidateCommand
        {
            Requestor = new RequestorModel { IdentifierId = "ignored" },
            FtsSourceLocationId = Guid.NewGuid().ToString(),
            ConfigId = 50,
            OperatorId = "operator-from-command",
            DestinationAddress = null
        };

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.Equal(sourceLocationId, result.Payload.AggregateId);
        Assert.Equal("operator-from-command", result.Payload.OperatorId);

        var persisted = await dbContext.FtsSourceLocations.SingleAsync();
        Assert.False(persisted.IsValid);
        Assert.Equal("operator-from-command", persisted.OperatorId);
    }

    private static FtsSourceLocationAddCommand BuildAddCommand()
    {
        return new FtsSourceLocationAddCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            ConfigName = "cfg",
            FtpServerId = Guid.NewGuid().ToString(),
            Protocol = "1",
            Address = "adr",
            OwnerName = "owner",
            OwnerModule = Guid.NewGuid().ToString(),
            OwnerPhone = "123",
            OwnerEmail = "a@b.com",
            StartTime = "0000",
            EndTime = "2359",
            Mask = "*.txt",
            RunOnMonday = true,
            IsValid = true,
            DestinationAddress = new List<FtsDestinationLocationAdded>
            {
                new()
                {
                    DestinationAddress = "dest",
                    DestinationProtocol = "1",
                    FtpServerId = Guid.NewGuid(),
                    OperatorId = "operator",
                    IsValid = true
                }
            }
        };
    }

    private static FtsSourceLocationUpdateCommand BuildUpdateCommand(int configId)
    {
        return new FtsSourceLocationUpdateCommand
        {
            Requestor = new RequestorModel { IdentifierId = "ignored" },
            FtsSourceLocationId = Guid.NewGuid().ToString(),
            ConfigId = configId,
            ConfigName = "cfg",
            FtpServerId = Guid.NewGuid().ToString(),
            Protocol = "1",
            Address = "adr",
            OwnerName = "owner",
            OwnerModule = Guid.NewGuid().ToString(),
            OwnerPhone = "123",
            OwnerEmail = "a@b.com",
            StartTime = "0000",
            EndTime = "2359",
            Mask = "*.txt",
            RunOnMonday = true,
            OperatorId = "operator",
            DestinationAddress = new List<FtsDestinationLocationAdded>()
        };
    }

    private static (FxferDbContext DbContext, OutboxMessageWriter OutboxWriter, FxferDomainEventFactory EventFactory) CreateDb()
    {
        const string tenantId = "tenant-test";

        var httpContext = new DefaultHttpContext();
        httpContext.Items[ContextKeys.TenantIdItemKey] = tenantId;
        httpContext.Items[ContextKeys.CorrelationIdItemKey] = Guid.NewGuid().ToString();
        var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };

        var eventFactory = new FxferDomainEventFactory(httpContextAccessor);

        var dbOptions = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new FxferDbContext(dbOptions);
        var outboxWriter = new OutboxMessageWriter(dbContext, eventFactory);

        return (dbContext, outboxWriter, eventFactory);
    }
}

