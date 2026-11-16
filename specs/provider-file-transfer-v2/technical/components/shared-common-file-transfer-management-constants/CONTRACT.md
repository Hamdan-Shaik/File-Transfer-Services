---
type: Interface Contract
title: 'shared-common-file-transfer-management-constants: contract'
description: What the shared-common-file-transfer-management-constants component offers other components and what it uses from them.
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/shared-common-file-transfer-management-constants
tags:
- component
- provider-file-transfer-v2
- shared
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:54:12Z
sources:
- id: inventory
  resource: /technical/components/shared-common-file-transfer-management-constants/INVENTORY.yaml
  title: Every legacy class and method (8 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:shared-common-file-transfer-management-constants
  generation:
    status: done
    attempts: 1
  tier: shared
  classes: 8
  methods: 0
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Domain/Constants
  body_sha256: e697f15ec4aa03a431c7fb30f71a39fa72738e12cb83230b4471de689d210a72
---

# shared-common-file-transfer-management-constants: contract

## Provides

### FileTransferManagementConstants

Constants consumed by transfer execution and event distribution code.

- `FileTransferManagementConstants.ExceptionMessages`: exception message templates (some include `{0}` placeholders)
- `FileTransferManagementConstants.Information`: informational message templates used in logging
- `FileTransferManagementConstants.EventDistribution`: message templates for event distribution logging (`{0}` event name/type; `{1}` commit id)
- `FileTransferManagementConstants.Paths.PathToPrivateKeyFile`: path fragment `\\SSHPrivateKey\\SSHPrivateKeyFile`

### MaintainabilityDetails

An attribute type used by legacy code as non-functional review metadata.

### ErrorLogConstants

String keys used for structured error logging:

- `ExceptionType`, `Message`, `MethodName`, `StackTrace`

### FileDeliveryEventConstants

Single-character delivery event codes:

- `Renamed` (`R`), `Transferred` (`T`), `Archieved` (`A`), `Exception` (`E`)

## Uses

None.

## Data owned

None.

