using FxferService.Application.Contracts.Queries;
using FxferService.Application.QueryHandlers;
using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Tests;

public sealed class InboundTargetQueryExecutorRuleTests
{
    private sealed class TestDbContextBase : IDbContextBase
    {
        private readonly FxferDbContext _db;

        public TestDbContextBase(FxferDbContext db)
        {
            _db = db;
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class => _db.Set<TEntity>();

        public int SaveChanges() => _db.SaveChanges();
    }

    private static (FxferDbContext Db, IDbContextBase Context) CreateContext()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var db = new FxferDbContext(options);
        return (db, new TestDbContextBase(db));
    }

    private static InboundTargetQuery CreateDefaultQuery()
    {
        return new InboundTargetQuery()
        {
            Where = new InboundTargetQueryParms()
            {
                FTEMetadataId = Guid.Empty,
                ContractName = string.Empty,
            },
            SortCriteria = null,
        };
    }

    private static FtsInboundTargetEntity CreateInboundTarget(Guid id, Guid metadataId, string contractName, bool isValid)
    {
        return new FtsInboundTargetEntity()
        {
            FtsInboundTargetId = id,
            ExternalFileDefnId = metadataId,
            ImportJobName = "job",
            ContractName = contractName,
            OperatorId = "op",
            IsValid = isValid,
            CreatedAt = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            LastModifiedAt = new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
        };
    }

    [Fact]
    public void F04_038_filters_by_optional_fields_requires_valid_and_deduplicates_by_metadata_id()
    {
        var (db, context) = CreateContext();

        var meta1 = Guid.NewGuid();
        var meta2 = Guid.NewGuid();

        var id1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var id2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var id3 = Guid.Parse("00000000-0000-0000-0000-000000000003");

        db.InboundTargets.Add(CreateInboundTarget(id2, meta1, contractName: "A", isValid: true));
        db.InboundTargets.Add(CreateInboundTarget(id1, meta1, contractName: "B", isValid: true));
        db.InboundTargets.Add(CreateInboundTarget(id3, meta2, contractName: "A", isValid: true));
        db.InboundTargets.Add(CreateInboundTarget(Guid.NewGuid(), Guid.NewGuid(), contractName: "A", isValid: false));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        new InboundTargetQueryExecutor().Execute(context, query);

        Assert.Equal(2, query.Results!.Count);
        Assert.Single(query.Results!, x => x.FTEMetadataId == meta1);
        Assert.Equal(id1, query.Results!.Single(x => x.FTEMetadataId == meta1).FtsInboundTargetId);

        query = CreateDefaultQuery();
        query.Where.FTEMetadataId = meta1;
        new InboundTargetQueryExecutor().Execute(context, query);
        Assert.Single(query.Results!);
        Assert.Equal(meta1, query.Results![0].FTEMetadataId);

        query = CreateDefaultQuery();
        query.Where.ContractName = "B";
        new InboundTargetQueryExecutor().Execute(context, query);
        Assert.Single(query.Results!);
        Assert.Equal("B", query.Results![0].ContractName);
    }
}

