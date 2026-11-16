using FxferService.Application.CommandHandlers;
using RequestorModel = FxferService.Application.LegacyContracts.RequestorModel;
using FxferService.Domain.Commands;
using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Exceptions;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Tests;

public sealed class FtpServerCommandHandlerRuleTests
{
    private static FxferDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FxferDbContext(options);
    }

    private static FxferService.Infrastructure.Persistence.Entities.FtpServer CreateServer(Guid id, string name, bool isValid)
    {
        return new FxferService.Infrastructure.Persistence.Entities.FtpServer()
        {
            FtpServerId = id,
            FtpServerName = name,
            FtpDescription = "desc",
            FtpServerIp = "127.0.0.1",
            FtpTransferMode = "I",
            FtpUser = "user",
            FtpPassword = "pw",
            FtpPort = 21,
            FtpProxyAddress = null,
            FtpProxyPort = null,
            IsValid = isValid,
            OperatorId = "op",
            LastModifiedDate = new DateTime(2020, 1, 2),
            CreationDate = new DateTime(2020, 1, 1),
            UseSSHAuth = false,
        };
    }

    private static AddFtpServerCommand CreateValidAddCommand(Guid id, string name)
    {
        return new AddFtpServerCommand
        {
            FtpServerId = id,
            FtpServerName = name,
            FtpDescription = "desc",
            FtpServerIp = "127.0.0.1",
            FtpTransferMode = "I",
            FtpUser = "user",
            FtpPassword = "pw",
            FtpPort = 21,
            FtpProxyAddress = null,
            FtpProxyPort = null,
            UseSSHAuth = false,
            IsValid = false,
            Requestor = new RequestorModel { IdentifierId = "operator" },
            OperatorId = "operator",
        };
    }

    [Fact]
    public async Task F01_021_add_validates_command_before_returning_event()
    {
        var db = CreateDb();
        var handler = new FtpServerCommandHandler(db);

        var command = new AddFtpServerCommand
        {
            FtpServerName = null,
        };

        var exception = await Assert.ThrowsAsync<BusinessException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Contains(exception.BusinessMessages, m => m.FieldName == nameof(AddFtpServerCommand.FtpServerName));
    }

    [Fact]
    public async Task F01_022_update_validates_command_before_returning_event()
    {
        var db = CreateDb();
        var handler = new FtpServerCommandHandler(db);

        var command = new UpdateFtpServerCommand
        {
            FtpServerName = null,
        };

        var exception = await Assert.ThrowsAsync<BusinessException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Contains(exception.BusinessMessages, m => m.FieldName == nameof(AddFtpServerCommand.FtpServerName));
    }

    [Fact]
    public async Task F01_029_add_rejects_a_duplicate_valid_ftp_server_name_with_a_different_id()
    {
        var db = CreateDb();
        db.FtpServers.Add(CreateServer(Guid.NewGuid(), "Dup", isValid: true));
        db.SaveChanges();

        var handler = new FtpServerCommandHandler(db);
        var command = CreateValidAddCommand(Guid.NewGuid(), "  dup  ");

        var exception = await Assert.ThrowsAsync<BusinessException>(() => handler.HandleAsync(command, CancellationToken.None));
        var message = Assert.Single(exception.BusinessMessages, m => m.MessageKey == CommonConstants.Validation.ErrorType.DuplicatedFtpServerName);
        Assert.Equal("  dup  ", message.FieldName);
        Assert.Equal(CommonConstants.Validation.Messages.DuplicatedFtpServerName, message.MessageDefault);
    }

    [Fact]
    public async Task F01_032_add_builds_event_with_is_valid_forced_true_and_operator_id()
    {
        var db = CreateDb();
        var handler = new FtpServerCommandHandler(db);

        var command = CreateValidAddCommand(Guid.NewGuid(), "Name");
        command.IsValid = false;

        var ev = await handler.HandleAsync(command, CancellationToken.None);
        Assert.True(ev.IsValid);
        Assert.Equal("operator", ev.OperatorId);
    }

    [Fact]
    public async Task F01_033_update_builds_event_with_operator_id_from_requestor()
    {
        var db = CreateDb();
        var handler = new FtpServerCommandHandler(db);

        var command = new UpdateFtpServerCommand
        {
            FtpServerId = Guid.NewGuid(),
            FtpServerName = "Name",
            FtpDescription = "desc",
            FtpServerIp = "127.0.0.1",
            FtpTransferMode = "I",
            FtpUser = "user",
            FtpPassword = "pw",
            FtpPort = 21,
            IsValid = false,
            Requestor = new RequestorModel { IdentifierId = "operator-2" },
            OperatorId = "operator-2",
        };

        var ev = await handler.HandleAsync(command, CancellationToken.None);
        Assert.Equal("operator-2", ev.OperatorId);
        Assert.False(ev.IsValid);
    }

    [Fact]
    public async Task F01_034_invalidate_does_not_validate_and_always_forces_is_valid_false()
    {
        var db = CreateDb();
        var handler = new FtpServerCommandHandler(db);

        var command = new InvalidateFtpServerCommand
        {
            FtpServerId = Guid.NewGuid(),
            Requestor = new RequestorModel { IdentifierId = "operator" },
            OperatorId = "operator",
            FtpServerName = null,
        };

        var ev = await handler.HandleAsync(command, CancellationToken.None);
        Assert.False(ev.IsValid);
        Assert.Equal("operator", ev.OperatorId);
    }
}

