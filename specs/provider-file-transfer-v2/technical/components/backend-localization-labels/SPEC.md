---
type: Behavior Specification
title: 'backend-localization-labels: legacy localization label keys'
description: 'What the backend-localization-labels component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-localization-labels
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T20:30:25Z
sources:
- id: inventory
  resource: /technical/components/backend-localization-labels/INVENTORY.yaml
  title: Every legacy class and method (4 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-localization-labels
  generation:
    status: done
    attempts: 2
  tier: backend
  classes: 4
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-mfe
  folder: src/legacy-constants/localization
  body_sha256: 127f4fd1a06363196c85b5b7fb2056ef9b6c9416caed841e5214ba9d61865020
---

# backend-localization-labels: legacy localization label keys

This component contains legacy File Transfer UX localization label keys.
The keys are used by UX code to look up localized display text (field labels, titles, and button labels).

## HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Labels.Editor

Localization editor label keys.

- No members.

## HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Labels.Buttons

Localization button label keys.

- AddAndClose = FileTransfer.Label.Button.AddAndClose
- AddAnother = FileTransfer.Label.Button.AddAnother
- Clear = FileTransfer.Label.Button.Clear
- Save = FileTransfer.Label.Button.Save
- Search = FileTransfer.Label.Button.Search
- Edit = FileTransfer.Label.Button.Edit
- Remove = FileTransfer.Label.Button.Remove
- Reset = FileTransfer.Label.Button.Reset
- CreateFromExisting = FileTransfer.Label.Button.createFromExisting

## HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Labels.Fields

Localization field label keys.

- Active = FileTransfer.Label.Field.Active
- Address = FileTransfer.Label.Field.Address
- ConfigId = FileTransfer.Label.Field.ConfigId
- ConfigName = FileTransfer.Label.Field.ConfigName
- ConfigurationType = FileTransfer.Label.Field.ConfigurationType
- ContactName = FileTransfer.Label.Field.ContactName
- CreationDate = FileTransfer.Label.Field.FTPServerCreatedTS
- Description = FileTransfer.Label.Field.Description
- DestinationAddress = FileTransfer.Label.Field.DestinationAddress
- DestinationId = FileTransfer.Label.Field.DestinationId
- DestinationProtocol = FileTransfer.Label.Field.DestinationProtocol
- DestinationProtocal = FileTransfer.Label.Field.DestinationProtocal
- Direction = FileTransfer.Label.Field.Direction
- EndWindow = FileTransfer.Label.Field.EndWindow
- FileDirection = FileTransfer.Label.Field.FileDirection
- FileStatus = FileTransfer.Label.Field.FileStatus
- FtpDescription = FileTransfer.Label.Field.FTPServerDescription
- FtpPassword = FileTransfer.Label.FTPPassword
- FtpPasswordConfirmation = FileTransfer.Label.FTPPasswordConfirmation
- FtpPort = FileTransfer.Label.FTPPort
- FtpProxyAddress = FileTransfer.Label.FTPProxyAddress
- FtpProxyPort = FileTransfer.Label.FTPProxyPort
- FTPServer = FileTransfer.Label.Field.FTPServer
- FtpServerIp = FileTransfer.Label.Field.FTPServerIp
- FtpServerName = FileTransfer.Label.Field.FTPServerName
- FtpTransferMode = FileTransfer.Label.Field.FTPTransferMode
- FtpUser = FileTransfer.Label.Field.FTPServerUser
- Inactive = FileTransfer.Label.Field.Inactive
- IncludeInactives = FileTransfer.Label.Field.IncludeInactives
- IncludeInvalids = FileTransfer.Label.Field.IncludeInvalids
- IsValid = FileTransfer.Label.Field.IsValid
- LastModifiedDate = FileTransfer.Label.Field.FTPLastModifiedDate
- Mask = FileTransfer.Label.Field.Mask
- Message = FileTransfer.Label.Field.Message
- Modules = FileTransfer.Label.Field.ModuleList
- OperatorId = FileTransfer.Label.Field.FTPLastModifiedUserId
- OwnerName = FileTransfer.Label.Field.OwnerName
- OwnerModule = FileTransfer.Label.Field.OwnerModule
- OwnerEmail = FileTransfer.Label.Field.OwnerEmail
- OwnerPhoneNumber = FileTransfer.Label.Field.OwnerPhone
- Protocol = FileTransfer.Label.Field.Protocol
- RenameAtDestination = FileTransfer.Label.Field.RenameAtDestination
- RunOnMonday = FileTransfer.Label.Field.RunOnMonday
- RunOnTuesday = FileTransfer.Label.Field.RunOnTuesday
- RunOnWednesday = FileTransfer.Label.Field.RunOnWednesday
- RunOnThursday = FileTransfer.Label.Field.RunOnThursday
- RunOnFriday = FileTransfer.Label.Field.RunOnFriday
- RunOnSaturday = FileTransfer.Label.Field.RunOnSaturday
- RunOnSunday = FileTransfer.Label.Field.RunOnSunday
- RunSchedule = FileTransfer.Label.Field.RunSchedule
- RunOnLastDay = FileTransfer.Label.Field.RunOnLastDay
- RunOnFirstDay = FileTransfer.Label.Field.RunOnFirstDay
- RunOnThisDay = FileTransfer.Label.Field.RunOnThisDay
- SendNotification = FileTransfer.Label.Field.SendNotification
- Suspend = FileTransfer.Label.Field.Suspend
- SourceAddress = FileTransfer.Label.Field.SourceAddress
- SourceFileMask = FileTransfer.Label.Field.SourceFileMask
- StackTrace = FileTransfer.Label.Field.StackTrace
- StartWindow = FileTransfer.Label.Field.StartWindow
- Status = FileTransfer.Label.Field.Status
- Suspended = FileTransfer.Label.Field.Suspended
- TradingPartner = FileTransfer.Label.Field.TradingPartner
- TransportType = FileTransfer.Label.Field.TransportType

## HP.HSP.UA3.FileTransfer.UX.Data.FileTransferConstants.Localization.Labels.Titles

Localization title label keys.

- AddFileTransferBaseConfiguration = FileTransfer.Label.Title.AddFileTransferBaseConfiguration
- AddFileTransferDestinationConfiguration = FileTransfer.Label.Title.DestinationConfigurationEditPanel
- AddFTPServerCredentials = FileTransfer.Label.Title.AddFTPServerCredentials
- AuditDetails = FileTransfer.Label.Title.AuditDetails
- DestinationConfiguration = FileTransfer.Label.Title.DestinationConfiguration
- EditDestinationTitle = FileTransfer.Label.Title.EditDestinationTitle
- EditFileTransferBaseConfiguration = FileTransfer.Label.Title.EditFileTransferBaseConfiguration
- EditFileTransferDestinationConfiguration = FileTransfer.Label.Title.EditDestinationConfiguration
- EditFTPServerCredentials = FileTransfer.Label.Title.EditFTPServerCredentials
- Events = FileTransfer.Label.Title.Events
- Exceptions = FileTransfer.Label.Title.Exceptions
- FileTransferConfiguration = FileTransfer.Label.Menu.FileTransferConfiguration
- FileTransferTracking = FileTransfer.Label.Title.FileTransferTracking
- FTPServerCredentials = FileTransfer.Label.Title.FTPServerCredentials
- Information = FileTransfer.Label.Title.Information
- SourceConfiguration = FileTransfer.Label.Title.SourceConfiguration
- ViewDestinationTitle = FileTransfer.Label.Title.ViewDestinationTitle
- ViewFileTransferBaseConfiguration = FileTransfer.Label.Title.ViewFileTransferBaseConfiguration
- ViewFTPServerCredentials = FileTransfer.Label.Title.ViewFTPServerCredentials

## Rules implemented

None.

## Not carried over

None.

