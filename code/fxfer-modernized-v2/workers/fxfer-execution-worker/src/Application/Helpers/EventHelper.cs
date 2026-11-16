using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;

namespace Fxfer.ExecutionWorker.Application.Helpers;

public class EventHelper
{
    private readonly IDistributedEventStorage eventStorage;

    public EventHelper() : this(InitializeDistributedEventStorage())
    {
    }

    public EventHelper(IDistributedEventStorage distributedEventStorage)
    {
        this.eventStorage = distributedEventStorage;
    }

    public virtual void PublishFileTransferredEvent(FileToDeliverModel fileDeliveryData, string tenantId, TimeSpan elapsedTime)
    {
        if (this.eventStorage != null && this.IsDeliveryUsedForSystemIntegration(fileDeliveryData))
        {
            try
            {
                FileTransferred @event = this.BuildFileTransferredEvent(fileDeliveryData, elapsedTime);
                EventMessage eventMsg = this.SetUpEventMessage(@event, tenantId);
                LoggerManager.Logger.LogInformational(string.Format(FileTransferManagementConstants.EventDistribution.AttemptingToDistributeEvent, @event.GetType().Name, @event.CommitId));
                this.eventStorage.Add(eventMsg);
                LoggerManager.Logger.LogInformational(string.Format(FileTransferManagementConstants.EventDistribution.DistributionOfEventCompleted, @event.GetType().Name, @event.CommitId));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.LogError(ex.Message);
            }
        }
    }

    [ExcludeFromCodeCoverage]
    private static IDistributedEventStorage InitializeDistributedEventStorage()
    {
        DistributedEventStorageFactory factory = new DistributedEventStorageFactory();
        return factory.Create(EventHelperConstants.EventStore);
    }

    private EventMessage SetUpEventMessage(FileTransferred evt, string tenantId)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            DataContractSerializer serializer = new DataContractSerializer(evt.GetType(), this.GetContractTypes());

            serializer.WriteObject(ms, evt);

            return this.BuildEventMessage(evt, ms, evt.FileId.ToString(), tenantId);
        }
    }

    private EventMessage BuildEventMessage(FileTransferred convertedEvent, MemoryStream memoryStream, string aggregateRootValue, string tenantId)
    {
        return new EventMessage()
        {
            EventNamespace = convertedEvent.GetType().Namespace,
            AggregateRootID = aggregateRootValue,
            AggregateRootType = EventHelperConstants.IntegrationAggregate,
            CommitID = convertedEvent.CommitId.ToString(),
            EventType = convertedEvent.GetType().Name,
            ModuleName = EventHelperConstants.EventPublisherModule,
            EventPayLoad = memoryStream.ToArray(),
            TenantID = tenantId,
            INGroupId = convertedEvent.GroupId,
            EventFilterMetadata = this.SerializeToXMLEventFilterMetadata(convertedEvent.EventFilterMetadata)
        };
    }

    private FileTransferred BuildFileTransferredEvent(FileToDeliverModel fileDeliveryData, TimeSpan elapsedTime)
    {
        DestinationModel fteDestination = this.GetDestination(fileDeliveryData);
        return new FileTransferred()
        {
            ApplicationName = EventHelperConstants.OperatorId,
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
                { EventHelperConstants.EventFilters.UsedByFTE, fteDestination.FTEMetadataId.HasValue.ToString() }
            }
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
        return fileDeliveryData.SourceLocation.Destinations
            .First(dest => dest.FTEMetadataId.HasValue && dest.FTEMetadataId != Guid.Empty);
    }

    private string? SerializeToXMLEventFilterMetadata(Dictionary<string, string> eventFilters)
    {
        if (eventFilters.ContainsKey(EventHelperConstants.EventFilters.UsedByFTE))
        {
            XmlDocument xmlDoc = this.BuildEventMessageXML(this.BuildEventMessageFilters(eventFilters));
            return xmlDoc.InnerXml.ToString();
        }

        return null;
    }

    private Dictionary<string, string> BuildEventMessageFilters(Dictionary<string, string> eventFilters)
    {
        return new Dictionary<string, string>()
        {
            { EventHelperConstants.EventFilters.EventMessageNodes.Key, EventHelperConstants.EventFilters.UsedByFTE },
            { EventHelperConstants.EventFilters.EventMessageNodes.Value, eventFilters[EventHelperConstants.EventFilters.UsedByFTE] }
        };
    }

    private XmlDocument BuildEventMessageXML(Dictionary<string, string> eventMessageFilters)
    {
        XmlDocument xmlDoc = new XmlDocument();
        EventMessage eventMessage = new EventMessage();
        XmlElement rootElement = xmlDoc.CreateElement(nameof(eventMessage.EventFilterMetadata));
        XmlElement itemElement = xmlDoc.CreateElement(EventHelperConstants.EventFilters.EventMessageNodes.Item);

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
        return fileDeliveryData.SourceLocation.Destinations
            .Any(dest => dest.FTEMetadataId.HasValue && dest.FTEMetadataId != Guid.Empty);
    }
}

