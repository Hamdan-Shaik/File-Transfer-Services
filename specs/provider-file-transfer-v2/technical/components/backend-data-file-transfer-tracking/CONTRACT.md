---
type: Interface Contract
title: technical/components/backend-data-file-transfer-tracking/CONTRACT
description: What the backend-data-file-transfer-tracking component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-file-transfer-tracking
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:37:57Z
sources:
- id: inventory
  resource: /technical/components/backend-data-file-transfer-tracking/INVENTORY.yaml
  title: Every legacy class and method (6 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-file-transfer-tracking
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 6
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-bff
  folder: src/Contracts/Tracking
  body_sha256: cfb6fafca3de9b7bb9f274c0ac207edc64db7a51c9b628485eb9783bd56d4a1c
---

﻿# Contract: `backend-data-file-transfer-tracking`

This component defines the BFF contract DTOs used by `fxfer-bff` to return file-tracking search results and file-tracking detail payloads (header + events + exceptions).

## Offers

- Tracking DTOs
  - `FileTransferTracking`
  - `FileEvent`
  - `FileException`

## Uses

None (no component dependencies).

## Data owned

None (contract-only types; no storage).

## Enterprise-Architecture standards applied

- `patterns/application/api-microservices/PTN-APP-API-015-openapi-specification-pattern.md` section `1. OpenAPI Specification Document` (DTOs are treated as contract-first artifacts aligned to OpenAPI schemas)
- `patterns/application/api-microservices/PTN-APP-API-011-error-response-pattern.md` section `TL;DR` (diagnostic fields returned to browsers must be sanitized/redacted; no stack trace leakage)
