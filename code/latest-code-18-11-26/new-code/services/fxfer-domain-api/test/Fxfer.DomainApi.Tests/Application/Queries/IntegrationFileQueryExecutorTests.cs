using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Tests.Application.Queries;

public sealed class IntegrationFileQueryExecutorTests
{
    [Fact]
    public async Task F03_005_returns_only_active_integration_file_definitions()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsIntegrationFile.AddRange(
                new FtsIntegrationFile
                {
                    FtsIntegrationFileId = Guid.NewGuid(),
                    GroupId = Guid.NewGuid(),
                    ConfigId = 1,
                    FileName = "ACTIVE",
                    ETFOutboundMetadataDefnId = Guid.NewGuid(),
                    IsProcessed = false,
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsIntegrationFile
                {
                    FtsIntegrationFileId = Guid.NewGuid(),
                    GroupId = Guid.NewGuid(),
                    ConfigId = 2,
                    FileName = "INACTIVE",
                    ETFOutboundMetadataDefnId = Guid.NewGuid(),
                    IsProcessed = false,
                    OperatorId = "op",
                    IsValid = false,
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new IntegrationFileQueryExecutor();
            var query = new GetIntegrationFileQuery { Where = new IntegrationFileQueryParms() };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal("ACTIVE", results[0].FileName);
        }
    }

    [Fact]
    public async Task F03_006_filters_by_id_only_when_a_non_empty_id_is_supplied()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;
        var id1 = Guid.Parse("00000000-0000-0000-0000-000000000111");
        var id2 = Guid.Parse("00000000-0000-0000-0000-000000000222");

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsIntegrationFile.AddRange(
                new FtsIntegrationFile
                {
                    FtsIntegrationFileId = id1,
                    GroupId = Guid.NewGuid(),
                    ConfigId = 1,
                    FileName = "A",
                    ETFOutboundMetadataDefnId = Guid.NewGuid(),
                    IsProcessed = false,
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsIntegrationFile
                {
                    FtsIntegrationFileId = id2,
                    GroupId = Guid.NewGuid(),
                    ConfigId = 2,
                    FileName = "B",
                    ETFOutboundMetadataDefnId = Guid.NewGuid(),
                    IsProcessed = false,
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new IntegrationFileQueryExecutor();

            var noIdQuery = new GetIntegrationFileQuery { Where = new IntegrationFileQueryParms { FTSIntegrationFileId = Guid.Empty } };
            var noIdResults = await executor.ExecuteAsync(dbContext, noIdQuery, CancellationToken.None);
            Assert.Equal(2, noIdResults.Count);

            var idQuery = new GetIntegrationFileQuery { Where = new IntegrationFileQueryParms { FTSIntegrationFileId = id1 } };
            var idResults = await executor.ExecuteAsync(dbContext, idQuery, CancellationToken.None);
            Assert.Single(idResults);
            Assert.Equal(id1, idResults[0].FtsIntegrationFileId);
        }
    }

    [Fact]
    public async Task F03_007_filters_by_file_name_fragment_only_when_fragment_is_supplied()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsIntegrationFile.AddRange(
                new FtsIntegrationFile
                {
                    FtsIntegrationFileId = Guid.NewGuid(),
                    GroupId = Guid.NewGuid(),
                    ConfigId = 1,
                    FileName = "ABC-123",
                    ETFOutboundMetadataDefnId = Guid.NewGuid(),
                    IsProcessed = false,
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsIntegrationFile
                {
                    FtsIntegrationFileId = Guid.NewGuid(),
                    GroupId = Guid.NewGuid(),
                    ConfigId = 2,
                    FileName = "XYZ-999",
                    ETFOutboundMetadataDefnId = Guid.NewGuid(),
                    IsProcessed = false,
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new IntegrationFileQueryExecutor();
            var query = new GetIntegrationFileQuery
            {
                Where = new IntegrationFileQueryParms { FileName = "123" }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal("ABC-123", results[0].FileName);
        }
    }

    [Fact]
    public async Task F03_008_does_not_apply_any_explicit_ordering()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsIntegrationFile.AddRange(
                new FtsIntegrationFile
                {
                    FtsIntegrationFileId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    GroupId = Guid.NewGuid(),
                    ConfigId = 1,
                    FileName = "B",
                    ETFOutboundMetadataDefnId = Guid.NewGuid(),
                    IsProcessed = false,
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsIntegrationFile
                {
                    FtsIntegrationFileId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    GroupId = Guid.NewGuid(),
                    ConfigId = 2,
                    FileName = "A",
                    ETFOutboundMetadataDefnId = Guid.NewGuid(),
                    IsProcessed = false,
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new IntegrationFileQueryExecutor();
            var query = new GetIntegrationFileQuery { Where = new IntegrationFileQueryParms() };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Equal(2, results.Count);
            Assert.Equal("B", results[0].FileName);
            Assert.Equal("A", results[1].FileName);
        }
    }

    [Fact]
    public async Task F03_009_projects_only_a_subset_of_fields_leaving_other_model_fields_at_defaults()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;
        var integrationFileId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var metadataId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsIntegrationFile.Add(new FtsIntegrationFile
            {
                FtsIntegrationFileId = integrationFileId,
                GroupId = groupId,
                ConfigId = 123,
                FileName = "FILE-009",
                ETFOutboundMetadataDefnId = metadataId,
                IsProcessed = true,
                OperatorId = "op",
                IsValid = true,
                CreationDate = now,
                LastModifiedDate = now
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new IntegrationFileQueryExecutor();
            var query = new GetIntegrationFileQuery { Where = new IntegrationFileQueryParms { FTSIntegrationFileId = integrationFileId } };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);

            Assert.Equal(groupId, results[0].GroupId);
            Assert.Equal(metadataId, results[0].ETFOutboundMetadataDefnId);
            Assert.Equal(123, results[0].ConfigId);
            Assert.Equal("FILE-009", results[0].FileName);
            Assert.True(results[0].IsProcessed);
            Assert.Equal(integrationFileId, results[0].FtsIntegrationFileId);

            Assert.False(results[0].IsValid);
            Assert.Equal(default, results[0].CreationDate);
            Assert.Equal(default, results[0].LastModifiedDate);
        }
    }

    [Fact]
    public async Task F03_013_inactive_definitions_do_not_appear_in_later_lookups()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;
        var inactiveId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsIntegrationFile.Add(new FtsIntegrationFile
            {
                FtsIntegrationFileId = inactiveId,
                GroupId = Guid.NewGuid(),
                ConfigId = 1,
                FileName = "INACTIVE-013",
                ETFOutboundMetadataDefnId = Guid.NewGuid(),
                IsProcessed = false,
                OperatorId = "op",
                IsValid = false,
                CreationDate = now,
                LastModifiedDate = now
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new IntegrationFileQueryExecutor();
            var query = new GetIntegrationFileQuery { Where = new IntegrationFileQueryParms { FTSIntegrationFileId = inactiveId } };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Empty(results);
        }
    }
}

