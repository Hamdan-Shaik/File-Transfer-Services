using Fxfer.DomainApi.Application.Commands.InboundTargets;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using HPP.Core.BAS.CQRS.UserMeta;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Tests.Application.Commands.InboundTargets;

public sealed class InboundTargetCommandHandlerRuleTests
{
    [Fact]
    public async Task AddInboundTarget_returns_null_when_record_already_exists()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();

        var existing = new FtsInboundTargetModel { FtsInboundTargetId = Guid.NewGuid(), FTEMetadataId = Guid.NewGuid() };
        var handler = new AddFtsInboundTargetCommandHandler(new FakeReadOnlyRepository(existing), dbContext, eventFactory, outboxWriter);

        var command = new AddFtsInboundTargetCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            FTEMetadataId = existing.FTEMetadataId,
            ImportJobName = "job",
            ContractName = "contract"
        };

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.Null(result);
        Assert.Empty(dbContext.OutboxMessages);
    }

    [Fact]
    public async Task AddInboundTarget_persists_and_emits_event_when_missing()
    {
        var (dbContext, outboxWriter, eventFactory) = CreateDb();
        var handler = new AddFtsInboundTargetCommandHandler(new FakeReadOnlyRepository(), dbContext, eventFactory, outboxWriter);

        var command = new AddFtsInboundTargetCommand
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            FTEMetadataId = Guid.NewGuid(),
            ImportJobName = "job",
            ContractName = "contract"
        };

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.NotNull(result);

        Assert.Single(dbContext.FtsInboundTarget);
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

    private sealed class FakeReadOnlyRepository : IFileTransferReadOnlyRepository
    {
        private readonly IReadOnlyList<FtsInboundTargetModel> _inboundTargets;

        public FakeReadOnlyRepository(params FtsInboundTargetModel[] inboundTargets)
        {
            _inboundTargets = inboundTargets;
        }

        public Task<IReadOnlyList<FtpServerModel>> GetFtpServerAsync(FtpServerQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult((IReadOnlyList<FtpServerModel>)Array.Empty<FtpServerModel>());
        }

        public Task<IReadOnlyList<FtsInboundTargetModel>> GetInboundTargetAsync(InboundTargetQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(_inboundTargets);
        }
    }
}

