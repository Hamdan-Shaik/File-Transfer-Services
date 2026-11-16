using FxferService.Application.CommandHandlers;
using RequestorModel = FxferService.Application.LegacyContracts.RequestorModel;
using FxferService.Domain.Commands;
using FxferService.Domain.Contracts.Exceptions;
using Xunit;

namespace FxferService.Tests;

public sealed class FtsIntegrationFileCommandHandlerRuleTests
{
    [Fact]
    public async Task F02_068_update_validates_then_builds_integration_file_updated_event()
    {
        var handler = new FtsIntegrationFileCommandHandler();

        var invalidCommand = new UpdateIntegrationFileCommand
        {
            ConfigId = null,
        };

        await Assert.ThrowsAsync<BusinessException>(() => handler.HandleAsync(invalidCommand, CancellationToken.None));

        var command = new UpdateIntegrationFileCommand
        {
            FtsIntegrationFileId = Guid.NewGuid(),
            FileName = "file",
            ConfigId = 123,
            IsValid = false,
            Requestor = new RequestorModel { IdentifierId = "operator" },
            OperatorId = "operator",
        };

        var ev = await handler.HandleAsync(command, CancellationToken.None);
        Assert.Equal(command.FtsIntegrationFileId, ev.FtsIntegrationFileId);
        Assert.Equal(command.FileName, ev.FileName);
        Assert.Equal(command.ConfigId, ev.ConfigId);
        Assert.Equal(command.IsValid, ev.IsValid);
        Assert.Equal("operator", ev.OperatorId);
    }
}
