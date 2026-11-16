using FxferService.Application.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class GetConfigMetaDataDataAccess : DataAccessBase<SourceLocationModel?>
{
    private readonly Guid _fileId;
    private Guid _soureceLocationId;

    public GetConfigMetaDataDataAccess(Guid fileId)
    {
        _fileId = fileId;
    }

    protected override SourceLocationModel? ExecuteQuery(IDbContextBase dbcontext)
    {
        SetSoureceLocationId(dbcontext);

        var sourceLocation = dbcontext.Set<FtsSourceLocationEntity>().AsNoTracking().FirstOrDefault(x => x.FtsSourceLocationId == _soureceLocationId);
        SourceLocationModel? source = null;
        if (sourceLocation != null)
        {
            var ftpServer = new FtpServer();
            if (sourceLocation.FtpServerId != Guid.Empty && sourceLocation.FtpServerId != null)
            {
                var loaded = dbcontext.Set<FtpServer>().AsNoTracking().FirstOrDefault(x => x.FtpServerId == sourceLocation.FtpServerId);
                if (loaded != null)
                {
                    ftpServer = loaded;
                }
            }

            source = new SourceLocationModel
            {
                SourceLocationId = sourceLocation.FtsSourceLocationId,
                ConfigurationId = sourceLocation.ConfigId,
                SourceProtocolCode = sourceLocation.SourceProtocolCode,
                SourceAddress = sourceLocation.SourceAddress,
                OwnerEmail = sourceLocation.OwnerEmail,
                OwnerName = sourceLocation.OwnerName,
                SendNotificationsIndicator = sourceLocation.SendNotifications,
                FtpServerDetail = new FxferService.Domain.Contracts.Domain.FtpServerModel
                {
                    FtpServerId = ftpServer.FtpServerId,
                    FtpServerName = ftpServer.FtpServerName,
                    FtpServerIp = ftpServer.FtpServerIp,
                    FtpTransferMode = ftpServer.FtpTransferMode,
                    FtpUser = ftpServer.FtpUser,
                    FtpPort = ftpServer.FtpPort,
                    FtpPassword = ftpServer.FtpPassword,
                    UseSSHAuth = ftpServer.UseSSHAuth,
                },
            };
            var destinations = GetDestinations(dbcontext);
            source.Destinations.AddRange(destinations);
        }

        return source;
    }

    private void SetSoureceLocationId(IDbContextBase dbcontext)
    {
        var fileManagment = dbcontext.Set<FtsFileManagementEntity>().SingleOrDefault(f => f.FileId == _fileId);
        if (fileManagment != null)
        {
            _soureceLocationId = fileManagment.FtsSourceLocationId;
        }
    }

    private List<DestinationModel> GetDestinations(IDbContextBase dbcontext)
    {
        var deliveredDestinationIds = dbcontext.Set<FtsFileDeliveryEntity>().AsNoTracking()
            .Where(x => x.FileId == _fileId)
            .Select(s => s.DestinationId)
            .ToList();

        var result = dbcontext.Set<FtsDestinationLocations>().AsNoTracking()
            .Where(x => x.FtsSourceLocationId == _soureceLocationId && x.IsValid && (!deliveredDestinationIds.Contains(x.DestinationId) || x.DestIsArchive))
            .ToList();

        var destinations = new List<DestinationModel>();
        foreach (var item in result)
        {
            var destination = new DestinationModel
            {
                DestinationId = item.DestinationId,
                DestinationAddress = item.DestinationAddress,
                DestinationProtocolCode = item.DestProtocolCD,
                IsOverwriteAtDestination = item.OverwriteAtDest,
                IsCompressAtDestination = item.CompressAtDest,
                IsRenameAtDestination = item.ReNameAtDest,
                ContactName = item.ContactName,
                EmailId = item.ContactEmail,
                IsArchive = item.DestIsArchive,
                RenamePattern = item.RenamePattern,
                EncryptionCode = item.CdeEncryption,
                EncryptionPublicKey = item.EncryptionPublicKey,
                EncryptionPrivateKey = item.EncryptionPrivateKey,
                PrivateKeyPassword = item.PrivateKeyPassword,
                FTEMetadataId = item.FTEMetadataId,
                ContractName = item.ContractName,
            };

            if (item.FtpServerId != Guid.Empty)
            {
                destination.FtpServerDetail = dbcontext.Set<FtpServer>().AsNoTracking().Where(x => x.FtpServerId == item.FtpServerId)
                    .Select(f => new FxferService.Domain.Contracts.Domain.FtpServerModel
                    {
                        FtpServerId = f.FtpServerId,
                        FtpServerName = f.FtpServerName,
                        FtpServerIp = f.FtpServerIp,
                        FtpTransferMode = f.FtpTransferMode,
                        FtpUser = f.FtpUser,
                        FtpPort = f.FtpPort,
                        FtpPassword = f.FtpPassword,
                        UseSSHAuth = f.UseSSHAuth,
                    }).FirstOrDefault();
            }

            destinations.Add(destination);
        }

        return destinations;
    }
}
