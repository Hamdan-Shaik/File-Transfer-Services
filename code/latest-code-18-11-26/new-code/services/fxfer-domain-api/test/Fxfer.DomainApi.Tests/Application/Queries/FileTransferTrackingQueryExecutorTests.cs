using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Domain.Constants;
using Fxfer.DomainApi.Domain.Entities;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Tests.Application.Queries;

public sealed class FileTransferTrackingQueryExecutorTests
{
    [Fact]
    public async Task F05_023_treats_invalid_file_and_source_location_references_as_system_default()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.NewGuid();
        var file1Id = Guid.NewGuid();
        var file2Id = Guid.NewGuid();

        var now = new DateTime(2026, 6, 1, 2, 3, 4, DateTimeKind.Utc);

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigName = "UNC-CONFIG",
                SourceProtocolCD = DataAccessConstants.FileStatusConstant.UNCProtocolCD,
                SourceAddress = "unc://share",
                FileDirection = "IN",
                TradingPartner = "TP1",
                IsValid = true
            });

            dbContext.FtsFileManagement.AddRange(
                new FtsFileManagement { FileId = file1Id, FileSourceLocationId = sourceLocationId, FileName = "a.txt", FileStatus = "1", OperatorId = "op", FileSize = 10m, CreatedDate = now, LastModifiedTs = now, FileStatusTs = now, GroupId = Guid.NewGuid() },
                new FtsFileManagement { FileId = file2Id, FileSourceLocationId = sourceLocationId, FileName = "b.txt", FileStatus = "1", OperatorId = "op", FileSize = 10m, CreatedDate = now, LastModifiedTs = now, FileStatusTs = now, GroupId = Guid.NewGuid() });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new FileTransferTrackingQueryExecutor();
            var query = new FileTransferTrackingQuery
            {
                Where = new FileTransferTrackingQueryParms
                {
                    FileId = "not-a-guid",
                    FileSourceLocationId = "also-not-a-guid"
                }
            };

            var results = (await executor.ExecuteAsync(dbContext, query, CancellationToken.None)).ToList();

            Assert.Equal(2, results.Count);
            Assert.Contains(results, r => r.FileId == file1Id);
            Assert.Contains(results, r => r.FileId == file2Id);
        }
    }

    [Fact]
    public async Task F05_025_and_F05_026_unions_unc_and_ftp_results_and_enriches_with_events_and_exceptions()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var ftpServerId = Guid.NewGuid();
        var uncSourceLocationId = Guid.NewGuid();
        var ftpSourceLocationId = Guid.NewGuid();
        var invalidSourceLocationId = Guid.NewGuid();

        var fileUncId = Guid.NewGuid();
        var fileFtpId = Guid.NewGuid();
        var invalidFileId = Guid.NewGuid();

        var now = new DateTime(2026, 3, 4, 5, 6, 7, DateTimeKind.Utc);

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtpServers.Add(new FtpServer
            {
                FtpServerId = ftpServerId,
                FtpServerName = "FTP-A",
                FtpServerHost = "127.0.0.1",
                FtpTransferMode = "1",
                FtpUser = "user",
                FtpPasswordSecretRef = "secret-ref",
                FtpDescription = "desc",
                FtpPort = 21,
                FtpProxyAddress = null,
                FtpProxyPort = null,
                IsValid = true,
                UseSshAuth = false,
                OperatorId = "op",
                CreationDate = new DateTimeOffset(now),
                LastModifiedDate = new DateTimeOffset(now)
            });

            dbContext.FtsSourceLocations.AddRange(
                new FtsSourceLocation
                {
                    FtsSourceLocationId = uncSourceLocationId,
                    ConfigName = "UNC-CONFIG",
                    SourceProtocolCD = DataAccessConstants.FileStatusConstant.UNCProtocolCD,
                    SourceAddress = "unc://share",
                    FileDirection = "IN",
                    TradingPartner = "TP1",
                    IsValid = true
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = ftpSourceLocationId,
                    ConfigName = "FTP-CONFIG",
                    SourceProtocolCD = "1",
                    SourceAddress = "ftp://host",
                    FileDirection = "OUT",
                    TradingPartner = "TP2",
                    FtpServerId = ftpServerId,
                    IsValid = true
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = invalidSourceLocationId,
                    ConfigName = "INVALID-CONFIG",
                    SourceProtocolCD = DataAccessConstants.FileStatusConstant.UNCProtocolCD,
                    SourceAddress = "unc://invalid",
                    FileDirection = "IN",
                    TradingPartner = "TP3",
                    IsValid = false
                });

            dbContext.FtsFileManagement.AddRange(
                new FtsFileManagement { FileId = fileUncId, FileSourceLocationId = uncSourceLocationId, FileName = "b.txt", FileStatus = "1", OperatorId = "op", FileSize = 10m, CreatedDate = now, LastModifiedTs = now, FileStatusTs = now, GroupId = Guid.NewGuid() },
                new FtsFileManagement { FileId = fileFtpId, FileSourceLocationId = ftpSourceLocationId, FileName = "a.txt", FileStatus = "1", OperatorId = "op", FileSize = 20m, CreatedDate = now.AddMinutes(1), LastModifiedTs = now, FileStatusTs = now, GroupId = Guid.NewGuid() },
                new FtsFileManagement { FileId = invalidFileId, FileSourceLocationId = invalidSourceLocationId, FileName = "c.txt", FileStatus = "1", OperatorId = "op", FileSize = 30m, CreatedDate = now, LastModifiedTs = now, FileStatusTs = now, GroupId = Guid.NewGuid() });

            dbContext.FtsFileEvents.Add(new FtsFileEvent { EventId = Guid.NewGuid(), FileId = fileUncId, DestinationId = Guid.NewGuid(), EventCD = "E1", CreatedDate = now.AddDays(-1), OperatorId = "op", LastModifiedTs = now });
            dbContext.FtsFileExceptions.Add(new FtsFileException { ExceptionId = Guid.NewGuid(), FileId = fileUncId, DestinationId = Guid.NewGuid(), EventId = Guid.NewGuid(), ExceptionType = "X", MethodName = "M", CreatedDate = now.AddDays(-2), OperatorId = "op", LastModifiedTs = now });

            dbContext.FtsFileEvents.Add(new FtsFileEvent { EventId = Guid.NewGuid(), FileId = fileFtpId, DestinationId = Guid.NewGuid(), EventCD = "E2", CreatedDate = now.AddDays(-1), OperatorId = "op", LastModifiedTs = now });
            dbContext.FtsFileExceptions.Add(new FtsFileException { ExceptionId = Guid.NewGuid(), FileId = fileFtpId, DestinationId = Guid.NewGuid(), EventId = Guid.NewGuid(), ExceptionType = "Y", MethodName = "M", CreatedDate = now.AddDays(-2), OperatorId = "op", LastModifiedTs = now });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new FileTransferTrackingQueryExecutor();
            var query = new FileTransferTrackingQuery { Where = new FileTransferTrackingQueryParms() };

            var results = (await executor.ExecuteAsync(dbContext, query, CancellationToken.None)).ToList();

            Assert.Equal(2, results.Count);
            Assert.Equal("a.txt", results[0].FileName);
            Assert.Equal("b.txt", results[1].FileName);

            var ftp = results.Single(r => r.FileId == fileFtpId);
            Assert.Equal("FTP-A", ftp.OriginatingServerName);
            Assert.NotNull(ftp.Events);
            Assert.NotNull(ftp.Exceptions);
            Assert.Single(ftp.Events);
            Assert.Single(ftp.Exceptions);

            var unc = results.Single(r => r.FileId == fileUncId);
            Assert.Equal(string.Empty, unc.OriginatingServerName);
            Assert.NotNull(unc.Events);
            Assert.NotNull(unc.Exceptions);
            Assert.Single(unc.Events);
            Assert.Single(unc.Exceptions);
        }
    }

    [Fact]
    public async Task F05_023_filters_by_exact_match_and_inclusive_ranges_when_filters_are_provided()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.NewGuid();

        var file1Id = Guid.NewGuid();
        var file2Id = Guid.NewGuid();
        var file3Id = Guid.NewGuid();

        var start = new DateTime(2026, 5, 6, 7, 8, 9, DateTimeKind.Utc);
        var end = start.AddDays(1);

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigName = "UNC-CONFIG",
                SourceProtocolCD = DataAccessConstants.FileStatusConstant.UNCProtocolCD,
                SourceAddress = "unc://share",
                FileDirection = "IN",
                TradingPartner = "TP1",
                IsValid = true
            });

            dbContext.FtsFileManagement.AddRange(
                new FtsFileManagement { FileId = file1Id, FileSourceLocationId = sourceLocationId, FileName = "A.txt", FileStatus = "S1", OperatorId = "op", FileSize = 10m, CreatedDate = start, LastModifiedTs = start, FileStatusTs = start, GroupId = Guid.NewGuid() },
                new FtsFileManagement { FileId = file2Id, FileSourceLocationId = sourceLocationId, FileName = "B.txt", FileStatus = "S1", OperatorId = "op", FileSize = 15m, CreatedDate = end, LastModifiedTs = start, FileStatusTs = start, GroupId = Guid.NewGuid() },
                new FtsFileManagement { FileId = file3Id, FileSourceLocationId = sourceLocationId, FileName = "C.txt", FileStatus = "S2", OperatorId = "op", FileSize = 20m, CreatedDate = end.AddDays(1), LastModifiedTs = start, FileStatusTs = start, GroupId = Guid.NewGuid() });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new FileTransferTrackingQueryExecutor();
            var query = new FileTransferTrackingQuery
            {
                Where = new FileTransferTrackingQueryParms
                {
                    FileSourceLocationId = sourceLocationId.ToString(),
                    FileStatus = "S1",
                    TradingPartner = "TP1",
                    StartDate = start,
                    EndDate = end,
                    FileSizeFrom = 10m,
                    FileSizeThrough = 15m
                }
            };

            var results = (await executor.ExecuteAsync(dbContext, query, CancellationToken.None)).ToList();

            Assert.Equal(2, results.Count);
            Assert.Contains(results, r => r.FileId == file1Id);
            Assert.Contains(results, r => r.FileId == file2Id);
            Assert.DoesNotContain(results, r => r.FileId == file3Id);
        }
    }

    [Fact]
    public async Task F05_024_defaults_to_sort_by_file_name_ascending_when_no_sort_criteria_is_provided()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.NewGuid();
        var fileAId = Guid.NewGuid();
        var fileBId = Guid.NewGuid();

        var now = new DateTime(2026, 6, 1, 2, 3, 4, DateTimeKind.Utc);

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigName = "UNC-CONFIG",
                SourceProtocolCD = DataAccessConstants.FileStatusConstant.UNCProtocolCD,
                SourceAddress = "unc://share",
                FileDirection = "IN",
                TradingPartner = "TP1",
                IsValid = true
            });

            dbContext.FtsFileManagement.AddRange(
                new FtsFileManagement { FileId = fileBId, FileSourceLocationId = sourceLocationId, FileName = "b.txt", FileStatus = "1", OperatorId = "op", FileSize = 10m, CreatedDate = now, LastModifiedTs = now, FileStatusTs = now, GroupId = Guid.NewGuid() },
                new FtsFileManagement { FileId = fileAId, FileSourceLocationId = sourceLocationId, FileName = "a.txt", FileStatus = "1", OperatorId = "op", FileSize = 10m, CreatedDate = now, LastModifiedTs = now, FileStatusTs = now, GroupId = Guid.NewGuid() });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new FileTransferTrackingQueryExecutor();
            var query = new FileTransferTrackingQuery { Where = new FileTransferTrackingQueryParms() };

            var results = (await executor.ExecuteAsync(dbContext, query, CancellationToken.None)).ToList();

            Assert.Equal(2, results.Count);
            Assert.Equal("a.txt", results[0].FileName);
            Assert.Equal("b.txt", results[1].FileName);
        }
    }
}

