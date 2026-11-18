using Fxfer.DomainApi.Application.Commands.IntegrationFiles;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using HPP.Core.BAS.CQRS.Base;
using HPP.Core.BAS.CQRS.UserMeta;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Tests.Application.Commands.IntegrationFiles;

public sealed class IntegrationFileCommandHandlerRuleTests
{
    [Fact]
    public async Task AddIntegrationFile_returns_null_when_group_info_is_missing_or_invalid()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();
        var handler = new AddIntegrationFileCommandHandler(dbContext, eventFactory, outboxWriter);

        var command = new AddIntegrationFileCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            GroupInfo = null,
            FileName = "file.dat",
            ETFOutboundMetadataDefnId = Guid.NewGuid()
        };

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.Null(result);
        Assert.Empty(dbContext.OutboxMessages);
    }

    [Fact]
    public async Task AddIntegrationFile_persists_one_row_per_group_with_records()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();
        var handler = new AddIntegrationFileCommandHandler(dbContext, eventFactory, outboxWriter);

        var command = new AddIntegrationFileCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            FileName = "file.dat",
            ETFOutboundMetadataDefnId = Guid.NewGuid(),
            GroupInfo = new List<GroupInfo>
            {
                new() { GroupId = Guid.NewGuid(), NumberOfRecords = 0 },
                new() { GroupId = Guid.NewGuid(), NumberOfRecords = 10 }
            }
        };

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.NotNull(result);

        var rows = await dbContext.FtsIntegrationFile.ToListAsync();
        Assert.Single(rows);
        Assert.Single(dbContext.OutboxMessages);
    }

    [Fact]
    public async Task F03_011_update_integration_file_validates_and_emits_configuration_changed_event()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();
        var handler = new UpdateIntegrationFileCommandHandler(dbContext, eventFactory, outboxWriter);

        var integrationFileId = Guid.NewGuid();
        dbContext.FtsIntegrationFile.Add(new Fxfer.DomainApi.Infrastructure.DataAccess.Entities.FtsIntegrationFile
        {
            FtsIntegrationFileId = integrationFileId,
            GroupId = Guid.NewGuid(),
            ConfigId = null,
            FileName = "file.dat",
            ETFOutboundMetadataDefnId = Guid.NewGuid(),
            IsProcessed = false,
            OperatorId = "seed",
            IsValid = true,
            CreationDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        });
        await dbContext.SaveChangesAsync();

        var command = new UpdateIntegrationFileCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            FtsIntegrationFileId = integrationFileId,
            ConfigId = 123,
            FileName = "ignored",
            ETFOutboundMetadataDefnId = Guid.NewGuid(),
            IsValid = true
        };

        var domainEvent = await handler.Handle(command, CancellationToken.None);
        Assert.NotNull(domainEvent);

        Assert.Single(dbContext.OutboxMessages);
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

