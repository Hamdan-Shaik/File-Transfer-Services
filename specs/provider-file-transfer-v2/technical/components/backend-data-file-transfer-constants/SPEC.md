---
type: Behavior Specification
title: 'backend-data-file-transfer-constants: legacy UX constants (MVC/WebApi/localization)'
description: 'What the backend-data-file-transfer-constants component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-file-transfer-constants
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:29:46Z
sources:
- id: inventory
  resource: /technical/components/backend-data-file-transfer-constants/INVENTORY.yaml
  title: Every legacy class and method (16 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-file-transfer-constants
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 16
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants
  body_sha256: 32f494a6526c89b4a67c2cead25d2c3f054402e60321ad17642e0c5295e3d5be
---

# backend-data-file-transfer-constants: legacy UX constants (MVC/WebApi/localization)

This component contains the legacy constant groups used by the File Transfer UX for localization keys, MVC/WebApi controller and action names, search-field names, and other UI-facing string/int constants.
The constants are consumed by UX code when building routes/requests, looking up datalists/labels/messages, and rendering help identifiers.

## BusinessValidationExceptions
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.BusinessValidationExceptions`

Constants representing business-validation exception identifiers.

- `DuplicatedFTPServerName` = `DuplicatedFtpServerName`
- `InvalidFTPProxyPort` = `InvalidFtpProxyPort`
- `InvalidIpAddressFormat` = `InvalidIpAddressFormat`

## Ioc
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Ioc`

Constants used for inversion-of-control registrations.

- Nested group: `ComponentNames`

## Ioc.ComponentNames
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Ioc.ComponentNames`

IOC component-name string identifiers.

- `DataListReferenceDataProvider` = `DataListReferenceDataProvider`
- `FileTransferDataProvider` = `FileTransferDataProvider`
- `FileTransferQueryServiceDataProvider` = `FileTransferQueryServiceDataProvider`
- `FileTransferServiceDataProvider` = `FileTransferServiceDataProvider`

## Localization
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization`

Constants for localization lookups (datalist keys, label keys, message keys, and HTML block keys).

- `DataLists` (datalist names/keys)
  - `Contracts` = `Integration.DataList.HPPContractAssemblies`
  - `DestinationProtocol` = `FileTransfer.DataList.DestinationProtocol`
  - `DestinationType` = `FileTransfer.DataList.DestinationType`
  - `FTPServer` = `FileTransfer.DataList.FTPServer`
  - `FileDirection` = `FileTransfer.DataList.FileDirection`
  - `FileStatus` = `FileTransfer.DataList.FileStatus`
  - `Modules` = `FileTransfer.Label.Field.ModuleList`
  - `Module` = `Administration.DataList.Module`
  - `OwnerModule` = `FileTransfer.DataList.OwnerModule`
  - `ProtocolType` = `FileTransfer.DataList.ProtocolType`
  - `RenamePattern` = `FileTransfer.DataList.RenamePattern`
  - `RunOnThisDay` = `FileTransfer.DataList.DayOfMonth`
  - `RunSchedule` = `FileTransfer.DataList.RunSchedule`
  - `TimeWindow` = `FileTransfer.DataList.TimeWindow`
  - `YesNoList` = `FileTransfer.DataList.YesNoList`

- `DataListsItems` (datalist item keys/values)
  - `RunOnThisDay` = `0`
  - `RunScheduleMonhly` = `2`
  - `RunScheduleWeekly` = `1`

- `Entities`
  - `FileTransfer` = `FileTransfer.Label.Entity.FileTransfer`

- `HtmlBlocks`
  - `FTEMovingInstructions` = `FileTransfer.HtmlBlock.FTSMovingInstructions`

- `Images`
  - No members.

- `Labels`
  - `Buttons`
    - `AddAndClose` = `FileTransfer.Label.Button.AddAndClose`
    - `AddAnother` = `FileTransfer.Label.Button.AddAnother`
    - `Clear` = `FileTransfer.Label.Button.Clear`
    - `Save` = `FileTransfer.Label.Button.Save`
    - `Search` = `FileTransfer.Label.Button.Search`
    - `Edit` = `FileTransfer.Label.Button.Edit`
    - `Remove` = `FileTransfer.Label.Button.Remove`
    - `Reset` = `FileTransfer.Label.Button.Reset`
    - `CreateFromExisting` = `FileTransfer.Label.Button.createFromExisting`
  - `Fields`
    - `Active` = `FileTransfer.Label.Field.Active`
    - `Address` = `FileTransfer.Label.Field.Address`
    - `ConfigId` = `FileTransfer.Label.Field.ConfigId`
    - `ConfigName` = `FileTransfer.Label.Field.ConfigName`
    - `ConfigurationType` = `FileTransfer.Label.Field.ConfigurationType`
    - `ContactName` = `FileTransfer.Label.Field.ContactName`
    - `CreationDate` = `FileTransfer.Label.Field.FTPServerCreatedTS`
    - `Description` = `FileTransfer.Label.Field.Description`
    - `DestinationAddress` = `FileTransfer.Label.Field.DestinationAddress`
    - `DestinationId` = `FileTransfer.Label.Field.DestinationId`
    - `DestinationProtocol` = `FileTransfer.Label.Field.DestinationProtocol`
    - `DestinationProtocal` = `FileTransfer.Label.Field.DestinationProtocal`
    - `Direction` = `FileTransfer.Label.Field.Direction`
    - `EndWindow` = `FileTransfer.Label.Field.EndWindow`
    - `FileDirection` = `FileTransfer.Label.Field.FileDirection`
    - `FileStatus` = `FileTransfer.Label.Field.FileStatus`
    - `FtpDescription` = `FileTransfer.Label.Field.FTPServerDescription`
    - `FtpPassword` = `FileTransfer.Label.FTPPassword`
    - `FtpPasswordConfirmation` = `FileTransfer.Label.FTPPasswordConfirmation`
    - `FtpPort` = `FileTransfer.Label.FTPPort`
    - `FtpProxyAddress` = `FileTransfer.Label.FTPProxyAddress`
    - `FtpProxyPort` = `FileTransfer.Label.FTPProxyPort`
    - `FTPServer` = `FileTransfer.Label.Field.FTPServer`
    - `FtpServerIp` = `FileTransfer.Label.Field.FTPServerIp`
    - `FtpServerName` = `FileTransfer.Label.Field.FTPServerName`
    - `FtpTransferMode` = `FileTransfer.Label.Field.FTPTransferMode`
    - `FtpUser` = `FileTransfer.Label.Field.FTPServerUser`
    - `Inactive` = `FileTransfer.Label.Field.Inactive`
    - `IncludeInactives` = `FileTransfer.Label.Field.IncludeInactives`
    - `IncludeInvalids` = `FileTransfer.Label.Field.IncludeInvalids`
    - `IsValid` = `FileTransfer.Label.Field.IsValid`
    - `LastModifiedDate` = `FileTransfer.Label.Field.FTPLastModifiedDate`
    - `Mask` = `FileTransfer.Label.Field.Mask`
    - `Message` = `FileTransfer.Label.Field.Message`
    - `Modules` = `FileTransfer.Label.Field.ModuleList`
    - `OperatorId` = `FileTransfer.Label.Field.FTPLastModifiedUserId`
    - `OwnerName` = `FileTransfer.Label.Field.OwnerName`
    - `OwnerModule` = `FileTransfer.Label.Field.OwnerModule`
    - `OwnerEmail` = `FileTransfer.Label.Field.OwnerEmail`
    - `OwnerPhoneNumber` = `FileTransfer.Label.Field.OwnerPhone`
    - `Protocol` = `FileTransfer.Label.Field.Protocol`
    - `RenameAtDestination` = `FileTransfer.Label.Field.RenameAtDestination`
    - `RunOnMonday` = `FileTransfer.Label.Field.RunOnMonday`
    - `RunOnTuesday` = `FileTransfer.Label.Field.RunOnTuesday`
    - `RunOnWednesday` = `FileTransfer.Label.Field.RunOnWednesday`
    - `RunOnThursday` = `FileTransfer.Label.Field.RunOnThursday`
    - `RunOnFriday` = `FileTransfer.Label.Field.RunOnFriday`
    - `RunOnSaturday` = `FileTransfer.Label.Field.RunOnSaturday`
    - `RunOnSunday` = `FileTransfer.Label.Field.RunOnSunday`
    - `RunSchedule` = `FileTransfer.Label.Field.RunSchedule`
    - `RunOnLastDay` = `FileTransfer.Label.Field.RunOnLastDay`
    - `RunOnFirstDay` = `FileTransfer.Label.Field.RunOnFirstDay`
    - `RunOnThisDay` = `FileTransfer.Label.Field.RunOnThisDay`
    - `SendNotification` = `FileTransfer.Label.Field.SendNotification`
    - `Suspend` = `FileTransfer.Label.Field.Suspend`
    - `SourceAddress` = `FileTransfer.Label.Field.SourceAddress`
    - `SourceFileMask` = `FileTransfer.Label.Field.SourceFileMask`
    - `StackTrace` = `FileTransfer.Label.Field.StackTrace`
    - `StartWindow` = `FileTransfer.Label.Field.StartWindow`
    - `Status` = `FileTransfer.Label.Field.Status`
    - `Suspended` = `FileTransfer.Label.Field.Suspended`
    - `TradingPartner` = `FileTransfer.Label.Field.TradingPartner`
    - `TransportType` = `FileTransfer.Label.Field.TransportType`
  - `Titles`
    - `AddFileTransferBaseConfiguration` = `FileTransfer.Label.Title.AddFileTransferBaseConfiguration`
    - `AddFileTransferDestinationConfiguration` = `FileTransfer.Label.Title.DestinationConfigurationEditPanel`
    - `AddFTPServerCredentials` = `FileTransfer.Label.Title.AddFTPServerCredentials`
    - `AuditDetails` = `FileTransfer.Label.Title.AuditDetails`
    - `DestinationConfiguration` = `FileTransfer.Label.Title.DestinationConfiguration`
    - `EditDestinationTitle` = `FileTransfer.Label.Title.EditDestinationTitle`
    - `EditFileTransferBaseConfiguration` = `FileTransfer.Label.Title.EditFileTransferBaseConfiguration`
    - `EditFileTransferDestinationConfiguration` = `FileTransfer.Label.Title.EditDestinationConfiguration`
    - `EditFTPServerCredentials` = `FileTransfer.Label.Title.EditFTPServerCredentials`
    - `Events` = `FileTransfer.Label.Title.Events`
    - `Exceptions` = `FileTransfer.Label.Title.Exceptions`
    - `FileTransferConfiguration` = `FileTransfer.Label.Menu.FileTransferConfiguration`
    - `FileTransferTracking` = `FileTransfer.Label.Title.FileTransferTracking`
    - `FTPServerCredentials` = `FileTransfer.Label.Title.FTPServerCredentials`
    - `Information` = `FileTransfer.Label.Title.Information`
    - `SourceConfiguration` = `FileTransfer.Label.Title.SourceConfiguration`
    - `ViewDestinationTitle` = `FileTransfer.Label.Title.ViewDestinationTitle`
    - `ViewFileTransferBaseConfiguration` = `FileTransfer.Label.Title.ViewFileTransferBaseConfiguration`
    - `ViewFTPServerCredentials` = `FileTransfer.Label.Title.ViewFTPServerCredentials`

- `Messages`
  - `AddFtpServerCredentialsSuccess` = `FileTransfer.Msg.AddFTPServerCredentialsSuccess`
  - `AppSettingMissing` = `FileTransfer.Msg.AppSettingMissing`
  - `DestinationRequired` = `FileTransfer.Msg.DestinationRequired`
  - `DuplicateDestinationAddress` = `FileTransfer.Msg.DuplicateDestinationAddress`
  - `DuplicatedFTPServerName` = `FileTransfer.Msg.DuplicatedFTPServerName`
  - `EditFtpServerCredentialsSuccess` = `FileTransfer.Msg.EditFTPServerCredentialsSuccess`
  - `InvalidationAlert` = `FileTransfer.Msg.InvalidationAlert`
  - `InvalidateFtpServerCredentialsSuccess` = `FileTransfer.Msg.InvalidateFTPServerCredentialsSuccess`
  - `InvalidFTPAddress` = `FileTransfer.Msg.InvalidFTPAddress`
  - `InvalidFTPProxyPort` = `FileTransfer.Msg.InvalidFTPProxyPort`
  - `InvalidIpServerFormat` = `FileTransfer.Msg.InvalidIpServerFormat`
  - `InvalidProxyAddressFormat` = `FileTransfer.Msg.InvalidProxyAddressFormat`
  - `PasswordsDoNotMatch` = `FileTransfer.Msg.PasswordsDoNotMatch`
  - `SaveSuccess` = `FileTransfer.Msg.SaveSuccess`

## Miscellaneous
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Miscellaneous`

Other constants used by UX code.

- `FileToEvent` = `FileToEvent`
- `UNC` = `3`

## MVC
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.MVC`

Constants used by the legacy ASP.NET MVC UX.

- Nested group: `Actions`
- Nested group: `Controllers`

## MVC.Actions
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.MVC.Actions`

MVC action-name constants.

- `AddFileBaseConfiguration` = `AddFileBaseConfiguration`
- `AddFileDestinationConfiguration` = `DestinationConfigurationEditPanel`
- `AddFTPServerCredentials` = `AddFTPServerCredentials`
- `DestinationConfigurationEdit` = `DestinationConfigurationEdit`
- `DestinationConfigurationView` = `DestinationConfigurationView`
- `FileBaseConfiugrationViewPanel` = `FileBaseConfiugrationViewPanel`
- `FileTransferConfigurationEdit` = `FileTransferConfigurationEdit`
- `FileTransferConfigurationView` = `FileTransferConfigurationView`
- `FileTransferTrackingView` = `FileTransferTrackingView`
- `GetFTEMetadatas` = `GetFTEMetadatas`
- `SearchFileTransferTracking` = `SearchFileTransferTracking`
- `SearchFTPServerCredentials` = `SearchFTPServerCredentials`
- `SearchSourceLocations` = `SearchSourceLocations`

## MVC.Controllers
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.MVC.Controllers`

MVC controller-name constants.

- `FileTransfer` = `FileTransfer`

## SearchFields
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.SearchFields`

Search-field name constants used by UX search criteria.

- `DataListFieldSuffix` = `.ID`
- `FtpDescription` = `FtpDescription`
- `FtpServerId` = `FtpServerId`
- `FtpServerName` = `FtpServerName`
- `FtpUser` = `FtpUser`
- `IsValid` = `IsValid`

## Help
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Help`

Help identifier constants.

- Nested group: `UserGuide`

## Help.UserGuide
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Help.UserGuide`

Help IDs for user-guide content.

- `AddFTPServerCredentialsHelpId` = `FileTransfer.AddFTPServerCredentials`
- `SearchResultsHelpId` = `FileTransfer.FTPServerCredentialsSearchResults`
- `ViewFTPServerCredentialsId` = `FileTransfer.ViewEditFTPServerCredentials`

## WebApi
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.WebApi`

Constants used by the legacy ASP.NET Web API UX endpoints.

- Nested group: `Actions`
- Nested group: `Controllers`

## WebApi.Actions
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.WebApi.Actions`

Web API action-name constants.

- `AddFileTransferBaseConfiguration` = `AddFileTransferBaseConfiguration`
- `AddFileTransferDestinationConfiguration` = `DestinationConfigurationEditAdd`
- `AddFTPServerCredentials` = `AddFTPServerCredentials`
- `AdmittingDiagnosisCodes` = `AdmittingDiagnosisCodes`
- `EditFTPServerCredentials` = `UpdateFTPServerCredentials`
- `GetDataListData` = `GetDataListData`
- `GetFileBaseConfiugrationInformation` = `GetFileBaseConfiugrationInformation`
- `GetFTPServerCredentials` = `GetFTPServerCredentials`
- `GetFtsServerDetails` = `GetFtpServerDetails`
- `GetSourceLocations` = `GetSourceLocations`
- `SearchFileTransfer` = `SearchFileTransfer`
- `SearchFileTransferTracking` = `SearchFileTransferTracking`

## WebApi.Controllers
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.WebApi.Controllers`

Web API controller-name constants.

- `FileTransfer` = `FileTransfer`

## Xml
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Xml`

XML constants.

- Nested group: `URI`

## Xml.URI
Legacy type: `HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Xml.URI`

XML namespace constants.

- `NamespaceFileTransfer` = `urn:hp.com:hsp:ua3:FileTransfer:schemas`

## Rules implemented

None.

## Not carried over

None.

