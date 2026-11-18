using Fxfer.DomainApi.Application.Commands.FtpServers;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using HPP.Core.BAS.CQRS.Base;
using HPP.Core.BAS.CQRS.UserMeta;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Tests.Application.Commands.FtpServers;

public sealed class FtpServerCommandHandlerRuleTests
{
    [Fact]
    public async Task F02_023_add_ftp_server_runs_command_validation()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();
        var handler = new AddFtpServerCommandHandler(new FakeReadOnlyRepository(), dbContext, eventFactory, outboxWriter);

        var command = new AddFtpServerCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            FtpServerName = null,
            FtpDescription = "desc",
            FtpServerIp = "1.2.3.4",
            FtpTransferMode = "passive",
            FtpUser = "user",
            FtpPassword = "secret"
        };

        await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task F02_031_add_ftp_server_rejects_duplicate_name_among_valid_servers()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();

        var existing = new FtpServerModel
        {
            FtpServerId = Guid.NewGuid(),
            FtpServerName = "dup"
        };

        var handler = new AddFtpServerCommandHandler(new FakeReadOnlyRepository(existing), dbContext, eventFactory, outboxWriter);

        var command = BuildValidAddCommand("dup");

        await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task F02_032_add_ftp_server_forces_is_valid_true_and_operator_from_requestor()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();
        var handler = new AddFtpServerCommandHandler(new FakeReadOnlyRepository(), dbContext, eventFactory, outboxWriter);

        var command = BuildValidAddCommand("name");
        command.IsValid = false;

        var domainEvent = await handler.Handle(command, CancellationToken.None);
        Assert.NotNull(domainEvent);

        var persisted = await dbContext.FtpServers.SingleAsync();
        Assert.True(persisted.IsValid);
        Assert.Equal("operator", persisted.OperatorId);

        Assert.Single(dbContext.OutboxMessages);
    }

    [Fact]
    public async Task F02_033_update_ftp_server_sets_operator_from_requestor()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();

        var ftpServerId = Guid.NewGuid();

        dbContext.FtpServers.Add(new Fxfer.DomainApi.Domain.Entities.FtpServer
        {
            FtpServerId = ftpServerId,
            FtpServerName = "seed",
            FtpDescription = "seed",
            FtpServerHost = "1.2.3.4",
            FtpTransferMode = "passive",
            FtpUser = "user",
            FtpPasswordSecretRef = "secret",
            FtpPort = 21,
            IsValid = true,
            UseSshAuth = false,
            OperatorId = "seed",
            CreationDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        });
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        var handler = new UpdateFtpServerCommandHandler(new FakeReadOnlyRepository(), dbContext, eventFactory, outboxWriter);
        var command = new UpdateFtpServerCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            FtpServerId = ftpServerId,
            FtpServerName = "name",
            FtpDescription = "desc",
            FtpServerIp = "1.2.3.4",
            FtpTransferMode = "passive",
            FtpUser = "user",
            FtpPassword = "secret",
            FtpPort = 21,
            IsValid = true
        };

        var domainEvent = await handler.Handle(command, CancellationToken.None);
        Assert.Equal("operator", domainEvent.Payload.OperatorId);

        var persisted = await dbContext.FtpServers.SingleAsync();
        Assert.Equal("operator", persisted.OperatorId);

        Assert.Single(dbContext.OutboxMessages);
    }

    
    [Fact]
    public async Task F02_037_update_ftp_server_reuses_password_when_not_provided()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();

        var ftpServerId = Guid.NewGuid();

        dbContext.FtpServers.Add(new Fxfer.DomainApi.Domain.Entities.FtpServer
        {
            FtpServerId = ftpServerId,
            FtpServerName = "seed",
            FtpDescription = "seed",
            FtpServerHost = "1.2.3.4",
            FtpTransferMode = "passive",
            FtpUser = "user",
            FtpPasswordSecretRef = "secret-existing",
            FtpPort = 21,
            IsValid = true,
            UseSshAuth = false,
            OperatorId = "seed",
            CreationDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        });
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        var handler = new UpdateFtpServerCommandHandler(new FakeReadOnlyRepository(), dbContext, eventFactory, outboxWriter);
        var command = new UpdateFtpServerCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            FtpServerId = ftpServerId,
            FtpServerName = "name",
            FtpDescription = "desc",
            FtpServerIp = "1.2.3.4",
            FtpTransferMode = "passive",
            FtpUser = "user",
            FtpPassword = null,
            FtpPort = 21,
            IsValid = true
        };

        await handler.Handle(command, CancellationToken.None);

        var persisted = await dbContext.FtpServers.SingleAsync();
        Assert.Equal("secret-existing", persisted.FtpPasswordSecretRef);
    }
    [Fact]
    public async Task F02_034_invalidate_does_not_call_command_validate()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();
        var handler = new InvalidateFtpServerCommandHandler(dbContext, eventFactory, outboxWriter);

        var command = new InvalidateFtpServerCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            FtpServerName = null,
            FtpDescription = null,
            FtpServerIp = null,
            FtpTransferMode = null,
            FtpUser = null,
            FtpPassword = null
        };

        var domainEvent = await handler.Handle(command, CancellationToken.None);
        Assert.NotNull(domainEvent);
        Assert.Single(dbContext.OutboxMessages);
    }

    private static AddFtpServerCommand BuildValidAddCommand(string name)
    {
        return new AddFtpServerCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            FtpServerName = name,
            FtpDescription = "desc",
            FtpServerIp = "1.2.3.4",
            FtpTransferMode = "passive",
            FtpUser = "user",
            FtpPassword = "secret",
            FtpPort = 21
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

    private sealed class FakeReadOnlyRepository : IFileTransferReadOnlyRepository
    {
        private readonly IReadOnlyList<FtpServerModel> _ftpServers;

        public FakeReadOnlyRepository(params FtpServerModel[] ftpServers)
        {
            _ftpServers = ftpServers;
        }

        public Task<IReadOnlyList<FtpServerModel>> GetFtpServerAsync(FtpServerQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(_ftpServers);
        }

        public Task<IReadOnlyList<FtsInboundTargetModel>> GetInboundTargetAsync(InboundTargetQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult((IReadOnlyList<FtsInboundTargetModel>)Array.Empty<FtsInboundTargetModel>());
        }
    }
}

