---
type: Target Placement
title: technical/components/backend-business-helpers-2/TARGET
description: Where the backend-business-helpers-2 component goes in the new system and what to build, method by method.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-helpers-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:11:01Z
sources:
- id: inventory
  resource: /technical/components/backend-business-helpers-2/INVENTORY.yaml
  title: Every legacy class and method (5 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-helpers-2
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 5
  methods: 28
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Application/Helpers
  body_sha256: dbbe683a11dd2ab93b36960dd77a3bf73905ff563264d1ffdd01ca4042350124
---

﻿# Target: `backend-business-helpers-2`

## Where

- Deployable: `fxfer-transfer-worker` (`workers/fxfer-transfer-worker`)
- Folder: `workers/fxfer-transfer-worker/src/Application/Helpers`

## What to create

Create the helper types used by transfer execution/business code for integration event publishing and legacy notification service calls.

Crypto + stream helpers in this component (`DuplicateFileNameException`, `IOHelper`, `EncryptionHelper`) are placed into the same target folder and must not be duplicated. Treat them as merged into the target types created by component `backend-business-helpers`.

- `workers/fxfer-transfer-worker/src/Application/Helpers/EventHelper.cs`
  - Type: `public class EventHelper`
  - Constructor(s):
    - `EventHelper()` delegates to `EventHelper(InitializeDistributedEventStorage())`.
    - `EventHelper(IDistributedEventStorage distributedEventStorage)` assigns to a private readonly field.
  - Methods:
    - `void PublishFileTransferredEvent(FileToDeliverModel fileDeliveryData, string tenantId, TimeSpan elapsedTime)`
      - Preserve legacy gating: only publishes when `eventStorage != null` and `IsDeliveryUsedForSystemIntegration(fileDeliveryData)`.
      - Preserve legacy error handling: wrap publish in try/catch; log `ex.Message`; do not rethrow.
    - `IDistributedEventStorage InitializeDistributedEventStorage()`
      - Keep legacy factory usage: `new DistributedEventStorageFactory().Create(BatchJobConstants.EventStore)`.
    - `EventMessage SetUpEventMessage(FileTransferred evt, string tenantId)`
      - Keep legacy serializer behavior: `DataContractSerializer(evt.GetType(), GetContractTypes())` into a `MemoryStream`.
    - `EventMessage BuildEventMessage(FileTransferred convertedEvent, MemoryStream memoryStream, string aggregateRootValue, string tenantId)`
      - Preserve legacy envelope field population.
    - `FileTransferred BuildFileTransferredEvent(FileToDeliverModel fileDeliveryData, TimeSpan elapsedTime)`
      - Preserve field mapping and timestamp behaviors (`DateTime.UtcNow`, `Guid.NewGuid()`, `StartTime` derived from `elapsedTime.Milliseconds`).
    - `IEnumerable<Type> GetContractTypes()`
      - Keep `DataContractRegistry.GetContractTypes()` usage.
    - `DestinationModel GetDestination(FileToDeliverModel fileDeliveryData)`
      - Preserve selection: first destination where `FTEMetadataId.HasValue && FTEMetadataId != Guid.Empty`.
    - `string? SerializeToXMLEventFilterMetadata(Dictionary<string, string> eventFilters)`
      - Preserve `UsedByFTE`-only behavior and XML serialization.
    - `Dictionary<string, string> BuildEventMessageFilters(Dictionary<string, string> eventFilters)`
      - Preserve key/value node naming using `BatchJobConstants.EventFilters.EventMessageNodes`.
    - `XmlDocument BuildEventMessageXML(Dictionary<string, string> eventMessageFilters)`
      - Preserve XML structure (`EventFilterMetadata` root; single `Item` element; child elements per filter).
    - `bool IsDeliveryUsedForSystemIntegration(FileToDeliverModel fileDeliveryData)`
      - Preserve predicate: any destination where `FTEMetadataId.HasValue && FTEMetadataId != Guid.Empty`.

- `workers/fxfer-transfer-worker/src/Application/Helpers/NotificationServiceHelper.cs`
  - Type: `public class NotificationServiceHelper`
  - Constructor(s):
    - `NotificationServiceHelper(string tenantId, INotificationService notificationService)` assigns `tenantId` and `notificationServiceProxy`.
    - `NotificationServiceHelper(string tenantId, string url, string bindingConfiguration)` calls the other constructor and then sets `notificationServiceProxy = CreateServiceProxy<INotificationService>(url, bindingConfiguration)`.
  - Property:
    - `PostEvent PreviousEvent { get; private set; }`
  - Methods:
    - `void PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId)`
      - Preserve chaining behavior: `lastEventId` is `PreviousEvent.EventId` when `PreviousEvent != null`.
      - Calls `notificationServiceProxy.PostEvent(..., DateTime.UtcNow, lastEventId, operatorId, tenantId)`; stores to `PreviousEvent`; calls `CloseChannelFactory()`.
    - `void SendEmail(string subject, string emailBody, List<Address> addressList, Address fromAddress, MailPriority priority, List<Attachments> attachmentList)`
      - Calls `notificationServiceProxy.SendEmail(tenantId, ...)` and closes the channel factory.
    - `T CreateChannel<T>(ChannelFactory channelFactory)`
      - Creates a proxy and sets `((IContextChannel)proxy).OperationTimeout = new TimeSpan(0, 50, 0)`.
    - `T CreateServiceProxy<T>(string url, string bindingConfigurationName)`
      - Calls `InitializeChannelFactory<T>(...)` then `CreateChannel<T>(...)`.
    - `ChannelFactory<T> InitializeChannelFactory<T>(string endpointaddress, string bindingConfigurationName)`
      - Uses `ReturnBinding(bindingConfigurationName)` and `new EndpointAddress(endpointaddress)`.
    - `Binding ReturnBinding(string bindingType)`
      - Uses `WCFBinding.ReturnBinding(ParseEnum<WCFBinding.BindingTypes>(bindingType))` and rethrows with `throw ex`.
    - `T ParseEnum<T>(string value)`
      - Uses `Enum.Parse(typeof(T), value, true)`.
    - `void CloseChannelFactory()`
      - If created, calls `serviceChannelFactory.Close()`.

## Mapping

| Legacy | Target | Note |
| --- | --- | --- |
| `DuplicateFileNameException..ctor(string)` | `DuplicateFileNameException..ctor(string)` | Merged into component `backend-business-helpers` (single target type in `src/Application/Helpers`). |
| `IOHelper.GetStream(string)` | `IOHelper.GetStream(string)` | Merged into component `backend-business-helpers`. |
| `IOHelper.GetString(Stream)` | `IOHelper.GetString(Stream)` | Merged into component `backend-business-helpers`. |
| `IOHelper.WriteStream(Stream, ref byte[])` | `IOHelper.WriteStream(Stream, ref byte[])` | Merged into component `backend-business-helpers`. |
| `EncryptionHelper.DecryptPgpData(string, string, string, string)` | `EncryptionHelper.DecryptPgpData(string, string, string, string)` | Merged into component `backend-business-helpers`. |
| `EncryptionHelper.DecryptPgpData(Stream, string, Stream, string)` | `EncryptionHelper.DecryptPgpData(Stream, string, Stream, string)` | Merged into component `backend-business-helpers`. |
| `EncryptionHelper.EncryptPgpFile(string, string, string, bool, bool, out int)` | `EncryptionHelper.EncryptPgpFile(string, string, string, bool, bool, out int)` | Merged into component `backend-business-helpers`. |
| `EncryptionHelper.FindSecretKey(PgpSecretKeyRingBundle, long, char[])` | `EncryptionHelper.FindSecretKey(PgpSecretKeyRingBundle, long, char[])` | Merged into component `backend-business-helpers`. |
| `EncryptionHelper.GetPrivateKey(string)` | `drop: unused private helper` | Not referenced by other legacy methods in this component.
| `EncryptionHelper.ReadPublicKey(Stream)` | `EncryptionHelper.ReadPublicKey(Stream)` | Merged into component `backend-business-helpers`. |
| `EventHelper.PublishFileTransferredEvent(FileToDeliverModel, string, TimeSpan)` | `EventHelper.PublishFileTransferredEvent(FileToDeliverModel, string, TimeSpan)` | Direct parity. |
| `EventHelper.InitializeDistributedEventStorage()` | `EventHelper.InitializeDistributedEventStorage()` | Direct parity. |
| `EventHelper.SetUpEventMessage(FileTransferred, string)` | `EventHelper.SetUpEventMessage(FileTransferred, string)` | Direct parity. |
| `EventHelper.BuildEventMessage(FileTransferred, MemoryStream, string, string)` | `EventHelper.BuildEventMessage(FileTransferred, MemoryStream, string, string)` | Direct parity. |
| `EventHelper.BuildFileTransferredEvent(FileToDeliverModel, TimeSpan)` | `EventHelper.BuildFileTransferredEvent(FileToDeliverModel, TimeSpan)` | Direct parity. |
| `EventHelper.GetContractTypes()` | `EventHelper.GetContractTypes()` | Direct parity. |
| `EventHelper.GetDestination(FileToDeliverModel)` | `EventHelper.GetDestination(FileToDeliverModel)` | Direct parity. |
| `EventHelper.SerializeToXMLEventFilterMetadata(Dictionary<string,string>)` | `EventHelper.SerializeToXMLEventFilterMetadata(Dictionary<string,string>)` | Direct parity. |
| `EventHelper.BuildEventMessageFilters(Dictionary<string,string>)` | `EventHelper.BuildEventMessageFilters(Dictionary<string,string>)` | Direct parity. |
| `EventHelper.BuildEventMessageXML(Dictionary<string,string>)` | `EventHelper.BuildEventMessageXML(Dictionary<string,string>)` | Direct parity. |
| `EventHelper.IsDeliveryUsedForSystemIntegration(FileToDeliverModel)` | `EventHelper.IsDeliveryUsedForSystemIntegration(FileToDeliverModel)` | Direct parity. |
| `NotificationServiceHelper.PostEvent(string, string, string, bool, string)` | `NotificationServiceHelper.PostEvent(string, string, string, bool, string)` | Direct parity. |
| `NotificationServiceHelper.SendEmail(string, string, List<Address>, Address, MailPriority, List<Attachments>)` | `NotificationServiceHelper.SendEmail(string, string, List<Address>, Address, MailPriority, List<Attachments>)` | Direct parity. |
| `NotificationServiceHelper.CreateChannel<T>(ChannelFactory)` | `NotificationServiceHelper.CreateChannel<T>(ChannelFactory)` | Direct parity. |
| `NotificationServiceHelper.CreateServiceProxy<T>(string, string)` | `NotificationServiceHelper.CreateServiceProxy<T>(string, string)` | Direct parity. |
| `NotificationServiceHelper.InitializeChannelFactory<T>(string, string)` | `NotificationServiceHelper.InitializeChannelFactory<T>(string, string)` | Direct parity. |
| `NotificationServiceHelper.ReturnBinding(string)` | `NotificationServiceHelper.ReturnBinding(string)` | Direct parity. |
| `NotificationServiceHelper.ParseEnum<T>(string)` | `NotificationServiceHelper.ParseEnum<T>(string)` | Direct parity. |
| `NotificationServiceHelper.CloseChannelFactory()` | `NotificationServiceHelper.CloseChannelFactory()` | Direct parity. |

## Dependencies

- Component dependencies: none.
- Package dependencies: `BouncyCastle.Crypto`.

## Enterprise-Architecture standards applied

- `patterns/security/api-microservices/PTN-SEC-API-008-secret-management-pattern.md` section `Executive Summary > Solution Overview` (key material and passphrases treated as secrets and retrieved at runtime)
- `standards/application/multi-tenant/STD-APP-MTN-001-multi-tenant-applications.md` section `2.1 Data Protection Requirements` (tenant identifiers are treated as mandatory context and not mixed across tenants)
- `standards/application/monitoring/STD-APP-MON-002-opentelemetry-observability.md` section `2.1 Mandatory Requirements` (W3C trace context propagation and no PII/PHI in telemetry/log payloads)
- `patterns/itsm-operations/api-microservices/PTN-OPS-API-001-correlation-id-pattern.md` section `Solution Overview` (correlation id propagation posture across async boundaries)