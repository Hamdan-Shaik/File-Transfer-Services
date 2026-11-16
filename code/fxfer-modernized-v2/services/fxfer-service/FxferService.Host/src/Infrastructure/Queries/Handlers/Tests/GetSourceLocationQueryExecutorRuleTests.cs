using System.Reflection;

using FxferService.Application.Contracts.Queries;
using FxferService.Application.QueryHandlers;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Tests;

public sealed class GetSourceLocationQueryExecutorRuleTests
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

    private sealed class TestableGetSourceLocationQueryExecutor : GetSourceLocationQueryExecutor
    {
        public IQueryable<SourceLocationModel> GetBaseQueryPublic(IDbContextBase context, GetSourceLocationQuery query)
            => base.GetBaseQuery(context, query);
    }

    private static (FxferDbContext Db, IDbContextBase Context) CreateContext()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var db = new FxferDbContext(options);
        return (db, new TestDbContextBase(db));
    }

    private static GetSourceLocationQuery CreateQuery(Guid sourceLocationId)
    {
        return new GetSourceLocationQuery()
        {
            Where = new GetSourceLocationQueryParms()
            {
                SourceLocationId = sourceLocationId.ToString(),
                IsActive = false,
            },
            SortCriteria = null,
        };
    }

    private static FtsSourceLocationEntity CreateSourceLocation(Guid id, Guid? ftpServerId)
    {
        return new FtsSourceLocationEntity()
        {
            FtsSourceLocationId = id,
            ConfigId = 123,
            ConfigName = "Cfg",
            SourceProtocolCode = "FTP",
            SourceAddress = "addr",
            SourceFileMask = "mask",
            FtpServerId = ftpServerId,
            SendNotifications = true,
            OwnerName = "Owner",
            ModuleId = Guid.Parse("00000000-0000-0000-0000-000000000111"),
            OwnerPhone = "555",
            OwnerEmail = "owner@example.com",
            ConfigDescription = "desc",
            RunOnMonday = true,
            RunOnTuesday = false,
            RunOnWednesday = true,
            RunOnThursday = false,
            RunOnFriday = true,
            RunOnSaturday = false,
            RunOnSunday = true,
            RunOnFirstDayOfMonth = false,
            RunOnLastDayOfMonth = true,
            RunOnThisDayOfMonth = 7,
            StartTime24hhmm = "0100",
            EndTime24hhmm = "0200",
            FileDirection = "IN",
            TradingPartner = "TP",
            IsValid = true,
            IsSuspended = false,
            OperatorId = "op",
            CreatedAt = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            LastModifiedAt = new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
        };
    }

    private static FtsDestinationLocations CreateDestination(Guid sourceLocationId, Guid? metadataId, DateTime creationDate)
    {
        return new FtsDestinationLocations()
        {
            DestinationId = Guid.NewGuid(),
            FtsSourceLocationId = sourceLocationId,
            DestinationAddress = "dest",
            DestProtocolCD = "FTP",
            FtpServerId = Guid.Parse("00000000-0000-0000-0000-000000000222"),
            OverwriteAtDest = false,
            CompressAtDest = false,
            ReNameAtDest = false,
            DestIsArchive = false,
            IsValid = true,
            OperatorId = "op",
            CreationDate = creationDate,
            LastModifiedDate = creationDate,
            FTEMetadataId = metadataId,
            DestinationType = "T",
            ContractName = "C",
        };
    }

    private static FtsInboundTargetEntity CreateInboundTarget(Guid metadataId, string jobName)
    {
        return new FtsInboundTargetEntity()
        {
            FtsInboundTargetId = Guid.NewGuid(),
            ExternalFileDefnId = metadataId,
            ImportJobName = jobName,
            ContractName = "C",
            OperatorId = "op",
            IsValid = true,
            CreatedAt = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            LastModifiedAt = new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
        };
    }

    private static FtpServer CreateFtpServer(Guid id, string name)
    {
        return new FtpServer()
        {
            FtpServerId = id,
            FtpServerName = name,
            FtpServerIp = "127.0.0.1",
            FtpTransferMode = "A",
            FtpUser = "u",
            FtpPassword = "p",
            FtpDescription = "d",
            FtpPort = 21,
            UseSSHAuth = false,
            IsValid = true,
            OperatorId = "op",
            LastModifiedDate = new DateTime(2020, 1, 2, 0, 0, 0, DateTimeKind.Utc),
            CreationDate = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        };
    }

    [Fact]
    public void F02_014_loads_destination_and_inbound_target_tables_into_memory()
    {
        var (db, context) = CreateContext();

        var sourceLocationId = Guid.NewGuid();
        db.SourceLocations.Add(CreateSourceLocation(sourceLocationId, ftpServerId: null));

        db.Destinations.Add(CreateDestination(sourceLocationId, metadataId: null, creationDate: new DateTime(2020, 1, 1)));
        db.Destinations.Add(CreateDestination(Guid.NewGuid(), metadataId: null, creationDate: new DateTime(2020, 1, 1)));

        db.InboundTargets.Add(CreateInboundTarget(Guid.NewGuid(), "job1"));
        db.InboundTargets.Add(CreateInboundTarget(Guid.NewGuid(), "job2"));

        db.SaveChanges();

        var query = CreateQuery(sourceLocationId);
        var executor = new GetSourceLocationQueryExecutor();
        executor.Execute(context, query);

        var destField = typeof(GetSourceLocationQueryExecutor).GetField("_dest", BindingFlags.NonPublic | BindingFlags.Instance);
        var inboundField = typeof(GetSourceLocationQueryExecutor).GetField("_inboundTarget", BindingFlags.NonPublic | BindingFlags.Instance);

        var dest = Assert.IsType<List<FtsDestinationLocations>>(destField!.GetValue(executor));
        var inbound = Assert.IsType<List<FtsInboundTargetEntity>>(inboundField!.GetValue(executor));

        Assert.Equal(2, dest.Count);
        Assert.Equal(2, inbound.Count);
    }

    [Fact]
    public void F02_015_get_base_query_returns_no_rows_when_no_source_location_matches()
    {
        var (_, context) = CreateContext();
        var query = CreateQuery(Guid.NewGuid());

        var executor = new TestableGetSourceLocationQueryExecutor();
        var rows = executor.GetBaseQueryPublic(context, query).ToList();

        Assert.Empty(rows);
    }

    [Fact]
    public void F02_016_maps_fields_and_looks_up_ftp_server_name_when_present()
    {
        var (db, context) = CreateContext();

        var ftpServerId = Guid.NewGuid();
        db.FtpServers.Add(CreateFtpServer(ftpServerId, "ftp-name"));

        var sourceLocationId = Guid.NewGuid();
        db.SourceLocations.Add(CreateSourceLocation(sourceLocationId, ftpServerId));
        db.SaveChanges();

        var query = CreateQuery(sourceLocationId);
        new GetSourceLocationQueryExecutor().Execute(context, query);

        Assert.Single(query.Results!);
        Assert.Equal(sourceLocationId, query.Results![0].FtsSourceLocationId);
        Assert.Equal(123, query.Results![0].ConfigId);
        Assert.Equal("Cfg", query.Results![0].ConfigName);
        Assert.Equal("desc", query.Results![0].Description);
        Assert.Equal("ftp-name", query.Results![0].FtpServerName);
        Assert.Equal("FTP", query.Results![0].Protocol);
        Assert.Equal("addr", query.Results![0].Address);
        Assert.Equal("mask", query.Results![0].Mask);
        Assert.True(query.Results![0].IsActive);
    }

    [Fact]
    public void F02_017_attaches_destinations_in_creation_date_desc_order_and_resolves_import_job_name()
    {
        var (db, context) = CreateContext();

        var sourceLocationId = Guid.NewGuid();
        db.SourceLocations.Add(CreateSourceLocation(sourceLocationId, ftpServerId: null));

        var meta1 = Guid.NewGuid();
        var meta2 = Guid.NewGuid();
        db.InboundTargets.Add(CreateInboundTarget(meta1, "job1"));
        db.InboundTargets.Add(CreateInboundTarget(meta2, "job2"));

        db.Destinations.Add(CreateDestination(sourceLocationId, meta1, new DateTime(2020, 1, 1)));
        db.Destinations.Add(CreateDestination(sourceLocationId, meta2, new DateTime(2020, 2, 1)));
        db.SaveChanges();

        var query = CreateQuery(sourceLocationId);
        new GetSourceLocationQueryExecutor().Execute(context, query);

        Assert.Single(query.Results!);
        var destinations = query.Results![0].DestinationAddress;
        Assert.Equal(2, destinations.Count);

        Assert.Equal(meta2, destinations[0].FTEMetadataId);
        Assert.Equal("job2", destinations[0].ImportJobName);
        Assert.Equal(meta1, destinations[1].FTEMetadataId);
        Assert.Equal("job1", destinations[1].ImportJobName);
    }

    [Fact]
    public void F02_017_throws_when_destination_metadata_has_no_matching_inbound_target()
    {
        var (db, context) = CreateContext();

        var sourceLocationId = Guid.NewGuid();
        db.SourceLocations.Add(CreateSourceLocation(sourceLocationId, ftpServerId: null));

        db.Destinations.Add(CreateDestination(sourceLocationId, Guid.NewGuid(), new DateTime(2020, 1, 1)));
        db.SaveChanges();

        var query = CreateQuery(sourceLocationId);
        Assert.Throws<NullReferenceException>(() => new GetSourceLocationQueryExecutor().Execute(context, query));
    }

    [Fact]
    public void F02_018_execute_throws_when_no_rows_are_returned_before_assigning_destinations()
    {
        var (_, context) = CreateContext();

        var query = CreateQuery(Guid.NewGuid());
        Assert.Throws<InvalidOperationException>(() => new GetSourceLocationQueryExecutor().Execute(context, query));
    }
}

