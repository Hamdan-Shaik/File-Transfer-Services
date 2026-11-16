using System.Security.Claims;

using FxferService.Api.Commands.Controllers;
using FxferService.Api.Commands.Models;
using FxferService.Application.CommandHandlers;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Host.UnitTests;

public sealed class F02_027_TransferConfigurationsCommandsControllerTests
{
    [Fact]
    public async Task F02_027_routes_add_transfer_configuration_command_and_returns_200()
    {
        await using var db = CreateDbContext();
        var handler = new FtsSourceLocationAddCommandHandler(db);
        var controller = CreateController(db, handler);

        var body = CreateValidUpsertBody(transferConfigurationId: null, configNumber: null);
        var result = await controller.UpsertTransferConfiguration(body, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<TransferConfigurationIdResponse>(ok.Value);
        Assert.NotEqual(Guid.Empty, payload.TransferConfigurationId);
    }

    [Fact]
    public async Task F02_039_routes_update_transfer_configuration_command_and_returns_200()
    {
        var id = Guid.NewGuid();
        await using var db = CreateDbContext();
        db.SourceLocations.Add(new FtsSourceLocationEntity
        {
            FtsSourceLocationId = id,
            ConfigId = 123,
            ConfigName = "existing",
        });
        await db.SaveChangesAsync();

        var handler = new FtsSourceLocationAddCommandHandler(db);
        var controller = CreateController(db, handler);

        var body = CreateValidUpsertBody(transferConfigurationId: id, configNumber: 123);
        var result = await controller.UpsertTransferConfiguration(body, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<TransferConfigurationIdResponse>(ok.Value);
        Assert.Equal(id, payload.TransferConfigurationId);
    }

    [Fact]
    public async Task F02_041_routes_invalidate_transfer_configuration_command_and_returns_200()
    {
        var id = Guid.NewGuid();
        await using var db = CreateDbContext();
        db.SourceLocations.Add(new FtsSourceLocationEntity
        {
            FtsSourceLocationId = id,
            ConfigId = 123,
            ConfigName = "existing",
        });
        await db.SaveChangesAsync();

        var handler = new FtsSourceLocationAddCommandHandler(db);
        var controller = CreateController(db, handler);

        var result = await controller.InvalidateTransferConfiguration(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<TransferConfigurationStateResponse>(ok.Value);
        Assert.Equal(id, payload.TransferConfigurationId);
        Assert.False(payload.IsActive);
    }

    private static TransferConfigurationUpsert CreateValidUpsertBody(Guid? transferConfigurationId, int? configNumber)
    {
        var ftpServerId = Guid.NewGuid();

        return new TransferConfigurationUpsert(
            TransferConfigurationId: transferConfigurationId,
            ConfigNumber: configNumber,
            Name: "cfg",
            Description: null,
            ProtocolCode: "1",
            Address: "addr",
            FileMask: "file.txt",
            FtpServerId: ftpServerId,
            SendNotifications: false,
            Owner: new OwnerInfo("owner", Guid.NewGuid(), "5551234", "a@b.com"),
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
                StartTime24hhmm: "0100",
                EndTime24hhmm: "0200"),
            FileDirection: null,
            TradingPartner: null,
            IsActive: true,
            IsSuspended: false,
            SuspendedChangedAt: null,
            Destinations: new[]
            {
                new DestinationUpsert(
                    DestinationId: null,
                    Address: "dest",
                    ProtocolCode: "1",
                    OverwriteAtDestination: false,
                    CompressAtDestination: false,
                    RenameAtDestination: false,
                    DestinationIsArchive: false,
                    IsActive: true,
                    FtpServerId: ftpServerId,
                    RenamePattern: null,
                    Contact: null,
                    Encryption: null,
                    ExternalFileDefinitionId: null,
                    DestinationType: null,
                    ContractName: null),
            });
    }

    private static FxferDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new FxferDbContext(options);
    }

    private static TransferConfigurationsCommandsController CreateController(
        FxferDbContext db,
        FtsSourceLocationAddCommandHandler handler)
    {
        var controller = new TransferConfigurationsCommandsController(handler, db);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.NameIdentifier, "operator-1") },
                    authenticationType: "Test")),
            },
        };
        return controller;
    }
}
