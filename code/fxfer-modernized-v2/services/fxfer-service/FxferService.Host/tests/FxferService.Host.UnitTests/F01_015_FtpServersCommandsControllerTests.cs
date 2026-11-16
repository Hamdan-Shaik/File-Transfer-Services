using System.Security.Claims;

using FxferService.Api.Commands.Controllers;
using FxferService.Api.Commands.Models;
using FxferService.Application.CommandHandlers;
using FxferService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Host.UnitTests;

public sealed class F01_015_FtpServersCommandsControllerTests
{
    [Fact]
    public async Task F01_015_routes_create_ftp_server_command_and_returns_201()
    {
        await using var db = CreateDbContext();
        var controller = CreateController(db);

        var body = new FtpServerUpsert(
            Name: "Server-1",
            IpAddress: "1.2.3.4",
            TransferMode: "I",
            User: "user1",
            Password: "pw",
            Description: "desc",
            Port: 21,
            ProxyAddress: null,
            ProxyPort: null,
            SshAuthentication: false,
            IsActive: true);

        var result = await controller.CreateFtpServer(body, CancellationToken.None);

        var created = Assert.IsType<CreatedResult>(result.Result);
        var payload = Assert.IsType<FtpServerIdResponse>(created.Value);
        Assert.NotEqual(Guid.Empty, payload.FtpServerId);
    }

    [Fact]
    public async Task F01_017_routes_update_ftp_server_command_and_returns_200()
    {
        await using var db = CreateDbContext();
        var controller = CreateController(db);

        var id = Guid.NewGuid();
        var body = new FtpServerUpsert(
            Name: "Server-1",
            IpAddress: "1.2.3.4",
            TransferMode: "I",
            User: "user1",
            Password: "pw",
            Description: "desc",
            Port: 21,
            ProxyAddress: null,
            ProxyPort: null,
            SshAuthentication: false,
            IsActive: true);

        var result = await controller.UpdateFtpServer(id, body, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<FtpServerStateResponse>(ok.Value);
        Assert.Equal(id, payload.FtpServerId);
        Assert.True(payload.IsActive);
    }

    [Fact]
    public async Task F01_019_routes_invalidate_ftp_server_command_and_returns_200()
    {
        await using var db = CreateDbContext();
        var controller = CreateController(db);

        var id = Guid.NewGuid();
        var result = await controller.InvalidateFtpServer(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<FtpServerStateResponse>(ok.Value);
        Assert.Equal(id, payload.FtpServerId);
        Assert.False(payload.IsActive);
    }

    private static FxferDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new FxferDbContext(options);
    }

    private static FtpServersCommandsController CreateController(FxferDbContext db)
    {
        var handler = new FtpServerCommandHandler(db);
        var controller = new FtpServersCommandsController(handler);
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
