---
type: Review Findings
title: technical/flows/configure-source-locations-and-destinations/REVIEW
description: What a reviewer session found the flow's rules miss, checked against the code.
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController
tags:
- api
- flow
- provider-file-transfer-v2
- wcf
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T08:09:56Z
sources:
- id: door-file-transfer-controller-file-transfer-configuration-view
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L88
  title: FileTransferConfigurationView (api door)
- id: door-file-transfer-controller-file-transfer-configuration-edit
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L113
  title: FileTransferConfigurationEdit (api door)
- id: door-file-transfer-controller-add-file-base-configuration
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L62
  title: AddFileBaseConfiguration (api door)
- id: door-file-transfer-controller-add-file-transfer-base-configuration
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.AddFileTransferBaseConfiguration.cs#L21
  title: AddFileTransferBaseConfiguration (api door)
- id: door-file-transfer-controller-destination-configuration-edit
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L132
  title: DestinationConfigurationEdit (api door)
- id: door-file-transfer-controller-destination-configuration-edit-panel
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Presentation/Controllers/FileTransferController.cs#L103
  title: DestinationConfigurationEditPanel (api door)
- id: door-file-transfer-svc-service-file-transfer-add
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L25
  title: FileTransferAdd (wcf door)
- id: door-file-transfer-svc-service-file-transfer-add-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L26
  title: FileTransferAdd (wcf door)
- id: door-file-transfer-svc-service-file-transfer-add-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L25
  title: FileTransferAdd (wcf door)
- id: door-file-transfer-svc-service-controller-file-transfer-add
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L22
  title: FileTransferAdd (api door)
- id: door-file-transfer-svc-service-update-file-transfer-source-location
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L31
  title: UpdateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-update-file-transfer-source-location-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L32
  title: UpdateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-update-file-transfer-source-location-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L31
  title: UpdateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-controller-update-file-transfer-source-location
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L28
  title: UpdateFileTransferSourceLocation (api door)
- id: door-file-transfer-svc-service-invalidate-file-transfer-source-location
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L37
  title: InvalidateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-invalidate-file-transfer-source-location-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Commands/FileTransferSvcService.cs#L38
  title: InvalidateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-invalidate-file-transfer-source-location-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L37
  title: InvalidateFileTransferSourceLocation (wcf door)
- id: door-file-transfer-svc-service-controller-invalidate-file-transfer-source-location
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcServiceController.cs#L34
  title: InvalidateFileTransferSourceLocation (api door)
- id: door-file-transfer-svc-query-service-get-source-locations
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L40
  title: GetSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-get-source-locations-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L31
  title: GetSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-get-source-locations-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L31
  title: GetSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-controller-get-source-locations
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L39
  title: GetSourceLocations (api door)
- id: door-file-transfer-controller-get-source-locations
  resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SourceLocation.cs#L24
  title: GetSourceLocations (api door)
- id: door-file-transfer-svc-query-service-search-source-locations
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L25
  title: SearchSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-search-source-locations-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L16
  title: SearchSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-search-source-locations-3
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L16
  title: SearchSourceLocations (wcf door)
- id: door-file-transfer-svc-query-service-controller-search-source-locations
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L21
  title: SearchSourceLocations (api door)
- id: footprint
  resource: /technical/flows/configure-source-locations-and-destinations/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:configure-source-locations-and-destinations
  generation:
    status: done
    attempts: 1
  rule_prefix: F01
  rules: 108
  doors:
  - file-transfer-controller-file-transfer-configuration-view
  - file-transfer-controller-file-transfer-configuration-edit
  - file-transfer-controller-add-file-base-configuration
  - file-transfer-controller-add-file-transfer-base-configuration
  - file-transfer-controller-destination-configuration-edit
  - file-transfer-controller-destination-configuration-edit-panel
  - file-transfer-svc-service-file-transfer-add
  - file-transfer-svc-service-file-transfer-add-2
  - file-transfer-svc-service-file-transfer-add-3
  - file-transfer-svc-service-controller-file-transfer-add
  - file-transfer-svc-service-update-file-transfer-source-location
  - file-transfer-svc-service-update-file-transfer-source-location-2
  - file-transfer-svc-service-update-file-transfer-source-location-3
  - file-transfer-svc-service-controller-update-file-transfer-source-location
  - file-transfer-svc-service-invalidate-file-transfer-source-location
  - file-transfer-svc-service-invalidate-file-transfer-source-location-2
  - file-transfer-svc-service-invalidate-file-transfer-source-location-3
  - file-transfer-svc-service-controller-invalidate-file-transfer-source-location
  - file-transfer-svc-query-service-get-source-locations
  - file-transfer-svc-query-service-get-source-locations-2
  - file-transfer-svc-query-service-get-source-locations-3
  - file-transfer-svc-query-service-controller-get-source-locations
  - file-transfer-controller-get-source-locations
  - file-transfer-svc-query-service-search-source-locations
  - file-transfer-svc-query-service-search-source-locations-2
  - file-transfer-svc-query-service-search-source-locations-3
  - file-transfer-svc-query-service-controller-search-source-locations
  tables: 3
  procedures: 0
  components:
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  - backend-configure-source-locations-and-destinations-api
  - backend-data-access-event-handlers
  - backend-data-access-event-handlers-2
  - shared-contracts-commands
  - ui-configure-source-locations-and-destinations-screens
  - ui-manage-ftp-servers-and-credentials-screens
  body_sha256: c42a5cf7079e46faafe64b97d4464db4a9beca813a63800d84133a06ca22b403
---

﻿# Review: configure-source-locations-and-destinations

**GAP-001** Applying a `FtsSourceLocationUpdated` event assumes `FtsSourceLocationsId` is a non-null GUID string; a missing or non-GUID value throws during `new Guid(...)` / `Guid.Parse(...)`.
- Door: file-transfer-svc-service-update-file-transfer-source-location
- Kind: missing
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:91-94

**GAP-002** Applying a `FtsSourceLocationInvalidated` event assumes `FtsSourceLocationsId` is a non-null GUID string; a missing or non-GUID value throws during `new Guid(...)` / `Guid.Parse(...)`.
- Door: file-transfer-svc-service-invalidate-file-transfer-source-location
- Kind: missing
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:145-148
