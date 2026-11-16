---
type: Test Plan
title: 'Tests: data-data-access-entities-3'
description: The tests that prove the data-data-access-entities-3 component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/data-data-access-entities-3
tags:
- component
- data
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:32:53Z
sources:
- id: inventory
  resource: /technical/components/data-data-access-entities-3/INVENTORY.yaml
  title: Every legacy class and method (14 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:data-data-access-entities-3
  generation:
    status: done
    attempts: 1
  tier: data
  classes: 14
  methods: 8
  owned_rules: 0
  flows:
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Infrastructure/DataAccess
  body_sha256: 37d204aac174f89555c8a8e69e77567709ae9516b60d1f29943beca2fa56f9be
---

# Tests: data-data-access-entities-3

Business rules owned: none.

## Unit tests

- FxferDbContext_model_builds_with_expected_entity_sets
  - Given DbContextOptions for the EF Core provider used in tests
  - When the model is built
  - Then the model includes the expected entity types and required relationships

- FileTransferReadOnlyRepository_get_ftp_servers_returns_rows
  - Given a seeded test database with at least one FTP server row
  - When GetFtpServerAsync is called
  - Then it returns the expected `FtpServerModel` values

- FileTransferReadOnlyRepository_get_inbound_targets_returns_rows
  - Given a seeded test database with at least one inbound target row
  - When GetInboundTargetAsync is called
  - Then it returns the expected `FtsInboundTargetModel` values

- FileTransferReadOnlyRepository_logs_and_returns_empty_on_exception
  - Given the repository throws during query execution
  - When GetFtpServerAsync/GetInboundTargetAsync is called
  - Then it logs an error including the query type and exception message
  - And returns an empty list

## Contract tests

- `openapi.yaml`: asserts `x-no-http-surface: true` and empty `paths`.
- `CONTRACT.md`: asserts offered types include `FxferDbContext` and `IFileTransferReadOnlyRepository`.