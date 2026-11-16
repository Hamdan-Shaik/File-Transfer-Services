---
type: Interface Contract
title: 'shared-exceptions-file-transfer-exception-constants: contract'
description: What the shared-exceptions-file-transfer-exception-constants component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-exceptions-file-transfer-exception-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T23:10:16Z
sources:
- id: inventory
  resource: /technical/components/shared-exceptions-file-transfer-exception-constants/INVENTORY.yaml
  title: Every legacy class and method (3 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-exceptions-file-transfer-exception-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 3
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Contracts/Exceptions
  body_sha256: 441e8612980a1aa3cc4adf86d5b07669bd8d5f8bdfe16d3d24ea713580cdc76f
---

# shared-exceptions-file-transfer-exception-constants: contract

## Provides

### FileTransferExceptionConstants

Nested constant groups used by other components to keep validation field/code keys and fault log prefixes consistent.

- `FileTransferExceptionConstants.Validation`: validation keys/codes and default validation message text
- `FileTransferExceptionConstants.Fields`: legacy field-name strings used to associate an error with a field
- `FileTransferExceptionConstants.FaultExceptions`: stable log prefixes/messages used by SOAP fault handling

## Uses

None.

## Data owned

None.

