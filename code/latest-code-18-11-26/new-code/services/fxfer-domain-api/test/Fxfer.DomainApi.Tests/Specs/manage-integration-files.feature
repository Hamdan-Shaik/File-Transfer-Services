# ---
# type: Acceptance Scenarios
# title: Manage integration file definitions
# description: Staff and partner organisations can look up the current integration file definitions used to exchange files, so they know which references and names are in use.
# resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
# tags:
# - flow
# - provider-file-transfer-v2
# - scenarios
# - wcf
# status: draft
# generated:
#   by: csharp-doc-agent/0.1.0
#   at: 2026-11-15T06:55:41Z
# sources:
# - id: rules
#   resource: /technical/flows/manage-integration-files/RULES.md
#   title: The rules these scenarios are tagged with
# - id: door-file-transfer-svc-query-service-get-integration-file-2
#   resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L41
#   title: GetIntegrationFile (wcf door)
# - id: door-file-transfer-svc-service-update-integration-file
#   resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L55
#   title: UpdateIntegrationFile (wcf door)
# - id: door-file-transfer-svc-service-update-integration-file-2
#   resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L55
#   title: UpdateIntegrationFile (wcf door)
# - id: src-5
#   resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business/CommandHandlers/FtsIntegrationFileCommandHandler.cs
#   title: FtsIntegrationFileCommandHandler.cs (2 methods)
# - id: footprint
#   resource: /technical/flows/manage-integration-files/FOOTPRINT.yaml
#   title: Flow footprint (every method, table and procedure)
# cef:
#   pack: provider-file-transfer-v2
#   item: flow:manage-integration-files
#   generation:
#     status: needs-review
#     attempts: 2
#     problems: 1
#   rule_prefix: F03
#   rules: 13
#   doors:
#   - file-transfer-svc-query-service-get-integration-file
#   - file-transfer-svc-query-service-get-integration-file-2
#   - file-transfer-svc-service-update-integration-file
#   - file-transfer-svc-service-update-integration-file-2
#   tables: 1
#   procedures: 0
#   components:
#   - backend-business-command-handlers
#   - backend-business-command-handlers-2
#   - backend-configure-source-locations-and-destinations-api
#   - shared-contracts-commands
#   body_sha256: 054e9eeb53c1f2f776dace9df0093f2c1555b9c89b6b0544e986efb8c38e75c6
# ---

﻿# ---
# type: Acceptance Scenarios
# title: business/manage-integration-files
# resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
# tags:
# - flow
# - provider-file-transfer-v2
# - scenarios
# - wcf
# status: draft
# generated:
#   by: csharp-doc-agent/0.1.0
#   at: 2026-11-15T06:55:41Z
# sources:
# - id: rules
#   resource: /technical/flows/manage-integration-files/RULES.md
#   title: The rules these scenarios are tagged with
# - id: door-file-transfer-svc-query-service-get-integration-file-2
#   resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L41
#   title: GetIntegrationFile (wcf door)
# - id: door-file-transfer-svc-service-update-integration-file
#   resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L55
#   title: UpdateIntegrationFile (wcf door)
# - id: door-file-transfer-svc-service-update-integration-file-2
#   resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L55
#   title: UpdateIntegrationFile (wcf door)
# - id: src-5
#   resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Business/CommandHandlers/FtsIntegrationFileCommandHandler.cs
#   title: FtsIntegrationFileCommandHandler.cs (2 methods)
# - id: footprint
#   resource: /technical/flows/manage-integration-files/FOOTPRINT.yaml
#   title: Flow footprint (every method, table and procedure)
# cef:
#   pack: provider-file-transfer-v2
#   item: flow:manage-integration-files
#   generation:
#     status: needs-review
#     attempts: 2
#     problems: 1
#   rule_prefix: F03
#   rules: 13
#   doors:
#   - file-transfer-svc-query-service-get-integration-file
#   - file-transfer-svc-query-service-get-integration-file-2
#   - file-transfer-svc-service-update-integration-file
#   - file-transfer-svc-service-update-integration-file-2
#   tables: 1
#   procedures: 0
#   components: []
#   body_sha256: 1a1dd8f503ffb770bed10e162148c2e043bc938801a951dd37a19da92f3c3cff
# ---

Feature: Manage integration file definitions
Staff and partner organisations can look up the current integration file definitions used to exchange files, so they know which references and names are in use.
They can also record that a definition was processed for a configuration number, keeping it active or retiring it.

  @rule:F03-001 @rule:F03-002 @rule:F03-005 @rule:F03-006 @rule:F03-007 @keep
  Scenario Outline: Looking up active integration file definitions
    Given there are active integration file definitions named "Daily export file" and "Weekly summary file"
    And there is an inactive integration file definition named "Old monthly report file"
    When another system looks up integration file definitions using <request>
    Then only active definitions are shown
    And the shown definitions match <expectation>

    Examples:
      | request | expectation |
      | no reference and no name | "Daily export file" and "Weekly summary file" are shown |
      | no reference and the name fragment "Daily" | only "Daily export file" is shown |
      | the reference "A1" and no name | only the matching definition for "A1" is shown |

  @rule:F03-008 @rule:F03-009 @flag
  Scenario: The lookup does not promise an ordering or time details
    Given there are three active integration file definitions
    When another system looks up integration file definitions without narrowing the request
    Then the definitions may be shown in any order
    And no created time or updated time is provided as part of what they see

  @rule:F03-010 @keep
  Scenario: An update requires a configuration number
    Given there is an active integration file definition with reference "A1"
    When another system tries to record processing for "A1" without a configuration number
    Then the update is rejected
    And they are told the configuration number is missing

  @rule:F03-003 @rule:F03-004 @rule:F03-011 @rule:F03-013 @keep
  Scenario Outline: Recording processing for an integration file definition
    Given there is an active integration file definition with reference "A1"
    When another system records processing for "A1" with configuration number <config> and marks it as <activity>
    Then the definition is marked as processed
    And the configuration number is stored as <config>
    And when integration file definitions are looked up afterwards, <visibility>

    Examples:
      | config | activity | visibility |
      | 123 | still active | "A1" is still shown |
      | 123 | no longer active | "A1" is no longer shown |

  @rule:F03-012 @flag
  Scenario: Changing the name in an update does not change the stored name
    Given there is an active integration file definition with reference "A1" named "Daily export file"
    When another system records processing for "A1" with configuration number 123 and includes the name "Daily export file renamed"
    Then the stored name for "A1" remains "Daily export file"
    And the update time is set to the current time
