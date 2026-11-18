using System.Runtime.Serialization;
using System.Xml;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Contracts.Eventing;
using Fxfer.TransferWorker.Domain.Constants;
using Fxfer.TransferWorker.Infrastructure.Eventing;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fxfer.TransferWorker.Application.Helpers;

public class EventHelper
{
    private readonly IDistributedEventStorage? _eventStorage;
    private readonly Microsoft.Extensions.Logging.ILogger<EventHelper> _logger;

    public EventHelper() : this(InitializeDistributedEventStorage())
    {
    }

    public EventHelper(IDistributedEventStorage? distributedEventStorage)
    {
        _eventStorage = distributedEventStorage;
        _logger = NullLogger<EventHelper>.Instance;
    }

    public virtual void PublishFileTransferredEvent(FileToDeliverModel fileDeliveryData, string tenantId, TimeSpan elapsedTime)
    {
        if (_eventStorage != null && IsDeliveryUsedForSystemIntegration(fileDeliveryData))
        {
            try
            {
                FileTransferred @event = BuildFileTransferredEvent(fileDeliveryData, elapsedTime);
                EventMessage eventMsg = SetUpEventMessage(@event, tenantId);
                _logger.LogInformation(
                    string.Format(
                        FileTransferManagementConstants.EventDistribution.AttemptingToDistributeEvent,
                        @event.GetType().Name,
                        @event.CommitId));

                _eventStorage.Add(eventMsg);

                _logger.LogInformation(
                    string.Format(
                        FileTransferManagementConstants.EventDistribution.DistributionOfEventCompleted,
                        @event.GetType().Name,
                        @event.CommitId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }

    private static IDistributedEventStorage? InitializeDistributedEventStorage()
    {
        DistributedEventStorageFactory factory = new DistributedEventStorageFactory();
        return factory.Create(FileTransferManagementBatchJobConstants.EventStore);
    }

    private EventMessage SetUpEventMessage(FileTransferred evt, string tenantId)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            DataContractSerializer serializer = new DataContractSerializer(evt.GetType(), GetContractTypes());

            serializer.WriteObject(ms, evt);

            return BuildEventMessage(evt, ms, evt.FileId.ToString(), tenantId);
        }
    }

    private EventMessage BuildEventMessage(FileTransferred convertedEvent, MemoryStream memoryStream, string aggregateRootValue, string tenantId)
    {
        return new EventMessage()
        {
            EventNamespace = convertedEvent.GetType().Namespace,
            AggregateRootID = aggregateRootValue,
            AggregateRootType = FileTransferManagementBatchJobConstants.IntegrationAggregate,
            CommitID = convertedEvent.CommitId.ToString(),
            EventType = convertedEvent.GetType().Name,
            ModuleName = FileTransferManagementBatchJobConstants.EventPublisherModule,
            EventPayLoad = memoryStream.ToArray(),
            TenantID = tenantId,
            INGroupId = convertedEvent.GroupId,
            EventFilterMetadata = SerializeToXMLEventFilterMetadata(convertedEvent.EventFilterMetadata),
        };
    }

    private FileTransferred BuildFileTransferredEvent(FileToDeliverModel fileDeliveryData, TimeSpan elapsedTime)
    {
        DestinationModel fteDestination = GetDestination(fileDeliveryData);
        return new FileTransferred()
        {
            ApplicationName = FileTransferManagementBatchJobConstants.OperatorId,
            DestinationAddressPath = fteDestination.DestinationAddress,
            FileId = fileDeliveryData.FileId,
            FileName = Path.GetFileName(fileDeliveryData.FileName),
            FTEMetadataId = fteDestination.FTEMetadataId!.Value,
            GroupId = fileDeliveryData.GroupId,
            ModuleContract = fteDestination.ContractName,
            OwnerModule = fileDeliveryData.SourceLocation.OwnerName,
            SourceAddressPath = fileDeliveryData.SourceLocation.SourceAddress,
            SourceLocationId = fileDeliveryData.SourceLocation.SourceLocationId,
            SourceMask = fileDeliveryData.SourceLocation.FileMask,
            SourceProtocolCode = fileDeliveryData.SourceLocation.SourceProtocolCode,
            Timestamp = DateTime.UtcNow,
            TransferDate = DateTime.UtcNow,
            ConfigurationId = fileDeliveryData.SourceLocation.ConfigurationId,
            ConfigurationName = fileDeliveryData.SourceLocation.ConfigurationName,
            ConfigurationDescription = fileDeliveryData.SourceLocation.ConfigurationDescription,
            FileDirection = fileDeliveryData.SourceLocation.FileDirection,
            FileType = fileDeliveryData.FileType,
            FileSize = fileDeliveryData.FileSize,
            StartTime = DateTime.UtcNow.AddMilliseconds(-elapsedTime.Milliseconds),
            CommitId = Guid.NewGuid(),
            EventFilterMetadata = new Dictionary<string, string>()
            {
                { FileTransferManagementBatchJobConstants.EventFilters.UsedByFTE, fteDestination.FTEMetadataId.HasValue.ToString() },
            },
        };
    }

    private IEnumerable<Type> GetContractTypes()
    {
        using (DataContractRegistry registry = new DataContractRegistry())
        {
            return registry.GetContractTypes();
        }
    }

    private DestinationModel GetDestination(FileToDeliverModel fileDeliveryData)
    {
        return fileDeliveryData.SourceLocation.Destinations.First(dest => dest.FTEMetadataId.HasValue && dest.FTEMetadataId != Guid.Empty);
    }

    private string? SerializeToXMLEventFilterMetadata(Dictionary<string, string> eventFilters)
    {
        if (eventFilters.ContainsKey(FileTransferManagementBatchJobConstants.EventFilters.UsedByFTE))
        {
            XmlDocument xmlDoc = BuildEventMessageXML(BuildEventMessageFilters(eventFilters));
            return xmlDoc.InnerXml.ToString();
        }

        return null;
    }

    private Dictionary<string, string> BuildEventMessageFilters(Dictionary<string, string> eventFilters)
    {
        return new Dictionary<string, string>()
        {
            { FileTransferManagementBatchJobConstants.EventFilters.EventMessageNodes.Key, FileTransferManagementBatchJobConstants.EventFilters.UsedByFTE },
            { FileTransferManagementBatchJobConstants.EventFilters.EventMessageNodes.Value, eventFilters[FileTransferManagementBatchJobConstants.EventFilters.UsedByFTE] },
        };
    }

    private XmlDocument BuildEventMessageXML(Dictionary<string, string> eventMessageFilters)
    {
        XmlDocument xmlDoc = new XmlDocument();
        EventMessage eventMessage = new EventMessage();
        XmlElement rootElement = xmlDoc.CreateElement(nameof(eventMessage.EventFilterMetadata));
        XmlElement itemElement = xmlDoc.CreateElement(FileTransferManagementBatchJobConstants.EventFilters.EventMessageNodes.Item);

        foreach (KeyValuePair<string, string> kvp in eventMessageFilters)
        {
            XmlElement keyElement = xmlDoc.CreateElement(kvp.Key);
            keyElement.InnerText = kvp.Value;
            itemElement.AppendChild(keyElement);
        }

        rootElement.AppendChild(itemElement);
        xmlDoc.AppendChild(rootElement);

        return xmlDoc;
    }

    private bool IsDeliveryUsedForSystemIntegration(FileToDeliverModel fileDeliveryData)
    {
        return fileDeliveryData.SourceLocation.Destinations.Any(dest => dest.FTEMetadataId.HasValue && dest.FTEMetadataId != Guid.Empty);
    }
}
