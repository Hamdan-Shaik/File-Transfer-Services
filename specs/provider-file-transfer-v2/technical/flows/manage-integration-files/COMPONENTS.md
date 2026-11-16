---
type: Component Map
title: Components of the flow manage-integration-files
description: The technical components that implement this flow, and the rules each one owns.
resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
tags:
- flow
- provider-file-transfer-v2
- wcf
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T06:55:41Z
sources:
- id: door-file-transfer-svc-query-service-get-integration-file
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L50
  title: GetIntegrationFile (wcf door)
- id: door-file-transfer-svc-query-service-get-integration-file-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L41
  title: GetIntegrationFile (wcf door)
- id: door-file-transfer-svc-service-update-integration-file
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L55
  title: UpdateIntegrationFile (wcf door)
- id: door-file-transfer-svc-service-update-integration-file-2
  resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L55
  title: UpdateIntegrationFile (wcf door)
- id: src-5
  resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business/CommandHandlers/FtsIntegrationFileCommandHandler.cs
  title: FtsIntegrationFileCommandHandler.cs (2 methods)
- id: footprint
  resource: /technical/flows/manage-integration-files/FOOTPRINT.yaml
  title: Flow footprint (every method, table and procedure)
cef:
  pack: provider-file-transfer-v2
  item: flow:manage-integration-files
  generation:
    status: needs-review
    attempts: 2
    problems: 1
  rule_prefix: F03
  rules: 13
  doors:
  - file-transfer-svc-query-service-get-integration-file
  - file-transfer-svc-query-service-get-integration-file-2
  - file-transfer-svc-service-update-integration-file
  - file-transfer-svc-service-update-integration-file-2
  tables: 1
  procedures: 0
  components:
  - backend-business-command-handlers
  - backend-business-command-handlers-2
  - backend-configure-source-locations-and-destinations-api
  - shared-contracts-commands
  body_sha256: 506dbf1e988d71423d99a3b26c29bc725b9cb582ecb204cd88100b793f3185bd
---

# Components of the flow manage-integration-files

The technical components that implement this flow, and the rules each one owns. Each component's spec is in `technical/components/<component>/`.

| Component | Tier | Doors here | Rules it owns |
| --- | --- | ---: | --- |
| [backend-data-access-event-handlers](../../../technical/components/backend-data-access-event-handlers/SPEC.md) | backend | 0 | F03-012..013 |
| [backend-data-access-query-handlers](../../../technical/components/backend-data-access-query-handlers/SPEC.md) | backend | 0 | F03-005..009 |
| [shared-contracts-commands](../../../technical/components/shared-contracts-commands/SPEC.md) | shared | 0 | F03-010 |
| [backend-business-command-handlers](../../../technical/components/backend-business-command-handlers/SPEC.md) | backend | 0 | F03-011 |
| [backend-business-command-handlers-2](../../../technical/components/backend-business-command-handlers-2/SPEC.md) | backend | 0 | - |
| [backend-configure-source-locations-and-destinations-api](../../../technical/components/backend-configure-source-locations-and-destinations-api/SPEC.md) | backend | 4 | F03-001..004 |
