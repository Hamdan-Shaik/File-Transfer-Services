---
type: Generation Brief
title: 'Brief: target architecture for module provider-file-transfer-v2'
description: Instructions the tool gave the writer session for the target architecture.
tags:
- architecture
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T14:31:31Z
cef:
  pack: provider-file-transfer-v2
  item: architecture
  generation:
    status: done
    attempts: 2
  deployables: 7
  body_sha256: 6d1027d55d8de869a00a11423f1bc92364430449d89e92f7b50058b3dc6bf55f
---

# Brief: target architecture for module `provider-file-transfer-v2`

- Playbook: `inputs/playbook/architecture.md`
- Target state: `inputs/target-state.md` (its folder structure decides every path)
- Write exactly: `technical/target/architecture.yaml`, `technical/target/TARGET-ARCHITECTURE.md`, `technical/target/requirements.yaml`
- The components: `technical/components.yaml` (classes, dependencies, flows, endpoints, tables); each one's classes and methods: `technical/components/<id>/INVENTORY.yaml`
- The legacy system: `technical/ARCHITECTURE.md`

## Components to place (52: 16 reached by a door, 36 not; 0 left out as tests, archived or generated code, or by a person's decision)

Place every component listed, reached or not: a component no door reaches is still legacy code, and the graph may only have missed the calls into it. Only a person leaves code out, in `inputs/coverage-decisions.yaml`.

| Component | Tier | Reached | Classes | Methods | Flows | Endpoints | Tables | Depends on |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| `backend-bas-file-transfer-api` | backend | yes | 6 | 10 | 0 | 0 | 4 | - |
| `backend-bas-file-transfer-svc` | backend | no | 5 | 10 | 0 | 0 | 4 | - |
| `backend-bas-file-transfer-svc-2` | backend | yes | 6 | 10 | 0 | 0 | 4 | - |
| `backend-business-aggregates` | backend | no | 5 | 8 | 0 | 0 | 1 | - |
| `backend-business-aggregates-2` | backend | no | 3 | 0 | 0 | 0 | 0 | - |
| `backend-business-helpers` | backend | no | 3 | 9 | 0 | 0 | 0 | - |
| `backend-business-helpers-2` | backend | no | 5 | 28 | 0 | 0 | 0 | - |
| `backend-business-profiles` | backend | no | 6 | 6 | 0 | 0 | 1 | - |
| `backend-data-access-batch-data-access` | backend | no | 16 | 47 | 0 | 0 | 8 | - |
| `backend-data-access-batch-data-access-2` | backend | no | 16 | 47 | 0 | 0 | 8 | - |
| `backend-data-access-event-handlers-2` | backend | yes | 4 | 18 | 2 | 0 | 5 | - |
| `backend-data-file-transfer` | backend | yes | 11 | 1 | 1 | 0 | 0 | - |
| `backend-data-file-transfer-constants` | backend | no | 16 | 0 | 0 | 0 | 0 | - |
| `backend-data-file-transfer-tracking` | backend | no | 6 | 0 | 0 | 0 | 0 | - |
| `backend-file-transfer-constants-localization` | backend | no | 7 | 0 | 0 | 0 | 0 | - |
| `backend-file-transfer-mgmt-data-access` | backend | no | 18 | 54 | 0 | 0 | 8 | - |
| `backend-file-transfer-svc-main` | backend | yes | 5 | 14 | 0 | 0 | 0 | - |
| `backend-localization-labels` | backend | no | 4 | 0 | 0 | 0 | 0 | - |
| `backend-providers-file-transfer` | backend | no | 4 | 73 | 0 | 0 | 3 | - |
| `backend-run-file-transfer-batch-api` | backend | yes | 2 | 7 | 1 | 0 | 0 | - |
| `data-data-access-entities` | data | yes | 14 | 8 | 1 | 0 | 10 | - |
| `data-data-access-entities-2` | data | no | 10 | 2 | 0 | 0 | 8 | - |
| `data-data-access-entities-3` | data | yes | 14 | 8 | 1 | 0 | 10 | - |
| `shared-common-common-constants` | shared | no | 5 | 0 | 0 | 0 | 0 | - |
| `shared-common-constants-validation` | shared | no | 3 | 0 | 0 | 0 | 0 | - |
| `shared-common-file-transfer-management-constants` | shared | no | 8 | 0 | 0 | 0 | 0 | - |
| `shared-contracts-batch` | shared | no | 15 | 1 | 0 | 0 | 0 | - |
| `shared-contracts-domain` | shared | no | 8 | 0 | 0 | 0 | 0 | - |
| `shared-contracts-domain-2` | shared | no | 15 | 1 | 0 | 0 | 0 | - |
| `shared-contracts-events` | shared | no | 15 | 0 | 0 | 0 | 0 | - |
| `shared-contracts-exceptions` | shared | no | 4 | 1 | 0 | 0 | 0 | - |
| `shared-contracts-queries` | shared | no | 8 | 5 | 0 | 0 | 0 | - |
| `shared-contracts-utilities` | shared | yes | 4 | 6 | 1 | 0 | 0 | - |
| `shared-data-file-transfer-constants` | shared | no | 3 | 0 | 0 | 0 | 0 | - |
| `shared-exceptions-file-transfer-exception-constants` | shared | no | 3 | 0 | 0 | 0 | 0 | - |
| `shared-file-transfer-svc-data-access` | shared | no | 3 | 0 | 0 | 0 | 0 | - |
| `shared-file-transfer-svc-data-access-2` | shared | no | 3 | 0 | 0 | 0 | 0 | - |
| `shared-queries-parameters` | shared | no | 8 | 0 | 0 | 0 | 0 | - |
| `shared-queries-parameters-2` | shared | no | 3 | 2 | 0 | 0 | 0 | - |
| `ui-configure-source-locations-and-destinations-screens` | ui | yes | 3 | 23 | 3 | 4 | 2 | - |
| `ui-file-transfer-api-controllers` | ui | no | 1 | 0 | 0 | 0 | 0 | - |
| `backend-batch-business` | backend | no | 19 | 78 | 0 | 0 | 5 | backend-business-helpers |
| `backend-data-access-event-handlers` | backend | yes | 4 | 18 | 2 | 0 | 5 | backend-data-access-event-handlers-2 |
| `backend-data-access-query-handlers` | backend | no | 8 | 43 | 0 | 0 | 8 | shared-contracts-queries |
| `backend-data-access-query-handlers-2` | backend | no | 8 | 38 | 0 | 0 | 8 | shared-contracts-queries, shared-queries-parameters-2 |
| `backend-file-transfer-mgmt-business` | backend | no | 23 | 121 | 0 | 0 | 6 | backend-business-helpers-2 |
| `shared-contracts-commands` | shared | yes | 12 | 36 | 3 | 0 | 2 | shared-contracts-utilities |
| `ui-manage-ftp-servers-and-credentials-screens` | ui | yes | 1 | 25 | 3 | 0 | 2 | backend-data-file-transfer |
| `backend-business-command-handlers` | backend | yes | 5 | 25 | 3 | 0 | 4 | backend-business-command-handlers-2, backend-data-access-event-handlers, backend-data-access-event-handlers-2, shared-contracts-commands |
| `backend-business-command-handlers-2` | backend | yes | 5 | 25 | 3 | 0 | 4 | backend-business-command-handlers, backend-data-access-event-handlers, backend-data-access-event-handlers-2, shared-contracts-commands |
| `backend-configure-source-locations-and-destinations-api` | backend | yes | 6 | 49 | 4 | 0 | 2 | backend-bas-file-transfer-api, backend-bas-file-transfer-svc, backend-bas-file-transfer-svc-2, backend-business-command-handlers, backend-business-command-handlers-2 |
| `backend-file-transfer-mgmt-ctrl` | backend | no | 4 | 2 | 0 | 0 | 1 | backend-configure-source-locations-and-destinations-api |

## Flows (for `flow_order`)

- `configure-source-locations-and-destinations` (rules F01-NNN): `technical/flows/configure-source-locations-and-destinations/`
- `manage-ftp-servers-and-credentials` (rules F02-NNN): `technical/flows/manage-ftp-servers-and-credentials/`
- `manage-integration-files` (rules F03-NNN): `technical/flows/manage-integration-files/`
- `run-file-transfer-batch` (rules F04-NNN): `technical/flows/run-file-transfer-batch/`
- `search-and-track-file-transfers` (rules F05-NNN): `technical/flows/search-and-track-file-transfers/`

Every listed component gets a placement; `flow_order` lists every flow once. A part of this module that is built here (its database schema, for example) is a deployable, never a `uses` entry.
