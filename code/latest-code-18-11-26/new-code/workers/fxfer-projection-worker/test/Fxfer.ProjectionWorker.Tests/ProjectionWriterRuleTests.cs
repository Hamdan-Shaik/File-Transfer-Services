using System.Text.Json;
using Fxfer.ProjectionWorker.Infrastructure.DataAccess;
using Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;
using Fxfer.ProjectionWorker.Infrastructure.Projections;
using Fxfer.ProjectionWorker.Worker.Eventing;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.ProjectionWorker.Tests;

public sealed class ProjectionWriterRuleTests
{
    private static FxferProjectionDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<FxferProjectionDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FxferProjectionDbContext(options);
    }

    private static FxferEventEnvelope CreateConfigurationChangedEnvelope(string operatorId)
    {
        var payload = new ConfigurationChangedV1
        {
            AggregateType = "inbound_target",
            AggregateId = Guid.NewGuid(),
            ChangeKind = "invalidated",
            OperatorId = operatorId,
            ChangedAt = DateTimeOffset.UtcNow,
        };

        using var doc = JsonDocument.Parse(JsonSerializer.Serialize(payload));

        return new FxferEventEnvelope
        {
            TenantId = "t1",
            EventName = ConsumedEvents.ConfigurationChangedV1Name,
            EventVersion = ConsumedEvents.V1,
            EventId = Guid.NewGuid(),
            OccurredAt = DateTimeOffset.UtcNow,
            CorrelationId = Guid.NewGuid().ToString(),
            TraceId = Guid.NewGuid().ToString(),
            Producer = "tests",
            Payload = doc.RootElement.Clone(),
        };
    }

    private static DestinationProjectionPayload CreateDestinationPayload(Guid destinationId, bool isValid)
    {
        return new DestinationProjectionPayload
        {
            DestinationId = destinationId,
            DestinationAddress = string.Empty,
            DestinationProtocol = string.Empty,
            OverwriteAtDestination = true,
            CompressAtDestination = false,
            RenameAtDestination = false,
            DestinationArchive = false,
            IsValid = isValid,
            FtpServerId = null,
            OperatorId = string.Empty,
            RenamePattern = null,
            ContactName = null,
            ContactPhone = null,
            ContactEmail = null,
            FteMetadataId = null,
            DestinationType = null,
            ContractName = null,
        };
    }

    private static SourceLocationProjectionPayload CreateSourceLocationPayload(
        Guid sourceLocationId,
        bool isValid,
        string? ftpServerId,
        List<DestinationProjectionPayload> destinations)
    {
        return new SourceLocationProjectionPayload
        {
            SourceLocationId = sourceLocationId.ToString(),
            ConfigId = 1,
            ConfigName = string.Empty,
            Description = null,
            FtpServerId = ftpServerId,
            Protocol = string.Empty,
            Address = string.Empty,
            Mask = string.Empty,
            FileDirection = null,
            SendNotifications = true,
            IsValid = isValid,
            IsSuspended = false,
            IsSuspendedChangedDate = null,
            OwnerName = string.Empty,
            OwnerModule = Guid.NewGuid().ToString(),
            OwnerPhone = null,
            OwnerEmail = null,
            RunOnMonday = false,
            RunOnTuesday = false,
            RunOnWed = false,
            RunOnThrus = false,
            RunOnFri = false,
            RunOnSat = false,
            RunOnSun = false,
            RunOnFrstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = null,
            StartTime = string.Empty,
            EndTime = string.Empty,
            TradingPartner = null,
            OperatorId = string.Empty,
            Destinations = destinations,
        };
    }

    [Fact]
    public async Task F01_048_generates_new_source_location_id_and_sets_created_and_modified_ts_to_utc_now()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var inputSourceLocationId = Guid.NewGuid();
        var payload = CreateSourceLocationPayload(
            inputSourceLocationId,
            isValid: true,
            ftpServerId: Guid.NewGuid().ToString(),
            destinations: new() { CreateDestinationPayload(Guid.NewGuid(), isValid: true) });

        var before = DateTimeOffset.UtcNow;
        await writer.HandleSourceLocationCreatedAsync(CreateConfigurationChangedEnvelope(string.Empty), payload, CancellationToken.None);
        var after = DateTimeOffset.UtcNow;

        var row = await db.SourceLocations.SingleAsync();
        Assert.NotEqual(inputSourceLocationId, row.FtsSourceLocationId);
        Assert.Equal(row.CreatedTs, row.LastModifiedTs);
        Assert.InRange(row.CreatedTs, before, after);
    }

    [Fact]
    public async Task F01_049_inserts_source_location_as_valid_even_when_event_is_invalid()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var payload = CreateSourceLocationPayload(
            Guid.NewGuid(),
            isValid: false,
            ftpServerId: null,
            destinations: new() { CreateDestinationPayload(Guid.NewGuid(), isValid: true) });

        await writer.HandleSourceLocationCreatedAsync(CreateConfigurationChangedEnvelope(string.Empty), payload, CancellationToken.None);

        var row = await db.SourceLocations.SingleAsync();
        Assert.True(row.IsValid);
    }

    [Fact]
    public async Task F01_050_stores_empty_ftp_server_id_as_null_else_parses_guid()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var withEmpty = CreateSourceLocationPayload(
            Guid.NewGuid(),
            isValid: true,
            ftpServerId: string.Empty,
            destinations: new() { CreateDestinationPayload(Guid.NewGuid(), isValid: true) });

        var ftpServerId = Guid.NewGuid();
        var withGuid = CreateSourceLocationPayload(
            Guid.NewGuid(),
            isValid: true,
            ftpServerId: ftpServerId.ToString(),
            destinations: new() { CreateDestinationPayload(Guid.NewGuid(), isValid: true) });

        await writer.HandleSourceLocationCreatedAsync(CreateConfigurationChangedEnvelope(string.Empty), withEmpty, CancellationToken.None);
        await writer.HandleSourceLocationCreatedAsync(CreateConfigurationChangedEnvelope(string.Empty), withGuid, CancellationToken.None);

        var emptyRow = await db.SourceLocations.Where(x => x.FtpServerId == null).SingleAsync();
        Assert.Null(emptyRow.FtpServerId);

        var guidRow = await db.SourceLocations.Where(x => x.FtpServerId == ftpServerId).SingleAsync();
        Assert.Equal(ftpServerId, guidRow.FtpServerId);
    }

    [Fact]
    public async Task F01_051_forces_destination_invalid_when_source_location_event_invalid()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var payload = CreateSourceLocationPayload(
            Guid.NewGuid(),
            isValid: false,
            ftpServerId: null,
            destinations: new() { CreateDestinationPayload(Guid.NewGuid(), isValid: true) });

        await writer.HandleSourceLocationCreatedAsync(CreateConfigurationChangedEnvelope(string.Empty), payload, CancellationToken.None);

        var destination = await db.Destinations.SingleAsync();
        Assert.False(destination.IsValid);
    }

    [Fact]
    public async Task F01_052_inserts_destinations_with_new_id_linked_to_new_source_location_id()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var inputDestinationId = Guid.NewGuid();
        var payload = CreateSourceLocationPayload(
            Guid.NewGuid(),
            isValid: true,
            ftpServerId: null,
            destinations: new() { CreateDestinationPayload(inputDestinationId, isValid: true) });

        await writer.HandleSourceLocationCreatedAsync(CreateConfigurationChangedEnvelope(string.Empty), payload, CancellationToken.None);

        var source = await db.SourceLocations.SingleAsync();
        var destination = await db.Destinations.SingleAsync();

        Assert.NotEqual(inputDestinationId, destination.DestinationId);
        Assert.Equal(source.FtsSourceLocationId, destination.FtsSourceLocationId);
    }

    [Fact]
    public async Task F01_053_inserts_destination_fields_and_sets_timestamps_to_utc_now()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var destinationAddress = new string(new[] { 'a', 'd', 'd', 'r' });
        var destinationProtocol = new string(new[] { 'S', 'F', 'T', 'P' });
        var renamePattern = new string(new[] { 'r', 'p' });
        var contactName = new string(new[] { 'c', 'n' });
        var contactPhone = new string(new[] { 'c', 'p' });
        var contactEmail = new string(new[] { 'c', 'e' });
        var destinationType = new string(new[] { 't', 'y', 'p', 'e' });
        var contractName = new string(new[] { 'c', 'o', 'n', 't' });
        var operatorId = new string(new[] { 'o', 'p', '2' });
        var ftpServerId = Guid.NewGuid();
        var externalFileDefnId = Guid.NewGuid();

        var dest = CreateDestinationPayload(Guid.NewGuid(), isValid: true) with
        {
            DestinationAddress = destinationAddress,
            DestinationProtocol = destinationProtocol,
            OverwriteAtDestination = false,
            CompressAtDestination = true,
            RenameAtDestination = true,
            DestinationArchive = true,
            FtpServerId = ftpServerId,
            OperatorId = operatorId,
            RenamePattern = renamePattern,
            ContactName = contactName,
            ContactPhone = contactPhone,
            ContactEmail = contactEmail,
            FteMetadataId = externalFileDefnId,
            DestinationType = destinationType,
            ContractName = contractName,
        };

        var payload = CreateSourceLocationPayload(
            Guid.NewGuid(),
            isValid: true,
            ftpServerId: null,
            destinations: new() { dest });

        var before = DateTimeOffset.UtcNow;
        await writer.HandleSourceLocationCreatedAsync(CreateConfigurationChangedEnvelope(operatorId), payload, CancellationToken.None);
        var after = DateTimeOffset.UtcNow;

        var row = await db.Destinations.SingleAsync();
        Assert.Equal(destinationAddress, row.DestinationAddress);
        Assert.Equal(destinationProtocol, row.DestProtocolCd);
        Assert.False(row.OverwriteAtDestInd);
        Assert.True(row.CompressAtDestInd);
        Assert.True(row.RenameAtDestInd);
        Assert.True(row.DestIsArchiveInd);
        Assert.Equal(ftpServerId, row.FtpServerId);
        Assert.Equal(operatorId, row.OperatorId);
        Assert.Equal(renamePattern, row.RenamePattern);
        Assert.Equal(contactName, row.ContactName);
        Assert.Equal(contactPhone, row.ContactPhone);
        Assert.Equal(contactEmail, row.ContactEmail);
        Assert.Equal(externalFileDefnId, row.ExternalFileDefnId);
        Assert.Equal(destinationType, row.DestinationType);
        Assert.Equal(contractName, row.ContractName);
        Assert.Equal(row.CreatedTs, row.LastModifiedTs);
        Assert.InRange(row.CreatedTs, before, after);
    }

    [Fact]
    public async Task F01_054_persists_source_location_and_destinations_with_save_changes()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<FxferProjectionDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using (var db = new FxferProjectionDbContext(options))
        {
            var writer = new FtsSourceLocationProjectionWriter(db);
            var payload = CreateSourceLocationPayload(
                Guid.NewGuid(),
                isValid: true,
                ftpServerId: null,
                destinations: new() { CreateDestinationPayload(Guid.NewGuid(), isValid: true) });

            await writer.HandleSourceLocationCreatedAsync(CreateConfigurationChangedEnvelope(string.Empty), payload, CancellationToken.None);
        }

        await using (var db = new FxferProjectionDbContext(options))
        {
            Assert.Single(await db.SourceLocations.ToListAsync());
            Assert.Single(await db.Destinations.ToListAsync());
        }
    }

    [Fact]
    public async Task F01_078_upserts_destination_by_destination_id_update_when_exists_else_insert()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var sourceLocationId = Guid.NewGuid();
        db.SourceLocations.Add(new FtsSourceLocation
        {
            FtsSourceLocationId = sourceLocationId,
            ConfigId = 1,
            ConfigName = string.Empty,
            SourceProtocolCd = string.Empty,
            SourceAddress = string.Empty,
            SourceFileMask = string.Empty,
            FtpServerId = null,
            SendNotificationsInd = false,
            OwnerName = string.Empty,
            OwnerModule = Guid.NewGuid(),
            OwnerPhone = null,
            OwnerEmail = null,
            Description = null,
            RunOnMondayInd = false,
            RunOnTuesdayInd = false,
            RunOnWednesdayInd = false,
            RunOnThursdayInd = false,
            RunOnFridayInd = false,
            RunOnSaturdayInd = false,
            RunOnSundayInd = false,
            RunOnFirstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = null,
            StartTime24hhmm = string.Empty,
            EndTime24hhmm = string.Empty,
            FileDirection = null,
            TradingPartner = null,
            IsValid = true,
            IsSuspended = false,
            IsSuspendedChangedTs = null,
            OperatorId = string.Empty,
            CreatedTs = DateTimeOffset.UtcNow,
            LastModifiedTs = DateTimeOffset.UtcNow,
        });

        var existingDestinationId = Guid.NewGuid();
        db.Destinations.Add(new FtsDestination
        {
            DestinationId = existingDestinationId,
            FtsSourceLocationId = sourceLocationId,
            DestinationAddress = string.Empty,
            DestProtocolCd = string.Empty,
            OverwriteAtDestInd = false,
            CompressAtDestInd = false,
            RenameAtDestInd = false,
            DestIsArchiveInd = false,
            IsValid = true,
            FtpServerId = null,
            RenamePattern = null,
            ContactName = null,
            ContactPhone = null,
            ContactEmail = null,
            OperatorId = string.Empty,
            CreatedTs = DateTimeOffset.UtcNow,
            LastModifiedTs = DateTimeOffset.UtcNow,
            CdeEncryption = null,
            EncryptionPublicKey = null,
            EncryptionPrivateKeySecretRef = null,
            PrivateKeyPasswordSecretRef = null,
            ExternalFileDefnId = null,
            DestinationType = null,
            ContractName = null,
        });

        await db.SaveChangesAsync();

        var updatedAddress = new string(new[] { 'u', 'p', 'd' });
        var newAddress = new string(new[] { 'n', 'e', 'w' });
        var destUpdate = CreateDestinationPayload(existingDestinationId, isValid: true) with { DestinationAddress = updatedAddress };
        var destInsert = CreateDestinationPayload(Guid.NewGuid(), isValid: true) with { DestinationAddress = newAddress };

        var payload = CreateSourceLocationPayload(
            sourceLocationId,
            isValid: true,
            ftpServerId: null,
            destinations: new() { destUpdate, destInsert });

        await writer.HandleSourceLocationUpdatedAsync(CreateConfigurationChangedEnvelope(string.Empty), payload, CancellationToken.None);

        var destinations = await db.Destinations.OrderBy(x => x.DestinationAddress).ToListAsync();
        Assert.Equal(2, destinations.Count);

        var updated = destinations.Single(x => x.DestinationId == existingDestinationId);
        Assert.Equal(updatedAddress, updated.DestinationAddress);

        var inserted = destinations.Single(x => x.DestinationId != existingDestinationId);
        Assert.NotEqual(destInsert.DestinationId, inserted.DestinationId);
        Assert.Equal(sourceLocationId, inserted.FtsSourceLocationId);
        Assert.Equal(newAddress, inserted.DestinationAddress);
    }

    [Fact]
    public async Task F03_012_updates_integration_file_fields_on_update()
    {
        await using var db = CreateDb();
        var writer = new FtsIntegrationFileProjectionWriter(db);

        var id = Guid.NewGuid();
        var ts = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);
        var operatorId = new string(new[] { 'o', 'p', '2' });

        db.IntegrationFiles.Add(new FtsIntegrationFile
        {
            FtsIntegrationFileId = id,
            GroupId = Guid.NewGuid(),
            ConfigId = null,
            FileName = string.Empty,
            EtfOutboundMetadataDefnId = Guid.NewGuid(),
            IsProcessed = false,
            OperatorId = string.Empty,
            IsValid = true,
            CreatedTs = ts,
            LastModifiedTs = ts,
        });
        await db.SaveChangesAsync();

        var payload = new IntegrationFileUpdatedProjectionPayload
        {
            FtsIntegrationFileId = id,
            ConfigId = 123,
            OperatorId = operatorId,
            IsValid = true,
        };

        await writer.HandleIntegrationFileUpdatedAsync(CreateConfigurationChangedEnvelope(operatorId), payload, CancellationToken.None);

        var updated = await db.IntegrationFiles.SingleAsync();
        Assert.Equal(123, updated.ConfigId);
        Assert.True(updated.IsProcessed);
        Assert.True(updated.IsValid);
        Assert.Equal(operatorId, updated.OperatorId);
        Assert.Equal(ts, updated.CreatedTs);
        Assert.True(updated.LastModifiedTs > ts);
    }

    [Fact]
    public async Task F03_013_persists_inactive_flag_on_integration_file_update()
    {
        await using var db = CreateDb();
        var writer = new FtsIntegrationFileProjectionWriter(db);

        var id = Guid.NewGuid();
        var ts = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);
        var operatorId = new string(new[] { 'o', 'p', '2' });

        db.IntegrationFiles.Add(new FtsIntegrationFile
        {
            FtsIntegrationFileId = id,
            GroupId = Guid.NewGuid(),
            ConfigId = null,
            FileName = string.Empty,
            EtfOutboundMetadataDefnId = Guid.NewGuid(),
            IsProcessed = false,
            OperatorId = string.Empty,
            IsValid = true,
            CreatedTs = ts,
            LastModifiedTs = ts,
        });
        await db.SaveChangesAsync();

        var payload = new IntegrationFileUpdatedProjectionPayload
        {
            FtsIntegrationFileId = id,
            ConfigId = null,
            OperatorId = operatorId,
            IsValid = false,
        };

        await writer.HandleIntegrationFileUpdatedAsync(CreateConfigurationChangedEnvelope(operatorId), payload, CancellationToken.None);

        var updated = await db.IntegrationFiles.SingleAsync();
        Assert.False(updated.IsValid);
    }

    [Fact]
    public async Task F02_035_sets_ftp_server_created_and_modified_to_utc_now()
    {
        await using var db = CreateDb();
        var writer = new FtpServerProjectionWriter(db);

        var payload = new FtpServerProjectionPayload
        {
            FtpServerId = Guid.NewGuid(),
            FtpServerName = "s1",
            FtpDescription = "d1",
            FtpServerHost = "127.0.0.1",
            FtpTransferMode = "FTP",
            FtpUser = "u1",
            FtpPasswordSecretRef = "secret://pw",
            FtpPort = 21,
            FtpProxyAddress = null,
            FtpProxyPort = null,
            UseSshAuth = false,
            IsValid = true,
            OperatorId = "op1",
        };

        var before = DateTimeOffset.UtcNow;
        await writer.HandleFtpServerCreatedAsync(CreateConfigurationChangedEnvelope("op1"), payload, CancellationToken.None);
        var after = DateTimeOffset.UtcNow;

        var row = await db.FtpServers.SingleAsync();
        Assert.Equal(payload.FtpServerId, row.FtpServerId);
        Assert.Equal(row.CreatedTs, row.LastModifiedTs);
        Assert.InRange(row.CreatedTs, before, after);
    }

    [Fact]
    public async Task F02_036_updates_ftp_server_fields_and_last_modified_ts()
    {
        await using var db = CreateDb();
        var writer = new FtpServerProjectionWriter(db);

        var id = Guid.NewGuid();
        var createdTs = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);
        var original = new FtpServer
        {
            FtpServerId = id,
            FtpServerName = "old",
            FtpDescription = "old desc",
            FtpServerHost = "oldhost",
            FtpTransferMode = "FTP",
            FtpUser = "olduser",
            FtpPasswordSecretRef = "secret://old",
            FtpPort = 1,
            FtpProxyAddress = null,
            FtpProxyPort = null,
            UseSshAuth = false,
            IsValid = true,
            OperatorId = "oldop",
            CreatedTs = createdTs,
            LastModifiedTs = createdTs,
        };

        db.FtpServers.Add(original);
        await db.SaveChangesAsync();

        var payload = new FtpServerProjectionPayload
        {
            FtpServerId = id,
            FtpServerName = "new",
            FtpDescription = "new desc",
            FtpServerHost = "newhost",
            FtpTransferMode = "SFTP",
            FtpUser = "newuser",
            FtpPasswordSecretRef = "secret://new",
            FtpPort = 22,
            FtpProxyAddress = "proxy",
            FtpProxyPort = 99,
            UseSshAuth = true,
            IsValid = true,
            OperatorId = "op2",
        };

        await writer.HandleFtpServerUpdatedAsync(CreateConfigurationChangedEnvelope("op2"), payload, CancellationToken.None);

        var updated = await db.FtpServers.SingleAsync();
        Assert.Equal("new", updated.FtpServerName);
        Assert.Equal("secret://new", updated.FtpPasswordSecretRef);
        Assert.Equal("op2", updated.OperatorId);
        Assert.Equal(createdTs, updated.CreatedTs);
        Assert.True(updated.LastModifiedTs > createdTs);
    }

    [Fact]
    public async Task F02_037_invalidates_ftp_server_and_updates_last_modified_ts()
    {
        await using var db = CreateDb();
        var writer = new FtpServerProjectionWriter(db);

        var id = Guid.NewGuid();
        var ts = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);
        db.FtpServers.Add(new FtpServer
        {
            FtpServerId = id,
            FtpServerName = "s",
            FtpDescription = "d",
            FtpServerHost = "h",
            FtpTransferMode = "FTP",
            FtpUser = "u",
            FtpPasswordSecretRef = "secret://pw",
            FtpPort = 21,
            FtpProxyAddress = null,
            FtpProxyPort = null,
            UseSshAuth = false,
            IsValid = true,
            OperatorId = "op1",
            CreatedTs = ts,
            LastModifiedTs = ts,
        });
        await db.SaveChangesAsync();

        var payload = new FtpServerProjectionPayload
        {
            FtpServerId = id,
            FtpServerName = "s",
            FtpDescription = "d",
            FtpServerHost = "h",
            FtpTransferMode = "FTP",
            FtpUser = "u",
            FtpPasswordSecretRef = "secret://pw",
            FtpPort = 21,
            FtpProxyAddress = null,
            FtpProxyPort = null,
            UseSshAuth = false,
            IsValid = false,
            OperatorId = "op2",
        };

        await writer.HandleFtpServerInvalidatedAsync(CreateConfigurationChangedEnvelope("op2"), payload, CancellationToken.None);

        var updated = await db.FtpServers.SingleAsync();
        Assert.False(updated.IsValid);
        Assert.Equal("op2", updated.OperatorId);
        Assert.True(updated.LastModifiedTs > ts);
    }

    [Fact]
    public async Task F01_073_updates_source_location_fields_and_last_modified_ts()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var id = Guid.NewGuid();
        var ts = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);

        db.SourceLocations.Add(new FtsSourceLocation
        {
            FtsSourceLocationId = id,
            ConfigId = 1,
            ConfigName = "c1",
            SourceProtocolCd = "FTP",
            SourceAddress = "a",
            SourceFileMask = "m",
            FtpServerId = null,
            SendNotificationsInd = false,
            OwnerName = "o",
            OwnerModule = Guid.NewGuid(),
            OwnerPhone = null,
            OwnerEmail = null,
            Description = null,
            RunOnMondayInd = false,
            RunOnTuesdayInd = false,
            RunOnWednesdayInd = false,
            RunOnThursdayInd = false,
            RunOnFridayInd = false,
            RunOnSaturdayInd = false,
            RunOnSundayInd = false,
            RunOnFirstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = null,
            StartTime24hhmm = "0000",
            EndTime24hhmm = "2359",
            FileDirection = null,
            TradingPartner = null,
            IsValid = true,
            IsSuspended = false,
            IsSuspendedChangedTs = null,
            OperatorId = "old",
            CreatedTs = ts,
            LastModifiedTs = ts,
        });
        await db.SaveChangesAsync();

        var changedDate = new DateTimeOffset(2026, 01, 02, 0, 0, 0, TimeSpan.Zero);
        var payload = new SourceLocationProjectionPayload
        {
            SourceLocationId = id.ToString(),
            ConfigId = 2,
            ConfigName = "c2",
            Description = "desc",
            FtpServerId = "",
            Protocol = "SFTP",
            Address = "addr",
            Mask = "mask",
            FileDirection = "in",
            SendNotifications = true,
            IsValid = true,
            IsSuspended = true,
            IsSuspendedChangedDate = changedDate,
            OwnerName = "owner",
            OwnerModule = Guid.NewGuid().ToString(),
            OwnerPhone = "p",
            OwnerEmail = "e",
            RunOnMonday = true,
            RunOnTuesday = true,
            RunOnWed = false,
            RunOnThrus = false,
            RunOnFri = false,
            RunOnSat = false,
            RunOnSun = false,
            RunOnFrstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = null,
            StartTime = "0100",
            EndTime = "0200",
            TradingPartner = "tp",
            OperatorId = "op1",
            Destinations = new(),
        };

        await writer.HandleSourceLocationUpdatedAsync(CreateConfigurationChangedEnvelope("op1"), payload, CancellationToken.None);

        var updated = await db.SourceLocations.SingleAsync();
        Assert.Equal(2, updated.ConfigId);
        Assert.True(updated.IsSuspended);
        Assert.Equal(changedDate, updated.IsSuspendedChangedTs);
        Assert.Equal("op1", updated.OperatorId);
        Assert.True(updated.LastModifiedTs > ts);
    }

    [Fact]
    public async Task F01_074_calls_destination_upsert_when_source_location_id_is_present()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var sourceId = Guid.NewGuid();

        db.SourceLocations.Add(new FtsSourceLocation
        {
            FtsSourceLocationId = sourceId,
            ConfigId = 1,
            ConfigName = "c1",
            SourceProtocolCd = "FTP",
            SourceAddress = "a",
            SourceFileMask = "m",
            FtpServerId = null,
            SendNotificationsInd = false,
            OwnerName = "o",
            OwnerModule = Guid.NewGuid(),
            OwnerPhone = null,
            OwnerEmail = null,
            Description = null,
            RunOnMondayInd = false,
            RunOnTuesdayInd = false,
            RunOnWednesdayInd = false,
            RunOnThursdayInd = false,
            RunOnFridayInd = false,
            RunOnSaturdayInd = false,
            RunOnSundayInd = false,
            RunOnFirstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = null,
            StartTime24hhmm = "0000",
            EndTime24hhmm = "2359",
            FileDirection = null,
            TradingPartner = null,
            IsValid = true,
            IsSuspended = false,
            IsSuspendedChangedTs = null,
            OperatorId = "old",
            CreatedTs = DateTimeOffset.UtcNow,
            LastModifiedTs = DateTimeOffset.UtcNow,
        });
        await db.SaveChangesAsync();

        var dest = new DestinationProjectionPayload
        {
            DestinationId = Guid.NewGuid(),
            DestinationAddress = "d1",
            DestinationProtocol = "FTP",
            OverwriteAtDestination = true,
            CompressAtDestination = false,
            RenameAtDestination = false,
            DestinationArchive = false,
            IsValid = true,
            FtpServerId = null,
            OperatorId = "op1",
            RenamePattern = null,
            ContactName = null,
            ContactPhone = null,
            ContactEmail = null,
            FteMetadataId = null,
            DestinationType = null,
            ContractName = null,
        };

        var payload = new SourceLocationProjectionPayload
        {
            SourceLocationId = sourceId.ToString(),
            ConfigId = 1,
            ConfigName = "c1",
            Description = null,
            FtpServerId = "",
            Protocol = "FTP",
            Address = "a",
            Mask = "m",
            FileDirection = null,
            SendNotifications = false,
            IsValid = true,
            IsSuspended = false,
            IsSuspendedChangedDate = null,
            OwnerName = "o",
            OwnerModule = Guid.NewGuid().ToString(),
            OwnerPhone = null,
            OwnerEmail = null,
            RunOnMonday = false,
            RunOnTuesday = false,
            RunOnWed = false,
            RunOnThrus = false,
            RunOnFri = false,
            RunOnSat = false,
            RunOnSun = false,
            RunOnFrstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = null,
            StartTime = "0000",
            EndTime = "0000",
            TradingPartner = null,
            OperatorId = "op1",
            Destinations = new() { dest },
        };

        await writer.HandleSourceLocationUpdatedAsync(CreateConfigurationChangedEnvelope("op1"), payload, CancellationToken.None);

        var destinations = await db.Destinations.ToListAsync();
        Assert.Single(destinations);
        Assert.Equal(sourceId, destinations[0].FtsSourceLocationId);
        Assert.Equal("d1", destinations[0].DestinationAddress);
    }

    [Fact]
    public async Task F01_075_invalidates_all_destinations_when_source_location_updated_is_invalid()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var sourceId = Guid.NewGuid();
        var destId = Guid.NewGuid();

        db.SourceLocations.Add(new FtsSourceLocation
        {
            FtsSourceLocationId = sourceId,
            ConfigId = 1,
            ConfigName = "c1",
            SourceProtocolCd = "FTP",
            SourceAddress = "a",
            SourceFileMask = "m",
            FtpServerId = null,
            SendNotificationsInd = false,
            OwnerName = "o",
            OwnerModule = Guid.NewGuid(),
            OwnerPhone = null,
            OwnerEmail = null,
            Description = null,
            RunOnMondayInd = false,
            RunOnTuesdayInd = false,
            RunOnWednesdayInd = false,
            RunOnThursdayInd = false,
            RunOnFridayInd = false,
            RunOnSaturdayInd = false,
            RunOnSundayInd = false,
            RunOnFirstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = null,
            StartTime24hhmm = "0000",
            EndTime24hhmm = "2359",
            FileDirection = null,
            TradingPartner = null,
            IsValid = true,
            IsSuspended = false,
            IsSuspendedChangedTs = null,
            OperatorId = "old",
            CreatedTs = DateTimeOffset.UtcNow,
            LastModifiedTs = DateTimeOffset.UtcNow,
        });

        var destTs = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);
        db.Destinations.Add(new FtsDestination
        {
            DestinationId = destId,
            FtsSourceLocationId = sourceId,
            DestinationAddress = "d",
            DestProtocolCd = "FTP",
            OverwriteAtDestInd = false,
            CompressAtDestInd = false,
            RenameAtDestInd = false,
            DestIsArchiveInd = false,
            IsValid = true,
            FtpServerId = null,
            RenamePattern = null,
            ContactName = null,
            ContactPhone = null,
            ContactEmail = null,
            OperatorId = "old",
            CreatedTs = destTs,
            LastModifiedTs = destTs,
            CdeEncryption = null,
            EncryptionPublicKey = null,
            EncryptionPrivateKeySecretRef = null,
            PrivateKeyPasswordSecretRef = null,
            ExternalFileDefnId = null,
            DestinationType = null,
            ContractName = null,
        });

        await db.SaveChangesAsync();

        var payload = new SourceLocationProjectionPayload
        {
            SourceLocationId = sourceId.ToString(),
            ConfigId = 1,
            ConfigName = "c1",
            Description = null,
            FtpServerId = "",
            Protocol = "FTP",
            Address = "a",
            Mask = "m",
            FileDirection = null,
            SendNotifications = false,
            IsValid = false,
            IsSuspended = false,
            IsSuspendedChangedDate = null,
            OwnerName = "o",
            OwnerModule = Guid.NewGuid().ToString(),
            OwnerPhone = null,
            OwnerEmail = null,
            RunOnMonday = false,
            RunOnTuesday = false,
            RunOnWed = false,
            RunOnThrus = false,
            RunOnFri = false,
            RunOnSat = false,
            RunOnSun = false,
            RunOnFrstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = null,
            StartTime = "0000",
            EndTime = "0000",
            TradingPartner = null,
            OperatorId = "op2",
            Destinations = new(),
        };

        await writer.HandleSourceLocationUpdatedAsync(CreateConfigurationChangedEnvelope("op2"), payload, CancellationToken.None);

        var updatedDest = await db.Destinations.SingleAsync();
        Assert.False(updatedDest.IsValid);
        Assert.Equal("op2", updatedDest.OperatorId);
        Assert.Equal(destTs, updatedDest.LastModifiedTs);
    }

    [Fact]
    public async Task F01_076_invalidates_source_location_without_touching_last_modified_ts()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var sourceId = Guid.NewGuid();
        var ts = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);

        db.SourceLocations.Add(new FtsSourceLocation
        {
            FtsSourceLocationId = sourceId,
            ConfigId = 1,
            ConfigName = "c1",
            SourceProtocolCd = "FTP",
            SourceAddress = "a",
            SourceFileMask = "m",
            FtpServerId = null,
            SendNotificationsInd = false,
            OwnerName = "o",
            OwnerModule = Guid.NewGuid(),
            OwnerPhone = null,
            OwnerEmail = null,
            Description = null,
            RunOnMondayInd = false,
            RunOnTuesdayInd = false,
            RunOnWednesdayInd = false,
            RunOnThursdayInd = false,
            RunOnFridayInd = false,
            RunOnSaturdayInd = false,
            RunOnSundayInd = false,
            RunOnFirstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = null,
            StartTime24hhmm = "0000",
            EndTime24hhmm = "2359",
            FileDirection = null,
            TradingPartner = null,
            IsValid = true,
            IsSuspended = false,
            IsSuspendedChangedTs = null,
            OperatorId = "old",
            CreatedTs = ts,
            LastModifiedTs = ts,
        });
        await db.SaveChangesAsync();

        var payload = new SourceLocationInvalidatedProjectionPayload
        {
            SourceLocationId = sourceId.ToString(),
            OperatorId = "op2",
            Destinations = null,
        };

        await writer.HandleSourceLocationInvalidatedAsync(CreateConfigurationChangedEnvelope("op2"), payload, CancellationToken.None);

        var updated = await db.SourceLocations.SingleAsync();
        Assert.False(updated.IsValid);
        Assert.Equal("op2", updated.OperatorId);
        Assert.Equal(ts, updated.LastModifiedTs);
    }

    [Fact]
    public async Task F01_077_upserts_destinations_then_invalidates_all_for_source_location()
    {
        await using var db = CreateDb();
        var writer = new FtsSourceLocationProjectionWriter(db);

        var sourceId = Guid.NewGuid();

        db.SourceLocations.Add(new FtsSourceLocation
        {
            FtsSourceLocationId = sourceId,
            ConfigId = 1,
            ConfigName = "c1",
            SourceProtocolCd = "FTP",
            SourceAddress = "a",
            SourceFileMask = "m",
            FtpServerId = null,
            SendNotificationsInd = false,
            OwnerName = "o",
            OwnerModule = Guid.NewGuid(),
            OwnerPhone = null,
            OwnerEmail = null,
            Description = null,
            RunOnMondayInd = false,
            RunOnTuesdayInd = false,
            RunOnWednesdayInd = false,
            RunOnThursdayInd = false,
            RunOnFridayInd = false,
            RunOnSaturdayInd = false,
            RunOnSundayInd = false,
            RunOnFirstDayOfMonthInd = false,
            RunOnLastDayOfMonthInd = false,
            RunOnThisDayOfMonth = null,
            StartTime24hhmm = "0000",
            EndTime24hhmm = "2359",
            FileDirection = null,
            TradingPartner = null,
            IsValid = true,
            IsSuspended = false,
            IsSuspendedChangedTs = null,
            OperatorId = "old",
            CreatedTs = DateTimeOffset.UtcNow,
            LastModifiedTs = DateTimeOffset.UtcNow,
        });
        await db.SaveChangesAsync();

        var dest = new DestinationProjectionPayload
        {
            DestinationId = Guid.NewGuid(),
            DestinationAddress = "d1",
            DestinationProtocol = "FTP",
            OverwriteAtDestination = true,
            CompressAtDestination = false,
            RenameAtDestination = false,
            DestinationArchive = false,
            IsValid = true,
            FtpServerId = null,
            OperatorId = "op1",
            RenamePattern = null,
            ContactName = null,
            ContactPhone = null,
            ContactEmail = null,
            FteMetadataId = null,
            DestinationType = null,
            ContractName = null,
        };

        var payload = new SourceLocationInvalidatedProjectionPayload
        {
            SourceLocationId = sourceId.ToString(),
            OperatorId = "op2",
            Destinations = new() { dest },
        };

        await writer.HandleSourceLocationInvalidatedAsync(CreateConfigurationChangedEnvelope("op2"), payload, CancellationToken.None);

        var destinations = await db.Destinations.ToListAsync();
        Assert.Single(destinations);
        Assert.False(destinations[0].IsValid);
        Assert.Equal("op2", destinations[0].OperatorId);
    }
}
