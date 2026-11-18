using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Domain.Entities;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Tests.Application.Queries;

public sealed class GetSourceLocationQueryExecutorTests
{
    [Fact]
    public async Task maps_source_location_and_assigns_destination_records_ordered_by_creation_date_desc()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.Parse("00000000-0000-0000-0000-000000000111");
        var ftpServerId = Guid.Parse("00000000-0000-0000-0000-000000000222");

        var now = new DateTimeOffset(new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc));

        var fteMetadataId = Guid.Parse("00000000-0000-0000-0000-000000000333");

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigId = 10,
                ConfigName = "CONFIG-10",
                Description = "desc",
                SourceProtocolCD = "FTP",
                SourceAddress = "10.0.0.1",
                SourceFileMask = "*.txt",
                FtpServerId = ftpServerId,
                SendNotificationsInd = true,
                OwnerName = "owner",
                OwnerModule = Guid.Parse("00000000-0000-0000-0000-000000000444"),
                OwnerPhone = "555",
                OwnerEmail = "owner@example.com",
                FileDirection = "IN",
                TradingPartner = "tp",
                RunOnMondayInd = true,
                RunOnTuesdayInd = false,
                RunOnWednesdayInd = true,
                RunOnThursdayInd = false,
                RunOnFridayInd = true,
                RunOnSaturdayInd = false,
                RunOnSundayInd = false,
                RunOnFirstDayOfMonthInd = false,
                RunOnLastDayOfMonthInd = true,
                RunOnThisDayOfMonth = 12,
                StartTime24HHMM = "0000",
                EndTime24HHMM = "2359",
                IsValid = true,
                IsSuspended = false,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtpServers.Add(new FtpServer
            {
                FtpServerId = ftpServerId,
                FtpServerName = "FTP-A",
                FtpServerHost = "127.0.0.1",
                FtpTransferMode = "1",
                FtpUser = "user",
                FtpPasswordSecretRef = "secret-ref",
                FtpDescription = "d",
                FtpPort = 21,
                IsValid = true,
                UseSshAuth = false,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsInboundTarget.Add(new FtsInboundTarget
            {
                FtsInboundTargetId = Guid.NewGuid(),
                FTEMetadataId = fteMetadataId,
                ImportJobName = "JOB-333",
                ContractName = "c",
                OperatorId = "op",
                IsValid = true,
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsDestinationLocations.AddRange(
                new FtsDestinationLocations
                {
                    DestinationId = Guid.Parse("00000000-0000-0000-0000-000000000010"),
                    FtsSourceLocationId = sourceLocationId,
                    DestinationAddress = "older",
                    DestProtocolCD = "FTP",
                    OverwriteAtDest = false,
                    CompressAtDest = false,
                    RenameAtDest = false,
                    DestIsArchive = false,
                    IsValid = true,
                    FtpServerId = null,
                    RenamePattern = "r",
                    ContactName = "cn",
                    ContactPhone = "cp",
                    ContactEmail = "ce",
                    OperatorId = "op",
                    CreationDate = now.AddDays(-1),
                    LastModifiedDate = now,
                    FTEMetadataId = null,
                    DestinationType = "t",
                    ContractName = "contract"
                },
                new FtsDestinationLocations
                {
                    DestinationId = Guid.Parse("00000000-0000-0000-0000-000000000011"),
                    FtsSourceLocationId = sourceLocationId,
                    DestinationAddress = "newer",
                    DestProtocolCD = "FTP",
                    OverwriteAtDest = true,
                    CompressAtDest = true,
                    RenameAtDest = true,
                    DestIsArchive = true,
                    IsValid = true,
                    FtpServerId = Guid.NewGuid(),
                    RenamePattern = "r",
                    ContactName = "cn",
                    ContactPhone = "cp",
                    ContactEmail = "ce",
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now,
                    FTEMetadataId = fteMetadataId,
                    DestinationType = "t",
                    ContractName = "contract"
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = sourceLocationId.ToString() }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal(sourceLocationId, results[0].FtsSourceLocationId);
            Assert.Equal("FTP-A", results[0].FtpServerName);

            Assert.NotNull(results[0].DestinationAddress);
            Assert.Equal(2, results[0].DestinationAddress!.Count);
            Assert.Equal("newer", results[0].DestinationAddress[0].DestinationAddress);
            Assert.Equal("older", results[0].DestinationAddress[1].DestinationAddress);
            Assert.Equal("JOB-333", results[0].DestinationAddress[0].ImportJobName);
            Assert.Equal(string.Empty, results[0].DestinationAddress[1].ImportJobName);
        }
    }

    [Fact]
    public async Task throws_when_source_location_row_is_not_found()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = Guid.NewGuid().ToString() }
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => executor.ExecuteAsync(dbContext, query, CancellationToken.None));
        }
    }

    [Fact]
    public async Task throws_when_import_job_lookup_row_is_missing()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        var fteMetadataId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigId = 1,
                ConfigName = "c",
                SourceProtocolCD = "FTP",
                SourceAddress = "addr",
                OwnerModule = Guid.NewGuid(),
                IsValid = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsDestinationLocations.Add(new FtsDestinationLocations
            {
                DestinationId = Guid.NewGuid(),
                FtsSourceLocationId = sourceLocationId,
                DestinationAddress = "a",
                DestProtocolCD = "FTP",
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now,
                FTEMetadataId = fteMetadataId,
                IsValid = true
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = sourceLocationId.ToString() }
            };

            await Assert.ThrowsAsync<NullReferenceException>(() => executor.ExecuteAsync(dbContext, query, CancellationToken.None));
        }
    }

    [Fact]
    public async Task F01_014_returns_no_source_location_when_multiple_rows_match_the_requested_id()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            // shortcut: EF InMemory enforces unique primary keys, so we can only cover the
            // "zero rows" path in tests ("more than one row" is defensive legacy behavior).
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = Guid.NewGuid(),
                ConfigId = 1,
                ConfigName = "c",
                SourceProtocolCD = "FTP",
                SourceAddress = "addr",
                OwnerModule = Guid.NewGuid(),
                IsValid = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = sourceLocationId.ToString() }
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => executor.ExecuteAsync(dbContext, query, CancellationToken.None));
        }
    }

    [Fact]
    public async Task F01_015_includes_is_active_and_suspended_from_the_source_location_row()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigId = 1,
                ConfigName = "c",
                SourceProtocolCD = "FTP",
                SourceAddress = "addr",
                OwnerModule = Guid.NewGuid(),
                IsValid = false,
                IsSuspended = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsDestinationLocations.Add(new FtsDestinationLocations
            {
                DestinationId = Guid.NewGuid(),
                FtsSourceLocationId = sourceLocationId,
                DestinationAddress = "a",
                DestProtocolCD = "FTP",
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now,
                IsValid = true
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = sourceLocationId.ToString() }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.False(results[0].IsActive);
            Assert.True(results[0].Suspended);
        }
    }

    [Fact]
    public async Task F01_016_orders_destination_addresses_by_creation_date_descending()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.NewGuid();
        var now = new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigId = 1,
                ConfigName = "c",
                SourceProtocolCD = "FTP",
                SourceAddress = "addr",
                OwnerModule = Guid.NewGuid(),
                IsValid = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsDestinationLocations.AddRange(
                new FtsDestinationLocations
                {
                    DestinationId = Guid.NewGuid(),
                    FtsSourceLocationId = sourceLocationId,
                    DestinationAddress = "older",
                    DestProtocolCD = "FTP",
                    OperatorId = "op",
                    CreationDate = now.AddDays(-1),
                    LastModifiedDate = now,
                    IsValid = true
                },
                new FtsDestinationLocations
                {
                    DestinationId = Guid.NewGuid(),
                    FtsSourceLocationId = sourceLocationId,
                    DestinationAddress = "newer",
                    DestProtocolCD = "FTP",
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now,
                    IsValid = true
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = sourceLocationId.ToString() }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Equal("newer", results[0].DestinationAddress![0].DestinationAddress);
            Assert.Equal("older", results[0].DestinationAddress![1].DestinationAddress);
        }
    }

    [Fact]
    public async Task F01_020_uses_the_first_matching_inbound_target_record_when_resolving_import_job_name()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        var fteMetadataId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigId = 1,
                ConfigName = "c",
                SourceProtocolCD = "FTP",
                SourceAddress = "addr",
                OwnerModule = Guid.NewGuid(),
                IsValid = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsInboundTarget.AddRange(
                new FtsInboundTarget
                {
                    FtsInboundTargetId = Guid.NewGuid(),
                    FTEMetadataId = fteMetadataId,
                    ImportJobName = "FIRST",
                    ContractName = "c",
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsInboundTarget
                {
                    FtsInboundTargetId = Guid.NewGuid(),
                    FTEMetadataId = fteMetadataId,
                    ImportJobName = "SECOND",
                    ContractName = "c",
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                });

            dbContext.FtsDestinationLocations.Add(new FtsDestinationLocations
            {
                DestinationId = Guid.NewGuid(),
                FtsSourceLocationId = sourceLocationId,
                DestinationAddress = "a",
                DestProtocolCD = "FTP",
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now,
                FTEMetadataId = fteMetadataId,
                IsValid = true
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = sourceLocationId.ToString() }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Equal("FIRST", results[0].DestinationAddress![0].ImportJobName);
        }
    }

    [Fact]
    public async Task F01_101_throws_when_source_location_id_is_not_a_guid()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = "not-a-guid" }
            };

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
    public async Task F01_104_does_not_filter_by_requested_is_active_and_returns_the_stored_is_active_flag()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigId = 1,
                ConfigName = "c",
                SourceProtocolCD = "FTP",
                SourceAddress = "addr",
                OwnerModule = Guid.NewGuid(),
                IsValid = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsDestinationLocations.Add(new FtsDestinationLocations
            {
                DestinationId = Guid.NewGuid(),
                FtsSourceLocationId = sourceLocationId,
                DestinationAddress = "a",
                DestProtocolCD = "FTP",
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now,
                IsValid = true
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms
                {
                    SourceLocationId = sourceLocationId.ToString(),
                    IsActive = false
                }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.True(results[0].IsActive);
        }
    }

    [Fact]
    public async Task F01_103_sets_ftp_server_name_to_null_when_the_source_location_has_no_ftp_server_id()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var sourceLocationId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigId = 1,
                ConfigName = "c",
                SourceProtocolCD = "FTP",
                SourceAddress = "addr",
                OwnerModule = Guid.NewGuid(),
                IsValid = true,
                FtpServerId = null,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsDestinationLocations.Add(new FtsDestinationLocations
            {
                DestinationId = Guid.NewGuid(),
                FtsSourceLocationId = sourceLocationId,
                DestinationAddress = "a",
                DestProtocolCD = "FTP",
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now,
                IsValid = true
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = sourceLocationId.ToString() }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Null(results[0].FtpServerName);
        }
    }

    [Fact]
    public async Task F01_102_loads_destination_and_inbound_target_rows_and_maps_only_destinations_for_the_requested_source_location()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;
        var sourceLocationId = Guid.NewGuid();
        var otherSourceLocationId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.AddRange(
                new FtsSourceLocation
                {
                    FtsSourceLocationId = sourceLocationId,
                    ConfigId = 1,
                    ConfigName = "c",
                    SourceProtocolCD = "FTP",
                    SourceAddress = "addr",
                    OwnerModule = Guid.NewGuid(),
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsSourceLocation
                {
                    FtsSourceLocationId = otherSourceLocationId,
                    ConfigId = 2,
                    ConfigName = "c2",
                    SourceProtocolCD = "FTP",
                    SourceAddress = "addr2",
                    OwnerModule = Guid.NewGuid(),
                    IsValid = true,
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now
                });

            dbContext.FtsDestinationLocations.AddRange(
                new FtsDestinationLocations
                {
                    DestinationId = Guid.NewGuid(),
                    FtsSourceLocationId = sourceLocationId,
                    DestinationAddress = "requested",
                    DestProtocolCD = "FTP",
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now,
                    IsValid = true
                },
                new FtsDestinationLocations
                {
                    DestinationId = Guid.NewGuid(),
                    FtsSourceLocationId = otherSourceLocationId,
                    DestinationAddress = "other",
                    DestProtocolCD = "FTP",
                    OperatorId = "op",
                    CreationDate = now,
                    LastModifiedDate = now,
                    IsValid = true
                });

            dbContext.FtsInboundTarget.Add(new FtsInboundTarget
            {
                FtsInboundTargetId = Guid.NewGuid(),
                FTEMetadataId = Guid.NewGuid(),
                ImportJobName = "JOB",
                ContractName = "c",
                OperatorId = "op",
                IsValid = true,
                CreationDate = now,
                LastModifiedDate = now
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = sourceLocationId.ToString() }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Single(results[0].DestinationAddress!);
            Assert.Equal("requested", results[0].DestinationAddress[0].DestinationAddress);
        }
    }

    [Fact]
    public async Task F01_105_assigns_destination_address_list_onto_the_first_result_only()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;
        var sourceLocationId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigId = 1,
                ConfigName = "c",
                SourceProtocolCD = "FTP",
                SourceAddress = "addr",
                OwnerModule = Guid.NewGuid(),
                IsValid = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsDestinationLocations.Add(new FtsDestinationLocations
            {
                DestinationId = Guid.NewGuid(),
                FtsSourceLocationId = sourceLocationId,
                DestinationAddress = "a",
                DestProtocolCD = "FTP",
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now,
                IsValid = true
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = sourceLocationId.ToString() }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.NotNull(results[0].DestinationAddress);
        }
    }

    [Fact]
    public async Task F01_106_maps_destination_records_and_sets_import_job_name_from_inbound_targets()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;
        var sourceLocationId = Guid.NewGuid();
        var fteMetadataId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsSourceLocations.Add(new FtsSourceLocation
            {
                FtsSourceLocationId = sourceLocationId,
                ConfigId = 1,
                ConfigName = "c",
                SourceProtocolCD = "FTP",
                SourceAddress = "addr",
                OwnerModule = Guid.NewGuid(),
                IsValid = true,
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsInboundTarget.Add(new FtsInboundTarget
            {
                FtsInboundTargetId = Guid.NewGuid(),
                FTEMetadataId = fteMetadataId,
                ImportJobName = "JOB-106",
                ContractName = "c",
                OperatorId = "op",
                IsValid = true,
                CreationDate = now,
                LastModifiedDate = now
            });

            dbContext.FtsDestinationLocations.Add(new FtsDestinationLocations
            {
                DestinationId = Guid.NewGuid(),
                FtsSourceLocationId = sourceLocationId,
                DestinationAddress = "dest",
                DestProtocolCD = "FTP",
                OperatorId = "op",
                CreationDate = now,
                LastModifiedDate = now,
                FTEMetadataId = fteMetadataId,
                IsValid = true
            });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new GetSourceLocationQueryExecutor();
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms { SourceLocationId = sourceLocationId.ToString() }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal("JOB-106", results[0].DestinationAddress![0].ImportJobName);
        }
    }
}

