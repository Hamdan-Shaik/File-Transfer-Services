---
type: Test Plan
title: technical/components/shared-contracts-events/TESTS
description: The tests that prove the shared-contracts-events component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-events
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:28:25Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-events/INVENTORY.yaml
  title: Every legacy class and method (15 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-events
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 15
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Events
  body_sha256: e0b9e72ab8235fde706fc1b9926d414e0ccf85da003a55f450c0b81e9a2e0807
---

﻿# Tests: `shared-contracts-events`

This component owns no business rules; its tests focus on event contract shape, envelope metadata requirements, and spec/contract validation.

## Target-state requirements

### T-006

- `T_006_publishes_canonical_event_envelope_and_versioned_payload`
  - Given a tenant-scoped domain event emitted by `fxfer-domain-api`
  - When the event is published to the Event Backbone via the outbox + Event Adapter flow
  - Then the message headers/attributes contain the canonical envelope fields: `tenant_id`, `correlation_id`, `trace_id`, `producer`, `event_name`, `event_version`, `event_id`, `occurred_at`
  - And the payload conforms to the schema for its `event_name` + `event_version`

## Contract tests

- `openapi_yaml_is_valid_openapi_document`
  - Validate `technical/components/shared-contracts-events/openapi.yaml` is syntactically valid OpenAPI 3.0.

- `contract_md_is_present_and_declares_no_external_dependencies`
  - Validate `technical/components/shared-contracts-events/CONTRACT.md` declares no used components and no owned data.

- `event_contract_does_not_expose_secret_material`
  - Given the published event payload DTOs for `fxfer-domain-api`
  - Then event payloads do not include raw credential fields (passwords/keys/certificates) and carry only ids/handles.

- `delivery_outcome_event_payload_is_minimized`
  - Given a `Fxfer.DeliveryOutcomeRecorded` event
  - Then the payload contains only safe identifiers/metadata needed by consumers and does not include raw file contents.
