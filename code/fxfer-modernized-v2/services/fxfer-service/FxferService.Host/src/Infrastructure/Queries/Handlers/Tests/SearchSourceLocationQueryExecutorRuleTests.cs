using FxferService.Application.Contracts.Queries;
using FxferService.Application.QueryHandlers;
using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Tests;

public sealed class SearchSourceLocationQueryExecutorRuleTests
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

    private static SourceLocationQuery CreateDefaultQuery()
    {
        return new SourceLocationQuery()
        {
            Where = new SourceLocationQueryParms()
            {
                ConfigId = null,
                OwnerName = string.Empty,
                ConfigName = string.Empty,
                OwnerSubsystem = string.Empty,
                Protocol = string.Empty,
                IsActive = false,
            },
            SortCriteria = null,
        };
    }

    private static FtsSourceLocationEntity CreateSourceLocation(Guid id, int configId, string configName, string ownerName, Guid moduleId, string protocol, string address, string mask, bool isValid)
    {
        return new FtsSourceLocationEntity()
        {
            FtsSourceLocationId = id,
            ConfigId = configId,
            ConfigName = configName,
            OwnerName = ownerName,
            ModuleId = moduleId,
            SourceProtocolCode = protocol,
            SourceAddress = address,
            SourceFileMask = mask,
            IsValid = isValid,
            OwnerEmail = "owner@example.com",
            OwnerPhone = "123",
            OperatorId = "op",
            StartTime24hhmm = "0000",
            EndTime24hhmm = "2359",
            CreatedAt = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            LastModifiedAt = new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
        };
    }

    [Fact]
    public void F02_009_projects_source_location_fields_and_returns_all_rows_by_default()
    {
        var (db, context) = CreateContext();

        var moduleId = Guid.NewGuid();
        db.SourceLocations.Add(CreateSourceLocation(Guid.NewGuid(), 1, "Cfg1", "Owner1", moduleId, "FTP", "A1", "M1", isValid: true));
        db.SourceLocations.Add(CreateSourceLocation(Guid.NewGuid(), 2, "Cfg2", "Owner2", Guid.NewGuid(), "UNC", "A2", "M2", isValid: false));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        new SearchSourceLocationQueryExecutor().Execute(context, query);

        Assert.Equal(2, query.Results!.Count);
        Assert.Contains(query.Results!, r => r.ConfigId == 1 && r.ConfigName == "Cfg1" && r.OwnerName == "Owner1" && r.OwnerSubsystem == moduleId && r.Protocol == "FTP" && r.Address == "A1" && r.Mask == "M1" && r.IsActive);
        Assert.Contains(query.Results!, r => r.ConfigId == 2 && r.ConfigName == "Cfg2" && r.OwnerName == "Owner2" && r.Protocol == "UNC" && r.Address == "A2" && r.Mask == "M2" && !r.IsActive);
    }

    [Fact]
    public void F02_010_applies_exact_match_filters_and_parses_owner_subsystem_as_guid()
    {
        var (db, context) = CreateContext();

        var moduleId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        db.SourceLocations.Add(CreateSourceLocation(matchId, 10, "Config", "Alice", moduleId, "FTP", "addr", "mask", isValid: true));
        db.SourceLocations.Add(CreateSourceLocation(Guid.NewGuid(), 11, "Other", "Bob", Guid.NewGuid(), "UNC", "addr2", "mask2", isValid: false));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.ConfigId = 10;
        query.Where.OwnerName = "ALICE";
        query.Where.ConfigName = "cOnFiG";
        query.Where.OwnerSubsystem = moduleId.ToString();
        query.Where.Protocol = "FTP";
        query.Where.IsActive = true;

        new SearchSourceLocationQueryExecutor().Execute(context, query);
        Assert.Single(query.Results!);
        Assert.Equal(matchId, query.Results![0].FtsSourceLocationId);

        query = CreateDefaultQuery();
        query.Where.OwnerSubsystem = "not-a-guid";
        var exception = Assert.Throws<InvalidOperationException>(() => new SearchSourceLocationQueryExecutor().Execute(context, query));
        Assert.IsType<FormatException>(exception.InnerException);
    }

    [Fact]
    public void F02_053_returns_no_results_when_config_id_does_not_exist()
    {
        var (db, context) = CreateContext();

        db.SourceLocations.Add(CreateSourceLocation(Guid.NewGuid(), 10, "Cfg", "Owner", Guid.NewGuid(), "FTP", "A", "M", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.ConfigId = 999;

        new SearchSourceLocationQueryExecutor().Execute(context, query);

        Assert.Empty(query.Results!);
    }

    [Fact]
    public void F02_055_returns_no_results_when_config_id_does_not_exist()
    {
        var (db, context) = CreateContext();

        db.SourceLocations.Add(CreateSourceLocation(Guid.NewGuid(), 10, "Cfg", "Owner", Guid.NewGuid(), "FTP", "A", "M", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.ConfigId = 999;

        new SearchSourceLocationQueryExecutor().Execute(context, query);

        Assert.Empty(query.Results!);
    }

    [Fact]
    public void F02_011_supports_dynamic_sorting_and_defaults_to_config_id_then_status()
    {
        var (db, context) = CreateContext();

        db.SourceLocations.Add(CreateSourceLocation(Guid.NewGuid(), 2, "B", "O", Guid.NewGuid(), "P", "A", "M", isValid: false));
        db.SourceLocations.Add(CreateSourceLocation(Guid.NewGuid(), 1, "A", "O", Guid.NewGuid(), "P", "A", "M", isValid: false));
        db.SourceLocations.Add(CreateSourceLocation(Guid.NewGuid(), 1, "C", "O", Guid.NewGuid(), "P", "A", "M", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        new SearchSourceLocationQueryExecutor().Execute(context, query);
        Assert.Equal(new[] { (1, true), (1, false), (2, false) }, query.Results!.Select(x => (x.ConfigId, x.IsActive)).ToArray());

        query = CreateDefaultQuery();
        query.SortCriteria = new List<SortCriteria>
        {
            new()
            {
                FieldName = "ConfigName",
                Order = SortOrder.Desc,
            },
        };
        new SearchSourceLocationQueryExecutor().Execute(context, query);
        Assert.Equal(new[] { "C", "B", "A" }, query.Results!.Select(x => x.ConfigName).ToArray());
    }
}

