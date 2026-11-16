using FxferService.Application.Contracts.Queries;
using FxferService.Application.QueryHandlers;
using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Tests;

public sealed class IntegrationFileQueryExecutorRuleTests
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

    private static GetIntegrationFileQuery CreateDefaultQuery()
    {
        return new GetIntegrationFileQuery()
        {
            Where = new IntegrationFileQueryParms()
            {
                FTSIntegrationFileId = Guid.Empty,
                FileName = string.Empty,
            },
            SortCriteria = null,
        };
    }

    private static FtsIntegrationFileEntity CreateIntegrationFile(Guid id, string fileName, bool isValid)
    {
        return new FtsIntegrationFileEntity()
        {
            FtsIntegrationFileId = id,
            GroupId = Guid.NewGuid(),
            OutboundMetadataDefnId = Guid.NewGuid(),
            ConfigId = 123,
            FileName = fileName,
            IsProcessed = false,
            OperatorId = "op",
            IsValid = isValid,
            CreatedAt = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            LastModifiedAt = new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
        };
    }

    [Fact]
    public void F02_065_returns_only_valid_rows_and_applies_optional_id_and_filename_filters()
    {
        var (db, context) = CreateContext();

        var id1 = Guid.NewGuid();
        db.IntegrationFiles.Add(CreateIntegrationFile(id1, fileName: "AlphaFile.txt", isValid: true));
        db.IntegrationFiles.Add(CreateIntegrationFile(Guid.NewGuid(), fileName: "BetaFile.txt", isValid: false));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        new IntegrationFileQueryExecutor().Execute(context, query);
        Assert.Single(query.Results!);
        Assert.Equal(id1, query.Results![0].FtsIntegrationFileId);

        query = CreateDefaultQuery();
        query.Where.FTSIntegrationFileId = id1;
        new IntegrationFileQueryExecutor().Execute(context, query);
        Assert.Single(query.Results!);
        Assert.Equal(id1, query.Results![0].FtsIntegrationFileId);

        query = CreateDefaultQuery();
        query.Where.FileName = "Alpha";
        new IntegrationFileQueryExecutor().Execute(context, query);
        Assert.Single(query.Results!);
        Assert.Equal("AlphaFile.txt", query.Results![0].FileName);
    }

    [Fact]
    public void F02_066_projects_expected_fields_for_each_result()
    {
        var (db, context) = CreateContext();

        var id = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var outboundId = Guid.NewGuid();

        db.IntegrationFiles.Add(new FtsIntegrationFileEntity()
        {
            FtsIntegrationFileId = id,
            GroupId = groupId,
            OutboundMetadataDefnId = outboundId,
            ConfigId = 456,
            FileName = "Name.txt",
            IsProcessed = true,
            OperatorId = "op",
            IsValid = true,
            CreatedAt = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            LastModifiedAt = new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
        });
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.FTSIntegrationFileId = id;
        new IntegrationFileQueryExecutor().Execute(context, query);

        Assert.Single(query.Results!);
        var result = query.Results![0];
        Assert.Equal(groupId, result.GroupId);
        Assert.Equal(outboundId, result.ETFOutboundMetadataDefnId);
        Assert.Equal(456, result.ConfigId);
        Assert.Equal("Name.txt", result.FileName);
        Assert.True(result.IsProcessed);
        Assert.Equal(id, result.FtsIntegrationFileId);
    }
}

