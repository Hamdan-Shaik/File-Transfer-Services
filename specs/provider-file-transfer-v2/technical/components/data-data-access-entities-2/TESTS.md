---
type: Test Plan
title: 'Tests: data-data-access-entities-2'
description: The tests that prove the data-data-access-entities-2 component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/data-data-access-entities-2
tags:
- component
- data
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:22:58Z
sources:
- id: inventory
  resource: /technical/components/data-data-access-entities-2/INVENTORY.yaml
  title: Every legacy class and method (10 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:data-data-access-entities-2
  generation:
    status: done
    attempts: 1
  tier: data
  classes: 10
  methods: 2
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Infrastructure/DataAccess
  body_sha256: 8a6ec283ad2a918e805ea32481ad54e1db41d8a18dfce03441806b47cf638686
---

# Tests: data-data-access-entities-2

Business rules owned: none.

## Unit tests

- FxferDbContext_model_builds_with_expected_entity_sets_and_relationships
  - Given DbContextOptions for the EF Core provider used in tests
  - When the model is built
  - Then the model includes the expected entity types and required relationships
  - And delete behaviors are configured as Restrict/NoAction for the mapped relationships

- FxferDbContext_file_status_is_fixed_length
  - Given the EF Core model
  - When the `FtsFileManagement.FileStatus` property mapping is inspected
  - Then it is configured as fixed-length `char(1)`

## Contract tests

- `openapi.yaml`: asserts `x-no-http-surface: true` and empty `paths`.
- `CONTRACT.md`: asserts offered types include `FxferDbContext` and the entity types listed in this component.

