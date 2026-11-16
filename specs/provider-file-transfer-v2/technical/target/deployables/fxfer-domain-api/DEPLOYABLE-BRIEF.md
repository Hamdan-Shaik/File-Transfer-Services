---
type: Generation Brief
title: 'Brief: deployable fxfer-domain-api'
description: Instructions the tool gave the writer session for the fxfer-domain-api deployable.
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-domain-api
tags:
- deployable
- dotnet-service
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:03:10Z
sources:
- id: component-backend-bas-file-transfer-api
  resource: /technical/components/backend-bas-file-transfer-api/SPEC.md
  title: Component placed in it
- id: component-backend-business-aggregates
  resource: /technical/components/backend-business-aggregates/SPEC.md
  title: Component placed in it
- id: component-backend-business-aggregates-2
  resource: /technical/components/backend-business-aggregates-2/SPEC.md
  title: Component placed in it
- id: component-backend-business-command-handlers
  resource: /technical/components/backend-business-command-handlers/SPEC.md
  title: Component placed in it
- id: component-backend-business-command-handlers-2
  resource: /technical/components/backend-business-command-handlers-2/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (26 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-domain-api
  generation:
    status: done
    attempts: 1
  path: services/fxfer-domain-api
  kind: dotnet-service
  stack: .NET 10 (ASP.NET Core; AOT preferred)
  components: 26
  depends_on:
  - fxfer-db-schema
  body_sha256: de4a76d73cb234a0f309bb97ce6305bc0a61717c056b212339a8847f4e88adc0
---

# Brief: deployable `fxfer-domain-api`

- Playbook: `inputs/playbook/deployable.md`
- Target state: `inputs/target-state.md`
- Architecture: `technical/target/architecture.yaml`, `technical/target/TARGET-ARCHITECTURE.md`
- Write exactly these files in `technical/target/deployables/fxfer-domain-api/`: `README.md`, `STRUCTURE.md`, `CONTRACT.md`
- Its path in the new repository: `services/fxfer-domain-api`

## The deployable (from architecture.yaml)

```yaml
name: fxfer-domain-api
kind: dotnet-service
path: services/fxfer-domain-api
stack: .NET 10 (ASP.NET Core; AOT preferred)
role: Authoritative command/query APIs for FXFER configuration and tracking.
build: dotnet build services/fxfer-domain-api
depends_on:
- fxfer-db-schema
```

## Components placed in it

- `backend-bas-file-transfer-api` in `src/Api`: `technical/components/backend-bas-file-transfer-api/INVENTORY.yaml`
- `backend-business-aggregates` in `src/Domain`: `technical/components/backend-business-aggregates/INVENTORY.yaml`
- `backend-business-aggregates-2` in `src/Domain`: `technical/components/backend-business-aggregates-2/INVENTORY.yaml`
- `backend-business-command-handlers` in `src/Application/Commands`: `technical/components/backend-business-command-handlers/INVENTORY.yaml`
- `backend-business-command-handlers-2` in `src/Application/Commands`: `technical/components/backend-business-command-handlers-2/INVENTORY.yaml`
- `backend-business-profiles` in `src/Domain/Profiles`: `technical/components/backend-business-profiles/INVENTORY.yaml`
- `backend-configure-source-locations-and-destinations-api` in `src/Api`: `technical/components/backend-configure-source-locations-and-destinations-api/INVENTORY.yaml`
- `backend-data-access-query-handlers` in `src/Application/Queries`: `technical/components/backend-data-access-query-handlers/INVENTORY.yaml`
- `backend-data-access-query-handlers-2` in `src/Application/Queries`: `technical/components/backend-data-access-query-handlers-2/INVENTORY.yaml`
- `data-data-access-entities` in `src/Infrastructure/DataAccess`: `technical/components/data-data-access-entities/INVENTORY.yaml`
- `data-data-access-entities-2` in `src/Infrastructure/DataAccess`: `technical/components/data-data-access-entities-2/INVENTORY.yaml`
- `data-data-access-entities-3` in `src/Infrastructure/DataAccess`: `technical/components/data-data-access-entities-3/INVENTORY.yaml`
- `shared-common-common-constants` in `src/Domain/Constants`: `technical/components/shared-common-common-constants/INVENTORY.yaml`
- `shared-common-constants-validation` in `src/Domain/Validation`: `technical/components/shared-common-constants-validation/INVENTORY.yaml`
- `shared-contracts-commands` in `src/Contracts/Commands`: `technical/components/shared-contracts-commands/INVENTORY.yaml`
- `shared-contracts-domain` in `src/Contracts/Domain`: `technical/components/shared-contracts-domain/INVENTORY.yaml`
- `shared-contracts-domain-2` in `src/Contracts/Domain`: `technical/components/shared-contracts-domain-2/INVENTORY.yaml`
- `shared-contracts-events` in `src/Contracts/Events`: `technical/components/shared-contracts-events/INVENTORY.yaml`
- `shared-contracts-exceptions` in `src/Contracts/Exceptions`: `technical/components/shared-contracts-exceptions/INVENTORY.yaml`
- `shared-contracts-queries` in `src/Contracts/Queries`: `technical/components/shared-contracts-queries/INVENTORY.yaml`
- `shared-contracts-utilities` in `src/Contracts/Utilities`: `technical/components/shared-contracts-utilities/INVENTORY.yaml`
- `shared-exceptions-file-transfer-exception-constants` in `src/Contracts/Exceptions`: `technical/components/shared-exceptions-file-transfer-exception-constants/INVENTORY.yaml`
- `shared-file-transfer-svc-data-access` in `src/Infrastructure/DataAccess/Constants`: `technical/components/shared-file-transfer-svc-data-access/INVENTORY.yaml`
- `shared-file-transfer-svc-data-access-2` in `src/Infrastructure/DataAccess/Constants`: `technical/components/shared-file-transfer-svc-data-access-2/INVENTORY.yaml`
- `shared-queries-parameters` in `src/Contracts/Queries`: `technical/components/shared-queries-parameters/INVENTORY.yaml`
- `shared-queries-parameters-2` in `src/Contracts/Queries`: `technical/components/shared-queries-parameters-2/INVENTORY.yaml`

## Deployables it depends on

- `fxfer-db-schema`: `technical/target/deployables/fxfer-db-schema/`
