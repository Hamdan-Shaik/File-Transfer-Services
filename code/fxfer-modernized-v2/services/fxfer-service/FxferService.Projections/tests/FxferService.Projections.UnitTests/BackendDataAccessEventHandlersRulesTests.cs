using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FxferService.Projections.Infrastructure.Messaging.Contracts.Events;
using FxferService.Projections.Infrastructure.Persistence;
using FxferService.Projections.Infrastructure.Persistence.Entities;
using FxferService.Projections.Projections.Writers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Projections.UnitTests;

public sealed class BackendDataAccessEventHandlersRulesTests
{
    [Fact]
    public async Task F02_037_handle_source_location_added_generates_new_guid_sets_is_valid_from_event_and_stamps_utc_timestamps()
    {
        var utcNow = new DateTimeOffset(2026, 11, 7, 1, 2, 3, TimeSpan.Zero);
        await using var db = CreateDb();

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        var evt = CreateSourceLocationAddedEvent(
            ftsSourceLocationsId: Guid.NewGuid().ToString(),
            isValid: true,
            destinationAddress: new List<FtsDestinationLocationAdded>());

        await writer.HandleAsync(evt, CancellationToken.None);

        var row = await db.FtsSourceLocations.SingleAsync();
        Assert.NotEqual(Guid.Parse(evt.FtsSourceLocationsId), row.FtsSourceLocationId);
        Assert.Equal(evt.IsValid, row.IsValid);
        Assert.Equal(utcNow, row.CreatedTs);
        Assert.Equal(utcNow, row.LastModifiedTs);
    }

    [Fact]
    public async Task F02_057_handle_source_location_updated_updates_by_id_and_stamps_last_modified_utc_now()
    {
        var originalTs = new DateTimeOffset(2026, 11, 7, 1, 2, 3, TimeSpan.Zero);
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);

        await using var db = CreateDb();
        var sourceId = Guid.NewGuid();

        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceId, originalTs));
        await db.SaveChangesAsync();

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        var evt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceId.ToString(),
            ftpServerId: "",
            ownerModule: Guid.NewGuid().ToString(),
            isValid: true,
            destinationAddress: new List<FtsDestinationLocationAdded>(),
            operatorId: "newop",
            configName: "updated");

        await writer.HandleAsync(evt, CancellationToken.None);

        var row = await db.FtsSourceLocations.SingleAsync(x => x.FtsSourceLocationId == sourceId);
        Assert.Equal("updated", row.ConfigName);
        Assert.Equal(originalTs, row.CreatedTs);
        Assert.Equal(utcNow, row.LastModifiedTs);
    }

    [Fact]
    public async Task F02_058_handle_source_location_updated_stores_empty_ftp_server_id_as_null_otherwise_parsed_guid()
    {
        var ts1 = new DateTimeOffset(2026, 11, 7, 1, 2, 3, TimeSpan.Zero);
        var ts2 = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);

        await using var db = CreateDb();
        var sourceId = Guid.NewGuid();
        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceId, ts1));
        await db.SaveChangesAsync();

        var writer1 = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(ts1));
        var emptyEvt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceId.ToString(),
            ftpServerId: "",
            ownerModule: Guid.NewGuid().ToString(),
            isValid: true,
            destinationAddress: new List<FtsDestinationLocationAdded>());

        await writer1.HandleAsync(emptyEvt, CancellationToken.None);
        Assert.Null((await db.FtsSourceLocations.SingleAsync(x => x.FtsSourceLocationId == sourceId)).FtpServerId);

        var ftpServerId = Guid.NewGuid();
        var writer2 = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(ts2));
        var guidEvt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceId.ToString(),
            ftpServerId: ftpServerId.ToString(),
            ownerModule: Guid.NewGuid().ToString(),
            isValid: true,
            destinationAddress: new List<FtsDestinationLocationAdded>());

        await writer2.HandleAsync(guidEvt, CancellationToken.None);
        Assert.Equal(ftpServerId, (await db.FtsSourceLocations.SingleAsync(x => x.FtsSourceLocationId == sourceId)).FtpServerId);
    }

    [Fact]
    public async Task F02_059_handle_source_location_updated_parses_and_stores_owner_module_guid()
    {
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);
        await using var db = CreateDb();
        var sourceId = Guid.NewGuid();

        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceId, utcNow));
        await db.SaveChangesAsync();

        var moduleId = Guid.NewGuid();
        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        var evt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceId.ToString(),
            ftpServerId: "",
            ownerModule: moduleId.ToString(),
            isValid: true,
            destinationAddress: new List<FtsDestinationLocationAdded>());

        await writer.HandleAsync(evt, CancellationToken.None);

        var row = await db.FtsSourceLocations.SingleAsync(x => x.FtsSourceLocationId == sourceId);
        Assert.Equal(moduleId, row.ModuleId);
    }

    [Fact]
    public async Task F02_060_handle_source_location_updated_upserts_destinations_by_destination_id_inserts_generate_new_destination_id_and_stamp_utc()
    {
        var originalTs = new DateTimeOffset(2026, 11, 7, 1, 2, 3, TimeSpan.Zero);
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);

        await using var db = CreateDb();
        var sourceId = Guid.NewGuid();
        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceId, originalTs));

        var existingDestinationId = Guid.NewGuid();
        db.FtsDestinations.Add(CreateDestinationRow(
            destinationId: existingDestinationId,
            sourceId: sourceId,
            destinationAddress: "/old",
            operatorId: "oldop",
            creationTs: originalTs,
            lastModifiedTs: originalTs));

        await db.SaveChangesAsync();

        var incomingNewDestinationId = Guid.NewGuid();
        var ftpServerId = Guid.NewGuid();

        var evt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceId.ToString(),
            ftpServerId: "",
            ownerModule: Guid.NewGuid().ToString(),
            isValid: true,
            destinationAddress: new List<FtsDestinationLocationAdded>
            {
                CreateDestinationAddedEvent(
                    ftsSourceLocationsId: sourceId.ToString(),
                    destinationId: existingDestinationId,
                    destinationAddress: "/updated",
                    ftpServerId: ftpServerId,
                    isValid: true,
                    operatorId: "destop"),
                CreateDestinationAddedEvent(
                    ftsSourceLocationsId: sourceId.ToString(),
                    destinationId: incomingNewDestinationId,
                    destinationAddress: "/new",
                    ftpServerId: ftpServerId,
                    isValid: true,
                    operatorId: "destop"),
            });

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        await writer.HandleAsync(evt, CancellationToken.None);

        var destinations = await db.FtsDestinations.OrderBy(x => x.DestinationAddress).ToListAsync();
        Assert.Equal(2, destinations.Count);

        var updatedExisting = destinations.Single(x => x.DestinationId == existingDestinationId);
        Assert.Equal("/updated", updatedExisting.DestinationAddress);
        Assert.Equal(originalTs, updatedExisting.CreationTs);
        Assert.Equal(utcNow, updatedExisting.LastModifiedTs);

        var inserted = destinations.Single(x => x.DestinationAddress == "/new");
        Assert.NotEqual(incomingNewDestinationId, inserted.DestinationId);
        Assert.Equal(sourceId, inserted.FtsSourceLocationId);
        Assert.Equal(utcNow, inserted.CreationTs);
        Assert.Equal(utcNow, inserted.LastModifiedTs);
    }

    [Fact]
    public async Task F02_061_handle_source_location_updated_always_attempts_destination_upsert_from_destination_address()
    {
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);
        await using var db = CreateDb();
        var sourceId = Guid.NewGuid();

        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceId, utcNow));
        await db.SaveChangesAsync();

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        var evt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceId.ToString(),
            ftpServerId: "",
            ownerModule: Guid.NewGuid().ToString(),
            isValid: true,
            destinationAddress: null!);

        await Assert.ThrowsAsync<NullReferenceException>(() => writer.HandleAsync(evt, CancellationToken.None));
    }

    [Fact]
    public async Task F02_062_handle_source_location_updated_when_is_valid_false_forces_all_destinations_invalid_and_overwrites_operator_id()
    {
        var originalTs = new DateTimeOffset(2026, 11, 7, 1, 2, 3, TimeSpan.Zero);
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);

        await using var db = CreateDb();
        var sourceId = Guid.NewGuid();
        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceId, originalTs));

        db.FtsDestinations.Add(CreateDestinationRow(Guid.NewGuid(), sourceId, "/a", operatorId: "old", creationTs: originalTs, lastModifiedTs: originalTs));
        db.FtsDestinations.Add(CreateDestinationRow(Guid.NewGuid(), sourceId, "/b", operatorId: "old", creationTs: originalTs, lastModifiedTs: originalTs));
        await db.SaveChangesAsync();

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        var evt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceId.ToString(),
            ftpServerId: "",
            ownerModule: Guid.NewGuid().ToString(),
            isValid: false,
            destinationAddress: new List<FtsDestinationLocationAdded>(),
            operatorId: "newop");

        await writer.HandleAsync(evt, CancellationToken.None);

        var destinations = await db.FtsDestinations.Where(x => x.FtsSourceLocationId == sourceId).ToListAsync();
        Assert.All(destinations, d => Assert.False(d.IsValid));
        Assert.All(destinations, d => Assert.Equal("newop", d.OperatorId));
    }

    [Fact]
    public async Task F02_063_handle_source_location_invalidated_sets_source_is_valid_false_and_stores_operator_id()
    {
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);
        await using var db = CreateDb();
        var sourceId = Guid.NewGuid();

        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceId, utcNow, isValid: true, operatorId: "old"));
        await db.SaveChangesAsync();

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        var evt = new FtsSourceLocationInvalidated
        {
            FtsSourceLocationsId = sourceId.ToString(),
            OperatorId = "invop",
            ConfigId = 1,
            DestinationAddress = null,
            UserID = null,
        };

        await writer.HandleAsync(evt, CancellationToken.None);

        var row = await db.FtsSourceLocations.SingleAsync(x => x.FtsSourceLocationId == sourceId);
        Assert.False(row.IsValid);
        Assert.Equal("invop", row.OperatorId);
    }

    [Fact]
    public async Task F02_064_handle_source_location_invalidated_optionally_upserts_destinations_then_forces_all_destinations_invalid_and_overwrites_operator_id()
    {
        var originalTs = new DateTimeOffset(2026, 11, 7, 1, 2, 3, TimeSpan.Zero);
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);

        await using var db = CreateDb();
        var sourceId = Guid.NewGuid();
        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceId, originalTs));

        var existingDestinationId = Guid.NewGuid();
        db.FtsDestinations.Add(CreateDestinationRow(existingDestinationId, sourceId, "/old", operatorId: "old", creationTs: originalTs, lastModifiedTs: originalTs));
        await db.SaveChangesAsync();

        var ftpServerId = Guid.NewGuid();
        var incomingNewDestinationId = Guid.NewGuid();

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        var evt = new FtsSourceLocationInvalidated
        {
            FtsSourceLocationsId = sourceId.ToString(),
            OperatorId = "invop",
            ConfigId = 1,
            DestinationAddress = new List<FtsDestinationLocationAdded>
            {
                CreateDestinationAddedEvent(sourceId.ToString(), existingDestinationId, "/updated", ftpServerId, isValid: true, operatorId: "destop"),
                CreateDestinationAddedEvent(sourceId.ToString(), incomingNewDestinationId, "/new", ftpServerId, isValid: true, operatorId: "destop"),
            },
            UserID = null,
        };

        await writer.HandleAsync(evt, CancellationToken.None);

        var destinations = await db.FtsDestinations.Where(x => x.FtsSourceLocationId == sourceId).OrderBy(x => x.DestinationAddress).ToListAsync();
        Assert.Equal(2, destinations.Count);

        var updatedExisting = destinations.Single(x => x.DestinationId == existingDestinationId);
        Assert.Equal("/updated", updatedExisting.DestinationAddress);

        var inserted = destinations.Single(x => x.DestinationAddress == "/new");
        Assert.NotEqual(incomingNewDestinationId, inserted.DestinationId);

        Assert.All(destinations, d => Assert.False(d.IsValid));
        Assert.All(destinations, d => Assert.Equal("invop", d.OperatorId));
    }

    [Fact]
    public async Task FtsSourceLocationProjectionWriter_Handle_FtsSourceLocationAdded_throws_when_destination_address_is_null()
    {
        var utcNow = new DateTimeOffset(2026, 11, 7, 1, 2, 3, TimeSpan.Zero);
        await using var db = CreateDb();

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        var evt = CreateSourceLocationAddedEvent(
            ftsSourceLocationsId: Guid.NewGuid().ToString(),
            isValid: true,
            destinationAddress: null!);

        await Assert.ThrowsAsync<NullReferenceException>(() => writer.HandleAsync(evt, CancellationToken.None));
    }

    [Fact]
    public async Task FtsSourceLocationProjectionWriter_Handle_FtsSourceLocationUpdated_throws_when_ftp_server_id_is_non_empty_and_not_a_guid()
    {
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);
        await using var db = CreateDb();
        var sourceId = Guid.NewGuid();

        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceId, utcNow));
        await db.SaveChangesAsync();

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        var evt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceId.ToString(),
            ftpServerId: "not-a-guid",
            ownerModule: Guid.NewGuid().ToString(),
            isValid: true,
            destinationAddress: new List<FtsDestinationLocationAdded>());

        await Assert.ThrowsAsync<FormatException>(() => writer.HandleAsync(evt, CancellationToken.None));
    }

    [Fact]
    public async Task FtsSourceLocationProjectionWriter_Handle_FtsSourceLocationUpdated_throws_when_owner_module_is_not_a_guid()
    {
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);
        await using var db = CreateDb();
        var sourceId = Guid.NewGuid();

        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceId, utcNow));
        await db.SaveChangesAsync();

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        var evt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceId.ToString(),
            ftpServerId: "",
            ownerModule: "not-a-guid",
            isValid: true,
            destinationAddress: new List<FtsDestinationLocationAdded>());

        await Assert.ThrowsAsync<FormatException>(() => writer.HandleAsync(evt, CancellationToken.None));
    }

    [Fact]
    public async Task FtsSourceLocationProjectionWriter_AddDestinationAddress_destination_existence_check_is_global_not_scoped_to_source_location()
    {
        var originalTs = new DateTimeOffset(2026, 11, 7, 1, 2, 3, TimeSpan.Zero);
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);

        await using var db = CreateDb();
        var sourceA = Guid.NewGuid();
        var sourceB = Guid.NewGuid();

        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceA, originalTs));
        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceB, originalTs));

        var sharedDestinationId = Guid.NewGuid();
        db.FtsDestinations.Add(CreateDestinationRow(sharedDestinationId, sourceB, "/old", operatorId: "old", creationTs: originalTs, lastModifiedTs: originalTs));
        await db.SaveChangesAsync();

        var ftpServerId = Guid.NewGuid();
        var evt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceA.ToString(),
            ftpServerId: "",
            ownerModule: Guid.NewGuid().ToString(),
            isValid: true,
            destinationAddress: new List<FtsDestinationLocationAdded>
            {
                CreateDestinationAddedEvent(sourceA.ToString(), sharedDestinationId, "/updated", ftpServerId, isValid: true, operatorId: "destop"),
            });

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        await writer.HandleAsync(evt, CancellationToken.None);

        var rows = await db.FtsDestinations.ToListAsync();
        Assert.Single(rows);
        Assert.Equal(sharedDestinationId, rows[0].DestinationId);
        Assert.Equal("/updated", rows[0].DestinationAddress);
        Assert.Equal(sourceB, rows[0].FtsSourceLocationId);
    }

    [Fact]
    public async Task FtsSourceLocationProjectionWriter_UpdateDestination_does_not_set_fts_source_location_id()
    {
        var originalTs = new DateTimeOffset(2026, 11, 7, 1, 2, 3, TimeSpan.Zero);
        var utcNow = new DateTimeOffset(2026, 11, 7, 4, 5, 6, TimeSpan.Zero);

        await using var db = CreateDb();
        var sourceA = Guid.NewGuid();
        var sourceB = Guid.NewGuid();

        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceA, originalTs));
        db.FtsSourceLocations.Add(CreateSourceLocationRow(sourceB, originalTs));

        var destinationId = Guid.NewGuid();
        db.FtsDestinations.Add(CreateDestinationRow(destinationId, sourceB, "/old", operatorId: "old", creationTs: originalTs, lastModifiedTs: originalTs));
        await db.SaveChangesAsync();

        var ftpServerId = Guid.NewGuid();
        var evt = CreateSourceLocationUpdatedEvent(
            ftsSourceLocationsId: sourceA.ToString(),
            ftpServerId: "",
            ownerModule: Guid.NewGuid().ToString(),
            isValid: true,
            destinationAddress: new List<FtsDestinationLocationAdded>
            {
                CreateDestinationAddedEvent(sourceA.ToString(), destinationId, "/updated", ftpServerId, isValid: true, operatorId: "destop"),
            });

        var writer = new FtsSourceLocationProjectionWriter(db, new FixedTimeProvider(utcNow));
        await writer.HandleAsync(evt, CancellationToken.None);

        var row = await db.FtsDestinations.SingleAsync(x => x.DestinationId == destinationId);
        Assert.Equal(sourceB, row.FtsSourceLocationId);
    }

    private static FxferProjectionsDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<FxferProjectionsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FxferProjectionsDbContext(options);
    }

    private static FtsSourceLocationProjection CreateSourceLocationRow(
        Guid sourceLocationId,
        DateTimeOffset ts,
        bool isValid = true,
        string operatorId = "op")
    {
        return new FtsSourceLocationProjection
        {
            FtsSourceLocationId = sourceLocationId,
            ConfigId = 1,
            ConfigName = "cfg",
            SourceProtocolCd = "SFTP",
            SourceAddress = "/in",
            SourceFileMask = "*.txt",
            FtpServerId = null,
            SendNotificationsInd = true,
            OwnerName = "own",
            ModuleId = Guid.NewGuid(),
            OwnerPhone = "123",
            OwnerEmail = "a@b.com",
            ConfigDescription = "desc",
            RunOnMondayInd = false,
            RunOnTuesdayInd = false,
            RunOnWedInd = false,
            RunOnThursInd = false,
            RunOnFriInd = false,
            RunOnSatInd = false,
            RunOnSunInd = false,
            RunOnFrstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = 0,
            StartTime24hhmm = "0000",
            EndTime24hhmm = "2359",
            FileDirection = "IN",
            TradingPartner = "tp",
            IsValid = isValid,
            IsSuspended = false,
            IsSuspendedChangedTs = null,
            OperatorId = operatorId,
            LastModifiedTs = ts,
            CreatedTs = ts,
        };
    }

    private static FtsDestinationProjection CreateDestinationRow(
        Guid destinationId,
        Guid sourceId,
        string destinationAddress,
        string operatorId,
        DateTimeOffset creationTs,
        DateTimeOffset lastModifiedTs)
    {
        return new FtsDestinationProjection
        {
            DestinationId = destinationId,
            FtsSourceLocationId = sourceId,
            DestinationAddress = destinationAddress,
            DestProtocolCd = "SFTP",
            OverwriteAtDestInd = false,
            CompressAtDestInd = false,
            RenameAtDestInd = false,
            DestIsArchiveInd = false,
            IsValid = true,
            FtpServerId = Guid.NewGuid(),
            RenamePattern = null,
            ContactName = null,
            ContactPhone = null,
            ContactEmail = null,
            OperatorId = operatorId,
            CreationTs = creationTs,
            LastModifiedTs = lastModifiedTs,
            CdeEncryption = null,
            EncryptionPublicKey = null,
            EncryptionPrivateKey = null,
            PrivateKeyPassword = null,
            ExternalFileDefnId = null,
            DestinationType = null,
            ContractName = null,
        };
    }

    private static FtsSourceLocationAdded CreateSourceLocationAddedEvent(
        string ftsSourceLocationsId,
        bool isValid,
        List<FtsDestinationLocationAdded>? destinationAddress)
    {
        return new FtsSourceLocationAdded
        {
            FtsSourceLocationsId = ftsSourceLocationsId,
            ConfigId = 1,
            ConfigName = "cfg",
            Description = "desc",
            FtpServerId = "",
            Protocol = "SFTP",
            Address = "/in",
            Mask = "*.txt",
            FileDirection = "IN",
            SendNotifications = true,
            IsValid = isValid,
            IsSuspended = false,
            IsSuspendedChangedDate = null,
            OwnerName = "own",
            OwnerModule = Guid.NewGuid().ToString(),
            OwnerPhone = "123",
            OwnerEmail = "a@b.com",
            RunOnMonday = false,
            RunOnTuesday = false,
            RunOnWed = false,
            RunOnThrus = false,
            RunOnFri = false,
            RunOnSat = false,
            RunOnSun = false,
            RunOnFrstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = 0,
            StartTime = "0000",
            EndTime = "2359",
            TradingPartner = "tp",
            OperatorId = "op",
            DestinationAddress = destinationAddress!,
        };
    }

    private static FtsSourceLocationUpdated CreateSourceLocationUpdatedEvent(
        string ftsSourceLocationsId,
        string ftpServerId,
        string ownerModule,
        bool isValid,
        List<FtsDestinationLocationAdded> destinationAddress,
        string operatorId = "op",
        string configName = "cfg")
    {
        return new FtsSourceLocationUpdated
        {
            FtsSourceLocationsId = ftsSourceLocationsId,
            UserID = null,
            ConfigId = 1,
            ConfigName = configName,
            Description = "desc",
            FtpServerId = ftpServerId,
            Protocol = "SFTP",
            Address = "/in",
            Mask = "*.txt",
            FileDirection = "IN",
            SendNotifications = true,
            IsValid = isValid,
            IsSuspended = false,
            IsSuspendedChangedDate = null,
            OwnerName = "own",
            OwnerModule = ownerModule,
            OwnerPhone = "123",
            OwnerEmail = "a@b.com",
            RunOnMonday = false,
            RunOnTuesday = false,
            RunOnWed = false,
            RunOnThrus = false,
            RunOnFri = false,
            RunOnSat = false,
            RunOnSun = false,
            RunOnFrstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = 0,
            StartTime = "0000",
            EndTime = "2359",
            TradingPartner = "tp",
            OperatorId = operatorId,
            DestinationAddress = destinationAddress,
        };
    }

    private static FtsDestinationLocationAdded CreateDestinationAddedEvent(
        string ftsSourceLocationsId,
        Guid destinationId,
        string destinationAddress,
        Guid ftpServerId,
        bool isValid,
        string operatorId)
    {
        return new FtsDestinationLocationAdded
        {
            FtsSourceLocationsId = ftsSourceLocationsId,
            UserID = null,
            DestinationId = destinationId,
            SourceLocationId = Guid.NewGuid(),
            DestinationAddress = destinationAddress,
            DestinationProtocol = "SFTP",
            OverWriteAtDestination = false,
            CompressAtDestination = false,
            RenameAtDestination = false,
            DestinationArchive = false,
            IsValid = isValid,
            FtpServerId = ftpServerId,
            OperatorId = operatorId,
            FTEMetadataId = null,
            DestinationType = null,
            ContractName = null,
            RenamePattern = null,
            ContactName = null,
            ContactPhone = null,
            ContactEmail = null,
        };
    }

    private sealed class FixedTimeProvider : TimeProvider
    {
        private readonly DateTimeOffset _utcNow;

        public FixedTimeProvider(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }

        public override DateTimeOffset GetUtcNow() => _utcNow;
    }
}