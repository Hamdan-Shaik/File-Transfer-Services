using FxferService.Projections.Infrastructure.Messaging.Contracts.Events;
using FxferService.Projections.Infrastructure.Persistence;
using FxferService.Projections.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Projections.Projections.Writers;

public sealed class FtpServerProjectionWriter
{
    private readonly FxferProjectionsDbContext _dbContext;
    private readonly TimeProvider _timeProvider;

    public FtpServerProjectionWriter(FxferProjectionsDbContext dbContext, TimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _timeProvider = timeProvider;
    }

    public async Task HandleAsync(FtpServerAdded @event, CancellationToken ct)
    {
        // F01-035: insert FTP_SERVER row and stamp UTC timestamps.
        await AddFtpServerAsync(@event, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(FtpServerUpdated @event, CancellationToken ct)
    {
        // MaintainabilityDetails(ApprovedIndex = 58, ReviewedOnSprint = 96, ReviewerName = Hector Murga)
        // F01-036: update FTP_SERVER fields + UTC last-modified + operator.
        var entity = await _dbContext.FtpServers.SingleAsync(x => x.FtpServerId == @event.FtpServerId, ct);
        entity.FtpDescription = @event.FtpDescription;
        entity.FtpServerIp = @event.FtpServerIp;
        entity.FtpServerName = @event.FtpServerName;
        entity.FtpPassword = @event.FtpPassword;
        entity.FtpPort = @event.FtpPort;
        entity.FtpProxyAddress = @event.FtpProxyAddress;
        entity.FtpProxyPort = @event.FtpProxyPort;
        entity.FtpTransferMode = @event.FtpTransferMode;
        entity.FtpUser = @event.FtpUser;
        entity.UseSshAuth = @event.UseSSHAuth;
        entity.LastModifiedTs = _timeProvider.GetUtcNow();
        entity.OperatorId = @event.OperatorId;
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(FtpServerInvalidated @event, CancellationToken ct)
    {
        // F01-037: update FTP_SERVER validity + UTC last-modified + operator.
        var entity = await _dbContext.FtpServers.SingleAsync(x => x.FtpServerId == @event.FtpServerId, ct);
        entity.IsValid = @event.IsValid;
        entity.OperatorId = @event.OperatorId;
        entity.LastModifiedTs = _timeProvider.GetUtcNow();
        await _dbContext.SaveChangesAsync(ct);
    }

    private Task AddFtpServerAsync(FtpServerAdded @event, CancellationToken ct)
    {
        var utcNow = _timeProvider.GetUtcNow();

        var newFtpServer = new FtpServerProjection
        {
            FtpServerId = @event.FtpServerId,
            FtpDescription = @event.FtpDescription,
            FtpServerIp = @event.FtpServerIp,
            FtpServerName = @event.FtpServerName,
            FtpPassword = @event.FtpPassword,
            FtpPort = @event.FtpPort,
            FtpProxyAddress = @event.FtpProxyAddress,
            FtpProxyPort = @event.FtpProxyPort,
            FtpTransferMode = @event.FtpTransferMode,
            FtpUser = @event.FtpUser,
            UseSshAuth = @event.UseSSHAuth,
            IsValid = @event.IsValid,
            OperatorId = @event.OperatorId,
            CreatedTs = utcNow,
            LastModifiedTs = utcNow,
        };

        _dbContext.FtpServers.Add(newFtpServer);
        return Task.CompletedTask;
    }
}

