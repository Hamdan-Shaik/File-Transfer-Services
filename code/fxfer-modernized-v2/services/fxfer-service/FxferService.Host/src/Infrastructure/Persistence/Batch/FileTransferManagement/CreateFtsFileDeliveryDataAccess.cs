using FxferService.Application.Persistence.Batch;
using FxferService.Domain.Constants;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;

namespace FxferService.Infrastructure.Persistence.Batch;

public class CreateFtsFileDeliveryDataAccess : DataAccessBase<Guid>
{
    private readonly Guid _fileId;
    private readonly Guid _destinationId;
    private readonly string _destinationFileName;
    private readonly string _operatorId;
    private Guid _deliveryResult;
    private readonly List<DestinationModel> _destinations;

    public CreateFtsFileDeliveryDataAccess(
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId,
        List<DestinationModel> destinations)
    {
        _fileId = fileId;
        _destinationId = destinationId;
        _destinationFileName = destinationFileName;
        _operatorId = operatorId;
        _destinations = destinations;
    }

    protected override Guid ExecuteQuery(IDbContextBase dbcontext)
    {
        FtsFileDeliveryEntity ftsDelivery = new()
        {
            DeliveryId = Guid.NewGuid(),
            FileId = _fileId,
            DestinationId = _destinationId,
            DestinationFileName = _destinationFileName,
            DeliveryAt = DateTimeOffset.UtcNow,
            OperatorId = _operatorId,
            CreatedAt = DateTimeOffset.UtcNow,
            LastModifiedAt = DateTimeOffset.UtcNow,
        };

        dbcontext.Set<FtsFileDeliveryEntity>().Add(ftsDelivery);
        _deliveryResult = AddFileArchievedOrTransferredEvent(dbcontext);
        dbcontext.SaveChanges();
        return _deliveryResult;
    }

    private Guid AddFileArchievedOrTransferredEvent(IDbContextBase dbcontext)
    {
        var destinationDetails = _destinations.FindAll(x => x.DestinationId == _destinationId);

        FtsFileEventEntity ftsEvent = new();

        if (destinationDetails != null && destinationDetails.Any())
        {
            foreach (var destfileName in destinationDetails)
            {
                if (destfileName.IsRenameAtDestination)
                {
                    FtsFileEventEntity ftsEventRename = new();
                    UpdateFileEventData(ref dbcontext, ref ftsEventRename, FileDeliveryEventConstants.Renamed);
                }

                if (destfileName.IsArchive == true)
                {
                    return UpdateFileEventData(ref dbcontext, ref ftsEvent, FileDeliveryEventConstants.Archieved);
                }
                else
                {
                    return UpdateFileEventData(ref dbcontext, ref ftsEvent, FileDeliveryEventConstants.Transferred);
                }
            }
        }

        return _deliveryResult;
    }

    private Guid UpdateFileEventData(ref IDbContextBase dbcontext, ref FtsFileEventEntity ftsEvent, string eventCode)
    {
        ftsEvent.EventId = Guid.NewGuid();
        ftsEvent.FileId = _fileId;
        ftsEvent.EventCode = eventCode;
        ftsEvent.DestinationId = _destinationId;
        ftsEvent.OperatorId = _operatorId;
        ftsEvent.TrackDescription = _destinationFileName;
        ftsEvent.CreatedAt = DateTimeOffset.UtcNow;
        ftsEvent.LastModifiedAt = DateTimeOffset.UtcNow;
        dbcontext.Set<FtsFileEventEntity>().Add(ftsEvent);
        return ftsEvent.EventId;
    }
}