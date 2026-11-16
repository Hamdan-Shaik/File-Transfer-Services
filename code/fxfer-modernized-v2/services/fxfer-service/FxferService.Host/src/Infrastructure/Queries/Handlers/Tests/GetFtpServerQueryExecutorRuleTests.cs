using FxferService.Application.Contracts.Queries;
using FxferService.Application.QueryHandlers;
using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Utilities;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Tests;

public sealed class GetFtpServerQueryExecutorRuleTests
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

    private static FtpServerQuery CreateDefaultQuery()
    {
        return new FtpServerQuery()
        {
            Where = new FtpServerQueryParms()
            {
                IsValid = null,
                FtpServerId = Guid.Empty,
                FtpServerName = string.Empty,
                FtpDescription = string.Empty,
                FtpUser = string.Empty,
                SearchOptions = new List<SearchModeOption>(),
            },
            SortCriteria = null,
        };
    }

    private static FxferService.Infrastructure.Persistence.Entities.FtpServer CreateServer(Guid id, string name, string description, string user, bool isValid)
    {
        return new FxferService.Infrastructure.Persistence.Entities.FtpServer()
        {
            FtpServerId = id,
            FtpServerName = name,
            FtpDescription = description,
            FtpServerIp = "127.0.0.1",
            FtpTransferMode = "I",
            FtpUser = user,
            FtpPassword = "secret",
            FtpPort = 21,
            FtpProxyAddress = null,
            FtpProxyPort = null,
            IsValid = isValid,
            OperatorId = "op",
            LastModifiedDate = new DateTime(2020, 1, 2),
            CreationDate = new DateTime(2020, 1, 1),
            UseSSHAuth = false,
        };
    }

    [Fact]
    public void F01_005_projects_ftp_server_rows_including_password()
    {
        var (db, context) = CreateContext();

        var id = Guid.NewGuid();
        db.FtpServers.Add(CreateServer(id, "A", "Desc", "User", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        new GetFtpServerQueryExecutor().Execute(context, query);

        Assert.Single(query.Results!);
        Assert.Equal("secret", query.Results![0].FtpPassword);
    }

    [Fact]
    public void F01_006_filters_is_valid_when_specified()
    {
        var (db, context) = CreateContext();

        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "A", "D", "U", isValid: true));
        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "B", "D", "U", isValid: false));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.IsValid = true;
        new GetFtpServerQueryExecutor().Execute(context, query);

        Assert.Single(query.Results!);
        Assert.True(query.Results![0].IsValid);
    }

    [Fact]
    public void F01_007_does_not_apply_default_sorting()
    {
        var (db, context) = CreateContext();

        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "b", "D", "U", isValid: true));
        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "a", "D", "U", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.SortCriteria = null;
        new GetFtpServerQueryExecutor().Execute(context, query);

        Assert.Equal(new[] { "b", "a" }, query.Results!.Select(x => x.FtpServerName).ToArray());
    }

    [Fact]
    public void F01_008_ignores_requested_ordering()
    {
        var (db, context) = CreateContext();

        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "b", "D", "U", isValid: true));
        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "a", "D", "U", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.SortCriteria = new List<SortCriteria>
        {
            new()
            {
                FieldName = CommonConstants.SortFields.FtpServer.FtpServerName,
                Order = SortOrder.Asc,
            },
        };

        new GetFtpServerQueryExecutor().Execute(context, query);

        Assert.Equal(new[] { "b", "a" }, query.Results!.Select(x => x.FtpServerName).ToArray());
    }

    [Fact]
    public void F01_009_does_not_apply_ftp_server_id_filter()
    {
        var (db, context) = CreateContext();

        var targetId = Guid.NewGuid();
        db.FtpServers.Add(CreateServer(targetId, "A", "D", "U", isValid: true));
        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "B", "D", "U", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.FtpServerId = targetId;
        query.Where.SearchOptions = new List<SearchModeOption>
        {
            new()
            {
                FieldName = CommonConstants.SortFields.FtpServer.FtpServerId,
                FilterType = SearchModeOption.SearchMode.EqualTo,
            },
        };

        new GetFtpServerQueryExecutor().Execute(context, query);

        Assert.Equal(2, query.Results!.Count);
    }

    [Fact]
    public void F01_010_only_supports_exact_name_matching_case_insensitively()
    {
        var (db, context) = CreateContext();

        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "MyServer", "D", "U", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.FtpServerName = "serv";
        new GetFtpServerQueryExecutor().Execute(context, query);
        Assert.Empty(query.Results!);

        query = CreateDefaultQuery();
        query.Where.FtpServerName = "myserver";
        new GetFtpServerQueryExecutor().Execute(context, query);
        Assert.Single(query.Results!);
    }

    [Fact]
    public void F01_011_does_not_apply_description_filtering()
    {
        var (db, context) = CreateContext();

        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "A", "Alpha", "U", isValid: true));
        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "B", "Beta", "U", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.FtpDescription = "Alpha";
        query.Where.SearchOptions = new List<SearchModeOption>
        {
            new()
            {
                FieldName = CommonConstants.SortFields.FtpServer.FtpDescription,
                FilterType = SearchModeOption.SearchMode.Contains,
            },
        };

        new GetFtpServerQueryExecutor().Execute(context, query);

        Assert.Equal(2, query.Results!.Count);
    }

    [Fact]
    public void F01_012_does_not_apply_user_name_filtering()
    {
        var (db, context) = CreateContext();

        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "A", "D", "USER1", isValid: true));
        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "B", "D", "USER2", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.FtpUser = "USER1";
        query.Where.SearchOptions = new List<SearchModeOption>
        {
            new()
            {
                FieldName = CommonConstants.SortFields.FtpServer.FtpUser,
                FilterType = SearchModeOption.SearchMode.EqualTo,
            },
        };

        new GetFtpServerQueryExecutor().Execute(context, query);

        Assert.Equal(2, query.Results!.Count);
    }

    [Fact]
    public void F01_029_duplicate_name_detection_queries_only_valid_records()
    {
        var (db, context) = CreateContext();

        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "Dup", "D", "U", isValid: true));
        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "Dup", "D", "U", isValid: false));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.IsValid = true;
        query.Where.FtpServerName = "dup";
        new GetFtpServerQueryExecutor().Execute(context, query);

        Assert.Single(query.Results!);
        Assert.True(query.Results![0].IsValid);
    }

    [Fact]
    public void F01_030_name_matching_ignores_case_but_does_not_trim_spaces()
    {
        var (db, context) = CreateContext();

        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "  Server  ", "D", "U", isValid: true));
        db.SaveChanges();

        var query = CreateDefaultQuery();
        query.Where.FtpServerName = "server";
        new GetFtpServerQueryExecutor().Execute(context, query);
        Assert.Empty(query.Results!);

        query = CreateDefaultQuery();
        query.Where.FtpServerName = "  server  ";
        new GetFtpServerQueryExecutor().Execute(context, query);
        Assert.Single(query.Results!);
    }
}

