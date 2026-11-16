using System.Security.Claims;

using FxferService.Api.Commands.Controllers;
using FxferService.Api.Commands.Models;
using FxferService.Api.Queries.Controllers;
using FxferService.Api.Queries.Models;
using FxferService.Application.CommandHandlers;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Tests;

public sealed class BackendManageSourceLocationsApiDoorRuleTests
{
    private static FxferDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FxferDbContext(options);
    }

    private static ClaimsPrincipal CreateUser(string operatorId)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, operatorId),
        }, authenticationType: "test");

        return new ClaimsPrincipal(identity);
    }

    [Fact]
    public void F01_001_get_ftp_servers_routes_query_and_returns_paged_results()
    {
        var db = CreateDb();
        db.FtpServers.Add(new FxferService.Infrastructure.Persistence.Entities.FtpServer
        {
            FtpServerId = Guid.NewGuid(),
            FtpServerName = "Srv",
            FtpServerIp = "127.0.0.1",
            FtpTransferMode = "I",
            FtpUser = "user",
            FtpPassword = "pw",
            FtpDescription = "desc",
            FtpPort = 21,
            FtpProxyAddress = null,
            FtpProxyPort = null,
            UseSSHAuth = false,
            IsValid = true,
            OperatorId = "op",
            CreationDate = new DateTime(2020, 1, 1),
            LastModifiedDate = new DateTime(2020, 1, 2),
        });
        db.SaveChanges();

        var controller = new FtpServersQueriesController(db);
        var result = controller.SearchFtpServers(
            isActive: true,
            name: "Srv",
            description: null,
            user: null,
            page: null,
            pageSize: null,
            sortBy: null,
            sortDir: null,
            includePassword: false);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var paged = Assert.IsType<PagedResult<FxferService.Api.Commands.Models.FtpServer>>(ok.Value);
        var server = Assert.Single(paged.Items);
        Assert.Null(server.Password);
        Assert.Equal(1, paged.RowCount);
    }

    [Fact]
    public void F02_001_search_source_locations_routes_query_and_returns_paged_results()
    {
        var db = CreateDb();
        var id = Guid.NewGuid();

        db.SourceLocations.Add(new FtsSourceLocationEntity
        {
            FtsSourceLocationId = id,
            ConfigId = 123,
            ConfigName = "Cfg",
            SourceProtocolCode = "FTP",
            SourceAddress = "addr",
            SourceFileMask = "mask",
            FtpServerId = null,
            SendNotifications = false,
            OwnerName = "Owner",
            ModuleId = Guid.NewGuid(),
            OwnerPhone = "555",
            OwnerEmail = "owner@example.com",
            ConfigDescription = "desc",
            RunOnMonday = false,
            RunOnTuesday = false,
            RunOnWednesday = false,
            RunOnThursday = false,
            RunOnFriday = false,
            RunOnSaturday = false,
            RunOnSunday = false,
            RunOnFirstDayOfMonth = false,
            RunOnLastDayOfMonth = false,
            RunOnThisDayOfMonth = 0,
            StartTime24hhmm = "0000",
            EndTime24hhmm = "0000",
            FileDirection = "IN",
            TradingPartner = "TP",
            IsValid = true,
            IsSuspended = false,
            OperatorId = "op",
            CreatedAt = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            LastModifiedAt = new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
        });

        db.SaveChanges();

        var controller = new TransferConfigurationsQueriesController(db);
        var result = controller.SearchTransferConfigurations(
            isActive: null,
            configNumber: 123,
            nameContains: null,
            ownerNameContains: null,
            protocolCode: null,
            page: null,
            pageSize: null,
            sortBy: null,
            sortDir: null);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var paged = Assert.IsType<PagedResult<TransferConfigurationSummary>>(ok.Value);
        var cfg = Assert.Single(paged.Items);
        Assert.Equal(id, cfg.TransferConfigurationId);
        Assert.Equal(1, paged.RowCount);
    }

    [Fact]
    public void F02_002_get_source_location_routes_query_and_returns_transfer_configuration()
    {
        var db = CreateDb();
        var id = Guid.NewGuid();

        db.SourceLocations.Add(new FtsSourceLocationEntity
        {
            FtsSourceLocationId = id,
            ConfigId = 123,
            ConfigName = "Cfg",
            SourceProtocolCode = "FTP",
            SourceAddress = "addr",
            SourceFileMask = "mask",
            FtpServerId = null,
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
        });

        db.SaveChanges();

        var controller = new TransferConfigurationsQueriesController(db);
        var result = controller.GetTransferConfiguration(id);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var cfg = Assert.IsType<TransferConfiguration>(ok.Value);
        Assert.Equal(id, cfg.TransferConfigurationId);
    }

    [Fact]
    public void F02_043_get_integration_file_routes_query_and_returns_paged_results()
    {
        var db = CreateDb();
        var id = Guid.NewGuid();

        db.IntegrationFiles.Add(new FtsIntegrationFileEntity
        {
            FtsIntegrationFileId = id,
            GroupId = Guid.NewGuid(),
            ConfigId = 123,
            FileName = "file.txt",
            OutboundMetadataDefnId = Guid.NewGuid(),
            IsProcessed = false,
            OperatorId = "op",
            IsValid = true,
            CreatedAt = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            LastModifiedAt = new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
        });
        db.SaveChanges();

        var controller = new IntegrationFilesQueriesController(db);
        var result = controller.SearchIntegrationFiles(id, fileNameContains: null, page: null, pageSize: null);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var paged = Assert.IsType<PagedResult<IntegrationFile>>(ok.Value);
        Assert.Single(paged.Items);
        Assert.Equal(1, paged.RowCount);
    }

    [Fact]
    public async Task F02_044_update_integration_file_routes_command_and_returns_id_response()
    {
        var handler = new FtsIntegrationFileCommandHandler();
        var controller = new IntegrationFilesCommandsController(handler);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CreateUser("operator") },
        };

        var id = Guid.NewGuid();
        var result = await controller.UpdateIntegrationFile(id, new IntegrationFileUpdate(123, IsActive: true), CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<IntegrationFileIdResponse>(ok.Value);
        Assert.Equal(id, payload.IntegrationFileId);
    }

    [Fact]
    public async Task F01_015_add_ftp_server_routes_command_and_returns_created_response()
    {
        var db = CreateDb();
        var handler = new FtpServerCommandHandler(db);

        var controller = new FtpServersCommandsController(handler);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CreateUser("operator") },
        };

        var result = await controller.CreateFtpServer(
            new FtpServerUpsert(
                Name: "Srv",
                IpAddress: "127.0.0.1",
                TransferMode: "I",
                User: "user",
                Password: "pw",
                Description: "desc",
                Port: 21,
                ProxyAddress: null,
                ProxyPort: null,
                SshAuthentication: false,
                IsActive: true),
            CancellationToken.None);

        var created = Assert.IsType<CreatedResult>(result.Result);
        var payload = Assert.IsType<FtpServerIdResponse>(created.Value);
        Assert.NotEqual(Guid.Empty, payload.FtpServerId);
    }

    [Fact]
    public async Task F01_017_update_ftp_server_routes_command_and_returns_state_response()
    {
        var db = CreateDb();
        var handler = new FtpServerCommandHandler(db);

        var controller = new FtpServersCommandsController(handler);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CreateUser("operator") },
        };

        var id = Guid.NewGuid();
        var result = await controller.UpdateFtpServer(
            id,
            new FtpServerUpsert(
                Name: "Srv",
                IpAddress: "127.0.0.1",
                TransferMode: "I",
                User: "user",
                Password: "pw",
                Description: "desc",
                Port: 21,
                ProxyAddress: null,
                ProxyPort: null,
                SshAuthentication: false,
                IsActive: true),
            CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<FtpServerStateResponse>(ok.Value);
        Assert.Equal(id, payload.FtpServerId);
        Assert.True(payload.IsActive);
    }

    [Fact]
    public async Task F01_019_invalidate_ftp_server_routes_command_and_returns_inactive_state_response()
    {
        var db = CreateDb();
        var handler = new FtpServerCommandHandler(db);

        var controller = new FtpServersCommandsController(handler);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CreateUser("operator") },
        };

        var id = Guid.NewGuid();
        var result = await controller.InvalidateFtpServer(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<FtpServerStateResponse>(ok.Value);
        Assert.Equal(id, payload.FtpServerId);
        Assert.False(payload.IsActive);
    }

    [Fact]
    public async Task F02_027_file_transfer_add_routes_command_and_returns_id_response()
    {
        var db = CreateDb();
        var handler = new FtsSourceLocationAddCommandHandler(db);

        var controller = new TransferConfigurationsCommandsController(handler, db);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CreateUser("operator") },
        };

        var result = await controller.UpsertTransferConfiguration(
            new TransferConfigurationUpsert(
                TransferConfigurationId: null,
                ConfigNumber: null,
                Name: "Cfg",
                Description: "desc",
                ProtocolCode: "FTP",
                Address: "addr",
                FileMask: "mask.txt",
                FtpServerId: null,
                SendNotifications: true,
                Owner: new OwnerInfo(Name: "Owner", ModuleId: Guid.NewGuid(), Phone: "555", Email: "owner@example.com"),
                Schedule: new ScheduleInfo(
                    RunOnMonday: true,
                    RunOnTuesday: false,
                    RunOnWednesday: false,
                    RunOnThursday: false,
                    RunOnFriday: false,
                    RunOnSaturday: false,
                    RunOnSunday: false,
                    RunOnFirstDayOfMonth: false,
                    RunOnLastDayOfMonth: false,
                    RunOnThisDayOfMonth: 0,
                    StartTime24hhmm: "0000",
                    EndTime24hhmm: "2359"),
                FileDirection: "IN",
                TradingPartner: "TP",
                IsActive: true,
                IsSuspended: false,
                SuspendedChangedAt: null,
                Destinations: new[]
                {
                    new DestinationUpsert(
                        DestinationId: null,
                        Address: "dest",
                        ProtocolCode: "FTP",
                        OverwriteAtDestination: false,
                        CompressAtDestination: false,
                        RenameAtDestination: false,
                        DestinationIsArchive: false,
                        IsActive: true,
                        FtpServerId: Guid.NewGuid(),
                        RenamePattern: null,
                        Contact: null,
                        Encryption: null,
                        ExternalFileDefinitionId: null,
                        DestinationType: null,
                        ContractName: null),
                }),
            CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<TransferConfigurationIdResponse>(ok.Value);
        Assert.NotEqual(Guid.Empty, payload.TransferConfigurationId);
    }

    [Fact]
    public async Task F02_039_update_file_transfer_source_location_routes_command_and_returns_id_response()
    {
        var db = CreateDb();
        var handler = new FtsSourceLocationAddCommandHandler(db);

        var existingId = Guid.NewGuid();
        db.SourceLocations.Add(new FtsSourceLocationEntity
        {
            FtsSourceLocationId = existingId,
            ConfigId = 123,
            ConfigName = "Existing",
            SourceProtocolCode = "FTP",
            SourceAddress = "addr",
            SourceFileMask = "mask",
            FtpServerId = null,
            SendNotifications = false,
            OwnerName = "Owner",
            ModuleId = Guid.NewGuid(),
            OwnerPhone = "555",
            OwnerEmail = "owner@example.com",
            ConfigDescription = "desc",
            RunOnMonday = true,
            RunOnTuesday = false,
            RunOnWednesday = false,
            RunOnThursday = false,
            RunOnFriday = false,
            RunOnSaturday = false,
            RunOnSunday = false,
            RunOnFirstDayOfMonth = false,
            RunOnLastDayOfMonth = false,
            RunOnThisDayOfMonth = 0,
            StartTime24hhmm = "0000",
            EndTime24hhmm = "2359",
            FileDirection = "IN",
            TradingPartner = "TP",
            IsValid = true,
            IsSuspended = false,
            OperatorId = "operator",
            CreatedAt = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            LastModifiedAt = new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
        });
        db.SaveChanges();

        var controller = new TransferConfigurationsCommandsController(handler, db);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CreateUser("operator") },
        };

        var result = await controller.UpsertTransferConfiguration(
            new TransferConfigurationUpsert(
                TransferConfigurationId: existingId,
                ConfigNumber: 123,
                Name: "Updated",
                Description: "desc",
                ProtocolCode: "FTP",
                Address: "addr",
                FileMask: "mask.txt",
                FtpServerId: null,
                SendNotifications: true,
                Owner: new OwnerInfo(Name: "Owner", ModuleId: Guid.NewGuid(), Phone: "555", Email: "owner@example.com"),
                Schedule: new ScheduleInfo(
                    RunOnMonday: true,
                    RunOnTuesday: false,
                    RunOnWednesday: false,
                    RunOnThursday: false,
                    RunOnFriday: false,
                    RunOnSaturday: false,
                    RunOnSunday: false,
                    RunOnFirstDayOfMonth: false,
                    RunOnLastDayOfMonth: false,
                    RunOnThisDayOfMonth: 0,
                    StartTime24hhmm: "0000",
                    EndTime24hhmm: "2359"),
                FileDirection: "IN",
                TradingPartner: "TP",
                IsActive: true,
                IsSuspended: false,
                SuspendedChangedAt: null,
                Destinations: new[]
                {
                    new DestinationUpsert(
                        DestinationId: null,
                        Address: "dest",
                        ProtocolCode: "FTP",
                        OverwriteAtDestination: false,
                        CompressAtDestination: false,
                        RenameAtDestination: false,
                        DestinationIsArchive: false,
                        IsActive: true,
                        FtpServerId: Guid.NewGuid(),
                        RenamePattern: null,
                        Contact: null,
                        Encryption: null,
                        ExternalFileDefinitionId: null,
                        DestinationType: null,
                        ContractName: null),
                }),
            CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<TransferConfigurationIdResponse>(ok.Value);
        Assert.Equal(existingId, payload.TransferConfigurationId);
    }

    [Fact]
    public async Task F02_041_invalidate_file_transfer_source_location_routes_command_and_returns_inactive_state_response()
    {
        var db = CreateDb();
        var handler = new FtsSourceLocationAddCommandHandler(db);

        var id = Guid.NewGuid();
        db.SourceLocations.Add(new FtsSourceLocationEntity
        {
            FtsSourceLocationId = id,
            ConfigId = 123,
            ConfigName = "Existing",
            SourceProtocolCode = "FTP",
            SourceAddress = "addr",
            SourceFileMask = "mask",
            FtpServerId = null,
            SendNotifications = false,
            OwnerName = "Owner",
            ModuleId = Guid.NewGuid(),
            OwnerPhone = "555",
            OwnerEmail = "owner@example.com",
            ConfigDescription = "desc",
            RunOnMonday = true,
            RunOnTuesday = false,
            RunOnWednesday = false,
            RunOnThursday = false,
            RunOnFriday = false,
            RunOnSaturday = false,
            RunOnSunday = false,
            RunOnFirstDayOfMonth = false,
            RunOnLastDayOfMonth = false,
            RunOnThisDayOfMonth = 0,
            StartTime24hhmm = "0000",
            EndTime24hhmm = "2359",
            FileDirection = "IN",
            TradingPartner = "TP",
            IsValid = true,
            IsSuspended = false,
            OperatorId = "operator",
            CreatedAt = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            LastModifiedAt = new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
        });
        db.SaveChanges();

        var controller = new TransferConfigurationsCommandsController(handler, db);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CreateUser("operator") },
        };

        var result = await controller.InvalidateTransferConfiguration(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<TransferConfigurationStateResponse>(ok.Value);
        Assert.Equal(id, payload.TransferConfigurationId);
        Assert.False(payload.IsActive);
    }
}

