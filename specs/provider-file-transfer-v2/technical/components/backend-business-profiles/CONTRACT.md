---
type: Interface Contract
title: 'Contract: backend-business-profiles'
description: What the backend-business-profiles component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-business-profiles
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:23:32Z
sources:
- id: inventory
  resource: /technical/components/backend-business-profiles/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-business-profiles
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 6
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Domain/Profiles
  body_sha256: 49ae920d24cc09bc8d07b0057ebe575c7254fe40d8f8ae42b520e6a04d41dc0c
---

# Contract: backend-business-profiles

This component provides AutoMapper profiles used by the FXFER domain API to translate between commands, events, and persistence projection entities.

## Offers

### Command/event/projection mapping profiles

- `FtpServerCommandHandlerProfile`
  - AutoMapper profile mapping `AddFtpServerCommand`/`UpdateFtpServerCommand` to `FtpServerAdded`/`FtpServerUpdated`.

- `FtpServerProjectionWriterProfile`
  - AutoMapper profile mapping `FtpServerAdded` to `FtpServer` projection entity.

- `FtsInboundTargetProjectionWriterProfile`
  - AutoMapper profile mapping `FtsInboundTargetAdded` to `FtsInboundTarget` projection entity.

- `FtsIntegrationFileProjectionWriterProfile`
  - AutoMapper profile mapping `IntegrationFileAdded` to `FtsIntegrationFile` projection entity.

- `InboundTargetProfile`
  - AutoMapper profile mapping inbound-target metadata events (`FTEMetadataUpdated`/`FTEMetadataInvalidated`) to FXFER commands (`AddFtsInboundTargetCommand`/`InvalidateFtsInboundTargetCommand`).

- `IntegrationFileProfile`
  - AutoMapper profile mapping `ExtractFileGenerated` and `SummaryReportGenerated` to `AddIntegrationFileCommand`.
  - Includes group-info translation helpers.

## Uses

None (no component-to-component dependencies declared for this component).

## Data

This component does not directly read or write persistence stores. It defines mapping configuration; command handlers and projection writers perform validation, persistence, and event publication.

## Packages

- `automapper`

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-005-cqrs-pattern.md` section `Command Side Components` (mapping profiles support the command-side and projection update responsibilities).

