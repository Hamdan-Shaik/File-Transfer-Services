using FxferService.Application.CommandHandlers;
using RequestorModel = FxferService.Application.LegacyContracts.RequestorModel;
using FxferService.Domain.Commands;
using FxferService.Domain.Contracts.Events;
using FxferService.Domain.Contracts.Exceptions;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Tests;

public sealed class FtsSourceLocationAddCommandHandlerRuleTests
{
    private static FxferDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FxferDbContext(options);
    }

    private static FtsSourceLocationAddCommand CreateValidAddCommand()
    {
        return new FtsSourceLocationAddCommand
        {
            ConfigName = "cfg",
            Protocol = "0",
            Address = "addr",
            OwnerName = "owner",
            OwnerModule = "module",
            OwnerPhone = "phone",
            OwnerEmail = "email",
            StartTime = "0000",
            EndTime = "2359",
            RunOnMonday = true,
            Mask = "file.txt",
            Requestor = new RequestorModel { IdentifierId = "operator" },
            OperatorId = "operator",
            DestinationAddress = new List<FtsDestinationLocationAdded> { new() { IsValid = true } },
        };
    }

    private static FtsSourceLocationUpdateCommand CreateValidUpdateCommand(int configId, string commandSourceLocationId)
    {
        return new FtsSourceLocationUpdateCommand
        {
            FtsSourceLocationId = commandSourceLocationId,
            ConfigId = configId,
            ConfigName = "cfg",
            Protocol = "0",
            Address = "addr",
            OwnerName = "owner",
            OwnerModule = "module",
            OwnerPhone = "phone",
            OwnerEmail = "email",
            StartTime = "0000",
            EndTime = "2359",
            RunOnMonday = true,
            Mask = "file.txt",
            OperatorId = "operator",
            DestinationAddress = new List<FtsDestinationLocationAdded> { new() { IsValid = true } },
        };
    }

    private static FtsSourceLocationInvalidateCommand CreateValidInvalidateCommand(int configId, string commandSourceLocationId)
    {
        return new FtsSourceLocationInvalidateCommand
        {
            FtsSourceLocationId = commandSourceLocationId,
            ConfigId = configId,
            OperatorId = "operator",
            DestinationAddress = new List<FtsDestinationLocationAdded> { new() { IsValid = true } },
        };
    }

    [Fact]
    public async Task F02_035_add_validates_and_assigns_new_config_id_from_sequence()
    {
        var db = CreateDb();
        var handler = new FtsSourceLocationAddCommandHandler(db);

        var command = CreateValidAddCommand();
        var ev = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(1, ev.ConfigId);
        Assert.Equal("operator", ev.OperatorId);
        Assert.Same(command.DestinationAddress, ev.DestinationAddress);
    }

    [Fact]
    public async Task F02_053_update_requires_an_existing_source_location_by_config_id()
    {
        var db = CreateDb();
        var handler = new FtsSourceLocationAddCommandHandler(db);

        var command = CreateValidUpdateCommand(123, commandSourceLocationId: "ignored");

        var exception = await Assert.ThrowsAsync<BusinessException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(FileTransferExceptionConstants.Validation.Message, exception.ErrorType);
        Assert.Equal(FileTransferExceptionConstants.Validation.ProcessingSystem, exception.ProcessingSystem);

        var message = Assert.Single(exception.BusinessMessages);
        Assert.Equal(FileTransferExceptionConstants.Validation.ConfigId, message.FieldName);
        Assert.Equal(FileTransferExceptionConstants.Validation.ConfigIdNotFound, message.MessageKey);
        Assert.Equal(FileTransferExceptionConstants.Validation.ProcessingSystem, message.MessageDefault);
    }

    [Fact]
    public async Task F02_054_update_identifies_record_by_config_id_and_ignores_command_source_location_id()
    {
        var db = CreateDb();
        var existingId = Guid.NewGuid();
        db.SourceLocations.Add(new FtsSourceLocationEntity { FtsSourceLocationId = existingId, ConfigId = 123, ConfigName = "cfg" });
        db.SaveChanges();

        var handler = new FtsSourceLocationAddCommandHandler(db);
        var command = CreateValidUpdateCommand(123, commandSourceLocationId: "different");

        var ev = await handler.HandleAsync(command, CancellationToken.None);
        Assert.Equal(existingId.ToString(), ev.FtsSourceLocationsId);
    }

    [Fact]
    public async Task F02_055_invalidate_requires_an_existing_source_location_by_config_id()
    {
        var db = CreateDb();
        var handler = new FtsSourceLocationAddCommandHandler(db);

        var command = CreateValidInvalidateCommand(123, commandSourceLocationId: "ignored");

        var exception = await Assert.ThrowsAsync<BusinessException>(() => handler.HandleAsync(command, CancellationToken.None));
        Assert.Equal(FileTransferExceptionConstants.Validation.Message, exception.ErrorType);
        Assert.Equal(FileTransferExceptionConstants.Validation.ProcessingSystem, exception.ProcessingSystem);

        var message = Assert.Single(exception.BusinessMessages);
        Assert.Equal(FileTransferExceptionConstants.Validation.ConfigId, message.FieldName);
        Assert.Equal(FileTransferExceptionConstants.Validation.ConfigIdNotFound, message.MessageKey);
    }

    [Fact]
    public async Task F02_056_invalidate_identifies_record_by_config_id_and_ignores_command_source_location_id()
    {
        var db = CreateDb();
        var existingId = Guid.NewGuid();
        db.SourceLocations.Add(new FtsSourceLocationEntity { FtsSourceLocationId = existingId, ConfigId = 123, ConfigName = "cfg" });
        db.SaveChanges();

        var handler = new FtsSourceLocationAddCommandHandler(db);
        var command = CreateValidInvalidateCommand(123, commandSourceLocationId: "different");

        var ev = await handler.HandleAsync(command, CancellationToken.None);
        Assert.Equal(existingId.ToString(), ev.FtsSourceLocationsId);
    }
}
