---
type: Test Plan
title: 'Tests: shared-contracts-domain'
description: The tests that prove the shared-contracts-domain component, one or more per business rule.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-contracts-domain
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T22:10:36Z
sources:
- id: inventory
  resource: /technical/components/shared-contracts-domain/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-contracts-domain
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Domain
  body_sha256: 91bb8e21a2d257fe16e7789136c5ae77ca892d2ea833be13d9dd2ea8b37e1a3a
---

# Tests: `shared-contracts-domain`

This component owns no business rules; its tests focus on contract shape, secret-handling posture, and spec/contract validation.

## Contract tests

- `openapi_yaml_is_valid_openapi_document`
  - Validate `technical/components/shared-contracts-domain/openapi.yaml` is syntactically valid OpenAPI 3.0.

- `contract_md_is_present_and_declares_no_external_dependencies`
  - Validate `technical/components/shared-contracts-domain/CONTRACT.md` declares no used components and no owned data.

- `ftp_server_contract_does_not_expose_raw_password_fields`
  - Given the domain contract types
  - Then the DTOs used by the domain API do not include raw password properties (credentials are represented as secret references only).

- `file_tracking_exception_contract_supports_redaction`
  - Given a file tracking response containing exceptions
  - Then diagnostic fields (`message`, `stackTrace`) are handled using the target-state redaction/sanitization behavior for non-privileged callers.

