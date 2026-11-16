using FxferService.Domain.Commands;
using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Events;
using FxferService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Application.CommandHandlers;

public sealed class FtpServerCommandHandler
{
    private readonly FxferDbContext _dbContext;

    public FtpServerCommandHandler(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FtpServerAdded> HandleAsync(AddFtpServerCommand command, CancellationToken ct)
    {
        // F01-021: validate before returning a FtpServerAdded event.
        await ValidateFtpServerCommand(command, ct);

        var operatorId = GetOperatorIdOrThrow(command);
        var now = DateTime.UtcNow;

        _dbContext.FtpServers.Add(new FxferService.Infrastructure.Persistence.Entities.FtpServer
        {
            FtpServerId = command.FtpServerId,
            FtpServerName = command.FtpServerName!,
            FtpDescription = command.FtpDescription!,
            FtpServerIp = command.FtpServerIp!,
            FtpTransferMode = command.FtpTransferMode!,
            FtpUser = command.FtpUser!,
            FtpPassword = command.FtpPassword!,
            FtpPort = command.FtpPort,
            FtpProxyAddress = command.FtpProxyAddress,
            FtpProxyPort = command.FtpProxyPort,
            UseSSHAuth = command.UseSSHAuth,
            IsValid = true,
            OperatorId = operatorId,
            CreationDate = now,
            LastModifiedDate = now,
        });

        await _dbContext.SaveChangesAsync(ct);
        return BuildEvent(command);
    }

    public async Task<FtpServerUpdated> HandleAsync(UpdateFtpServerCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.FtpPassword))
        {
            var existingPassword = await _dbContext.FtpServers
                .AsNoTracking()
                .Where(x => x.FtpServerId == command.FtpServerId)
                .Select(x => x.FtpPassword)
                .FirstOrDefaultAsync(ct);

            if (!string.IsNullOrWhiteSpace(existingPassword))
            {
                command.FtpPassword = existingPassword;
            }
        }

        // F01-022: validate before returning a FtpServerUpdated event.
        await ValidateFtpServerCommand(command, ct);

        var operatorId = GetOperatorIdOrThrow(command);
        var now = DateTime.UtcNow;

        var entity = await _dbContext.FtpServers
            .Where(x => x.FtpServerId == command.FtpServerId)
            .FirstOrDefaultAsync(ct);

        if (entity is null)
        {
            entity = new FxferService.Infrastructure.Persistence.Entities.FtpServer
            {
                FtpServerId = command.FtpServerId,
                CreationDate = now,
            };
            _dbContext.FtpServers.Add(entity);
        }

        entity.FtpServerName = command.FtpServerName!;
        entity.FtpDescription = command.FtpDescription!;
        entity.FtpServerIp = command.FtpServerIp!;
        entity.FtpTransferMode = command.FtpTransferMode!;
        entity.FtpUser = command.FtpUser!;
        entity.FtpPassword = command.FtpPassword!;
        entity.FtpPort = command.FtpPort;
        entity.FtpProxyAddress = command.FtpProxyAddress;
        entity.FtpProxyPort = command.FtpProxyPort;
        entity.UseSSHAuth = command.UseSSHAuth;
        entity.IsValid = command.IsValid;
        entity.OperatorId = operatorId;
        entity.LastModifiedDate = now;

        await _dbContext.SaveChangesAsync(ct);
        return BuildEvent(command);
    }

    public Task<FtpServerInvalidated> HandleAsync(InvalidateFtpServerCommand command, CancellationToken ct)
    {
        // F01-034: do not call command.Validate(); always force IsValid=false.
        return InvalidateAndBuildEventAsync(command, ct);
    }

    private async Task<FtpServerInvalidated> InvalidateAndBuildEventAsync(InvalidateFtpServerCommand command, CancellationToken ct)
    {
        var entity = await _dbContext.FtpServers
            .Where(x => x.FtpServerId == command.FtpServerId)
            .FirstOrDefaultAsync(ct);

        if (entity is not null)
        {
            entity.IsValid = false;
            entity.OperatorId = GetOperatorIdOrThrow(command);
            entity.LastModifiedDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(ct);
        }

        return BuildEvent(command);
    }

    private async Task ValidateFtpServerCommand(AddFtpServerCommand command, CancellationToken ct)
    {
        command.Validate();

        // F01-029: reject a duplicate FTP server name when another valid server exists with a different id.
        if (await IsFtpServerNameDuplicatedAsync(command.FtpServerName!, command.FtpServerId, ct))
        {
            command.AddValidation(
                command.FtpServerName!,
                CommonConstants.Validation.ErrorType.DuplicatedFtpServerName,
                CommonConstants.Validation.Messages.DuplicatedFtpServerName);
            command.HandleValidations();
        }
    }

    private Task<bool> IsFtpServerNameDuplicatedAsync(string ftpServerName, Guid ftpServerId, CancellationToken ct)
    {
        var normalizedName = ftpServerName.ToUpperInvariant().Trim();

        return _dbContext.FtpServers
            .AsNoTracking()
            .Where(x => x.IsValid)
            .Where(x => x.FtpServerId != ftpServerId)
            .AnyAsync(x => x.FtpServerName.ToUpperInvariant().Trim() == normalizedName, ct);
    }

    private static string GetOperatorIdOrThrow(FileTransferCommand command)
    {
        var operatorId = command.Requestor?.IdentifierId;
        if (string.IsNullOrWhiteSpace(operatorId))
        {
            throw new InvalidOperationException("Missing requestor operator id");
        }

        return operatorId;
    }

    private static FtpServerAdded BuildEvent(AddFtpServerCommand command)
    {
        // F01-032: map fields, force IsValid=true, and assign OperatorId.
        return new FtpServerAdded()
        {
            FtpServerId = command.FtpServerId,
            FtpServerName = command.FtpServerName,
            FtpDescription = command.FtpDescription,
            FtpServerIp = command.FtpServerIp,
            FtpTransferMode = command.FtpTransferMode,
            FtpUser = command.FtpUser,
            FtpPassword = command.FtpPassword,
            FtpPort = command.FtpPort,
            FtpProxyAddress = command.FtpProxyAddress,
            FtpProxyPort = command.FtpProxyPort,
            UseSSHAuth = command.UseSSHAuth,
            IsValid = true,
            OperatorId = command.Requestor!.IdentifierId,
        };
    }

    private static FtpServerUpdated BuildEvent(UpdateFtpServerCommand command)
    {
        // F01-033: map fields and assign OperatorId.
        return new FtpServerUpdated()
        {
            FtpServerId = command.FtpServerId,
            FtpServerName = command.FtpServerName,
            FtpDescription = command.FtpDescription,
            FtpServerIp = command.FtpServerIp,
            FtpTransferMode = command.FtpTransferMode,
            FtpUser = command.FtpUser,
            FtpPassword = command.FtpPassword,
            FtpPort = command.FtpPort,
            FtpProxyAddress = command.FtpProxyAddress,
            FtpProxyPort = command.FtpProxyPort,
            UseSSHAuth = command.UseSSHAuth,
            IsValid = command.IsValid,
            OperatorId = command.Requestor!.IdentifierId,
        };
    }

    private static FtpServerInvalidated BuildEvent(InvalidateFtpServerCommand command)
    {
        return new FtpServerInvalidated()
        {
            FtpServerId = command.FtpServerId,
            IsValid = false,
            OperatorId = command.Requestor!.IdentifierId,
        };
    }
}
