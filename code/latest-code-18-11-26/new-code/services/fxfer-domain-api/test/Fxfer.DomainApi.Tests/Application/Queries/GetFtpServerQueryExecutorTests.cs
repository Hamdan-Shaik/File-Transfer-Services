using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Domain.Constants;
using Fxfer.DomainApi.Domain.Entities;
using Fxfer.DomainApi.Infrastructure.DataAccess;

namespace Fxfer.DomainApi.Tests.Application.Queries;

public sealed class GetFtpServerQueryExecutorTests
{
    [Fact]
    public async Task F02_005_filters_by_is_valid_when_provided()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtpServers.AddRange(
                new FtpServer
                {
                    FtpServerId = Guid.NewGuid(),
                    FtpServerName = "A",
                    FtpServerHost = "127.0.0.1",
                    FtpTransferMode = "1",
                    FtpUser = "user",
                    FtpPasswordSecretRef = "secret-ref",
                    FtpDescription = "desc",
                    FtpPort = 21,
                    IsValid = true,
                    UseSshAuth = false,
                    OperatorId = "op",
                    CreationDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow
                },
                new FtpServer
                {
                    FtpServerId = Guid.NewGuid(),
                    FtpServerName = "B",
                    FtpServerHost = "127.0.0.2",
                    FtpTransferMode = "1",
                    FtpUser = "user",
                    FtpPasswordSecretRef = "secret-ref",
                    FtpDescription = "desc",
                    FtpPort = 21,
                    IsValid = false,
                    UseSshAuth = false,
                    OperatorId = "op",
                    CreationDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetFtpServerQueryExecutor();

            var query = new FtpServerQuery { Where = new FtpServerQueryParms { IsValid = true } };
            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.True(results[0].IsValid);
        }
    }

    [Fact]
    public async Task F02_006_defaults_sort_by_ftp_server_name_for_legacy_semantics_only()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtpServers.AddRange(
                new FtpServer
                {
                    FtpServerId = Guid.NewGuid(),
                    FtpServerName = "b",
                    FtpServerHost = "127.0.0.1",
                    FtpTransferMode = "1",
                    FtpUser = "user",
                    FtpPasswordSecretRef = "secret-ref",
                    FtpDescription = "desc",
                    FtpPort = 21,
                    IsValid = true,
                    UseSshAuth = false,
                    OperatorId = "op",
                    CreationDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow
                },
                new FtpServer
                {
                    FtpServerId = Guid.NewGuid(),
                    FtpServerName = "a",
                    FtpServerHost = "127.0.0.2",
                    FtpTransferMode = "1",
                    FtpUser = "user",
                    FtpPasswordSecretRef = "secret-ref",
                    FtpDescription = "desc",
                    FtpPort = 21,
                    IsValid = true,
                    UseSshAuth = false,
                    OperatorId = "op",
                    CreationDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetFtpServerQueryExecutor();
            var query = new FtpServerQuery
            {
                Where = new FtpServerQueryParms
                {
                    IsValid = true,
                    SearchOptions = new List<SearchModeOption>()
                }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Equal(2, results.Count);
            Assert.Equal("a", results[0].FtpServerName);
            Assert.Equal("b", results[1].FtpServerName);
        }
    }

    [Fact]
    public async Task F02_007_and_F02_008_apply_door_specific_filters_for_id_and_trimmed_text_matching()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var ftpServerId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtpServers.AddRange(
                new FtpServer
                {
                    FtpServerId = ftpServerId,
                    FtpServerName = "ALPHA",
                    FtpServerHost = "127.0.0.1",
                    FtpTransferMode = "1",
                    FtpUser = "USER",
                    FtpPasswordSecretRef = "secret-ref",
                    FtpDescription = "DESC",
                    FtpPort = 21,
                    IsValid = true,
                    UseSshAuth = false,
                    OperatorId = "op",
                    CreationDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow
                },
                new FtpServer
                {
                    FtpServerId = Guid.NewGuid(),
                    FtpServerName = "BETA",
                    FtpServerHost = "127.0.0.2",
                    FtpTransferMode = "1",
                    FtpUser = "USER",
                    FtpPasswordSecretRef = "secret-ref",
                    FtpDescription = "DESC",
                    FtpPort = 21,
                    IsValid = true,
                    UseSshAuth = false,
                    OperatorId = "op",
                    CreationDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetFtpServerQueryExecutor();

            var legacyQuery = new FtpServerQuery
            {
                Where = new FtpServerQueryParms
                {
                    IsValid = true,
                    FtpServerId = ftpServerId,
                    FtpServerName = "  alpha ",
                    SearchOptions = new List<SearchModeOption>
                    {
                        new() { FieldName = CommonConstants.SortFields.FtpServer.FtpServerId, FilterType = SearchModeOption.SearchMode.EqualTo },
                        new() { FieldName = CommonConstants.SortFields.FtpServer.FtpServerName, FilterType = SearchModeOption.SearchMode.EqualTo }
                    }
                }
            };

            var legacyResults = await executor.ExecuteAsync(dbContext, legacyQuery, CancellationToken.None);
            Assert.Single(legacyResults);
            Assert.Equal(ftpServerId, legacyResults[0].FtpServerId);

            var newDoorQuery = new FtpServerQuery
            {
                Where = new FtpServerQueryParms
                {
                    IsValid = true,
                    FtpServerName = "  alpha "
                }
            };

            var newDoorResults = await executor.ExecuteAsync(dbContext, newDoorQuery, CancellationToken.None);
            Assert.Empty(newDoorResults);
        }
    }

    [Fact]
    public async Task F02_009_projects_a_fixed_set_of_fields_including_credential_fields()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var ftpServerId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtpServers.Add(new FtpServer
            {
                FtpServerId = ftpServerId,
                FtpServerName = "A",
                FtpServerHost = "127.0.0.1",
                FtpTransferMode = "1",
                FtpUser = "user",
                FtpPasswordSecretRef = "secret-ref-009",
                FtpDescription = "desc",
                FtpPort = 21,
                IsValid = true,
                UseSshAuth = false,
                OperatorId = "op",
                CreationDate = DateTimeOffset.UtcNow,
                LastModifiedDate = DateTimeOffset.UtcNow
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetFtpServerQueryExecutor();
            var query = new FtpServerQuery
            {
                Where = new FtpServerQueryParms
                {
                    IsValid = true,
                    SearchOptions = new List<SearchModeOption>()
                }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal(ftpServerId, results[0].FtpServerId);
            Assert.Equal("secret-ref-009", results[0].FtpPassword);
        }
    }

    [Fact]
    public async Task F02_031_supports_duplicate_name_detection_by_exact_match_on_trimmed_uppercase_name()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var existingId = Guid.NewGuid();
        var candidateId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtpServers.AddRange(
                new FtpServer
                {
                    FtpServerId = existingId,
                    FtpServerName = "ALPHA",
                    FtpServerHost = "127.0.0.1",
                    FtpTransferMode = "1",
                    FtpUser = "user",
                    FtpPasswordSecretRef = "secret-ref",
                    FtpDescription = "desc",
                    FtpPort = 21,
                    IsValid = true,
                    UseSshAuth = false,
                    OperatorId = "op",
                    CreationDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow
                },
                new FtpServer
                {
                    FtpServerId = Guid.NewGuid(),
                    FtpServerName = "BETA",
                    FtpServerHost = "127.0.0.2",
                    FtpTransferMode = "1",
                    FtpUser = "user",
                    FtpPasswordSecretRef = "secret-ref",
                    FtpDescription = "desc",
                    FtpPort = 21,
                    IsValid = true,
                    UseSshAuth = false,
                    OperatorId = "op",
                    CreationDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetFtpServerQueryExecutor();
            var query = new FtpServerQuery
            {
                Where = new FtpServerQueryParms
                {
                    IsValid = true,
                    FtpServerName = "  alpha ",
                    SearchOptions = new List<SearchModeOption>
                    {
                        new()
                        {
                            FieldName = CommonConstants.SortFields.FtpServer.FtpServerName,
                            FilterType = SearchModeOption.SearchMode.EqualTo
                        }
                    }
                }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal(existingId, results[0].FtpServerId);
            Assert.NotEqual(candidateId, results[0].FtpServerId);
        }
    }
}

