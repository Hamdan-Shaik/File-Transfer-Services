using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Tests.Application.Queries;

public sealed class SearchSourceLocationQueryExecutorTests
{
    private static DbContextOptions<FxferDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task F01_091_projects_source_location_rows_into_source_location_model_results()
    {
        var options = CreateOptions();

        var now = DateTimeOffset.UtcNow;
        var ownerModule = Guid.NewGuid();
        var sourceLocationId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigId = 10,
                ConfigName = "CFG",
                OwnerName = "Owner",
                OwnerModule = ownerModule,
                SourceProtocolCD = "SFTP",
                SourceAddress = "10.0.0.1",
                SourceFileMask = "*.txt",
                IsValid = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();
            var query = new SourceLocationQuery { Where = new SourceLocationQueryParms() };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal(sourceLocationId, results[0].FtsSourceLocationId);
            Assert.Equal(10, results[0].ConfigId);
            Assert.Equal("CFG", results[0].ConfigName);
            Assert.Equal("Owner", results[0].OwnerName);
            Assert.Equal(ownerModule, results[0].OwnerSubsystem);
            Assert.Equal("SFTP", results[0].Protocol);
            Assert.Equal("10.0.0.1", results[0].Address);
            Assert.Equal("*.txt", results[0].Mask);
            Assert.True(results[0].IsActive);
        }
    }

    [Fact]
    public async Task F01_092_filters_by_config_id_when_config_id_is_greater_than_zero()
    {
        var options = CreateOptions();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.AddRange(
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 1,
                    ConfigName = "A",
                    OwnerName = "o",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "FTP",
                    SourceAddress = "a",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 2,
                    ConfigName = "B",
                    OwnerName = "o",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "FTP",
                    SourceAddress = "b",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();
            var query = new SourceLocationQuery { Where = new SourceLocationQueryParms { ConfigId = 2 } };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal(2, results[0].ConfigId);
        }
    }

    [Fact]
    public async Task F01_093_filters_by_owner_name_using_case_insensitive_exact_match()
    {
        var options = CreateOptions();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.AddRange(
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 1,
                    ConfigName = "A",
                    OwnerName = "ALICE",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "FTP",
                    SourceAddress = "a",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 2,
                    ConfigName = "B",
                    OwnerName = "BOB",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "FTP",
                    SourceAddress = "b",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();
            var query = new SourceLocationQuery { Where = new SourceLocationQueryParms { OwnerName = "alice" } };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal("ALICE", results[0].OwnerName);
        }
    }

    [Fact]
    public async Task F01_094_filters_by_config_name_using_case_insensitive_exact_match()
    {
        var options = CreateOptions();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.AddRange(
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 1,
                    ConfigName = "CONFIG-A",
                    OwnerName = "o",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "FTP",
                    SourceAddress = "a",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 2,
                    ConfigName = "CONFIG-B",
                    OwnerName = "o",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "FTP",
                    SourceAddress = "b",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();
            var query = new SourceLocationQuery { Where = new SourceLocationQueryParms { ConfigName = "config-b" } };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal("CONFIG-B", results[0].ConfigName);
        }
    }

    [Fact]
    public async Task F01_095_filters_by_owner_subsystem_guid_parsed_from_the_owner_subsystem_string()
    {
        var options = CreateOptions();
        var now = DateTimeOffset.UtcNow;
        var targetOwnerModule = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.AddRange(
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 1,
                    ConfigName = "A",
                    OwnerName = "o",
                    OwnerModule = targetOwnerModule,
                    SourceProtocolCD = "FTP",
                    SourceAddress = "a",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 2,
                    ConfigName = "B",
                    OwnerName = "o",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "FTP",
                    SourceAddress = "b",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();
            var query = new SourceLocationQuery { Where = new SourceLocationQueryParms { OwnerSubsystem = targetOwnerModule.ToString() } };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal(targetOwnerModule, results[0].OwnerSubsystem);
        }
    }

    [Fact]
    public async Task F01_095_throws_when_owner_subsystem_is_not_a_guid()
    {
        var options = CreateOptions();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = Guid.NewGuid(),
                ConfigId = 1,
                ConfigName = "A",
                OwnerName = "o",
                OwnerModule = Guid.NewGuid(),
                SourceProtocolCD = "FTP",
                SourceAddress = "a",
                SourceFileMask = "*",
                IsValid = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();
            var query = new SourceLocationQuery { Where = new SourceLocationQueryParms { OwnerSubsystem = "not-a-guid" } };

            var exception = await Assert.ThrowsAnyAsync<Exception>(() => executor.ExecuteAsync(dbContext, query, CancellationToken.None));

            Exception? inner = exception;
            while (inner is not null && inner is not FormatException)
            {
                inner = inner.InnerException;
            }

            Assert.IsType<FormatException>(inner);
        }
    }

    [Fact]
    public async Task F01_096_filters_by_protocol_when_protocol_is_provided()
    {
        var options = CreateOptions();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.AddRange(
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 1,
                    ConfigName = "A",
                    OwnerName = "o",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "FTP",
                    SourceAddress = "a",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 2,
                    ConfigName = "B",
                    OwnerName = "o",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "SFTP",
                    SourceAddress = "b",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();
            var query = new SourceLocationQuery { Where = new SourceLocationQueryParms { Protocol = "SFTP" } };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal("SFTP", results[0].Protocol);
        }
    }

    [Fact]
    public async Task F01_097_filters_to_active_source_locations_only_when_is_active_is_true()
    {
        var options = CreateOptions();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.AddRange(
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 1,
                    ConfigName = "A",
                    OwnerName = "o",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "FTP",
                    SourceAddress = "a",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 2,
                    ConfigName = "B",
                    OwnerName = "o",
                    OwnerModule = Guid.NewGuid(),
                    SourceProtocolCD = "FTP",
                    SourceAddress = "b",
                    SourceFileMask = "*",
                    IsValid = false,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();

            var activeOnlyQuery = new SourceLocationQuery { Where = new SourceLocationQueryParms { IsActive = true } };
            var activeOnly = await executor.ExecuteAsync(dbContext, activeOnlyQuery, CancellationToken.None);
            Assert.Single(activeOnly);
            Assert.True(activeOnly[0].IsActive);

            var noFilterQuery = new SourceLocationQuery { Where = new SourceLocationQueryParms { IsActive = false } };
            var noFilter = await executor.ExecuteAsync(dbContext, noFilterQuery, CancellationToken.None);
            Assert.Equal(2, noFilter.Count);
        }
    }

    [Fact]
    public async Task F01_098_applies_default_sort_by_config_id_asc_then_is_active_desc()
    {
        var options = CreateOptions();
        var now = DateTimeOffset.UtcNow;
        var ownerModule = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.AddRange(
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 2,
                    ConfigName = "B",
                    OwnerName = "o",
                    OwnerModule = ownerModule,
                    SourceProtocolCD = "FTP",
                    SourceAddress = "b",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 1,
                    ConfigName = "A",
                    OwnerName = "o",
                    OwnerModule = ownerModule,
                    SourceProtocolCD = "FTP",
                    SourceAddress = "a",
                    SourceFileMask = "*",
                    IsValid = false,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 1,
                    ConfigName = "A2",
                    OwnerName = "o",
                    OwnerModule = ownerModule,
                    SourceProtocolCD = "FTP",
                    SourceAddress = "a2",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();
            var query = new SourceLocationQuery { Where = new SourceLocationQueryParms() };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Equal(3, results.Count);
            Assert.Equal(1, results[0].ConfigId);
            Assert.True(results[0].IsActive);
            Assert.Equal(1, results[1].ConfigId);
            Assert.False(results[1].IsActive);
            Assert.Equal(2, results[2].ConfigId);
        }
    }

    [Fact]
    public async Task F01_098_supports_sorting_by_config_name_desc()
    {
        var options = CreateOptions();
        var now = DateTimeOffset.UtcNow;
        var ownerModule = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.AddRange(
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 1,
                    ConfigName = "A",
                    OwnerName = "o",
                    OwnerModule = ownerModule,
                    SourceProtocolCD = "FTP",
                    SourceAddress = "a",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = Guid.NewGuid(),
                    ConfigId = 2,
                    ConfigName = "B",
                    OwnerName = "o",
                    OwnerModule = ownerModule,
                    SourceProtocolCD = "FTP",
                    SourceAddress = "b",
                    SourceFileMask = "*",
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();
            var query = new SourceLocationQuery
            {
                Where = new SourceLocationQueryParms(),
                SortCriteria = new List<SortCriteria>
                {
                    new() { FieldName = "ConfigName", Order = SortOrder.Desc }
                }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Equal(2, results.Count);
            Assert.Equal("B", results[0].ConfigName);
            Assert.Equal("A", results[1].ConfigName);
        }
    }

    [Fact]
    public async Task F01_099_materializes_results_to_a_list()
    {
        var options = CreateOptions();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = Guid.NewGuid(),
                ConfigId = 1,
                ConfigName = "A",
                OwnerName = "o",
                OwnerModule = Guid.NewGuid(),
                SourceProtocolCD = "FTP",
                SourceAddress = "a",
                SourceFileMask = "*",
                IsValid = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new SearchSourceLocationQueryExecutor();
            var query = new SourceLocationQuery { Where = new SourceLocationQueryParms() };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.IsType<List<Fxfer.DomainApi.Contracts.Domain.SourceLocationModel>>(results);
        }
    }
}

