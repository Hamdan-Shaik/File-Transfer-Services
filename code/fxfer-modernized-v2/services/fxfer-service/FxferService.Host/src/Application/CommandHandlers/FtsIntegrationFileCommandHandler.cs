using FxferService.Domain.Commands;
using FxferService.Domain.Contracts.Events;

namespace FxferService.Application.CommandHandlers;

using LegacyAddIntegrationFileCommand = FxferService.Application.LegacyContracts.AddIntegrationFileCommand;

public sealed class FtsIntegrationFileCommandHandler
{
    public Task<IntegrationFileAdded?> HandleAsync(LegacyAddIntegrationFileCommand command, CancellationToken ct)
    {
        _ = ct;

        var isValidGroupInfo = command.GroupInfo != null && command.GroupInfo.Any(e => e.NumberOfRecords > 0 && e.GroupId != null);
        return Task.FromResult(isValidGroupInfo ? BuildAddEvent(command) : null);
    }

    public Task<IntegrationFileUpdated> HandleAsync(UpdateIntegrationFileCommand command, CancellationToken ct)
    {
        _ = ct;

        // F02-068: validate then build the update event.
        command.Validate();
        return Task.FromResult(BuildUpdateEvent(command));
    }

    private static IntegrationFileAdded BuildAddEvent(LegacyAddIntegrationFileCommand command)
    {
        return new IntegrationFileAdded()
        {
            GroupInfo = command.GroupInfo,
            FileName = command.FileName,
            ETFOutboundMetadataId = command.ETFOutboundMetadataDefnId,
            IsProcessed = false,
            IsValid = true,
            OperatorId = command.Requestor!.IdentifierId,
        };
    }

    private static IntegrationFileUpdated BuildUpdateEvent(UpdateIntegrationFileCommand command)
    {
        return new IntegrationFileUpdated()
        {
            FtsIntegrationFileId = command.FtsIntegrationFileId,
            FileName = command.FileName,
            ConfigId = command.ConfigId,
            IsValid = command.IsValid,
            OperatorId = command.Requestor!.IdentifierId,
        };
    }
}
